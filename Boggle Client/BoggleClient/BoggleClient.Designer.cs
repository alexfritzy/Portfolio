namespace BoggleClient
{
    partial class BoggleClient
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.limitLabel = new System.Windows.Forms.Label();
            this.limitBox = new System.Windows.Forms.TextBox();
            this.requestButton = new System.Windows.Forms.Button();
            this.leaveButton = new System.Windows.Forms.Button();
            this.timerLabel = new System.Windows.Forms.Label();
            this.timerBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.player1Label = new System.Windows.Forms.Label();
            this.player2Label = new System.Windows.Forms.Label();
            this.player1Box = new System.Windows.Forms.TextBox();
            this.player2Box = new System.Windows.Forms.TextBox();
            this.scoreLabel1 = new System.Windows.Forms.Label();
            this.scoreLabel2 = new System.Windows.Forms.Label();
            this.score1Box = new System.Windows.Forms.TextBox();
            this.score2Box = new System.Windows.Forms.TextBox();
            this.helpButton = new System.Windows.Forms.Button();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.Submit = new System.Windows.Forms.Label();
            this.Player1WordBox = new System.Windows.Forms.TextBox();
            this.Player2WordBox = new System.Windows.Forms.TextBox();
            this.Player1Words = new System.Windows.Forms.Label();
            this.Player2Words = new System.Windows.Forms.Label();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(214, 27);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name";
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.Location = new System.Drawing.Point(255, 24);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(211, 20);
            this.nameBox.TabIndex = 1;
            this.nameBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(15, 27);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(38, 13);
            this.serverLabel.TabIndex = 2;
            this.serverLabel.Text = "Server";
            // 
            // serverBox
            // 
            this.serverBox.Location = new System.Drawing.Point(59, 24);
            this.serverBox.Name = "serverBox";
            this.serverBox.Size = new System.Drawing.Size(149, 20);
            this.serverBox.TabIndex = 3;
            this.serverBox.TextChanged += new System.EventHandler(this.nameBox_TextChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(472, 24);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(50, 50);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.registerButton.Enabled = false;
            this.registerButton.Location = new System.Drawing.Point(17, 50);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(449, 23);
            this.registerButton.TabIndex = 5;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // limitLabel
            // 
            this.limitLabel.AutoSize = true;
            this.limitLabel.Location = new System.Drawing.Point(14, 83);
            this.limitLabel.Name = "limitLabel";
            this.limitLabel.Size = new System.Drawing.Size(68, 13);
            this.limitLabel.TabIndex = 6;
            this.limitLabel.Text = "Time Limit (s)";
            // 
            // limitBox
            // 
            this.limitBox.Location = new System.Drawing.Point(88, 80);
            this.limitBox.Name = "limitBox";
            this.limitBox.Size = new System.Drawing.Size(117, 20);
            this.limitBox.TabIndex = 7;
            this.limitBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // requestButton
            // 
            this.requestButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestButton.Enabled = false;
            this.requestButton.Location = new System.Drawing.Point(211, 79);
            this.requestButton.Name = "requestButton";
            this.requestButton.Size = new System.Drawing.Size(255, 23);
            this.requestButton.TabIndex = 8;
            this.requestButton.Text = "Request Game";
            this.requestButton.UseVisualStyleBackColor = true;
            this.requestButton.Click += new System.EventHandler(this.requestButtonClick);
            // 
            // leaveButton
            // 
            this.leaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.leaveButton.Enabled = false;
            this.leaveButton.Location = new System.Drawing.Point(472, 80);
            this.leaveButton.Name = "leaveButton";
            this.leaveButton.Size = new System.Drawing.Size(50, 23);
            this.leaveButton.TabIndex = 9;
            this.leaveButton.Text = "Leave";
            this.leaveButton.UseVisualStyleBackColor = true;
            this.leaveButton.Click += new System.EventHandler(this.leaveButton_Click);
            // 
            // timerLabel
            // 
            this.timerLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.timerLabel.AutoSize = true;
            this.timerLabel.Location = new System.Drawing.Point(260, 103);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(33, 13);
            this.timerLabel.TabIndex = 10;
            this.timerLabel.Text = "Timer";
            // 
            // timerBox
            // 
            this.timerBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.timerBox.Enabled = false;
            this.timerBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timerBox.Location = new System.Drawing.Point(230, 119);
            this.timerBox.Name = "timerBox";
            this.timerBox.Size = new System.Drawing.Size(90, 38);
            this.timerBox.TabIndex = 11;
            this.timerBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(185, 170);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(45, 40);
            this.textBox1.TabIndex = 12;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(230, 170);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(45, 40);
            this.textBox2.TabIndex = 13;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(275, 170);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(45, 40);
            this.textBox3.TabIndex = 14;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(320, 170);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(45, 40);
            this.textBox4.TabIndex = 15;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.Location = new System.Drawing.Point(185, 210);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(45, 40);
            this.textBox5.TabIndex = 16;
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.Location = new System.Drawing.Point(230, 210);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(45, 40);
            this.textBox6.TabIndex = 17;
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox7
            // 
            this.textBox7.Enabled = false;
            this.textBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.Location = new System.Drawing.Point(275, 210);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(45, 40);
            this.textBox7.TabIndex = 18;
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox8
            // 
            this.textBox8.Enabled = false;
            this.textBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox8.Location = new System.Drawing.Point(320, 210);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(45, 40);
            this.textBox8.TabIndex = 19;
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox9
            // 
            this.textBox9.Enabled = false;
            this.textBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox9.Location = new System.Drawing.Point(185, 250);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(45, 40);
            this.textBox9.TabIndex = 20;
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox10
            // 
            this.textBox10.Enabled = false;
            this.textBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox10.Location = new System.Drawing.Point(230, 250);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(45, 40);
            this.textBox10.TabIndex = 21;
            this.textBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox11
            // 
            this.textBox11.Enabled = false;
            this.textBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox11.Location = new System.Drawing.Point(275, 250);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(45, 40);
            this.textBox11.TabIndex = 22;
            this.textBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox12
            // 
            this.textBox12.Enabled = false;
            this.textBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox12.Location = new System.Drawing.Point(320, 250);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(45, 40);
            this.textBox12.TabIndex = 23;
            this.textBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox13
            // 
            this.textBox13.Enabled = false;
            this.textBox13.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox13.Location = new System.Drawing.Point(185, 290);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(45, 40);
            this.textBox13.TabIndex = 24;
            this.textBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox14
            // 
            this.textBox14.Enabled = false;
            this.textBox14.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox14.Location = new System.Drawing.Point(230, 290);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(45, 40);
            this.textBox14.TabIndex = 25;
            this.textBox14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox15
            // 
            this.textBox15.Enabled = false;
            this.textBox15.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox15.Location = new System.Drawing.Point(275, 290);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(45, 40);
            this.textBox15.TabIndex = 26;
            this.textBox15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox16
            // 
            this.textBox16.Enabled = false;
            this.textBox16.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox16.Location = new System.Drawing.Point(320, 290);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(45, 40);
            this.textBox16.TabIndex = 27;
            this.textBox16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // player1Label
            // 
            this.player1Label.AutoSize = true;
            this.player1Label.Location = new System.Drawing.Point(18, 122);
            this.player1Label.Name = "player1Label";
            this.player1Label.Size = new System.Drawing.Size(45, 13);
            this.player1Label.TabIndex = 28;
            this.player1Label.Text = "Player 1";
            // 
            // player2Label
            // 
            this.player2Label.AutoSize = true;
            this.player2Label.Location = new System.Drawing.Point(371, 122);
            this.player2Label.Name = "player2Label";
            this.player2Label.Size = new System.Drawing.Size(45, 13);
            this.player2Label.TabIndex = 29;
            this.player2Label.Text = "Player 2";
            // 
            // player1Box
            // 
            this.player1Box.Enabled = false;
            this.player1Box.Location = new System.Drawing.Point(70, 119);
            this.player1Box.Name = "player1Box";
            this.player1Box.Size = new System.Drawing.Size(100, 20);
            this.player1Box.TabIndex = 30;
            // 
            // player2Box
            // 
            this.player2Box.Enabled = false;
            this.player2Box.Location = new System.Drawing.Point(422, 119);
            this.player2Box.Name = "player2Box";
            this.player2Box.Size = new System.Drawing.Size(100, 20);
            this.player2Box.TabIndex = 31;
            // 
            // scoreLabel1
            // 
            this.scoreLabel1.AutoSize = true;
            this.scoreLabel1.Location = new System.Drawing.Point(18, 149);
            this.scoreLabel1.Name = "scoreLabel1";
            this.scoreLabel1.Size = new System.Drawing.Size(35, 13);
            this.scoreLabel1.TabIndex = 32;
            this.scoreLabel1.Text = "Score";
            // 
            // scoreLabel2
            // 
            this.scoreLabel2.AutoSize = true;
            this.scoreLabel2.Location = new System.Drawing.Point(371, 149);
            this.scoreLabel2.Name = "scoreLabel2";
            this.scoreLabel2.Size = new System.Drawing.Size(35, 13);
            this.scoreLabel2.TabIndex = 33;
            this.scoreLabel2.Text = "Score";
            // 
            // score1Box
            // 
            this.score1Box.Enabled = false;
            this.score1Box.Location = new System.Drawing.Point(70, 149);
            this.score1Box.Name = "score1Box";
            this.score1Box.Size = new System.Drawing.Size(100, 20);
            this.score1Box.TabIndex = 34;
            // 
            // score2Box
            // 
            this.score2Box.Enabled = false;
            this.score2Box.Location = new System.Drawing.Point(422, 149);
            this.score2Box.Name = "score2Box";
            this.score2Box.Size = new System.Drawing.Size(100, 20);
            this.score2Box.TabIndex = 35;
            // 
            // helpButton
            // 
            this.helpButton.Location = new System.Drawing.Point(447, 502);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 23);
            this.helpButton.TabIndex = 36;
            this.helpButton.Text = "Help";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // inputBox
            // 
            this.inputBox.Enabled = false;
            this.inputBox.Location = new System.Drawing.Point(255, 354);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(100, 20);
            this.inputBox.TabIndex = 38;
            // 
            // Submit
            // 
            this.Submit.AutoSize = true;
            this.Submit.Location = new System.Drawing.Point(192, 357);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(57, 13);
            this.Submit.TabIndex = 39;
            this.Submit.Text = "Type Here";
            // 
            // Player1WordBox
            // 
            this.Player1WordBox.Location = new System.Drawing.Point(21, 210);
            this.Player1WordBox.Multiline = true;
            this.Player1WordBox.Name = "Player1WordBox";
            this.Player1WordBox.ReadOnly = true;
            this.Player1WordBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Player1WordBox.Size = new System.Drawing.Size(149, 269);
            this.Player1WordBox.TabIndex = 40;
            // 
            // Player2WordBox
            // 
            this.Player2WordBox.Location = new System.Drawing.Point(374, 210);
            this.Player2WordBox.Multiline = true;
            this.Player2WordBox.Name = "Player2WordBox";
            this.Player2WordBox.ReadOnly = true;
            this.Player2WordBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Player2WordBox.Size = new System.Drawing.Size(149, 269);
            this.Player2WordBox.TabIndex = 41;
            // 
            // Player1Words
            // 
            this.Player1Words.AutoSize = true;
            this.Player1Words.Location = new System.Drawing.Point(56, 189);
            this.Player1Words.Name = "Player1Words";
            this.Player1Words.Size = new System.Drawing.Size(79, 13);
            this.Player1Words.TabIndex = 42;
            this.Player1Words.Text = "Player 1 Words";
            // 
            // Player2Words
            // 
            this.Player2Words.AutoSize = true;
            this.Player2Words.Location = new System.Drawing.Point(408, 189);
            this.Player2Words.Name = "Player2Words";
            this.Player2Words.Size = new System.Drawing.Size(79, 13);
            this.Player2Words.TabIndex = 43;
            this.Player2Words.Text = "Player 2 Words";
            // 
            // SubmitButton
            // 
            this.SubmitButton.Enabled = false;
            this.SubmitButton.Location = new System.Drawing.Point(263, 380);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 23);
            this.SubmitButton.TabIndex = 44;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // BoggleClient
            // 
            this.AcceptButton = this.registerButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 537);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.Player2Words);
            this.Controls.Add(this.Player1Words);
            this.Controls.Add(this.Player2WordBox);
            this.Controls.Add(this.Player1WordBox);
            this.Controls.Add(this.Submit);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.score2Box);
            this.Controls.Add(this.score1Box);
            this.Controls.Add(this.scoreLabel2);
            this.Controls.Add(this.scoreLabel1);
            this.Controls.Add(this.player2Box);
            this.Controls.Add(this.player1Box);
            this.Controls.Add(this.player2Label);
            this.Controls.Add(this.player1Label);
            this.Controls.Add(this.textBox16);
            this.Controls.Add(this.textBox15);
            this.Controls.Add(this.textBox14);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.timerBox);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.leaveButton);
            this.Controls.Add(this.requestButton);
            this.Controls.Add(this.limitBox);
            this.Controls.Add(this.limitLabel);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.serverBox);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.nameLabel);
            this.Name = "BoggleClient";
            this.Text = "BoggleClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Label limitLabel;
        private System.Windows.Forms.TextBox limitBox;
        private System.Windows.Forms.Button requestButton;
        private System.Windows.Forms.Button leaveButton;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.TextBox timerBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Label player1Label;
        private System.Windows.Forms.Label player2Label;
        private System.Windows.Forms.TextBox player1Box;
        private System.Windows.Forms.TextBox player2Box;
        private System.Windows.Forms.Label scoreLabel1;
        private System.Windows.Forms.Label scoreLabel2;
        private System.Windows.Forms.TextBox score1Box;
        private System.Windows.Forms.TextBox score2Box;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Label Submit;
        private System.Windows.Forms.TextBox Player1WordBox;
        private System.Windows.Forms.TextBox Player2WordBox;
        private System.Windows.Forms.Label Player1Words;
        private System.Windows.Forms.Label Player2Words;
        private System.Windows.Forms.Button SubmitButton;
    }
}

