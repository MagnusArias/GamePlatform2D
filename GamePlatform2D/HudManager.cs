using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class HudManager
    {
        private Vector2 position, origin, scale;
        private Texture2D image;
        private SpriteFont font;
        private ContentManager content;
        private FileManager fileManager;
        private Color color;
        private String text;
        


        public void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            fileManager = new FileManager();

            this.image = image;
            this.text = text;
            this.position = position;

            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("AnimationFont");
                color = new Color(114, 77, 233);
            }

        }

        public void UnloadContent()
        {
            content.Unload();
            text = String.Empty;
            position = Vector2.Zero;
            image = null;
        }

        public virtual void Update(GameTime gameTime, ref Animation a)
        {

        }

        /*public void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
            {
                origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                spriteBatch.Draw(image, position + origin, sourceRect, drawColor * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
            }

            if (text != String.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin, color * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
            }
        }*/
    }
}
