using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class NetworkManager
    {
        #region Properties
            public PlayerNetwork Player { set; get; }
            private INetwork Network { set; get; }
            private PackageManager Packer { set; get; }
            private string MasterServerHost { set; get; }
            private int MasterServerPort { set; get; }
            private List<byte> Buffer { set; get; }
            public GameEngine Engine { set; get; }
            public string ResourceStateCode { set; get; }
            private byte[] DataSent { set; get; }
        #endregion
        #region Constructor
            public NetworkManager(INetwork network, PackageManager packer) : this(network, packer, string.Empty, 0)
            {
            } 

            public NetworkManager(INetwork network, PackageManager packer, string masterServerHost, int masterServerPort) 
            {
                this.Network = network;
                this.Packer = packer;
                this.MasterServerHost = masterServerHost;
                this.MasterServerPort = masterServerPort;
                this.Buffer = new List<byte>();
            }
        #endregion

        #region Connect
            public bool ConnectMaster() 
            {
                return(Connect(this.MasterServerHost, this.MasterServerPort));
            } 

            public bool Connect(string host, int port) 
            {
                return(this.Network.Connect(host, port));
            }

            public bool Connect(ServerState server) 
            {
                return (this.Connect(server.Address, Int32.Parse(server.Port)));
            }

            public bool Connect(string game) 
            {
                if (!this.ConnectMaster())
                    return (false);
                this.Send(this.Packer.CreateRequestServers(VersionManager.Current, game));
                Package response = this.WaitReceive();
                if (response.Type != PackageType.ReponseServers)
                    return(false);
                List<ServerState> servers = new List<ServerState>();
                foreach (PackageItem item in response.Items)
                    servers.Add(this.Packer.CreateServerState(item));
                return (this.Connect(servers)); 
            }

            public bool Connect(List<ServerState> servers) 
            {
                return (this.Connect(servers, this.RetrieveServersSlotsAndLatency(servers))); 
            } 

            public bool Connect(List<ServerState> servers, List<Tuple<int, int, int>> serversSlotsAndLatency) 
            {
                List<int> serversDenied = new List<int>();
                while (serversDenied.Count < servers.Count) 
                {
                    ServerState server = RetrieveServerLatency(servers, serversSlotsAndLatency, serversDenied);
                    if (this.Connect(server))
                        return(true);
                    serversDenied.Add(servers.IndexOf(server));
                }
                return (false);
            }
        #endregion
        #region Disconnect
            public void Disconnect() 
            {
                this.ResourceStateCode = string.Empty;
                this.Network.Disconnect();
            } 
        #endregion
        #region Join
            public bool Join() 
            {
                bool isPlaying;
                return (Join(out isPlaying));
            }

            public bool Join(out bool isPlaying) 
            {
                isPlaying = false;
                if (this.Engine == null)
                    return (false);
                this.Send(this.Packer.Create(PackageType.RequestGameJoin, PackageItemType.Player, this.Engine.Query.Get(QueryType.PlayerName)));
                //Response
                Package response = this.WaitReceive();
                if (response.Type != PackageType.ResponseGameJoin)
                    return(false);
                string mapCode = response.Items[0].Data[0];
                //Load Game 
                this.Engine.InitializeMap(mapCode);
                //Start Game
                this.Send(this.Packer.Create(PackageType.RequestGameStart));
                //Response
                response = this.WaitReceive();
                if (response.Type != PackageType.ResponseGameStart)
                    return(false);
                this.ResourceStateCode = response.Items[0].Data[0];
                return(!string.IsNullOrEmpty(this.ResourceStateCode));
            }
        #endregion
        #region Update
            public bool Update(Package package, GameState gameState, ResourceManager resources) 
            {
                bool updated = false;
                bool clearProperties, clearTexts = false;
                GameState gameStateNetwork = this.Packer.CreateGameState(gameState.Game, package, resources, out clearProperties, out clearTexts);
                //GameState
                if (gameState.Screen != gameStateNetwork.Screen) 
                {
                    gameState.Screen = gameStateNetwork.Screen;
                    updated = true;
                }
                //AnimationStates
                for (int i = 0; i < gameStateNetwork.AnimationStates.Count; i++) 
                {
                    if (i >= gameState.AnimationStates.Count)
                    {
                        gameState.AnimationStates.Add(gameStateNetwork.AnimationStates[i]);
                        updated = true;
                    }else { 
                        if(this.Packer.Update(gameState.AnimationStates[i], gameStateNetwork.AnimationStates[i]))
                            updated = true;
                    }
                }
                while (gameState.AnimationStates.Count > gameStateNetwork.AnimationStates.Count) 
                {
                    gameState.AnimationStates.RemoveAt(gameState.AnimationStates.Count - 1);
                    updated = true;
                }
                //Properties
                if (clearProperties) 
                {
                    gameState.Properties.Clear();
                    gameState.Properties.AddRange(gameStateNetwork.Properties);
                    updated = true;
                }
                //Texts
                if (clearTexts)
                {
                    gameState.Texts.Clear();
                    gameState.Texts.AddRange(gameStateNetwork.Texts);
                    updated = true;
                }
                return (updated);
            }
        #endregion

        #region Send
            public void Send(Package package) 
            {
                Send(package, true); 
            }

            public void Send(Package package, bool repeat) 
            {
                byte[] data = this.Packer.GetBytes(package).ToArray();
                if ((repeat) || (!IsEqual(data, this.DataSent)))
                    this.Network.Send(this.GetDataPackageDiff(DataSent,data) ?? data);
                this.DataSent = data;
            }

            private bool IsEqual(byte[] data1, byte[] data2) 
            {
                if ((data1 == null) || (data2 == null))
                    return (false);
                if (data1.Length != data2.Length)
                    return (false);
                for(int i = 0; i < data1.Length;i++)
                    if (data1[i] != data2[i])
                        return (false);
                return (true);
            }

            private byte[] GetDataPackageDiff(byte[] dataPrevious, byte[] dataCurrent) 
            {
                if ((dataPrevious == null) || (dataCurrent == null) || (dataPrevious.Length != dataCurrent.Length))
                    return (null);
                List<byte> dataDiff = new List<byte>();
                int offset = 0;
                for (int i = 0; i < dataPrevious.Length; i++) 
                {
                    if (dataPrevious[i] == dataCurrent[i])
                        continue;
                    dataDiff.Add(this.Packer.GetByte(i - offset));
                    dataDiff.Add(dataCurrent[i]);
                    offset = i;
                }
                dataDiff.Insert(0, (byte)PackageType.PackageDiff);
                dataDiff.InsertRange(0, this.Packer.GetBytes(dataDiff.Count + 4));
                return (dataDiff.ToArray());
            }
        #endregion
        #region Receive
            public Package Receive() 
            {
                byte[] data = this.Network.Receive();
                if (data != null)
                    this.Buffer.AddRange(data);
                return (this.Packer.Create(this.Buffer));
            }

            public Package WaitReceive() 
            {
                Package package = null;
                while ((package = Receive()) == null)
                    this.Sleep(1000);
                return (package);
            }

            private void Sleep(int miliseconds) 
            {
                //TODO: nothing here
            }
        #endregion

        #region Commands
            public List<Tuple<int, int, int>> RetrieveServersSlotsAndLatency(List<ServerState> servers) 
            {
                List<Tuple<int, int, int>> serversSlotsAndLatency = new List<Tuple<int, int, int>>();
                for (int i = 0; i < servers.Count; i++)
                {
                    ServerState server = servers[i];
                    if (!this.Connect(server.Address, Int32.Parse(server.Port)))
                        continue;
                    DateTime now = DateTime.Now;
                    this.Send(this.Packer.Create(PackageType.RequestSlots));
                    Package response = this.WaitReceive();
                    int latency = DateTime.Now.Subtract(now).Milliseconds;
                    if (response.Type != PackageType.ResponseSlots)
                        continue;
                    int slots = Int32.Parse(response.Items[0].Data[0]);
                    if (slots == 0)
                        continue;
                    serversSlotsAndLatency.Add(new Tuple<int, int, int>(i, slots, latency));
                }
                return (serversSlotsAndLatency);
            }

            public ServerState RetrieveServerLatency(List<ServerState> servers, List<Tuple<int, int, int>> serversSlotsAndLatency, List<int> serversDenied)
            {
                int latency = Int32.MaxValue;
                int? serverIndex = null;
                foreach (Tuple<int, int, int> serverInfo in serversSlotsAndLatency) 
                {
                    if (serversDenied.Contains(serverInfo.Item1))
                        continue;
                    if (serverInfo.Item2 == 0)
                        continue;
                    if (latency < serverInfo.Item3)
                        continue;
                    latency = serverInfo.Item3;
                    serverIndex = serverInfo.Item1;
                }
                return(serverIndex.HasValue ? servers[serverIndex.Value] : null);
            }
        #endregion

        #region ToString
            public override string ToString()
            {
                return (this.Network.ToString());
            }
        #endregion
    }
}
