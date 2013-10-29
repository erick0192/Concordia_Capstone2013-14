using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverClient.Log
{
    public class LogEventSubject
    {
        private static ArrayList observers = new ArrayList();

        public static void Attach(LogEventObserver observer)
        {
            observers.Add(observer);
        }

        public static void Detach(LogEventObserver observer)
        {
            observers.Remove(observer);
        }

        public static void Notify(string longdate, string level, string callsite, string message)
        {
            foreach (LogEventObserver observer in observers)
            {
                observer.UpdateLogList(longdate, level, callsite, message);
            }
        }
    }
}
