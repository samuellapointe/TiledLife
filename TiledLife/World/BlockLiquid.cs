using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledLife.World.Materials;

namespace TiledLife.World
{
    // A solid block is full of a single material
    class BlockLiquid : Block
    {
        public Material material;

        private byte col, row, depth; // Local coords
        private int worldCol, worldRow;
        Tile tile;

        public byte quantity;

        public BlockLiquid(Material material, int worldCol, int worldRow, byte depth, byte quantity = byte.MaxValue)
        {
            this.material = material;

            this.worldCol = worldCol;
            this.worldRow = worldRow;

            this.quantity = quantity;

            tile = Map.GetInstance().GetTileFromBlockPosition(worldCol, worldRow);

            col = (byte)(worldCol - (tile.tileX * Map.TILE_WIDTH));
            row = (byte)(worldRow - (tile.tileY * Map.TILE_HEIGHT));
            this.depth = depth;

            AddToUpdateQueue();
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(material.GetTexture(spriteBatch), offset);
        }

        public override void Update(GameTime gameTime)
        {
            // When there is no liquid left, there's no point in keeping the block around
            if (quantity == 0)
            {
                tile.SetBlock(col, row, depth, new BlockEmpty());
                return;
            }

            // Check for an valid neighbor below
            if (depth > 0)
            {
                // Empty neighbor below, we can just move down directly
                Block neighborBelow = Map.GetInstance().GetBlockAt(worldCol, worldRow, depth - 1);
                if (neighborBelow is BlockEmpty)
                {
                    tile.SetBlock(col, row, depth, new BlockEmpty());

                    BlockLiquid newBlockBelow = new BlockLiquid(material, worldCol, worldRow, (byte)(depth - 1), quantity);
                    newBlockBelow.SetSelfInTile();

                    quantity = 0;
                    return;     
                }

                // Liquid below, transfer what we can
                if (neighborBelow is BlockLiquid)
                {
                    BlockLiquid liquidNeighborBelow = (BlockLiquid)neighborBelow;
                    byte freeSpaceBelow = (byte)(byte.MaxValue - liquidNeighborBelow.quantity);

                    if (freeSpaceBelow > 0)
                    {
                        if (quantity <= freeSpaceBelow )
                        {
                            tile.SetBlock(col, row, depth, new BlockEmpty());
                            liquidNeighborBelow.quantity += quantity;
                        }
                        else
                        {
                            quantity -= freeSpaceBelow;
                            AddToUpdateQueue();
                            liquidNeighborBelow.quantity += freeSpaceBelow;
                        }

                        liquidNeighborBelow.AddToUpdateQueue();
                    }
                }

                
            }

            if (quantity == 1)
            {
                quantity = 0;
                tile.SetBlock(col, row, depth, new BlockEmpty());
            }

            /*if (quantity < 3)
            {
                return;
            }*/

            // If we reach this point, it means there is still water in this block
            // Try to spread to sides
            List<BlockLiquid> spreadBlocks = GetAvailableNeighbors();

            if (spreadBlocks.Count == 0)
            {
                return;
            }

            spreadBlocks.Add(this);

            int totalQuantity = 0;
            foreach (BlockLiquid block in spreadBlocks)
            {
                totalQuantity += block.quantity;
            }

            byte averageQuantity = (byte)(totalQuantity / spreadBlocks.Count);
            int rest = totalQuantity % spreadBlocks.Count;

            foreach (BlockLiquid block in spreadBlocks)
            {
                if (Math.Abs(quantity - averageQuantity) <= 1) return;

                block.quantity = averageQuantity;
                if (rest > 0 && block.quantity < byte.MaxValue)
                {
                    rest--;
                    block.quantity++;
                }
                block.AddToUpdateQueue();
            }
        }

        public void AddToUpdateQueue()
        {
            tile.AddBlockToUpdateQueue(col, row, depth);
        }

        public void SetSelfInTile()
        {
            tile.SetBlock(col, row, depth, this);
        }

        private List<BlockLiquid> GetAvailableNeighbors()
        {
            List<BlockLiquid> neighbors = new List<BlockLiquid>();
            int[,] positionsToCheck =
            {
                { worldCol + 1, worldRow, depth },
                { worldCol - 1, worldRow, depth },
                { worldCol, worldRow + 1, depth },
                { worldCol, worldRow - 1, depth },
            };

            for (int i = 0; i < positionsToCheck.GetLength(0); i++)
            {
                // First, get the block
                Block neighbor = Map.GetInstance().GetBlockAt(
                    positionsToCheck[i, 0],
                    positionsToCheck[i, 1],
                    positionsToCheck[i, 2]
                );

                // We'll need this later
                BlockLiquid neighborLiquid;

                // If there's an empty block, put an empty liquid block there
                BlockEmpty neighborEmpty = neighbor as BlockEmpty;
                if (neighborEmpty != null)
                {
                    neighborLiquid = new BlockLiquid(
                        material,
                        positionsToCheck[i, 0],
                        positionsToCheck[i, 1],
                        (byte)positionsToCheck[i, 2],
                        0);

                    neighborLiquid.SetSelfInTile();

                    neighbors.Add(neighborLiquid);
                }
                else if (neighbor is BlockLiquid)
                {
                    //TODO: DO
                    /*neighborLiquid = (BlockLiquid)neighbor;
                    neighbors.Add(neighborLiquid);*/
                }
            }

            return neighbors;
        }
    }
}
