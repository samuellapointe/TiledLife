using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledLife.World.Materials;
using System.Diagnostics;
using static TiledLife.World.Materials.Material.Name;

/* A block is a 1 meter x 1 meter tile */
namespace TiledLife.World
{
    class BlockPosition
    {
        private int[] position;
        public BlockPosition(int col, int row, int depth)
        {
            position = new int[] { col, row, depth };
        }
        public int Col() { return position[0]; }
        public int Row() { return position[1]; }
        public int Depth() { return position[2]; }
    }

    class Block
    {
        private int width = Map.PIXELS_PER_METER;
        private int height = Map.PIXELS_PER_METER;

        public BlockPosition worldPosition { get; private set; }
        public BlockPosition localPosition { get; private set; }
        private Texture2D texture;

        /* What a block contains, e.g. air or dirt
         * The int represents the volume in cm³
         * The total of materials in a block should always be 1,000,000 cm³ (1m³)
         * This would amount to 1000 L of water
         */
        private Dictionary<Material.Name, byte> contents;

        private Tile tile;

        public Block (BlockPosition worldPosition, BlockPosition localPosition, Tile tile)
        {
            this.worldPosition = worldPosition;
            this.localPosition = localPosition;
            contents = new Dictionary<Material.Name, byte>();
            contents.Add(Material.Name.Air, 255);
            this.tile = tile;
        }

        public Block (BlockPosition worldPosition, BlockPosition localPosition, Dictionary<Material.Name, byte> contents, Tile tile)
        {
            this.worldPosition = worldPosition;
            this.localPosition = localPosition;
            this.contents = contents;
            this.tile = tile;

            // Blocks with water will need to be updated
            if (GetVolume(Material.Name.Water) > 0)
            {
                tile.AddBlockToUpdateQueue(this);
            }
        }


        private void UpdateTexture(SpriteBatch spriteBatch)
        {
            Material mostCommonMaterial = GetMostCommonVisibleMaterial();
            texture = mostCommonMaterial.GetRandomTexture(spriteBatch);
        }

        public Material GetMostCommonVisibleMaterial()
        {
            Material.Name mostCommon = Material.Name.Air;
            int maxVolume = 0;

            foreach (KeyValuePair<Material.Name, byte> entry in contents)
            {
                if (entry.Key != Material.Name.Air && entry.Value > maxVolume)
                {
                    mostCommon = entry.Key;
                    maxVolume = entry.Value;
                }
            }

            return Material.GetMaterial(mostCommon);
        }

        private List<Block> GetNeighbors()
        {
            List<Block> neighbors = new List<Block>();

            BlockPosition[] positions = new BlockPosition[] {
                new BlockPosition(worldPosition.Col()+1, worldPosition.Row(), worldPosition.Depth()),
                new BlockPosition(worldPosition.Col()-1, worldPosition.Row(), worldPosition.Depth()),
                new BlockPosition(worldPosition.Col(), worldPosition.Row()+1, worldPosition.Depth()),
                new BlockPosition(worldPosition.Col(), worldPosition.Row()-1, worldPosition.Depth())
            };

            foreach (BlockPosition blockPosition in positions)
            {
                Block block = Map.GetInstance().GetBlockAt(blockPosition);
                if (block != null)
                {
                    neighbors.Add(block);
                }
            }

            return neighbors;
        }

        public byte GetVolume(Material.Name name)
        {
            byte volume;
            if (contents.TryGetValue(name, out volume))
            {
                return volume;
            } else {
                return 0;
            }
        }

        private void SetVolume(Material.Name name, byte amount)
        {
            if (contents.ContainsKey(name))
            {
                contents[name] = amount;
            }
            else
            {
                contents.Add(name, amount);
            }
            texture = null; // Force texture update
        }

        public void RemoveVolume(Material.Name name, byte amount)
        {
            SetVolume(name, (byte)(GetVolume(name) - amount));
            SetVolume(Material.Name.Air, (byte)(GetVolume(Material.Name.Air) + amount));
        }

        public void AddVolume(Material.Name name, byte amount)
        {
            SetVolume(name, (byte)(GetVolume(name) + amount));
            SetVolume(Material.Name.Air, (byte)(GetVolume(Material.Name.Air) - amount));
        }

        public void GoToVolume(Material.Name name, byte newVolume)
        {
            byte currentVolume = GetVolume(name);

            if (currentVolume > newVolume)
            {
                RemoveVolume(name, (byte)(currentVolume - newVolume));
            }
            else
            {
                AddVolume(name, (byte)(newVolume - currentVolume));
            }
        }

        public void SwapVolume(Block other)
        {
            Dictionary<Material.Name, byte> tmpContent = new Dictionary<Material.Name, byte>(other.contents); // Clone
            other.contents = new Dictionary<Material.Name, byte>(this.contents); // Clone
            this.contents = tmpContent;
        }

        // GameElement functions
        public void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY)
        {
            Vector2 offset = new Vector2(offsetX + localPosition.Col() * width, offsetY + localPosition.Row() * height);
            if (texture == null)
            {
                UpdateTexture(spriteBatch);
            }

            spriteBatch.Draw(texture, offset);
        }

        public void Update(GameTime gameTime)
        {
            // Handle liquid physics
            byte waterVolume = GetVolume(Material.Name.Water);
            byte viscosity = 0;

            if (waterVolume > 0)
            {
                if (worldPosition.Depth() > 0)
                {
                    Block bottomNeighbor = tile.GetBlockAt(localPosition.Col(), localPosition.Row(), localPosition.Depth() - 1);
                    byte volumeAirBelow = bottomNeighbor.GetVolume(Material.Name.Air);

                    // Tile below is empty, swap
                    if (volumeAirBelow == Material.FULL)
                    {
                        SwapVolume(bottomNeighbor);
                        tile.AddBlockToUpdateQueue(bottomNeighbor);
                        return;
                    }

                    // Tile below has air, transfer some water down
                    if (volumeAirBelow > 0)
                    {
                        // More or same amount of space below
                        if (volumeAirBelow >= waterVolume)
                        {
                            RemoveVolume(Material.Name.Water, waterVolume);
                            bottomNeighbor.AddVolume(Material.Name.Water, waterVolume);
                            tile.AddBlockToUpdateQueue(bottomNeighbor);
                            return;
                        }
                        else
                        {
                            RemoveVolume(Material.Name.Water, volumeAirBelow);
                            bottomNeighbor.AddVolume(Material.Name.Water, volumeAirBelow);
                            tile.AddBlockToUpdateQueue(bottomNeighbor);
                            tile.AddBlockToUpdateQueue(this);
                            // We don't return here, there's still water to spread
                        }
                    }

                    // Update above too
                    if (worldPosition.Depth() < Map.TILE_DEPTH - 1)
                    {
                        Block topNeighbor = tile.GetBlockAt(localPosition.Col(), localPosition.Row(), localPosition.Depth() + 1);
                        tile.AddBlockToUpdateQueue(topNeighbor);
                    }
                }

                // Evaporate
                if (waterVolume == 1)
                {
                    RemoveVolume(Material.Name.Water, 1);
                }

                if (waterVolume < viscosity)
                {
                    return;
                }

                // Now spread to sides
                List<Block> neighbors = GetNeighbors();
                List<Block> spreadBlocks = new List<Block>();
                byte volumeAirHere = GetVolume(Material.Name.Air);
                int totalAir = volumeAirHere;

                // Find neighbors with more free space than here
                foreach (Block neighbor in neighbors)
                {
                    byte volumeAirNeighbor = neighbor.GetVolume(Material.Name.Air);
                    if (volumeAirNeighbor > volumeAirHere)
                    {
                        spreadBlocks.Add(neighbor);
                        totalAir += volumeAirNeighbor;
                    }
                }

                // No free space
                if (spreadBlocks.Count == 0)
                {
                    return;
                }

                // Average the air between the cells
                spreadBlocks.Add(this);
                int rest = totalAir % spreadBlocks.Count;
                byte average = (byte)(totalAir / spreadBlocks.Count);

                foreach (Block block in spreadBlocks)
                {
                    byte volumeAirBlock = block.GetVolume(Material.Name.Air);
                    if (volumeAirBlock > average)
                    {
                        byte difference = (byte)(volumeAirBlock - average);
                        if (difference > viscosity)
                        {
                            if (rest > 0)
                            {
                                rest--;
                                difference -= 1;
                            }

                            block.RemoveVolume(Material.Name.Air, difference);
                            block.AddVolume(Material.Name.Water, difference);
                            tile.AddBlockToUpdateQueue(block);
                        }
                    }
                    else if (volumeAirBlock < average)
                    {
                        byte difference = (byte)(average - volumeAirBlock);
                        if (difference > viscosity)
                        {
                            if (rest > 0)
                            {
                                rest--;
                                difference += 1;
                            }

                            block.RemoveVolume(Material.Name.Water, difference);
                            block.AddVolume(Material.Name.Air, difference);
                            tile.AddBlockToUpdateQueue(block);
                        }
                    }
                }

            }
        }

    }
}
