using Data;
using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Color = System.Drawing.Color;

namespace ResourceBuilderWindows
{
    public partial class FormSpriteMap : Form
    {
        #region Attributes
        #endregion
        #region Properties
            public GameEngine Game {set;get;}
            private Resource Resource { set; get; }
            private SpriteMap SpriteMap { set; get; }
            private int? Index { set; get; }
        #endregion
        #region Constructors
            public FormSpriteMap(GameEngine game, Resource resource, SpriteMap spriteMap)
            {
                this.Game = game;
                this.Resource = resource;
                this.SpriteMap = spriteMap;
                if (this.SpriteMap == null)
                    this.SpriteMap = new SpriteMap();
                else
                    this.Index = resource.SpritesMaps.IndexOf(spriteMap);
                InitializeComponent();
                RefreshSpriteMap();
            }
        #endregion

        #region Events
            private void comboBoxImages_SelectedIndexChanged(object sender, EventArgs e)
            {
                Data.Image image = this.comboBoxImages.SelectedItem as Data.Image;
                if (this.SpriteMap.ImageName == image.Name)
                    return;
                this.SpriteMap.ImageName = image.Name;
                this.SpriteMap.Sprites.Clear();
                RefreshImage();
            }

            private void buttonOk_Click(object sender, EventArgs e)
            {
                if (this.Index.HasValue)
                    this.Resource.SpritesMaps[this.Index.Value] = this.SpriteMap;
                else
                    this.Resource.SpritesMaps.Add(this.SpriteMap);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }

            private void buttonCancel_Click(object sender, EventArgs e)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }

            private void listBoxSprites_SelectedIndexChanged(object sender, EventArgs e)
            {
                this.RefreshImage();
                this.RefreshResult();
            }

            private void textBoxName_TextChanged(object sender, EventArgs e)
            {
                this.SpriteMap.Name = this.textBoxName.Text;
            }

            private void textBoxTransparency_TextChanged(object sender, EventArgs e)
            {
                this.SpriteMap.Transparency = this.textBoxTransparency.Text;
            }

            private void buttonSpriteNew_Click(object sender, EventArgs e)
            {
                FormSprite formSprite = new FormSprite(this.SpriteMap, this.listBoxSprites.SelectedItem as Sprite, true);
                if (formSprite.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                this.RefreshSprites();
                this.RefreshImage();
                this.RefreshResult();
            }

            private void buttonSpriteEdit_Click(object sender, EventArgs e)
            {
                Sprite sprite = this.listBoxSprites.SelectedItem as Sprite;
                if (sprite == null)
                    return;
                FormSprite formSprite = new FormSprite(this.SpriteMap, sprite, false);
                if (formSprite.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                this.RefreshSprites();
                this.RefreshImage();
                this.RefreshResult();
            }

            private void buttonSpriteDelete_Click(object sender, EventArgs e)
            {
                Sprite sprite = this.listBoxSprites.SelectedItem as Sprite;
                if (sprite == null)
                    return;
                this.SpriteMap.Sprites.Remove(sprite);
                this.RefreshSprites();
                this.RefreshImage();
                this.RefreshResult();
            }
        #endregion

        #region SpriteMap
            private void RefreshSpriteMap() 
            {
                this.RefreshProperties();
                this.RefreshSprites();
                this.RefreshImages();
                this.RefreshImage();
                this.RefreshResult();
            }

            private void RefreshProperties() 
            {
                this.textBoxName.Text = this.SpriteMap.Name;
                this.textBoxTransparency.Text = this.SpriteMap.Transparency; 
            }

            private void RefreshResult()
            {
                this.SpriteMap.CacheLayers = null;
                Layer layer = this.Game.CreateLayers(this.Resource.Images, this.SpriteMap, 0, 0)[0];
                MemoryStream memo = new MemoryStream(layer.Data.ToArray());
                this.pictureBoxSpritemap.Image = Bitmap.FromStream(memo);
                Sprite sprite = this.listBoxSprites.SelectedItem as Sprite;
                if (sprite == null)
                    return;
                this.pictureBoxSpritemap.Image = CreateRectangle(this.pictureBoxSpritemap.Image, sprite.X, sprite.Y, sprite.Width - 1, sprite.Height - 1, Color.Red);
            }
        #endregion

        #region Images
            private void RefreshImages() 
            {
                this.comboBoxImages.Items.Clear();
                foreach (Data.Image image in this.Resource.Images)
                    this.comboBoxImages.Items.Add(image);
                this.comboBoxImages.SelectedItem = this.Resource.Images.GetImage(this.SpriteMap.ImageName);
            }

            private void RefreshImage() 
            {
                if (this.comboBoxImages.SelectedItem == null)
                    return;
                MemoryStream memo = new MemoryStream((this.comboBoxImages.SelectedItem as Data.Image).Data.ToArray());
                this.pictureBoxImage.Image = Bitmap.FromStream(memo);
                Sprite sprite = this.listBoxSprites.SelectedItem as Sprite;
                if (sprite == null)
                    return;
                this.pictureBoxImage.Image = CreateRectangle(this.pictureBoxImage.Image, sprite.XImage, sprite.YImage, sprite.Width, sprite.Height, Color.Red);
            }
        #endregion

        #region Sprites
            private void RefreshSprites() 
            {
                this.listBoxSprites.Items.Clear();
                foreach (Sprite sprite in this.SpriteMap.Sprites)
                    this.listBoxSprites.Items.Add(sprite);
            }
        #endregion

        #region Bitmap
            private System.Drawing.Image CreateRectangle(System.Drawing.Image image, int x, int y, int width, int height, System.Drawing.Color color) 
            {
                Bitmap imageNew = new Bitmap(image);
                Graphics g = Graphics.FromImage(imageNew);
                g.DrawRectangle(new Pen(color), x, y, width, height);
                return (imageNew);
            }
        #endregion
    }
}
