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
        Player player;
        Layers layer;
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            player = new Player();
            layer = new Layers();

            player.LoadContent(content, input);
            layer.LoadContent(content, "Map1");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            player.Update(gameTime, inputManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            layer.Draw(spriteBatch);
            player.Draw(spriteBatch);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
