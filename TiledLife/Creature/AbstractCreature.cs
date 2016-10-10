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
        public float health
        {
            get;
            set;
        }

        public float hunger
        {
            get;
            set;
        }

        public float energy
        {
            get;
            set;
        }

        public Vector2 position
        {
            get;
            set;
        }
   
        // Needed by the game
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
    }
}
