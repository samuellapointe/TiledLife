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
        private int width = Map.PIXELS_PER_METER;
        private int height = Map.PIXELS_PER_METER;

        public BlockPosition position { get; private set; }
        private List<Material> materials;

        private Texture2D texture;

        public Block (BlockPosition position)
        {
            this.position = position;
            materials = new List<Material>();
            materials.Add(new Air(1f));
        }

        public Block (BlockPosition position, List<Material> materials)
        {
            this.position = position;
            this.materials = materials;
        }

        private void UpdateTexture(SpriteBatch spriteBatch)
        {
            Material mostCommonMaterial = GetMostCommonMaterial();
            texture = mostCommonMaterial.GetRandomTexture(spriteBatch);
        }

        public Material GetMostCommonMaterial()
        {
            Material mostCommon = materials.First();
            float maxVolume = 0f;
            foreach (Material material in materials)
            {
                if (material.volume > maxVolume)
                {
                    maxVolume = material.volume;
                    mostCommon = material;
                }
            }
            return mostCommon;
        }

        // GameElement functions
        public void Draw(SpriteBatch spriteBatch, int offsetX, int offsetY)
        {
            Vector2 offset = new Vector2(offsetX + position.Col() * width, offsetY + position.Row() * height);
            if (texture == null)
            {
                UpdateTexture(spriteBatch);
            }

            spriteBatch.Draw(texture, offset);
        }
    }
}
