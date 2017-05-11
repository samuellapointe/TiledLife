using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLife.Tools;

namespace TiledLife.World.Materials
{
    class Material
    {
        private Texture2D texture;
        private Color baseColor;

        public Material(Color baseColor)
        {
            this.baseColor = baseColor;
        }

        public Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (texture == null)
            {
                texture = TextureTools.GenerateTexture(spriteBatch, Map.BLOCK_WIDTH, Map.BLOCK_HEIGHT, baseColor, 10);
            }

            return texture;
        }
    }
}
