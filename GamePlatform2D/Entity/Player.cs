﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class Player : Entity
    {
        string text;
        SpriteFont font;

        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            fileManager = new FileManager();
            moveAnimation = new SpriteSheetAnimation();
            Vector2 tempFrames = Vector2.Zero;
            //position = Vector2.Zero;

            text = "";
            if (font == null)
                font = content.Load<SpriteFont>("Font1");

            fileManager.LoadContent("Load/Player.ma", attributes, contents);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Health":
                            health = int.Parse(contents[i][j]);
                            break;

                        case "Frames":
                            string[] frames = contents[i][j].Split(' ');
                            tempFrames = new Vector2(float.Parse(frames[0]), float.Parse(frames[1]));
                            break;

                        case "Image":
                            image = content.Load<Texture2D>(contents[i][j]);
                            break;

                        case "Position":
                            string[] pos = contents[i][j].Split(' ');
                            position = new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));
                            break;
                    }
                }
            }

            moveAnimation.LoadContent(content, image, "", position);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            moveAnimation.IsActive = true;
            if (input.KeyDown(Keys.Right, Keys.D))
            {
                
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                text = "right";
            }
            else if (input.KeyDown(Keys.Left, Keys.A))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                text = "left";
            }
            else
            {
                moveAnimation.IsActive = false;
                text = "";
            }
            moveAnimation.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
            spriteBatch.DrawString(font, text, new Vector2(200, 200), Color.Black);
        }
    }
}
