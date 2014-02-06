using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RoverOperator.Log
{
    public class LogEventSubject
    {
        public static ObservableCollection<LogEvent> Events = new ObservableCollection<LogEvent>();
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
                BindingOperations.EnableCollectionSynchronization(Events, _lock);
                Initialized = true;
            }

            LogEvent newEvent = new LogEvent(longdate, level, callsite, message);

            //If event is added from non-UI thread
            App.Current.Dispatcher.Invoke((Action)delegate {
                Events.Add(newEvent);
            });

            foreach (LogEventObserver observer in observers)
            {
                observer.RefreshLogList();
            }
        }
    }
}
