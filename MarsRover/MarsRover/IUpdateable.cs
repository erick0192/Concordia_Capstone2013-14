using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover
{
    public interface IUpdateable
    {
        /// <summary>
        /// Property to hold a regular expression
        /// </summary>
        String RegEx { get; set; }

        /// <summary>
        /// Indicates whether the regular expression referenced by the property RegEx 
        /// finds a match in a specified input string.
        /// </summary>
        /// <param name="input">The string to search for a match</param>
        /// <returns>true if the regular expression finds a match; otherwise, false.</returns>
        bool IsMatch(string input);

        /// <summary>
        /// Updates the current object using the string passed through updateString
        /// </summary>
        /// <param name="updateString">The string that contains update information</param>
        void UpdateFromString(string updateString);

        /// <summary>
        /// Creates a string containing the values of the object's properties
        /// </summary>
        /// <returns>The update string</returns>
        string GetUpdateString();
    }
}
