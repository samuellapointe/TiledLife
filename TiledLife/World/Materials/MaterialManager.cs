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


        /*enum Id {None, Dirt};
        
        // Properties

        public Color color { get; private set; }
        public static int NB_UNIQUE_TEXTURES = 20;
        private Texture2D[] textures;

        // All the materials
        private static Dictionary<String, MaterialManager> materials;

        public MaterialManager(String name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        public static MaterialManager GetMaterial(Name name)
        {
            if (materials == null)
            {
                materials = new Dictionary<Name, MaterialManager>();
            }

            if (materials.ContainsKey(name))
            {
                return materials[name];
            }
            else
            {
                MaterialManager newMaterial = CreateMaterial(name);
                materials.Add(name, newMaterial);
                return newMaterial;
            }
        }

        private static MaterialManager CreateMaterial(Name name)
        {
            switch (name)
            {
                case Name.None:     return new MaterialManager(name, Color.SkyBlue,        Phase.Gas);
                case Name.Dirt:     return new MaterialManager(name, Color.SaddleBrown,    Phase.Solid);
                case Name.Water:    return new MaterialManager(name, Color.CadetBlue,      Phase.Liquid);
                default: return null;
            }
        }

        public Texture2D GetRandomTexture(SpriteBatch spriteBatch)
        {
            int index = RandomGen.GetInstance().Next(MaterialManager.NB_UNIQUE_TEXTURES);
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
        }*/
    }
}
