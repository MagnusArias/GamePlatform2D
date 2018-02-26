using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class GameScreen
    {
        #region Variables
        protected ContentManager content;
        protected List<List<string>> attributes, contents;
        protected InputManager inputManager;
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        public virtual void LoadContent(ContentManager Content, InputManager inputManager)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            this.inputManager = inputManager;
        }

        public virtual void UnloadContent()
        {
            content.Unload();
            inputManager = null;
            attributes.Clear();
            contents.Clear();
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        #endregion

        #region Private Methods

        #endregion
    }
}
