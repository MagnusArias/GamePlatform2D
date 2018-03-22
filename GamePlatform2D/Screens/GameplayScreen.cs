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
        EntityManager player;//, enemies;
        Map map;
        bool blockInput;
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            blockInput = false;
            player = new EntityManager();
            //enemies = new EntityManager();
            map = new Map();

            //enemies.LoadContent("Enemy", content, "Load/Entity/Enemy.ma", "Level1", input);
            map.LoadContent(content, map, "Map1");
            player.LoadContent("Player", content, "Load/Entity/Player.ma", "", input, map.layer);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            //enemies.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            HandleInput();
            map.Update(gameTime);
            player.Update(gameTime, map);
            //enemies.Update(gameTime, map);


            Entity e;

            for (int i = 0; i < player.Entities.Count; i++)
            {
                e = player.Entities[i];
                player.Entities[i] = e;
            }

            /*for (int i = 0; i < enemies.Entities.Count; i++)
            {
                e = enemies.Entities[i];
                map.UpdateCollision(ref e);
                enemies.Entities[i] = e;
            }*/

            //player.EntityColision(enemies);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            //enemies.Draw(spriteBatch);
        }

        public override void HandleInput()
        {
           if (!blockInput)
            {
                player.Entities[0].SetJumping(Keys.Up);
                player.Entities[0].SetLeft(Keys.Left);
                player.Entities[0].SetRight(Keys.Right);
                player.Entities[0].SetDown(Keys.Down);
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
