using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.Creature.AI.Behavior
{
    class Wander : AbstractBehavior
    {
        const int CircleDistance = 5;
        const int CircleRadius = 5;

        const float MaximumAngleChange = 0.02f;
        float angle = 0;
        float displacementAngle = 0;

        Vector2 currentVelocity;
        Vector2 circleCenter; // Actually a point

        private static readonly Random random = new Random();

        IControllable controllable;
        
        public Wander(IControllable controllable)
        {
            this.controllable = controllable;
        }

        public override void Start()
        {
            // Init direction
            currentVelocity = new Vector2(0, 0);

            // Init circle
            float circleX = (float)Math.Cos(angle);
            float circleY = (float)Math.Sin(angle);
            circleCenter = new Vector2(circleX, circleY);
        }

        public override void Run(GameTime gameTime)
        {
            // Update angle
            angle = (float)Math.Atan2(currentVelocity.X, -currentVelocity.Y);

            // Update circle
            float circleX = (float)Math.Cos(angle);
            float circleY = (float)Math.Sin(angle);
            circleCenter = new Vector2(circleX, circleY);

            displacementAngle += ((float)random.NextDouble() * MaximumAngleChange) - (MaximumAngleChange / 2);
            float displacementX = (float)Math.Cos(displacementAngle);
            float displacementY = (float)Math.Sin(displacementAngle);
            Vector2 displacementVector = new Vector2(displacementX, displacementY);
            displacementVector *= CircleRadius;
            displacementVector += circleCenter;

            currentVelocity = currentVelocity + displacementVector;
            if (currentVelocity.LengthSquared() > Math.Pow(controllable.GetWalkSpeed(), 2))
            {
                currentVelocity.Normalize();
                currentVelocity *= controllable.GetWalkSpeed();
            }

            controllable.Move(currentVelocity, gameTime);
        }

        public override void End()
        {
            throw new NotImplementedException();
        }
    }
}
