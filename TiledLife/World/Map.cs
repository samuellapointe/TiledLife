using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TiledLife.Creature;

namespace TiledLife.World
{
    class Map : GameElement
    {
        List<Tile> tiles = new List<Tile>();
        List<AbstractCreature> creatures = new List<AbstractCreature>();

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
            for (int i = 0; i < 10; i++)
            {
                creatures.Add(new Sheep(new Vector2(100 + i*10, 500)));
            }
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Tile tile in tiles)
            {
                tile.LoadContent(content);
            }
            foreach (AbstractCreature creature in creatures)
            {
                creature.LoadContent(content);
            }
        }

        public void UnloadContent()
        {
            foreach (Tile tile in tiles)
            {
                tile.UnloadContent();
            }
            foreach (AbstractCreature creature in creatures)
            {
                creature.UnloadContent();
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(spriteBatch, gameTime);
            }
            foreach (AbstractCreature creature in creatures)
            {
                creature.Draw(spriteBatch, gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tile tile in tiles)
            {
                tile.Update(gameTime);
            }
            foreach (AbstractCreature creature in creatures)
            {
                creature.Update(gameTime);
            }
        }
    }
}
