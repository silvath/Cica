using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class CicaServer
    {
        #region Properties
            internal IListenner Listenner { set; get; }
            internal PackageManager Packer { set; get; }
            internal List<NetworkManager> Networks { set; get; }
        #endregion
        #region Constructors
            public CicaServer(IListenner listenner, PackageManager packer) 
            {
                this.Listenner = listenner;
                this.Packer = packer;
                this.Networks = new List<NetworkManager>();
            }
        #endregion

        #region NetWork
            internal void AddNetwork(INetwork network) 
            {
                this.Networks.Add(new NetworkManager(network, this.Packer)); 
            }
        #endregion
    }
}
