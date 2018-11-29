using Data;
using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceBuilderWindows
{
    public partial class FormResourceList : Form
    {
        #region Properties
            private ResourceManager Resources { set; get; }
            public Resource Resource { set; get; }
        #endregion
        #region Constructors
            public FormResourceList(ResourceManager resources)
            {
                this.Resources = resources;
                InitializeComponent();
                this.RefreshResources();
            }

            private void RefreshResources()
            {
                this.Resources.LoadAll();
                this.listBoxResources.Items.Clear();
                foreach (Resource resource in this.Resources.GetResources())
                    this.listBoxResources.Items.Add(resource);
            }
        #endregion

            private void buttonOk_Click(object sender, EventArgs e)
            {
                if (this.listBoxResources.SelectedItem == null)
                    return;
                this.Resource = this.listBoxResources.SelectedItem as Resource;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }

            private void buttonCancel_Click(object sender, EventArgs e)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
    }
}
