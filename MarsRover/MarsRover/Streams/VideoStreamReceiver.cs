//Class to get the video stream from the robot using our communication protocols

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video;
using NLog;

namespace MarsRover.Streams
{    
    public class VideoStreamReceiver: IVideoSource
    {
        #region Members

        private int port;
        private IPAddress sourceIPAddress;
        private UdpClient udpClient;
        private IPEndPoint udp_ep;

        private const int chunckSize = 500;
        private int totalJPEGsize;
        private byte[] JPEGchuck = new byte[chunckSize];
        private int currentChuckNumber;
        private int ChuckNumber_expected = 0;
        private int numOfChucks;
        private int jpegNumber_expected;
        private List<byte[]> entireJPEG = new List<byte[]>();

        private Logger logger = LogManager.GetCurrentClassLogger();
        private Timer sendCommandTimer;

        #endregion

        #region Properties

        private long bytesReceived;
        public long BytesReceived
        {
            get
            {
                long br = bytesReceived;
                bytesReceived = 0;
                return br;
            }
        }

        private int framesReceived;
        public int FramesReceived
        {
            get 
            {
                int fr = framesReceived;
                framesReceived = 0;
                return fr; 
            }
        }

        private volatile bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
        }

        #endregion

        #region Events

        public event NewFrameEventHandler NewFrame;

        public event PlayingFinishedEventHandler PlayingFinished;

        #endregion

        #region Constructor

        public VideoStreamReceiver(String sourceIPAddress, int port)
        {
            this.sourceIPAddress = IPAddress.Parse(sourceIPAddress);
            this.port = port;           
            sendCommandTimer = new Timer(new TimerCallback(this.SendCommand), null, 0, 1000);
        }

        #endregion

        #region Methods

        private void StartListening()
        {
            byte[] data = new byte[512];
            udp_ep = new IPEndPoint(sourceIPAddress, this.port);
            udpClient = new UdpClient(this.port);
            //udpClient.Connect(udp_ep);            

            try
            {
                while (isRunning)
                {
                    data = udpClient.Receive(ref udp_ep);
                    bytesReceived += data.LongLength;
                    ReceiveData(data);
                }

                if (PlayingFinished != null)
                {
                    if (!isRunning)
                    {
                        PlayingFinished(this, ReasonToFinishPlaying.StoppedByUser);
                    }
                    else
                    {
                        PlayingFinished(this, ReasonToFinishPlaying.DeviceLost);
                        if (VideoSourceError != null)
                        {
                            VideoSourceError(this, new VideoSourceErrorEventArgs("Lost connection to UDP stream."));
                        }
                    }
                }
            }
            catch (SocketException se)
            {
                //Log error
                logger.Log(LogLevel.Error, se.Message);
                if (VideoSourceError != null)
                {
                    VideoSourceError(this, new VideoSourceErrorEventArgs(se.Message));
                }
            }
            finally
            {
                udpClient.Close();
                isRunning = false;
            }
        }

        private void ReceiveData(byte[] data)
        {
            totalJPEGsize = BitConverter.ToInt32(data, 4);

            //Gathers all chucks forming a JPEG.
            //If a chuck is missing, entire JPEG is invalid
            int jpegNumber = BitConverter.ToInt32(data, 0);
            currentChuckNumber = BitConverter.ToInt32(data, 8);

            //first chuck of the JPEG? new JPEG time!
            if (currentChuckNumber == 0)
            {

                jpegNumber_expected = jpegNumber;
                ChuckNumber_expected = 0;
                totalJPEGsize = BitConverter.ToInt32(data, 4);
                numOfChucks = (totalJPEGsize / chunckSize) + 1;

            }
            else if (jpegNumber_expected != jpegNumber)
            {
                return;
            }

            //is it the chunck we expect to get?
            if (currentChuckNumber == ChuckNumber_expected)
            {
                byte[] jpegByteArray = new byte[data.Length - 12];
                Array.Copy(data, 12, jpegByteArray, 0, data.Length - 12);
                entireJPEG.Add(jpegByteArray);

                //last chunck means we're finishd 
                if ((numOfChucks - 1) == currentChuckNumber)
                {
                    
                    byte[] retrievedBytes = new byte[(entireJPEG.Count - 1) * chunckSize + entireJPEG[entireJPEG.Count - 1].Length];
                    retrievedBytes = entireJPEG.SelectMany(a => a).ToArray();
                   
                    System.Drawing.Bitmap tempbit = (System.Drawing.Bitmap)((new System.Drawing.ImageConverter()).ConvertFrom(retrievedBytes));
                    NewFrameEventArgs args = new NewFrameEventArgs(tempbit);
                    
                    if(NewFrame != null)
                    {
                        NewFrame(this, args);
                    }

                    framesReceived++;
                    entireJPEG.Clear();      
                }

                ChuckNumber_expected++;
            }
            //if not.. we'll have to wait until the next JPEG
            else
            {
                ChuckNumber_expected = -1;
                return;
            }
        }

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
            isRunning = true;
            
            Thread t = new Thread(new ThreadStart(StartListening));
            t.Start();
            
        }

        private void SendCommand(Object o)
        {
           if(isRunning)
           {
                //spam start command
           }
           else
           {
                //spam stop command
           }
        }

        public void Stop()
        {
            isRunning = false;
            udpClient.Close();
        }

        public event VideoSourceErrorEventHandler VideoSourceError;

        public void WaitForStop()
        {
            while (isRunning) ;
        }

        #endregion
    }
}
