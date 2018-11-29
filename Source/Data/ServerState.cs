using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class ServerState
    {
        #region Properties
            public string Version { set; get; }
            public string Game { set; get; }
            public string Name {set;get;}
            public string Address { set; get; }
            public string Port { set; get; }
        #endregion
        #region Override
            public override string ToString()
            {
                return (string.Format("{0} - {1} - {2} - {3} - {4}", this.Version, this.Game, this.Name, this.Address, this.Port));
            }
        #endregion
    }
}
