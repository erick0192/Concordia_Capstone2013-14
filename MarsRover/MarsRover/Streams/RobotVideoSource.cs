//Class to get the video stream from the robot using our communication protocols

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;

namespace MarsRover.Streams
{
    
    public class RobotVideoSource: IVideoSource
    {
        public long BytesReceived
        {
            get { throw new NotImplementedException(); }
        }

        public int FramesReceived
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsRunning
        {
            get { throw new NotImplementedException(); }
        }

        public event NewFrameEventHandler NewFrame;

        public event PlayingFinishedEventHandler PlayingFinished;

        public void SignalToStop()
        {
            throw new NotImplementedException();
        }

        public string Source
        {
            get { throw new NotImplementedException(); }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event VideoSourceErrorEventHandler VideoSourceError;

        public void WaitForStop()
        {
            throw new NotImplementedException();
        }
    }
}
