using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSGui
{
    public class SpreadsheetStub : ISpreadsheetGUI

    {
        public SpreadsheetStub()
        {
            CalledUpdate = false;
            CalledCloseWarning = false;
            CalledDoClose = false;
            CalledAskSave = false;
            CalledDoKeyDown = false;
            CalledDoOpen = false;
            CalledOpenNew = false;
        }
        public bool CalledAskSave
        {
            get; private set;
        }
        public bool CalledDoClose
        {
            get; private set;
        }
        public bool CalledCloseWarning
        {
            get; private set;
        }
        public bool CalledDoKeyDown
        {
            get; private set;
        }
        public bool CalledDoOpen
        {
            get; private set;
        }
        public bool CalledOpenNew
        {
            get; private set;
        }
        public bool CalledUpdate
        {
            get; private set;
        }
        public void FireNewEvent()
        {
            if (NewEvent != null)
            {
                NewEvent();
            }
        }
        public void FireOpenEvent()
        {
            if (OpenEvent != null)
            {
                OpenEvent();
            }
        }
        public void FireCloseEvent()
        {
            if (CloseEvent != null)
            {
                CloseEvent();
            }
        }
        public void FireSaveEvent()
        {
            if (SaveEvent != null)
            {
                SaveEvent();
            }
        }
        public void FireKeyDownEvent(Keys key)
        {
            if (KeyDownEvent != null)
            {
                KeyDownEvent(key);
            }
        }
        public void FireSelectionChangeEvent(SpreadsheetPanel panel)
        {
            if (SelectionChangeEvent != null)
            {
                SelectionChangeEvent(panel);
            }
        }
        public void FireContentsChangeEvent(SpreadsheetPanel panel)
        {
            if (ContentsChangeEvent != null)
            {
                ContentsChangeEvent(panel);
            }
        }
        public void FireSaveCheckEvent(FormClosingEventArgs even)
        {
            if (SaveCheckEvent != null)
            {
                SaveCheckEvent(even);
            }
        }
        public void FireSavingEvent(string str)
        {
            if (SavingEvent != null)
            {
                SavingEvent(str);
            }
        }
        public string CellName { set; get; }
        public string CellValue { set; get; }
        public string CellContents { get; set; }

        public event Action NewEvent;
        public event Action OpenEvent;
        public event Action CloseEvent;
        public event Action SaveEvent;
        public event Action<Keys> KeyDownEvent;
        public event Action<SpreadsheetPanel> SelectionChangeEvent;
        public event Action<SpreadsheetPanel> ContentsChangeEvent;
        public event Action<FormClosingEventArgs> SaveCheckEvent;
        public event Action<string> SavingEvent;

        public void AskSave()
        {
            CalledAskSave = true;
        }

        public void CloseWarning(FormClosingEventArgs e)
        {
            CalledCloseWarning = true;
        }

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void DoKeyDown(Keys key)
        {
            CalledDoKeyDown = true;
        }

        public void DoOpen()
        {
            CalledDoOpen = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }

        public void Update(string name, string value)
        {
            CalledUpdate = true;
        }
    }
}
