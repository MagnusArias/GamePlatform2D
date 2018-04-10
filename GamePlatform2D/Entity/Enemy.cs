using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePlatform2D
{
    public class Enemy : Entity
    {
        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input, TileMap l)
        {
            base.LoadContent(content, attributes, contents, input, l);
            direction = Directions.Right;

            origPosition = position;
            if (direction == Directions.Right) dest.X = origPosition.X + range;
            else dest.X = origPosition.X - range;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, TileMap layer)
        {
            base.Update(gameTime, input, layer);

           
            moveAnimation.Position = position;
            ssAnimation.Update(gameTime, ref moveAnimation);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch, SpriteEffects.None);
        }
    }
}
