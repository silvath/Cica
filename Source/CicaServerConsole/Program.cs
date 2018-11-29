using Cica.CicaServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaServerConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Server server = Server.CreateInstance();
            Server.LogEvent += ConsoleWrite;
            server.Start();
            while (server.IsRunning) 
            {
                System.Threading.Thread.Sleep(5000);
            }
        }

        public static void ConsoleWrite(string message) 
        {
            Console.WriteLine(string.Format("{0} - {1}",DateTime.Now.ToString("hh:mm:ss"), message));
        }
    }
}
