using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    class ActionDrink : BaseNode
    {
        Human human;
        public ActionDrink (Human human)
        {
            this.human = human;
        }
        public override void Initialize()
        {
            this.currentStatus = Status.Running;
        }

        public override Status Run(GameTime gameTime)
        {
            if (currentStatus == Status.New)
            {
                Initialize();
            }

            if (!human.needsManager.IsThirstFull())
            {
                human.needsManager.Drink(gameTime);
                return Status.Running;
            }
            else
            {
                return Status.Success;
            }
        }
    }
}
