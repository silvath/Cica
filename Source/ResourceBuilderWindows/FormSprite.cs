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
    public partial class FormSprite : Form
    {
        #region Properties
            private SpriteMap SpriteMap { set; get; }
            private Sprite Sprite { set; get; }
            private int? Index { set; get; }
        #endregion
        #region Constructors
            public FormSprite(SpriteMap spriteMap, Sprite sprite, bool isNew)
            {
                this.SpriteMap = spriteMap;
                this.Sprite = sprite;
                if (isNew)
                {
                    this.Sprite = new Sprite();
                    if (sprite != null) 
                    {
                        this.Sprite.XImage = sprite.XImage;
                        this.Sprite.YImage = sprite.YImage;
                        this.Sprite.Width = sprite.Width;
                        this.Sprite.Height = sprite.Height;
                    }
                }else{
                    this.Index = this.SpriteMap.Sprites.IndexOf(sprite);
                }
                InitializeComponent();
                this.textBoxXI.Text = this.Sprite.XImage.ToString();
                this.textBoxYI.Text = this.Sprite.YImage.ToString();
                this.textBoxWidth.Text = this.Sprite.Width.ToString();
                this.textBoxHeight.Text = this.Sprite.Height.ToString();
                this.textBoxX.Text = this.Sprite.X.ToString();
                this.textBoxY.Text = this.Sprite.Y.ToString();
            }
        #endregion

        #region Events
            private void buttonOk_Click(object sender, EventArgs e)
            {
                this.Sprite.XImage = Int32.Parse(this.textBoxXI.Text);
                this.Sprite.YImage = Int32.Parse(this.textBoxYI.Text);
                this.Sprite.Width = Int32.Parse(this.textBoxWidth.Text);
                this.Sprite.Height = Int32.Parse(this.textBoxHeight.Text);
                this.Sprite.X = Int32.Parse(this.textBoxX.Text);
                this.Sprite.Y = Int32.Parse(this.textBoxY.Text);
                if (this.Index.HasValue)
                {
                    this.SpriteMap.Sprites[this.Index.Value] = this.Sprite;
                }else{
                    int xCount = Int32.Parse(this.textBoxXCount.Text);
                    int yCount = Int32.Parse(this.textBoxYCount.Text);
                    for (int x = 0; x < xCount; x++) 
                    {
                        for (int y = 0; y < yCount; y++) 
                        {
                            Sprite sprite = new Sprite();
                            sprite.XImage = this.Sprite.XImage;
                            sprite.YImage = this.Sprite.YImage;
                            sprite.Width = this.Sprite.Width;
                            sprite.Height = this.Sprite.Height;
                            sprite.X = this.Sprite.X + (x * sprite.Width);
                            sprite.Y = this.Sprite.Y + (y * sprite.Width);
                            this.SpriteMap.Sprites.Add(sprite);
                        } 
                    }
                }
                this.SpriteMap.Width = 0;
                this.SpriteMap.Heigth = 0;
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
