using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SS;
using Formulas;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SSGui
{
    /// <summary>
    /// A controller for the SpreadsheetGUI.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// The controller's view.
        /// </summary>
        private ISpreadsheetGUI window;

        /// <summary>
        /// The Controller's model.
        /// </summary>
        private AbstractSpreadsheet spreadsheet;

        /// <summary>
        /// File saved to or opened from.
        /// </summary>
        private string save;

        /// <summary>
        /// Controller constructor.
        /// </summary>
        public Controller(ISpreadsheetGUI window)
        {
            this.window = window;
            spreadsheet = new Spreadsheet();
            save = "";
            window.NewEvent += HandleNew;
            window.CloseEvent += HandleClose;
            window.KeyDownEvent += HandleKeyDown;
            window.SelectionChangeEvent += HandleSelectionChange;
            window.ContentsChangeEvent += HandleContentsChange;
            window.SaveCheckEvent += HandleSaveCheck;
            window.OpenEvent += HandleOpen;
            window.SaveEvent += HandleSave;
            window.SavingEvent += HandleSaving;
        }

        /// <summary>
        /// Controller constructor for opening file.
        /// </summary>
        public Controller(ISpreadsheetGUI window, string filename)
        {
            this.window = window;
            TextReader reader = new StreamReader(filename);
            spreadsheet = new Spreadsheet(reader, new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
            reader.Close();
            save = filename;
            foreach (string s in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                window.Update(s, spreadsheet.GetCellValue(s).ToString());
            }
            string CellName = (((char)('A')).ToString()) + (1).ToString();
            window.CellName = CellName;
            window.CellValue = spreadsheet.GetCellValue(CellName).ToString();
            window.CellContents = spreadsheet.GetCellContents(CellName).ToString();
            window.NewEvent += HandleNew;
            window.CloseEvent += HandleClose;
            window.KeyDownEvent += HandleKeyDown;
            window.SelectionChangeEvent += HandleSelectionChange;
            window.ContentsChangeEvent += HandleContentsChange;
            window.SaveCheckEvent += HandleSaveCheck;
            window.OpenEvent += HandleOpen;
            window.SaveEvent += HandleSave;
            window.SavingEvent += HandleSaving;
        }

        /// <summary>
        /// Handles NewEvent.
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

        /// <summary>
        /// Handles CloseEvent.
        /// </summary>
        private void HandleClose()
        {
            window.DoClose();
        }

        /// <summary>
        /// Handles SaveEvent.
        /// </summary>
        private void HandleSave()
        {
            if (save == "")
            {
                window.AskSave();
            }
            else
            {
                TextWriter writer = new StreamWriter(save);
                spreadsheet.Save(writer);
                writer.Close();
            }
        }

        /// <summary>
        /// Handles SavingEvent.
        /// </summary>
        private void HandleSaving(string filename)
        {
            TextWriter writer = new StreamWriter(filename);
            spreadsheet.Save(writer);
            writer.Close();
            save = filename;
        }

        /// <summary>
        /// Handles OpenEvent.
        /// </summary>
        private void HandleOpen()
        {
            window.DoOpen();
        }

        /// <summary>
        /// Handles SaveCheckEvent.
        /// </summary>
        private void HandleSaveCheck(FormClosingEventArgs e)
        {
            if (spreadsheet.Changed)
            {
                window.CloseWarning(e);
            }
        }

        /// <summary>
        /// Handles KeyDownEvent.
        /// </summary>
        private void HandleKeyDown(Keys key)
        {
            window.DoKeyDown(key);
        }

        /// <summary>
        /// Handles SelectionChangeEvent.
        /// </summary>
        private void HandleSelectionChange(SpreadsheetPanel panel)
        {
            panel.GetSelection(out int x, out int y);
            string CellName = (((char)('A' + x)).ToString()) + (y + 1).ToString();
            window.CellName = CellName;
            window.CellValue = spreadsheet.GetCellValue(CellName).ToString();
            string equals = "";
            if (spreadsheet.GetCellContents(CellName).GetType() == typeof(Formula))
            {
                equals = "=";
            }
            window.CellContents = equals + spreadsheet.GetCellContents(CellName).ToString();
        }

        /// <summary>
        /// Handles ContentsChangeEvent.
        /// </summary>
        private void HandleContentsChange(SpreadsheetPanel panel)
        {
            panel.GetSelection(out int x, out int y);
            string CellName = (((char)('A' + x)).ToString()) + (y + 1).ToString();
            ISet<string> dependents = spreadsheet.SetContentsOfCell(CellName, window.CellContents);
            window.CellValue = spreadsheet.GetCellValue(CellName).ToString();
            foreach (string s in dependents)
            {
                window.Update(s, spreadsheet.GetCellValue(s).ToString());
            }
        }
    }
}
