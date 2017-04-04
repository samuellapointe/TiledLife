using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    class ActionRotate : BaseNode
    {
        Human human;
        float angle;
        float rotateSpeed = 1;
        float targetAngle;

        public ActionRotate(Human human, float angle)
        {
            this.human = human;
            this.angle = angle;
        }

        public override void Initialize()
        {
            targetAngle = human.angle + angle;
            initialized = true;
        }

        public override Status Run(GameTime gameTime)
        {
            if (!initialized) Initialize();

            float secondsPassed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float currentAngle = human.angle;
            
            float deltaAngle = rotateSpeed * secondsPassed;
            deltaAngle *= angle > 0 ? 1 : -1;

            float finalAngle = currentAngle + deltaAngle;

            // Rotate clockwise too far
            if (angle >= 0 && finalAngle > targetAngle)
            {
                finalAngle = targetAngle;
            } else if (angle < 0 && finalAngle < targetAngle)
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
