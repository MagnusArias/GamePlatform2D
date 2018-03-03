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
    public class Collision
    {
        FileManager fileManager;
        List<List<string>> collisionMap;
        List<string> row;
        public List<List<string>> CollisionMap
        {
            get { return collisionMap; }
        }
        public void LoadContent(ContentManager content, string mapID)
        {
            fileManager = new FileManager();
            collisionMap = new List<List<string>>();
            row = new List<string>();

            fileManager.LoadContent("Load/Maps/" + mapID + ".mma", "Collision");
            for (int i= 0; i < fileManager.Contents.Count; i++)
            { 
                for (int j = 0; j < fileManager.Contents[i].Count; j++)
                {
                    row.Add(fileManager.Contents[i][j]);
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
