using System;
using System.Collections;
using System.Collections.Generic;
using MarsRoverClient.Log;

namespace MarsRoverClient.Log
{
    public interface LogEventObserver
    {
        void RefreshLogList();
    }
}
