using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    class AIController
    {
        IControllable controllable;

        public AIController(IControllable controllable)
        {
            this.controllable = controllable;
        }

        internal void Update(GameTime gameTime)
        {
            controllable.Move(new Vector2(1, 0), gameTime);
        }
    }
}
