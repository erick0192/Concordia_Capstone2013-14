using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using MarsRover;

namespace MarsRoverTest
{
    class PriorityQueueTests
    {
        private const int MaxCapacity = 5;
        private const string VALID_ELEMENT_MOVEMENT = "<MF255MF255>";
        private const string VALID_ELEMENT_CAM_ON = "<C1O>";
        Thread[] threads = new Thread[2];

        [Test]
        public void Enqueue_InsertNull_ThrowsArgumentNullException()
        {
            Assert.Throws<System.ArgumentNullException>(
                delegate
                {
                    IQueue queue = new PriorityQueue(MaxCapacity);
                    queue.Enqueue(null, QueuePriorities.Medium);
                }
            );
        }

        [Test]
        public void Enqueue_InsertValidElement_HasOneElement()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);
            queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.Medium);
            uint expected = 1;
            uint actual = queue.Count;
            Assert.AreEqual(expected, actual);  
        }

        
        [Test]
        public void Enqueue_ConcurrentlyInsertTwoValidElements_CountEqualsTwo()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);
            threads[0] = new Thread(() => InsertIntoQueue(queue, QueuePriorities.Medium));
            threads[1] = new Thread(() => InsertIntoQueue(queue, QueuePriorities.Medium));

            threads[0].Start();
            threads[1].Start();

            threads[0].Join();
            threads[1].Join();

            uint expected = 2;
            uint actual = queue.Count;
            Assert.AreEqual(expected, actual);  
        }

        [Test]
        public void TryDequeue_RemoveFromEmptyQueue_ReturnsFalse()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);
            string actual;
            bool result = queue.TryDequeue(out actual);
            Assert.IsFalse(result);
        }

        [Test]
        public void TryDequeue_AddTwoElementsSequentiallyAndRemoveConcurrently_QueueIsEmpty()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);
            queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.Medium);
            queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.Medium);

            threads[0] = new Thread(() => RemoveFromQueue(queue));
            threads[1] = new Thread(() => RemoveFromQueue(queue));

            threads[0].Start();
            threads[1].Start();

            threads[0].Join();
            threads[1].Join();

            uint expected = 0;
            uint actual = queue.Count;

            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void Enqueue_InsertLowPriorityHighPriorityElements_GetElementsBackInOrder()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);

            queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.Low);
            queue.Enqueue(VALID_ELEMENT_CAM_ON, QueuePriorities.High);

            string expected1 = VALID_ELEMENT_CAM_ON;
            string expected2 = VALID_ELEMENT_MOVEMENT;

            string actual1; 
            bool result1 = queue.TryDequeue(out actual1);
            string actual2; 
            bool result2 = queue.TryDequeue(out actual2);

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        [Test]
        public void Enqueue_InsertHighPriorityLowPriorityElements_GetElementsBackInOrder()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);

            queue.Enqueue(VALID_ELEMENT_CAM_ON, QueuePriorities.High);
            queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.Low);

            string expected1 = VALID_ELEMENT_CAM_ON;
            string expected2 = VALID_ELEMENT_MOVEMENT;

            string actual1;
            string actual2;
            bool result1 = queue.TryDequeue(out actual1);
            bool result2 = queue.TryDequeue(out actual2);

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        [Test]
        public void Enqueue_InsertLowMediumMediumHighPriorityElements_GetElementsBackInOrder()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);

            queue.Enqueue(VALID_ELEMENT_CAM_ON, QueuePriorities.Low);
            queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.Medium);
            queue.Enqueue(VALID_ELEMENT_CAM_ON, QueuePriorities.Medium);
            queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.High);

            string expected1 = VALID_ELEMENT_MOVEMENT;
            string expected2 = VALID_ELEMENT_CAM_ON;
            string expected3 = VALID_ELEMENT_MOVEMENT;
            string expected4 = VALID_ELEMENT_CAM_ON;

            string actual1;
            string actual2;
            string actual3;
            string actual4;

            bool result1 = queue.TryDequeue(out actual1);
            bool result2 = queue.TryDequeue(out actual2);
            bool result3 = queue.TryDequeue(out actual3);
            bool result4 = queue.TryDequeue(out actual4);

            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
            Assert.AreEqual(expected3, actual3);
            Assert.AreEqual(expected4, actual4);
        }

        [Test]
        public void Enqueue_InsertOneBeyondMaximumCapacity_QueueContainsOneElement()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);

            for(int i = 0; i < (MaxCapacity + 1); i++)
            {
                queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.High);
            }

            uint expected = 1;
            uint actual = queue.Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Enqueue_InsertToMaximumCapacity_QueueContainsAllElements()
        {
            IQueue queue = new PriorityQueue(MaxCapacity);
            

            for (int i = 0; i < MaxCapacity; i++)
            {
                queue.Enqueue(VALID_ELEMENT_MOVEMENT, QueuePriorities.Medium);
            }

            uint expected = MaxCapacity;
            uint actual = queue.Count;
            Assert.AreEqual(expected, actual);
        }

        public void InsertIntoQueue(IQueue queue, QueuePriorities priority)
        {
            queue.Enqueue(VALID_ELEMENT_MOVEMENT, priority);
        }

        public string RemoveFromQueue(IQueue queue)
        {
            string result = null;
            queue.TryDequeue(out result);            
            return result;
        }
    }
}
