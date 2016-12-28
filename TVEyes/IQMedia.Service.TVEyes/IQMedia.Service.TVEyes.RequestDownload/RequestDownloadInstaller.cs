using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace IQMedia.Service.TVEyes.RequestDownload
{
    [RunInstaller(true)]
    public partial class RequestDownloadInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller _processInstaller;
        private readonly ServiceInstaller _svcInstaller;

        public RequestDownloadInstaller()
        {
            InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _svcInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalService;

            _svcInstaller.StartType = ServiceStartMode.Manual;
            _svcInstaller.Description = "TVEyes DownLoad Request Service";
            _svcInstaller.DisplayName = "IQMedia TVEyes Download Request";
            _svcInstaller.ServiceName = "RequestDownload";

            Installers.Add(_svcInstaller);
            Installers.Add(_processInstaller);
        }
    }
}
