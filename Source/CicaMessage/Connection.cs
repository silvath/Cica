using Cica.CicaMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaMessage
{
    public class Connection
    {
        #region Attributes
            private TcpClient _connectionCommand;
            private TcpClient _connectionData;
            private List<byte> _bufferCommand = new List<byte>();
            private List<byte> _bufferData = new List<byte>();
        #endregion
        #region Properties
            public int Code { set; get; }

            public TcpClient ConnectionCommand 
            {
                get 
                {
                    return (this._connectionCommand);
                }
            }

            public TcpClient ConnectionData
            {
                set 
                {
                    this._connectionData = value;
                }
                get
                {
                    return (this._connectionData);
                }
            }

            public bool IsConnectionDataConnected
            {
                get 
                {
                    return ((this.ConnectionData != null) && (this.ConnectionData.Connected));
                }                    
            }

            public int? SessionCode { set; get; }
            public bool IsResourcesSynchronized { set; get; }

            public bool IsReady { get { return (this.IsResourcesSynchronized && this.IsConnectionDataConnected); } }
        #endregion
        #region Constructors
            public Connection(TcpClient connectionCommand) 
            {
                this._connectionCommand = connectionCommand;
                this.IsResourcesSynchronized = false;
            }
        #endregion

        #region Identify
            public static int GetIdentification(TcpClient client)
            {
                NetworkStream stream = client.GetStream();
                List<byte> buffer = new List<byte>();
                while(buffer.Count < 4)
                {
                    if(stream.DataAvailable)
                        buffer.Add((byte)stream.ReadByte());
                }
                return(Data.GetInt(buffer, 0));

            }
        #endregion
        #region Send
            public void SendCommand(Command command) 
            {
                NetworkStream stream = this._connectionCommand.GetStream();
                byte[] buffer = command.GetBytes();
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }

            public void SendData(Data data) 
            {
                if (data == null)
                    return;
                NetworkStream stream = this._connectionData.GetStream();
                byte[] buffer = data.GetBytes();
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            } 
        #endregion
        #region Receive
            public List<Command> ReceiveCommands() 
            {
                List<Command> commands = new List<Command>();
                NetworkStream stream = this._connectionCommand.GetStream();
                if(stream.DataAvailable)
                {
                    //Read
                    byte[] buffer = new byte[4096];
                    int count = stream.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < count; i++)
                        this._bufferCommand.Add(buffer[i]);
                    //Commands
                    if (count > 0) 
                    {
                        int offset = 0;
                        int length = 0;
                        Command command = null;
                        while ((command = Command.Create(this._bufferCommand, offset, out length)) != null) 
                        {
                            offset += length;
                            commands.Add(command);
                        }
                        this._bufferCommand.RemoveRange(0, offset + length);
                    }
                }
                return (commands);
            }

            public List<Data> ReceiveData() 
            {
                List<Data> datas = new List<Data>();
                NetworkStream stream = this._connectionData.GetStream();
                if (stream.DataAvailable)
                {
                    //Read
                    byte[] buffer = new byte[4096];
                    int count = stream.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < count; i++)
                        this._bufferData.Add(buffer[i]);
                    //Commands
                    if (count > 0)
                    {
                        int offset = 0;
                        int length = 0;
                        Data data = null;
                        while ((data = Data.Create(this._bufferData, offset, out length)) != null)
                        {
                            offset += length;
                            datas.Add(data);
                        }
                        this._bufferData.RemoveRange(0, offset + length);
                    }
                }
                return (datas);
            }
        #endregion

    }
}
