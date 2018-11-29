using Cica.CicaResource;
using Cica.CicaServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CicaServerCatchWinForms
{
    public partial class FormServer : Form
    {
        #region Attributes
            private Server _server = Server.CreateInstance();
        #endregion
        #region Properties
            private Server Server 
            {
                get 
                {
                    return (this._server);
                }
            }
        #endregion
        #region Constructors
            public FormServer()
            {
                InitializeComponent();
                //Server
                Server.LogEvent += WriteLog;
                this.Server.Start();
            }

            private void WriteLog(string message)
            {
                this.textBoxLog.Text = message + System.Environment.NewLine + this.textBoxLog.Text;
            }
        #endregion

        private void buttonResourcesCreate_Click(object sender, EventArgs e)
        {
            //Clear
            this.Server.ResourceManager.Clear();
            //Game
            Game game = new Game();
            //Actors
            Actor actor = null;
            actor = new Actor();
            actor.Name = "Iron Man";
            actor.Sprites.Create(GetBytes(@"..\Images\Catch\IronMan.png"));
            game.Requirements.Create(actor);
            this.Server.ResourceManager.Save(actor);
            actor = new Actor();
            actor.Name = "Darth";
            actor.Sprites.Create(GetBytes(@"..\Images\Catch\Darth.png"));
            game.Requirements.Create(actor);
            this.Server.ResourceManager.Save(actor);
            actor = new Actor();
            actor.Name = "Halo";
            actor.Sprites.Create(GetBytes(@"..\Images\Catch\Halo.png"));
            game.Requirements.Create(actor);
            this.Server.ResourceManager.Save(actor);
            actor = new Actor();
            actor.Name = "Metroid";
            actor.Sprites.Create(GetBytes(@"..\Images\Catch\Metroid.png"));
            game.Requirements.Create(actor);
            this.Server.ResourceManager.Save(actor);
            actor = new Actor();
            actor.Name = "Sonic";
            actor.Sprites.Create(GetBytes(@"..\Images\Catch\Sonic.png"));
            game.Requirements.Create(actor);
            this.Server.ResourceManager.Save(actor);
            actor = new Actor();
            actor.Name = "Wally";
            actor.Sprites.Create(GetBytes(@"..\Images\Catch\Wally.png"));
            game.Requirements.Create(actor);
            this.Server.ResourceManager.Save(actor);
            //Maps
            Map map = new Map();
            map.Name = "Ghost Town";
            map.Sprites.Create(GetBytes(@"..\Images\Catch\Map1.png"));
            this.Server.ResourceManager.Save(map);
            //Game
            game.Name = "Catch";
            this.Server.ResourceManager.Save(game);
        }

        private List<byte> GetBytes(string relativePath)
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            Image image = Image.FromFile(path);
            MemoryStream memory = new MemoryStream();
            image.Save(memory, ImageFormat.Png);
            return (new List<byte>(memory.ToArray()));         
        }

        private void buttonResourcesRefresh_Click(object sender, EventArgs e)
        {
            List<string> resources = this.Server.ResourceManager.ListRepositoryResourcesNames();
            this.listBoxResources.Items.Clear();
            foreach (string resource in resources)
                this.listBoxResources.Items.Add(resource);
        }
    }
}
