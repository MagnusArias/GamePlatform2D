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
        List<List<string>> attributes, contents;
        FileManager fileManager;
        InputManager input;


        public List<Entity> Entities { get => entities; set => entities = value; }

        public void LoadContent(string entityType, ContentManager content, string filename, string identifier, InputManager input)
        {
            entities = new List<Entity>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            fileManager = new FileManager();
            this.input = input;

            if (identifier == String.Empty)
                fileManager.LoadContent(filename, attributes, contents);
            else
                fileManager.LoadContent(filename, attributes, contents, identifier);

            for (int i = 0; i < attributes.Count; i++)
            {
                Type newClass = Type.GetType("GamePlatform2D." + entityType);
                entities.Add((Entity)Activator.CreateInstance(newClass));
                entities[i].LoadContent(content, attributes[i], contents[i], this.input);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw(spriteBatch);
        }
    }

}
