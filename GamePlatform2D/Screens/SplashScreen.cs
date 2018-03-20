using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class SplashScreen : GameScreen
    {
        #region Variables
        SpriteFont font;
        List<Animation> animation;
        List<Texture2D> images;
        FileManager fileManager;
        int imageNumber;
        FadeAnimation fadeAnimation;
        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (font == null)
                font = this.content.Load<SpriteFont>("Font1");

            imageNumber = 0;
            fileManager = new FileManager();
            animation = new List<Animation>();
            images = new List<Texture2D>();
            fadeAnimation = new FadeAnimation();

            fileManager.LoadContent("Load/Splash.ma", "");
            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int n = 0; n < fileManager.Attributes[i].Count; n++)
                {
                    switch (fileManager.Attributes[i][n])
                    {
                        case "Image":
                            images.Add(this.content.Load<Texture2D>(fileManager.Contents[i][n]));
                            animation.Add(new FadeAnimation());
                            break;

                        case "Sound":
                            break;
                    }
                }
            }
            for (int i = 0; i < animation.Count; i++)
            {
                animation[i].LoadContent(content, images[i], "", new Vector2(120, 40));
                animation[i].Scale = 2.0f;
                animation[i].IsActive = true;
            }

            string[] sAttribute = { "Attribute1" };
            string[] sContent = { "Content1", "Content2" };
            fileManager.SaveContent("Load/Splash.ma", sAttribute, sContent, "");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            Animation a = animation[imageNumber];
            fadeAnimation.Update(gameTime, ref a);
            animation[imageNumber] = a;

            if (animation[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if (imageNumber >= animation.Count - 1 || inputManager.KeyPressed(Keys.Z))
            {
                if (animation[imageNumber].Alpha != 1.0f)
                    ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager, animation[imageNumber].Alpha);
                else
                    ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation[imageNumber].Draw(spriteBatch, SpriteEffects.None);
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
