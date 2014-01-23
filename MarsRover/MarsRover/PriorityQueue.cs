using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public class PriorityQueue : IQueue
    {
        private class Node
        {
            public QueuePriorities Priority { get; private set; }
            public string Value { get; private set; }
            public Node(QueuePriorities priority, string value) {
                Priority = priority;
                Value = value;
            }
        }

        public uint Count { 
            get { return (uint)(lastElement + 1); }            
        }

        private Node[] queue;
        private int lastElement = -1;
        private const int RootIndex = 0;
        private readonly int MaxCapacity = 0;
        private static object WriteLock = new Object();


        public PriorityQueue(int maxCapacity) {
            MaxCapacity = maxCapacity;
            queue = new Node[MaxCapacity];
        }
            
        public bool TryDequeue(out string result)
        {
            result = null;
            lock (WriteLock)
            {
                if (Count > 0) {               
                    result = queue[RootIndex].Value;
                    queue[RootIndex] = queue[lastElement];
                    queue[lastElement] = null;
                    lastElement -= 1;

                    BubbleDown(RootIndex);                                        
                }
            }
                        
            return result != null;
        }

        public void Enqueue(string value, QueuePriorities priority = QueuePriorities.Medium)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            lock (WriteLock)
            {
                if (Count + 1 > MaxCapacity) {
                    ClearQueue();
                }
                lastElement += 1;
                queue[lastElement] = new Node(priority, value);
                BubbleUp(lastElement);
            }
        }

        public void ClearQueue()
        {
            lock (WriteLock)
            {
                Array.Clear(queue, 0, MaxCapacity);
                lastElement = -1;
                Console.WriteLine("***Queue Cleared***");
            }
        }

        private void BubbleDown(int index)
        {
            if (index >= MaxCapacity) { return; }

            int leftChild = index * 2 + 1;
            int rightChild = leftChild + 1;

            if (leftChild >= MaxCapacity) { return; }


            if (IsLeftNodeHigherPriority(leftChild, rightChild))
            {
                if (IsLeftNodeHigherPriority(leftChild, index)) {
                    Swap(ref queue[leftChild], ref queue[index]);
                    BubbleDown(leftChild);
                }
            }           
            else
            {
                if (IsLeftNodeHigherPriority(rightChild, index)) {                                
                    Swap(ref queue[rightChild], ref queue[index]);
                    BubbleDown(rightChild);
                }
            }
        }

        private bool IsLeftNodeHigherPriority(int leftIndex, int rightIndex)
        {
            if (queue[leftIndex] == null && queue[rightIndex] == null) 
            {
                return false;
            }
            else if (queue[rightIndex] == null) {
                return true;
            }
            else if (queue[leftIndex] == null)
            {
                return false;
            }
            else
            {
                return (queue[leftIndex].Priority < queue[rightIndex].Priority);
            }
        }
        private void BubbleUp(int last) {
            if (last == 0) { return; }

            int parentIndex = (int)(Math.Ceiling(last / 2.0) - 1d);
            if (queue[parentIndex].Priority > queue[last].Priority) 
            {
                Swap(ref queue[parentIndex], ref queue[last]);
                BubbleUp(parentIndex);
            }
        }

        private void Swap(ref Node a, ref Node b)
        {
            Node temp = a;
            a = b;
            b = temp;
        }
    }

}
