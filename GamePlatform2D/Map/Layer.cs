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
        List<List<Tile>> tiles;     // tutaj przechowywana jest mapa
        List<string> motion, solid;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;
        string nullTile;
        Vector2 tileDimension, mapDimensions;

        int numCols, numRows; // potrzebne do kolizji

        public Vector2 TileSize
        {
            get { return tileDimension; }
        }
        public Vector2 MapDimensions
        {
            get { return mapDimensions; }
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
            tiles = new List<List<Tile>>();
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

                        case "TileDimensions":
                            string[] tileDim = fileManager.Contents[i][j].Split(',');
                            tileDimension.X = int.Parse(tileDim[0]);
                            tileDimension.Y = int.Parse(tileDim[1]);
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
                            List<Tile> tempTiles = new List<Tile>();

                            //wczytanie jednej linii
                            for (int col = 0; col < fileManager.Contents[i].Count; col++)
                            {
                                // sprawdzanie pojeydnczeog symbolu
                                string[] split = fileManager.Contents[i][col].Split(',');

                                tempTiles.Add(new Tile());

                                if (solid.Contains(fileManager.Contents[i][col]))
                                    tempState = Tile.State.Solid;
                                else
                                    tempState = Tile.State.Empty;

                                // pętla dla ruchomych klocków
                                foreach (string m in motion)
                                {
                                    string[] getMotion = m.Split(':');

                                    if (getMotion[0] == fileManager.Contents[i][col])
                                    {
                                        tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                    }
                                }
                                if (fileManager.Contents[i][col] != nullTile)
                                    tempTiles[col].SetTile(tempState,tempMotion,new Vector2(col * 32, indexY * 32),tileSheet,new Rectangle(int.Parse(split[0]) * 32,int.Parse(split[1]) * 32,32,32));
                                else tempTiles[col].SetTile(tempState, tempMotion, new Vector2(col * 32, indexY * 32), tileSheet, new Rectangle(0 * 32, 0 * 32, 32, 32));

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
            for (int row = 0; row < tiles.Count; row++)
                for (int col = 0; col < tiles[row].Count; col++)
                    tiles[row][col].Update(gameTime);
        }

        public Tile.State GetTileType(Vector2 pos)
        {
            return tiles[(int)pos.X][(int)pos.Y].States;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < tiles.Count; row++)
                for (int col = 0; col < tiles[row].Count; col++)
                    tiles[row][col].Draw(spriteBatch);

        }
    }
}
