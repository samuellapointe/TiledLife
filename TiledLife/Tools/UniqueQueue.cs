using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiledLife.Tools
{
    // Taken from http://stackoverflow.com/a/19956468/4500041
    class UniqueQueue<T>
    {
        private readonly Queue<T> queue;
        private HashSet<T> alreadyAdded;

        public UniqueQueue()
        {
            queue = new Queue<T>();
            alreadyAdded = new HashSet<T>();
        }

        public UniqueQueue(UniqueQueue<T> clone)
        {
            queue = new Queue<T>(clone.queue);
            alreadyAdded = new HashSet<T>();
        }
        
        public virtual void Enqueue(T item)
        {
            if (alreadyAdded.Add(item)) { queue.Enqueue(item); }
        }
        public int Count { get { return queue.Count; } }

        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            return item;
        }

        public virtual void Clear()
        {
            queue.Clear();
            alreadyAdded.Clear();
        }

    }
}
