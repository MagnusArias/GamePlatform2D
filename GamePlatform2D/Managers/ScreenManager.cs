using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePlatform2D
{
    
    public class ScreenManager
    {
        private static ScreenManager instance;

        #region Variables
        ContentManager content;
        GameScreen currentScreen;
        GameScreen newScreen;
       
      
        Stack<GameScreen> screenStack = new Stack<GameScreen>();
        Vector2 dimensions, scale;
        bool transition;
        Animation animation = new Animation();
        FadeAnimation fade = new FadeAnimation();
        
        Texture2D fadeTexture, nullImage;
        InputManager inputManager;
        #endregion

        #region Properties
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null) instance = new ScreenManager();
                return instance;
            }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Texture2D NullImage
        {
            get { return nullImage; }

        }

        public ContentManager Content
        {
            get { return content; }
        }
        #endregion

        #region Public Methods
        public void AddScreen(GameScreen screen, InputManager inputManager)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 0.0f;
            fade.Increase = true;
            fade.ActivateValue = 1.0f;
            this.inputManager = inputManager;
        }

        public void AddScreen(GameScreen screen, InputManager inputManager, float alpha)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.ActivateValue = 1.0f;
            this.inputManager = inputManager;

            if (alpha != 1.0f) fade.Alpha = 1.0f - alpha;
            else fade.Alpha = alpha;

            fade.Increase = true;
        }

        public void Initialize()
        {
            currentScreen = new GameplayScreen();
            fade = new FadeAnimation();
            inputManager = new InputManager();
        }

        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(content, inputManager);
            fadeTexture = this.content.Load<Texture2D>("sprite");
            animation.LoadContent(content, fadeTexture, "", Vector2.Zero);
            animation.Scale = dimensions.X;
            nullImage = this.content.Load<Texture2D>("null");
        }

        public void Update(GameTime gameTime)
        {
            if (!transition) currentScreen.Update(gameTime);
            else Transition(gameTime);

            Camera.Instance.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (transition) animation.Draw(spriteBatch, SpriteEffects.None);
        }
        #endregion

        #region Private Methods
        private void Transition(GameTime gameTime)
        {
            if (transition)
            {
                fade.Update(gameTime, ref animation);
                if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 1.0f)
                {
                    screenStack.Push(newScreen);
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    currentScreen.LoadContent(content, this.inputManager);
                }
                else if (fade.Alpha == 0.0f)
                {
                    transition = false;
                    fade.IsActive = false;
                }
            }
        }
        #endregion
    }
}
