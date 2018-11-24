using System;
using System.Windows.Forms;

namespace SSGui
{
    /// <summary>
    /// GUI for a spreadsheet.
    /// </summary>
    public partial class SpreadsheetGUI : Form, ISpreadsheetGUI
    {
        /// <summary>
        /// Constructor for SpreadsheetGUI.
        /// </summary>
        public SpreadsheetGUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// New Spreadsheet.
        /// </summary>
        public event Action NewEvent;

        /// <summary>
        /// Open Spreadsheet from file.
        /// </summary>
        public event Action OpenEvent;

        /// <summary>
        /// Close Spreadsheet.
        /// </summary>
        public event Action CloseEvent;

        /// <summary>
        /// Save Spreadsheet.
        /// </summary>
        public event Action SaveEvent;

        /// <summary>
        /// Arrow key selection.
        /// </summary>
        public event Action<Keys> KeyDownEvent;

        /// <summary>
        /// If the selection is changed.
        /// </summary>
        public event Action<SpreadsheetPanel> SelectionChangeEvent;

        /// <summary>
        /// If enter is pressed.
        /// </summary>
        public event Action<SpreadsheetPanel> ContentsChangeEvent;

        /// <summary>
        /// Save check.
        /// </summary>
        public event Action<FormClosingEventArgs> SaveCheckEvent;

        /// <summary>
        /// Saving.
        /// </summary>
        public event Action<string> SavingEvent;

        /// <summary>
        /// Cell Name box.
        /// </summary>
        public string CellName
        {
            set { cellName.Text = value.ToString(); }
        }

        /// <summary>
        /// Cell Value box.
        /// </summary>
        public string CellValue
        {
            set { cellValue.Text = value.ToString(); }
        }

        /// <summary>
        /// Cell Contents box.
        /// </summary>
        public string CellContents
        {
            get { return cellContents.Text; }
            set { cellContents.Text = value.ToString(); }
        }

        /// <summary>
        /// Close.
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// New.
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetGUIApplicationContext.GetContext().RunNew();
        }

        /// <summary>
        /// Open Save Dialog Box
        /// </summary>
        public void AskSave()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "spreadsheet files (*.ss)|*.ss";
            if (save.ShowDialog() == DialogResult.OK && save.FileName != null)
            {
                SavingEvent(save.FileName);
            }
        }

        /// <summary>
        /// Open.
        /// </summary>
        public void DoOpen()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "spreadsheet files (*.ss)|*.ss|All files (*.*)|*.*";
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                SpreadsheetGUIApplicationContext.GetContext().RunNew(open.FileName);
            }
        }

        /// <summary>
        /// Updates cell values.
        /// </summary>
        public void Update(string name, string value)
        {
            int letter = name[0];
            Int32.TryParse(name.Remove(0, 1), out int n);
            spreadsheetPanel1.SetValue(letter - 65, n - 1, value);
        }

        /// <summary>
        /// Unsaved Data Warning.
        /// </summary>
        public void CloseWarning(FormClosingEventArgs e)
        {
            var result = MessageBox.Show("There is unsaved data. Are you sure you want to exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Arrow Key Selection.
        /// </summary>
        public void DoKeyDown(Keys key)
        {
            if (key == Keys.Left)
            {
                spreadsheetPanel1.GetSelection(out int x, out int y);
                spreadsheetPanel1.SetSelection(x - 1, y);
            }
            if (key == Keys.Right)
            {
                spreadsheetPanel1.GetSelection(out int x, out int y);
                spreadsheetPanel1.SetSelection(x + 1, y);
            }
            if (key == Keys.Up)
            {
                spreadsheetPanel1.GetSelection(out int x, out int y);
                spreadsheetPanel1.SetSelection(x, y - 1);
            }
            if (key == Keys.Down)
            {
                spreadsheetPanel1.GetSelection(out int x, out int y);
                spreadsheetPanel1.SetSelection(x, y + 1);
            }
            if (SelectionChangeEvent != null)
            {
                SelectionChangeEvent(spreadsheetPanel1);
            }
        }

        /// <summary>
        /// Close this spreadsheet window.
        /// </summary>
        private void CloseMenu(object sender, EventArgs e)
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }

        /// <summary>
        /// Allows for cell selection with arrow keys.
        /// </summary>
        private void SelectWithArrowKeys(object sender, KeyEventArgs e)
        {
            if (KeyDownEvent != null)
            {
                KeyDownEvent(e.KeyCode);
            }
        }

        /// <summary>
        /// Creates a new spreadsheet window.
        /// </summary>
        private void NewSpreadsheet(object sender, EventArgs e)
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }

        /// <summary>
        /// Detects a new contents entry.
        /// </summary>
        private void ContentsChange(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ContentsChangeEvent != null)
                {
                    ContentsChangeEvent(spreadsheetPanel1);
                }
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
            }
        }

        /// <summary>
        /// Check for loss of data.
        /// </summary>
        private void SaveCheck(object sender, FormClosingEventArgs e)
        {
            if (SaveCheckEvent != null)
            {
                SaveCheckEvent(e);
            }
        }

        /// <summary>
        /// Selection change.
        /// </summary>
        private void SelectionChanged(SpreadsheetPanel sender)
        {
            if (SelectionChangeEvent != null)
            {
                SelectionChangeEvent(sender);
            }
        }

        /// <summary>
        /// Open clicked in file menu.
        /// </summary>
        private void OpenMenu_click(object sender, EventArgs e)
        {
            if (OpenEvent != null)
            {
                OpenEvent();
            }
        }

        /// <summary>
        /// Save clicked in file menu.
        /// </summary>
        private void SaveMenu_Click(object sender, EventArgs e)
        {
            if (SaveEvent != null)
            {
                SaveEvent();
            }
        }

        private void SaveAsMenu_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "spreadsheet files (*.ss)|*.ss";
            if (save.ShowDialog() == DialogResult.OK && save.FileName != null)
            {
                SavingEvent(save.FileName);
            }
        }

        private void HelpMenu_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The top textbox displays the currently selected cell. The one below it displays that cell's value.  The one below that displays it's contents.  A cell's contents must be edited by changing the contents text box and pressing enter.", "Help");
        }
    }
}
