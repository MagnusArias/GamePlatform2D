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
    public class Layer
    {
        List<Tile> tiles;
        List<string> motion, solid;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;
        string nullTile;

        int numCols, numRows; // potrzebne do kolizji

        public static Vector2 TileDimensions
        {
            get { return new Vector2(32, 32); }
        }

        public int NumCols
        {
            get { return numCols; }
        }

        public int NumRows
        {
            get { return numRows; }
        }


        public void LoadContent(Map map, string layerID)
        {
            tiles = new List<Tile>();
            motion = new List<string>();
            solid = new List<string>();

            fileManager = new FileManager();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            fileManager.LoadContent("Load/Maps/" + map.ID + ".mma", layerID);

            int indexY = 0;

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    {
                        case "TileSet":
                            tileSheet = content.Load<Texture2D>("TileSets/" + fileManager.Contents[i][j]);
                            break;

                        case "Solid":
                            solid.Add(fileManager.Contents[i][j]);
                            break;

                        case "NullTile":
                            nullTile = fileManager.Contents[i][j];
                            break;

                        case "MapSize":
                            string[] mapSize = fileManager.Contents[i][j].Split(',');
                            numRows = int.Parse(mapSize[1]);
                            numCols = int.Parse(mapSize[0]);
                            break;

                        case "Motion":
                            motion.Add(fileManager.Contents[i][j]);
                            break;

                        case "StartLayer":

                            Tile.Motion tempMotion = Tile.Motion.Static;
                            Tile.State tempState;

                            for (int k = 0; k < fileManager.Contents[i].Count; k++)
                            {
                                if (fileManager.Contents[i][k] != nullTile)
                                {
                                    string[] split = fileManager.Contents[i][k].Split(',');
                                    tiles.Add(new Tile());

                                    if (solid.Contains(fileManager.Contents[i][k]))
                                        tempState = Tile.State.Solid;
                                    else
                                        tempState = Tile.State.Passive;

                                    foreach (string m in motion)
                                    {
                                        string[] getMotion = m.Split(':');

                                        if (getMotion[0] == fileManager.Contents[i][k])
                                        {
                                            tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                        }
                                    }

                                    tiles[tiles.Count - 1].SetTile(tempState, tempMotion, new Vector2(k * 32, indexY * 32), tileSheet,
                                        new Rectangle(int.Parse(split[0]) * 32, int.Parse(split[1]) * 32, 32, 32));
                                }
                            }
                            indexY++;
                            break;

                        case "EndLayer":

                            break;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Update(gameTime);
            }
        }

        public Tile.State GetTileType(Vector2 pos)
        {
            foreach (Tile t in tiles)
            {
                if (t.Position == pos)
                    return t.GetState;
            }
            return Tile.State.Passive;
        }

        public void UpdateCollision(ref Entity e)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].UpdateCollision(ref e);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch);
            }
        }
    }
}
