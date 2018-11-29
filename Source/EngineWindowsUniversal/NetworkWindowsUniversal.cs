using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace EngineWindowsUniversal
{
    public class NetworkWindowsUniversal : INetwork
    {
        #region Attributes
        private StreamSocket _connection = null;
        private DataWriter _streamWriter = null;
        private DataReader _streamReader = null;
        private DataReaderLoadOperation _taskReader = null;
        #endregion
        #region Properties

        #endregion

        #region INetwork
        bool INetwork.IsConnected()
        {
            return ((this._connection != null) && (this._connection != null));
        }

        bool INetwork.Connect(string server, int port)
        {
            try
            {
                this._connection = new StreamSocket();
                this._connection.Control.NoDelay = true;
                this._connection.Control.KeepAlive = true;
                this._connection.ConnectAsync(new HostName(server), port.ToString(), SocketProtectionLevel.PlainSocket).AsTask().Wait();
                this._streamWriter = new DataWriter(this._connection.OutputStream);
                this._streamReader = new DataReader(this._connection.InputStream);
                this._streamReader.InputStreamOptions = InputStreamOptions.Partial;
            }
            catch
            {
                this._connection = null;
                this._streamWriter = null;
                this._streamReader = null;
            }
            return (this._connection != null);
        }

        void INetwork.Disconnect()
        {
            if (this._connection != null)
            {
                this._streamWriter.DetachStream();
                this._streamWriter.Dispose();
                this._streamReader.DetachStream();
                this._streamReader.Dispose();
                this._connection.Dispose();
                this._connection = null;
            }
        }

        void INetwork.Send(byte[] data)
        {
            this._streamWriter.WriteBytes(data);
            this._streamWriter.StoreAsync().AsTask();
        }

        byte[] INetwork.Receive()
        {
            if ((this._streamReader.UnconsumedBufferLength == 0) && ((this._taskReader == null) || (this._taskReader.Status == AsyncStatus.Completed)))
                this._taskReader = this._streamReader.LoadAsync(1024);
            if (this._streamReader.UnconsumedBufferLength == 0)
                return (null);
            byte[] buffer = new byte[this._streamReader.UnconsumedBufferLength];
            this._streamReader.ReadBytes(buffer);
            return (buffer);
        }
        #endregion

    }
}
