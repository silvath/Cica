using Engine;
using EngineWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerWindows
{
    public class ListennerWindows : IListenner
    {
        #region Events
            public delegate void LogDelegate(string message);
            public static LogDelegate LogEvent;
        #endregion

        #region Attributes
            private Thread _thread;
            private bool _isRunning = false;
        #endregion
        #region Properties
            private string AddressInternal { set; get; }
            private int PortInternal{set;get;}

            public bool IsRunning 
            { 
                get
                {
                    return(this._isRunning);
                }
            }

            private List<TcpClient> Clients { set; get; }
            private Queue<INetwork> ClientsNew { set; get; }
        #endregion
        #region Constructors
            public ListennerWindows(string address, int port) 
            {
                this.AddressInternal = address;
                this.PortInternal = port;
                this.Clients = new List<TcpClient>();
                this.ClientsNew = new Queue<INetwork>();
            }
        #endregion

        #region IListenner
            string IListenner.Address 
            {
                get 
                {
                    return (this.AddressInternal);
                }
            }

            int IListenner.Port
            {
                get 
                {
                    return (this.PortInternal);
                }
            }
            
            bool IListenner.Start()
            {
                this._isRunning = true;
                this._thread = new Thread(new ThreadStart(this.ListenConnections));
                this._thread.Start();
                return (true);
            }

            bool IListenner.Stop()
            {
                return (true);
            }

            INetwork IListenner.Client() 
            {
                if(this.ClientsNew.Count == 0)
                    return (null);
                return (this.ClientsNew.Dequeue());
            } 
        #endregion

        #region Listen
            private void ListenConnections()
            {
                IPAddress ipAddress = IPAddress.Parse(this.AddressInternal);
                TcpListener tcpListener = new TcpListener(ipAddress, this.PortInternal);
                tcpListener.Start();
                while (this.IsRunning)
                {
                    if (tcpListener.Pending())
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        this.Clients.Add(client);
                        this.LogWrite(string.Format("Client Connected - {0}", client.Client.RemoteEndPoint.ToString()));
                        this.ClientsNew.Enqueue(new NetworkWindows(client));
                    }else{
                        Thread.Sleep(2000);
                    }
                }
            }
        #endregion

        #region Log
            private void LogWrite(string message)
            {
                if (ListennerWindows.LogEvent != null)
                    ListennerWindows.LogEvent(message);
            }
        #endregion
    }
}
