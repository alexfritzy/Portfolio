using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using Formulas;
using System.Text.RegularExpressions;
using System.IO;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Can construct an empty spreadsheet.
        /// </summary>
        [TestMethod]
        public void SSConstruct1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
        }
        /// <summary>
        /// Empty spreadsheet
        /// </summary>
        [TestMethod]
        public void SSGetNames1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsTrue(sheet.GetNamesOfAllNonemptyCells() == Enumerable.Empty<string>());
        }
        /// <summary>
        /// Spreadsheet with filled cells
        /// </summary>
        [TestMethod]
        public void SSGetNames2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            Assert.IsTrue(sheet.GetNamesOfAllNonemptyCells().Contains("A1") && sheet.GetNamesOfAllNonemptyCells().Count() == 1);
            sheet.SetContentsOfCell("B1", "5");
            Assert.IsTrue(sheet.GetNamesOfAllNonemptyCells().Contains("B1") && sheet.GetNamesOfAllNonemptyCells().Count() == 2);
        }
        /// <summary>
        /// Works after cells are made empty again
        /// </summary>
        [TestMethod]
        public void SSGetNames3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            Assert.IsTrue(sheet.GetNamesOfAllNonemptyCells().Contains("A1") && sheet.GetNamesOfAllNonemptyCells().Count() == 1);
            sheet.SetContentsOfCell("A1", "");
            Assert.IsTrue(sheet.GetNamesOfAllNonemptyCells() == Enumerable.Empty<string>());
        }
        /// <summary>
        /// Null test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SSGetContents1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }
        /// <summary>
        /// Invalid cell name test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SSGetContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("A0");
        }
        /// <summary>
        /// Empty cell test.
        /// </summary>
        [TestMethod]
        public void SSGetContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsTrue(sheet.GetCellContents("A1").Equals(""));
        }
        /// <summary>
        /// Non-empty cell test.
        /// </summary>
        [TestMethod]
        public void SSGetContents4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            double x = 5;
            Assert.AreEqual(sheet.GetCellContents("A1"), x);
        }
        /// <summary>
        /// Null test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SSSetContentsInt1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "5");
        }
        /// <summary>
        /// Invalid cell name test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SSSetContentsInt2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A0", "5");
        }
        /// <summary>
        /// No dependency test.
        /// </summary>
        [TestMethod]
        public void SSSetContentsInt3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "5").Contains("A1"));
        }
        /// <summary>
        /// Direct Dependency test.
        /// </summary>
        [TestMethod]
        public void SSSetContentsInt4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1*A1");
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "5").Contains("B1"));
        }
        /// <summary>
        /// Indirect Dependency test.
        /// </summary>
        [TestMethod]
        public void SSSetContentsInt5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1*B1");
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "5").Contains("C1"));
        }
        /// <summary>
        /// Change cell.
        /// </summary>
        [TestMethod]
        public void SSSetContentsInt6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("A1", "6");
            double x = 6;
            Assert.IsTrue(sheet.GetCellContents("A1").Equals(x));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SSSetContentsStr1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "a");
        }
        /// <summary>
        /// Invalid cell name test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SSSetContentsStr2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A0", "a");
        }
        /// <summary>
        /// No dependency test.
        /// </summary>
        [TestMethod]
        public void SSSetContentsStr3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "a").Contains("A1"));
        }
        /// <summary>
        /// Direct Dependency test.
        /// </summary>
        [TestMethod]
        public void SSSetContentsStr4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "a");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "a").Contains("B1"));
        }
        /// <summary>
        /// Indirect Dependency test.
        /// </summary>
        [TestMethod]
        public void SSSetContentsStr5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "a");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1*B1");
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "a").Contains("C1"));
        }
        /// <summary>
        /// Null Text test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SSSetContentsStr6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", null);
        }
        /// <summary>
        /// Change cell.
        /// </summary>
        [TestMethod]
        public void SSSetContentsStr7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "a");
            sheet.SetContentsOfCell("A1", "b");
            Assert.IsTrue(sheet.GetCellContents("A1").Equals("b"));
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SSSetContentsForm1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "=A1*A1");
        }
        /// <summary>
        /// Invalid cell name test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SSSetContentsForm2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A0", "=A1*A1");
        }
        /// <summary>
        /// Self loop test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SSSetContentsForm3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=A1*A1");
        }
        /// <summary>
        /// Direct Dependency test.
        /// </summary>
        [TestMethod]
        public void SSSetContentsForm4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=5");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "=5").Contains("B1"));
        }
        /// <summary>
        /// Indirect Dependency test.
        /// </summary>
        [TestMethod]
        public void SSSetContentsForm5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=5");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1*B1");
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "=5").Contains("C1"));
        }
        /// <summary>
        /// Direct Circular test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SSSetContentsForm6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=5");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("A1", "=B1*B1");
        }
        /// <summary>
        /// Indirect Circular test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SSSetContentsForm7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=5");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1*B1");
            sheet.SetContentsOfCell("A1", "=C1*C1");
        }
        /// <summary>
        /// Null Text test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SSSetContentsForm8()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=A0*A0");
        }
        /// <summary>
        /// Change cell.
        /// </summary>
        [TestMethod]
        public void SSSetContentsForm9()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=5");
            sheet.SetContentsOfCell("C1", "=5");
            sheet.SetContentsOfCell("A1", "=B1*B1");
            Assert.IsFalse(sheet.SetContentsOfCell("A1", "=C1*C1").Contains("B1"));
        }
        /// <summary>
        /// Valid cell tests.
        /// </summary>
        [TestMethod]
        public void ValidCells()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A15", "=5");
            sheet.SetContentsOfCell("XY32", "=5");
            sheet.SetContentsOfCell("BC7", "=5");
            sheet.SetContentsOfCell("C10000000", "=5");
            sheet.SetContentsOfCell("CCCCCCCCCCCCCCCC1", "=5");
            sheet.SetContentsOfCell("A1", "=A15*XY32*BC7*C10000000*CCCCCCCCCCCCCCCC1");
        }
        /// <summary>
        /// Invalid cell tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCells1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("AOIBDN1asdlifj");
        }
        /// <summary>
        /// Invalid cell tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCells2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("1A");
        }
        /// <summary>
        /// Invalid cell tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCells3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("A01234");
        }
        /// <summary>
        /// Invalid cell tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCells4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("A");
        }
        /// <summary>
        /// Invalid cell tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidCells5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents("1A1");
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsTrue(sheet.GetCellValue("A1").Equals(""));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void CellValue2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("A0");
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void CellValue3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue(null);
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            double n = 5;
            Assert.IsTrue(sheet.GetCellValue("A1").Equals(n));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "a");
            Assert.IsTrue(sheet.GetCellValue("A1").Equals("a"));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=5");
            double n = 5;
            Assert.IsTrue(sheet.GetCellValue("A1").Equals(n));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("A1", "6");
            double n = 6;
            Assert.IsTrue(sheet.GetCellValue("A1").Equals(n));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue8()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1*B1");
            double n = 16;
            Assert.IsTrue(sheet.GetCellValue("C1").Equals(n));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue9()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1*B1");
            sheet.SetContentsOfCell("A1", "3");
            double n = 9;
            double x = 81;
            Assert.IsTrue(sheet.GetCellValue("B1").Equals(n));
            Assert.IsTrue(sheet.GetCellValue("C1").Equals(x));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue10()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1*B1");
            sheet.SetContentsOfCell("A1", "a");
            Assert.IsTrue(sheet.GetCellValue("A1").Equals("a"));
            Assert.IsTrue(sheet.GetCellValue("B1").GetType() == typeof(FormulaError));
            Assert.IsTrue(sheet.GetCellValue("C1").GetType() == typeof(FormulaError));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue11()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("A2", "a");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1*B1");
            sheet.SetContentsOfCell("B1", "=A1*A2");
            Assert.IsTrue(sheet.GetCellValue("A2").Equals("a"));
            Assert.IsTrue(sheet.GetCellValue("B1").GetType() == typeof(FormulaError));
            Assert.IsTrue(sheet.GetCellValue("C1").GetType() == typeof(FormulaError));
        }
        /// <summary>
        /// Cell value tests.
        /// </summary>
        [TestMethod]
        public void CellValue12()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("A2", "1");
            sheet.SetContentsOfCell("B1", "=A1/A2");
            double n = 2;
            Assert.IsTrue(sheet.GetCellValue("B1").Equals(n));
            sheet.SetContentsOfCell("A2", "0");
            Assert.IsTrue(sheet.GetCellValue("B1").GetType() == typeof(FormulaError));
        }
        /// <summary>
        /// Secondary constructor test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void IsValid1()
        {
            //Does not take the letter A.
            AbstractSpreadsheet sheet = new Spreadsheet(new Regex(@"^[b-zB-Z]+[1-9][0-9]*$"));
            sheet.SetContentsOfCell("A1", "2");
        }
        /// <summary>
        /// Save and changed test.
        /// </summary>
        [TestMethod]
        public void Save1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsTrue(sheet.Changed == false);
            sheet.SetContentsOfCell("A1", "1.5");
            Assert.IsTrue(sheet.Changed == true);
            sheet.SetContentsOfCell("A2", "8.0");
            sheet.SetContentsOfCell("A3", "=A1*A2+23");
            sheet.SetContentsOfCell("B2", "Hello");
            double y = 35;
            Assert.IsTrue(sheet.GetCellValue("A3").Equals(y));
            TextWriter writer = new StreamWriter("C:/Users/Fritz/Documents/University of Utah/CS 3500/Homework/Saves/Save1.xml");
            sheet.Save(writer);
            Assert.IsTrue(sheet.Changed == false);
            sheet.SetContentsOfCell("A1", "10");
            Assert.IsTrue(sheet.Changed == true);
        }
        /// <summary>
        /// Third constructor test.
        /// </summary>
        [TestMethod]
        public void Load1()
        {
            ///This will fail if run with Save1() because it is creating the file for it.
            TextReader reader = new StreamReader("C:/Users/Fritz/Documents/University of Utah/CS 3500/Homework/Saves/Save1.xml");
            AbstractSpreadsheet sheet = new Spreadsheet(reader, new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
            double n = 1.5;
            double x = 8;
            double y = 35;
            Assert.IsTrue(sheet.GetCellValue("A1").Equals(n));
            Assert.IsTrue(sheet.GetCellValue("A2").Equals(x));
            Assert.IsTrue(sheet.GetCellValue("A3").Equals(y));
            Assert.IsTrue(sheet.GetCellValue("B2").Equals("Hello"));
        }
    }
}
