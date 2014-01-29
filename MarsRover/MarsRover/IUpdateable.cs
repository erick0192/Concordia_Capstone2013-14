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
