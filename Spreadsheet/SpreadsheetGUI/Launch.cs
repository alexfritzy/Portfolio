using System;
using System.Windows.Forms;

namespace SSGui
{
    /// <summary>
    /// Runs the Spreadsheet GUI.
    /// </summary>
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Get the application context and run one form inside it
            var context = SpreadsheetGUIApplicationContext.GetContext();
            context.RunNew();
            Application.Run(context);
        }
    }
}
