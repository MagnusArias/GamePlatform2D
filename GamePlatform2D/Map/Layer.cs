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
        List<List<Tile>> tiles;
        List<List<string>> attributes, contents;
        List<string> motion, solid;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;

        public static Vector2 TileDimensions
        {
            get { return new Vector2(32, 32); }
        }

        public void LoadContent(Map map, string layerID)
        {
            tiles = new List<List<Tile>>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            motion = new List<string>();
            solid = new List<string>();

            fileManager = new FileManager();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            fileManager.LoadContent("Load/Maps/" + map.ID + ".mma", attributes, contents, layerID);

            int indexY = 0;

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "TileSet":
                            tileSheet = content.Load<Texture2D>("TileSets/" + contents[i][j]);
                            break;

                        case "Solid":
                            solid.Add(contents[i][j]);
                            break;

                        case "Motion":
                            motion.Add(contents[i][j]);
                            break;

                        case "StartLayer":
                            List<Tile> tempTiles = new List<Tile>();
                            Tile.Motion tempMotion = Tile.Motion.Static;
                            Tile.State tempState;

                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                string[] split = contents[i][k].Split(',');
                                tempTiles.Add(new Tile());

                                if (solid.Contains(contents[i][k]))
                                    tempState = Tile.State.Solid;
                                else
                                    tempState = Tile.State.Passive;

                                foreach (string m in motion)
                                {
                                    string[] getMotion = m.Split(':');

                                    if (getMotion[0] == contents[i][k])
                                    {
                                        tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                    }
                                }

                                tempTiles[k].SetTile(tempState, tempMotion, new Vector2(k * 32, indexY * 32), tileSheet,
                                    new Rectangle(int.Parse(split[0]) * 32, int.Parse(split[1]) * 32, 32, 32));
                            }
                            tiles.Add(tempTiles);
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
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].Update(gameTime);
                }
            }
        }

        public void UpdateCollision(ref Entity e)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].UpdateCollision(ref e);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].Draw(spriteBatch);
                }
            }
        }
    }
}
