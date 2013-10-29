using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverClient.Log
{
    public interface LogEventObserver
    {
        void UpdateLogList(string longdate, string level, string callsite, string message);
    }
}
