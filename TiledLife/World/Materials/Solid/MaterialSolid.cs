using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.World.Materials.Solid
{
    class MaterialSolid : Material
    {
        public enum MaterialSolidNames { Dirt }

        public MaterialSolid(Color color, string name) : base(color, name, true)
        {

        }

        public static MaterialSolid CreateMaterial(MaterialSolidNames material)
        {
            switch (material)
            {
                case MaterialSolidNames.Dirt:
                    return new MaterialSolid(Color.SaddleBrown, "dirt");
                default:
                    return new MaterialSolid(Color.White, "unknown solid");
            }
        }
    }
}
