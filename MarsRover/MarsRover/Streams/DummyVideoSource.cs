using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;

namespace MarsRover.Streams
{
    public class DummyVideoSource : IVideoSource
    {
        public long BytesReceived
        {
            get { throw new NotImplementedException(); }
        }

        public int FramesReceived
        {
            get { throw new NotImplementedException(); }
        }

        private bool mIsRunning = false;
        public bool IsRunning
        {
            get { return mIsRunning; }
        }

        public event NewFrameEventHandler NewFrame;

        public event PlayingFinishedEventHandler PlayingFinished;

        public void SignalToStop()
        {
            Stop();
        }

        public string Source
        {
            get { throw new NotImplementedException(); }
        }

        public void Start()
        {
            mIsRunning = true;
        }

        public void Stop()
        {
            mIsRunning = false;
            if (null != PlayingFinished)
            {
                PlayingFinished(this, new ReasonToFinishPlaying());
            }
        }

        public event VideoSourceErrorEventHandler VideoSourceError;

        public void WaitForStop()
        {
            throw new NotImplementedException();
        }
    }
}
