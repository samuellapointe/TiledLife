using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TiledLife.World.Materials;
using TiledLife.World;

namespace TiledLife.Creature.AI
{
    class ActionLookForMaterial : BaseNode
    {
        Human human;
        string materialName;
        Queue<Vector2> pointsToCheck;
        const int NB_POINTS_TO_CHECK_PER_TICK = 5;

        public Vector2 finalPosition { get; protected set; }

        public ActionLookForMaterial(Human human, string materialName)
        {
            this.human = human;
            this.materialName = materialName;
            pointsToCheck = new Queue<Vector2>();
        }

        public override void Initialize()
        {
            // Origin point
            Vector2 origin = human.position;

            // From angle, get direction vectors
            float angle = human.angle;
            Vector2 directionVector = new Vector2(
                (float)Math.Cos(angle), 
                (float)Math.Sin(angle)
            );
            Vector2 perpendicularVector = new Vector2(
                (float)Math.Cos(angle + (Math.PI / 2)), 
                (float)Math.Sin(angle + (Math.PI / 2))
            );

            // How far to look
            float viewDistance = human.GetPhysicalAttr(DNA.PhysicalAttribute.ViewDistance);
            float lookIncrement = Map.PIXELS_PER_METER * 0.75f;
            int nbPointsForward = (int)(viewDistance / lookIncrement);

            // Get the points we'll look at
            for (int i = 0; i < nbPointsForward; i++)
            {
                Vector2 pointPosition = origin + (i * directionVector * lookIncrement);
                pointsToCheck.Enqueue(pointPosition);

                for (int j = -i; j <= i; j++)
                {
                    if (j != 0)
                    {
                        Vector2 sidePoint = pointPosition + (j * perpendicularVector * lookIncrement);
                        pointsToCheck.Enqueue(sidePoint);
                    }
                }
            }

            currentStatus = Status.Running;
        }

        public override Status Run(GameTime gameTime)
        {
            if (currentStatus == Status.New)
            {
                Initialize();
                return currentStatus;
            }
            else if (currentStatus != Status.Running)
            {
                return currentStatus;
            }
            else
            {
                for (int i = 0; i < NB_POINTS_TO_CHECK_PER_TICK; i++)
                {
                    if (pointsToCheck.Count > 0)
                    {
                        Vector2 point = pointsToCheck.Dequeue();
                        //Map.GetInstance().AddDebugDot(point);
                        Block block = Map.GetInstance().GetBlockAtPixelPosition(point);
                        if (block != null && block.material.name.Equals(materialName))
                        {
                            finalPosition = point;
                            currentStatus = Status.Success;
                            return currentStatus;
                        }
                    }
                    else
                    {
                        currentStatus = Status.Failure;
                        return currentStatus;
                    }
                }
                return Status.Running;
            }
        }
    }
}
