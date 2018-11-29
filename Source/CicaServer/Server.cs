using Cica.CicaMessage;
using Cica.CicaResource;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaServer
{
    public class Server
    {
        #region Events
            public delegate void LogDelegate(string message);
            public static LogDelegate LogEvent;
        #endregion

        #region Attributes
            private GameEngine _game;
            private static Server _instance = null;
            private Lobby _executor = null;
            private Listener _listener = null;
            private int _portConnections = 9999;
            private SessionCollection _sessions = null;
            private ResourceManager _resourceManager = null;
        #endregion
        #region Properties
            private Lobby Executor 
            {
                get 
                {
                    if (this._executor == null) 
                    {
                        this._executor = new Lobby(this);
                        Lobby.LogEvent += LogWrite;
                    }
                    return (this._executor);
                }
            }

            private Listener Listener 
            {
                get 
                {
                    if (this._listener == null) 
                    {
                        Listener.AddClientConnectionEvent += this.AddClientConnection;
                        this._listener = new Listener();
                        this._listener.PortConnections = this._portConnections;
                        this._listener.AcceptNewConnections = true;
                    }
                    return(this._listener);
                }
            }

            public bool IsRunning{set;get;}

            public SessionCollection Sessions 
            {
                get 
                {
                    if (this._sessions == null)
                        this._sessions = new SessionCollection(this);
                    return (this._sessions);
                }
            }

            public ResourceManager ResourceManager
            {
                get 
                {
                    if (this._resourceManager == null)
                        this._resourceManager = new ResourceManager();
                    return(_resourceManager); 
                }
            }

            public GameEngine GameEngine 
            {
                get 
                {
                    return (this._game);
                }
            }

        #endregion

        #region Constructors
            public static Server CreateInstance()
            {
                if (_instance == null)
                    _instance = new Server(new GameEngine());
                return (_instance);
            }

            private Server(GameEngine game) 
            {
                this._game = game;
            } 
        #endregion

        #region Events
            public void Start() 
            {
                this.IsRunning = true;
                this.LogWrite("Server Start");
                this.LogWrite("Server Starting Executor");
                this.Executor.Start();
                this.LogWrite("Server Started Executor");
                this.LogWrite("Server Starting Listener");
                this.Listener.Start();
                this.LogWrite(string.Format("Server Started Listenning Commands on Port: {0}", this.Listener.PortConnections));
                this.LogWrite("Server Started");
            }

            public void Stop()
            {
                this.LogWrite("Server Stoping");
                this.Executor.Stop();
                this.Listener.Stop();
                this.LogWrite("Server Stop");
                this.IsRunning = false;
            }
        #endregion
        #region Client
            private void AddClientConnection(TcpClient client) 
            {
                this.LogWrite("Client Command Connected");
                this.Executor.AddConnection(client);
            }
        #endregion

        #region Log
            private void LogWrite(string message) 
            {
                if (Server.LogEvent != null)
                    Server.LogEvent(message);
            }
        #endregion
    }
}
