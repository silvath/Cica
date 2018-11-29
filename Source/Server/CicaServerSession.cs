using Data;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CicaServerSession : CicaServer
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
            public ServerStatus Status { set; get; }
            private GameEngine Engine { set; get; }
            private string Game { set; get; }
            private string Name { set; get; }
            private NetworkManager Network { set; get; }
            private ResourceManager Resources { set; get; }
            private int MaxSlots { set; get; }
            private int MaxPlayers { set; get; }
            internal List<NetworkManager> NetworksLoading { set; get; }
            internal Queue<NetworkManager> NetworksQueue { set; get; }
            internal List<NetworkManager> NetworksPlaying { set; get; }
        #endregion

        #region Constructor
            public CicaServerSession(GameEngine engine, string game, string name, int maxSlots, IListenner listenner, ResourceManager resources, NetworkManager network, PackageManager packer) : base(listenner, packer)
            {
                this.Status = ServerStatus.Iddle;
                this.Engine = engine;                 
                this.Game = game;
                this.Name = name;
                this.Resources = resources;
                this.Network = network;
                this.MaxSlots = maxSlots;
                this.NetworksLoading = new List<NetworkManager>();
                this.NetworksQueue = new Queue<NetworkManager>();
                this.NetworksPlaying = new List<NetworkManager>();
            }
        #endregion

        #region Start
            public bool Start()
            {
                this.LogWrite("Server Starting");
                //Game
                this.LogWrite("Game Starting");
                this.Engine.Initialize(this.Game);
                this.LogWrite("Game Started");
                //Listenner
                this.LogWrite("Listenner Starting");
                if (!this.Listenner.Start())
                    return (false);
                this.LogWrite("Listenner Started");
                //Master
                this.LogWrite("Master Connect");
                if (!this.Network.ConnectMaster())
                {
                    this.Stop();
                    this.LogWrite("Error Connecting Master");
                    return(false);
                }
                this.LogWrite("Master Connected");
                //Register
                this.LogWrite("Server Register");
                this.Network.Send(this.Packer.Create(PackageType.RequestRegisterServer, PackageItemType.Server, VersionManager.Current, this.Game, this.Name, this.Engine.Query.Get(QueryType.AddressExternal), this.Listenner.Port.ToString()));
                Package response = this.Network.WaitReceive();
                if (response.Type != PackageType.ReponseRegisterServer) 
                {
                    this.Stop();
                    this.LogWrite("Error Registering Server");
                    return (false);
                }
                this.LogWrite("Server Registered");
                this._running = true;
                this.LogWrite("Server Started");
                return (true);
            }
        #endregion
        #region Update
            public void Update() 
            {
                //Clients New
                INetwork client = this.Listenner.Client();
                if (client != null)
                    this.AddNetwork(client);
                //Packages
                for (int i = this.Networks.Count - 1; i >= 0; i--)
                {
                    NetworkManager network = this.Networks[i];
                    try
                    {
                        Package package = network.Receive();
                        if (package == null)
                            continue;
                        this.ProcessPackage(network, package);
                    }catch (System.IO.IOException e){
                        this.Disconnect(network);
                    }

                }
                //Playing
                if (this.Status == ServerStatus.Playing) 
                {
                    if (!this.Engine.Update())
                        return; 

                    Package packageGameState = this.Packer.CreateGameState(this.Engine.State);
                    for(int i = this.NetworksPlaying.Count - 1; i >= 0;i--)
                    {
                        NetworkManager network = this.NetworksPlaying[i];
                        try
                        {
                            network.Send(packageGameState, false);
                        }catch (System.IO.IOException e){
                            this.Disconnect(network); 
                        }
                    }
                }
            }
        #endregion
        #region Stop
            public void Stop() 
            {
                this.LogWrite("Stopping");
                this.Listenner.Stop();
                this.Network.Disconnect();
                foreach(NetworkManager network in this.Networks)
                    network.Disconnect();
                this._running = false;
                this.LogWrite("Stopped");
            }
        #endregion
        #region Disconnect
            private void Disconnect(NetworkManager network) 
            {
                this.LogWrite("Client Disconnected");
                network.Disconnect();
                this.Networks.Remove(network);
                //Playing
                if (this.NetworksPlaying.Contains(network)) 
                {
                    this.Engine.RemovePlayer(network.Player);
                    this.NetworksPlaying.Remove(network);
                    //Queue
                    if (this.NetworksQueue.Count > 0)
                    {
                        NetworkManager networkWaiting = this.NetworksQueue.Dequeue();
                        if (networkWaiting != null)
                        {
                            ResourceState resourceState = this.Engine.AddPlayer(networkWaiting.Player);
                            if (resourceState != null)
                            {
                                //Response
                                networkWaiting.Send(this.Packer.Create(PackageType.ResponseGameStart, PackageItemType.Player, resourceState != null ? resourceState.Code : string.Empty));
                            }
                        }
                    }
                }
            }
        #endregion

        #region Log
            private void LogWrite(string message)
            {
                if (CicaServerSession.LogEvent != null)
                    CicaServerSession.LogEvent(message);
            }
        #endregion

        #region Process
            private bool ProcessPackage(NetworkManager network, Package package)
            {
                if (package.Type == PackageType.RequestSlots)
                    return (this.RequestSlots(network, package));
                else if (package.Type == PackageType.RequestGameJoin)
                    return (this.RequestGameJoin(network, package));
                else if (package.Type == PackageType.RequestGameStart)
                    return (this.RequestGameStart(network, package));
                else if (package.Type == PackageType.RequestCommand)
                    return (this.RequestCommand(network, package));
                network.Send(this.Packer.CreateError("Unknown Package"));
                return (false);
            }

            private bool RequestSlots(NetworkManager network, Package package)
            {
                int slots = (this.MaxSlots - this.NetworksPlaying.Count) + 1;
                //Log
                this.LogWrite(string.Format("Request Slots: {0}", slots.ToString()));
                //Response
                network.Send(this.Packer.Create(PackageType.ResponseSlots, PackageItemType.Slots, slots.ToString()));
                return (true);
            }

            private bool RequestGameJoin(NetworkManager network, Package package)
            {
                //Initialize Match
                if (this.Status == ServerStatus.Iddle)
                {
                    this.Status = ServerStatus.Loading;
                    this.Engine.CreateSession();
                }
                //Player Network
                string name = package.Items[0].Data[0];
                network.Player = new PlayerNetwork(name);
                this.NetworksLoading.Add(network);
                //Log
                this.LogWrite(string.Format("Request Game Join: {0}", network.ToString()));
                //Response
                network.Send(this.Packer.Create(PackageType.ResponseGameJoin, PackageItemType.Map, this.Engine.State.Map.Code.ToString()));
                return (true);
            }

            private bool RequestGameStart(NetworkManager network, Package package) 
            {
                this.NetworksLoading.Remove(network);
                bool isPlaying = false;
                ResourceState resourceState = this.Engine.AddPlayer(network.Player);
                if (isPlaying = (resourceState != null))
                    this.NetworksPlaying.Add(network);
                else 
                    this.NetworksQueue.Enqueue(network);
                if (this.Status == ServerStatus.Loading) 
                {
                    this.Status = ServerStatus.Playing;
                    this.Engine.StartSession();
                }
                //Log
                this.LogWrite(string.Format("Request Game Start: {0}", network.ToString()));
                //Response
                network.Send(this.Packer.Create(PackageType.ResponseGameStart, PackageItemType.Player, resourceState != null ? resourceState.Code : string.Empty));
                return (true);
            }

            private bool RequestCommand(NetworkManager network, Package package) 
            {
                if (this.Status != ServerStatus.Playing)
                    return (false);
                network.Player.CommandName = package.Items[0].Data[0];
                return (true);
            }
        #endregion
    }
}
