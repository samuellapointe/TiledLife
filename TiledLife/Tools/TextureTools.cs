using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.Tools
{
    class TextureTools
    {
        public static Texture2D GenerateTexture(SpriteBatch spriteBatch, int width, int height, Color baseColor, int noise)
        {
            Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, width, height);

            int nbPixels = width * height;
            Color[] colorData = new Color[nbPixels];

            for (int j = 0; j < nbPixels; j++)
            {
                colorData[j] = AddNoise(noise, baseColor);
            }
            texture.SetData<Color>(colorData);

            return texture;
        }

        public static Color AddNoise(int amount, Color color)
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

        public static Color RandomColor()
        {
            Color randomColor = new Color(
                    RandomGen.GetFloat(0, 1),
                    RandomGen.GetFloat(0, 1),
                    RandomGen.GetFloat(0, 1)
                );

            return randomColor;
        }
    }
}
