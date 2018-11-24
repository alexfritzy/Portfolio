using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Dynamic;

namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }
    [TestClass]
    public class BoggleTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");

        /// <summary>
        /// Note that DoGetAsync (and the other similar methods) returns a Response object, which contains
        /// the response Stats and the deserialized JSON response (if any).  See RestTestClient.cs
        /// for details.
        /// </summary>

        //[TestMethod]
        //public void SingleUserGame()
        //{
        //    dynamic user = new ExpandoObject();
        //    user.Nickname = "Conner";
        //    Response r = client.DoPostAsync("users", user).Result;
        //    Assert.AreEqual(Created, r.Status);
        //    user.UserToken = r.Data.UserToken;
        //    user.TimeLimit = 30;
        //    r = client.DoPostAsync("games", user).Result;
        //    Assert.AreEqual(Created, r.Status);
        //    r = client.DoPutAsync(user, "games").Result;
        //    Assert.AreEqual(OK, r.Status);
        //}

        //[TestMethod]
        //public void Test()
        //{
        //    // Register 1
        //    dynamic user = new ExpandoObject();
        //    user.Nickname = "Alex";
        //    Response r = client.DoPostAsync("users", user).Result;
        //    Assert.AreEqual(Created, r.Status);
        //    user.UserToken = r.Data.UserToken;
        //    // Join 1
        //    user.TimeLimit = 5;
        //    r = client.DoPostAsync("games", user).Result;
        //    Assert.AreEqual(Accepted, r.Status);
        //    user.GameID = r.Data.GameID;
        //    // ReJoin 1
        //    r = client.DoPostAsync("games", user).Result;
        //    Assert.AreEqual(Conflict, r.Status);
        //    // Leave 1
        //    r = client.DoPutAsync(user, "games").Result;
        //    Assert.AreEqual(OK, r.Status);
        //    // Bad Leave 1
        //    r = client.DoPutAsync(user, "games").Result;
        //    Assert.AreEqual(Forbidden, r.Status);
        //    // Join 1
        //    r = client.DoPostAsync("games", user).Result;
        //    Assert.AreEqual(Accepted, r.Status);
        //    user.GameID = r.Data.GameID;
        //    // Register 2
        //    dynamic user2 = new ExpandoObject();
        //    user2.Nickname = "Conner";
        //    r = client.DoPostAsync("users", user2).Result;
        //    Assert.AreEqual(Created, r.Status);
        //    user2.UserToken = r.Data.UserToken;
        //    // Join 2
        //    user2.TimeLimit = 15;
        //    r = client.DoPostAsync("games", user2).Result;
        //    Assert.AreEqual(Created, r.Status);
        //}





        [TestMethod]
        public void ForbiddenStuff()
        {
            // Forbidden Names
            dynamic user = new ExpandoObject();
            Response r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);
            user.Nickname = "";
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);
            user.Nickname = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Forbidden, r.Status);
            // Forbidden JoinRequests
            user.UserToken = "abc";
            user.TimeLimit = 0;
            r = client.DoPostAsync("games", user).Result;
            Assert.AreEqual(Forbidden, r.Status);
            // Forbidden Cancel
            r = client.DoPutAsync(user, "games").Result;
            Assert.AreEqual(Forbidden, r.Status);
            // Forbidden Word
            user.Word = null;
            user.GameID = "abc";
            r = client.DoPutAsync(user, "games/" + user.GameID).Result;
            Assert.AreEqual(Forbidden, r.Status);
            // Get status of non-existing game
            r = client.DoGetAsync("games/5").Result;
            Assert.AreEqual(Forbidden, r.Status);
        }
        [TestMethod]
        public void TestGame()
        {
            // Register first player
            dynamic user = new ExpandoObject();
            user.Nickname = "Conner";
            Response r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
            user.UserToken = r.Data.UserToken;
            user.TimeLimit = 5;
            // First player joins game
            r = client.DoPostAsync("games", user).Result;
            Assert.AreEqual(Accepted, r.Status);
            // First player leaves games
            r = client.DoPutAsync(user, "games").Result;
            Assert.AreEqual(OK, r.Status);
            // First player joins again
            r = client.DoPostAsync("games", user).Result;
            Assert.AreEqual(Accepted, r.Status);
            user.GameID = r.Data.GameID;
            // First player tries to play word before game starts.
            user.Word = "test";
            r = client.DoPutAsync(user, "games/" + user.GameID).Result;
            Assert.AreEqual(Conflict, r.Status);
            // Status call from pending game.
            r = client.DoGetAsync("games/" + user.GameID + "/yes").Result;
            Assert.AreEqual((string)r.Data.GameState, "pending");
            // Second player registers
            dynamic user2 = new ExpandoObject();
            user2.Nickname = "Alex";
            r = client.DoPostAsync("users", user2).Result;
            Assert.AreEqual(Created, r.Status);
            // Second player joins
            user2.UserToken = r.Data.UserToken;
            user2.TimeLimit = 5;
            r = client.DoPostAsync("games", user2).Result;
            Assert.AreEqual(Created, r.Status);
            user2.GameID = r.Data.GameID;
            // Second player plays a word
            user2.Word = "notaword";
            r = client.DoPutAsync(user2, "games/" + user2.GameID).Result;
            Assert.AreEqual(OK, r.Status);
            int score = r.Data.Score;
            Assert.AreEqual(-1, score);
            // Second player plays same word
            user2.Word = "notaword";
            r = client.DoPutAsync(user2, "games/" + user2.GameID).Result;
            score = r.Data.Score;
            Assert.AreEqual(0, score);
            // First player plays word
            user.Word = "ao";
            r = client.DoPutAsync(user, "games/" + user.GameID).Result;
            score = r.Data.Score;
            Assert.AreEqual(0, score);
            // Check state of game.
            r = client.DoGetAsync("games/" + user.GameID + "/yes").Result;
            Assert.AreEqual((int)r.Data.Player1.Score, 0);
            Assert.AreEqual((int)r.Data.Player2.Score, -1);
            Assert.AreEqual((string)r.Data.GameState, "active");
            Assert.AreEqual(r.Status, OK);
            r = client.DoGetAsync("games/" + user.GameID).Result;
            Assert.AreEqual((string)r.Data.Player1.Nickname, "Conner");
            // Check state of game after 6 seconds.
            System.Threading.Thread.Sleep(6000);
            r = client.DoGetAsync("games/" + user.GameID + "/yes").Result;
            Assert.AreEqual((int)r.Data.Player1.Score, 0);
            Assert.AreEqual((int)r.Data.Player2.Score, -1);
            Assert.AreEqual((string)r.Data.GameState, "completed");
            Assert.AreEqual(r.Status, OK);
            r = client.DoGetAsync("games/" + user.GameID).Result;
            Assert.AreEqual((string)r.Data.Player1.Nickname, "Conner");
            // First player joins again
            r = client.DoPostAsync("games", user).Result;
            Assert.AreEqual(Accepted, r.Status);
            user.GameID = r.Data.GameID;
            // Second player joins again
            r = client.DoPostAsync("games", user2).Result;
            Assert.AreEqual(Created, r.Status);
            user2.GameID = r.Data.GameID;
            // Check state of game brief second.
            r = client.DoGetAsync("games/" + user.GameID).Result;
            Assert.AreEqual((string)r.Data.Player1.Nickname, "Conner");
            r = client.DoGetAsync("games/" + user.GameID + "/yes").Result;
            Assert.AreEqual((string)r.Data.GameState, "active");
            Assert.AreEqual(r.Status, OK);
            // Check state of game after 6 seconds brief second.
            System.Threading.Thread.Sleep(6000);
            r = client.DoGetAsync("games/" + user.GameID).Result;
            Assert.AreEqual((string)r.Data.Player1.Nickname, "Conner");
            r = client.DoGetAsync("games/" + user.GameID + "/yes").Result;
            Assert.AreEqual((string)r.Data.GameState, "completed");
            Assert.AreEqual(r.Status, OK);
        }
        [TestMethod]
        public void RunAlotOfGames()
        {
            dynamic user = new ExpandoObject();
            user.Nickname = "Conner";
            Response r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
            user.UserToken = r.Data.UserToken;
            user.TimeLimit = 30;
            // First player joins game
            r = client.DoPostAsync("games", user).Result;
            Assert.AreEqual(Accepted, r.Status);
            // Second player registers
            dynamic user2 = new ExpandoObject();
            user2.Nickname = "Alex";
            r = client.DoPostAsync("users", user2).Result;
            Assert.AreEqual(Created, r.Status);
            // Second player joins
            user2.UserToken = r.Data.UserToken;
            user2.TimeLimit = 34;
            r = client.DoPostAsync("games", user2).Result;
            Assert.AreEqual(Created, r.Status);
            user2.GameID = r.Data.GameID;
            user = new ExpandoObject();
            user.Nickname = "Conner";
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
            // First player joins game
            user.UserToken = r.Data.UserToken;
            user.TimeLimit = 30;
            r = client.DoPostAsync("games", user).Result;
            Assert.AreEqual(Accepted, r.Status);
            // Second player registers
            user2 = new ExpandoObject();
            user2.Nickname = "Alex";
            r = client.DoPostAsync("users", user2).Result;
            Assert.AreEqual(Created, r.Status);
            // Second player joins
            user2.UserToken = r.Data.UserToken;
            user2.TimeLimit = 34;
            r = client.DoPostAsync("games", user2).Result;
            Assert.AreEqual(Created, r.Status);
            user2.GameID = r.Data.GameID;
            user = new ExpandoObject();
            user.Nickname = "Conner";
            r = client.DoPostAsync("users", user).Result;
            Assert.AreEqual(Created, r.Status);
            // First player joins game
            user.UserToken = r.Data.UserToken;
            user.TimeLimit = 30;
            r = client.DoPostAsync("games", user).Result;
            Assert.AreEqual(Accepted, r.Status);
            // Second player registers
            user2 = new ExpandoObject();
            user2.Nickname = "Alex";
            r = client.DoPostAsync("users", user2).Result;
            Assert.AreEqual(Created, r.Status);
            // Second player joins
            user2.UserToken = r.Data.UserToken;
            user2.TimeLimit = 34;
            r = client.DoPostAsync("games", user2).Result;
            Assert.AreEqual(Created, r.Status);
            user2.GameID = r.Data.GameID;
        }
    }
}
