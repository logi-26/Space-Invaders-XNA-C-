using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space_Invaders
{
    class Player
    {
        // Player lives
        private int _playerLives = 3;

        // Player score
        private int _playerScore = 0;

        // Player end score
        private int _playerFinalScore = 0;

        // Get/Set for the player lives variable
        public int playerLives
        {
            get { return _playerLives; }
            set { _playerLives = value; }
        }

        // Get/Set for the player score variable
        public int playerScore
        {
            get { return _playerScore; }
            set { _playerScore = value; }
        }

        // Get/Set for the player final score variable
        public int playerFinalScore
        {
            get { return _playerFinalScore; }
            set { _playerFinalScore = value; }
        }
    }
}
