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
using IQMedia.Common.Util;
using IQMedia.Service.Media.News.GeneratePDF.Config;
using System.Configuration;
using System.Xml;
using System.Data.SqlClient;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using System.IO;
using IQMedia.Service.Media.Logic;

namespace IQMedia.Service.Media.News.GeneratePDF
{
    public partial class GeneratePDFService : ServiceBase
    {
        //public static GeneratePDFService Instance;
        private Thread _workerThread;
        private ServiceHost _host;
        private Queue<GeneratePDFTask> _queOfGenPDFTsk;
        private readonly string connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;

        public GeneratePDFService()
        {
            InitializeComponent();
            _workerThread = new Thread(Run);
            _workerThread.IsBackground = true;
            //We need an externally accessable reference to this service...
            //  Instance = this;
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

        public void Run()
        {
            try
            {
                Logger.Info("GeneratePDF Service started at: " + DateTime.Now);
                /*var baseAddress = new Uri("http://localhost:" + ConfigSettings.Settings.WCFServicePort + "/NewsGeneratePDFWebService");

                _host = new ServiceHost(typeof(Service.NewsGeneratePDFWebService), baseAddress);
                // Enable metadata publishing.
                var smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                _host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                _host.Open();*/


                StartTasks();
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

        private void StartTasks()
        {
            while (true)
            {
                InitializeService();

                GetRecordsFromDB();

                while (_queOfGenPDFTsk.Count > 0)
                {
                    var totalNoOfTsk = _queOfGenPDFTsk.Count > Config.ConfigSettings.Settings.NoOfTasks ? Config.ConfigSettings.Settings.NoOfTasks : _queOfGenPDFTsk.Count;

                    List<Task> listOfTask = new List<Task>(totalNoOfTsk);

                    var tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;

                    for (int index = 0; index < totalNoOfTsk; index++)
                    {
                        var tmpPDFTask = _queOfGenPDFTsk.Dequeue();
                        Logger.Info("Assign " + tmpPDFTask.ArticleID + "to Task " + index);
                        listOfTask.Add(Task.Factory.StartNew((object obj) => ProcessDBRecords(tmpPDFTask, token), tmpPDFTask, token));
                    }

                    try
                    {
                        Task.WaitAll(listOfTask.ToArray(), Convert.ToInt32(Config.ConfigSettings.Settings.MaxTimeOut), token);
                        tokenSource.Cancel();
                    }
                    catch (AggregateException ex)
                    {
                        foreach (var item in ex.InnerExceptions)
                        {
                            Logger.Error("AggregateException", item);
                        }

                        Logger.Error("AggregateException ", ex);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Exception in waitall ", ex);
                    }

                    Logger.Info("Waitall complete");

                    foreach (var tsk in listOfTask)
                    {
                        GeneratePDFTask genPDFTsk = (GeneratePDFTask)tsk.AsyncState;

                        Logger.Debug(genPDFTsk.ArticleID + ": Tsk Status- " + tsk.Status);
                        Logger.Debug(genPDFTsk.ArticleID + " GenPDF Status- " + genPDFTsk.Status);

                        // Do it later, that if File is already generated, then though task is cancelled, let it give 5 more secs. to complete whole process.

                        /*if (genPDFTsk.Status == GeneratePDFTask.TskStatus.File_Generated)
                        {
                            Logger.Info(genPDFTsk.ArticleID + " PDF file is already generated, let give 5 secs. to complete process");
                            Thread.Sleep(5000);

                            Logger.Info(genPDFTsk.ArticleID + ": 5 secs. time complete");
                            Logger.Debug(genPDFTsk.ArticleID + ": Tsk Status- " + tsk.Status);
                            Logger.Debug(genPDFTsk.ArticleID + ": GenPDF Status- " + genPDFTsk.Status);

                            if (genPDFTsk.Status != GeneratePDFTask.TskStatus.Generated)
                            {
                                Logger.Debug(genPDFTsk.ArticleID + ": GenPDF Status is still not \"Generate\", might be something going wrong.");
                            }

                            UpdateTaskStatus(GeneratePDFTask.TskStatus.Not_Returned_From_.ToString() + genPDFTsk.Status.ToString(), genPDFTsk.ArticleID);
                        }*/
                    }

                    listOfTask.Clear();
                }

                Logger.Info("GeneratePDF Service Enqueuer sleeping for " + ConfigSettings.Settings.PollInterval + " seconds.");
                Thread.Sleep(Convert.ToInt32(TimeSpan.FromSeconds(ConfigSettings.Settings.PollInterval).TotalMilliseconds));
            }
        }

        private void ProcessDBRecords(GeneratePDFTask p_GeneratePDFTask, CancellationToken p_CT)
        {
            var filename="";

            try
            {
                if (p_CT.IsCancellationRequested)
                {
                    Logger.Info("AID: " + p_GeneratePDFTask.ArticleID + " is cancelled.");
                    p_CT.ThrowIfCancellationRequested();
                }

                Logger.Info("AID: " + p_GeneratePDFTask.ArticleID + " is processing.");

                p_GeneratePDFTask.Status = GeneratePDFTask.TskStatus.IN_PROCESS;
                UpdateTaskStatus(GeneratePDFTask.TskStatus.IN_PROCESS.ToString(), p_GeneratePDFTask.ArticleID);

                 filename = ConfigSettings.Settings.WorkingDirectory + p_GeneratePDFTask.ArticleID + ".pdf";

                Logger.Info("AID: " + p_GeneratePDFTask.ArticleID + " going to generate PDF file.");

                var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;

                short iqLicense=0;
                string iqLicenseSettings = "";
                Int64? clientID=null;
                Int64? customerID = null;

                using (var conn = new SqlConnection(connStr))
                {
                    Logger.Debug("Fetch IQLicense detail.");
                    conn.Open();

                    var cmdStr = "usp_genpdf_ArchiveNM_SelectIQLicenseByArticleID";

                    using (var cmd = conn.GetCommand(cmdStr, CommandType.StoredProcedure))
                    {
                        cmd.Parameters.Add("@ArticleID", SqlDbType.VarChar).Value = p_GeneratePDFTask.ArticleID;
                        cmd.Parameters.Add("@URL", SqlDbType.VarChar).Value = p_GeneratePDFTask.Url;

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            clientID= Convert.ToInt64(reader["ClientID"]);
                            iqLicense = Convert.ToInt16(reader["IQLicense"]);
                            iqLicenseSettings = Convert.ToString(reader["IQLicenseSettings"]);
                            customerID = Convert.ToInt64(reader["CustomerID"]);
                        }                        
                    }
                }

                if (clientID.HasValue)
                {
                    string appendParams = "";

                    if (iqLicense>0)
                    {
                        appendParams = "&u1=cliq40";

                        List<string> license = iqLicenseSettings.Split(',').ToList();

                        if (!license.Contains("2") && iqLicense==1)
                        {
                            appendParams = appendParams + "&u2=1";                            
                        }
                        else
                        {
                            appendParams = appendParams + "&u2=" + clientID;
                        }
                    }

                    var IsTracked = false;

                    if (iqLicense>0)
                    {
                        using (var conn = new SqlConnection(connStr))
                        {
                            Logger.Debug("AID: " + p_GeneratePDFTask.ArticleID + " going to track.");
                            conn.Open();

                            var cmdStr = "usp_v4_IQTrack_LicenseClick_Insert";

                            using (var cmd = conn.GetCommand(cmdStr, CommandType.StoredProcedure))
                            {
                                cmd.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = customerID;
                                cmd.Parameters.Add("@MOURL", SqlDbType.VarChar).Value = p_GeneratePDFTask.Url + appendParams;
                                cmd.Parameters.Add("@Event", SqlDbType.VarChar).Value = "SaveArticle";
                                cmd.Parameters.Add("@IQLicense", SqlDbType.TinyInt).Value = iqLicense;

                                var result = cmd.ExecuteNonQuery();

                                Logger.Debug("AID: " + p_GeneratePDFTask.ArticleID + " result: " + result);

                                if (result>0)
                                {                                    
                                    Logger.Debug("AID: " + p_GeneratePDFTask.ArticleID + " Tracked.");
                                    IsTracked = true;
                                }
                                else
                                {
                                    Logger.Debug("AID: " + p_GeneratePDFTask.ArticleID + " Tracking failed.");
                                    IsTracked = false;

                                    p_GeneratePDFTask.Status = GeneratePDFTask.TskStatus.TRACKING_FAILED;
                                    UpdateTaskStatus(GeneratePDFTask.TskStatus.TRACKING_FAILED.ToString(), p_GeneratePDFTask.ArticleID);

                                    return;
                                }
                            }
                        } 
                    }
                    else
                    {
                        IsTracked = true;
                    }

                    if (IsTracked)
                    {
                        if (GeneratePDFFile(p_GeneratePDFTask.Url, filename, appendParams))
                        {
                            p_GeneratePDFTask.Status = GeneratePDFTask.TskStatus.File_Generated;
                            UpdateTaskStatus(GeneratePDFTask.TskStatus.File_Generated.ToString(), p_GeneratePDFTask.ArticleID);

                            if (p_CT.IsCancellationRequested)
                            {
                                Logger.Info("AID: " + p_GeneratePDFTask.ArticleID + " is cancelled.");
                                p_CT.ThrowIfCancellationRequested();
                            }

                            Logger.Info("AID: " + p_GeneratePDFTask.ArticleID + " PDF is generated. Going to copy file to destination.");

                            var rp = (RootPathLogic)LogicFactory.GetLogic(LogicType.RootPath);

                            var outputPath = rp.GetRootPathByID(p_GeneratePDFTask.RootPathID).StoragePath + "\\" + p_GeneratePDFTask.Harvest_Time.Year + "\\" + p_GeneratePDFTask.Harvest_Time.Month + "\\" + p_GeneratePDFTask.Harvest_Time.Day + "\\";

                            if (FileHelper.CopyFile(filename, outputPath + p_GeneratePDFTask.ArticleID + ".pdf"))
                            {
                                p_GeneratePDFTask.Status = GeneratePDFTask.TskStatus.Generated;

                                using (var conn = new SqlConnection(connStr))
                                {
                                    conn.Open();
                                    var cmdStr = "UPDATE IQCore_NM "
                                                 + "SET Status='" + GeneratePDFTask.TskStatus.Generated.ToString() + "'"
                                                       + ",LastModified='" + DateTime.Now + "'"
                                                       + ",Location='" + p_GeneratePDFTask.Harvest_Time.Year + "\\" + p_GeneratePDFTask.Harvest_Time.Month + "\\" + p_GeneratePDFTask.Harvest_Time.Day + "\\" + p_GeneratePDFTask.ArticleID + ".pdf" + "'"
                                                 + "WHERE ArticleID='" + p_GeneratePDFTask.ArticleID + "'";
                                    using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                        cmd.ExecuteNonQuery();
                                }

                                /*UpdateTaskStatus(GeneratePDFTask.TskStatus.Generated.ToString(), p_GeneratePDFTask.ArticleID);*/
                            }
                            else
                            {
                                Logger.Info("AID: " + p_GeneratePDFTask.ArticleID + " unable to copy file to destination, " + outputPath + p_GeneratePDFTask.ArticleID + ".pdf");

                                p_GeneratePDFTask.Status = GeneratePDFTask.TskStatus.Unable_To_Copy;
                                UpdateTaskStatus(GeneratePDFTask.TskStatus.Unable_To_Copy.ToString(), p_GeneratePDFTask.ArticleID);
                            }
                        }
                        else
                        {
                            Logger.Info("AID: " + p_GeneratePDFTask.ArticleID + " PDF File could not be generated.");

                            p_GeneratePDFTask.Status = GeneratePDFTask.TskStatus.File_Not_Generated;
                            UpdateTaskStatus(GeneratePDFTask.TskStatus.File_Not_Generated.ToString(), p_GeneratePDFTask.ArticleID);
                        }  
                    }
                }
                else
                {
                    Logger.Info("AID: " + p_GeneratePDFTask.ArticleID + " Invalid Client.");

                    p_GeneratePDFTask.Status = GeneratePDFTask.TskStatus.Invalid_Client;
                    UpdateTaskStatus(GeneratePDFTask.TskStatus.Invalid_Client.ToString(), p_GeneratePDFTask.ArticleID);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("AID: " + p_GeneratePDFTask.ArticleID + " error ", ex);

                p_GeneratePDFTask.Status = GeneratePDFTask.TskStatus.Exception;
                UpdateTaskStatus(GeneratePDFTask.TskStatus.Exception.ToString(), p_GeneratePDFTask.ArticleID);

                throw;
            }
            finally
            {
                if (!string.IsNullOrEmpty(filename))
                {
                    FileHelper.DeleteFile(filename);
                }
            }
        }

        private void UpdateTaskStatus(string p_Status, Int64 p_ArticleID)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmdStr = "UPDATE IQCore_NM "
                             + "SET Status='" + p_Status + "', LastModified='" + DateTime.Now + "' "
                             + "WHERE ArticleID='" + p_ArticleID + "'";
                using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                    cmd.ExecuteNonQuery();
            }
        }

        private bool GeneratePDFFile(string p_url, string p_filename, string p_appendParams)
        {
            if (!string.IsNullOrWhiteSpace(p_appendParams))
            {
                p_url = p_url + p_appendParams;
            }

            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            //NOTE: there seems to be an issue with the redirect
            startInfo.RedirectStandardError = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = ConfigSettings.Settings.WkhtmltopdfPath;
            startInfo.Arguments = p_url + " " + p_filename;

            try
            {
                Logger.Debug(startInfo.FileName + " " + startInfo.Arguments);
                using (var exeProcess = Process.Start(startInfo))
                {
                    if (!exeProcess.WaitForExit(ConfigSettings.Settings.WkhtmltopdfTimeout))
                    {
                        Logger.Debug("AID File: " + p_filename + " kill process.");

                        exeProcess.Kill();
                    }
                }

                return File.Exists(p_filename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }


        /// <summary>
        /// Attempts to stop the worker threads somewhat gracefully.
        /// </summary>
        public void Quit()
        {
            //Worker.Instance.Stop();
            Logger.Info("GeneratePDF Service stopped at: " + DateTime.Now);
        }

        /// <summary>
        /// Initializes the service and verifies config data.
        /// </summary>
        protected static void InitializeService()
        {
            Logger.Info("Initializing settings and parameters");

            //Re-fetch the config settings...
            ConfigurationManager.RefreshSection("GeneratePDFSettings");

            //Test our config file to make sure we have everything we need...
            if (ConfigSettings.Settings == null)
                throw new XmlException("App.config is missing <GeneratePDFSettings> node.");
        }

        public void GetRecordsFromDB()
        {
            try
            {
                var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                _queOfGenPDFTsk = new Queue<GeneratePDFTask>();

                using (var conn = new SqlConnection(connStr))
                {
                    Logger.Debug("Fetching queued items from database.");
                    conn.Open();

                    var cmdStr = "usp_genpdf_IQCore_NM_SelectQueued";

                    using (var cmd = conn.GetCommand(cmdStr, CommandType.StoredProcedure))
                    {
                        cmd.Parameters.Add("@TopRows", SqlDbType.Int).Value = Config.ConfigSettings.Settings.QueueLimit;
                        cmd.Parameters.Add("@MachineName", SqlDbType.VarChar).Value = System.Environment.MachineName;

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            _queOfGenPDFTsk.Enqueue(new GeneratePDFTask(Convert.ToInt64(reader["ArticleID"]),
                                                                 Convert.ToString(reader["Url"]),
                                                                 Convert.ToDateTime(reader["Harvest_Time"]),
                                                                 Convert.ToInt32(reader["_RootPathID"])
                                                                 )
                                            );
                        }
                        Logger.Info(_queOfGenPDFTsk.Count + " new items enqueued.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred while attempting to retrieve new tasks from the database.", ex);
            }
        }


    }
}
