using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EngineWindows
{
    public class NetworkWindows : INetwork
    {
        #region Attributes
            private TcpClient _connection;
            private NetworkStream _stream;
        #endregion
        #region Properties
            public bool Delay { set; get; }

            private NetworkStream Stream 
            {
                get 
                {
                    if (this._stream == null)
                        this._stream = this._connection.GetStream();
                    return (this._stream);
                }
            }
        #endregion
        #region Constructors
            public NetworkWindows() 
            {
                this.Delay = false;
            }

            public NetworkWindows(TcpClient connection)
            {
                this._connection = connection;
                this.Delay = false;
            }
        #endregion

        #region INetwork
            bool INetwork.IsConnected()
            {
                return ((this._connection != null) && (this._connection.Connected));
            }

            bool INetwork.Connect(string server, int port)
            {
                this._stream = null;
                if ((this._connection != null) && (this._connection.Connected))
                    this._connection.Close();
                this._connection = null;
                try
                {
                    this._connection = new TcpClient();
                    this._connection.Connect(server, port);
                    this._connection.NoDelay = !this.Delay;
                }catch{
                    this._connection = null;
                    return (false);
                }
                return (true);
            }

            void INetwork.Disconnect()
            {
                this._stream = null;
                if ((this._connection != null) && (this._connection.Connected))
                    this._connection.Close();
                this._connection = null;
            }

            void INetwork.Send(byte[] data) 
            {
                NetworkStream stream = this.Stream;
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }

            byte[] INetwork.Receive() 
            {
                NetworkStream stream = this.Stream;
                if (!stream.DataAvailable) 
                    return(null);
                byte[] buffer = new byte[4096];
                int length = stream.Read(buffer, 0, buffer.Length);
                byte[] data = new byte[length];
                for (int i = 0; i < length; i++)
                    data[i] = buffer[i];
                return (data);
            }
        #endregion
        #region ToString
            public override string ToString()
            {
                return (this._connection.Client.RemoteEndPoint.ToString());
            }
        #endregion
    }
}
