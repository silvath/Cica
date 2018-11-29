using Client;
using Data;
using Engine;
using EngineWindows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PackageManager packer = new PackageManager();
            NetworkManager network = new NetworkManager(new NetworkWindows(), packer, ConfigurationManager.AppSettings["MasterHost"], Int32.Parse(ConfigurationManager.AppSettings["MasterPort"]));
            QueryManager query = new QueryManager(new QueryWindows());
            GameEngine engine = new GameEngine(new ResourceManager(new StorageWindows(StorageWindows.GetPathFolderResources())), new Canvas(new CanvasWindows()), network, query);
            network.Engine = engine;
            ScriptParser parser = new ScriptParser();
            CicaClient client = new CicaClient(engine, network, parser, packer);
            CicaClient.LogEvent += ConsoleWrite;
            client.Start();
            while (client.IsRunning) 
            {
                client.Send(Console.ReadLine());
            }
        }

        public static void ConsoleWrite(string message)
        {
            Console.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("hh:mm:ss"), message));
        }
    }
}
