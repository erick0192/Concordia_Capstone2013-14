using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover
{
    public interface ISensorLog //change RawCommand to RawData.
    {
        string RawCommand { get; }
        string Identifier { get; }
        bool IsUpdated { get; }

        void LogData();
        void UpdateValues();
        void ReconstructCommand();
    }
}
