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
        Player player;
        private Vector2 playerPos;



        public void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            fileManager = new FileManager();
            playerPos = Vector2.Zero;

            this.image = image;
            this.text = text;
            this.position = position;

            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("Font1");
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

        public virtual void Update(GameTime gameTime, Entity p)
        {
            player = (Player)p;
            playerPos = player.Position;
            text = "X= " + playerPos.X + "\nY= " + playerPos.Y;
            position = new Vector2((int)player.Position.X + 50, (int)player.Position.Y - 50);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int alpha = 255;
            if (text != String.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin, color * alpha, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
            }
        }
    }
}
