using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public abstract class Entity
    {
        #region Variables
        protected int health, range, direction;
        protected Animation moveAnimation;
        protected SpriteSheetAnimation ssAnimation;
        protected float moveSpeed, gravity, jumpSpeed;
        protected ContentManager content;
        protected Texture2D image;
        protected Vector2 position, velocity, prevPosition, origPosition, destPosition;
        protected bool activateGravity, syncTilePosition, onTile;

        protected List<List<string>> attributes, contents;
        #endregion

        #region Properties
        public Vector2 PrevPosition
        {
            get { return prevPosition; }
        }

        public int Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                destPosition.X = (direction == 2) ? destPosition.X = origPosition.X - range : destPosition.X = origPosition.X + range;
            }
        }
        public bool OnTile
        {
            get { return onTile; }
            set { onTile = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool ActivateGravity
        {
            set { activateGravity = value; }
            get { return activateGravity; }
        }

        public bool SyncTilePosition
        {
            get { return syncTilePosition; }
            set { syncTilePosition = value; }
        }
        public Animation Animation
        {
            get { return moveAnimation; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public FloatRect Rect
        {
            get { return new FloatRect(position.X, position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight); }
        }
        #endregion

        #region Public Methods
        public virtual void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i])
                {
                    case "Health":
                        health = int.Parse(contents[i]);
                        break;

                    case "Frames":
                        string[] frames = contents[i].Split(' ');
                        moveAnimation.Frames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                        break;

                    case "Image":
                        image = this.content.Load<Texture2D>(contents[i]);
                        break;

                    case "Position":
                        string[] pos = contents[i].Split(' ');
                        position = new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));
                        break;

                    case "MoveSpeed":
                        moveSpeed = float.Parse(contents[i]);
                        break;

                    case "Range":
                        range = int.Parse(contents[i]);
                        break;
                }
            }

            jumpSpeed = 250.0f;
            gravity = 10.0f;
            velocity = Vector2.Zero;
            syncTilePosition = false;
            activateGravity = true;

            moveAnimation.LoadContent(content, image, "", position);
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void OnCollision(Entity e)
        {

        }

        public virtual void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            syncTilePosition = false;
            prevPosition = position;
            moveAnimation.IsActive = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        #endregion

        #region Private Methods

        #endregion
    }
}
