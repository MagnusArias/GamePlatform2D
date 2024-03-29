﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class Tile
    {
        public enum State { Empty, Solid, Platform };
        public enum Motion { Static, Horizontal, Vertical };

        private State state;
        Motion motion;
        Vector2 position, prevPosition, velocity;
        Texture2D tileImage;

        float range;
        int counter;
        bool increase;
        float moveSpeed;
        bool containsEntity;

        Animation animation;

        public State States
        {
            get { return state; }
            set { state = value; }
        }
        public Motion Motions
        {
            get { return motion; }
            set { motion = value; }
        }

        public Vector2 PrevPosition
        {
            get { return prevPosition; }
            set { position = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool ContainsEntity
        {
            get { return containsEntity; }
            set { containsEntity = value; }
        }

        private Texture2D CropImage(Texture2D tileSheet, Rectangle tileArea)
        {
            Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);
            Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
            Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

            tileSheet.GetData<Color>(tileSheetData);

            int index = 0;

            for (int y = tileArea.Y; y < tileArea.Y + tileArea.Height; y++)
            {
                for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
                {
                    croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
                    index++;
                }
            }

            croppedImage.SetData<Color>(croppedImageData);
            return croppedImage;
        }

        public void SetTile(State state, Motion motion, Vector2 position, Texture2D tileSheet, Rectangle tileArea)
        {
            this.state = state;
            this.motion = motion;
            this.position = position;
            containsEntity = false;
            velocity = new Vector2(0, 0);
            tileImage = CropImage(tileSheet, tileArea);
            range = 50;
            counter = 0;
            moveSpeed = 100.0f;
            increase = true;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
        }

        public void Update(GameTime gameTime)
        {
            counter++;
            prevPosition = position;

            if (counter >= range)
            {
                counter = 0;
                increase = !increase;
            }
            if (motion == Motion.Horizontal)
            {
                if (increase)
                    velocity.X = (float)gameTime.ElapsedGameTime.TotalSeconds * moveSpeed;
                else velocity.X = -(float)gameTime.ElapsedGameTime.TotalSeconds * moveSpeed;
            }
            else if (motion == Motion.Vertical)
            {
                if (increase)
                    velocity.Y = (float)gameTime.ElapsedGameTime.TotalSeconds * moveSpeed;
                else velocity.Y = -(float)gameTime.ElapsedGameTime.TotalSeconds * moveSpeed;
            }

            position += velocity;
            animation.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch, SpriteEffects.None);
        }
    }
}
