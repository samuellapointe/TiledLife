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
                switch(currentNode.Run(gameTime))
                {
                    case BaseNode.Status.Success:
                        currentNode = null;
                        break;
                    case BaseNode.Status.Failure:
                        break;
                    case BaseNode.Status.Error:
                        break;
                    case BaseNode.Status.Running:
                        break;
                }
            } else
            {
                // Choose random angle and rotate that amount
                /*float randomAngle = (float)RandomGen.GetInstance().Next(-20, 21) / 10;
                currentNode = new ActionRotate(human, randomAngle);*/
                currentNode = new ActionLookForMaterial(human, "water");
            }

            return BaseNode.Status.Running;
        }
    }
}
