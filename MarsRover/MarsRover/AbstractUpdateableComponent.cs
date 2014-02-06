using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRover.Commands;

namespace MarsRover
{
    public abstract class AbstractUpdateableComponent
    {
        protected string regex = string.Empty;

        public abstract string UpdateIdentifier { get; }

        #region Delegates and Events

        public delegate void ComponentUpdatedDelegate(AbstractUpdateableComponent component);
        public event ComponentUpdatedDelegate ComponentedUpdated;

        #endregion

        #region Methods

        protected bool IsValidUpdateString(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, regex);
        }
        
        protected static string[] GetUpdateStringArrayWithoutIdentifier(string updateString)
        {
            int posIdentifer = updateString.IndexOf(CommandMetadata.Update.StartOfValuesIdentifier);                    
            int length = updateString.Length - posIdentifer - 2;
            
            return updateString.Substring(posIdentifer + 1, length).Split(CommandMetadata.Update.ValuesDelimiter);
        }

        public static string GetUpdateIdentifierFromUpdateString(string updateString)
        {
            return updateString.Substring(1, updateString.IndexOf(CommandMetadata.Update.StartOfValuesIdentifier) - 1);
        }

        public abstract void UpdateFromString(string updateString);
        public abstract string GetUpdateString();

        protected virtual string CreateUpdateString(params object[] values)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CommandMetadata.StartDelimiter);
            sb.Append(UpdateIdentifier + CommandMetadata.Update.StartOfValuesIdentifier);
            foreach(object value in values)
            {
                sb.Append(value.ToString() + CommandMetadata.Update.ValuesDelimiter);
            }
            sb.Remove(sb.Length - 1, 1);//Remove the last values delimiter
            sb.Append(CommandMetadata.EndDelimiter);

            return sb.ToString();
        }

        #endregion
    }
}
