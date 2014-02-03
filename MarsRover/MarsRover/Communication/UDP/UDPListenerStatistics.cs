using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;


namespace MarsRover
{
    public class UDPListenerStatistics
    {
        private System.Timers.Timer FpsTimer;
        private UDPListener Listener;
        private float RateInBps;
        private float RateInKBps;
        private float RateInMBps;
        private int PreviousNumberOfFrames;
        private int TimerResolutionMiliSec;

        public UDPListenerStatistics( UDPListener aListener, int aTimerResolutionMiliSec)
        {
            Listener = aListener;
            TimerResolutionMiliSec = aTimerResolutionMiliSec;
            PreviousNumberOfFrames = 0;

            FpsTimer = new System.Timers.Timer();
            FpsTimer.Interval = TimerResolutionMiliSec;
            FpsTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            FpsTimer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            float DeltaFrames = Listener.GetTotalNbDataINOUT() - PreviousNumberOfFrames;

            RateInBps = (float)(DeltaFrames / (float)(TimerResolutionMiliSec / 1000.0f));
            RateInKBps = RateInBps / 1024.0f;
            RateInMBps = RateInKBps / 1024.0f;

            PreviousNumberOfFrames = Listener.GetTotalNbDataINOUT();

            //PrintInfo();
        }

        public float GetCalculatedRateInBps()
        {
            return RateInBps;
        }

        public float GetCalculatedRateInKBps()
        {
            return RateInKBps;
        }

        public float GetCalculatedRateInMBps()
        {
            return RateInMBps;
        }

        public void Start()
        {
            PreviousNumberOfFrames = 0;
            FpsTimer.Start();
        }

        public void Stop()
        {
            PreviousNumberOfFrames = 0;
            FpsTimer.Stop();
        }

        public void PrintInfo()
        {
            if (RateInBps <= 1024)
            {
                Console.WriteLine("Rate Bps:" + RateInBps);
            }
            else if (RateInBps >= 1024 && RateInBps < 1024 * 1024)
            {
                Console.WriteLine("Rate KBbs:" + RateInKBps);
            }
            else
            {
                Console.WriteLine("Rate MBps:" + RateInMBps);
            }
        }
    }
}
