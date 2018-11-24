using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

/// <summary>
/// Contains definitions of classes implemented in the BoggleService
/// </summary>

namespace Boggle
{
    public class RegisterRequest
    {
        public string Nickname { get; set; }
    }

    public class UserID
    {
        public string UserToken { get; set; }
    }

    public class gameID
    {
        public string GameID { get; set; }
    }


    [DataContract]
    public class Game
    {
        [DataMember]
        public string GameState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Board { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int TimeLimit { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public int TimeLeft { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public Player Player1 { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public Player Player2 { get; set; }

        public BoggleBoard Boggle { get; set; }

        public DateTime startTime { get; set; }
    }

    public class Player
    {
        public string UserToken { get; set; }

        public string Nickname { get; set; }

        [DataMember(EmitDefaultValue = true)]
        public int Score { get; set; }

        public List<WordScorePair> WordsPlayed { get; set; }
    }

    public class WordScorePair
    {
        public string Word { get; set; }

        public int Score { get; set; }
    }

    public class PlayWord
    {
        public string UserToken { get; set; }

        public string Word { get; set; }
    }

    public class JoinRequest
    {
        public string UserToken { get; set; }

        public int TimeLimit { get; set; }
    }

    public class score
    {
        public int Score { get; set; }
    }
}