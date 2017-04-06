using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    class ActionTemplate : BaseNode
    {
        Human human;
        public ActionTemplate (Human human)
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
            return Status.Success;
        }
    }
}
