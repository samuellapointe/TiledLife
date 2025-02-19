﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledLife.World.Materials;

namespace TiledLife.World
{
    // A solid block is full of a single material
    class BlockSolid : Block
    {
        public Material material;

        public BlockSolid(Material material)
        {
            this.material = material;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.Draw(material.GetTexture(spriteBatch), offset);
        }

        public override void Update(GameTime gameTime)
        {
            throw new Exception("Tried to update a solid block");
        }
    }
}
