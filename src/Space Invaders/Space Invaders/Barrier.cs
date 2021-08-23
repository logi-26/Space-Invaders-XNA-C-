using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders
{
    class Barrier : DrawableGameComponent
    {
        // SpriteBatch
        SpriteBatch spriteBatch;

        // Barrier variables
        private int _barrierHits = 0;
        private Texture2D barriertexture;
        private bool _spriteActive = true;
        private Rectangle _collisionRect = new Rectangle(0, 0, 0, 0);
        float _barrierPositionX;
        float _barrierPositionY;

        // Barrier constructor
        public Barrier(Game game, float x, float y)
            : base(game)
        {
            this._barrierPositionX = x;
            this._barrierPositionY = y;
        }

        // Get/Set for the barrier hits variable
        public int barrierHits
        {
            get { return _barrierHits; }
            set { _barrierHits = value; }
        }

        // Get/Set for the collision rectangle
        public Rectangle collisionRect
        {
            get { return _collisionRect; }
            set { _collisionRect = value; }
        }

        // Get/Set for the barrier position X
        public float barrierPositionX
        {
            get { return _barrierPositionX; }
            set { _barrierPositionX = value; }
        }

        // Get/Set for the barrier position Y
        public float barrierPositionY
        {
            get { return _barrierPositionY; }
            set { _barrierPositionY = value; }
        }

        // Get/Set for the barrier active variable
        public bool spriteActive
        {
            get { return _spriteActive; }
            set { _spriteActive = value; }
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Loads the barrier texture
            barriertexture = Game.Content.Load<Texture2D>(@"Backgrounds/Barrier");

            _barrierHits = 0;
            _spriteActive = true;

            // Convert barrier position to int
            int x = (int)Math.Ceiling(_barrierPositionX);
            int y = (int)Math.Ceiling(_barrierPositionY);

            // Creates the collision rectangle
            _collisionRect.X = x;
            _collisionRect.Y = y;
            _collisionRect.Width = barriertexture.Width;
            _collisionRect.Height = barriertexture.Height;

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            // Switches the barrier textures depending on the value of the barrier hits variable
            if(_barrierHits == 0) barriertexture = Game.Content.Load<Texture2D>(@"Backgrounds/Barrier");
            else if (_barrierHits == 1) barriertexture = Game.Content.Load<Texture2D>(@"Backgrounds\Barrier_Crack_1");
            else if (_barrierHits == 2) barriertexture = Game.Content.Load<Texture2D>(@"Backgrounds\Barrier_Crack_2");
            
            // If the barrier has been hit 3 times, it is made in-active
            if (_barrierHits == 3) _spriteActive = false;

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Draws the barrier if it is active
            if (_spriteActive == true) spriteBatch.Draw(barriertexture, new Vector2(_barrierPositionX, _barrierPositionY), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
