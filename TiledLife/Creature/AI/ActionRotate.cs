using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    class ActionRotate : BaseNode
    {
        Human human;
        float angle;
        float rotateSpeed;
        float targetAngle;

        public ActionRotate(Human human, float angle)
        {
            this.human = human;
            this.angle = angle;
            this.rotateSpeed = human.GetPhysicalAttr(DNA.PhysicalAttribute.RotateSpeed);
        }

        public override void Initialize()
        {
            targetAngle = human.angle + angle;
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

            float secondsPassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float currentAngle = human.angle;
            
            float deltaAngle = rotateSpeed * secondsPassed;
            deltaAngle *= angle > 0 ? 1 : -1;

            float finalAngle = currentAngle + deltaAngle;

            // Rotate clockwise too far
            if (angle > 0 && finalAngle > targetAngle)
            {
                finalAngle = targetAngle;
            } else if (angle <= 0 && finalAngle < targetAngle)
            {
                finalAngle = targetAngle;
            }

            human.angle = finalAngle;

            if (finalAngle == targetAngle)
            {
                return Status.Success;
            } else
            {
                return Status.Running;
            }
        }
    }
}
