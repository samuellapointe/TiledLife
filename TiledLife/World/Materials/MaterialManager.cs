using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TiledLife.Tools;

namespace TiledLife.World.Materials
{
    class MaterialManager
    {
        // instance
        private static MaterialManager instance;
        private static List<Material> materials;


        // private constructor
        private MaterialManager()
        {
            materials = new List<Material>();
        }

        // Get instance
        public static MaterialManager GetInstance()
        {
            if (instance == null)
            {
                instance = new MaterialManager();
            }
            return instance;
        }

        // Get material using search criterias
        public Material GetMaterial()
        {
            Material material = new Material(TextureTools.RandomColor());
            materials.Add(material);
            return material;
        }
    }
}
