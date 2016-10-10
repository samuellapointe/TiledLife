using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.Creature.AI
{
    interface IControllable
    {
        void Move(Vector2 direction, GameTime gameTime);
        float GetWalkSpeed();
    }
}
