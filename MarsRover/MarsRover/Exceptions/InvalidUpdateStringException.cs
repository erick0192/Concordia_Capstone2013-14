using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Exceptions
{
    public class InvalidUpdateStringException : Exception
    {
        public InvalidUpdateStringException()
            : base("The update string provided is invalid.") { }

        public InvalidUpdateStringException(String message) : base(message) { }

        public InvalidUpdateStringException(String updateString, String message) : base("The update string \'" + updateString + "\' is invalid." + message) { }

        public InvalidUpdateStringException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public InvalidUpdateStringException(string message, Exception innerException)
            : base(message, innerException) { }

        public InvalidUpdateStringException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        public InvalidUpdateStringException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
