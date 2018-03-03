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
    public class EntityManager
    {
        List<Entity> entities;
        FileManager fileManager;
        InputManager input;


        public List<Entity> Entities { get => entities; set => entities = value; }

        public void LoadContent(string entityType, ContentManager content, string filename, string identifier, InputManager input)
        {
            entities = new List<Entity>();
            fileManager = new FileManager();
            this.input = input;

            if (identifier == String.Empty)
                fileManager.LoadContent(filename);
            else
                fileManager.LoadContent(filename, identifier);

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                Type newClass = Type.GetType("GamePlatform2D." + entityType);
                entities.Add((Entity)Activator.CreateInstance(newClass));
                entities[i].LoadContent(content, fileManager.Attributes[i], fileManager.Contents[i], this.input);
            }
        }

        public void UnloadContent()
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].UnloadContent();
        }

        public void Update(GameTime gameTime, Map map)
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].Update(gameTime, input, map.collision, map.layer);
        }

        public void EntityColision(EntityManager E2)
        {
            foreach (Entity e in entities)
            {
                foreach (Entity e2 in E2.Entities)
                {
                    if (e.Rect.Intersects(e2.Rect))
                    {
                        e.OnCollision(e2);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw(spriteBatch);
        }
    }

}
