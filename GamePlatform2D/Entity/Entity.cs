using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class Entity
    {
        #region Variables
        protected int health;
        protected Animation moveAnimation;
        protected SpriteSheetAnimation ssAnimation;
        protected float moveSpeed, gravity;
        protected ContentManager content;
        protected FileManager fileManager;
        protected Texture2D image;
        protected Vector2 position, velocity, prevPosition;
        protected bool activateGravity, syncTilePosition;

        protected List<List<string>> attributes, contents;
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        public virtual void LoadContent(ContentManager content, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        #endregion

        #region Private Methods

        #endregion
    }
}
