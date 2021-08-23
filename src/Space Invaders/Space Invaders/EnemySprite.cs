using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders
{
    class EnemySprite : Sprite
    {
        // Enemy sprite constructor
        public EnemySprite(Texture2D textureImage, Vector2 position, Point frameSize,
            Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, float scale, Boolean spriteActive, Boolean movingFoward, Vector2 firePosition)
            : base(textureImage, position, frameSize,currentFrame,
            sheetSize, speed, millisecondsPerFrame, scale, spriteActive, movingFoward, firePosition)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // This makes the enemy sprites move
            if (movingFoward != true)
                spritePosition -= new Vector2(1, 0);
            if (movingFoward != false)
                spritePosition += new Vector2(1, 0);

            base.Update(gameTime, clientBounds);
        }
    }
}
