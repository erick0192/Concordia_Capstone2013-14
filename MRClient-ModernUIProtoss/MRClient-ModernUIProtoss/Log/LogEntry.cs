using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRClient_ModernUIProtoss.Log
{
    public class LogEntry
    {
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public LogLevel Level { get; set; }

        public LogEntry(string iMessage, DateTime iTime, LogLevel iLevel)
        {
            Message = iMessage;
            Time = iTime;
            Level = iLevel;
        }

        public LogEntry(string iMessage, LogLevel iLevel)
        {
            Message = iMessage;
            Time = DateTime.Now;
            Level = iLevel;
        }

        public override string ToString()
        {
            string format = "dd/mm/yy HH:mm";
            return ToString(format);    
        }

        public string ToString(string iTimeDateFormat)
        {
            return String.Format("{0} | {1} | {2}", Time.ToString(iTimeDateFormat), Level.ToString(), Message);
        }
    }
}
