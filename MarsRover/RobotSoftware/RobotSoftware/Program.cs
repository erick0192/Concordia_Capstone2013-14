using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotSoftware
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ICommand> commandList = new List<ICommand>();
            commandList.Add(new MovementCommand('F', 255, 'F', 255));
            commandList.Add(new MovementCommand('F', 200, 'B', 150));
            commandList.Add(new MovementCommand("<MF222F222>"));

            foreach (ICommand command in commandList){
                command.Execute();
                Console.Write("\n");
                command.UnExecute();
                Console.Write("\n");
            }

            Console.WriteLine("-----------------------------");

            CommandFactory factory = new CommandFactory();

            ICommand testMovementCommand = factory.CreateCommand("<MF255F255>");
            ICommand testCommand2 = factory.CreateCommand("<MF020F020>");


            testMovementCommand.Execute();
            testCommand2.Execute();

            Console.Read();
        }
    }

}
