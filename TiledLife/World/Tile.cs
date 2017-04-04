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
        // The blocks
        Block[,] blocks;

        // Position
        int tileX;
        int tileY;
        Rectangle bounds;

        // Creatures contained in this tile
        List<AbstractCreature> creatures = new List<AbstractCreature>();

        public Tile(int tileX, int tileY)
        {
            this.tileX = tileX;
            this.tileY = tileY;

            bounds = new Rectangle(tileX, tileY, Map.TILE_WIDTH, Map.TILE_HEIGHT);

        }

        public Block GetBlockAt(int col, int row)
        {
            return blocks[row, col];
        }

        public void Initialize()
        {
            blocks = TileGenerator.GenerateTile(Map.TILE_HEIGHT, Map.TILE_WIDTH);

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

            // Remove dead creatures 
            // iterate backwards to remove them from the array as we loop
            for (int i = creatures.Count - 1; i >= 0; i--)
            {
                if (!creatures[i].alive)
                {
                    creatures.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int offsetX = Map.TILE_WIDTH * tileX;
            int offsetY = Map.TILE_HEIGHT * tileY;
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
                int col = RandomGen.GetInstance().Next(0, Map.TILE_WIDTH);
                int row = RandomGen.GetInstance().Next(0, Map.TILE_HEIGHT);
                if (blocks[row,col].CanWalkOn())
                {
                    return new Vector2((col * Map.PIXELS_PER_METER) + padding, (row * Map.PIXELS_PER_METER) + padding);
                }
            }

            // Couldn't find a valid spawn location
            return new Vector2(0, 0);
        }
    }
}
