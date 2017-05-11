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

        private static Material materialDirt;

        private static bool initialized = false;

        private static void InitializeMaterials()
        {
            materialDirt = MaterialManager.GetInstance().GetMaterial();
            initialized = true;
        }

        public static Block[,,] GenerateTile(int tileHeight, int tileWidth, int tileDepth, Tile tile)
        {
            if (!initialized)
            {
                InitializeMaterials();
            }

            Block[,,] blocks = new Block[tileHeight, tileWidth, tileDepth];
            float[,] depth1 = Simplex.Noise.Calc2D(tileWidth, tileHeight, 0.001f);
            float[,] depth2 = Simplex.Noise.Calc2D(tileWidth, tileHeight, 0.01f);
            float[,] depth3 = Simplex.Noise.Calc2D(tileWidth, tileHeight, 0.05f);
            float[,] depth4 = Simplex.Noise.Calc2D(tileWidth, tileHeight, 0.1f);

            int offsetX = tile.tileX * Map.TILE_WIDTH;
            int offsetY = tile.tileY * Map.TILE_HEIGHT;

            for (byte i = 0; i < tileDepth; i++)
            {
                for (byte j = 0; j < tileHeight; j++)
                {
                    int blockDepth1 = (int)Math.Round((depth1[i, j] / 512) * Map.TILE_DEPTH);
                    int blockDepth2 = (int)Math.Round((depth2[i, j] / 1024) * Map.TILE_DEPTH);
                    int blockDepth3 = (int)Math.Round((depth3[i, j] / 2048) * Map.TILE_DEPTH);
                    int blockDepth4 = (int)Math.Round((depth4[i, j] / 4096) * Map.TILE_DEPTH);
                    int blockDepth = (blockDepth1 + blockDepth2 + blockDepth3 + blockDepth4) + 20;

                    for (int k = 0; k < tileWidth; k++)
                    {
                        if (k < blockDepth)
                        {
                            blocks[i, j, k] = new BlockSolid(materialDirt);
                        }
                        /*else if (i == 0 && j == 0)
                        //else if (k < WATER_LEVEL)
                        {
                            material = Material.GetMaterial(Material.Name.Water);
                        }
                        else
                        {
                            material = Material.GetMaterial(Material.Name.None);
                        }*/

                        /*blocks[i, j, k] = new Block(
                            new BlockPosition(i + offsetX, j + offsetY, k),
                            i, j,
                            material, tile
                        );*/
                    }
                }
            }

            return blocks;
        }
    }
}
