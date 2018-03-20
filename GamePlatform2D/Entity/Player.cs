using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GamePlatform2D
{
    public class Player : Entity
    {
        FileManager fileManager;
        List<Bullet> bullets;        
        Texture2D bulletImage;

        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            base.LoadContent(content, attributes, contents, input);
            string[] saveAttribute = { "PlayerPosition" };
            string[] saveContent = { position.X.ToString() + ',' + position.Y.ToString() };

            fileManager = new FileManager();
            fileManager.SaveContent("Load/Maps/Map1.mma", saveAttribute, saveContent, "");

            bullets = new List<Bullet>();
            bulletImage = content.Load<Texture2D>("bullet");

            jumpSpeed = 300.0f;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            base.Update(gameTime, input, col, layer);

            moveAnimation.DrawColor = Color.White;

            if (input.KeyDown(Keys.Right, Keys.D))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                velocity.X = moveSpeed* (float)gameTime.ElapsedGameTime.TotalSeconds;
                direction = 1;
            }
            else if (input.KeyDown(Keys.Left, Keys.A))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                velocity.X = -moveSpeed* (float)gameTime.ElapsedGameTime.TotalSeconds;
                direction = -1;
            }
            else
            {
                moveAnimation.IsActive = false;
                velocity.X = 0;
            }

            if (input.KeyDown(Keys.Up, Keys.W) && !activateGravity)
            {
                velocity.Y = -jumpSpeed* (float)gameTime.ElapsedGameTime.TotalSeconds;
                activateGravity = true;
            }

            if (activateGravity)
                velocity.Y += gravity* (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                velocity.Y = 0;

            if (input.KeyDown(Keys.Z))
            {
                bullets.Add(new Bullet(new Vector2(position.X, position.Y), direction, 400.0f, bulletImage));
            }

            foreach (Bullet b in bullets) b.Update(gameTime);

            position += velocity;

            moveAnimation.Position = position;
            ssAnimation.Update(gameTime, ref moveAnimation);

            Camera.Instance.SetFocalPoint(new Vector2(position.X, position.Y));
        }

        public override void OnCollision(Entity e)
        {
            Type type = e.GetType();
            if (type == typeof(Enemy))
            {
                health--;
                moveAnimation.DrawColor = Color.Red;

                fileManager = new FileManager();
                fileManager.LoadContent("Load/Maps/Map1.mma", "");
                for (int i = 0; i < fileManager.Attributes.Count; i++)
                {
                    for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                    {
                        switch (fileManager.Attributes[i][j])
                        {
                            case "PlayerPosition":
                                string[] split = fileManager.Contents[i][j].Split(',');
                                position = new Vector2(int.Parse(split[0]), int.Parse(split[1]));
                                break;
                        }
                    }
                }
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
            
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }
            
        }
    }
}
