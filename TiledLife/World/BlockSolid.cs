using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TiledLife.World.Materials
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
    }
}
