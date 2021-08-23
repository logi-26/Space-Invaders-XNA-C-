using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders
{
    class GameTimer : DrawableGameComponent
    {
        public GameTimer(Game game)
            : base(game)
        {
        }

        // SpriteBatch
        SpriteBatch spriteBatch;

        // Font
        private SpriteFont timerFont;

        // Timer variables
        private int _counter = 1;
        private float _countDuration = 1f;
        private float _currentTime = 0f;

        // Get/Set for the counter variable
        public int counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        // Get/Set for the count duration variable
        public float countDuration
        {
            get { return _countDuration; }
            set { _countDuration = value; }
        }

        // Get/Set for the current time variable
        public float currentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; }
        }


        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Loads the game timer font
            timerFont = Game.Content.Load<SpriteFont>(@"Font/timerFont");

            // Initialises the game timer variables
            _counter = 1;
            _countDuration = 1.0f;
            _currentTime = 0.0f;

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            // Updates the game timer
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_currentTime >= _countDuration)
            {
                _counter++;
                _currentTime -= _countDuration;
            }
            
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Draws the timer font
            spriteBatch.DrawString(timerFont, _counter.ToString(), new Vector2(480, 10), Color.White,
                0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
