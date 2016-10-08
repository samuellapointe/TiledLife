using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TiledLife.World
{
    class Tile : GameElement
    {
        Texture2D texture;
        Vector2 size;
        Vector2 position;
        Random random;

        public Tile(Vector2 position)
        {
            this.position = position;
            size = new Vector2(1000, 1000);
            random = new Random();
        }
        public void Initialize()
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

                Color[] colorData = new Color[(int)size.X * (int)size.Y];
                for (int i = 0; i < colorData.GetLength(0); i++)
                {
                    Color pixelColor = new Color(random.Next(0, 50), random.Next(100, 120), 0);
                    colorData[i] = pixelColor;
                }
                texture.SetData<Color>(colorData);
            }
            spriteBatch.Draw(texture, position);
        }
    }
}
