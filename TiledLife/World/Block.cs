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
    class BlockPosition
    {
        private int[] position;
        public BlockPosition(int col, int row, int depth)
        {
            position = new int[] { col, row, depth };
        }
        public int Col() { return position[0]; }
        public int Row() { return position[1]; }
        public int Depth() { return position[2]; }
    }

    class Block
    {
        public Material material { get; private set; }

        private int width = Map.PIXELS_PER_METER;
        private int height = Map.PIXELS_PER_METER;

        private int blockX;
        private int blockY;
        private int blockZ;

        private Block[,,] blocks;

        private Texture2D texture;

        public Block (Material material, int blockX, int blockY, int blockZ, Block[,,] blocks)
        {
            this.material = material;
            this.blockX = blockX;
            this.blockY = blockY;
            this.blockZ = blockZ;
            this.blocks = blocks;
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

        public bool IsEmpty()
        {
            return material == null;
        }

        public bool IsVisible()
        {
            for (int i = blockZ; i < Map.TILE_DEPTH-1; i++)
            {
                if (!blocks[blockX, blockY, i].IsEmpty())
                {
                    return false;
                }
            }
            return true;
        }

        private Color AddNoise(int amount, Color color)
        {
            int darkness = (Map.TILE_DEPTH - blockZ)*2;
            int R = color.R + RandomGen.GetInstance().Next(-amount, amount + 1) - darkness;
            int G = color.G + RandomGen.GetInstance().Next(-amount, amount + 1) - darkness;
            int B = color.B + RandomGen.GetInstance().Next(-amount, amount + 1) - darkness;
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
