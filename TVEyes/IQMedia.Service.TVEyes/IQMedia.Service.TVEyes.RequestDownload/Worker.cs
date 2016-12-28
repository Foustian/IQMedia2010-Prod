using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.TVEyes.Common.Threading;
using System.Threading;
using IQMedia.TVEyes.Common.Util;
using IQMedia.Service.TVEyes.RequestDownload.Config;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace IQMedia.Service.TVEyes.RequestDownload
{
    public class Worker
    {
        #region Attributes
        private static readonly object _lock = new object();
        private static Worker _worker;
        private static ThreadSafeQueue<RequestDownloadTask> _queue = new ThreadSafeQueue<RequestDownloadTask>();
        private readonly Thread[] _threadPool;
        private List<RequestDownloadTask> _activeTasks;
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
            _activeTasks = new List<RequestDownloadTask>(threadPoolSize);
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

        public void Enqueue(RequestDownloadTask task)
        {
            if (!_queue.Contains(task))
            {
                Logger.Debug("RequestDownload Service is enqueing task: " + task.ID);
                _queue.Enqueue(task);
            }
            else Logger.Debug(String.Format("RequestDownload Service is skipping task {0} because it is already enqueued.", task.ID));
        }

        protected void Execute()
        {
            while (true)
            {
                RequestDownloadTask task = null;

                try
                {
                    Logger.Debug("Waiting for data to dequeue from the queue.");
                    task = _queue.DequeueOrWait();
                    if (_activeTasks.Contains(task))
                    {
                        Logger.Debug(String.Format("The task {0} is being processed by another thread and will be skipped.", task.ID));
                        continue;
                    }

                    Logger.Info("Processing task: " + task.ID);
                    _activeTasks.Add(task);

                    var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var cmdStr = "UPDATE ArchiveTVEyes "
                                     + "SET Status='IN_PROCESS' "
                                     + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                            cmd.ExecuteNonQuery();
                    }

                    Logger.Info("stationid : " + task.StationID + "  and local datetime : " + task.LocalDateTime.ToString() + " for task id :" + task.ID);

                    var DownloadRequestUrl = string.Format(ConfigSettings.Settings.DownloadRequestURL, task.StationID, task.LocalDateTime.ToString("M/d/yyy hh:mm:ss tt"), task.Duration, string.Format(ConfigurationManager.AppSettings["ExternalNofifyUrl"], task.ID));
                    var webServiceUrl = string.Format(ConfigSettings.Settings.ProxyServerURL,ConfigSettings.Settings.ParentID,System.Web.HttpUtility.UrlEncode(DownloadRequestUrl));

                    Logger.Info("calling download webrequest for id :" + task.ID);
                    Logger.Info("webrequest url :" + webServiceUrl);

                    string response = DoHttpGetRequest(webServiceUrl);

                    Logger.Info("web request competed for id : " + task.ID);
                    Logger.Info("parse response xml : " + response);

                    XDocument xdoc = XDocument.Parse(response);

                    Logger.Info("parse response successfully for task :"+task.ID);

                    if (xdoc.Descendants(ConfigurationManager.AppSettings["SuccessResponseTag"]).Count() > 0)
                    {
                        Logger.Info("download request sent successfully for task :" + task.ID);
                        using (var conn = new SqlConnection(connStr))
                        {
                            conn.Open();
                            var cmdStr = "UPDATE ArchiveTVEyes "
                                            + "SET Status='DOWNLOAD_IN_PROCESS', "
                                            + "Response='" + new System.Data.SqlTypes.SqlXml(xdoc.CreateReader()).Value.Replace("'","''") + "' "
                                            + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                            using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Logger.Info("exeption occured on download request for task :" + task.ID);
                        using (var conn = new SqlConnection(connStr))
                        {
                            conn.Open();
                            var cmdStr = "UPDATE ArchiveTVEyes "
                                            + "SET Status='DOWNLOAD_REQUEST_FAILED', "
                                            + "Response='" + new System.Data.SqlTypes.SqlXml(xdoc.CreateReader()).Value.Replace("'", "''") + "' "
                                            + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                            using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                cmd.ExecuteNonQuery();
                        }
                    }
                    _activeTasks.Remove(task);

                }
                catch (Exception ex)
                {
                    var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var cmdStr = "UPDATE ArchiveTVEyes "
                                     + "SET Status='Exception' "
                                     + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                            cmd.ExecuteNonQuery();
                    }
                    _activeTasks.Remove(task);
                    var dir = (task != null) ? " (" + task.ID + ")" : "";
                    Logger.Error("An error occurred while processing RequestDownload task." + dir, ex);
                }
            }
        }

        #endregion

        #region utility methods
        public string DoHttpGetRequest(string p_URL)
        {
            try
            {
                Uri _Uri = new Uri(p_URL);
                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);

                _objWebRequest.Timeout = 100000000;
                _objWebRequest.Method = "GET";

                StreamReader _StreamReader = null;

                string _ResponseRawMedia = string.Empty;

                if ((_objWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_objWebRequest.GetResponse().GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                return _ResponseRawMedia;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
