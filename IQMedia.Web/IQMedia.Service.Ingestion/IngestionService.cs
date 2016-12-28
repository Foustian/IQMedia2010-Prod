using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Xml;
using IQMedia.Common.Util;
using IQMedia.Service.Ingestion.Config;

namespace IQMedia.Service.Ingestion
{
    public partial class IngestionService : ServiceBase
    {
        private Thread _workerThread;

        public IngestionService()
        {
            InitializeComponent();
            _workerThread = new Thread(Run);
            _workerThread.IsBackground = true;
        }

        protected override void OnStart(string[] args)
        {
            _workerThread.Start();
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Quit();
            base.OnStop();
        }

        /// <summary>
        /// Initializes the service and verifies config data.
        /// </summary>
        protected static void InitializeService()
        {
            Logger.Info("Initializing settings and parameters");

            //Re-fetch the config settings...
            ConfigurationManager.RefreshSection("IngestionSettings");
            ConfigurationManager.RefreshSection("IngestionPaths");
            //Test our config file to make sure we have everything we need...
            if(ConfigSettings.Settings == null)
                throw new XmlException("App.config is missing <IngestionSettings> node.");
            if(ConfigSettings.Paths == null)
                throw new XmlException("App.Config is missing <IngestionPaths> node.");
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            try
            {
                Logger.Info("Ingestion Service started at: " + DateTime.Now);
                while(true)
                {
                    Logger.Info("Ingestion Service Enqueuer running at: " + DateTime.Now);
                    InitializeService();

                    Logger.Info(String.Format("There are {0} directories to enqueue.", ConfigSettings.Paths.Directories.Count));
                    var worker = Worker.Instance;
                    foreach (var directory in ConfigSettings.Paths.Directories)
                        worker.Enqueue(new IngestionTask(directory));

                    //Let the worker thread sleep until the next poll interval
                    Logger.Info(String.Format("Ingestion Service Enqueuer sleeping for {0} seconds.", ConfigSettings.Settings.PollInterval));
                    Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(ConfigSettings.Settings.PollInterval).TotalMilliseconds));
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Attempts to stop the worker threads somewhat gracefully.
        /// </summary>
        public void Quit()
        {
            Worker.Instance.Stop();
            Logger.Info("Ingestion Service stopped at: " + DateTime.Now);
        }
    }
}
