using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace IQMedia.Service.SM.GeneratePDF
{
    [RunInstaller(true)]
    public partial class GeneratePDFInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller _processInstaller;
        private readonly ServiceInstaller _svcInstaller;

        public GeneratePDFInstaller()
        {
            InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _svcInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalService;

            _svcInstaller.StartType = ServiceStartMode.Manual;
            _svcInstaller.Description = "Generate pdf of social media article saved by user.";
            _svcInstaller.DisplayName = "IQMedia Generate PDF Service SM";
            _svcInstaller.ServiceName = "SMGeneratePDFService";

            Installers.Add(_svcInstaller);
            Installers.Add(_processInstaller);
        }
    }
}
