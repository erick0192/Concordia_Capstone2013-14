using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MarsRoverClient.Log
{
    public class LogEventSubject
    {
        public static ObservableCollection<LogEvent> Events;
        private static object _lock = new object();
        private static ArrayList observers = new ArrayList();
        private static bool Initialized { get; set; }

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
            if (!Initialized)
            {
                Events = new ObservableCollection<LogEvent>();
                BindingOperations.EnableCollectionSynchronization(Events, _lock);
                Initialized = true;
            }

            LogEvent newEvent = new LogEvent(longdate, level, callsite, message);
            Events.Add(newEvent);
            foreach (LogEventObserver observer in observers)
            {
                observer.RefreshLogList();
            }
        }
    }
}
