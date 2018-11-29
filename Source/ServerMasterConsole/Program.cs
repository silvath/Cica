using Engine;
using Server;
using ServerWindows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMasterConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            ListennerWindows listenner = new ListennerWindows(ConfigurationManager.AppSettings["MasterAddress"], Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]));
            ListennerWindows.LogEvent += ListennerConsoleWrite; 
            CicaServerMaster server = new CicaServerMaster(listenner, new PackageManager());
            CicaServerMaster.LogEvent += ConsoleWrite;
            server.Start();
            while (server.IsRunning) 
            {
                server.Update();
                System.Threading.Thread.Sleep(10);
            }
        }

        private static void ListennerConsoleWrite(string message)
        {
            ConsoleWrite(string.Format("Listenner - {0}", message));
        }

        public static void ConsoleWrite(string message)
        {
            Console.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("hh:mm:ss"), message));
        }
    }
}
