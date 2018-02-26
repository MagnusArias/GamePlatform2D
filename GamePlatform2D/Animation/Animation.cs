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
        protected Texture2D image;
        protected string text;
        protected SpriteFont font;
        protected Color color;
        protected Rectangle sourceRect;
        protected float rotation, scale, axis;
        protected Vector2 origin, position;
        protected ContentManager content;
        protected bool isActive;
        protected float alpha;
        #endregion

        #region Properties
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
        #endregion

        #region Methods
        public virtual void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
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

            if (image != null)
                sourceRect = new Rectangle(0, 0, image.Width, image.Height);

            rotation = 0.0f;
            axis = 0.0f;
            scale = 1.0f;
            alpha = 1.0f;
            isActive = false;
        }

        public virtual void UnloadContent()
        {
            content.Unload();
            text = String.Empty;
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch)
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
