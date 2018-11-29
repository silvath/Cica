using Data;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CicaServerMaster : CicaServer
    {
        #region Events
            public delegate void LogDelegate(string message);
            public static LogDelegate LogEvent;
        #endregion

        #region Attributes
            private bool _running = false;
        #endregion
        #region Properties
            public bool IsRunning 
            {
                get 
                {
                    return (this._running);
                } 
            }

            private List<ServerState> Servers { set; get; }
        #endregion

        #region Constructor
            public CicaServerMaster(IListenner listenner, PackageManager packer)  : base (listenner, packer)
            {
                this.Servers = new List<ServerState>();
            }
        #endregion

        #region Start
            public bool Start() 
            {
                this.LogWrite("Master Starting");
                this.LogWrite("Listenner Starting ");
                if (!this.Listenner.Start())
                    return (false);
                this.LogWrite("Listenner Started");
                this._running = true;
                this.LogWrite("Master Started");
                return (true);
            }
        #endregion
        #region Update
            public void Update() 
            {
                //Clients New
                INetwork client = this.Listenner.Client();
                if(client != null)
                    this.AddNetwork(client);
                //Commands
                foreach (NetworkManager network in this.Networks) 
                {
                    Package package = network.Receive();
                    if (package == null)
                        continue;
                    this.ProcessPackage(network, package); 
                }
            }
        #endregion
        #region Stop
            public void Stop() 
            {
                this.LogWrite("Stopping");
                this._running = false;
                this.LogWrite("Stopped");
            }
        #endregion

        #region Log
            private void LogWrite(string message)
            {
                if (CicaServerMaster.LogEvent != null)
                    CicaServerMaster.LogEvent(message);
            }
        #endregion

        #region Process
            private bool ProcessPackage(NetworkManager network, Package package) 
            {
                if (package.Type == PackageType.RequestRegisterServer)
                    return (this.RequestRegisterServer(network, package));
                else if (package.Type == PackageType.RequestServers)
                    return (this.RequestServers(network, package)); 
                network.Send(this.Packer.CreateError("Unknown Package"));
                return (false);
            }

            private bool RequestRegisterServer(NetworkManager network, Package package) 
            {
                if (package.Items.Count == 0) 
                {
                    network.Send(this.Packer.CreateError("Not Registered"));
                    return (false);
                }
                ServerState server = this.Packer.CreateServerState(package.Items[0]);
                this.Servers.Add(server);
                //Log
                this.LogWrite(string.Format("Server Registered: {0}", server.ToString()));
                //Response
                network.Send(this.Packer.Create(PackageType.ReponseRegisterServer));
                return (true);
            }

            private bool RequestServers(NetworkManager network, Package package) 
            {
                if ((package.Items.Count < 2) && (package.Items[0].Data.Count != 1) && (package.Items[1].Data.Count != 1))
                {
                    network.Send(this.Packer.CreateError("Request Invalid"));
                    return (false);
                }
                string version  = package.Items[0].Data[0];
                string game = package.Items[1].Data[0];
                List<ServerState> servers = this.Servers.FindAll(ss => (ss.Version == version) && (ss.Game == game));
                Package response = this.Packer.Create(PackageType.ReponseServers);
                foreach (ServerState server in servers) 
                    response.Items.Add(this.Packer.CreatePackageItem(server));
                network.Send(response);
                return (true);
            }
        #endregion
    }
}
