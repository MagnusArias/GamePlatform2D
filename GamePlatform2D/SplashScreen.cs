using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class SplashScreen : GameScreen
    {
        KeyboardState keyState;
        SpriteFont font;
        String text;
        List<FadeAnimation> fade;
        List<Texture2D> images;
        FileManager fileManager;
        int imageNumber;

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            text = "MagnusArias\npresents";
            if (font == null)
                font = content.Load<SpriteFont>("Font1");

            imageNumber = 0;
            fileManager = new FileManager();
            fade = new List<FadeAnimation>();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.ma", attributes, contents);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int n = 0; n < attributes[i].Count; n++)
                {
                    switch (attributes[i][n])
                    {
                        case "Image":
                            images.Add(content.Load<Texture2D>(contents[i][n]));
                            fade.Add(new FadeAnimation());
                            break;

                        case "Sound":
                            break;
                    }
                }
            }
            for (int i = 0; i < fade.Count; i++)
            {
                fade[i].LoadContent(content, images[i], "", Vector2.Zero);
                fade[i].Scale = 1.0f;
                fade[i].IsActive = true;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            // if (keyState.IsKeyDown(Keys.Z))
            //    ScreenManager.Instance.AddScreen(new TitleScreen());

            fade[imageNumber].Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            fade[imageNumber].Draw(spriteBatch);
            /*
            spriteBatch.DrawString(font, text,
                new Vector2(ScreenManager.Instance.Dimensions.X / 2 - font.MeasureString(text).X / 2,
                ScreenManager.Instance.Dimensions.Y / 2 - font.MeasureString(text).Y / 2), Color.Black);
                */
        }
    }
}
