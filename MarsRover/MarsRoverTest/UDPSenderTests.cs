using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MarsRover;
using NUnit.Framework;

namespace MarsRoverTest
{
    class UDPSenderTests
    {
       

        [Test]
        public void SendBytesNow_TestWith1Byte()
        {
            UDPSender us = new UDPSender("127.0.0.1", 7777);
            
            byte[] data =  new byte[1];

            us.SendBytesNow(data, 1);
            
            Assert.AreEqual(us.GetTotalNbDataINOUT(), 1);
                
        }

        [Test]
        public void SendBytesNow_TestWith1K()
        {
            UDPSender us = new UDPSender("127.0.0.1", 7777);

            byte[] data = new byte[1024];
            
            us.SendBytesNow(data, 1024);

            Assert.AreEqual(us.GetTotalNbDataINOUT(), 1024);

        }

        [Test]
        public void SendBytesNow_TestWith4K()
        {
            UDPSender us = new UDPSender("127.0.0.1", 7777);

            byte[] data = new byte[1024 * 4];

            us.SendBytesNow(data, 1024 * 4);

            Assert.AreEqual(us.GetTotalNbDataINOUT(), 1024 * 4);

        }

        [Test]
        public void SendBytesNow_TestWith10M_BufferOverflow()
        {
            UDPSender us = new UDPSender("127.0.0.1", 7777);

            byte[] data = new byte[1024 *1024 *10];

            us.SendBytesNow(data, 1024*1024*10);

            Assert.AreEqual(us.GetTotalNbDataINOUT(),0);

        }

        [Test]
        public void SendStringNow_TestWithSring()
        {
            UDPSender us = new UDPSender("127.0.0.1", 7777);

            string t = "I am testing the udp sender SendStringNow method";

            us.SendStringNow(t);

            Assert.AreEqual(us.GetTotalNbDataINOUT(), t.Length) ;

        }

        [Test]
        public void SendBytesNow_LoopBack1k()
        {
            UDPSender us = new UDPSender("127.0.0.1", 7778);

            byte[] data = new byte[1024 *1];

            UdpClient listener = new UdpClient(7778);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 7778);

            //Blocking Call
            us.SendBytesNow(data, 1024 * 1);
            
            //BLOCKING CALL                    
            byte[] bytes = listener.Receive(ref groupEP);

            Assert.AreEqual(us.GetTotalNbDataINOUT(), bytes.Length);

            listener.Close();
        }
       

        [Test]
        public void SendBytesNow_LoopBack_SequenceNumber1K()
        {
            int i;
            bool result = false;

            UDPSender us = new UDPSender("127.0.0.1", 7770);

            byte[] data = new byte[1024 * 1];

            for ( i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }

            UdpClient listener = new UdpClient(7770);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 7770);

            //Blocking Call
            us.SendBytesNow(data, 1024 * 1);

            //BLOCKING CALL                    
            byte[] bytes = listener.Receive(ref groupEP);

            for (i = 0; i < data.Length; i++)
            {
                if (bytes[i] != (byte)i)
                {
                    Assert.True(false);
                }
            }

            Assert.True(true);
            
            listener.Close();
           
        }

        [Test]
        public void SendBytesNow_LoopBack_SequenceNumber16K()
        {
            int i;
            bool result = false;

            UDPSender us = new UDPSender("127.0.0.1", 7770);

            byte[] data = new byte[1024 * 16];

            for (i = 0; i < data.Length; i++)
            {
                data[i] = (byte)i;
            }

            UdpClient listener = new UdpClient(7770);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 7770);

            //Blocking Call
            us.SendBytesNow(data, 1024 * 16);

            //BLOCKING CALL                    
            byte[] bytes = listener.Receive(ref groupEP);

            for (i = 0; i < data.Length; i++)
            {
                if (bytes[i] != (byte)i)
                {
                    Assert.True(false);
                }
            }

            Assert.True(true);

            listener.Close();

        }
    }
}
