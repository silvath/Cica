using Cica.CicaClient;
using Cica.CicaMessage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CicaClientWinForms
{
    public partial class Form1 : Form
    {
        #region Attributes
            private Client _client = new Client();
        #endregion
        #region Constructors
            public Form1()
            {
                InitializeComponent();
                Client.CommandEvent += ExecuteCommand;
                Client.ImageEvent += DrawImage;
                System.Threading.Thread.Sleep(1000);
                this._client.SyncConnect();
                this._client.SyncJoinOrCreateSession();
            }
        #endregion

        #region Connection
        #endregion
        #region Commands
            public void ExecuteCommand(Command command) 
            {

            } 
        #endregion
        #region Image
            private void DrawImage(Image image) 
            {
                this.pictureBox1.Image = image;
            } 
        #endregion
    }
}
