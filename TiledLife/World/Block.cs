using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledLife.World.Materials;

/* A block is a 1 meter x 1 meter tile */
namespace TiledLife.World
{
    class Block
    {
        private Material material { get; }

        private int width = Tile.PIXELS_PER_METER;
        private int height = Tile.PIXELS_PER_METER;

        private int blockX;
        private int blockY;

        private Texture2D texture;
        private Random random;

        public Block (Random random, Material material, int blockX, int blockY)
        {
            this.material = material;
            this.blockX = blockX;
            this.blockY = blockY;
            this.random = random;
        }

        private void GenerateTexture(SpriteBatch spriteBatch)
        {
            texture = new Texture2D(spriteBatch.GraphicsDevice, width, height);
            int nbPixels = width * height;
            Color[] colorData = new Color[nbPixels];

            for (int i = 0; i < nbPixels; i++)
            {
                colorData[i] = AddNoise(10, material.color);
            }
            texture.SetData<Color>(colorData);
        }

        // GameElement functions
        public void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY)
        {
            if (texture == null)
            {
                GenerateTexture(spriteBatch);
            }

            Vector2 offset = new Vector2(offsetX + blockX * width, offsetY + blockY * height);
            spriteBatch.Draw(texture, offset);
        }

        public bool CanWalkOn()
        {
            return material.isSolid;
        }

        private Color AddNoise(int amount, Color color)
        {
            int R = color.R + random.Next(-amount, amount + 1);
            int G = color.G + random.Next(-amount, amount + 1);
            int B = color.B + random.Next(-amount, amount + 1);
            R = R > 255 ? 255 : R;
            R = R < 0 ? 0 : R;
            G = G > 255 ? 255 : G;
            G = G < 0 ? 0 : G;
            B = B > 255 ? 255 : B;
            B = B < 0 ? 0 : B;

            return new Color(R, G, B);
        }
    }
}
