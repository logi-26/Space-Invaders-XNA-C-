using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space_Invaders
{
    class GameStateManager : GameComponent
    {
        public GameStateManager(Game game)
            : base(game)
        {
        }

        //Game states
        public enum GameState { GameStart, GamePlay, GameOver, GameWon };
        private GameState _currentGameState = GameState.GameStart;


        public GameState currentGameState
        {
            get { return _currentGameState; }
            set { _currentGameState = value; }
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
