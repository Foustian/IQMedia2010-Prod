using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace IQMediaGroup.Services.IOS.ExportClip
{
    [RunInstaller(true)]
    public partial class ExportInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller _processInstaller;
        private readonly ServiceInstaller _svcInstaller;


        public ExportInstaller()
        {
            InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _svcInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalService;

            _svcInstaller.StartType = ServiceStartMode.Manual;
            _svcInstaller.Description = "Export Clip for IOS.";
            _svcInstaller.DisplayName = "IQMedia IOS ClipExport Service";
            _svcInstaller.ServiceName = "IOSExportClip";

            Installers.Add(_svcInstaller);
            Installers.Add(_processInstaller);

        }
    }
}
