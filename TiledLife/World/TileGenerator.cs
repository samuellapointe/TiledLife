using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TiledLife.World.Materials.Liquid;
using TiledLife.World.Materials.Solid;

namespace TiledLife.World
{
    static class TileGenerator
    {
        public static Block[,] GenerateTile(Random random, int tileHeight, int tileWidth)
        {
            Block[,] blocks = new Block[tileHeight, tileWidth];
            for (int i = 0; i < tileHeight; i++)
            {
                for (int j = 0; j < tileWidth; j++)
                {
                    if (random.Next(20) == 0)
                    {
                        blocks[i, j] = new Block(
                                random,
                                MaterialLiquid.CreateMaterial(MaterialLiquid.MaterialLiquidNames.water),
                                j, i
                            );
                    }
                    else
                    {
                        blocks[i, j] = new Block(
                                random,
                                MaterialSolid.CreateMaterial(MaterialSolid.MaterialSolidNames.dirt),
                                j, i
                            );
                    }
                }
            }

            return blocks;
        }
    }
}
