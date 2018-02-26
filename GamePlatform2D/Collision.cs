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
    public class Collision : Map
    {
        FileManager fileManager;
        List<List<string>> attributes, contents, collisionMap;
        List<string> row;
        public List<List<string>> CollisionMap
        {
            get { return collisionMap; }
        }
        public void LoadContent(ContentManager content, string mapID)
        {
            fileManager = new FileManager();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            collisionMap = new List<List<string>>();
            row = new List<string>();

            fileManager.LoadContent("Load/Maps/" + mapID + ".mma", attributes, contents, "Collision");
            for (int i= 0; i < contents.Count; i++)
            { 
                for (int j = 0; j < contents[i].Count; j++)
                {
                    row.Add(contents[i][j]);
                }
                collisionMap.Add(row);
                row = new List<string>();
            }
        }

        public void Update(GameTime gameTime, ref Vector2 playerPosition, Vector2 playerDimensions, Vector2 tileDimensions)
        { 
            for (int i = 0; i < collisionMap.Count; i++)
            {
                for (int j = 0; j < collisionMap[i].Count; j++)
                {
                    if (    playerPosition.X    +   playerDimensions.X  <   j   *   tileDimensions.X    ||
                            playerPosition.X                            >   j   *   tileDimensions.X    +   tileDimensions.X    ||
                            playerPosition.Y    +   playerDimensions.Y  <   i   *   tileDimensions.Y    ||
                            playerPosition.Y                            >   i   *   tileDimensions.Y    +   tileDimensions.Y)
                    {
                        // no collision
                    }
                    else
                    {

                    }
                }
            }
        }
    }
}
