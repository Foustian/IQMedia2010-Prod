using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Common.IOS.Threading;
using System.Threading;
using IQMediaGroup.Common.IOS.Util;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using IQMediaGroup.Services.IOS.ExportClip.Config;
using System.IO;
using System.Diagnostics;

namespace IQMediaGroup.Services.IOS.ExportClip
{
    public class Worker
    {

        #region Attributes
        private static readonly object _lock = new object();
        private static Worker _worker;
        private static ThreadSafeQueue<ExportTask> _queue = new ThreadSafeQueue<ExportTask>();
        private readonly Thread[] _threadPool;
        private List<ExportTask> _activeTasks;
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
            _activeTasks = new List<ExportTask>(threadPoolSize);
        }
        #endregion


        #region Operations

        private void Start()
        {
            // loop through and fire up the threads
            for (var i = 0; i < _threadPool.Length; i++)
            {
                _threadPool[i] = new Thread(Execute);
                _threadPool[i].Name = "Worker " + i;
                _threadPool[i].IsBackground = true;
                _threadPool[i].Start();
            }
        }

        public void Stop()
        {
            Log4NetLogger.Info("Stopping running threads...");
            foreach (var t in _threadPool)
                //If the thread is currently running, give it 5 seconds to quit...
                if (t.IsAlive) t.Join(5000);
        }

        public void Enqueue(ExportTask task)
        {
            if (!_queue.Contains(task))
            {
                Log4NetLogger.Debug("Export Service is enqueing task: " + task.id);
                _queue.Enqueue(task);
            }
            else Log4NetLogger.Debug(String.Format("Export Service is skipping task {0} because it is already enqueued.", task.id));
        }

        protected void Execute()
        {
            while (true)
            {
                ExportTask task = null;
                
                string outputFile = null;
                string status = string.Empty;
                string destinationPath = string.Empty;
                string convfile = string.Empty;
                var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;

                try
                {
                    Log4NetLogger.Debug("Waiting for data to dequeue from the queue.");
                    task = _queue.DequeueOrWait();
                    if (_activeTasks.Contains(task))
                    {
                        Log4NetLogger.Debug(String.Format("The task {0} is being processed by another thread and will be skipped.", task.id));
                        continue;
                    }

                    Log4NetLogger.Info("Processing task: " + task.id);
                    _activeTasks.Add(task);


                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var cmdStr = "UPDATE IQIOSService_Export "
                                     + "SET Status='IN_PROCESS', LastModified='" + DateTime.Now + "' "
                                     + "WHERE ID='" + task.id + "'";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                            cmd.ExecuteNonQuery();
                    }

                    Log4NetLogger.Info("task set IN_PROCESS : " + task.id);

                    string clipDirection = ConfigurationManager.AppSettings["IOSClipDirection"];
                    outputFile = ConfigurationManager.AppSettings["IOSClipLocation"];
                    var filePath = string.Empty;
                    var fileName = string.Empty;

                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();

                        var cmdStr = "SELECT [FileLocation],[FileName] " +
                                        "FROM " +
                                "( SELECT " +
                                        "IQCore_ClipMeta.Field,IQCore_ClipMeta.Value " +
                                    "FROM " +
                                            "IQCore_ClipMeta  with(nolock)	" +
                                    "Where " +
                                            "_clipGuid = '" + task.clipGuid + "' " +
                                ") AS SourceTable " +
                                "PIVOT " +
                        "( Max(Value) " +
                           "FOR Field IN ([FileName],[FileLocation]) " +
                        " ) AS PivotTable";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                        {
                            var reader  = cmd.ExecuteReader();
                            while(reader.Read())
                            {
                                filePath = reader.GetString(0);
                                fileName = reader.GetString(1);
                            }

                        }
                            

                    }

                    Log4NetLogger.Info("fetched processing file : " + filePath + fileName);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        fileName = string.IsNullOrEmpty(fileName) ? task.clipGuid + ".mp4" : fileName;
                        string inputfile = filePath + fileName;
                        var isSuccess = true;

                        if (Path.GetExtension(fileName).ToLower() == ".mp3")
                        {
                            fileName = Path.GetFileNameWithoutExtension(fileName) + ".m4a";
                            convfile = ConfigSettings.Settings.WorkingDirectory + fileName;
                            Log4NetLogger.Info("convert mp3 file to mp4a at path :" + convfile + "");
                            string ffmpegParams = String.Format("-i {0} -bit_rate copy {1}", "\"" + inputfile + "\"", "\"" + convfile + "\"");
                            if (!RunConvertProcess(convfile, ffmpegParams))
                            {
                                isSuccess = false;
                            }
                            inputfile = convfile;
                        }
                        if (isSuccess)
                        {
                            destinationPath = "\\" + task.startDate.Year + "\\" + task.startDate.Month + "\\" + task.startDate.Day + "\\" + fileName;
                            if (clipDirection.ToLower().Equals("copy"))
                            {

                                if (FileHelper.CopyFile(inputfile, outputFile + destinationPath))
                                {
                                    status = "EXPORTED";
                                    Log4NetLogger.Debug("File copied successfully for IOS export on Path : " + outputFile + destinationPath);
                                }
                                else
                                {
                                    status = "FAILED_COPY_TO_OUTPUT";
                                    Log4NetLogger.Debug("Failed to copy file on Path : " + outputFile + destinationPath);
                                }

                            }
                            else if (clipDirection.ToLower().Equals("ftp"))
                            {
                                if (UploadToFTP(filePath + fileName, destinationPath, task.startDate))
                                {
                                    status = "EXPORTED";
                                    Log4NetLogger.Debug("File uploaded to FTP successfully for IOS export on Path : " + String.Format("ftp://{0}/{1}{2}/", ConfigSettings.Settings.FtpHost,
                                      ConfigSettings.Settings.FtpRootPath, destinationPath));
                                }
                                else
                                {
                                    status = "FAILED_UPLOAD_TO_FTP";
                                    Log4NetLogger.Debug("Failed to upload to FTP for IOS export on Path : " + String.Format("ftp://{0}/{1}{2}/", ConfigSettings.Settings.FtpHost,
                                      ConfigSettings.Settings.FtpRootPath, destinationPath));
                                }
                            }

                            try
                            {
                                if (!string.IsNullOrWhiteSpace(status) && status.Equals("EXPORTED"))
                                {
                                    using (var conn = new SqlConnection(connStr))
                                    {
                                        conn.Open();
                                        var cmdStr = "SELECT Value from IQCore_ClipMeta with(nolock) WHERE _ClipGuid = '" + task.clipGuid + "' AND Field= 'IOSLocation'";
                                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                        {
                                            var reader = cmd.ExecuteReader();
                                            if (reader.HasRows)
                                            {
                                                cmdStr = "UPDATE IQCore_ClipMeta SET Value = '"+ destinationPath +"' "
                                                     + " WHERE _ClipGuid = '" + task.clipGuid + "' AND Field = 'IOSLocation'";
                                                using (var cmd1 = conn.GetCommand(cmdStr, CommandType.Text))
                                                    cmd1.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                cmdStr = "INSERT INTO IQCore_ClipMeta ( _ClipGuid,Field,Value ) Values "
                                                     + "('" + task.clipGuid + "','IOSLocation','" + destinationPath + "') ";
                                                using (var cmd1 = conn.GetCommand(cmdStr, CommandType.Text))
                                                    cmd1.ExecuteNonQuery();
                                            }
                                            
                                        }


                                        cmdStr = "SELECT Value from IQCore_ClipMeta with(nolock) WHERE _ClipGuid = '" + task.clipGuid + "' AND Field= 'IOSRootPathID'";
                                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                        {
                                            var reader = cmd.ExecuteReader();
                                            if (reader.HasRows)
                                            {
                                                cmdStr = "UPDATE IQCore_ClipMeta SET Value = '" + ConfigurationManager.AppSettings["IOSRootPathID"] + "' "
                                                     + " WHERE _ClipGuid = '" + task.clipGuid + "' AND Field = 'IOSRootPathID'";
                                                using (var cmd1 = conn.GetCommand(cmdStr, CommandType.Text))
                                                    cmd1.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                cmdStr = "INSERT INTO IQCore_ClipMeta ( _ClipGuid,Field,Value ) Values "
                                                     + "('" + task.clipGuid + "','IOSRootPathID','" + ConfigurationManager.AppSettings["IOSRootPathID"] + "') ";
                                                using (var cmd1 = conn.GetCommand(cmdStr, CommandType.Text))
                                                    cmd1.ExecuteNonQuery();
                                            }

                                        }

                                    }
                                    Log4NetLogger.Debug("Inserted record into IQCore_ClipMeta with IOSFileLocation : " + destinationPath);
                                }
                            }
                            catch (Exception ex)
                            {
                                status = "FAILED_TO_INSERT_IN_IQCORE_CLIPMETA";
                                Log4NetLogger.Error("An error occurred while inserting record into IQCore_ClipMeta.", ex);
                            }
                        }
                        else
                        {
                            status = "FAILED_TO_CONVERT_MP3";
                        }
                    }
                    else
                    {
                        status = "FILE_LOCATION_NOT_FOUND";
                        Log4NetLogger.Debug("File not found for IOS export on Path :" + filePath);
                    }

                    //TODO: Notify whatever service that the clip has been exported successfully
                }
                catch (Exception ex)
                {
                    status = "FAILED";
                    var dir = (task != null) ? " (" + task.id + ")" : "";
                    Log4NetLogger.Error("An error occurred while processing export task." + dir, ex);
                }
                finally
                {
                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var cmdStr = "UPDATE IQIOSService_Export "
                                     + "SET Status='" + status + "', LastModified='" + DateTime.Now + "' "
                                     + "WHERE ID='" + task.id + "'";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                            cmd.ExecuteNonQuery();
                    }

                    //We put this in the finally block because, no matter what happens, we want the active task removed
                    if (task != null) _activeTasks.Remove(task);

                    if (!string.IsNullOrWhiteSpace(convfile))
                    {
                        FileHelper.DeleteFile(convfile);
                    }

                    ////We delete the local file no matter what...
                    //if (inputFile != null)
                    //    FileHelper.DeleteFile(inputFile);
                    //if (outputFile != null)
                    //    FileHelper.DeleteFile(outputFile);
                }
            }
        }

        private bool UploadToFTP(string localFilename, string remoteFilename, DateTime startDate, int bufferSize = (1024 * 128))
        {
            BinaryReader reader = null;
            Stream writer = null;
            var buffer = new byte[bufferSize];

            try
            {
                //Validate directory structure...
                var remotePath = remoteFilename.Substring(0, remoteFilename.LastIndexOf("\\"));
                var dirs = remotePath.Split('\\');
                var path = "";
                foreach (var dir in dirs)
                {
                    //Skip blanks
                    if (String.IsNullOrWhiteSpace(dir)) continue;

                    path += "\\" + dir;
                    if (!CreateDirectoryIfNotExists(path))
                    {
                        Log4NetLogger.Error("An error occurred while creating the directory structure.");
                        return false;
                    }
                }

                //Time to upload the file
                Log4NetLogger.Debug("Uploading file (" + localFilename + ") to (" + ConfigSettings.Settings.FtpRootPath + remoteFilename + ")");
                var request = (FtpWebRequest)WebRequest.Create(
                    String.Format("ftp://{0}/{1}{2}", ConfigSettings.Settings.FtpHost,
                                  ConfigSettings.Settings.FtpRootPath, remoteFilename));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ConfigSettings.Settings.FtpUsername,
                                                            ConfigSettings.Settings.FtpPassword);
                request.UsePassive = ConfigSettings.Settings.FtpUsePassive;

                reader =
                    new BinaryReader(new FileStream(localFilename, FileMode.Open, FileAccess.Read,
                                                    FileShare.Read, bufferSize, FileOptions.SequentialScan));

                request.ContentLength = reader.BaseStream.Length;
                writer = request.GetRequestStream();

                int bytesRead;
                while ((bytesRead = reader.Read(buffer, 0, bufferSize)) > 0)
                    writer.Write(buffer, 0, bytesRead);
                writer.Close();

                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    Log4NetLogger.Debug(response.StatusCode + " " + response.StatusDescription);
                    //ClosingData means it worked! Weird name...
                    return response.StatusCode.Equals(FtpStatusCode.ClosingData);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                    Log4NetLogger.Error("An FTP error occurred: " + ((FtpWebResponse)ex.Response).StatusDescription, ex);
                return false;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("An error occurred while uploading a file to the CDN: " + localFilename, ex);
                return false;
            }
            finally
            {
                if (null != reader) reader.Dispose();
                if (null != writer) writer.Dispose();
                buffer = null;
            }
        }

        private static bool CreateDirectoryIfNotExists(string path)
        {
            //Validate that the directory exists.
            try
            {
                Log4NetLogger.Debug("Validating that the remote directory exists: \\" + ConfigSettings.Settings.FtpRootPath + path);
                var chkReq = (FtpWebRequest)WebRequest.Create(
                    String.Format("ftp://{0}/{1}{2}/", ConfigSettings.Settings.FtpHost,
                                  ConfigSettings.Settings.FtpRootPath, path));
                chkReq.Method = WebRequestMethods.Ftp.ListDirectory;
                chkReq.Credentials = new NetworkCredential(ConfigSettings.Settings.FtpUsername,
                                                            ConfigSettings.Settings.FtpPassword);
                chkReq.UsePassive = ConfigSettings.Settings.FtpUsePassive;
                using (var response = (FtpWebResponse)chkReq.GetResponse())
                {
                    //The directory exists... we're good...
                    return true;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null && ((FtpWebResponse)ex.Response).StatusCode.Equals(FtpStatusCode.ActionNotTakenFileUnavailable))
                {
                    //Create the nessecary directory structure...
                    Log4NetLogger.Debug("Creating directory: \\" + ConfigSettings.Settings.FtpRootPath + path);
                    var createReq = (FtpWebRequest)WebRequest.Create(
                        String.Format("ftp://{0}/{1}{2}", ConfigSettings.Settings.FtpHost,
                                      ConfigSettings.Settings.FtpRootPath, path));
                    createReq.Method = WebRequestMethods.Ftp.MakeDirectory;
                    createReq.Credentials = new NetworkCredential(ConfigSettings.Settings.FtpUsername,
                                                                ConfigSettings.Settings.FtpPassword);
                    createReq.UsePassive = ConfigSettings.Settings.FtpUsePassive;
                    using (var response = (FtpWebResponse)createReq.GetResponse())
                    {
                        Log4NetLogger.Debug(response.StatusCode + " " + response.StatusDescription);
                        response.Close();
                        if (!response.StatusCode.Equals(FtpStatusCode.PathnameCreated))
                        {
                            Log4NetLogger.Error("An error occurred while creating the directory structure on the CDN.");
                            return false;
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        private static bool RunConvertProcess(string outputFile, string ffmpegParams)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            //NOTE: there seems to be an issue with the redirect
            startInfo.RedirectStandardError = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = ConfigSettings.Settings.FFMpegPath;
            startInfo.Arguments = ffmpegParams;

            try
            {
                Log4NetLogger.Debug("\"" + startInfo.FileName + "\" " + startInfo.Arguments);
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
                Log4NetLogger.Error(ex);
                return false;
            }
        }

        #endregion
    }
}
