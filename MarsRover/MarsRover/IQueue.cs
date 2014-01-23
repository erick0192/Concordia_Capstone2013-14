using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover
{
    public interface IQueue
    {
        uint Count { get; }

        bool TryDequeue(out string result);
        void Enqueue(string element, QueuePriorities priority = QueuePriorities.Medium);
        void ClearQueue();
    }

    public enum QueuePriorities { Highest, High, Medium, Low, Lowest };
}
