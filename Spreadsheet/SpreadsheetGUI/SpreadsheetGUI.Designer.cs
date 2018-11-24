namespace SSGui
{
    partial class SpreadsheetGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spreadsheetPanel1 = new SSGui.SpreadsheetPanel();
            this.cellContents = new System.Windows.Forms.TextBox();
            this.cellName = new System.Windows.Forms.TextBox();
            this.cellValue = new System.Windows.Forms.TextBox();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(654, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewSpreadsheet);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Open";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.OpenMenu_click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Save";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.SaveMenu_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseMenu);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.HelpMenu_Click);
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 84);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(654, 401);
            this.spreadsheetPanel1.TabIndex = 1;
            this.spreadsheetPanel1.SelectionChanged += new SSGui.SelectionChangedHandler(this.SelectionChanged);
            this.spreadsheetPanel1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectWithArrowKeys);
            // 
            // cellContents
            // 
            this.cellContents.Dock = System.Windows.Forms.DockStyle.Top;
            this.cellContents.Location = new System.Drawing.Point(0, 64);
            this.cellContents.Name = "cellContents";
            this.cellContents.Size = new System.Drawing.Size(654, 20);
            this.cellContents.TabIndex = 2;
            this.cellContents.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ContentsChange);
            // 
            // cellName
            // 
            this.cellName.Dock = System.Windows.Forms.DockStyle.Top;
            this.cellName.Location = new System.Drawing.Point(0, 24);
            this.cellName.Name = "cellName";
            this.cellName.ReadOnly = true;
            this.cellName.Size = new System.Drawing.Size(654, 20);
            this.cellName.TabIndex = 3;
            this.cellName.Text = "A1";
            // 
            // cellValue
            // 
            this.cellValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.cellValue.Location = new System.Drawing.Point(0, 44);
            this.cellValue.Name = "cellValue";
            this.cellValue.ReadOnly = true;
            this.cellValue.Size = new System.Drawing.Size(654, 20);
            this.cellValue.TabIndex = 4;
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsMenu_Click);
            // 
            // SpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 485);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.cellContents);
            this.Controls.Add(this.cellValue);
            this.Controls.Add(this.cellName);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpreadsheetGUI";
            this.Text = "Spreadsheet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveCheck);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TextBox cellContents;
        private System.Windows.Forms.TextBox cellName;
        private System.Windows.Forms.TextBox cellValue;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    }
}

