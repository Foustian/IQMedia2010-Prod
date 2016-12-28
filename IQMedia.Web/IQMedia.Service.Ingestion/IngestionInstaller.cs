using System.ComponentModel;
using System.ServiceProcess;


namespace IQMedia.Service.Ingestion
{
    [RunInstaller(true)]
    public partial class IngestionInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller _processInstaller;
        private readonly ServiceInstaller _svcInstaller;

        public IngestionInstaller()
        {
            InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _svcInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalService;

            _svcInstaller.StartType = ServiceStartMode.Manual;
            _svcInstaller.Description = "Moves media files from ingestion to storage.";
            _svcInstaller.DisplayName = "IQMedia Ingestion Service";
            _svcInstaller.ServiceName = "IngestionService";

            Installers.Add(_svcInstaller);
            Installers.Add(_processInstaller);
        }
    }
}
