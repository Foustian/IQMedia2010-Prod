using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using IQMedia.Common.Threading;
using IQMedia.Common.Util;
using IQMedia.Service.NM.GeneratePDF.Config;
using System.Net;
using System.Text;
using IQMedia.Logic.NM;


namespace IQMedia.Service.NM.GeneratePDF
{
    public class Worker
    {
        #region Attributes
        private static readonly object _lock = new object();
        private static Worker _worker;
        private static ThreadSafeQueue<GeneratePDFTask> _queue = new ThreadSafeQueue<GeneratePDFTask>();
        private readonly Thread[] _threadPool;
        private List<GeneratePDFTask> _activeTasks;
        #endregion

        public static Worker Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_worker == null)
                    {
                        _worker = new Worker(ConfigSettings.Settings.WorkerThreads);
                        _worker.Start();
                    }
                }
                return _worker;
            }
        }

        #region Constructors
        private Worker(int threadPoolSize)
        {
            _threadPool = new Thread[threadPoolSize];
            _activeTasks = new List<GeneratePDFTask>(threadPoolSize);
        }
        #endregion

        #region Operations

        private void Start()
        {
            // loop through and fire up the threads
            for (var i = 0; i < _threadPool.Length; i++)
            {
                //changed from changes of theading issues resolved.
                //_threadPool[i] = new Thread(Execute);
                _threadPool[i] = new Thread(new ThreadStart(Execute));
                _threadPool[i].Name = "Worker " + i;
                _threadPool[i].IsBackground = true;
                _threadPool[i].Start();

                
                
            }
        }

        public void Stop()
        {
            Logger.Info("Stopping running threads...");
            foreach (var t in _threadPool)
                //If the thread is currently running, give it 5 seconds to quit...
                if (t.IsAlive) t.Join(5000);
        }

        public void Enqueue(GeneratePDFTask task)
        {
            if (!_queue.Contains(task))
            {
                Logger.Debug("GeneratePDF Service is enqueing task: " + task.ArticleID);
                _queue.Enqueue(task);
            }
            else Logger.Debug(String.Format("GeneratePDF Service is skipping task {0} because it is already enqueued.", task.ArticleID));
        }

        protected void Execute()
        {
            while (true)
            {
                GeneratePDFTask task = null;
                string inputFile = null;
                string outputFile = null;

                var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;

                try
                {
                    Logger.Debug("Waiting for data to dequeue from the queue.");
                    task = _queue.DequeueOrWait();
                    if (_activeTasks.Contains(task))
                    {
                        Logger.Debug(String.Format("The task {0} is being processed by another thread and will be skipped.", task.ArticleID));
                        continue;
                    }

                    Logger.Info("Processing task: " + task.ArticleID);
                    _activeTasks.Add(task);

                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var cmdStr = "UPDATE IQCore_NM "
                                     + "SET Status='IN_PROCESS', LastModified='" + DateTime.Now + "' "
                                     + "WHERE ArticleID='" + task.ArticleID + "'";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                            cmd.ExecuteNonQuery();
                    }
                                                           
                    var filename = task.ArticleID + ".pdf" ;
                    outputFile = ConfigSettings.Settings.WorkingDirectory + filename;

                    if (GenerateHtmlFile(task.ArticleID, task.Url))
                    {
                        inputFile = ConfigSettings.Settings.WorkingDirectory + task.ArticleID + ".html";

                        var wkhtmltopdfParams = inputFile + " " + outputFile;

                        if (RunConvertProcess(outputFile, wkhtmltopdfParams))
                        {
                            var rp = (RootPathLogic) LogicFactory.GetLogic(LogicType.RootPath);
                            
                            var outputPath = rp.GetRootPathByID(task.RootPathID).StoragePath+"\\" + task.Harvest_Time.Year + "\\" + task.Harvest_Time.Month + "\\" + task.Harvest_Time.Day + "\\";

                            if (FileHelper.CopyFile(outputFile, outputPath + filename))
                            {
                                using (var conn = new SqlConnection(connStr))
                                {
                                    conn.Open();
                                    var cmdStr = "UPDATE IQCore_NM "
                                                 + "SET Status='Generated', LastModified='" + DateTime.Now + "', Location='" + task.Harvest_Time.Year + "\\" + task.Harvest_Time.Month + "\\" + task.Harvest_Time.Day + "\\" +filename+ "' "
                                                 + "WHERE ArticleID='" + task.ArticleID + "'";
                                    using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                        cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                Logger.Error("An error occurred while copying the generated file to its output path.");
                                using (var conn = new SqlConnection(connStr))
                                {
                                    conn.Open();
                                    var cmdStr = "UPDATE IQCore_NM "
                                                 + "SET Status='FAILED_COPY_TO_OUTPUT', LastModified='" + DateTime.Now + "' "
                                                 + "WHERE ArticleID='" + task.ArticleID + "'";
                                    using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                        cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            Logger.Error("The file failed to convert successfully: " + task.ArticleID);
                            using (var conn = new SqlConnection(connStr))
                            {
                                conn.Open();
                                var cmdStr = "UPDATE IQCore_NM "
                                             + "SET Status='FAILED_TO_GENERATE_PDF', LastModified='" + DateTime.Now + "' "
                                             + "WHERE ArticleID='" + task.ArticleID + "'";
                                using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                    cmd.ExecuteNonQuery();
                            }
                        } 
                    }
                    else
                    {
                        Logger.Error("Unable to generate html file for: "+task.ArticleID);
                    }
                }
                catch (Exception ex)
                {
                    var dir = (task != null) ? " (" + task.ArticleID + ")" : "";
                    Logger.Error("An error occurred while processing generate PDF task." + dir, ex);

                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var cmdStr = "UPDATE IQCore_NM "
                                     + "SET Status='FAILED', LastModified='" + DateTime.Now + "' "
                                     + "WHERE ArticleID='" + task.ArticleID + "'";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                            cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    //We put this in the finally block because, no matter what happens, we want the active task removed
                    if (task != null) _activeTasks.Remove(task);

                    //We delete the local file no matter what...
                    if (inputFile != null)
                        FileHelper.DeleteFile(inputFile);
                    if (outputFile != null)
                        FileHelper.DeleteFile(outputFile);
                }
            }
        }

        private bool GenerateHtmlFile(string articleID, string url)
        {
            try
            {
                Logger.Debug("Generating html file for article: " + articleID);

                WebClient wc = new WebClient();
                string htmlContent = wc.DownloadString(url);

                using (FileStream fs = new FileStream(ConfigSettings.Settings.WorkingDirectory+articleID+".html", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(htmlContent);
                    }
                }

                FileInfo file = new FileInfo(ConfigSettings.Settings.WorkingDirectory + articleID + ".html");

                if (file.Exists && file.Length>0)
                {
                    return true;
                }
                else
                {                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        private static bool RunConvertProcess(string outputFile, string wkhtmltopdfParams)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            //NOTE: there seems to be an issue with the redirect
            startInfo.RedirectStandardError = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = ConfigSettings.Settings.wkhtmltopdfPath;
            startInfo.Arguments = wkhtmltopdfParams;

            try
            {
                Logger.Debug(startInfo.FileName + " " + startInfo.Arguments);
                using (var exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    //NOTE: Can't do this; see error above about redirect
                    //var res = exeProcess.StandardError.ReadToEnd();
                    //Logger.Debug(res);
                }

                return File.Exists(outputFile);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        #endregion
    }
}
