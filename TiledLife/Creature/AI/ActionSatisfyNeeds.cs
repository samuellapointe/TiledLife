﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    class ActionSatisfyNeeds : BaseNode
    {
        Human human;
        BaseNode sequence;

        public ActionSatisfyNeeds(Human human)
        {
            this.human = human;
        }

        public override void Initialize()
        {
            Queue<BaseNode> nodes = new Queue<BaseNode>();
            if (human.needsManager.IsThirsty())
            {
                nodes.Enqueue(new ActionSatisfyThirst(human));
            }

            sequence = new Sequence(nodes);
            currentStatus = Status.Running;
        }

        public override Status Run(GameTime gameTime)
        {
            if (currentStatus == Status.New)
            {
                Initialize();
            }

            return sequence.Run(gameTime);
        }
    }
}
