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
using IQMediaGroup.Common.IOS.Util;
using System.Configuration;
using System.Xml;
using IQMediaGroup.Services.IOS.ExportClip.Config;
using System.ServiceModel.Description;
using System.Data.SqlClient;


namespace IQMediaGroup.Services.IOS.ExportClip
{
    public partial class ExportService : ServiceBase
    {
        public static ExportService Instance;
        private Thread _workerThread;
        private ServiceHost _host;


        public ExportService()
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
            Log4NetLogger.Info("Initializing settings and parameters");

            //Re-fetch the config settings...
            ConfigurationManager.RefreshSection("ExportSettings");
            
            //Test our config file to make sure we have everything we need...
            if (ConfigSettings.Settings == null)
                throw new XmlException("App.config is missing <ExportSettings> node.");
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            try
            {
                Log4NetLogger.Info("IOS Export Service started at: " + DateTime.Now);
                var baseAddress = new Uri("http://localhost:" + ConfigSettings.Settings.WCFServicePort + "/IOSExportWebSErvice");

                _host = new ServiceHost(typeof(Service.IOSExportWebService), baseAddress);
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
                Log4NetLogger.Fatal("You must start this service with administrative rights.", ex);
                Stop();
            }
            catch (AddressAlreadyInUseException ex)
            {
                Log4NetLogger.Fatal("The WCF Service Port you have specified for this service is already in use. Please specify another.", ex);
                Stop();
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal(ex);
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
                    Log4NetLogger.Debug("Fetching queued items from database.");
                    conn.Open();
                    var cmdStr = "Select IQIOSService_Export.ID, IQIOSService_Export.ClipGuid, IQCore_Recording.StartDate "
                                 + "FROM IQIOSService_Export INNER JOIN IQCore_Clip ON IQIOSService_Export.ClipGuid = IQCore_Clip.Guid " 
	                             + "Inner join IQCore_Recordfile ON IQCore_Clip._RecordfileGuid = IQCore_Recordfile.Guid "
                                 + "INNER JOIN IQCore_Recording ON IQCore_Recordfile._RecordingID = IQCore_Recording.ID "
                                 + "WHERE IQIOSService_Export.Status = 'QUEUED' OR IQIOSService_Export.Status = 'FILE_LOCATION_NOT_FOUND'";
                    using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                    {
                        var reader = cmd.ExecuteReader();
                        var items = 0;
                        while (reader.Read())
                        {
                            Worker.Instance.Enqueue(new ExportTask
                            {
                                id = reader.GetGuid(0),
                                clipGuid = reader.GetGuid(1),
                                startDate  = reader.GetDateTime(2)
                            });
                            items++;
                        }
                        Log4NetLogger.Info(items + " new items enqueued.");
                    }
                }

                //Let the worker thread sleep until the next poll interval
                Log4NetLogger.Info(String.Format("Export Service Enqueuer sleeping for {0} seconds.",
                    ConfigSettings.Settings.PollInterval));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("An error occurred while attempting to retrieve new tasks from the database.", ex);
            }
        }

        /// <summary>
        /// Attempts to stop the worker threads somewhat gracefully.
        /// </summary>
        public void Quit()
        {
            Worker.Instance.Stop();
            Log4NetLogger.Info("IOS Export Service stopped at: " + DateTime.Now);
        }
    }
}
