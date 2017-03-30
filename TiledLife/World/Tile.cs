using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledLife.Creature;

namespace TiledLife.World
{
    class Tile : GameElement
    {
        // These should be the same for every tile on the map
        const int TileHeight = 100;
        const int TileWidth = 100;
        const int PixelsPerMeter = 10;

        // Position
        int tileX;
        int tileY;
        Rectangle bounds;

        // Some objects
        Texture2D texture;
        Random random;

        // The size for the image
        int imageWidth;
        int imageHeight;

        // Creatures contained in this tile
        List<AbstractCreature> creatures = new List<AbstractCreature>();

        public Tile(int tileX, int tileY)
        {
            this.tileX = tileX;
            this.tileY = tileY;

            imageWidth = TileWidth * PixelsPerMeter;
            imageHeight = TileHeight * PixelsPerMeter;

            bounds = new Rectangle(tileX, tileY, TileWidth, TileHeight);
            
            random = new Random();
        }

        public void Initialize()
        {
            for (int i = 0; i < 10; i++)
            {
                creatures.Add(new Sheep(new Vector2(100 + i * 10, 500)));
            }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (AbstractCreature creature in creatures)
            {
                creature.LoadContent(content);
            }
        }

        public void UnloadContent()
        {
            foreach (AbstractCreature creature in creatures)
            {
                creature.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (AbstractCreature creature in creatures)
            {
                creature.Update(gameTime);
            }
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
                    colorData[i] = new Color(random.Next(50, 60), random.Next(30, 40), 0);
                }
                texture.SetData<Color>(colorData);
            }
            Vector2 offset = new Vector2(TileWidth * tileX, TileHeight * tileY);
            spriteBatch.Draw(texture, offset);

            foreach (AbstractCreature creature in creatures)
            {
                creature.Draw(spriteBatch, gameTime);
            }
        }
    }
}
