using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.World.Materials
{
    class Air : Material
    {
        // Override basic material definitions, share instances across class
        private static Color color = new Color(255, 255, 255);
        public override Color GetColor() { return color; }

        private static string description = "Air is what you find where there isn't anything else";
        public override string GetDescription() { return description; }

        private static string name = "Air";
        public override string GetName() { return name; }

        private static float speedModifier = 1f;
        public override float GetSpeedModifier() { return speedModifier; }

        private static Texture2D[] textures;
        public override Texture2D GetRandomTexture(SpriteBatch spriteBatch)
        {
            int index = RandomGen.GetInstance().Next(Material.NB_UNIQUE_TEXTURES);
            if (textures == null)
            {
                textures = GenerateTextures(spriteBatch, 10, 10, 0);
            }
            return textures[index];
        }


        public Air(float volume) : base(volume)
        {

        }
    }
}
