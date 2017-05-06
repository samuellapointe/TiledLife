using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.World.Materials
{
    abstract class Material
    {
        public static int NB_UNIQUE_TEXTURES = 10;

        // Abstract values
        public abstract string GetName();
        public abstract string GetDescription();
        public abstract Color GetColor();
        public abstract float GetSpeedModifier();
        public abstract Texture2D GetRandomTexture(SpriteBatch spriteBatch);

        // Non-abstract
        public float volume;

        public Material(float volume)
        {
            this.volume = volume;
        }

        protected Texture2D[] GenerateTextures(SpriteBatch spriteBatch, int width, int height, int noise)
        {
            Texture2D[] textures = new Texture2D[NB_UNIQUE_TEXTURES];

            for (int i = 0; i < NB_UNIQUE_TEXTURES; i++)
            {
                textures[i] = new Texture2D(spriteBatch.GraphicsDevice, width, height);
                int nbPixels = width * height;
                Color[] colorData = new Color[nbPixels];

                for (int j = 0; j < nbPixels; j++)
                {
                    colorData[j] = AddNoise(noise, GetColor());
                }
                textures[i].SetData<Color>(colorData);
            }

            return textures;
        }

        private Color AddNoise(int amount, Color color)
        {
            int R = color.R + RandomGen.GetInstance().Next(-amount, amount + 1);
            int G = color.G + RandomGen.GetInstance().Next(-amount, amount + 1);
            int B = color.B + RandomGen.GetInstance().Next(-amount, amount + 1);
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
