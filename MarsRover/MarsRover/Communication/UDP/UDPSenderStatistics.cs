using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;


namespace MarsRover
{
    public class LocalUDPStatistics
    {
        private System.Timers.Timer FpsTimer;
        private UDPSender Sender;
        private float RateInBps;
        private float RateInKBps;
        private float RateInMBps;
        private int PreviousNumberOfFrames;
        private int TimerResolutionMiliSec;

        public LocalUDPStatistics(UDPSender aSender, int aTimerResolutionMiliSec)
        {
            Sender = aSender;
            TimerResolutionMiliSec = aTimerResolutionMiliSec;
            PreviousNumberOfFrames = 0;

            FpsTimer = new System.Timers.Timer();
            FpsTimer.Interval = TimerResolutionMiliSec;
            FpsTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            FpsTimer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            float DeltaFrames = Sender.GetTotalNbDataINOUT() - PreviousNumberOfFrames;

            RateInBps = (float)(DeltaFrames / (float)(TimerResolutionMiliSec / 1000.0f));
            RateInKBps = RateInBps / 1024.0f;
            RateInMBps = RateInKBps / 1024.0f;

            PreviousNumberOfFrames = Sender.GetTotalNbDataINOUT();

            PrintInfo();
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
            //Console.WriteLine("Rate Bps:" + RateInBps);
            //Console.WriteLine("Rate KBbs:" + RateInKBps);
            Console.WriteLine("Rate MBps:" + RateInMBps);
        }
    }
}
