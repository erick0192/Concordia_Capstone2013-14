using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Streams
{
    public interface IVideoStreamManager<T>
    {
        T GetFrontCameraStream();
        T GetBackCameraStream();
        T GetLeftCameraStream();
        T GetRightCameraStream();
    }
}
