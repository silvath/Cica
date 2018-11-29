namespace CicaServerCatchWinForms
{
    partial class FormServer
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.buttonResourcesCreate = new System.Windows.Forms.Button();
            this.tabLobby = new System.Windows.Forms.TabPage();
            this.tabSessions = new System.Windows.Forms.TabPage();
            this.tabConnections = new System.Windows.Forms.TabPage();
            this.tabResources = new System.Windows.Forms.TabPage();
            this.buttonResourcesRefresh = new System.Windows.Forms.Button();
            this.listBoxResources = new System.Windows.Forms.ListBox();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.tabResources.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDebug);
            this.tabControl1.Controls.Add(this.tabLobby);
            this.tabControl1.Controls.Add(this.tabSessions);
            this.tabControl1.Controls.Add(this.tabConnections);
            this.tabControl1.Controls.Add(this.tabResources);
            this.tabControl1.Controls.Add(this.tabLog);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(873, 495);
            this.tabControl1.TabIndex = 0;
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.buttonResourcesCreate);
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Size = new System.Drawing.Size(865, 469);
            this.tabDebug.TabIndex = 3;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // buttonResourcesCreate
            // 
            this.buttonResourcesCreate.Location = new System.Drawing.Point(13, 15);
            this.buttonResourcesCreate.Name = "buttonResourcesCreate";
            this.buttonResourcesCreate.Size = new System.Drawing.Size(122, 23);
            this.buttonResourcesCreate.TabIndex = 0;
            this.buttonResourcesCreate.Text = "Create Resources";
            this.buttonResourcesCreate.UseVisualStyleBackColor = true;
            this.buttonResourcesCreate.Click += new System.EventHandler(this.buttonResourcesCreate_Click);
            // 
            // tabLobby
            // 
            this.tabLobby.Location = new System.Drawing.Point(4, 22);
            this.tabLobby.Name = "tabLobby";
            this.tabLobby.Padding = new System.Windows.Forms.Padding(3);
            this.tabLobby.Size = new System.Drawing.Size(865, 469);
            this.tabLobby.TabIndex = 1;
            this.tabLobby.Text = "Lobby";
            this.tabLobby.UseVisualStyleBackColor = true;
            // 
            // tabSessions
            // 
            this.tabSessions.Location = new System.Drawing.Point(4, 22);
            this.tabSessions.Name = "tabSessions";
            this.tabSessions.Padding = new System.Windows.Forms.Padding(3);
            this.tabSessions.Size = new System.Drawing.Size(865, 469);
            this.tabSessions.TabIndex = 0;
            this.tabSessions.Text = "Sessions";
            this.tabSessions.UseVisualStyleBackColor = true;
            // 
            // tabConnections
            // 
            this.tabConnections.Location = new System.Drawing.Point(4, 22);
            this.tabConnections.Name = "tabConnections";
            this.tabConnections.Size = new System.Drawing.Size(865, 469);
            this.tabConnections.TabIndex = 2;
            this.tabConnections.Text = "Connections";
            this.tabConnections.UseVisualStyleBackColor = true;
            // 
            // tabResources
            // 
            this.tabResources.Controls.Add(this.buttonResourcesRefresh);
            this.tabResources.Controls.Add(this.listBoxResources);
            this.tabResources.Location = new System.Drawing.Point(4, 22);
            this.tabResources.Name = "tabResources";
            this.tabResources.Size = new System.Drawing.Size(865, 469);
            this.tabResources.TabIndex = 4;
            this.tabResources.Text = "Resources";
            this.tabResources.UseVisualStyleBackColor = true;
            // 
            // buttonResourcesRefresh
            // 
            this.buttonResourcesRefresh.Location = new System.Drawing.Point(3, 13);
            this.buttonResourcesRefresh.Name = "buttonResourcesRefresh";
            this.buttonResourcesRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonResourcesRefresh.TabIndex = 1;
            this.buttonResourcesRefresh.Text = "Refresh";
            this.buttonResourcesRefresh.UseVisualStyleBackColor = true;
            this.buttonResourcesRefresh.Click += new System.EventHandler(this.buttonResourcesRefresh_Click);
            // 
            // listBoxResources
            // 
            this.listBoxResources.FormattingEnabled = true;
            this.listBoxResources.Location = new System.Drawing.Point(3, 42);
            this.listBoxResources.Name = "listBoxResources";
            this.listBoxResources.Size = new System.Drawing.Size(859, 420);
            this.listBoxResources.TabIndex = 0;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.textBoxLog);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Size = new System.Drawing.Size(865, 469);
            this.tabLog.TabIndex = 5;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(3, 3);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(859, 463);
            this.textBoxLog.TabIndex = 0;
            // 
            // FormServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 519);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormServer";
            this.Text = "Catch";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.tabControl1.ResumeLayout(false);
            this.tabDebug.ResumeLayout(false);
            this.tabResources.ResumeLayout(false);
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLobby;
        private System.Windows.Forms.TabPage tabSessions;
        private System.Windows.Forms.TabPage tabConnections;
        private System.Windows.Forms.TabPage tabDebug;
        private System.Windows.Forms.Button buttonResourcesCreate;
        private System.Windows.Forms.TabPage tabResources;
        private System.Windows.Forms.Button buttonResourcesRefresh;
        private System.Windows.Forms.ListBox listBoxResources;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.TextBox textBoxLog;
    }
}

