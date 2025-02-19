﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiledLife.Creature.AI;

namespace TiledLife.Creature
{
    class ActionSatisfyThirst : BaseNode
    {
        Human human;

        bool foundWater = false;

        ActionLookForMaterial lookForWater;
        Sequence sequence;
        Vector2 waterLocation;

        public ActionSatisfyThirst(Human human)
        {
            this.human = human;
        }

        public override void Initialize()
        {
            lookForWater = new ActionLookForMaterial(human, "water");

            currentStatus = Status.Running;
        }

        public override Status Run(GameTime gameTime)
        {
            if (currentStatus == Status.New)
            {
                Initialize();
            }

            if (!foundWater)
            {
                Status status = lookForWater.Run(gameTime);
                switch (status)
                {
                    case Status.Success:
                        foundWater = true;

                        // Calculate the final angle to rotate to
                        waterLocation = lookForWater.finalPosition;
                        float targetAngle = (float)Math.Atan2(
                            waterLocation.Y - human.position.Y, 
                            waterLocation.X - human.position.X
                        );
                        float angle = targetAngle - human.angle;

                        Queue<BaseNode> queue = new Queue<BaseNode>();
                        queue.Enqueue(new ActionRotate(human, angle));
                        queue.Enqueue(new ActionWalkTo(human, waterLocation));
                        queue.Enqueue(new ActionDrink(human));
                        sequence = new Sequence(queue);
                        return Status.Running;
                    default:
                        return status;
                }
            }
            else
            {
                return sequence.Run(gameTime);
            }
        }
    }
}
