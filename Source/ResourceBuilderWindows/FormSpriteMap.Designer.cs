namespace ResourceBuilderWindows
{
    partial class FormSpriteMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.comboBoxImages = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxSprites = new System.Windows.Forms.ListBox();
            this.buttonSpriteDelete = new System.Windows.Forms.Button();
            this.buttonSpriteEdit = new System.Windows.Forms.Button();
            this.buttonSpriteNew = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBoxSpritemap = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTransparency = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpritemap)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(1030, 427);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 25);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(965, 427);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(61, 25);
            this.buttonOk.TabIndex = 3;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pictureBoxImage);
            this.groupBox1.Controls.Add(this.comboBoxImages);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(547, 411);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxImage.Location = new System.Drawing.Point(10, 38);
            this.pictureBoxImage.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(533, 362);
            this.pictureBoxImage.TabIndex = 1;
            this.pictureBoxImage.TabStop = false;
            // 
            // comboBoxImages
            // 
            this.comboBoxImages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxImages.DisplayMember = "Name";
            this.comboBoxImages.FormattingEnabled = true;
            this.comboBoxImages.Location = new System.Drawing.Point(10, 16);
            this.comboBoxImages.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxImages.Name = "comboBoxImages";
            this.comboBoxImages.Size = new System.Drawing.Size(534, 21);
            this.comboBoxImages.TabIndex = 0;
            this.comboBoxImages.ValueMember = "Name";
            this.comboBoxImages.SelectedIndexChanged += new System.EventHandler(this.comboBoxImages_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listBoxSprites);
            this.groupBox2.Controls.Add(this.buttonSpriteDelete);
            this.groupBox2.Controls.Add(this.buttonSpriteEdit);
            this.groupBox2.Controls.Add(this.buttonSpriteNew);
            this.groupBox2.Location = new System.Drawing.Point(559, 120);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(225, 299);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sprites";
            // 
            // listBoxSprites
            // 
            this.listBoxSprites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSprites.DisplayMember = "Name";
            this.listBoxSprites.FormattingEnabled = true;
            this.listBoxSprites.Location = new System.Drawing.Point(4, 45);
            this.listBoxSprites.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxSprites.Name = "listBoxSprites";
            this.listBoxSprites.Size = new System.Drawing.Size(217, 251);
            this.listBoxSprites.TabIndex = 1;
            this.listBoxSprites.ValueMember = "Name";
            this.listBoxSprites.SelectedIndexChanged += new System.EventHandler(this.listBoxSprites_SelectedIndexChanged);
            // 
            // buttonSpriteDelete
            // 
            this.buttonSpriteDelete.Location = new System.Drawing.Point(151, 16);
            this.buttonSpriteDelete.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSpriteDelete.Name = "buttonSpriteDelete";
            this.buttonSpriteDelete.Size = new System.Drawing.Size(69, 25);
            this.buttonSpriteDelete.TabIndex = 11;
            this.buttonSpriteDelete.Text = "Delete";
            this.buttonSpriteDelete.UseVisualStyleBackColor = true;
            this.buttonSpriteDelete.Click += new System.EventHandler(this.buttonSpriteDelete_Click);
            // 
            // buttonSpriteEdit
            // 
            this.buttonSpriteEdit.Location = new System.Drawing.Point(77, 16);
            this.buttonSpriteEdit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSpriteEdit.Name = "buttonSpriteEdit";
            this.buttonSpriteEdit.Size = new System.Drawing.Size(69, 25);
            this.buttonSpriteEdit.TabIndex = 10;
            this.buttonSpriteEdit.Text = "Edit";
            this.buttonSpriteEdit.UseVisualStyleBackColor = true;
            this.buttonSpriteEdit.Click += new System.EventHandler(this.buttonSpriteEdit_Click);
            // 
            // buttonSpriteNew
            // 
            this.buttonSpriteNew.Location = new System.Drawing.Point(4, 16);
            this.buttonSpriteNew.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSpriteNew.Name = "buttonSpriteNew";
            this.buttonSpriteNew.Size = new System.Drawing.Size(69, 25);
            this.buttonSpriteNew.TabIndex = 9;
            this.buttonSpriteNew.Text = "New";
            this.buttonSpriteNew.UseVisualStyleBackColor = true;
            this.buttonSpriteNew.Click += new System.EventHandler(this.buttonSpriteNew_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.pictureBoxSpritemap);
            this.groupBox3.Location = new System.Drawing.Point(788, 8);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(307, 411);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result";
            // 
            // pictureBoxSpritemap
            // 
            this.pictureBoxSpritemap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxSpritemap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxSpritemap.Location = new System.Drawing.Point(4, 16);
            this.pictureBoxSpritemap.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxSpritemap.Name = "pictureBoxSpritemap";
            this.pictureBoxSpritemap.Size = new System.Drawing.Size(294, 391);
            this.pictureBoxSpritemap.TabIndex = 0;
            this.pictureBoxSpritemap.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.textBoxTransparency);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.textBoxName);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(559, 8);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(225, 109);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Properties";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(7, 32);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(215, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // textBoxTransparency
            // 
            this.textBoxTransparency.Location = new System.Drawing.Point(7, 73);
            this.textBoxTransparency.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTransparency.Name = "textBoxTransparency";
            this.textBoxTransparency.Size = new System.Drawing.Size(215, 20);
            this.textBoxTransparency.TabIndex = 3;
            this.textBoxTransparency.TextChanged += new System.EventHandler(this.textBoxTransparency_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 57);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Transparency";
            // 
            // FormSpriteMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 463);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormSpriteMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SpriteMap";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpritemap)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.ComboBox comboBoxImages;
        private System.Windows.Forms.Button buttonSpriteDelete;
        private System.Windows.Forms.Button buttonSpriteEdit;
        private System.Windows.Forms.Button buttonSpriteNew;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBoxSprites;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxSpritemap;
        private System.Windows.Forms.TextBox textBoxTransparency;
        private System.Windows.Forms.Label label2;
    }
}