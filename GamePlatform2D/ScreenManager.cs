using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePlatform2D
{
    public class ScreenManager
    {

        #region Variables
        /// <summary>
        /// Custom content manager
        /// </summary>
        ContentManager content;

        GameScreen currentScreen;
        GameScreen newScreen;

        /// <summary>
        /// ScreenManager Instance
        /// </summary>
        private static ScreenManager instance;

        /// <summary>
        ///  Storing the GameScreens
        /// </summary>
       // Dictionary<string, GameScreen> screens = new Dictionary<string, GameScreen>();

        /// <summary>
        /// Screen stack - kolejność
        /// </summary>
        Stack<GameScreen> screenStack = new Stack<GameScreen>();

        /// <summary>
        /// Screen width and height
        /// </summary>
        Vector2 dimensions;
        bool transition;

        FadeAnimation fade;
        Texture2D fadeTexture;
        InputManager inputManager;
        #endregion

        #region Properties
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }

        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        #endregion

        #region Main Methods

        public void AddScreen(GameScreen screen, InputManager inputManager)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 0.0f;
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

            if (alpha != 1.0f)
                fade.Alpha = 1.0f - alpha;
            else fade.Alpha = alpha;

            fade.Increase = true;
        }

        public void Initialize()
        {
            currentScreen = new SplashScreen();
            fade = new FadeAnimation();
            inputManager = new InputManager();
        }

        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(content, inputManager);
            fadeTexture = content.Load<Texture2D>("sprite");
            fade.LoadContent(content, fadeTexture, "", Vector2.Zero);
            fade.Scale = dimensions.X;
        }

        public void Update(GameTime gameTime)
        {
            if (!transition)
                currentScreen.Update(gameTime);
            else
                Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (transition)
                fade.Draw(spriteBatch);
        }

        #endregion


        #region PrivateMethods
        private void Transition(GameTime gameTime)
        {
            if (transition)
            {
                fade.Update(gameTime);
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
