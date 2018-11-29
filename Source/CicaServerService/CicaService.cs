using Cica.CicaServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaServerService
{
    public partial class CicaService : ServiceBase
    {
        #region Attributes
            private Server server = Server.CreateInstance();
        #endregion
        #region Constructors
            public CicaService()
            {
                InitializeComponent();
            }
        #endregion

        protected override void OnStart(string[] args)
        {
            server.Start();
        }

        protected override void OnStop()
        {
            server.Stop();
        }
    }
}
