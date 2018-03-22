using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GamePlatform2D
{
    public class Map
    {
        public Layer layer;
        public Collision collision;
        string id;

        public string ID { get => id; }
    

    public void LoadContent(ContentManager content, Map map, string mapID)
    {
        layer = new Layer();
        collision = new Collision();
        id = mapID;

        layer.LoadContent(map, "Layer1");
        collision.LoadContent(content, mapID);
    }

    public void UnloadContent()
    {

    }

    public void Update(GameTime gameTime)
    {
        layer.Update(gameTime);
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        layer.Draw(spriteBatch);
    }
}
}
