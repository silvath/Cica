using Data;
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
    public partial class FormFrame : Form
    {
        #region Properties
            public Resource Resource { set; get; }
            public Animation Animation { set; get; }
            public Frame Frame { set; get; }
            public int? Index { set; get; }
        #endregion
        #region Constructors
            public FormFrame(Resource resource, Animation animation, Frame frame)
            {
                this.Resource = resource;
                this.Animation = animation;
                this.Frame = frame;
                if (this.Frame != null)
                {
                    this.Index = this.Animation.Frames.IndexOf(this.Frame);
                }else { 
                    this.Frame = new Frame();
                    this.Frame.X = 0;
                    this.Frame.Y = 0;
                    this.Frame.Frames = 1;
                }
                InitializeComponent();
                this.comboBoxSpriteMap.Items.Clear();
                foreach (SpriteMap spriteMap in this.Resource.SpritesMaps)
                    this.comboBoxSpriteMap.Items.Add(spriteMap);
                this.comboBoxSpriteMap.SelectedItem = this.Resource.SpritesMaps.GetSpriteMap(this.Frame.SpriteMapName);
                this.textBoxFrames.Text = this.Frame.Frames.ToString();
                this.textBoxX.Text = this.Frame.X.ToString();
                this.textBoxY.Text = this.Frame.Y.ToString();
            }
        #endregion
        #region Events
            private void buttonOk_Click(object sender, EventArgs e)
            {
                this.Frame.SpriteMapName = this.comboBoxSpriteMap.SelectedItem != null ? (this.comboBoxSpriteMap.SelectedItem as SpriteMap).Name : string.Empty;
                this.Frame.Frames = Int32.Parse(this.textBoxFrames.Text);
                this.Frame.X = Int32.Parse(this.textBoxX.Text);
                this.Frame.Y = Int32.Parse(this.textBoxY.Text);
                if (this.Index.HasValue)
                    this.Animation.Frames[this.Index.Value] = this.Frame;
                else
                    this.Animation.Frames.Add(this.Frame);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }

            private void buttonCancel_Click(object sender, EventArgs e)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        #endregion
    }
}
