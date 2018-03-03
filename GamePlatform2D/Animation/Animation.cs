using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePlatform2D
{
    public class Animation
    {
        #region Variables
        private Texture2D image;
        private string text;
        private SpriteFont font;
        private Color color;
        private Rectangle sourceRect;
        private float rotation, scale, alpha;
        private Vector2 origin, position, frames, currentFrame;
        private ContentManager content;
        private bool isActive;

        #endregion

        #region Properties

        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }
        public bool IsActive
        {
            set { isActive = value; }
            get { return isActive; }
        }

        public virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public virtual float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public virtual SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }
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

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public Texture2D Image
        {
            get { return image; }
        }
        #endregion

        #region Public Methods
        public void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");

            this.image = image;
            this.text = text;
            this.position = position;

            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("AnimationFont");
                color = new Color(114, 77, 233);
            }

            rotation = 0.0f;
            scale = 1.0f;
            alpha = 1.0f;
            isActive = false;
            currentFrame = new Vector2(0, 0);
            if (image != null && frames != Vector2.Zero)
                sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
            else
                sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public void UnloadContent()
        {
            content.Unload();
            text = String.Empty;
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;
        }

        public virtual void Update(GameTime gameTime, ref Animation a)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
            {
                origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                spriteBatch.Draw(image, position + origin, sourceRect, Color.White * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
            }

            if (text != String.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin, color * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
            }
        }
        #endregion
    }
}
