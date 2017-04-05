using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TiledLife.Creature.AI
{
    abstract class BaseNode
    {
        public enum Status { New, Success, Failure, Running, Error };
        protected Status currentStatus;
        List<BaseNode> children;

        public BaseNode()
        {
            children = new List<BaseNode>();
            currentStatus = Status.New;
        }

        // Called the first time the node is visited by its parent
        public abstract void Initialize();

        public abstract Status Run(GameTime gameTime);
    }
}
