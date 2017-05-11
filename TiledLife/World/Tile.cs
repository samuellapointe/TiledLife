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

        UniqueQueue<byte[]> blockUpdateQueue;
        UniqueQueue<byte[]> nextBlockUpdateQueue;

        // Position
        public int tileX { get; private set; }
        public int tileY { get; private set; }

        // Creatures contained in this tile
        List<AbstractCreature> creatures = new List<AbstractCreature>();

        // Depth map
        int[,] depthMap;
        Texture2D depthMapTexture;
        Color[] colorData;

        public Tile(int tileX, int tileY)
        {
            this.tileX = tileX;
            this.tileY = tileY;

            blockUpdateQueue = new UniqueQueue<byte[]>();
            nextBlockUpdateQueue = new UniqueQueue<byte[]>();
        }

        public Block GetBlockAt(byte col, byte row, byte depth)
        {
            return blocks[col, row, depth];
        }

        public void Initialize()
        {
            blocks = TileGenerator.GenerateTile(Map.TILE_HEIGHT, Map.TILE_WIDTH, Map.TILE_DEPTH, this);

            // Depth map
            colorData = new Color[Map.TILE_HEIGHT * Map.TILE_WIDTH];

            depthMap = new int[Map.TILE_HEIGHT, Map.TILE_WIDTH];
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

        public void AddBlockToUpdateQueue(byte col, byte row, byte depth)
        {
            nextBlockUpdateQueue.Enqueue(new byte[] { col, row, depth });
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
                blockUpdateQueue = new UniqueQueue<byte[]>(nextBlockUpdateQueue);
                nextBlockUpdateQueue.Clear();
            }

            for (int i = 0; i < 64000 && blockUpdateQueue.Count > 0; i++)
            {
                byte[] blockPosition = blockUpdateQueue.Dequeue();
                blocks[blockPosition[0],blockPosition[1],blockPosition[2]].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            int offsetX = Map.TILE_WIDTH * Map.BLOCK_WIDTH * tileX;
            int offsetY = Map.TILE_HEIGHT * Map.BLOCK_HEIGHT * tileY;

            if (depthMapTexture == null)
            {
                depthMapTexture = new Texture2D(spriteBatch.GraphicsDevice, Map.TILE_WIDTH, Map.TILE_HEIGHT);
            }

            for (int row = 0; row < Map.TILE_HEIGHT; row++)
            {
                for (int col = 0; col < Map.TILE_WIDTH; col++)
                {
                    int depth = depthMap[col, row];
                    Block block = blocks[col, row, depth];

                    Vector2 offset = new Vector2(
                        (float)offsetX + (col * Map.BLOCK_WIDTH), 
                        (float)offsetY + (row * Map.BLOCK_HEIGHT)
                    );
                    
                    block.Draw(spriteBatch, offset);
                }
            }

            depthMapTexture.SetData<Color>(colorData);


            // Draw depth map
            spriteBatch.Draw(
                depthMapTexture, 
                new Rectangle(
                    offsetX, offsetY, 
                    Map.TILE_WIDTH*Map.BLOCK_WIDTH, 
                    Map.TILE_HEIGHT*Map.BLOCK_HEIGHT
                    ),
                Color.White
            );

            foreach (AbstractCreature creature in creatures)
            {
                creature.Draw(spriteBatch, gameTime);
            }
        }

        public AbstractCreature GetCreatureAtPixelPosition(float x, float y)
        {
            float minDistance = float.MaxValue;
            AbstractCreature closestCreature = null;
            foreach(AbstractCreature c in creatures)
            {
                float distance = Vector2.DistanceSquared(new Vector2(x, y), c.position);
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
                Block block = blocks[col, row, i];
                if (!(block is BlockEmpty))
                {
                    depthMap[col, row] = i;

                    float darkness = 1 - ((float)i / Map.TILE_DEPTH);
                    colorData[row * Map.TILE_HEIGHT + col] = new Color(0f, 0f, 0f, darkness);
                    return;
                }
            }
        }

        public void UpdateAllTopmostBlocks()
        {
            for (int row = 0; row < Map.TILE_HEIGHT; row++)
            {
                for (int col = 0; col < Map.TILE_WIDTH; col++)
                {
                    UpdateTopmostBlock(col, row);
                }
            }
        }

        public void SetBlock(byte col, byte row, byte depth, Block block)
        {
            blocks[col, row, depth] = block;
            UpdateTopmostBlock(col, row);
        }

        /*public bool IsBlockPositionInTile(BlockPosition blockPosition)
        {
            return IsBlockPositionInTile(blockPosition.Col(), blockPosition.Row(), blockPosition.Depth());
        }

        public bool IsBlockPositionInTile(int col, int row, int depth)
        {
            int leftBound = tileX * Map.TILE_WIDTH;
            int rightBound = (tileX + 1) * Map.TILE_WIDTH;
            int topBound = tileY * Map.TILE_HEIGHT;
            int bottomBound = (tileY + 1) * Map.TILE_HEIGHT;
            return (
                col >= leftBound &&
                col < rightBound &&
                row >= topBound &&
                row < bottomBound &&
                depth >= 0 &&
                depth < Map.TILE_DEPTH
            );
        }*/
    }

}
