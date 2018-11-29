namespace Cica.CicaServerService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstallerCica = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerCica = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerCica
            // 
            this.serviceProcessInstallerCica.Password = null;
            this.serviceProcessInstallerCica.Username = null;
            // 
            // serviceInstallerCica
            // 
            this.serviceInstallerCica.Description = "Cica Server Service";
            this.serviceInstallerCica.DisplayName = "Cica Server";
            this.serviceInstallerCica.ServiceName = "Cica";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerCica,
            this.serviceInstallerCica});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerCica;
        private System.ServiceProcess.ServiceInstaller serviceInstallerCica;
    }
}