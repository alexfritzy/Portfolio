using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace SS
{
    /// <summary> 
    /// A string is a valid cell name if and only if (1) s consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits AND (2) the C#
    /// expression IsValid.IsMatch(s.ToUpper()) is true.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names, so long as they also
    /// are accepted by IsValid.  On the other hand, "Z", "X07", and "hello" are not valid cell 
    /// names, regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized by converting all letters to upper case before it is used by this 
    /// this spreadsheet.  For example, the Formula "x3+a5" should be normalize to "X3+A5" before 
    /// use.  Similarly, all cell names and Formulas that are returned or written to a file must also
    /// be normalized.
    /// 
    /// A spreadsheet contains a unique cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
    /// The value of a Formula, of course, can depend on the values of variables.  The value 
    /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
    /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet 
    {
        /// <summary>
        /// Library of non-empty cells.
        /// </summary>
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// Cell DependencyGraph.
        /// </summary>
        private DependencyGraph dependencies;

        /// <summary>
        /// Regex string for valid cell names.
        /// </summary>
        private string valid;

        /// <summary>
        /// Regex of isValid.
        /// </summary>
        private Regex IsValid;

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// </summary>
        public Spreadsheet()
        {
            cells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
            valid = @"^[a-zA-Z]+[1-9][0-9]*$";
            Changed = false;
            IsValid = new Regex(@".?");
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        /// </summary>
        public Spreadsheet(Regex isValid)
        {
            cells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
            valid = @"^[a-zA-Z]+[1-9][0-9]*$";
            Changed = false;
            IsValid = isValid;
        }

        /// <summary>
        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        /// </summary>
        public Spreadsheet(TextReader source, Regex newIsValid)
        {
            cells = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
            valid = @"^[a-zA-Z]+[1-9][0-9]*$";
            Changed = false;
            IsValid = new Regex(@".?");
            Regex oldIsValid = new Regex(@".?");
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, "Spreadsheet.xsd");
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallBack;
            using (XmlReader reader = XmlReader.Create(source, settings))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                try
                                {
                                    Regex.IsMatch("", reader["IsValid"]);

                                }
                                catch
                                {
                                    throw new SpreadsheetReadException("IsValid is not a valid C# regular expression.");
                                }
                                oldIsValid = new Regex(reader["IsValid"]);
                                break;

                            case "cell":
                                if (!IsValidCell(reader["name"]) || !oldIsValid.IsMatch(reader["name"].ToUpper()))
                                {
                                    throw new SpreadsheetReadException("There is an invalid cell according to oldIsValid.");
                                }
                                if (!newIsValid.IsMatch(reader["name"].ToUpper()))
                                {
                                    throw new SpreadsheetVersionException("There is an invalid cell according to newIsValid.");
                                }
                                if (cells.ContainsKey(reader["name"]))
                                {
                                    throw new SpreadsheetReadException("Duplicate cell.");
                                }
                                SetContentsOfCell(reader["name"], reader["contents"]);
                                break;
                        }
                    }
                }
            }
            IsValid = newIsValid;
        }

        /// <summary>
        /// Throws SpreadsheetReadException if the source is not consistant with the schema.
        /// </summary>
        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            throw new SpreadsheetReadException(e.Message);
        }

        /// <summary>
        /// Determines if a cell name is valid or not.
        /// </summary>
        private bool IsValidCell(string name)
        {
            if (Regex.IsMatch(name, valid) && IsValid.IsMatch(name.ToUpper()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if a cell is empty or not.
        /// </summary>
        private bool IsEmpty(string name)
        {
            if (cells.ContainsKey(name))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the IsValid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            using (XmlWriter writer = XmlWriter.Create(dest))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", IsValid.ToString());
                foreach (KeyValuePair<string, Cell> c in cells)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteAttributeString("name", c.Key);
                    if (c.Value.Contents.GetType() == typeof(Formula))
                    {
                        writer.WriteAttributeString("contents", "=" + c.Value.Contents.ToString());
                    }
                    else
                    {
                        writer.WriteAttributeString("contents", c.Value.Contents.ToString());
                    }
                    writer.WriteFullEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            Changed = false;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            if (cells.Count == 0)
            {
                return Enumerable.Empty<string>();
            }
            List<string> list = new List<string>();
            foreach (string name in cells.Keys)
            {
                list.Add(name);
            }
            return list;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null || !IsValidCell(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (IsEmpty(name))
            {
                return "";
            }
            return cells[name].Contents;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(String name)
        {
            if (name == null || !IsValidCell(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (IsEmpty(name))
            {
                return "";
            }
            return cells[name].Value;
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<String> SetContentsOfCell(String name, String content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (name == null || !IsValidCell(name))
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (double.TryParse(content, out double n))
            {
                return SetCellContents(name, n);
            }
            else if (content == "")
            {
                return SetCellContents(name, content);
            }
            else if (content[0] == 61)
            {
                content = content.Remove(0, 1);
                Formula formula = new Formula(content, s => s.ToUpper(), s => IsValidCell(s));
                return SetCellContents(name, formula);
            }
            else
            {
                return SetCellContents(name, content);
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, double number)
        {
            if (name == null || !IsValidCell(name))
            {
                throw new InvalidNameException();
            }
            if (cells.ContainsKey(name))
            {
                Cell cell = new Cell
                {
                    Contents = number,
                    Value = number
                };
                cells[name] = cell;
            }
            else
            {
                Cell cell = new Cell
                {
                    Contents = number,
                    Value = number
                };
                cells.Add(name, cell);
            }
            MethodInfo method = typeof(Formula).GetMethod("Evaluate");
            Lookup o = new Lookup(n => Convert.ToDouble(GetCellValue(n)));
            bool zero = false;
            foreach (string s in GetCellsToRecalculate(name))
            {
                if (cells[s].Contents.GetType() == typeof(Formula))
                {
                    try
                    {
                        method.Invoke(cells[s].Contents, new object[] { o });
                    }
                    catch
                    {
                        zero = true;
                    }
                    Cell cell;
                    if (zero == true)
                    {
                        cell = new Cell
                        {
                            Contents = cells[s].Contents,
                            Value = new FormulaError("Undefined.")
                        };
                    }
                    else
                    {
                        cell = new Cell
                        {
                            Contents = cells[s].Contents,
                            Value = method.Invoke(cells[s].Contents, new object[] { o })
                        };
                    }
                    cells[s] = cell;
                }
            }
            HashSet<string> names = new HashSet<string> { name };
            HashSet<string> temp;
            int counter = 0;
            while (counter != names.Count)
            {
                counter = names.Count;
                temp = new HashSet<string>();
                foreach (string s in names)
                {                  
                    foreach (string t in dependencies.GetDependents(s))
                    {
                        temp.Add(t);
                    }
                }
                foreach (string x in temp)
                {
                    names.Add(x);
                }
            }
            Changed = true;
            return names;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, String text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (name == null || !IsValidCell(name))
            {
                throw new InvalidNameException();
            }
            if (text == "")
            {
                if (cells.ContainsKey(name))
                {
                    cells.Remove(name);
                }
                HashSet<string> namese = new HashSet<string> { name };
                return namese;
            }
            if (cells.ContainsKey(name))
            {
                Cell cell = new Cell
                {
                    Contents = text,
                    Value = text
                };
                cells[name] = cell;
            }
            else
            {
                Cell cell = new Cell
                {
                    Contents = text,
                    Value = text
                };
                cells.Add(name, cell);
            }
            string reason = name + " is a string.";
            foreach (string s in GetCellsToRecalculate(name))
            {
                if (cells[s].Contents.GetType() == typeof(Formula))
                {
                    Cell cell = new Cell
                    {
                        Contents = cells[s].Contents,
                        Value = new FormulaError(name)
                    };
                    cells[s] = cell;
                }
            }
            HashSet<string> names = new HashSet<string> { name };
            HashSet<string> temp;
            int counter = 0;
            while (counter != names.Count)
            {
                counter = names.Count;
                temp = new HashSet<string>();
                foreach (string s in names)
                {
                    foreach (string t in dependencies.GetDependents(s))
                    {
                        temp.Add(t);
                    }
                }
                foreach (string x in temp)
                {
                    names.Add(x);
                }
            }
            Changed = true;
            return names;
        }

        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, Formula formula)
        {
            foreach (string s in formula.GetVariables())
            {
                if (!IsValidCell(s))
                {
                    throw new InvalidNameException();
                }
            }
            if (name == null || !IsValidCell(name))
            {
                throw new InvalidNameException();
            }
            DependencyGraph backup = new DependencyGraph(dependencies);
            dependencies.ReplaceDependees(name, formula.GetVariables());
            try
            {
                GetCellsToRecalculate(name);
            }
            catch (CircularException e)
            {
                dependencies = new DependencyGraph(backup);
                throw e;
            }
            bool undefined = false;
            string reason = "";
            foreach (string s in formula.GetVariables())
            {
                if (!GetNamesOfAllNonemptyCells().Contains(s) || !(GetCellValue(s).GetType() == typeof(double)))
                {
                    undefined = true;
                    reason = s + " is undefined.";
                }
            }
            Cell cell;
            if (undefined == true)
            {
                cell = new Cell
                {
                    Contents = formula,
                    Value = new FormulaError(reason)
                };
            }
            else
            {
                bool error = false;
                cell = new Cell
                {
                    Contents = formula,                   
                };
                try
                {
                    formula.Evaluate(s => Convert.ToDouble(GetCellValue(s)));
                }
                catch
                {
                    error = true;
                    cell = new Cell
                    {
                        Contents = formula,
                        Value = new FormulaError("Formula Evaluation Error.")
                    };
                }
                if (!error)
                {
                    cell.Value = formula.Evaluate(s => Convert.ToDouble(GetCellValue(s)));
                }
            }
            if (cells.ContainsKey(name))
            {
                cells[name] = cell;
            }
            else
            {
                cells.Add(name, cell);
            }
            MethodInfo method = typeof(Formula).GetMethod("Evaluate");
            Lookup o = new Lookup(n => Convert.ToDouble(GetCellValue(n)));
            bool zero = false;
            foreach (string s in GetCellsToRecalculate(name))
            {
                try
                {
                    method.Invoke(cells[s].Contents, new object[] { o });
                }
                catch
                {
                    zero = true;
                }
                if (zero == true)
                {
                    cell = new Cell
                    {
                        Contents = cells[s].Contents,
                        Value = new FormulaError("Undefined.")
                    };
                }
                else
                {
                    cell = new Cell
                    {
                        Contents = cells[s].Contents,
                        Value = method.Invoke(cells[s].Contents, new object[] { o })
                    };
                }
                cells[s] = cell;
            }
            HashSet<string> names = new HashSet<string> { name };
            HashSet<string> temp;
            int counter = 0;
            while (counter != names.Count)
            {
                counter = names.Count;
                temp = new HashSet<string>();
                foreach (string s in names)
                {
                    foreach (string t in dependencies.GetDependents(s))
                    {
                        temp.Add(t);
                    }
                }
                foreach (string x in temp)
                {
                    names.Add(x);
                }
            }
            Changed = true;
            return names;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (!IsValidCell(name))
            {
                throw new InvalidNameException();
            }
            return dependencies.GetDependents(name);
        }
    }
    /// <summary>
    /// An individual cell in a spreadsheet.
    /// </summary>
    struct Cell
    {
        /// <summary>
        /// Contents of a cell.
        /// </summary>
        private object contents;
        /// <summary>
        /// Value of a cell.
        /// </summary>
        private object value;
        /// <summary>
        /// Get/Set for cell contents.
        /// </summary>
        public object Contents
        {
            get
            {
                if (contents == null)
                {
                    contents = "";
                }
                return contents;
            }
            set => contents = value;
        }
        /// <summary>
        /// Get/Set for cell value.
        /// </summary>
        public object Value
        {
            get
            {
                if (value == null)
                {
                    value = "";
                }
                return value;
            }
            set => this.value = value;
        }
    }
}