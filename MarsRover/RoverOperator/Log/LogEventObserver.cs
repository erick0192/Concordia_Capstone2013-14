using System;
using System.Collections;
using System.Collections.Generic;
using RoverOperator.Log;

namespace RoverOperator.Log
{
    public interface LogEventObserver
    {
        void RefreshLogList();
    }
}
