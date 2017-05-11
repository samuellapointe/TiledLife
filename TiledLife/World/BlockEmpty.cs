using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TiledLife.World
{
    // A solid block is full of a single material
    class BlockEmpty : Block
    {
        public BlockEmpty()
        {

        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            throw new Exception("Tried to draw an empty block");
        }

        public override void Update(GameTime gameTime)
        {
            //throw new Exception("Tried to update a solid block");
        }
    }
}
