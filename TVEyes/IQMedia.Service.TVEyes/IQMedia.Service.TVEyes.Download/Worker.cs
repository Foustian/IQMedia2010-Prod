using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.TVEyes.Common.Threading;
using System.Threading;
using IQMedia.TVEyes.Common.Util;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using IQMedia.Service.TVEyes.Download.Config;
using IQMedia.TVEyes.Logic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System.Diagnostics;
using System.Net;

namespace IQMedia.Service.TVEyes.Download
{
    public class Worker
    {
        #region Attributes
        private static readonly object _lock = new object();
        private static Worker _worker;
        private static ThreadSafeQueue<DownloadTask> _queue = new ThreadSafeQueue<DownloadTask>();
        private readonly Thread[] _threadPool;
        private List<DownloadTask> _activeTasks;
        private List<string> _decompressFiles = new List<string>();
        private const int BUFFER_SIZE = 4096;
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
            _activeTasks = new List<DownloadTask>(threadPoolSize);
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

        public void Enqueue(DownloadTask task)
        {
            if (!_queue.Contains(task))
            {
                Logger.Debug("Download Service is enqueing task: " + task.ID);
                _queue.Enqueue(task);
            }
            else Logger.Debug(String.Format("Download Service is skipping task {0} because it is already enqueued.", task.ID));
        }

        protected void Execute()
        {
            while (true)
            {
                DownloadTask task = null;

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

                    _decompressFiles = new List<string>();

                    var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var cmdStr = "UPDATE ArchiveTVEyes "
                                     + "SET Status='DOWNLOAD_IN_PROCESS' "
                                     + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                            cmd.ExecuteNonQuery();
                    }

                    if (!string.IsNullOrEmpty(task.Package))
                    {
                        var packageFile = ConfigSettings.Settings.WorkingDirectory + task.TMGuid + Path.GetExtension(task.Package);
                        Logger.Info("downloading file : " + task.Package + " at :" + packageFile);
                        if(DownloadFile(task.Package,packageFile))
                        {
                            Logger.Info("fetching root path location from rootpath id :" + task._RootPathID);

                            TVEyesLogic tvEyesLogic = (TVEyesLogic)LogicFactory.GetLogic(LogicType.TVEyes);
                            string outputPath = tvEyesLogic.GetRootPathByRootPathID(task._RootPathID);
                            if (!string.IsNullOrEmpty(outputPath))
                            {
                                Logger.Info("going to decompress file :" + packageFile + " at location :" + ConfigSettings.Settings.WorkingDirectory);
                                if (Decompress(packageFile, task.TMGuid, ConfigSettings.Settings.WorkingDirectory))
                                {
                                    var date = task.UTCDateTime;
                                    var location = "\\" + date.Year + "\\" + date.Month + "\\" + date.Day + "\\";
                                    var audiofile = string.Empty;
                                    var transcriptfile = string.Empty;
                                    outputPath = outputPath + location;
                                    bool IsAllFileCopied = true;
                                    bool ISConverted = false;

                                    if (!FileHelper.CopyFile(packageFile, outputPath + Path.GetFileName(packageFile)))
                                    {
                                        Logger.Warning("can not copy zip file :" + packageFile + " to location :" + outputPath + Path.GetFileName(packageFile) + " for task :" + task.ID);
                                    }

                                    foreach (string filepath in _decompressFiles)
                                    {
                                        var copyfilepath = filepath;
                                        string mimetype = GetMimeType(Path.GetExtension(filepath).ToLower());
                                        Logger.Info("mime type for file :" + Path.GetFileName(filepath) + " is :" + mimetype);
                                        switch (mimetype)
                                        {
                                            case "text/xml":
                                            case "text/html":
                                            case "text/plain":
                                            case "application/xml":
                                                transcriptfile = Path.GetFileName(filepath);
                                                break;
                                            case "audio/mpeg":
                                            case "audio/mpeg2":
                                            case "audio/x-ms-wma":
                                                audiofile = Path.GetFileName(filepath);
                                                break;
                                            default:
                                                break;
                                        }

                                        if (audiofile == Path.GetFileName(filepath) && !ISConverted)
                                        {

                                            if (Path.GetExtension(audiofile).ToLower() != ".mp3")
                                            {
                                                var newFile = Path.GetFileNameWithoutExtension(audiofile) + ".mp3";

                                                Logger.Info("going to convert " + audiofile + " to " + newFile + "");

                                                var ffmpegParams = String.Format("-y -i {0} {2} {1}",
                                                        filepath,
                                                        ConfigSettings.Settings.ProcessDirectory + newFile,
                                                        ConfigSettings.Settings.FfmpegParametersMp3);

                                                Logger.Info("ffmpeg command :" + ffmpegParams);

                                                if (RunConvertProcess(ConfigSettings.Settings.ProcessDirectory + newFile, ffmpegParams))
                                                {
                                                    ISConverted = true;
                                                    copyfilepath = ConfigSettings.Settings.ProcessDirectory + newFile;
                                                    audiofile = Path.GetFileName(newFile);
                                                }
                                                else
                                                {
                                                    ISConverted = false;
                                                }
                                            }
                                        }

                                        if (!FileHelper.CopyFile(copyfilepath, outputPath + Path.GetFileName(copyfilepath)))
                                        {
                                            IsAllFileCopied = false;
                                            Logger.Error("error occured while copying file from location : " + copyfilepath + " to :" + outputPath + Path.GetFileName(filepath));
                                            break;
                                        }
                                    }

                                    if (IsAllFileCopied && !string.IsNullOrEmpty(audiofile) && !string.IsNullOrEmpty(transcriptfile) && ISConverted)
                                    {
                                        connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                                        using (var conn = new SqlConnection(connStr))
                                        {
                                            conn.Open();
                                            var cmdStr = "UPDATE ArchiveTVEyes "
                                                         + "SET Status='DOWNLOADED', "
                                                         + "IsDownLoaded=1, "
                                                         + "Location='" + location + "', "
                                                         + "AudioFile='" + audiofile + "', "
                                                         + "TranscriptFile='" + transcriptfile + "' "
                                                         + "WHERE ArchiveTVEyesKey='" + task.ID + "' "
                                                         
                                                         + " UPDATE IQArchive_Media Set IsActive = 1 WHERE MediaType ='TM' AND _ArchiveMediaID = '" + task.ID + "'";

                                            using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                                cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        var Status = string.Empty;
                                        if (!IsAllFileCopied)
                                        {
                                            Status = "FAILED_TO_COPY";
                                        }
                                        else if (!ISConverted)
                                        {
                                            Status = "FAILED_TO_CONVERT";
                                        }
                                        else
                                        {
                                            Status = "FAILED_TO_LOCATE_FILE";
                                            Logger.Error("can not locate transcript file or audio file for task :" + task.ID);
                                            Logger.Error("transcript file name : " + transcriptfile + " audi file name :" + audiofile);
                                        }
                                        connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                                        using (var conn = new SqlConnection(connStr))
                                        {
                                            conn.Open();
                                            var cmdStr = "UPDATE ArchiveTVEyes "
                                                         + "SET Status='" + Status + "' "
                                                         + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                                            using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                                cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                else
                                {
                                    connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                                    using (var conn = new SqlConnection(connStr))
                                    {
                                        conn.Open();
                                        var cmdStr = "UPDATE ArchiveTVEyes "
                                                     + "SET Status='FAILED_TO_DECOMPRESS' "
                                                     + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                            cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                                using (var conn = new SqlConnection(connStr))
                                {
                                    conn.Open();
                                    var cmdStr = "UPDATE ArchiveTVEyes "
                                                 + "SET Status='FAILED_ROOPATH_NOT_EXIST' "
                                                 + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                                    using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                        cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            Logger.Info("failed to download file for task :" + task.ID);
                            connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                            using (var conn = new SqlConnection(connStr))
                            {
                                conn.Open();
                                var cmdStr = "UPDATE ArchiveTVEyes "
                                             + "SET Status='FAILED_TO_DOWNLOAD' "
                                             + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                                using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                    cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                        using (var conn = new SqlConnection(connStr))
                        {
                            conn.Open();
                            var cmdStr = "UPDATE ArchiveTVEyes "
                                         + "SET Status='FAILED_PACKAGE_NOT_EXIST' "
                                         + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                            using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                                cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var connStr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        var cmdStr = "UPDATE ArchiveTVEyes "
                                     + "SET Status='Exception_In_Download' "
                                     + "WHERE ArchiveTVEyesKey='" + task.ID + "'";
                        using (var cmd = conn.GetCommand(cmdStr, CommandType.Text))
                            cmd.ExecuteNonQuery();
                    }
                    _activeTasks.Remove(task);
                    var dir = (task != null) ? " (" + task.ID + ")" : "";
                    Logger.Error("An error occurred while processing Download task." + dir, ex);
                }
                finally
                {
                    _activeTasks.Remove(task);
                    DirectoryInfo dir = new DirectoryInfo(ConfigSettings.Settings.WorkingDirectory);
                    foreach (FileInfo f in dir.GetFiles())
                    {
                        f.Delete();
                    }

                    DirectoryInfo dir2 = new DirectoryInfo(ConfigSettings.Settings.ProcessDirectory);
                    foreach (FileInfo f in dir2.GetFiles())
                    {
                        f.Delete();
                    }
                }
            }
        }

        #endregion

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
                Logger.Debug(startInfo.FileName + " " + startInfo.Arguments);
                using (var exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }

                return File.Exists(outputFile);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }

        private static bool DownloadFile(string address, string filePath)
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(address))
                    {
                        using (var file = File.Create(filePath))
                        {
                            var buffer = new byte[BUFFER_SIZE];
                            int bytesReceived;
                            while ((bytesReceived = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                file.Write(buffer, 0, bytesReceived);
                            }
                        }
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Logger.Error("error occured while downloading file :" + filePath, ex);
                return false;
            }
        }

        #region Utility Methods

        public bool Decompress(string filePathforDecompress,Guid TMGuid,string ExtractPath)
        {
            try
            {
                if(!Directory.Exists(ExtractPath))
                {
                    Directory.CreateDirectory(ExtractPath);
                }

                using (ZipInputStream s = new ZipInputStream(File.OpenRead(filePathforDecompress)))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        if (!theEntry.IsFile)
                            continue;



                        string outFileName = Path.Combine(ExtractPath, TMGuid + Path.GetExtension(theEntry.Name));
                        Logger.Info("copying file from : " + theEntry.Name + " to : " + outFileName);
                        using (FileStream streamWriter = File.Create(outFileName))
                        {

                            byte[] data = new byte[BUFFER_SIZE];
                            while (true)
                            {
                                int size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            _decompressFiles.Add(outFileName);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("error occured while decompressing file : " + filePathforDecompress, ex);
                return false;
            }
        }

        private string GetMimeType(FileInfo fileInfo)
        {
            string mimeType = "application/unknown";

            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(
            fileInfo.Extension.ToLower()
            );

            if (regKey != null)
            {
                object contentType = regKey.GetValue("Content Type");

                if (contentType != null)
                    mimeType = contentType.ToString();
            }

            return mimeType.ToLower();
        }


        private string GetMimeType(string extention)
        {
            string mimeType = string.Empty;

            switch (extention)
            {
                case ".wma" :
                    mimeType = "audio/x-ms-wma";
                    break;
                case ".mp3" :
                    mimeType = "audio/mpeg";
                    break;
                case ".txt" :
                    mimeType = "text/plain";
                    break;
                case ".htm":
                case ".html":
                    mimeType = "text/html";
                    break;
                case ".xml":
                    mimeType = "text/xml";
                    break;
                default :
                    mimeType = "application/unknown";
                    break;
            }

            return mimeType.ToLower();
        }

        #endregion
    }
}
