using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledLife.Creature;

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
        int pixelsPerMeter;
        int imageWidth;
        int imageHeight;

        bool viewGrid;

        public Tile(Vector2 position)
        {
            this.position = position;

            height = 100;
            width = 100;
            pixelsPerMeter = 10;
            imageWidth = width * pixelsPerMeter;
            imageHeight = height * pixelsPerMeter;

            viewGrid = false;

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
            sheep.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (texture == null)
            {
                texture = new Texture2D(spriteBatch.GraphicsDevice, imageWidth, imageHeight);
                int nbPixels = imageWidth * imageHeight;
                Color[] colorData = new Color[nbPixels];

                for (int i = 0; i < nbPixels; i++)
                {
                    if (viewGrid && (i % pixelsPerMeter == 0 || Math.Floor((float)i/(float)imageWidth) % pixelsPerMeter == 0))
                    {
                        colorData[i] = new Color(40, 40, 40);
                    } else
                    {
                        colorData[i] = new Color(random.Next(50, 60), random.Next(30, 40), 0);
                    }
                }
                texture.SetData<Color>(colorData);
            }
            Vector2 offset = new Vector2(size.X * position.X, size.Y * position.Y);
            spriteBatch.Draw(texture, offset);

            sheep.Draw(spriteBatch, gameTime);
        }
    }
}
