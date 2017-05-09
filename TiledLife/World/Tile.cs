using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TiledLife.Creature;
using TiledLife.Tools;
using TiledLife.World.Materials;

namespace TiledLife.World
{
    class Tile : GameElement
    {
        // The blocks
        // [0, 0, 0]: north-west corner, at the bottom of the earth
        // [col, row, depth]
        Block[,,] blocks;
        int[,] topmostBlocks;
        UniqueQueue<Block> blockUpdateQueue;
        UniqueQueue<Block> nextBlockUpdateQueue;

        // Position
        int tileX;
        int tileY;

        // Creatures contained in this tile
        List<AbstractCreature> creatures = new List<AbstractCreature>();

        // Depth map
        Texture2D depthMap;
        Color[] colorData;
        bool depthMapNeedsUpdate = false;

        public Tile(int tileX, int tileY)
        {
            this.tileX = tileX;
            this.tileY = tileY;

            blockUpdateQueue = new UniqueQueue<Block>();
            nextBlockUpdateQueue = new UniqueQueue<Block>();
        }

        public Block GetBlockAt(int col, int row, int depth)
        {
            return blocks[col, row, depth];
        }

        public void Initialize()
        {
            blocks = TileGenerator.GenerateTile(Map.TILE_HEIGHT, Map.TILE_WIDTH, Map.TILE_DEPTH, this);

            // Depth map
            colorData = new Color[Map.TILE_HEIGHT * Map.TILE_WIDTH];

            topmostBlocks = new int[Map.TILE_HEIGHT, Map.TILE_WIDTH];
            UpdateAllTopmostBlocks();

            for (int i = 0; i < 100; i++)
            {
                //creatures.Add(new Sheep(new Vector2(100 + i * 10, 500)));
                //creatures.Add(new Human(GetRandomValidPosition()));
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

        public void AddBlockToUpdateQueue(Block block)
        {
            nextBlockUpdateQueue.Enqueue(block);
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

            if (blockUpdateQueue.Count == 0)
            {
                blockUpdateQueue = new UniqueQueue<Block>(nextBlockUpdateQueue);
                nextBlockUpdateQueue.Clear();
            }

            for (int i = 0; i < 64000 && blockUpdateQueue.Count > 0; i++)
            {
                Block block = blockUpdateQueue.Dequeue();
                block.Update(gameTime);
                UpdateTopmostBlock(block.position.Col(), block.position.Row());
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int offsetX = Map.TILE_WIDTH * Map.PIXELS_PER_METER * tileX;
            int offsetY = Map.TILE_HEIGHT * Map.PIXELS_PER_METER * tileY;

            if (depthMap == null)
            {
                depthMap = new Texture2D(spriteBatch.GraphicsDevice, Map.TILE_WIDTH, Map.TILE_HEIGHT);
            }

            for (int row = 0; row < Map.TILE_HEIGHT - 1; row++)
            {
                for (int col = 0; col < Map.TILE_WIDTH - 1; col++)
                {
                    int depth = topmostBlocks[col, row];
                    Block block = blocks[col, row, depth];
                    
                    block.Draw(spriteBatch, offsetX, offsetY);
                }
            }

            if (depthMapNeedsUpdate)
            {
                depthMap.SetData<Color>(colorData);
                depthMapNeedsUpdate = false;
            }

            // Draw depth map
            spriteBatch.Draw(
                depthMap, 
                new Rectangle(
                    0, 0, 
                    Map.TILE_WIDTH*Map.PIXELS_PER_METER, 
                    Map.TILE_HEIGHT*Map.PIXELS_PER_METER
                    ),
                Color.White
            );

            foreach (AbstractCreature creature in creatures)
            {
                creature.Draw(spriteBatch, gameTime);
            }
        }

        // Get a position for spawning, like on a block of dirt
        public Vector2 GetRandomValidPosition()
        {
            int maxNbOfTries = 10;
            //int padding = 2;
            for (int i = 0; i < maxNbOfTries; i++)
            {
                int col = RandomGen.GetInstance().Next(0, Map.TILE_WIDTH);
                int row = RandomGen.GetInstance().Next(0, Map.TILE_HEIGHT);
                /*if (blocks[row,col,0].CanWalkOn())
                {
                    return new Vector2(
                        (col * Map.PIXELS_PER_METER) + padding + (tileX * Map.TILE_WIDTH * Map.PIXELS_PER_METER), 
                        (row * Map.PIXELS_PER_METER) + padding + (tileY * Map.TILE_HEIGHT * Map.PIXELS_PER_METER)
                    );
                }*/
            }

            // Couldn't find a valid spawn location
            return new Vector2(0, 0);
        }

        public AbstractCreature GetCreatureAtPixelPosition(Vector2 position)
        {
            float minDistance = float.MaxValue;
            AbstractCreature closestCreature = null;
            foreach(AbstractCreature c in creatures)
            {
                float distance = Vector2.DistanceSquared(position, c.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCreature = c;
                }
            }

            return closestCreature;
        }

        public void UpdateTopmostBlock(int col, int row)
        {
            for (int i = Map.TILE_DEPTH - 1; i >= 0; i--)
            {
                if (blocks[col, row, i].GetMostCommonVisibleMaterial().phase != Material.Phase.Gas)
                {
                    topmostBlocks[col, row] = i;

                    // Update depth map
                    float darkness = 1 - ((float)i / Map.TILE_DEPTH);
                    colorData[row*Map.TILE_HEIGHT + col] = new Color(0f, 0f, 0f, darkness);
                    depthMapNeedsUpdate = true;

                    return;
                }
            }
        }

        public void UpdateAllTopmostBlocks()
        {
            for (int row = 0; row < Map.TILE_HEIGHT - 1; row++)
            {
                for (int col = 0; col < Map.TILE_WIDTH - 1; col++)
                {
                    UpdateTopmostBlock(col, row);
                }
            }
        }
    }

}
