using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class GameplayScreen : GameScreen
    {
        #region Variables
        EntityManager player, enemies;
        Map map;
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            player = new EntityManager();
            enemies = new EntityManager();
            map = new Map();
            player.LoadContent("Player", content, "Load/Entity/Player.ma", "", input);
            enemies.LoadContent("Enemy", content, "Load/Entity/Enemy.ma", "Level1", input);
            map.LoadContent(content, map, "Map1");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            enemies.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            player.Update(gameTime, map);
            enemies.Update(gameTime, map);
            map.Update(gameTime);

            Entity e;

            for (int i = 0; i < player.Entities.Count; i++)
            {
                e = player.Entities[i];
                map.UpdateCollision(ref e);
                player.Entities[i] = e;
            }

            for (int i = 0; i < enemies.Entities.Count; i++)
            {
                e = enemies.Entities[i];
                map.UpdateCollision(ref e);
                enemies.Entities[i] = e;
            }

            player.EntityColision(enemies);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            enemies.Draw(spriteBatch);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
