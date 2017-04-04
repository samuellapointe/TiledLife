using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using TiledLife.World;
using TiledLife.Creature.AI;

namespace TiledLife.Creature
{
    class Human : AbstractCreature
    {
        private Texture2D texture;

        // Unique attributes
        DNA dna;

        // AI
        BaseNode behavior;

        public Human(Vector2 position)
        {
            thirst = 0f;
            this.position = position;
            angle = ((float)(Math.PI / 2)) * RandomGen.GetInstance().Next(0, 4);
            scale = new Vector2(0.10f, 0.10f);
            dna = new DNA();
            alive = true;

            behavior = new ActionDecide(this);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, null, new Vector2(16f, 16f), angle, scale);
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
            // This shouldn't happen, but check in case we are trying to update a dead creature
            if (!alive) {
                Debug.Print("Warning: Attempted to update a dead creature");
                return;
            }

            // Update physical needs
            thirst += dna.GetPhysicalAttr(DNA.PhysicalAttribute.ThirstIncreaseRate) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check vital needs
            if (thirst > dna.GetPhysicalAttr(DNA.PhysicalAttribute.MaxThirst))
            {
                Die();
            }

            // Decide what to do
            /*if (thirst > dna.GetPhysicalAttr(DNA.PhysicalAttribute.MaxThirst) * 0.01)
            {
                findingWater = true;
            }

            // Do something
            if (findingWater && !foundWater)
            {
                Vector2 checkPosition = position + new Vector2(0, -8);
                Block testBlock = Map.GetInstance().GetBlockAt(checkPosition);
                if (testBlock != null && !testBlock.CanWalkOn())
                {
                    foundWater = true;
                    scale *= 4;
                }
                // Check in front
                angle += 0.8f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }*/

            // Update behavior nodes
            behavior.Run(gameTime);
        }

        private void Die()
        {
            alive = false;
        }
    }
}
