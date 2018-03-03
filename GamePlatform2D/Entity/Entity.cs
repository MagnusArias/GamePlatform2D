﻿using System.Collections.Generic;
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
        protected float moveSpeed, gravity, jumpSpeed;
        protected ContentManager content;
        protected Texture2D image;
        protected Vector2 position, velocity, prevPosition;
        protected bool activateGravity, syncTilePosition;

        protected List<List<string>> attributes, contents;
        #endregion

        #region Properties
        public Vector2 PrevPosition
        {
            get { return prevPosition; }
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
        #endregion

        #region Public Methods
        public virtual void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i])
                {
                    case "Health":
                        health = int.Parse(contents[i]);
                        break;

                    case "Frames":
                        string[] frames = contents[i].Split(' ');
                        moveAnimation.Frames = new Vector2(float.Parse(frames[0]), float.Parse(frames[1]));
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

                }
            }

            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
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