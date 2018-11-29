using Data;
using Engine;
using EngineWindows;
using Server;
using ServerWindows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PackageManager packer = new PackageManager();
            NetworkManager network = new NetworkManager(new NetworkWindows(), packer, ConfigurationManager.AppSettings["MasterHost"], Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]));
            ScriptParser parser = new ScriptParser();
            QueryManager query = new QueryManager(new QueryWindows());
            ListennerWindows listenner = new ListennerWindows(query.Get(QueryType.AddressInternal), Int32.Parse(ConfigurationManager.AppSettings["Port"]));
            ListennerWindows.LogEvent += ListennerConsoleWrite;
            GameEngine engine = new GameEngine(new ResourceManager(new StorageWindows(StorageWindows.GetPathFolderResources())), new Canvas(new CanvasWindows()), network, query);
            engine.Type = GameEngineType.Server;
            CicaServerSession server = new CicaServerSession(engine, ConfigurationManager.AppSettings["Game"], ConfigurationManager.AppSettings["Name"],Int32.Parse(ConfigurationManager.AppSettings["MaxSlots"]), listenner, new ResourceManager(new StorageWindows(StorageWindows.GetPathFolderResources())), network, packer);
            CicaServerSession.LogEvent += ConsoleWrite;
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
