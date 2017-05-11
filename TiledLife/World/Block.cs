using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TiledLife.World.Materials;
using System.Diagnostics;

namespace TiledLife.World
{
    abstract class Block
    {
        public abstract void Draw(SpriteBatch spriteBatch, Vector2 offset);
        public abstract void Update(GameTime gameTime);
    }
}
