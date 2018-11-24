using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    /// <summary>
    /// Boggle client GUI.
    /// </summary>
    public partial class BoggleClient : Form
    {
        /// <summary>
        /// List of all dice display boxes.
        /// </summary>
        private TextBox[] boxes;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BoggleClient()
        {
            InitializeComponent();
            boxes = new TextBox[] {textBox1, textBox2, textBox3, textBox4,
                                   textBox5, textBox6, textBox7, textBox8,
                                   textBox9, textBox10, textBox11, textBox12,
                                   textBox13, textBox14, textBox15, textBox16};
        }

        /// <summary>
        /// Enable status of buttons.
        /// </summary>
        public void EnableControls(bool state)
        {
            nameBox.Enabled = state;
            serverBox.Enabled = state;
            limitBox.Enabled = state;
            inputBox.Enabled = state && IsActive;
            registerButton.Enabled = state && nameBox.Text.Length > 0 && serverBox.Text.Length > 0 && !IsPending && !IsActive;
            requestButton.Enabled = state && IsUserRegistered && !IsPending && !IsActive && limitBox.Text.Length > 0;
            leaveButton.Enabled = state && (IsPending || IsActive);
            cancelButton.Enabled = !state;
            SubmitButton.Enabled = state && IsActive;
            if (IsUserRegistered)
            {
                if (IsPending || IsActive)
                {
                    AcceptButton = SubmitButton;
                }
                else
                {
                    AcceptButton = requestButton;
                }
            }
        }

        /// <summary>
        /// Activates inputbox
        /// </summary>
        public void Activating()
        {
            inputBox.Focus();
        }

        /// <summary>
        /// Starts or stops timer.
        /// </summary>
        public void StartTimer(bool start)
        {
            if (start)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        /// <summary>
        /// Updates the game timer box.
        /// </summary>
        public void UpdateTimer(int time)
        {
            timerBox.Text = time.ToString();
        }

        /// <summary>
        /// Updates the dice.
        /// </summary>
        /// <param name="board"></param>
        public void UpdateBoard(string board)
        {
            for (int i = 0; i < 16; i++)
            {
                if (board[i].ToString() == "Q")
                {
                    boxes[i].Text = "Qu";
                }
                else
                {
                    boxes[i].Text = board[i].ToString();
                }              
            }
        }

        /// <summary>
        /// Updates name boxes.
        /// </summary>
        public void UpdateNickNames(string player1, string player2)
        {
            player1Box.Text = player1;
            player2Box.Text = player2;
        }

        /// <summary>
        /// Updates scores.
        /// </summary>
        public void UpdateScores(int score1, int score2)
        {
            score1Box.Text = score1.ToString();
            score2Box.Text = score2.ToString();
        }
        /// <summary>
        /// Clear InputBox.
        /// </summary>
        public void ClearInput()
        {
            inputBox.Clear();
        }
        
        /// <summary>
        /// Clears board and word track.
        /// </summary>
        public void ClearBoard()
        {
            for (int i = 0; i < 16; i++)
            {
                boxes[i].Text = "";
            }
            Player1WordBox.Text = "";
            Player2WordBox.Text = "";
            timerBox.Text = "";
            player1Box.Text = "";
            player2Box.Text = "";
            score1Box.Text = "";
            score2Box.Text = "";
        }

        /// <summary>
        /// Tabs.
        /// </summary>
        public void Tab()
        {
            SendKeys.Send("{TAB}");
            SendKeys.Send("{TAB}");
            SendKeys.Send("{TAB}");
            SendKeys.Send("{TAB}");
            SendKeys.Send("{TAB}");
        }

        /// <summary>
        /// Focuses limitBox on register.
        /// </summary>
        public void Registered()
        {
            limitBox.Focus();
        }

        public void PutWord(string word, int score, bool player1)
        {
            if (player1)
            {
                Player1WordBox.Text += "Word: " + word + "  Points: " + score.ToString() + Environment.NewLine;
            }
            else
            {
                Player2WordBox.Text += "Word: " + word + "  Points: " + score.ToString() + Environment.NewLine;
            }
        }
        /// <summary>
        /// Is the user currently registered.
        /// </summary>
        public bool IsUserRegistered { get; set; }

        /// <summary>
        /// Is the user in a pending game.
        /// </summary>
        public bool IsPending { get; set; }

        /// <summary>
        /// Is the user in an active game.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Fires to register a user.
        /// Parameters are name and domain.
        /// </summary>
        public event Action<string, string> RegisterPressed;

        /// <summary>
        /// Fires to cancel an ongoing action.
        /// </summary>
        public event Action CancelPressed;

        /// <summary>
        /// Fires to request a game.
        /// Parameter is time limit.
        /// </summary>
        public event Action<string> RequestPressed;

        /// <summary>
        /// Fires to leave a game.
        /// </summary>
        public event Action LeavePressed;

        /// <summary>
        /// Fires when the timer ticks.
        /// </summary>
        public event Action Tick;

        /// <summary>
        /// Fires when word is submitted.
        /// </summary>
        public event Action<string> SubmitWord;

        /// <summary>
        /// Timer for connecting to server.
        /// </summary>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            Tick?.Invoke();
        }

        /// <summary>
        /// If nameBox or serverBox are changed.
        /// </summary>
        private void nameBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        /// <summary>
        /// Fired if register button is clicked.
        /// </summary>
        private void registerButton_Click(object sender, EventArgs e)
        {
            RegisterPressed?.Invoke(nameBox.Text.Trim(), serverBox.Text.Trim());
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            CancelPressed?.Invoke();
        }

        /// <summary>
        /// Fired if Time Limit box is changed.
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        
        private void requestButtonClick(object sender, EventArgs e)
        {
            RequestPressed?.Invoke(limitBox.Text.Trim());
        }

        private void leaveButton_Click(object sender, EventArgs e)
        {
            LeavePressed?.Invoke();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\t\t\t HOW TO PLAY:\n\nEnter the server in the Sever box. Enter a name then press Register.\nOnce registered, enter a time limit and request a game. Enter in your answers in the Type Here box then press Submit or simply press Enter.\n\t\t\t   Have fun!");
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (IsActive)
            {
                SubmitWord?.Invoke(inputBox.Text);
            }
        }
    }
}
