using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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
            foreach (Tile tile in tiles)
            {
                tile.Initialize();
            }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Tile tile in tiles)
            {
                tile.LoadContent(content);
            }
        }

        public void UnloadContent()
        {
            foreach (Tile tile in tiles)
            {
                tile.UnloadContent();
            }
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
