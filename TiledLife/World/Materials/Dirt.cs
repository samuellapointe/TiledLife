using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TiledLife.World.Materials
{
    class Dirt : Material
    {
        // Override basic material definitions, share instances across class
        private static Color color = Color.SaddleBrown;
        public override Color GetColor() { return color; }

        private static string description = "Plain old dirt";
        public override string GetDescription() { return description; }

        private static string name = "Dirt";
        public override string GetName() { return name; }

        // Can't walk through dirt easily; 
        private static float speedModifier = 0.001f;
        public override float GetSpeedModifier() { return speedModifier; }

        private static Texture2D[] textures;
        public override Texture2D GetRandomTexture(SpriteBatch spriteBatch)
        {
            int index = RandomGen.GetInstance().Next(Material.NB_UNIQUE_TEXTURES);
            if (textures == null)
            {
                textures = GenerateTextures(spriteBatch, Map.PIXELS_PER_METER, Map.PIXELS_PER_METER, 10);
            }
            return textures[index];
        }

        public Dirt(float volume) : base(volume)
        {

        }
    }
}
