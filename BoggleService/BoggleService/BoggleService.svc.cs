using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
        private static string BoggleDB;

        static BoggleService()
        {
            BoggleDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
        }

        /// <summary>
        /// Determines if a word is in the dictionary.
        /// </summary>
        private bool Legal(string word)
        {
            string line;
            using (StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line == word)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// The most recent call to SetStatus determines the response code used when
        /// an http response is sent.
        /// </summary>
        /// <param name="status"></param>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }

        /// <summary>
        /// Returns a Stream version of index.html.
        /// </summary>
        /// <returns></returns>
        public Stream API()
        {
            SetStatus(OK);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "index.html");
        }

        public UserID Register(RegisterRequest Nickname)
        {
            if (Nickname.Nickname == null || Nickname.Nickname.Trim().Length == 0 || Nickname.Nickname.Trim().Length > 50)
            {
                SetStatus(Forbidden);
                return null;
            }

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command =
                        new SqlCommand("insert into Users (UserID, Nickname) values(@UserID, @Nickname)",
                        conn,
                        trans))
                    {
                        string userID = Guid.NewGuid().ToString();

                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@Nickname", Nickname.Nickname.Trim());

                        command.ExecuteNonQuery();
                        SetStatus(Created);

                        trans.Commit();
                        UserID UserID = new UserID();
                        UserID.UserToken = userID;
                        SetStatus(Created);
                        return UserID;
                    }
                }
            }

        }

        public gameID Join(JoinRequest Request)
        {
            if (Request.UserToken == null || Request.TimeLimit < 5 || Request.TimeLimit > 120)
            {
                SetStatus(Forbidden);
                return null;
            }

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Checking if UserToken exists in Users table.
                    using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", Request.UserToken);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                                return null;
                            }
                        }
                    }

                    bool player1 = false;
                    int oldTime = 0;
                    //Checking if the UserToken is already in the pending game.
                    using (SqlCommand command = new SqlCommand("select Player1, TimeLimit from Games where Player1 is not null and Player2 is null", conn, trans))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if ((string)reader["Player1"] == Request.UserToken)
                                {
                                    SetStatus(Conflict);
                                    trans.Commit();
                                    return null;
                                }
                                if ((int?)reader["TimeLimit"] != null)
                                {
                                    player1 = true;
                                    oldTime = (int)reader["TimeLimit"];
                                }
                            }

                        }
                    }
                    gameID ID = new gameID();
                    if (player1)
                    {
                        using (SqlCommand command = new SqlCommand("update Games set Player2 = @UserID, Board = @Board, TimeLimit = @TimeLimit, StartTime = @StartTime output inserted.GameID where Player2 is null", conn, trans))
                        {
                            command.Parameters.AddWithValue("@UserID", Request.UserToken);
                            command.Parameters.AddWithValue("@Board", new BoggleBoard().ToString());
                            command.Parameters.AddWithValue("@TimeLimit", (Request.TimeLimit + oldTime) / 2);
                            command.Parameters.AddWithValue("@StartTime", DateTime.Now);
                            ID.GameID = command.ExecuteScalar().ToString();
                            SetStatus(Created);
                            trans.Commit();
                            return ID;
                        }
                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand("insert into Games (Player1, TimeLimit) output inserted.GameID values (@UserID, @TimeLimit)", conn, trans))
                        {
                            command.Parameters.AddWithValue("@UserID", Request.UserToken);
                            command.Parameters.AddWithValue("@TimeLimit", Request.TimeLimit);
                            ID.GameID = command.ExecuteScalar().ToString();
                            SetStatus(Accepted);
                            trans.Commit();
                            return ID;
                        }
                    }
                }
            }
        }

        public void Cancel(UserID UserToken)
        {
            if (UserToken.UserToken == null)
            {
                SetStatus(Forbidden);
                return;
            }
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Checking if UserToken exists in Users table.
                    using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", UserToken.UserToken);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                                return;
                            }
                        }
                    }

                    //Checking if the UserToken is already in the pending game.
                    using (SqlCommand command = new SqlCommand("delete from Games where Player1 = @UserID and Player2 is null", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", UserToken.UserToken);
                        if (command.ExecuteNonQuery() == 0)
                        {
                            SetStatus(Forbidden);
                        }
                        else
                        {
                            SetStatus(OK);
                        }
                        trans.Commit();
                    }
                }
            }
        }

        public score Play(PlayWord Word, string GameID)
        {
            if (Word.Word == null || Word.Word.Trim() == "" || Word.Word.Trim().Length > 30)
            {
                SetStatus(Forbidden);
                return null;
            }

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Checking if UserToken exists in Users table.
                    using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", Word.UserToken);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                                return null;
                            }
                        }
                    }

                    //Checking if GameID exists in Games table and that user is in the game.
                    using (SqlCommand command = new SqlCommand("select GameID from Games where GameID = @GameID and (Player1 = @UserID or Player2 = @UserID2)", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        command.Parameters.AddWithValue("@UserID", Word.UserToken);
                        command.Parameters.AddWithValue("@UserID2", Word.UserToken);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                                return null;
                            }
                        }
                    }

                    // Check if game is active or not
                    using (SqlCommand command = new SqlCommand("select * from Games where GameID = @GameID and Player2 is not null", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                SetStatus(Conflict);
                                trans.Commit();
                                return null;
                            }
                            while (reader.Read())
                            {
                                DateTime temp = (DateTime)reader["StartTime"];
                                int TimeLeft = (int)reader["TimeLimit"] + (int)(temp.Subtract(DateTime.Now).TotalSeconds);
                                if (TimeLeft <= 0)
                                {
                                    SetStatus(Conflict);
                                    trans.Commit();
                                    return null;
                                }
                            }
                        }
                    }

                    // Checks if word has been played
                    bool played = false;
                    string word = Word.Word.ToUpper();
                    using (SqlCommand command = new SqlCommand("select * from Words where GameID = @GameID and Player = @Player and Word = @Word", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        command.Parameters.AddWithValue("@Player", Word.UserToken);
                        command.Parameters.AddWithValue("@Word", word);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                played = true;
                            }
                        }
                    }

                    // Calculates score 
                    int score = 0;
                    using (SqlCommand command = new SqlCommand("select * from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BoggleBoard board = new BoggleBoard((string)reader["Board"]);
                                if (word.Length < 3)
                                {
                                    score = 0;
                                }
                                else if (played)
                                {
                                    score = 0;
                                }
                                else if (board.CanBeFormed(word) && Legal(word))
                                {
                                    if (word.Length < 5)
                                    {
                                        score = 1;
                                    }
                                    else if (word.Length == 5)
                                    {
                                        score = 2;
                                    }
                                    else if (word.Length == 6)
                                    {
                                        score = 3;
                                    }
                                    else if (word.Length == 7)
                                    {
                                        score = 5;
                                    }
                                    else
                                    {
                                        score = 11;
                                    }
                                }
                                else
                                {
                                    score = -1;
                                }
                            }
                        }
                    }

                    score Score = new score
                    {
                        Score = score
                    };
                    using (SqlCommand command = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
                    {
                        command.Parameters.AddWithValue("@Word", word);
                        command.Parameters.AddWithValue("@GameID", GameID);
                        command.Parameters.AddWithValue("@Player", Word.UserToken);
                        command.Parameters.AddWithValue("@Score", score);
                        command.ExecuteNonQuery();
                        SetStatus(OK);
                        trans.Commit();
                        return Score;
                    }
                }
            }
        }
        //        int TimeLeft = games[index].TimeLimit + (int)games[index].startTime.Subtract(DateTime.Now).TotalSeconds;
        //        if (TimeLeft <= 0)
        //        {
        //            SetStatus(Conflict);
        //            return null;
        //        }
        //        bool player1 = Player1(index, Word.UserToken);
        //        string word = Word.Word.ToUpper();
        //        int score;
        //        if (word.Length < 3)
        //        {
        //            score = 0;
        //        }
        //        else if (Played(word, player1, index))
        //        {
        //            score = 0;
        //        }
        //        else if (games[index].Boggle.CanBeFormed(word) && Legal(word))
        //        {
        //            if (word.Length < 5)
        //            {
        //                score = 1;
        //            }
        //            else if (word.Length == 5)
        //            {
        //                score = 2;
        //            }
        //            else if (word.Length == 6)
        //            {
        //                score = 3;
        //            }
        //            else if (word.Length == 7)
        //            {
        //                score = 5;
        //            }
        //            else
        //            {
        //                score = 11;
        //            }
        //        }
        //        else
        //        {
        //            score = -1;
        //        }
        //        WordScorePair pair = new WordScorePair();
        //        pair.Word = word;
        //        pair.Score = score;
        //        if (player1)
        //        {
        //            games[index].Player1.WordsPlayed.Add(pair);
        //            games[index].Player1.Score += score;
        //        }
        //        else
        //        {
        //            games[index].Player2.WordsPlayed.Add(pair);
        //            games[index].Player2.Score += score;
        //        }
        //        score Score = new score();
        //        Score.Score = score;
        //        return Score;
        public Game Status(string GameID, string Brief)
        {
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //Checking if GameID exists in Games table.
                    using (SqlCommand command = new SqlCommand("select GameID from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                                return null;
                            }
                        }
                    }

                    Game returngame = new Game();
                    //Checking if game is active or not
                    using (SqlCommand command = new SqlCommand("select * from Games where GameID = @GameID and Player2 is null", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                returngame.GameState = "pending";
                                SetStatus(OK);
                                return returngame;
                            }
                        }
                    }

                    //Gets UserID of both players and Board
                    string Player1 = "";
                    string Player2 = "";
                    string Board = "";
                    using (SqlCommand command = new SqlCommand("select * from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player1 = (string)reader["Player1"];
                                Player2 = (string)reader["Player2"];
                                Board = (string)reader["Board"];
                            }
                        }
                    }

                    //Gets NickNames of players
                    string Player1Nick = "";
                    string Player2Nick = "";
                    using (SqlCommand command = new SqlCommand("select * from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", Player1);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player1Nick = (string)reader["Nickname"];
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand("select NickName from Users where UserID=@UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", Player2);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player2Nick = (string)reader["Nickname"];
                            }
                        }
                    }

                    //Calculate scores of players and get list of words
                    int Player1Score = 0;
                    int Player2Score = 0;
                    List<WordScorePair> Player1WordsPlayed = new List<WordScorePair>();
                    List<WordScorePair> Player2WordsPlayed = new List<WordScorePair>();
                    WordScorePair word;
                    using (SqlCommand command = new SqlCommand("select * from Words where GameID = @GameID and Player = @Player", conn, trans))
                    {
                        command.Parameters.AddWithValue("@Player", Player1);
                        command.Parameters.AddWithValue("@GameID", GameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player1Score += (int)reader["Score"];
                                word = new WordScorePair();
                                word.Score = (int)reader["Score"];
                                word.Word = (string)reader["Word"];
                                Player1WordsPlayed.Add(word);
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand("select * from Words where GameID = @GameID and Player = @Player", conn, trans))
                    {
                        command.Parameters.AddWithValue("@Player", Player2);
                        command.Parameters.AddWithValue("@GameID", GameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player2Score += (int)reader["Score"];
                                word = new WordScorePair();
                                word.Score = (int)reader["Score"];
                                word.Word = (string)reader["Word"];
                                Player2WordsPlayed.Add(word);
                            }
                        }
                    }

                    using (SqlCommand command = new SqlCommand("select * from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", GameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (Brief == "Yes")
                                {
                                    DateTime temp = (DateTime)reader["StartTime"];
                                    returngame.TimeLeft = (int)reader["TimeLimit"] + (int)(temp.Subtract(DateTime.Now).TotalSeconds);
                                    if (returngame.TimeLeft > 0)
                                    {
                                        returngame.GameState = "active";
                                        returngame.Player1 = new Player();
                                        returngame.Player2 = new Player();
                                        returngame.Player1.Score = Player1Score;
                                        returngame.Player2.Score = Player2Score;
                                        return returngame;
                                    }
                                    else
                                    {
                                        returngame.GameState = "completed";
                                        returngame.TimeLeft = 0;
                                        returngame.Player1 = new Player();
                                        returngame.Player2 = new Player();
                                        returngame.Player1.Score = Player1Score;
                                        returngame.Player2.Score = Player2Score;
                                        SetStatus(OK);
                                        return returngame;
                                    }
                                }
                                else
                                {
                                    DateTime temp = (DateTime)reader["StartTime"];
                                    returngame.TimeLeft = (int)reader["TimeLimit"] + (int)(temp.Subtract(DateTime.Now).TotalSeconds);
                                    if (returngame.TimeLeft > 0)
                                    {
                                        returngame.GameState = "active";
                                        returngame.Board = Board;
                                        returngame.TimeLimit = (int)reader["TimeLimit"];
                                        returngame.Player1 = new Player();
                                        returngame.Player2 = new Player();
                                        returngame.Player1.Nickname = Player1Nick;
                                        returngame.Player1.Score = Player1Score;
                                        returngame.Player2.Nickname = Player2Nick;
                                        returngame.Player2.Score = Player2Score;
                                        SetStatus(OK);
                                        return returngame;
                                    }
                                    else
                                    {
                                        returngame.GameState = "completed";
                                        returngame.Board = Board;
                                        returngame.TimeLimit = (int)reader["TimeLimit"];
                                        returngame.TimeLeft = 0;
                                        returngame.Player1 = new Player();
                                        returngame.Player2 = new Player();
                                        returngame.Player1.Nickname = Player1Nick;
                                        returngame.Player1.Score = Player1Score;
                                        returngame.Player1.WordsPlayed = Player1WordsPlayed;
                                        returngame.Player2.Nickname = Player2Nick;
                                        returngame.Player2.Score = Player2Score;
                                        returngame.Player2.WordsPlayed = Player2WordsPlayed;
                                        SetStatus(OK);
                                        return returngame;
                                    }
                                }
                            }
                        }
                    }
                    return returngame;
                }
            }
            //{
            //        if (games.Count == 0)
            //        {
            //            //NewGame();
            //        }
            //        if (IsInvalidG(GameID))
            //        {
            //            SetStatus(Forbidden);
            //            return null;
            //        }
            //        else
            //        {
            //            Game returngame = new Game();
            //            int index = Int32.Parse(GameID);
            //            if (games[index].GameState == "pending")
            //            {
            //                returngame.GameState = "pending";
            //                SetStatus(OK);
            //                return returngame;
            //            }
            //            if (Brief == "Yes")
            //            {
            //                if (games[index].GameState == "active")
            //                {
            //                    returngame.TimeLeft = games[index].TimeLimit + (int)games[index].startTime.Subtract(DateTime.Now).TotalSeconds;
            //                    if (returngame.TimeLeft > 0)
            //                    {
            //                        returngame.GameState = "active";
            //                        returngame.Player1 = new Player();
            //                        returngame.Player2 = new Player();
            //                        returngame.Player1.Score = games[index].Player1.Score;
            //                        returngame.Player2.Score = games[index].Player2.Score;
            //                        SetStatus(OK);
            //                        return returngame;
            //                    }
            //                    else
            //                    {
            //                        returngame.GameState = "completed";
            //                        returngame.Board = games[index].Boggle.ToString();
            //                        returngame.TimeLimit = games[index].TimeLimit;
            //                        returngame.TimeLeft = 0;
            //                        returngame.Player1 = new Player();
            //                        returngame.Player2 = new Player();
            //                        returngame.Player1.Nickname = games[index].Player1.Nickname;
            //                        returngame.Player1.Score = games[index].Player1.Score;
            //                        returngame.Player1.WordsPlayed = games[index].Player1.WordsPlayed;
            //                        returngame.Player2.Nickname = games[index].Player2.Nickname;
            //                        returngame.Player2.Score = games[index].Player2.Score;
            //                        returngame.Player2.WordsPlayed = games[index].Player2.WordsPlayed;
            //                        games[index] = returngame;
            //                        returngame = new Game();
            //                        returngame.GameState = "completed";
            //                        returngame.TimeLeft = 0;
            //                        returngame.Player1 = new Player();
            //                        returngame.Player2 = new Player();
            //                        returngame.Player1.Score = games[index].Player1.Score;
            //                        returngame.Player2.Score = games[index].Player2.Score;
            //                        SetStatus(OK);
            //                        return returngame;
            //                    }
            //                }
            //                else
            //                {
            //                    returngame.GameState = "completed";
            //                    returngame.TimeLeft = 0;
            //                    returngame.Player1 = new Player();
            //                    returngame.Player2 = new Player();
            //                    returngame.Player1.Score = games[index].Player1.Score;
            //                    returngame.Player2.Score = games[index].Player2.Score;
            //                    SetStatus(OK);
            //                    return returngame;
            //                }

            //            }
            //            else
            //            {
            //                if (games[index].GameState == "active")
            //                {
            //                    returngame.TimeLeft = games[index].TimeLimit + (int)games[index].startTime.Subtract(DateTime.Now).TotalSeconds;
            //                    if (returngame.TimeLeft > 0)
            //                    {
            //                        returngame.GameState = "active";
            //                        returngame.Board = games[index].Boggle.ToString();
            //                        returngame.TimeLimit = games[index].TimeLimit;
            //                        returngame.Player1 = new Player();
            //                        returngame.Player2 = new Player();
            //                        returngame.Player1.Nickname = games[index].Player1.Nickname;
            //                        returngame.Player1.Score = games[index].Player1.Score;
            //                        returngame.Player2.Nickname = games[index].Player2.Nickname;
            //                        returngame.Player2.Score = games[index].Player2.Score;
            //                        SetStatus(OK);
            //                        return returngame;
            //                    }
            //                    else
            //                    {
            //                        returngame.GameState = "completed";
            //                        returngame.Board = games[index].Boggle.ToString();
            //                        returngame.TimeLimit = games[index].TimeLimit;
            //                        returngame.TimeLeft = 0;
            //                        returngame.Player1 = new Player();
            //                        returngame.Player2 = new Player();
            //                        returngame.Player1.Nickname = games[index].Player1.Nickname;
            //                        returngame.Player1.Score = games[index].Player1.Score;
            //                        returngame.Player1.WordsPlayed = games[index].Player1.WordsPlayed;
            //                        returngame.Player2.Nickname = games[index].Player2.Nickname;
            //                        returngame.Player2.Score = games[index].Player2.Score;
            //                        returngame.Player2.WordsPlayed = games[index].Player2.WordsPlayed;
            //                        games[index] = returngame;
            //                        //games[index] = new Game();
            //                        //games[index].GameState = "completed";
            //                        //games[index].Board = returngame.Board;
            //                        //games[index].TimeLimit = returngame.TimeLimit;
            //                        //games[index].TimeLeft = 0;
            //                        //games[index].Player1 = new Player();
            //                        //games[index].Player2 = new Player();
            //                        //games[index].Player1.Nickname = returngame.Player1.Nickname;
            //                        //games[index].Player1.Score = returngame.Player1.Score;
            //                        //games[index].Player1.WordsPlayed = returngame.Player1.WordsPlayed;
            //                        //games[index].Player2.Nickname = returngame.Player2.Nickname;
            //                        //games[index].Player2.Score = returngame.Player2.Score;
            //                        //games[index].Player2.WordsPlayed = returngame.Player2.WordsPlayed;
            //                        SetStatus(OK);
            //                        return games[index];
            //                    }
            //                }
            //                else
            //                {
            //                    SetStatus(OK);
            //                    return games[index];
            //                }
            //            }                  
            //        }
            //    }
            //}
        }
    }
}
    
