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

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            text = "MagnusArias\npresents";
            if (font == null)
                font = content.Load<SpriteFont>("Font1");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Z))
                ScreenManager.Instance.AddScreen(new TitleScreen());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text,
                new Vector2(ScreenManager.Instance.Dimensions.X/2 - font.MeasureString(text).X/2, 
                ScreenManager.Instance.Dimensions.Y / 2 - font.MeasureString(text).Y / 2), Color.Black);
        }
    }
}
