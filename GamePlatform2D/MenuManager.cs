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
    public class MenuManager
    {
        #region Variables
        List<string> menuItems;
        List<string> animationTypes;
        List<Texture2D> menuImages;
        List<List<Animation>> animation;

        ContentManager content;
        Rectangle sourceRect;
        Vector2 position;
        int axis;

        int itemNumber;

        FileManager fileManager;
        List<List<string>> attributes, contents;
        List<Animation> tempAnimation;
        SpriteFont font;
        string align;

        #endregion

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
                if (axis == 1)
                {
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
                }
                else if (axis == 2)
                {
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                }
            }
            else
            {
                pos = position;
            }

            tempAnimation = new List<Animation>();
            for (int i = 0; i < menuImages.Count; i++)
            {
                dimensions = new Vector2(
                    font.MeasureString(menuItems[i]).X + menuImages[i].Width,
                    font.MeasureString(menuItems[i]).Y + menuImages[i].Height);

                if (axis == 1)
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                else
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;

                for (int j = 0; j < animationTypes.Count; j++)
                {
                    switch (animationTypes[j])
                    {
                        case "Fade":
                            tempAnimation.Add(new FadeAnimation());
                            tempAnimation[tempAnimation.Count - 1].LoadContent(content, menuImages[i], menuItems[i], pos);
                            tempAnimation[tempAnimation.Count - 1].Font = font;
                            break;
                    }
                }

                if (tempAnimation.Count > 0)
                    animation.Add(tempAnimation);
                tempAnimation = new List<Animation>();


                if (axis == 1)
                    pos.X += dimensions.X;
                else
                    pos.Y += dimensions.Y;
            }
        }

        public void LoadContent(ContentManager content, string id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            menuItems = new List<string>();
            animationTypes = new List<string>();
            menuImages = new List<Texture2D>();
            animation = new List<List<Animation>>();
            fileManager = new FileManager();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();

            position = Vector2.Zero;
            itemNumber = 0;

            fileManager.LoadContent("Load/Menus.ma", attributes, contents, id);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int n = 0; n < attributes[i].Count; n++)
                {
                    switch (attributes[i][n])
                    {
                        case "Item":
                            menuItems.Add(contents[i][n]);
                            break;

                        case "Image":
                            menuImages.Add(content.Load<Texture2D>(contents[i][n]));
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
                            font = content.Load<SpriteFont>(contents[i][n]);
                            break;

                        case "Animation":
                            animationTypes.Add(contents[i][n]);
                            break;

                        case "Align":
                            align = contents[i][n];
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
                if (inputManager.KeyPressed(Keys.Right, Keys.D))
                {
                    itemNumber++;
                }
                else if (inputManager.KeyPressed(Keys.Left, Keys.A))
                {
                    itemNumber--;
                }
            }
            else
            {
                if (inputManager.KeyPressed(Keys.Down, Keys.S))
                {
                    itemNumber++;
                }
                else if (inputManager.KeyPressed(Keys.Up, Keys.W))
                {
                    itemNumber--;
                }
            }

            if (itemNumber < 0) itemNumber = 0;
            else if (itemNumber > menuItems.Count - 1) itemNumber = menuItems.Count - 1;


            for (int i = 0; i < animation.Count; i++)
            {
                for (int j = 0; j < animation[i].Count; j++)
                {
                    if (itemNumber == i)
                        animation[i][j].IsActive = true;
                    else
                        animation[i][j].IsActive = false;

                    animation[i][j].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animation.Count; i++)
            {
                for (int j = 0; j < animation[i].Count; j++)
                {
                    animation[i][j].Draw(spriteBatch);
                }
            }
        }
    }
}
