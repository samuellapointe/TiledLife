using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TiledLife.Creature.AI;

namespace TiledLife.Creature
{
    class Sheep : Creature, IControllable
    {
        float maxHealth = 100f;
        float maxHunger = 100f;
        float maxEnergy = 100f;
        float maxSpeed = 10f;
        float walkSpeed = 3f;

        AIController controller;
        Texture2D texture;

        public Sheep(Vector2 position)
        {
            health = 100f;
            hunger = 0f;
            energy = 100f;
            this.position = position;
            controller = new AIController(this);
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sheep");
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, null, null, null, 0, new Vector2(0.25f, 0.25f));
        }

        public override void Update(GameTime gameTime)
        {
            controller.Update(gameTime);
        }

        public void Move(Vector2 direction, GameTime gameTime)
        {
            direction.Normalize();
            position += direction * walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
