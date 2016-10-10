using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiledLife.Creature.AI.Behavior;

namespace TiledLife.Creature.AI
{
    class AIController
    {
        IControllable controllable;
        AbstractBehavior currentBehavior;
        Wander wanderBehavior;

        public AIController(IControllable controllable)
        {
            this.controllable = controllable;

            wanderBehavior = new Wander(controllable);
            currentBehavior = wanderBehavior;
            wanderBehavior.Start();
        }

        internal void Update(GameTime gameTime)
        {
            currentBehavior.Run(gameTime);
        }
    }
}
