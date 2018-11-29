using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cica.CicaServer
{
    internal class Listener
    {
        #region Events
            public delegate void AddClientConnectionDelegate(TcpClient client);
            public static AddClientConnectionDelegate AddClientConnectionEvent;
        #endregion

        #region Attributes
            private Thread _listenerConnections;
        #endregion
        #region Properties
            public int PortConnections { set; get; }
            public bool AcceptNewConnections { set; get; }
            public bool IsRunning { set; get; }
        #endregion

        #region Start
            public void Start() 
            {
                this.IsRunning = true;
                //Connections
                this._listenerConnections = new Thread(new ThreadStart(this.ListenConnections));
                this._listenerConnections.Start();
            }
        #endregion
        #region Stop
            public void Stop()
            {
                this.IsRunning = true;
                //Connections
                while (this._listenerConnections.ThreadState == ThreadState.Running)
                    Thread.Sleep(500);
                this._listenerConnections = null;
            }
        #endregion

        #region Listen
            private void ListenConnections() 
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Any, this.PortConnections);
                tcpListener.Start();
                while (this.IsRunning) 
                {
                    if ((this.AcceptNewConnections) && (tcpListener.Pending()))
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        if (AddClientConnectionEvent != null)
                            AddClientConnectionEvent(client);
                    }else {
                        Thread.Sleep(500);
                    }
                }
            }
        #endregion
    }
}
