using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI.Behavior
{
    abstract class AbstractBehavior
    {
        IControllable controllable;

        public abstract void Start();
        public abstract void Run(GameTime gameTime);
        public abstract void End();
    }
}
