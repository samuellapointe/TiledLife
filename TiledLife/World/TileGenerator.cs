using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLife.World.Materials;

namespace TiledLife.World
{
    static class TileGenerator
    {
        public const int WATER_LEVEL = 64;

        public static Block[,,] GenerateTile(int tileHeight, int tileWidth, int tileDepth, Tile tile)
        {
            Simplex.Noise.Seed = RandomGen.GetInstance().Next();

            Block[,,] blocks = new Block[tileHeight, tileWidth, tileDepth];
            float[,] depth1 = Simplex.Noise.Calc2D(tileWidth, tileHeight, 0.001f);
            float[,] depth2 = Simplex.Noise.Calc2D(tileWidth, tileHeight, 0.01f);
            float[,] depth3 = Simplex.Noise.Calc2D(tileWidth, tileHeight, 0.05f);
            float[,] depth4 = Simplex.Noise.Calc2D(tileWidth, tileHeight, 0.1f);

            for (int i = 0; i < tileDepth; i++)
            {
                for (int j = 0; j < tileHeight; j++)
                {
                    int blockDepth1 = (int)Math.Round((depth1[i, j] / 512) * Map.TILE_DEPTH);
                    int blockDepth2 = (int)Math.Round((depth2[i, j] / 1024) * Map.TILE_DEPTH);
                    int blockDepth3 = (int)Math.Round((depth3[i, j] / 2048) * Map.TILE_DEPTH);
                    int blockDepth4 = (int)Math.Round((depth4[i, j] / 4096) * Map.TILE_DEPTH);
                    int blockDepth = (blockDepth1 + blockDepth2 + blockDepth3 + blockDepth4) + 20;

                    for (int k = 0; k < tileWidth; k++)
                    {
                        Dictionary<Material.Name, byte> contents = new Dictionary<Material.Name, byte>();
             
                        if (k < blockDepth)
                        {
                            contents.Add(Material.Name.Dirt, Material.FULL);
                        }
                        else if (k < WATER_LEVEL)
                        {
                            contents.Add(Material.Name.Water, Material.FULL);
                        }
                        else
                        {
                            contents.Add(Material.Name.Air, Material.FULL);
                        }

                        blocks[i, j, k] = new Block(
                            new BlockPosition(i, j, k),
                            contents, tile
                        );
                    }
                }
            }

            return blocks;
        }
    }
}
