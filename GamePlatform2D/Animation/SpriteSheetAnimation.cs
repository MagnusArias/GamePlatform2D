using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class SpriteSheetAnimation : Animation
    {
        #region Variables
        int frameCounter, switchFrame;
        Vector2 currentFrame;
        #endregion

        #region Properties

        #endregion

        public SpriteSheetAnimation()
        {
            frameCounter = 0;
            switchFrame = 100;
        }

        #region Public Methods

        public override void Update(GameTime gameTime, ref Animation a)
        {
            CurrentFrame = a.CurrentFrame;
            if (a.IsActive)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    currentFrame.X++;

                    if (currentFrame.X * FrameWidth >= a.Image.Width) currentFrame.X = 0;
                }
            }
            else
            {
                frameCounter = 0;
                currentFrame.X = 1;
            }
            a.CurrentFrame = currentFrame;
            a.SourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
