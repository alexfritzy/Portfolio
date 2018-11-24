using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSGui
{
    /// <summary>
    /// Interface for SpreadsheetGUI.
    /// </summary>
    public interface ISpreadsheetGUI
    {
        /// <summary>
        /// New Spreadsheet.
        /// </summary>
        event Action NewEvent;

        /// <summary>
        /// Open Spreadsheet from file.
        /// </summary>
        event Action OpenEvent;

        /// <summary>
        /// Close Spreadsheet.
        /// </summary>
        event Action CloseEvent;

        /// <summary>
        /// Save Spreadsheet.
        /// </summary>
        event Action SaveEvent;

        /// <summary>
        /// Arrow key selection.
        /// </summary>
        event Action<Keys> KeyDownEvent;

        /// <summary>
        /// If the selection is changed.
        /// </summary>
        event Action<SpreadsheetPanel> SelectionChangeEvent;

        /// <summary>
        /// If enter is pressed.
        /// </summary>
        event Action<SpreadsheetPanel> ContentsChangeEvent;

        /// <summary>
        /// Save check.
        /// </summary>
        event Action<FormClosingEventArgs> SaveCheckEvent;

        /// <summary>
        /// Saving.
        /// </summary>
        event Action<string> SavingEvent;

        /// <summary>
        /// Cell Name box.
        /// </summary>
        string CellName { set; }

        /// <summary>
        /// Cell Value box.
        /// </summary>
        string CellValue { set; }

        /// <summary>
        /// Cell Contents box.
        /// </summary>
        string CellContents { get; set; }

        /// <summary>
        /// Close.
        /// </summary>
        void DoClose();

        /// <summary>
        /// New.
        /// </summary>
        void OpenNew();

        /// <summary>
        /// Open Save Dialog Box.
        /// </summary>
        void AskSave();

        /// <summary>
        /// Open.
        /// </summary>
        void DoOpen();

        /// <summary>
        /// Arrow Key Selection.
        /// </summary>
        void DoKeyDown(Keys key);

        /// <summary>
        /// Unsaved Data Warning.
        /// </summary>
        void CloseWarning(FormClosingEventArgs e);

        /// <summary>
        /// Updates cell values.
        /// </summary>
        void Update(string name, string value);
    }
}
