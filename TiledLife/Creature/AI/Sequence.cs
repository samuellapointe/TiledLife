using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace TiledLife.Creature.AI
{
    class Sequence : BaseNode
    {
        Queue<BaseNode> nodes;
        BaseNode currentRunningNode;

        public Sequence(Queue<BaseNode> nodes)
        {
            this.nodes = nodes;
        }

        public override void Initialize()
        {
            currentRunningNode = nodes.Dequeue();
            this.currentStatus = Status.Running;
        }

        public override Status Run(GameTime gameTime)
        {
            if (currentStatus == Status.New)
            {
                Initialize();
            }
            else if (currentStatus != Status.Running)
            {
                return currentStatus;
            }

            Status status = currentRunningNode.Run(gameTime);
            switch (status)
            {
                case Status.New:
                    Debug.Print("Sequence: received a New status from the current node. This shouldn't happen.");
                    currentStatus = Status.Error;
                    return currentStatus;
                case Status.Success:
                    // If all nodes ran with success
                    if (nodes.Count == 0)
                    {
                        return Status.Success;
                    }
                    // Otherwise run the next one in the next tick
                    else
                    {
                        currentRunningNode = nodes.Dequeue();
                        return Status.Running;
                    }
                default:
                    currentStatus = status;
                    return currentStatus;
            }
        }
    }
}
