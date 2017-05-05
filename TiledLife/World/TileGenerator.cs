using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TiledLife.World.Materials.Liquid;
using TiledLife.World.Materials.Solid;
using TiledLife.World.Materials;

namespace TiledLife.World
{
    static class TileGenerator
    {
        public static Block[,,] GenerateTile(int tileHeight, int tileWidth, int tileDepth)
        {
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
                    int blockDepth = (blockDepth1 + blockDepth2 + blockDepth3 + blockDepth4) + 30;

                    for (int k = 0; k < tileWidth; k++)
                    {
                        Material material = null;
                        if (k < blockDepth)
                        {
                            material = MaterialSolid.CreateMaterial(MaterialSolid.MaterialSolidNames.Dirt);
                        }
                        blocks[i, j, k] = new Block(
                            material,
                            i, j, k,
                            blocks
                        );
                    }
                }
            }

            return blocks;
        }
    }
}
