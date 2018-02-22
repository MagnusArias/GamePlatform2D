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

        #endregion

        private void SetMenuItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuImages.Count == i)
                    menuImages.Add(null);
            }

            for (int i = 0; i < menuImages.Count; i++)
            {
                if (menuItems.Count == i)
                    menuItems.Add("");
            }
        }

        private void SetAnimation()
        {
            Vector2 pos = position;
            Vector2 dimensions;

            for (int i = 0; i < menuImages.Count; i++)
            {
                for (int j = 0; j < animationTypes.Count; j++)
                {
                    switch (animationTypes[j])
                    {
                        case "Fade":
                            tempAnimation.Add(new FadeAnimation());
                            tempAnimation[tempAnimation.Count - 1].LoadContent(content, menuImages[i], menuItems[i], pos);
                            break;
                    }
                }
                animation.Add(tempAnimation);
                tempAnimation = new List<Animation>();
                dimensions = new Vector2(font.MeasureString(menuItems[i]).X + menuImages[i].Width,
                    font.MeasureString(menuItems[i]).Y + menuImages[i].Height);

                if (axis == 1)
                {
                    pos.X += dimensions.X;
                }
                else
                {
                    pos.Y += dimensions.Y;
                }
            }
        }

        public void LoadContent(ContentManager content, string id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            menuItems = new List<string>();
            animationTypes = new List<string>();
            menuImages = new List<Texture2D>();
            animation = new List<List<Animation>>();

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
                    }
                }
            }



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

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
