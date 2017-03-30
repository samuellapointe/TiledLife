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
        public const int TILE_HEIGHT = 100;
        public const int TILE_WIDTH = 100;
        public const int PIXELS_PER_METER = 8;

        // The blocks
        Block[,] blocks;

        // Position
        int tileX;
        int tileY;
        Rectangle bounds;

        // Some objects
        Random random;

        // Creatures contained in this tile
        List<AbstractCreature> creatures = new List<AbstractCreature>();

        public Tile(int tileX, int tileY)
        {
            this.tileX = tileX;
            this.tileY = tileY;

            bounds = new Rectangle(tileX, tileY, TILE_WIDTH, TILE_HEIGHT);
            
            random = new Random();

        }

        public void Initialize()
        {
            blocks = TileGenerator.GenerateTile(random, TILE_HEIGHT, TILE_WIDTH);

            for (int i = 0; i < 100; i++)
            {
                //creatures.Add(new Sheep(new Vector2(100 + i * 10, 500)));
                creatures.Add(new Human(GetRandomValidPosition()));
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

        // Get a position for spawning, like on a block of dirt
        public Vector2 GetRandomValidPosition()
        {
            int maxNbOfTries = 10;
            int padding = 2;
            for (int i = 0; i < maxNbOfTries; i++)
            {
                int x = random.Next(0, TILE_WIDTH);
                int y = random.Next(0, TILE_HEIGHT);
                if (blocks[y,x].CanWalkOn())
                {
                    return new Vector2((x * PIXELS_PER_METER) + padding, (y * PIXELS_PER_METER) + padding);
                }
            }

            // Couldn't find a valid spawn location
            return new Vector2(0, 0);
        }
    }
}
