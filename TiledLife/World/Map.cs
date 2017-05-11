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

        public const int BLOCK_WIDTH = 8;
        public const int BLOCK_HEIGHT = 8;

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

        /*public Block GetBlockAtPixelPosition(float x, float y)
        {
            Tile tile = GetTileFromPixelPosition(x, y);

            // Find tile
            if (tile != null)
            {
                BlockPosition positionWithinTile = GetBlockPositionFromPixelPosition(x, y, 0);

                return tile.GetBlockAt(
                    positionWithinTile.Col(), 
                    positionWithinTile.Row(), 
                    positionWithinTile.Depth()
                );
            }

            // Get the block coordinates
            //Debug.Print("Map.cs: Tried to get a block somewhere empty");
            return null;
        }*/

        /*public Block GetBlockAt(BlockPosition blockPosition)
        {
            return GetBlockAt(blockPosition.Col(), blockPosition.Row(), blockPosition.Depth());
        }

        public Block GetBlockAt(int col, int row, int depth)
        {
            Tile tile = GetTileFromBlockPosition(col, row);

            if (tile != null)
            {
                BlockPosition positionWithinTile = GetBlockPositionWithinTile(col, row, depth);

                return tile.GetBlockAt(
                    positionWithinTile.Col(),
                    positionWithinTile.Row(),
                    positionWithinTile.Depth()
                );
            }
            return null;
        }*/

        public AbstractCreature GetCreatureAt(float x, float y)
        {
            Tile tile = GetTileFromPixelPosition(x, y);
            if (tile != null)
            {
                return tile.GetCreatureAtPixelPosition(x, y);
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
            /*tiles.Add("0,1", new Tile(0, 1));
            tiles.Add("1,0", new Tile(1, 0));
            tiles.Add("1,1", new Tile(1, 1));*/
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

        private Tile GetTileFromPixelPosition(float x, float y)
        {
            // The position is a pixel. Turn into a block position;
            int blockCol = (int)Math.Floor(x / BLOCK_WIDTH);
            int blockRow = (int)Math.Floor(y / BLOCK_HEIGHT);

            // Find the tile containing that block
            return GetTileFromBlockPosition(blockCol, blockRow);
        }

        private Tile GetTileFromBlockPosition(int blockCol, int blockRow)
        {
            // Find the tile containing that block
            int tileCol = (int)Math.Floor((float)blockCol / TILE_WIDTH);
            int tileRow = (int)Math.Floor((float)blockRow / TILE_HEIGHT);

            String stringTilePosition = tileCol + "," + tileRow;

            // Find tile
            if (tiles.ContainsKey(stringTilePosition))
            {
                return tiles[stringTilePosition];
            }
            return null;
        }

        /*private BlockPosition GetBlockPositionFromPixelPosition(float col, float row, float depth)
        {
            // The position is a pixel. Turn into a block position;
            int blockCol = (int)Math.Floor(col / PIXELS_PER_METER);
            int blockRow = (int)Math.Floor(row / PIXELS_PER_METER);
            int blockDepth = (int)Math.Floor(depth / PIXELS_PER_METER);

            return GetBlockPositionWithinTile(blockCol, blockRow, blockDepth);
        }

        private BlockPosition GetBlockPositionWithinTile(int col, int row, int depth)
        {
            int tileCol = (int)Math.Floor((float)col / TILE_WIDTH);
            int tileRow = (int)Math.Floor((float)row / TILE_HEIGHT);

            int colWithinTile = col - (tileCol * TILE_WIDTH);
            int rowWithinTile = row - (tileRow * TILE_HEIGHT);
            int depthWithinTile = depth;

            return new BlockPosition(colWithinTile, rowWithinTile, depthWithinTile);
        }*/
    }
}
