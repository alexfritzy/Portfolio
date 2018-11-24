using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Dynamic;
using System.Threading;
using System.Windows.Forms;

namespace BoggleClient
{
    /// <summary>
    /// Controller for boggle client GUI.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// View for the controller.
        /// </summary>
        private BoggleClient view;

        /// <summary>
        /// User token of the user, "0" if unregistered.
        /// </summary>
        private string userToken;

        /// <summary>
        /// Domain in use, "0" if unregistered.
        /// </summary>
        private string domain;

        /// <summary>
        /// GameID of active/pending game, "" if none.
        /// </summary>
        private string gameID;

        /// <summary>
        /// Nickname.
        /// </summary>
        private string nickName;

        /// <summary>
        /// True if player 1, false if player 2.
        /// </summary>
        private bool player1;

        /// <summary>
        /// For cancelling an operation.
        /// </summary>
        private CancellationTokenSource tokenSource;
        public Controller(BoggleClient view)
        {
            this.view = view;

            userToken = "0";
            domain = "0";
            gameID = "";
            view.RegisterPressed += Register;
            view.CancelPressed += Cancel;
            view.RequestPressed += Request;
            view.LeavePressed += Leave;
            view.Tick += Tick;
            view.SubmitWord += SubmitWord;
        }

        /// <summary>
        /// Cancels current operation.
        /// </summary>
        private void Cancel()
        {
            tokenSource.Cancel();
        }

        /// <summary>
        /// Registers a user at the given domain with the given name.
        /// </summary>
        private async void Register(string name, string server)
        {
            try
            {
                view.EnableControls(false);
                domain = server;
                nickName = name;
                using (HttpClient client = CreateClient(domain))
                {
                    // Parameter
                    dynamic user = new ExpandoObject();
                    user.Nickname = name;

                    // Request
                    tokenSource = new CancellationTokenSource();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("BoggleService.svc/users", content, tokenSource.Token);

                    // Response
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = await response.Content.ReadAsStringAsync();
                        userToken = JsonConvert.DeserializeObject(result).UserToken;
                        view.IsUserRegistered = true;
                    }
                    else
                    {
                        MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                view.EnableControls(true);
                view.Registered();
            }
        }

        /// <summary>
        /// Requests a game to the server.
        /// </summary>
        private async void Request(string time)
        {
            if (Int32.TryParse(time, out int n))
            {
                try
                {
                    view.ClearBoard();
                    view.EnableControls(false);
                    using (HttpClient client = CreateClient(domain))
                    {
                        // Parameter
                        dynamic request = new ExpandoObject();
                        request.UserToken = userToken;
                        request.TimeLimit = n;

                        // Request
                        tokenSource = new CancellationTokenSource();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync("BoggleService.svc/games", content, tokenSource.Token);

                        // Response
                        if (response.IsSuccessStatusCode)
                        {
                            dynamic result = await response.Content.ReadAsStringAsync();
                            gameID = JsonConvert.DeserializeObject(result).GameID;
                            view.IsPending = true;
                            view.StartTimer(true);
                        }
                        else
                        {
                            MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                }
                finally
                {
                    view.EnableControls(true);
                }
            }
            else
            {
                MessageBox.Show("Please enter a number.");
            }
        }

        /// <summary>
        /// Requests leaving a game.
        /// </summary>
        private async void Leave()
        {
            if (view.IsPending)
            {
                try
                {
                    using (HttpClient client = CreateClient(domain))
                    {
                        // Parameter
                        dynamic user = new ExpandoObject();
                        user.UserToken = userToken;

                        // Request
                        tokenSource = new CancellationTokenSource();
                        StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PutAsync("BoggleService.svc/games", content, tokenSource.Token);

                        //Response
                        if (response.IsSuccessStatusCode)
                        {
                            gameID = "";
                            view.IsPending = false;
                            view.StartTimer(false);
                            MessageBox.Show("Left game.");
                        }
                        else
                        {
                            MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                        }
                    }
                }
                catch (TaskCanceledException)
                {
                }
                finally
                {
                    view.EnableControls(true);
                }
            }
            else if (view.IsActive)
            {
                gameID = "";
                view.IsActive = false;
                view.StartTimer(false);
                Register(nickName, domain);
                view.EnableControls(true);
                MessageBox.Show("Left game.");
            }
        }

        /// <summary>
        /// Requests game information.
        /// </summary>
        private async void Tick()
        {
            // Triggered when the game first becomes active.
            if (view.IsPending)
            {
                using (HttpClient client = CreateClient(domain))
                {
                    // Request
                    tokenSource = new CancellationTokenSource();
                    HttpResponseMessage response = await client.GetAsync("BoggleService.svc/games/" + gameID, tokenSource.Token);

                    //Response
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                        if (result.GameState == "pending")
                        {
                        }
                        else
                        {
                            view.IsPending = false;
                            view.IsActive = true;
                            view.EnableControls(true);
                            view.UpdateTimer((int)result.TimeLeft);
                            view.UpdateBoard((string)result.Board);
                            view.UpdateNickNames((string)result.Player1.Nickname, (string)result.Player2.Nickname);
                            if ((string)result.Player1.Nickname == nickName)
                            {
                                player1 = true;
                            }
                            else
                            {
                                player1 = false;
                            }
                            view.UpdateScores((int)result.Player1.Score, (int)result.Player2.Score);
                            view.Activating();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }
                }
            }
            // Triggered to update game state.
            else if (view.IsActive)
            {
                using (HttpClient client = CreateClient(domain))
                {
                    // Request
                    tokenSource = new CancellationTokenSource();
                    HttpResponseMessage response = await client.GetAsync("BoggleService.svc/games/" + gameID, tokenSource.Token);

                    //Response
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic result = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                        if (result.GameState == "active")
                        {
                            view.UpdateTimer((int)result.TimeLeft);
                            view.UpdateScores((int)result.Player1.Score, (int)result.Player2.Score);
                        }
                        else
                        {
                            view.UpdateTimer(0);
                            view.IsActive = false;
                            view.StartTimer(false);
                            view.EnableControls(true);
                            view.Tab();
                            if (player1)
                            {
                                foreach (dynamic item in result.Player2.WordsPlayed)
                                {
                                    view.PutWord((string)item.Word, (int)item.Score, !player1);
                                }
                            }
                            else
                            {
                                foreach (dynamic item in result.Player1.WordsPlayed)
                                {
                                    view.PutWord((string)item.Word, (int)item.Score, !player1);
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                    }
                }
            }
        }

        private async void SubmitWord(string word)
        {
            using (HttpClient client = CreateClient(domain))
            {
                // Parameter
                dynamic user = new ExpandoObject();
                user.UserToken = userToken;
                user.Word = word;

                // Request
                tokenSource = new CancellationTokenSource();
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("BoggleService.svc/games/" + gameID,content, tokenSource.Token);

                // Response
                if (response.IsSuccessStatusCode)
                {
                    dynamic result = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                    view.ClearInput();
                    view.PutWord(word, (int)result.Score, player1);
                }
                else
                {
                    MessageBox.Show("Error registering: " + response.StatusCode + "\n" + response.ReasonPhrase);
                }
            }
        }

        /// <summary>
        /// Creates a client to communicate with the given domain.
        /// </summary>
        private static HttpClient CreateClient(string domain)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(domain);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }
    }
}
