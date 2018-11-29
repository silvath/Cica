using Cica.CicaMessage;
using Cica.CicaResource;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cica.CicaClient
{
    public class Client
    {
        #region Events
            public delegate void LogDelegate(string message);
            public static LogDelegate LogEvent;
            public delegate void CommandDelegate(Command command);
            public static CommandDelegate CommandEvent;
            public delegate void ImageDelegate(Image image);
            public static ImageDelegate ImageEvent;
        #endregion

        #region Attributes
            private bool _close = false;
            private Thread _threadCommand;
            private Thread _threadData;
            private Queue<Command> _commands = new Queue<Command>();
            private Connection _connection = null;
        #endregion
        #region Properties
            private string Host { set; get; }
            private int PortConnection { set; get; }
            public bool Close 
            { 
                get
                {
                    return (this._close);
                }
            }

            private bool IsConnected
            {
                get
                {
                    return ((this._threadCommand != null) && (_threadCommand.ThreadState == ThreadState.Running));
                }
            }

            private bool IsRunning { set; get; }
            private Command LastCommand { set; get; }
            private ResourceManager ResourceManager { set; get; }
            private ResourceCollection Resources { set; get; }
        #endregion
        #region Constructors
            public Client() 
            {
                this.Host = "127.0.0.1";
                this.PortConnection = 9999;
                this.ResourceManager = new ResourceManager();
                this.Resources = new ResourceCollection();
            }
        #endregion

        #region Send
            public void Send(string message) 
            {
                if (string.IsNullOrEmpty(message))
                    return;
                Command command = CommandParser.Parse(message);
                if (command == null)
                    LogWrite(string.Format("Command Invalid: {0}", message));
                else
                    Send(command);
            }

            public void Send(CommandType type, params string[] parameters) 
            {
                Command command = new Command(type);
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                this.Send(command);
            } 

            public Command SendWait(CommandType type, params string[] parameters) 
            {
                this.LastCommand = null;
                Command command = new Command(type);
                if(parameters != null)
                    command.Parameters.AddRange(parameters);
                this.Send(command);
                while (this.LastCommand == null)
                    System.Threading.Thread.Sleep(50);
                return (this.LastCommand);
            } 

            public void Send(CommandType type) 
            {
                Send(new Command(type));
            }

            public void Send(Command command)
            {
                if (command.Type == CommandType.Close) 
                {
                    if(this.IsConnected)
                        this.Send(CommandType.Disconnect);
                    this._close = true;
                }else if (command.Type ==  CommandType.Connect){
                    this.ConnectCommand();
                }else if (command.Type == CommandType.Disconnect){
                    this.SendServer(command); 
                    this.DisconnectCommand();
                }else {
                    this.SendServer(command); 
                }
            }

            private void SendServer(CommandType type) 
            {
                SendServer(new Command(type));
            } 

            private void SendServer(Command command) 
            {
                this._commands.Enqueue(command);
            }
        #endregion

        #region Log
            private void LogWrite(string message)
            {
                if (Client.LogEvent != null)
                    Client.LogEvent(message);
            }
        #endregion
        #region Execute
            private void ExecuteCommand(Command command) 
            {
                if (command.Type == CommandType.DataConnect)
                    this.ExecuteCommandDataConnect();
                else if (command.Type == CommandType.ConnectedCommand)
                    ExecuteCommandConnected(command);
                else if (command.Type == CommandType.ResourceSynchronize)
                    ExecuteCommandResourceSynchronize();
                else if (command.Type == CommandType.SessionResourcesListed)
                    ExecuteCommandResourceListed(command);
                else if (command.Type == CommandType.ResourceRetrieved)
                    ExecuteCommandResourceRetrieved(command);
                else if (Client.CommandEvent != null)
                    Client.CommandEvent(command);
                LogWrite(command.ToString());
                this.LastCommand = command;
            }

            private void ExecuteCommandDataConnect() 
            {
                //Action
                ConnectData();
                //Response
                Command command = new Command(CommandType.DataConnected);
                this._connection.SendCommand(command); 
            }

            private void ExecuteCommandConnected(Command command) 
            {
                this._connection.Code = Int32.Parse(command.Parameters[0]);
            }

            private void ExecuteCommandResourceSynchronize()
            {
                //Resources
                this.Resources.Clear();
                this.Send(CommandType.SessionResourcesList);
            }

            private void ExecuteCommandResourceListed(Command command) 
            {
                //Synchronized
                this.Resources.Synchronized = command.Parameters;
                foreach (string resourceFullNameWithExtension in command.Parameters)
                {
                    Resource resource = this.ResourceManager.Load(resourceFullNameWithExtension);
                    if (resource != null)
                    {
                        this.Resources.Insert(resource);
                        if (this.Resources.IsReady)
                            this.Send(CommandType.ResourceSynchronized);
                    }else {
                        this.Send(CommandType.ResourceRetrieve, resourceFullNameWithExtension);
                    }
                }
            }

            private void ExecuteCommandResourceRetrieved(Command command) 
            {
                string resourceFullNameWithExtension = command.Parameters[0];
                if (!this.ResourceManager.Save(resourceFullNameWithExtension, command.Parameters[1]))
                    throw new Exception("Can not save resource");
                Resource resource = this.ResourceManager.Load(resourceFullNameWithExtension);
                this.Resources.Insert(resource);
                if (this.Resources.IsReady)
                    this.Send(CommandType.ResourceSynchronized);
            } 
        #endregion

        #region Connect
            private void ConnectCommand()
            {
                this.IsRunning = true;
                this._threadCommand = new Thread(new ThreadStart(this.ConnectionCommand));
                this._threadCommand.Start();
            }

            private void ConnectData()
            {
                this._threadData = new Thread(new ThreadStart(this.ConnectionData));
                this._threadData.Start();
            }
        #endregion
        #region Disconnect
            private void Disconnect() 
            {
                DisconnectCommand();
            }

            private void DisconnectCommand()
            {
                if (this._threadCommand == null)
                    return;
                this.IsRunning = false;
                while (this._threadCommand.ThreadState == ThreadState.Running) 
                    Thread.Sleep(500);
                this._threadCommand = null;
            }
        #endregion

        #region Connection
            private void ConnectionCommand() 
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse(this.Host), this.PortConnection));
                this._connection = new Connection(client);
                //Identify
                NetworkStream stream = client.GetStream();
                byte[] buffer = Command.GetBytes(0);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                while (this.IsRunning) 
                {
                    //Send
                    while(this._commands.Count > 0)
                        this._connection.SendCommand(this._commands.Dequeue());
                    //Receive
                    foreach (Command command in this._connection.ReceiveCommands())
                        this.ExecuteCommand(command);
                }
            }

            private void ConnectionData()
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse(this.Host), this.PortConnection));
                this._connection.ConnectionData = client;
                //Identify
                NetworkStream stream = client.GetStream();
                byte[] buffer = Command.GetBytes(this._connection.Code);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                while (this.IsRunning)
                {
                    //Receive
                    foreach (Data data in this._connection.ReceiveData())
                        this.ProcessData(data);
                }
            }
        #endregion
        #region Data
            private void ProcessData(Data data) 
            {
                if (Client.ImageEvent != null)
                    Client.ImageEvent(new Bitmap(100,100));
            }
        #endregion

        #region Commands
            public bool SyncConnect()
            {
                Command response = this.SendWait(CommandType.Connect);
                if (response.Type != CommandType.ConnectedCommand)
                    return (false);
                return (true);
            }

            public bool SyncJoinOrCreateSession()
            {
                Command response = this.SendWait(CommandType.SessionList);
                if (response.Type != CommandType.SessionListed)
                    return (false);
                string sessionCodeStart = string.Empty;
                if (response.Parameters.Count == 0)
                {
                    //Create
                    response = this.SendWait(CommandType.SessionCreate, DateTime.Now.ToString("HH:mm:ss"));
                    if (response.Type != CommandType.SessionCreated)
                        return (false);
                    sessionCodeStart = response.Parameters[0];
                }
                //Join
                response = this.SendWait(CommandType.SessionJoin);
                if (response.Type != CommandType.SessionJoined)
                    return (false);
                //Start
                if (!string.IsNullOrEmpty(sessionCodeStart))
                    this.Send(CommandType.SessionStart, sessionCodeStart);
                return (true);
            }
        #endregion
    }
}
