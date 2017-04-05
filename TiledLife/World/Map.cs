using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TiledLife.Creature;
using System.Diagnostics;

namespace TiledLife.World
{
    class Map : GameElement
    {
        private static Map map;

        Dictionary<String, Tile> tiles = new Dictionary<string, Tile>();

        // These should be the same for every tile on the map
        public const int TILE_HEIGHT = 100;
        public const int TILE_WIDTH = 100;
        public const int PIXELS_PER_METER = 8;

        private Map()
        {

        }

        public static Map GetInstance()
        {
            if (map == null)
            {
                map = new World.Map();
            }
            return map;
        }

        public Block GetBlockAt(Vector2 position)
        {
            // The position is a pixel. Turn into a block position;
            Vector2 blockPosition = position / PIXELS_PER_METER;

            // Find the tile containing that block
            Vector2 tilePosition = new Vector2(
                (float)Math.Floor(blockPosition.X) / TILE_WIDTH, 
                (float)Math.Floor(blockPosition.Y) / TILE_HEIGHT
            );
            String stringTilePosition = (int)Math.Floor(tilePosition.X) + "," + (int)Math.Floor(tilePosition.Y);

            // Find tile
            if (tiles.ContainsKey(stringTilePosition))
            {
                Tile tile = tiles[stringTilePosition];
                int xWithinTile = (int)Math.Floor(blockPosition.X) - ((int)tilePosition.X * TILE_WIDTH);
                int yWithinTile = (int)Math.Floor(blockPosition.Y) - ((int)tilePosition.Y * TILE_HEIGHT);

                return tile.GetBlockAt(xWithinTile, yWithinTile);
            }

            // Get the block coordinates
            //Debug.Print("Map.cs: Tried to get a block somewhere empty");
            return null;
        }

        public void Initialize()
        {
            tiles.Add("0,0", new Tile(0, 0));
            foreach (KeyValuePair<string, Tile> entry in tiles)
            {
                entry.Value.Initialize();
            }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (KeyValuePair<string, Tile> entry in tiles)
            {
                entry.Value.LoadContent(content);
            }
        }

        public void UnloadContent()
        {
            foreach (KeyValuePair<string, Tile> entry in tiles)
            {
                entry.Value.UnloadContent();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (KeyValuePair<string, Tile> entry in tiles)
            {
                entry.Value.Draw(spriteBatch, gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<string, Tile> entry in tiles)
            {
                entry.Value.Update(gameTime);
            }
        }
    }
}
