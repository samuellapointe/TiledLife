using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TiledLife.World.Materials
{
    abstract class Material
    {
        public Color color { get; }
        public string name { get; }

        public bool isSolid { get; }

        public Material(Color color, string name, bool isSolid)
        {
            this.color = color;
            this.name = name;
            this.isSolid = isSolid;
        }
    }
}
