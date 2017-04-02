using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace TiledLife.Creature
{
    class Human : AbstractCreature
    {
        private Texture2D texture;
        private float maxThirst = 100;

        // Unique attributes
        DNA dna;

        public Human(Vector2 position)
        {
            thirst = 0f;
            this.position = position;
            dna = new DNA();
            alive = true;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, null, null, 0, new Vector2(0.10f, 0.10f));
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Human");
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            if (!alive)
            {
                Debug.Print("Warning: Attempted to update a dead creature");
                return;
            }

            thirst += dna.GetPhysicalAttr(DNA.PhysicalAttributes.thirstIncreaseRate) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (thirst > dna.GetPhysicalAttr(DNA.PhysicalAttributes.maxThirst))
            {
                Die();
            }
        }

        private void Die()
        {
            alive = false;
        }
    }
}
