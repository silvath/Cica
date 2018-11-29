using Cica.CicaClient;
using Cica.CicaMessage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CicaClientConsole
{
    public class Program
    {
        public static int? _second = null;
        public static int _secondCount = 0;
        public static void Main(string[] args)
        {
            Client client = new Client();
            Client.CommandEvent += ExecuteCommand;
            Client.ImageEvent += DrawImage;
            //Client.LogEvent += WriteLog;
            bool isRunning = true;
            ConsoleWrite("Cica Client");
            while (isRunning) 
            {
                client.Send(Console.ReadLine());
                isRunning = !client.Close;
            }
        }

        public static void ConsoleWrite(string message)
        {
            Console.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("hh:mm:ss"), message));
        }

        public static void WriteLog(string message)
        {
            ConsoleWrite(string.Format("Log: {0}", message));
        }

        public static void ExecuteCommand(Command command) 
        {
            if (command.Type == CommandType.Talk)
                ExecuteCommandTask(command);
            else if (command.Type == CommandType.SessionCreated)
                ExecuteCommandSessionCreated(command);
            else if (command.Type == CommandType.SessionListed)
                ExecuteCommandSessionListed(command);
            else
                ConsoleWrite(string.Format("NOT IMPLEMENTED CMD: {0}", command.ToString()));
        }

        public static void ExecuteCommandTask(Command command) 
        {
            ConsoleWrite(command.ToString());
        }

        public static void ExecuteCommandSessionCreated(Command command)
        {
            ConsoleWrite(string.Format("Session Created: {0}",command.Parameters[0]));
        } 

        public static void ExecuteCommandSessionListed(Command command)
        {
            ConsoleWrite("Session List:");
            for (int i = 0; i < command.Parameters.Count; i = i + 2)
                ConsoleWrite(string.Format("{0} : {1}", command.Parameters[i], command.Parameters[i + 1]));
        }

        private static void DrawImage(Image image) 
        {
            int second = DateTime.Now.Second;
            if (!_second.HasValue)
            {
                _second = second;
                _secondCount = 0;
            }else{
                if (_second.Value != second)
                {
                    ConsoleWrite(_secondCount.ToString());
                    _second = second;
                    _secondCount = 0;
                }else {
                    _secondCount++;
                }
            }
        }
    }
}
