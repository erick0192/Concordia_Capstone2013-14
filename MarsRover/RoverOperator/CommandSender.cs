using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarsRover;
using MarsRover.Commands;

namespace RoverOperator
{
    public class CommandSender
    {
        private ConcurrentDictionary<string, string> commands;
        private UDPSender udpSender;
        private static object syncRoot = new Object();

        private const int SLEEP_TIME = 200;

        private static volatile CommandSender instance;
        public static CommandSender Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new CommandSender();
                        }
                    }
                }

                return instance;
            }
        }

        private CommandSender()
        {
            commands = new ConcurrentDictionary<string, string>();
            commands.TryAdd(CommandMetadata.Camera.Identifier, string.Empty);
            commands.TryAdd(CommandMetadata.Movement.LeftIdentifier, string.Empty);
            commands.TryAdd(CommandMetadata.Movement.RightIdentifier, string.Empty);            
            commands.TryAdd(CommandMetadata.Pan.Identifier, string.Empty);
            commands.TryAdd(CommandMetadata.Tilt.Identifier, string.Empty);

            udpSender = new UDPSender(Properties.NetworkSettings.Default.RoverIPAddress, Properties.NetworkSettings.Default.CommandsPort);
            Thread t = new Thread(SendCommands);
            t.IsBackground = true;
            t.Start();
        }

        public void UpdateCommand(string command)
        {
            string id = GetCommandId(command);

            commands[id] = command;     
        }


        private void SendCommands()
        {
            while (true)
            {
                foreach(KeyValuePair<string,string> command in commands)
                {
                    udpSender.SendStringNow(command.Value);
                }

                Thread.Sleep(SLEEP_TIME);
            }
        }

        private string GetCommandId(string command)
        {
            return command.Substring(CommandMetadata.IdIndex, CommandMetadata.IdLength);
        }
    }
}
