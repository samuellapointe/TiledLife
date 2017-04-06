using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    class ActionWalkTo : BaseNode
    {
        Human human;
        Vector2 target;
        Vector2 direction;

        public ActionWalkTo(Human human, Vector2 target)
        {
            this.human = human;
            this.target = target;
        }

        public override void Initialize()
        {
            direction = new Vector2(target.X - human.position.X, target.Y - human.position.Y);
            this.currentStatus = Status.Running;
        }

        public override Status Run(GameTime gameTime)
        {
            if (currentStatus == Status.New)
            {
                Initialize();
            }

            // human arrived at destination
            if (Vector2.DistanceSquared(target, human.position) < 1)
            {
                return Status.Success;
            }

            human.Walk(direction);
            return Status.Running;
        }
    }
}
