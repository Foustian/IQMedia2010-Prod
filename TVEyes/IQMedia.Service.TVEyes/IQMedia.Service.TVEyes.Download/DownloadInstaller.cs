using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace IQMedia.Service.TVEyes.Download
{
    [RunInstaller(true)]
    public partial class DownloadInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller _processInstaller;
        private readonly ServiceInstaller _svcInstaller;

        public DownloadInstaller()
        {
            InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _svcInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalService;

            _svcInstaller.StartType = ServiceStartMode.Manual;
            _svcInstaller.Description = "TVEyes DownLoad Service";
            _svcInstaller.DisplayName = "IQMedia TVEyes Download Service";
            _svcInstaller.ServiceName = "Download";

            Installers.Add(_svcInstaller);
            Installers.Add(_processInstaller);
        }
    }
}
