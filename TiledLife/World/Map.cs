using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace TiledLife.World
{
    class Map : GameElement
    {
        List<Tile> tiles = new List<Tile>();

        public Map()
        {

        }

        public void Initialize()
        {
            tiles.Add(new Tile(new Vector2(0, 0)));
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(spriteBatch, gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tile tile in tiles)
            {
                tile.Update(gameTime);
            }
        }
    }
}
