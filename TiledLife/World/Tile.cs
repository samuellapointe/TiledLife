using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledLife.World.Creature;

namespace TiledLife.World
{
    class Tile : GameElement
    {
        Texture2D texture;
        Vector2 size;
        Vector2 position;
        Random random;

        Sheep sheep;

        int height;
        int width;

        public Tile(Vector2 position)
        {
            this.position = position;

            height = 1000;
            width = 1000;

            size = new Vector2(width, height);
            random = new Random();
        }
        public void Initialize()
        {
            sheep = new Sheep(new Vector2(300, 500));
        }

        public void LoadContent(ContentManager content)
        {
            sheep.LoadContent(content);
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (texture == null)
            {
                texture = new Texture2D(spriteBatch.GraphicsDevice, (int)size.X, (int)size.Y);

                int nbPixels = width * height;
                Color[] colorData = new Color[nbPixels];
                for (int i = 0; i < nbPixels; i++)
                {
                    colorData[i] = new Color(random.Next(60, 80), random.Next(40, 60), 0);
                }
                texture.SetData<Color>(colorData);
            }
            Vector2 offset = new Vector2(size.X * position.X, size.Y * position.Y);
            spriteBatch.Draw(texture, offset);

            sheep.Draw(spriteBatch, gameTime);
        }
    }
}
