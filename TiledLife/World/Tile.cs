using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledLife.Creature;
using TiledLife.World.Materials.Liquid;
using TiledLife.World.Materials.Solid;

namespace TiledLife.World
{
    class Tile : GameElement
    {
        // These should be the same for every tile on the map
        public const int TILE_HEIGHT = 100;
        public const int TILE_WIDTH = 100;
        public const int PIXELS_PER_METER = 10;

        // The blocks
        Block[,] blocks;

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

            imageWidth = TILE_WIDTH * PIXELS_PER_METER;
            imageHeight = TILE_HEIGHT * PIXELS_PER_METER;

            bounds = new Rectangle(tileX, tileY, TILE_WIDTH, TILE_HEIGHT);
            
            random = new Random();

            // Create blocks
            blocks = new Block[TILE_HEIGHT, TILE_WIDTH];

        }

        public void Initialize()
        {
            for (int i = 0; i < 10; i++)
            {
                creatures.Add(new Sheep(new Vector2(100 + i * 10, 500)));
            }

            for (int i = 0; i < TILE_HEIGHT; i++)
            {
                for (int j = 0; j < TILE_WIDTH; j++)
                {
                    int randInt = random.Next(10);
                    if (random.Next(10) == 0)
                    {
                        blocks[i, j] = new Block(
                                random,
                                MaterialLiquid.CreateMaterial(MaterialLiquid.MaterialLiquidNames.water),
                                j, i
                            );
                    } else {
                        blocks[i, j] = new Block(
                                random,
                                MaterialSolid.CreateMaterial(MaterialSolid.MaterialSolidNames.dirt),
                                j, i
                            );
                    }
                }
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
            /*if (texture == null)
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
            Vector2 offset = new Vector2(TILE_WIDTH * tileX, TILE_HEIGHT * tileY);
            spriteBatch.Draw(texture, offset);*/

            int offsetX = TILE_WIDTH * tileX;
            int offsetY = TILE_HEIGHT * tileY;
            foreach (Block block in blocks)
            {
                block.Draw(spriteBatch, offsetX, offsetY);
            }

            foreach (AbstractCreature creature in creatures)
            {
                creature.Draw(spriteBatch, gameTime);
            }
        }
    }
}
