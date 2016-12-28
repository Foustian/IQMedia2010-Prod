using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.ServiceModel;
using IQMedia.TVEyes.Common.Util;
using System.Configuration;
using IQMedia.Service.TVEyes.Download.Config;
using System.Xml;
using System.ServiceModel.Description;
using System.Data.SqlClient;

namespace IQMedia.Service.TVEyes.Download
{
    public partial class Download : ServiceBase
    {

        public static Download Instance;
        private Thread _workerThread;
        private ServiceHost _host;

        public Download()
        {
            InitializeComponent();
            _workerThread = new Thread(Run);
            _workerThread.IsBackground = true;
            //We need an externally accessable reference to this service...
            Instance = this;
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
            ConfigurationManager.RefreshSection("DownloadSettings");
            //ConfigurationManager.RefreshSection("FFmpegSettings");
            //Test our config file to make sure we have everything we need...
            if (ConfigSettings.Settings == null)
                throw new XmlException("App.config is missing <DownloadSettings> node.");
        }


        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            try
            {
                Logger.Info("Download Service started at: " + DateTime.Now);
                var baseAddress = new Uri("http://localhost:" + ConfigSettings.Settings.WCFServicePort + "/DownloadWebService");

                _host = new ServiceHost(typeof(Service.DownloadWebService), baseAddress);
                // Enable metadata publishing.
                var smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                _host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                _host.Open();

                while (true)
                {
                    EnqueueTasks();
                    Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(ConfigSettings.Settings.PollInterval).TotalMilliseconds));
                }
            }
            catch (AddressAccessDeniedException ex)
            {
                Logger.Fatal("You must start this service with administrative rights.", ex);
                Stop();
            }
            catch (AddressAlreadyInUseException ex)
            {
                Logger.Fatal("The WCF Service Port you have specified for this service is already in use. Please specify another.", ex);
                Stop();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                Stop();
            }
            finally
            {
                if (_host != null && _host.State == CommunicationState.Faulted)
                    _host.Abort();
                else if (_host != null)
                    _host.Close();
            }
        }

        public void EnqueueTasks()
        {
            InitializeService();

            try
            {
                var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                {
                    Logger.Debug("Fetching queued items from database.");
                    conn.Open();
                    var cmdStr = "SELECT ArchiveTVEyesKey,_RootPathID,UTCDateTime,Package,TMGuid "
                                 + "FROM ArchiveTVEyes "
                                 + "WHERE Status = 'Ready_For_Download'";
                    using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                    {
                        var reader = cmd.ExecuteReader();
                        var items = 0;
                        while (reader.Read())
                        {
                            Worker.Instance.Enqueue(new DownloadTask
                            {
                                ID = reader.GetInt64(0),
                                _RootPathID = reader.GetInt64(1),
                                UTCDateTime = reader.GetDateTime(2),
                                Package = reader.GetString(3),
                                TMGuid = reader.GetGuid(4)
                            });
                            items++;
                        }
                        Logger.Info(items + " new items enqueued.");
                    }
                }

                //Let the worker thread sleep until the next poll interval
                Logger.Info(String.Format("RequestDownload Service Enqueuer sleeping for {0} seconds.",
                    ConfigSettings.Settings.PollInterval));
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred while attempting to retrieve new tasks from the database.", ex);
            }
        }

        /// <summary>
        /// Attempts to stop the worker threads somewhat gracefully.
        /// </summary>
        public void Quit()
        {
            Worker.Instance.Stop();
            Logger.Info("Download Service stopped at: " + DateTime.Now);
        }
    }
}
