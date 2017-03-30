using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.World.Materials.Liquid
{
    class MaterialLiquid : Material
    {
        public enum MaterialLiquidNames { water }

        public MaterialLiquid(Color color, string name) : base(color, name, false)
        {

        }

        public static MaterialLiquid CreateMaterial(MaterialLiquidNames material)
        {
            switch (material)
            {
                case MaterialLiquidNames.water:
                    return new MaterialLiquid(Color.Blue, "water");
                default:
                    return new MaterialLiquid(Color.White, "unknown liquid");
            }
        }
    }
}
