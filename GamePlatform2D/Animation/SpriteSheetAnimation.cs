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
        int frameCounter;
        int switchFrame;
        Vector2 frames;
        Vector2 currentFrame;
        #endregion

        #region Properties
        public Vector2 Frames
        {
            set { frames = value; }
            get { return frames; }
        }

        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }

        public int FrameWidth
        {
            get { return image.Width / (int)Frames.X; }
        }

        public int FrameHeight
        {
            get { return image.Height / (int)Frames.Y; }
        }
        #endregion

        #region Public Methods

        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            base.LoadContent(Content, image, text, position);
            frameCounter = 0;
            switchFrame = 100;
            frames = new Vector2(3, 4);
            currentFrame = new Vector2(0, 0);
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    currentFrame.X++;

                    if (currentFrame.X * FrameWidth >= image.Width) currentFrame.X = 0;
                }
            }
            else
            {
                frameCounter = 0;
                currentFrame.X = 1;
            }
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
