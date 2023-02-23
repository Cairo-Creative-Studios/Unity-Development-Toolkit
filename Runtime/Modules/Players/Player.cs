using UnityEngine;

namespace UDT.Core
{
    public class Player
    {
        [Tooltip("The ID of the player")]
        public int playerID;
        [Tooltip("The Player's Name")]
        public string Name;
        [Tooltip("The Player's Score")]
        public int Score;
        [Tooltip("Whether the Player is Local or not")]
        public bool IsLocalPlayer;
        [Tooltip("Is the Player Ready?")]
        public bool IsReady;
        [Tooltip("The Player's Input")]
        public byte Inputs = 0;

        public Player(int id, bool isLocal, string name = "", int score = 0, bool isReady = false)
        {
            playerID = id;
            Name = name;
            Score = score;
            IsLocalPlayer = isLocal;
            this.IsReady = isReady;
        }

        public void OnRemovePlayer()
        {
            
        }
    }
}