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

        // A map is made of tiles
        Dictionary<String, Tile> tiles = new Dictionary<string, Tile>();

        // Debugging vars
        private List<Vector2> debugDots = new List<Vector2>();
        private Texture2D debugDotTexture;

        // These should be the same for every tile on the map
        public const int TILE_HEIGHT = 100;
        public const int TILE_WIDTH = 100;
        public const int TILE_DEPTH = 100;
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

        public Block GetBlockAtPixelPosition(Vector2 pixelPosition)
        {
            Tile tile = GetTileFromPixelPosition(pixelPosition);

            // Find tile
            if (tile != null)
            {
                BlockPosition positionWithinTile = GetBlockPositionFromPixelPosition(pixelPosition);

                return tile.GetBlockAt(
                    positionWithinTile.Col(), 
                    positionWithinTile.Row(), 
                    positionWithinTile.Depth()
                );
            }

            // Get the block coordinates
            //Debug.Print("Map.cs: Tried to get a block somewhere empty");
            return null;
        }

        public AbstractCreature GetCreatureAt(Vector2 position)
        {
            Tile tile = GetTileFromPixelPosition(position);
            if (tile != null)
            {
                return tile.GetCreatureAtPixelPosition(position);
            }
            return null;
        }

        public void AddDebugDot(Vector2 position)
        {
            debugDots.Add(position);
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
            debugDotTexture = content.Load<Texture2D>("DebugDot");
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
            foreach (Vector2 position in debugDots)
            {
                spriteBatch.Draw(debugDotTexture, position, null, null, new Vector2(8f, 8f), 0, new Vector2(0.2f, 0.2f));
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<string, Tile> entry in tiles)
            {
                entry.Value.Update(gameTime);
            }
        }

        private Tile GetTileFromPixelPosition(Vector2 position)
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
                return tiles[stringTilePosition];
            }
            return null;
        }

        private BlockPosition GetBlockPositionFromPixelPosition(Vector2 position)
        {
            // The position is a pixel. Turn into a block position;
            Vector2 blockPosition = position / PIXELS_PER_METER;

            // Find the tile containing that block
            Vector2 tilePosition = new Vector2(
                (float)Math.Floor(blockPosition.X) / TILE_WIDTH,
                (float)Math.Floor(blockPosition.Y) / TILE_HEIGHT
            );

            int colWithinTile = (int)Math.Floor(blockPosition.X) - ((int)tilePosition.X * TILE_WIDTH);
            int rowWithinTile = (int)Math.Floor(blockPosition.Y) - ((int)tilePosition.Y * TILE_HEIGHT);
            int depthWithinTile = 0;

            return new BlockPosition(colWithinTile, rowWithinTile, depthWithinTile);
        }
    }
}
