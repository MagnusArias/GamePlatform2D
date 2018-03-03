using System;
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
        public enum State { Solid, Passive };
        public enum Motion { Static, Horizontal, Vertical };

        State state;
        Motion motion;
        Vector2 position, prevPosition, velocity;
        Texture2D tileImage;

        float range;
        int counter;
        bool increase;
        float moveSpeed;
        bool containsEntity;

        Animation animation;

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

        public void UpdateCollision(ref Entity e)
        {
            FloatRect rect = new FloatRect(position.X, position.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

            if (e.OnTile && containsEntity)
            {
                if (!e.SyncTilePosition)
                {
                    e.Position += velocity;
                    e.SyncTilePosition = true;
                }

                if (e.Rect.Right < rect.Left || e.Rect.Left > rect.Right || e.Rect.Bottom != rect.Top)
                {
                    e.OnTile = false;
                    containsEntity = false;
                    e.ActivateGravity = true;
                }
            }

            if (e.Rect.Intersects(rect) && state == State.Solid)
            {
                FloatRect preve = new FloatRect(e.PrevPosition.X, e.PrevPosition.Y, e.Animation.FrameWidth, e.Animation.FrameHeight);
                FloatRect prevTile = new FloatRect(prevPosition.X, prevPosition.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

                if (e.Rect.Bottom >= rect.Top && preve.Bottom <= prevTile.Top)
                {
                    // bottom collision
                    e.Position = new Vector2(e.Position.X, position.Y - e.Animation.FrameHeight);
                    e.ActivateGravity = false;
                    e.OnTile = true;
                    containsEntity = true;
                }
                else if (e.Rect.Top <= rect.Bottom && preve.Top >= prevTile.Bottom)
                {
                    // top collision
                    e.Position = new Vector2(e.Position.X, position.Y + Layer.TileDimensions.Y);
                    e.Velocity = new Vector2(e.Velocity.X, 0);
                    e.ActivateGravity = true;
                }
                else if (e.Rect.Right >= rect.Left && preve.Right <= prevTile.Left)
                {
                    e.Position = new Vector2(position.X - e.Animation.FrameWidth, e.Position.Y);
                    e.Direction = (e.Direction == 1) ? e.Direction = 2 : e.Direction = 1;
                }
                else if (e.Rect.Left <= rect.Right && preve.Left >= prevTile.Right)
                {
                    e.Position = new Vector2(position.X + Layer.TileDimensions.X, e.Position.Y);
                    e.Direction = (e.Direction == 1) ? e.Direction = 2 : e.Direction = 1;

                }
            }
            e.Animation.Position = e.Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
