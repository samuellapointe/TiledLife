using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TiledLife.Creature
{
    abstract class AbstractCreature : GameElement
    {
        public float health { get; protected set; }
        public float hunger { get; protected set; }
        public float energy { get; protected set; }
        public float thirst { get; protected set; }

        public Vector2 position { get; protected set; }
   
        // Needed by the game
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
    }
}
