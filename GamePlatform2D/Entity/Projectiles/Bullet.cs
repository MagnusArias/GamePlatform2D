using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class Bullet
    {
        private Vector2 position;
        private int direction;
        private float bulletSpeed;
        private Animation animation;
        private Texture2D image;
        ContentManager content;


        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Bullet(Vector2 pos, int dir, float speed, Texture2D img)
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            position = pos;
            direction = dir;
            bulletSpeed = speed;
            animation = new Animation();
            image = img;
            //animation.LoadContent(content, image, "", position);
        }

        public void LoadContent(ContentManager content, List<string> attributes, List<string> contents)
        {
        }

        public void Update(GameTime gameTime)
        {
            //float x = bullets[i].X;
            // x += bulletSpeed *(float)gameTime.ElapsedGameTime.TotalSeconds;
            // bullets[i] = new Vector2(x, bullets[i].Y);
            float x = position.X;
            x += bulletSpeed * direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position = new Vector2(x, position.Y);
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, Color.White);
        }
    }
}
