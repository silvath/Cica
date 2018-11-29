using Cica.CicaMessage;
using Cica.CicaResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cica.CicaServer
{
    internal class Session
    {
        #region Properties
            public SessionCollection Sessions { set; get; }
            public int Code { set; get; } 
            public string Name { set; get; }
            public int Max { set; get; }
            private List<Connection> Connections { set; get; }
            private Thread _threadSessionData;
            private SessionState State { set; get; }
            internal ResourceCollection Resources { set; get; }
            public Server Server
            {
                get
                {
                    return (this.Sessions.Server);
                }
            }
            internal Game Game { set; get; }
            internal Map Map { set; get; }
        #endregion
        #region Constructors
            public Session(SessionCollection sessions) 
            {
                this.State = SessionState.Waiting;
                this.Sessions = sessions;
                this.Max = 16;
                this.Connections = new List<Connection>();
                this.Resources = new ResourceCollection();
            }
        #endregion

        #region Connections
            public bool AcceptNewConnection() 
            {
                return ((this.Connections.Count - 1) < this.Max);
            }

            public bool AddConnection(Connection connection) 
            {
                if (!this.AcceptNewConnection())
                    return(false);
                if (this.State == SessionState.Running)
                {
                    connection.SessionCode = this.Code;
                    connection.SendCommand(new Command(CommandType.ResourceSynchronize));
                    connection.SendCommand(new Command(CommandType.DataConnect));
                }
                this.Connections.Add(connection);
                return (true);
            }
        #endregion

        #region Start
            public string Start() 
            {
                this.State = SessionState.Running;
                //Initialize Game
                string message = this.InitializeGameSession();
                if (!string.IsNullOrEmpty(message))
                    return (message);
                //Initialize Data Connection
                foreach (Connection connection in this.Connections)
                {
                    connection.IsResourcesSynchronized = false;
                    connection.SessionCode = this.Code;
                    connection.SendCommand(new Command(CommandType.ResourceSynchronize));
                    connection.SendCommand(new Command(CommandType.DataConnect));
                }
                //Process Data
                this._threadSessionData = new Thread(new ThreadStart(this.ProcessData));
                this._threadSessionData.Start();
                return (string.Empty);
            }
        #endregion
        #region Stop
            public void Stop()
            {
                this.State = SessionState.Stopped;
                while (this._threadSessionData.ThreadState == ThreadState.Running)
                    Thread.Sleep(500);
                this._threadSessionData = null;
                this.State = SessionState.Waiting;
            }
        #endregion

        #region Process
            private void ProcessData() 
            {
                while (this.State == SessionState.Running) 
                {
                    //Interaction
                    foreach (Connection connection in this.Connections)
                    {
                        if (connection.IsReady)
                            continue;
                        List<Data> datas = connection.ReceiveData();
                        foreach(Data data in datas)
                            this.Server.GameEngine.Interate(this, connection, data);
                    }
                    //Iteration
                    this.Server.GameEngine.Iterate(this);
                    //Snapshot
                    foreach (Connection connection in this.Connections)
                        if (connection.IsReady)
                            connection.SendData(this.Server.GameEngine.CreateSnapshot(this,connection));
                }
            }
        #endregion
        #region Commands
            internal void ExecuteSessionCommandResourcesList(Connection connection, Command command) 
            {
                Command commandResponse = new Command(CommandType.SessionResourcesListed);
                foreach (Resource resource in this.Resources)
                    commandResponse.Parameters.Add(resource.FullNameWithExtension);
                connection.SendCommand(commandResponse);
            }

            internal void ExecuteSessionCommandResourceRetrieve(Connection connection, Command command) 
            {
                Resource resource = this.Resources.Find(r => r.FullNameWithExtension == command.Parameters[0]);
                Command commandResponse = new Command(CommandType.ResourceRetrieved);
                commandResponse.Parameters.Add(resource.FullNameWithExtension);
                commandResponse.Parameters.Add(this.Server.ResourceManager.LoadData(resource.FullNameWithExtension));
                connection.SendCommand(commandResponse);
            }

            internal void ExecuteSessionCommandResourceSynchronized(Connection connection, Command command) 
            {
                connection.IsResourcesSynchronized = true;
            }
        #endregion

        #region Game
            private string InitializeGameSession() 
            {
                //Resources
                if(!InitializeGameSessionResources())
                    return ("Error loading resources");
                return (string.Empty);
            }
 
            private bool InitializeGameSessionResources() 
            {
                //Game
                this.Resources.Clear();
                if (this.Game == null)
                {
                    string gameName = this.Server.ResourceManager.ListRepositoryResourcesNames(ResourceType.Game).First();
                    if (string.IsNullOrEmpty(gameName))
                        return (false);
                    this.Game = this.Server.ResourceManager.Load(gameName) as Game;
                    if (this.Game == null)
                        return (false);
                }
                this.Resources.Add(this.Game);
                //Map
                if (this.Map == null)
                {
                    string mapName = this.Server.ResourceManager.ListRepositoryResourcesNames(ResourceType.Map).First();
                    if (string.IsNullOrEmpty(mapName))
                        return (false);
                    this.Map = this.Server.ResourceManager.Load(mapName) as Map;
                    if (this.Map == null)
                        return (false);
                }
                this.Resources.Add(this.Map);
                //Requirements
                foreach (Requirement requirement in this.Game.Requirements) 
                {
                    string requirementName = this.Server.ResourceManager.ListRepositoryResourcesNames().Find(r => r.StartsWith(requirement.ResourceCode.ToString()));
                    if (string.IsNullOrEmpty(requirementName))
                        return (false);
                    Resource resource = this.Server.ResourceManager.Load(requirementName);
                    if (resource == null)
                        return (false);
                    this.Resources.Add(resource);
                }
                return (true);
            }
        #endregion
    }
}
