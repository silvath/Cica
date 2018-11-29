using Cica.CicaMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cica.CicaServer
{
    internal class Lobby
    {
        #region Events
            public delegate void LogDelegate(string message);
            public static LogDelegate LogEvent;
        #endregion

        #region Attributes
            private Thread _threadLobby;
            private volatile List<Connection> _connections = new List<Connection>();
        #endregion
        #region Properties
            public Server Server { set; get; }
            public bool IsRunning { set; get; }
        #endregion
        #region Constructors
            public Lobby(Server server) 
            {
                this.Server = server;
            }
        #endregion

        #region Start
            public void Start() 
            {
                this.IsRunning = true;
                this._threadLobby = new Thread(new ThreadStart(this.Process));
                this._threadLobby.Start();
            }
        #endregion
        #region Stop
            public void Stop() 
            {
                this.IsRunning = false;
                while (this._threadLobby.ThreadState == ThreadState.Running)
                    Thread.Sleep(500);
                this._threadLobby = null;
            }
        #endregion

        #region Connections
            public void AddConnection(TcpClient client) 
            {
                int connectionCode = Connection.GetIdentification(client);
                if (connectionCode == 0)
                {
                    //New
                    Connection connection = new Connection(client);
                    connectionCode = 1;
                    while (this._connections.Find(c => c.Code == connectionCode) != null)
                        connectionCode++;
                    connection.Code = connectionCode;
                    this._connections.Add(connection);
                    //Connected
                    Command command = new Command(CommandType.ConnectedCommand);
                    command.Parameters.Add(connectionCode.ToString());
                    connection.SendCommand(command);

                }else { 
                    //Associate
                    Connection connection = AssociateDataConnection(client, connectionCode);
                    //Connected
                    Command command = new Command(CommandType.ConnectedData);
                    command.Parameters.Add(connectionCode.ToString());
                    connection.SendCommand(command);
                }
            }

            public Connection AssociateDataConnection(TcpClient client, int connectionCode) 
            {
                Connection connection = this._connections.Find(c => c.Code == connectionCode);
                if (connection != null)
                    connection.ConnectionData = client;
                return (connection);
            }
        #endregion

        #region Process
            private void Process() 
            {
                //Running
                while (this.IsRunning)
                {
                    if (this._connections.Count == 0)
                    {
                        Thread.Sleep(500);
                    }else {
                        for (int i = this._connections.Count - 1; i >= 0; i--)
                            ProcessLobbyConnection(this._connections[i]); 
                    }
                }
                //Close Connections
            }

            private void ProcessLobbyConnection(Connection connection) 
            {
                foreach (Command command in connection.ReceiveCommands())
                    this.ProcessLobbyConnectionCommand(connection, command);
            }

            private void ProcessLobbyConnectionCommand(Connection connection, Command command)
            {
                this.LogWrite(string.Format("{0} - {1}", connection.ConnectionCommand.Client.LocalEndPoint.ToString(), command.ToString()));
                if (command.Type == CommandType.Talk){
                    this.ExecuteCommandTalk(connection, command);
                }else if (command.Type == CommandType.SessionCreate){
                    this.ExecuteCommandSessionsCreate(connection, command);
                }else if (command.Type == CommandType.SessionList){
                    this.ExecuteCommandSessionsList(connection, command);
                }else if (command.Type == CommandType.SessionJoin){
                    this.ExecuteCommandSessionJoin(connection, command);
                }else if (command.Type == CommandType.SessionStart){
                    this.ExecuteCommandSessionStart(connection, command);
                }else if(command.Type == CommandType.SessionResourcesList){
                    this.Server.Sessions.ExecuteSessionCommandResourcesList(connection, command);
                }else if (command.Type == CommandType.ResourceRetrieve){
                    this.Server.Sessions.ExecuteSessionCommandResourceRetrieve(connection, command);
                }else if (command.Type == CommandType.ResourceSynchronized){
                    this.Server.Sessions.ExecuteSessionCommandResourceSynchronized(connection, command);
                }
            }

            private void BroadcastLobby(Command command) 
            {
                foreach (Connection connection in this._connections)
                    this.SendCommand(connection, command); 
            }

            private void SendCommand(Connection connection, Command command) 
            {
                connection.SendCommand(command);
            }
        #endregion
        #region Commands
            private void ExecuteCommandError(Connection connection, string errorMessage)
            {
                Command command = new Command(CommandType.Error);
                command.Parameters.Add(errorMessage);
                this.SendCommand(connection, command); 
            }

            private void ExecuteCommandTalk(Connection connection, Command command)
            {
                this.BroadcastLobby(command);
            }

            private void ExecuteCommandSessionsCreate(Connection connection, Command command) 
            {
                if (command.Parameters.Count == 0) 
                {
                    ExecuteCommandError(connection, "Missing Session Name");
                    return;
                }
                //Action
                string sessionCode = this.Server.Sessions.Create(command.Parameters[0]).ToString();
                //Response
                command = new Command(CommandType.SessionCreated);
                command.Parameters.Add(sessionCode);
                this.SendCommand(connection, command); 
            }

            private void ExecuteCommandSessionsList(Connection connection, Command command)
            {
                //Action 
                Dictionary<int, string> sessions = this.Server.Sessions.GetSessions();
                //Response
                command = new Command(CommandType.SessionListed);
                foreach (KeyValuePair<int, string> session in sessions) 
                {
                    command.Parameters.Add(session.Key.ToString());
                    command.Parameters.Add(session.Value);
                }
                this.SendCommand(connection, command); 
            }

            private void ExecuteCommandSessionJoin(Connection connection, Command command) 
            {
                //Action
                int? sessionCode = null;
                int sessionCodeTry;
                if ((command.Parameters.Count > 0) && (Int32.TryParse(command.Parameters[0], out sessionCodeTry)))
                    sessionCode = sessionCodeTry;
                string message = this.Server.Sessions.Join(connection, sessionCode);
                //Response
                if (string.IsNullOrEmpty(message))
                {
                    command = new Command(CommandType.SessionJoined);
                }else {
                    command = new Command(CommandType.Error);
                    command.Parameters.Add(message);
                }
                this.SendCommand(connection, command); 
            }

            private void ExecuteCommandSessionStart(Connection connection, Command command)
            {
                //Action
                if (command.Parameters.Count == 0)
                { 
                    ExecuteCommandError(connection,"Missing Session Code");
                    return;
                }
                int sessionCode;
                if (!Int32.TryParse(command.Parameters[0], out sessionCode)) 
                {
                    ExecuteCommandError(connection, "Invalid Session Code");
                    return;
                }
                string message = this.Server.Sessions.Start(sessionCode);
                //Response
                if (string.IsNullOrEmpty(message))
                {
                    command = new Command(CommandType.SessionStarted);
                }else{
                    command = new Command(CommandType.Error);
                    command.Parameters.Add(message);
                }
                this.SendCommand(connection, command);
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
