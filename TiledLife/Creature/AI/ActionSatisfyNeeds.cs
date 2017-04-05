using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    class ActionSatisfyNeeds : BaseNode
    {
        Human human;

        public ActionSatisfyNeeds(Human human)
        {
            this.human = human;
        }

        public override void Initialize()
        {
            currentStatus = Status.Running;
        }

        public override Status Run(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
