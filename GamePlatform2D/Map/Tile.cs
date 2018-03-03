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
        bool onTile;

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
            onTile = false;
            velocity = new Vector2(0, 0);
            tileImage = CropImage(tileSheet, tileArea);
            range = 50;
            counter = 0;
            moveSpeed = 100.0f;
            increase = true;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
        }

        public void Update(GameTime gameTime, ref Player player)
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

            FloatRect rect = new FloatRect(position.X, position.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

            if (onTile)
            {
                if (!player.SyncTilePosition)
                {
                    player.Position += velocity;
                    player.SyncTilePosition = true;
                }

                if (player.Rect.Right < rect.Left || player.Rect.Left > rect.Right || player.Rect.Bottom != rect.Top)
                {
                    onTile = false;
                    player.ActivateGravity = true;
                }
            }

            if (player.Rect.Intersects(rect) && state == State.Solid)
            {
                FloatRect prevPlayer = new FloatRect(player.PrevPosition.X, player.PrevPosition.Y, player.Animation.FrameWidth, player.Animation.FrameHeight);
                FloatRect prevTile = new FloatRect(prevPosition.X, prevPosition.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

                if (player.Rect.Bottom >= rect.Top && prevPlayer.Bottom <= prevTile.Top)
                {
                    // bottom collision
                    player.Position = new Vector2(player.Position.X, position.Y - player.Animation.FrameHeight);
                    player.ActivateGravity = false;
                    onTile = true;
                }
                else if (player.Rect.Top <= rect.Bottom && prevPlayer.Top >= prevTile.Bottom)
                {
                    // top collision
                    player.Position = new Vector2(player.Position.X, position.Y + Layer.TileDimensions.Y);
                    player.Velocity = new Vector2(player.Velocity.X, 0);
                    player.ActivateGravity = true;
                }
                else
                {
                    player.Position -= player.Velocity;
                }
            }
            player.Animation.Position = player.Position;
        }

        public void Draw(SpriteBatch sb)
        {
            animation.Draw(sb);
        }
    }
}
