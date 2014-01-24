using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rover
{
  
    public class TCPServer
    {
        public delegate void ReceivedCBType(TcpClient client, byte[] message, int bytesRead);
        public delegate void TransmittedCBType(TcpClient Tcpclient);
        public delegate void ClientFoundCBType(TcpClient client);
        public delegate void ClientLostCBType(TcpClient client);

        private TcpListener tcpListener;
        private Thread ServerListeningThread;    
    
        private ReceivedCBType aReceivedCBHandler;
        private TransmittedCBType aTransmittedCBHandler;
        private ClientFoundCBType aClientFoundCBHandler;
        private ClientLostCBType aClientLostCBHandler;

        private ArrayList TcpClients = new ArrayList();
        private ArrayList RxTCPClientThread = new ArrayList();

        /// <summary>
        /// TCP server constuctor, this instanciate the server on the specified port.
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="aTransmittedCBHandler"></param>
        /// <param name="aReceivedCBHandler"></param>
        /// <param name="aClientFoundCBHandler"></param>
        /// <param name="aClientLostCBHandler"></param>
        public TCPServer(int Port, TransmittedCBType aTransmittedCBHandler, ReceivedCBType aReceivedCBHandler, ClientFoundCBType aClientFoundCBHandler, ClientLostCBType aClientLostCBHandler)
        {
            this.tcpListener = new TcpListener(IPAddress.Any, Port);
            this.ServerListeningThread = new Thread(new ThreadStart(ServerListeningThreadHandler));
           
            this.aReceivedCBHandler = aReceivedCBHandler;
            this.aTransmittedCBHandler = aTransmittedCBHandler;
            this.aClientFoundCBHandler = aClientFoundCBHandler;
            this.aClientLostCBHandler = aClientLostCBHandler;

            //start server listening thread
            this.ServerListeningThread.Start();
        }

        /// <summary>
        /// Get all connected clients to the server
        /// </summary>
        /// <returns></returns>
        public ArrayList GetConnectedClients()
        {
            return TcpClients;
        }

        /// <summary>
        /// Get the client ID of the specified client.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetClientId(TcpClient client)
        {
            return TcpClients.IndexOf(client);
        }

        /// <summary>
        /// Print the client information
        /// </summary>
        /// <param name="client"></param>
        public void PrintClientInfo(TcpClient client)
        {
            Console.WriteLine("Address Family = " + client.Client.AddressFamily.ToString() +
                              "\nSocketType = " + client.Client.SocketType.ToString() +
                              "\nProtocolType = " + client.Client.ProtocolType.ToString());
            Console.WriteLine("RemoteEndPoint IP: " + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) +
                              " on Port: " + ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());
            Console.WriteLine("LocalEndPoint IP :" + IPAddress.Parse(((IPEndPoint)client.Client.LocalEndPoint).Address.ToString()) +
                              " on Port " + ((IPEndPoint)client.Client.LocalEndPoint).Port.ToString());

        }

        /// <summary>
        /// Print all client information
        /// </summary>
        public void PrintAllClientInfo()
        {
            Console.WriteLine("\n" + TcpClients.Count + " Clients are active\n");
            for (int i = 0; i < TcpClients.Count; i++)
            {
                Console.WriteLine("\nClient " + i);
                PrintClientInfo((TcpClient)TcpClients[i]);
            }
        }

        /// <summary>
        /// Send data to the specified client.
        /// </summary>
        /// <param name="client"></param> Client object from the list of existing client
        /// <param name="message"></param> The message to send
        /// <param name="size"></param> The size of the message
        public void SendData(object client, byte[] message, int size)
        {
            TcpClient Tcpclient = (TcpClient)client;
            NetworkStream clientStream = Tcpclient.GetStream();

            clientStream.Write(message, 0, message.Length);
            clientStream.Flush();

            if (aTransmittedCBHandler != null)
            {
                aTransmittedCBHandler(Tcpclient);
            }
        }

        /// <summary>
        /// Internal Listen thread, each time a server is created a listening thread is also created.
        /// </summary>
        private void ServerListeningThreadHandler()
        {
            //Stat the TCP listener.
            this.tcpListener.Start();

            while (true)
            {      
                //This method BLOCKS....
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //Add the new client to the client arraylist
                TcpClients.Add(client);
                
                //Trigger the event Clienf found
                if (aClientFoundCBHandler != null)
                {
                    aClientFoundCBHandler(client);
                }

                //create a thread to handle received data from the new client
                //Each time a new client is connecting a new thread is created and started.
                Thread ReceiveThread = new Thread(new ParameterizedThreadStart(ReceiveThreadHandler));
                RxTCPClientThread.Add(ReceiveThread);
                ReceiveThread.Start(client);
                
            }
        }
    
        /// <summary>
        /// Handles the data received from a client.
        /// </summary>
        /// <param name="client"></param>
        private void ReceiveThreadHandler(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message                    
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                if (aReceivedCBHandler != null)
                {
                    aReceivedCBHandler(tcpClient, message, bytesRead);
                }
            }

            //Join the client thread
            ((Thread)RxTCPClientThread[TcpClients.IndexOf(client)]).Join() ;      
            //remove the thread from the thread arraylist
            RxTCPClientThread.RemoveAt(TcpClients.IndexOf(client));
            //remove the client from the client arraylist
            TcpClients.Remove(client);
            //notify the user application
            if (aClientLostCBHandler != null)
            {
                aClientLostCBHandler(tcpClient);
            }
            //close the client socket.                 
            tcpClient.Close();
        }
    }
}
