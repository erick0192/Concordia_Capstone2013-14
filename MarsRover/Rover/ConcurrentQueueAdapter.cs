using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using MarsRover;

namespace Rover
{
    public class ConcurrentQueueAdapter : IQueue
    {
        public uint Count { get { return m_count; } }
        private uint m_count = 0;
        private ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        private Object syncRoot = new Object();
        private readonly uint MaxCapacity;

        public ConcurrentQueueAdapter(uint maxCapacity)
        {
            MaxCapacity = maxCapacity;
        }
        public bool TryDequeue(out string result)
        {
            if (queue.TryDequeue(out result))
            {
                lock (syncRoot)
                {
                    m_count -= 1;
                }
                return true;
            }
            return false;
        }

        public void Enqueue(string element, QueuePriorities priority = QueuePriorities.Medium)
        {
            lock (syncRoot)
            {
                if (m_count > MaxCapacity)
                {
                    ClearQueue();
                }
                m_count += 1;
            }
                queue.Enqueue(element);
        }

        public void ClearQueue()
        {
            Console.WriteLine("QueueCleared!!!!");
            lock (syncRoot)
            {
                queue = new ConcurrentQueue<string>();
                m_count = 0;
            }
        }
    }
}
