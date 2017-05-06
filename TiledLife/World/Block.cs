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

        public BlockPosition position { get; private set; }
        private Texture2D texture;

        /* What a block contains, e.g. air or dirt
         * The int represents the volume in cm³
         * The total of materials in a block should always be 1,000,000 cm³ (1m³)
         * This would amount to 1000 L of water
         */
        private Dictionary<Material.Name, byte> contents;

        private Tile tile;

        public Block (BlockPosition position, Tile tile)
        {
            this.position = position;
            contents = new Dictionary<Material.Name, byte>();
            contents.Add(Material.Name.Air, 255);
            this.tile = tile;
        }

        public Block (BlockPosition position, Dictionary<Material.Name, byte> contents, Tile tile)
        {
            this.position = position;
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

            if (position.Col() < Map.TILE_WIDTH - 1)
            {
                neighbors.Add(tile.GetBlockAt(position.Col() + 1, position.Row(), position.Depth()));
            }
            if (position.Col() > 0)
            {
                neighbors.Add(tile.GetBlockAt(position.Col() - 1, position.Row(), position.Depth()));
            }
            if (position.Row() < Map.TILE_HEIGHT - 1)
            {
                neighbors.Add(tile.GetBlockAt(position.Col(), position.Row() + 1, position.Depth()));
            }
            if (position.Row() > 0)
            {
                neighbors.Add(tile.GetBlockAt(position.Col(), position.Row() - 1, position.Depth()));
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

        // GameElement functions
        public void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY)
        {
            Vector2 offset = new Vector2(offsetX + position.Col() * width, offsetY + position.Row() * height);
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

            int viscosity = 8;
            if (waterVolume > 0 && position.Depth() > 0)
            {
                Block bottomNeighbor = tile.GetBlockAt(position.Col(), position.Row(), position.Depth() - 1);

                byte volumeAirBelow = bottomNeighbor.GetVolume(Material.Name.Air);

                // Air below, let water fall
                if (volumeAirBelow > 0)
                {
                    byte volumeToTransfer = Math.Min(volumeAirBelow, waterVolume);
                    RemoveVolume(Material.Name.Water, volumeToTransfer);
                    bottomNeighbor.AddVolume(Material.Name.Water, volumeToTransfer);
                    if (volumeToTransfer < waterVolume)
                    {
                        tile.AddBlockToUpdateQueue(this);
                    }
                    tile.AddBlockToUpdateQueue(bottomNeighbor);
                }
                else
                // No air below, spread water around
                {
                    List<Block> neighbors = GetNeighbors();
                    List<Block> validNeighbors = new List<Block>();
                    foreach (Block neighbor in neighbors)
                    {
                        if (neighbor.GetVolume(Material.Name.Air) > 0)
                        {
                            validNeighbors.Add(neighbor);
                        }
                    }

                    if (validNeighbors.Count > 0 && waterVolume > viscosity)
                    {
                        int index = RandomGen.GetInstance().Next(0, validNeighbors.Count);
                        Block spreadNeighbor = validNeighbors[index];

                        int neighborWaterVolume = spreadNeighbor.GetVolume(Material.Name.Water);

                        if (Math.Abs(neighborWaterVolume - waterVolume) > viscosity)
                        {
                            int totalWater = waterVolume + spreadNeighbor.GetVolume(Material.Name.Water);
                            //if (totalWater == 1) totalWater = 0;
                            bool uneven = totalWater % 2 == 1;

                            byte spreadVolume = (byte)(totalWater / 2);

                            GoToVolume(Material.Name.Water, spreadVolume);

                            if (uneven) spreadVolume++;
                            spreadNeighbor.GoToVolume(Material.Name.Water, spreadVolume);

                            tile.AddBlockToUpdateQueue(this);
                            tile.AddBlockToUpdateQueue(spreadNeighbor);
                        }
                    }
                   
                }
            }
        }



    }
}
