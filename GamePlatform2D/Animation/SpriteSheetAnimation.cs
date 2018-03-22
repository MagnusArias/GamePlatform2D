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
        int count, currFrame;
        #endregion

        #region Properties
        
        #endregion

        public SpriteSheetAnimation()
        {
            frameCounter = currFrame = 0;
            switchFrame = numFrames = (int)Frames.X;
            count = 0;
            delay = 5;
        }

        #region Public Methods

        public override void Update(GameTime gameTime, ref Animation a)
        {
            if (delay == -1) return;

            count++;

            if (count >= delay)
            {
                count = 0;

                if (a.IsActive)
                {
                    frameCounter++;
                    if (frameCounter >= switchFrame)
                    {
                        frameCounter = 0;
                        currentFrame.X++;

                        if (currentFrame.X * a.FrameWidth >= a.Image.Width) currentFrame.X = 0;

                    }
                }
                else
                {
                    frameCounter = 0;
                    currentFrame.X = 0;
                }

                a.CurrentFrame = currentFrame;
                a.SourceRect = new Rectangle((int)currentFrame.X * a.FrameWidth, (int)currentFrame.Y * a.FrameHeight, a.FrameWidth, a.FrameHeight);
            }

            currentFrame = a.CurrentFrame;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
