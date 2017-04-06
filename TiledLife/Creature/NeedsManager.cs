using Microsoft.Xna.Framework;
using PA = TiledLife.Creature.DNA.PhysicalAttribute;

namespace TiledLife.Creature
{
    class NeedsManager
    {
        Human human;

        float thirst;

        public NeedsManager(Human human)
        {
            thirst = 0f;
            this.human = human;
        }

        public void Update (GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            thirst += human.GetPhysicalAttr(PA.ThirstIncreaseRate) * deltaTime;
            if (thirst > GetPA(PA.MaxThirst))
            {
                human.Die();
            }
        }

        public bool IsThirsty()
        {
            float thirstPercentage = thirst / GetPA(PA.MaxThirst);
            float threshold = GetPA(PA.ThirstThreshold);

            return thirstPercentage > GetPA(PA.ThirstThreshold);
        }

        public void Drink(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            thirst -= human.GetPhysicalAttr(PA.ThirstIncreaseRate) * deltaTime * 5;
        }

        public bool IsThirstFull()
        {
            float thirstPercentage = thirst;

            return thirst < 0.05f;
        }

        // Shortcut for convenience
        private float GetPA(PA pa)
        {
            return human.GetPhysicalAttr(pa);
        }
    }
}
