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
        protected Layer layer;
        protected int health, range, direction;
        protected Animation moveAnimation;
        protected SpriteSheetAnimation ssAnimation;
        protected InputManager inputManager;
        protected float gravity;
        protected ContentManager content;
        protected Texture2D image;
        protected Vector2 position, velocity, prevPosition, origPosition;
        protected bool activateGravity, syncTilePosition, onTile;
        protected int[] numFrames = { 1, 1, 1, 8, 4, 4, 4, 1, 8, 1, 6 };
        protected SpriteFont spriteFont;
        protected bool alreadyDoubleJump;


        //KOLIZJE
        protected int currRow, currCol;
        protected Vector2 dest, temp, collisionBox;
        protected bool topLeft, topRight, bottomLeft, bottomRight; // potrzebne do kolizji

        public struct Speeds
        {
            public float move;
            public float max;
            public float stop;
            public float fall;
            public float maxFall;
            public float jump;
            public float stopJump;

        }
        public struct States
        {
            public bool left;
            public bool right;
            public bool up;
            public bool squat;
            public bool jumping;
            public bool falling;

            public bool standing;
            public bool highAttack;
            public bool normalAttack;
            public bool lowAttack;
            public bool doubleJump;
            public bool teleporting;
            public bool dashing;
            public bool knockback;
            public bool flinching;

            public void SetFalling(bool b)
            {
                falling = b;
            }
        }

        protected States state;
        protected Speeds speed;

        protected List<List<string>> attributes, contents;
        #endregion

        #region Properties
        public bool ActivateGravity
        {
            set { activateGravity = value; }
            get { return activateGravity; }
        }

        public Animation Animation
        {
            get { return moveAnimation; }
        }

        public int Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                //destPosition.X = (direction == 2) ? destPosition.X = origPosition.X - range : destPosition.X = origPosition.X + range;
            }
        }

        public Speeds GetSpeeds
        {
            get { return speed; }
        }

        public States GetStates
        {
            get { return state; }
            set { state = value; }
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

        public Vector2 PrevPosition
        {
            get { return prevPosition; }
        }
        
        public FloatRect Rect
        {
            get { return new FloatRect(position.X, position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight); }
        }
        
        public bool SyncTilePosition
        {
            get { return syncTilePosition; }
            set { syncTilePosition = value; }
        }
        
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
               
        #endregion

        #region Public Methods
        public virtual void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input, Layer lyr)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
            spriteFont = this.content.Load<SpriteFont>("Font1");
            inputManager = input;
            collisionBox = Vector2.Zero;

            layer = new Layer();

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
                        speed.move = float.Parse(contents[i]);
                        break;

                    case "Range":
                        range = int.Parse(contents[i]);
                        break;
                }
            }

            gravity = 10.0f;
            velocity = Vector2.Zero;
            temp = Vector2.Zero;
            syncTilePosition = false;
            activateGravity = true;
            layer = lyr;
            moveAnimation.LoadContent(content, image, "", position);
        }

        

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void OnCollision(Entity e)
        {

        }

        public void CalculateCorners(Vector2 pos)
        {
            int leftTile = (int)((pos.X - collisionBox.X / 2) / layer.TileSize.X);
            int rightTile = (int)((pos.X + collisionBox.X / 2 - 1) / layer.TileSize.X);
            int topTile = (int)((pos.Y - collisionBox.Y / 2) / layer.TileSize.Y);
            int bottomTile = (int)((pos.Y + collisionBox.Y / 2 - 1) / layer.TileSize.Y);

            if (topTile < 0 || bottomTile >= layer.NumRows || 
                leftTile < 0 || rightTile >= layer.NumCols)
            {
                topLeft = topRight = bottomLeft = bottomRight = false;
                return;
            }

            Tile.State tl = layer.GetTileType(new Vector2(topTile, leftTile));
            Tile.State tr = layer.GetTileType(new Vector2(topTile, rightTile));
            Tile.State bl = layer.GetTileType(new Vector2(bottomTile, leftTile));
            Tile.State br = layer.GetTileType(new Vector2(bottomTile, rightTile));
            topLeft = tl == Tile.State.Solid;
            topRight = tr == Tile.State.Solid;
            bottomLeft = bl == Tile.State.Solid;
            bottomRight = br == Tile.State.Solid;
        }

        public virtual void CheckMapCollision()
        {
            currCol = (int)(position.X / layer.TileSize.X);
            currRow = (int)(position.Y / layer.TileSize.Y);

            dest = position + velocity;
            temp = position;

            CalculateCorners(new Vector2(position.X, dest.Y));

            if (velocity.Y < 0)
            {
                if (topLeft || topRight)
                {
                    velocity.Y = 0;
                    temp.Y = currRow * layer.TileSize.Y + collisionBox.Y / 2;
                }
                else temp.Y += velocity.Y;
            }
            if (velocity.Y > 0)
            {
                if (bottomLeft || bottomRight)
                {
                    velocity.Y = 0;
                    state.falling = false;
                    temp.Y = (currRow + 1) * layer.TileSize.Y - collisionBox.Y / 2;
                }
                else temp.Y += velocity.Y;
            }

            CalculateCorners(new Vector2(dest.X, position.Y));

            if (velocity.X < 0)
            {
                if (topLeft || bottomLeft)
                {
                    velocity.X = 0;
                    temp.X = currCol * layer.TileSize.X + collisionBox.X / 2;
                }
                else temp.X += velocity.X;
            }
            if (velocity.X > 0)
            {
                if (topRight || bottomRight)
                {
                    velocity.X = 0;
                    temp.X = (currCol + 1) * layer.TileSize.X - collisionBox.X / 2;
                }
                else temp.X += velocity.X;
            }

            if (!state.falling)
            {
                CalculateCorners(new Vector2(position.X, dest.Y + 1));
                if (!bottomLeft && !bottomRight)
                {
                    state.falling = true;
                }
            }

        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
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


        public void SetJumping(Keys k)
        {
            if (state.knockback) return;

            if (inputManager.KeyDown(k) && !state.jumping && !alreadyDoubleJump)
            {
                state.doubleJump = true;
            }
            state.jumping = inputManager.KeyDown(k);
        }

        public void SetUp(Keys k)
        {
            state.jumping = inputManager.KeyDown(k);
        }
        public void SetLeft(Keys k)
        {
            state.left = inputManager.KeyDown(k);
        }
        public void SetRight(Keys k)
        {
            state.right = inputManager.KeyDown(k);
        }
        public void SetDown(Keys k)
        {
            state.squat = inputManager.KeyDown(k);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
