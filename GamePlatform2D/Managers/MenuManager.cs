using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GamePlatform2D
{
    public class MenuManager
    {
        #region Variables
        List<string> menuItems, animationTypes, linkType, linkID;
        List<Texture2D> menuImages;
        ContentManager content;
        Rectangle sourceRect;
        Vector2 position;
        int axis, itemNumber;
        FileManager fileManager;
        List<List<string>> attributes, contents;
        List<Animation> animation, tempAnimation;
        SpriteFont font;
        string align;
        FadeAnimation fAnimation;
        SpriteSheetAnimation ssAnimation;
        #endregion

        #region Properties

        #endregion

        #region Public Methods
        public void LoadContent(ContentManager content, string id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            menuItems = new List<string>();
            animationTypes = new List<string>();
            menuImages = new List<Texture2D>();
            animation = new List<Animation>();
            fileManager = new FileManager();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            linkType = new List<string>();
            linkID = new List<string>();
            fAnimation = new FadeAnimation();
            ssAnimation = new SpriteSheetAnimation();

            position = Vector2.Zero;
            itemNumber = 0;

            fileManager.LoadContent("Load/Menus.ma", id);
            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int n = 0; n < fileManager.Attributes[i].Count; n++)
                {
                    switch (fileManager.Attributes[i][n])
                    {
                        case "Item":
                            menuItems.Add(contents[i][n]);
                            break;

                        case "Image":
                            menuImages.Add(this.content.Load<Texture2D>(contents[i][n]));
                            break;

                        case "Axis":
                            axis = int.Parse(contents[i][n]);
                            break;

                        case "Position":
                            string[] temp = contents[i][n].Split(' ');
                            position = new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
                            break;

                        case "Source":
                            temp = contents[i][n].Split(' ');
                            sourceRect = new Rectangle(int.Parse(temp[0]),
                                int.Parse(temp[1]),
                                int.Parse(temp[2]),
                                int.Parse(temp[3]));
                            break;

                        case "Font":
                            font = this.content.Load<SpriteFont>(contents[i][n]);
                            break;

                        case "Animation":
                            animationTypes.Add(contents[i][n]);
                            break;

                        case "Align":
                            align = contents[i][n];
                            break;

                        case "LinkType":
                            linkType.Add(contents[i][n]);
                            break;

                        case "LinkID":
                            linkID.Add(contents[i][n]);
                            break;
                    }
                }
            }
            SetMenuItems();
            SetAnimation();
        }

        public void UnloadContent()
        {
            content.Unload();
            fileManager = null;
            position = Vector2.Zero;
            animation.Clear();
            menuImages.Clear();
            menuItems.Clear();
            animationTypes.Clear();
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            if (axis == 1)
            {
                if (inputManager.KeyPressed(Keys.Right, Keys.D)) itemNumber++;
                else if (inputManager.KeyPressed(Keys.Left, Keys.A)) itemNumber--;
            }
            else
            {
                if (inputManager.KeyPressed(Keys.Down, Keys.S)) itemNumber++;
                else if (inputManager.KeyPressed(Keys.Up, Keys.W)) itemNumber--;
            }

            if (inputManager.KeyPressed(Keys.Enter, Keys.Z))
            {
                if (linkType[itemNumber] == "Screen")
                {
                    Type newClass = Type.GetType("GamePlatform2D." + linkID[itemNumber]);
                    ScreenManager.Instance.AddScreen((GameScreen)Activator.CreateInstance(newClass), inputManager);
                }
                if (itemNumber < 0) itemNumber = 0;
                else if (itemNumber > menuItems.Count - 1) itemNumber = menuItems.Count - 1;
            }

            for (int i = 0; i < animation.Count; i++)
            {
                for (int j = 0; j < animationTypes.Count; j++)
                {
                    if (itemNumber == i)
                        animation[i].IsActive = true;
                    else
                        animation[i].IsActive = false;

                    Animation a = animation[i];

                    switch (animationTypes[i])
                    {
                        case "Fade":
                            fAnimation.Update(gameTime, ref a);
                            break;

                        case "SSheet":
                            ssAnimation.Update(gameTime, ref a);
                            break;
                    }

                    animation[i] = a;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animation.Count; i++)
            {
                animation[i].Draw(spriteBatch, SpriteEffects.None);
            }
        }
        #endregion

        #region Private Methods
        private void SetMenuItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuImages.Count == i)
                    menuImages.Add(ScreenManager.Instance.NullImage);
            }

            for (int i = 0; i < menuImages.Count; i++)
            {
                if (menuItems.Count == i)
                    menuItems.Add("");
            }
        }

        private void SetAnimation()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 pos = Vector2.Zero;

            if (align.Contains("Center"))
            {
                for (int i = 0; i < menuItems.Count; i++)
                {
                    dimensions.X += font.MeasureString(menuItems[i]).X + menuImages[i].Width;
                    dimensions.Y += font.MeasureString(menuItems[i]).Y + menuImages[i].Height;
                }

                if (axis == 1) pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
                else if (axis == 2) pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
            }
            else pos = position;

            tempAnimation = new List<Animation>();
            for (int i = 0; i < menuImages.Count; i++)
            {
                dimensions = new Vector2(
                    font.MeasureString(menuItems[i]).X + menuImages[i].Width,
                    font.MeasureString(menuItems[i]).Y + menuImages[i].Height);

                if (axis == 1) pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                else pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;

                animation.Add(new Animation());
                animation[animation.Count - 1].LoadContent(content, menuImages[i], menuItems[i], pos);
                animation[animation.Count - 1].Font = font;

                if (axis == 1) pos.X += dimensions.X;
                else pos.Y += dimensions.Y;
            }
        }
        #endregion
    }
}
