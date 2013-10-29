﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;

namespace MarsRover.Streams
{
    public class WebCamStreamManager: IVideoStreamManager<IVideoSource>
    {
        #region Private Members
        
        private FilterInfoCollection mVideoDevices;
        private IVideoSource mFrontCameraStream;
        private IVideoSource mBackCameraStream;
        private IVideoSource mLeftCameraStream;
        private IVideoSource mRightCameraStream;

        #endregion

        #region Properties

        private static WebCamStreamManager mInstance;
        public static WebCamStreamManager Instance
        {
            get
            {
                if (null == mInstance)
                    mInstance = new WebCamStreamManager();

                return mInstance;
            }
        }

        #endregion

        private WebCamStreamManager()
        {
            mVideoDevices = new FilterInfoCollection(
                    FilterCategory.VideoInputDevice);
        }

        public IVideoSource GetFrontCameraStream()
        {
            if (null == mFrontCameraStream)
            {
                mFrontCameraStream = new VideoCaptureDevice(
                    mVideoDevices[0].MonikerString);
            }

            return mFrontCameraStream;
        }

        public IVideoSource GetBackCameraStream()
        {
            if (null == mBackCameraStream)
            {
                mBackCameraStream = new DummyVideoSource();
            }

            return mBackCameraStream;
        }

        public IVideoSource GetLeftCameraStream()
        {
            if (null == mLeftCameraStream)
            {
                mLeftCameraStream = new DummyVideoSource();
            }

            return mLeftCameraStream;
        }

        public IVideoSource GetRightCameraStream()
        {
            if(null == mRightCameraStream)
            {
                mRightCameraStream = new DummyVideoSource();
            }

            return mRightCameraStream;
        }

        //Make sure to call this method when the application is closing.
        //Otherwise, streams will be left open and the appliction will not be shut down, even though the GUI has
        public void StopAllStreams()
        {
            if (null != mFrontCameraStream && mFrontCameraStream.IsRunning)
                mFrontCameraStream.SignalToStop();

            if (null != mBackCameraStream && mBackCameraStream.IsRunning)
                mBackCameraStream.SignalToStop();

            if (null != mLeftCameraStream && mLeftCameraStream.IsRunning)
                mLeftCameraStream.SignalToStop();

            if (null != mRightCameraStream && mRightCameraStream.IsRunning)
                mRightCameraStream.SignalToStop();
        }
    }
}