using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TiledLife.World.Materials
{
    class Material
    {
        // Enums and static constants
        public enum Name {Air, Dirt, Water };
        public enum Phase { Solid, Liquid, Gas };

        public static int NB_UNIQUE_TEXTURES = 20;
        public static byte FULL = 255;

        // Properties
        public Name name { get; private set; }
        public Color color { get; private set; }
        public Phase phase { get; private set; }
        private Texture2D[] textures;

        // All the materials
        private static Dictionary<Name, Material> materials;

        public Material(Name name, Color color, Phase phase)
        {
            this.name = name;
            this.color = color;
            this.phase = phase;
        }

        public static Material GetMaterial(Name name)
        {
            if (materials == null)
            {
                materials = new Dictionary<Name, Material>();
            }

            if (materials.ContainsKey(name))
            {
                return materials[name];
            }
            else
            {
                Material newMaterial = CreateMaterial(name);
                materials.Add(name, newMaterial);
                return newMaterial;
            }
        }

        private static Material CreateMaterial(Name name)
        {
            switch (name)
            {
                case Name.Air:      return new Material(name, Color.SkyBlue,        Phase.Gas);
                case Name.Dirt:     return new Material(name, Color.SaddleBrown,    Phase.Solid);
                case Name.Water:    return new Material(name, Color.CadetBlue,      Phase.Liquid);
                default: return null;
            }
        }

        public Texture2D GetRandomTexture(SpriteBatch spriteBatch)
        {
            int index = RandomGen.GetInstance().Next(Material.NB_UNIQUE_TEXTURES);
            if (textures == null)
            {
                textures = GenerateTextures(spriteBatch, Map.PIXELS_PER_METER, Map.PIXELS_PER_METER, 10);
            }
            return textures[index];
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
                    colorData[j] = AddNoise(noise, color);
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
