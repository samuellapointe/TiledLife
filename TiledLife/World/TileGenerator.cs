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
        public static Block[,] GenerateTile(int tileHeight, int tileWidth)
        {
            Block[,] blocks = new Block[tileHeight, tileWidth];
            for (int i = 0; i < tileHeight; i++)
            {
                for (int j = 0; j < tileWidth; j++)
                {
                    if (RandomGen.GetInstance().Next(20) == 0)
                    {
                        blocks[i, j] = new Block(
                                MaterialLiquid.CreateMaterial(MaterialLiquid.MaterialLiquidNames.Water),
                                j, i
                            );
                    }
                    else
                    {
                        blocks[i, j] = new Block(
                                MaterialSolid.CreateMaterial(MaterialSolid.MaterialSolidNames.Dirt),
                                j, i
                            );
                    }
                }
            }

            return blocks;
        }
    }
}
