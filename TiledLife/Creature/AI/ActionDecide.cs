using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TiledLife.Creature.AI
{
    // This behavior acts as the root of human behavior
    class ActionDecide : BaseNode
    {
        Human human;
        BaseNode currentNode;

        public ActionDecide(Human human)
        {
            this.human = human;
        }

        public override void Initialize()
        {
            currentStatus = Status.Running;
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

            if (currentNode != null)
            {
                Status status = currentNode.Run(gameTime);
                switch (status)
                {
                    case BaseNode.Status.Success:
                        currentNode = null;
                        break;
                    case BaseNode.Status.Failure:
                        currentNode = null;
                        break;
                    case BaseNode.Status.Error:
                        Debug.Print("Node returned an error");
                        if (Debugger.IsAttached) Debugger.Break();
                        break;
                    case BaseNode.Status.Running:
                    case BaseNode.Status.New:
                        break;
                }
            } else
            {
                // Choose random angle and rotate that amount
                float randomAngle = (float)RandomGen.GetInstance().Next(-20, 21) / 10;
                BaseNode node1 = new ActionSatisfyNeeds(human);
                BaseNode node2 = new ActionRotate(human, randomAngle);
                Queue<BaseNode> queue = new Queue<BaseNode>();
                queue.Enqueue(node2);
                queue.Enqueue(node1);
                currentNode = new Sequence(queue);
            }

            return BaseNode.Status.Running;
        }
    }
}
