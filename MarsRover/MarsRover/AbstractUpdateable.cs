using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover
{
    public abstract class AbstractUpdateable
    {
        protected string regex = string.Empty;

        protected bool IsValidUpdateString(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, regex);
        }

        protected static string[] GetUpdateStringArrayWithoutIdentifier(string updateString, char identifier = ';')
        {
            int posIdentifer = updateString.IndexOf(identifier);                    
            int length = updateString.Length - posIdentifer - 2;

            return updateString.Substring(posIdentifer + 1, length).Split(',');
        }

        public abstract void UpdateFromString(string updateString);

        public abstract string GetUpdateString();
    }
}
