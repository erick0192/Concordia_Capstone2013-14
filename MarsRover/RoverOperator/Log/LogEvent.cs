using System;

namespace RoverOperator.Log
{
    public class LogEvent
    {
        public string Date { get; set; }
        public string Level { get; set; }
        public string Callsite { get; set; }
        public string Message { get; set; }

        public LogEvent(string longdate, string level, string callsite, string message)
        {
            this.Date = longdate;
            this.Level = level;
            this.Callsite = callsite;
            this.Message = message;
        }
    }
}
