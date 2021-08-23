using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders
{
    class BackgroundSprite : Sprite
    {
        // Background sprite constructor
        public BackgroundSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame, float scale, Boolean spriteActive)
            : base(textureImage, position, frameSize, currentFrame,
            sheetSize, speed, millisecondsPerFrame, scale, spriteActive)
        {
        }
    }
}
