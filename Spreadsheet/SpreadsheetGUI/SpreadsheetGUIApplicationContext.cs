using System.Windows.Forms;

namespace SSGui
{
    class SpreadsheetGUIApplicationContext : ApplicationContext
    {
        // Number of open forms.
        private int windowCount = 0;

        //Single ApplicationContext
        private static SpreadsheetGUIApplicationContext context;

        /// <summary>
        /// Private constructor
        /// </summary>
        private SpreadsheetGUIApplicationContext()
        {
        }

        /// <summary>
        /// Returns the one ApplicationContext.
        /// </summary>
        public static SpreadsheetGUIApplicationContext GetContext()
        {
            if (context == null)
            {
                context = new SpreadsheetGUIApplicationContext();
            }
            return context;
        }

        /// <summary>
        /// Runs a form in this application context.
        /// </summary>
        public void RunNew()
        {
            // Create the window
            SpreadsheetGUI window = new SpreadsheetGUI();
            new Controller(window);

            // One more form is running
            windowCount++;

            // Detects if closed
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            // Run the form
            window.Show();
        }

        /// <summary>
        /// Runs a form in this application context.
        /// </summary>
        public void RunNew(string filename)
        {
            // Create the window
            SpreadsheetGUI window = new SpreadsheetGUI();
            new Controller(window, filename);

            // One more form is running
            windowCount++;

            // Detects if closed
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            // Run the form
            window.Show();
        }
    }
}
