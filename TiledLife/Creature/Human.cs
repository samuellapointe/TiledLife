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

        // Needs
        public NeedsManager needsManager { get; private set; }

        public Human(Vector2 position)
        {
            this.position = position;
            angle = ((float)(Math.PI / 2)) * RandomGen.GetInstance().Next(0, 4);
            scale = new Vector2(0.10f, 0.10f);
            dna = new DNA();
            needsManager = new NeedsManager(this);
            alive = true;

            behavior = new ActionDecide(this);
        }

        public float GetPhysicalAttr(DNA.PhysicalAttribute attribute)
        {
            return dna.GetPhysicalAttr(attribute);
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
            needsManager.Update(gameTime);

            // Update behavior nodes
            behavior.Run(gameTime);
        }

        public void Walk(Vector2 direction)
        {
            direction.Normalize();
            position += GetPhysicalAttr(DNA.PhysicalAttribute.WalkSpeed) * direction;
        }

        public void Die()
        {
            alive = false;
        }
    }
}
