using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using IQMedia.Common.Util;
using IQMedia.Domain;
using IQMedia.Logic;
using IQMedia.Service.Ingestion.Config;

namespace IQMedia.Service.Ingestion.Strategy
{
    public abstract class StrategyBase
    {
        #region Attributes
        private static readonly object _lock = new object();
        protected string _fileExt;
        #endregion

        #region Constructors
        protected StrategyBase(string fileExt)
        {
            _fileExt = fileExt;
        }
        #endregion

        #region Operations

        /// <summary>
        /// Static factory method for creating a strategy based on a file's extension
        /// </summary>
        /// <param name="fileExt">The file extension.</param>
        /// <returns>The relevant strategy.</returns>
        public static StrategyBase Create(string fileExt)
        {
		    const string CC_FILE_REGEX = @"\.?txt$";
		    const string AUDIO_FILE_REGEX = @"\.?(wma$|asf$|mp3$)";
		    const string VIDEO_FILE_REGEX = @"\.?(wmv$|flv$|mp4$)";

            if(String.IsNullOrWhiteSpace(fileExt))
                throw new ArgumentNullException("fileExt");

            if (Regex.IsMatch(fileExt, CC_FILE_REGEX))
                return new CCFileStrategy(fileExt);

            if (Regex.IsMatch(fileExt, AUDIO_FILE_REGEX))
                return new AudioFileStrategy(fileExt);

            if (Regex.IsMatch(fileExt, VIDEO_FILE_REGEX))
                return new VideoFileStrategy(fileExt);

            //If we got here, then there is no strategy for this file type
            throw new ArgumentException("Cannot create strategy for file type: '" + fileExt + "'");
        }

        /// <summary>
        /// Determines whether the specified source file matches the pattern set in the App.Config.
        /// If the file matches, it then validates that the specified source exists and returns via
        /// the 'source' parameter.
        /// </summary>
        /// <param name="sourceFile">The source file path.</param>
        /// <param name="source">The source to be returned.</param>
        /// <param name="dateRecorded">The datetime to be returned.</param>
        /// <returns>
        /// 	<c>true</c> if the specified source file is valid; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool TryParseFile(string sourceFile, out Source source, out DateTime dateRecorded)
        {
            var result = false;
            source = null;
            dateRecorded = DateTime.MinValue;

            try
            {
                var fileMatch = Regex.Match(sourceFile, ConfigSettings.Settings.SourcePattern);
                if(fileMatch.Success)
                {
                    var srcLgc = (SourceLogic)LogicFactory.GetLogic(LogicType.Source);
                    source = srcLgc.GetSourceBySourceID(fileMatch.Groups[1].Value);
                    dateRecorded = new DateTime(Convert.ToInt32(fileMatch.Groups[2].Value), Convert.ToInt32(fileMatch.Groups[3].Value), Convert.ToInt32(fileMatch.Groups[4].Value), Convert.ToInt32(fileMatch.Groups[5].Value), 0, 0);
                    //Parse was successful if we got a valid source and datetime out of the filename.
                    result = (source != null && !dateRecorded.Equals(DateTime.MinValue));
                }
            }
            catch(Exception ex)
            {
                Logger.Warning("Failed to validate file: " + sourceFile, ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the relevant recording or creates it if nonexistent.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="startDate">The start date.</param>
        /// <returns></returns>
        protected Recording GetOrCreateRecording(Source source, DateTime startDate)
        {
            Recording rec;
            lock (_lock)
            {
                var recLgc = (RecordingLogic) LogicFactory.GetLogic(LogicType.Recording);
                rec = recLgc.GetRecordingBySourceAndDate(source.Guid, startDate);

                if (rec == null)
                {
                    Logger.Debug(String.Format("No recording found. Creating Recording for {0} at {1}", source.SourceID, startDate));
                    rec = new Recording
                              {
                                  SourceGuid = source.Guid,
                                  StartDate = startDate,
                                  //TODO: Handle EndDate without hard-code 1-hour
                                  EndDate = startDate.AddHours(1)
                              };

                    try { recLgc.Insert(rec); }
                    catch(Exception ex) { Logger.Error("An error occurred while creating a new recording.", ex); }
                }
            }
            return rec;
        }

        protected bool SaveRecordfiles(IEnumerable<Recordfile> recordfiles)
        {
            lock(_lock)
            {
                var rfLgc = (RecordfileLogic)LogicFactory.GetLogic(LogicType.Recordfile);
                if (rfLgc.InsertAllOrRollback(recordfiles))
                {
                    Logger.Debug("All recordfiles successfully committed.");
                    return true;
                }
                Logger.Warning("An errror occurred while committing the recordfiles to the database.");
                return false;
            }
        }

        /// <summary>
        /// Copies the source file to the specified destination.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="destinationFile">The destination.</param>
        /// <returns><c>true</c> if successful, <c>false</c> if not.</returns>
        protected static bool CopyFile(string sourceFile, string destinationFile)
        {
            if(String.IsNullOrWhiteSpace(sourceFile) || String.IsNullOrWhiteSpace(destinationFile))
            {
                Logger.Warning(String.Format("Source file '{0}' or Destination file '{1}' was not valid.", sourceFile, destinationFile));
                return false;
            }

            var success = false;
            const int bufferSize = (1024 * 128); //128Kb buffer
            var buffer = new byte[bufferSize];
            BinaryReader reader = null;
            BinaryWriter writer = null;

            try
            {
                var destinationPath = Path.GetDirectoryName(destinationFile);
                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);

                //Set fileshare to NONE to attempt to minimize duplicates...
                reader = new BinaryReader(new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize, FileOptions.SequentialScan));
                writer = new BinaryWriter(new FileStream(destinationFile, FileMode.Create, FileAccess.Write));

                int bytesRead;
                while ((bytesRead = reader.Read(buffer, 0, bufferSize)) > 0)
                    writer.Write(buffer, 0, bytesRead);

                //File copied successfully...Log Success
                Logger.Info(String.Format("File '{0}' successfully copied to destination '{1}'", sourceFile, destinationFile));
                success = true;
            }
            catch (FileNotFoundException ex) {
                Logger.Warning(String.Format("The source file '{0}' no longer exists on the file system. (Another thread may have processed it already.)", sourceFile), ex);
            }
            catch(IOException ex) {
                Logger.Warning(String.Format("The source file '{0}' is in use by another thread and will be skipped.", sourceFile), ex);
            }
            catch(Exception ex) {
                Logger.Error(String.Format("Error copying '{0}' to '{1}'.", sourceFile, destinationFile), ex);
            }
            finally
            {
                if (null != reader) reader.Close();
                if (null != writer) writer.Close();
            }
            return success;
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filePath">The file path to be deleted.</param>
        /// <returns><c>true</c> if successful, <c>false</c> if not.</returns>
        protected static bool DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                Logger.Info(String.Format("File '{0}' successfully deleted.", filePath));
                return true;
            }
            catch(Exception ex)
            {
                Logger.Error("Error deleting file: " + filePath, ex);
                return false;
            }
        }

        /// <summary>
        /// This is just a wrapper for CopyFile and DeleteFile. If CopyFile is successful,
        /// it deletes the soureFile.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="destinationFile">The destination file.</param>
        /// <returns><c>true</c> if both operations are successful, <c>false</c> if either fails.</returns>
        protected static bool MoveFile(string sourceFile, string destinationFile)
        {
            if (CopyFile(sourceFile, destinationFile))
                if (DeleteFile(sourceFile))
                    return true;
            return false;
        }

        /// <summary>
        /// Processes the specified source file. This method is to be implemented
        /// by each of the media strategies.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        public abstract void Process(string sourceFile);

        /// <summary>
        /// Gets the next root path in a random pattern.
        /// </summary>
        /// <param name="pathType">Name of the RootPathType.</param>
        /// <returns>A random RootPath.</returns>
        protected RootPath GetNextRootPath(PathType pathType)
        {
            lock(_lock)
            {
                var rptLgc = (RootPathTypeLogic)LogicFactory.GetLogic(LogicType.RootPathType);
                var rpt = rptLgc.GetRootPathTypeByName(pathType.ToString());

                if (rpt == null)
                    throw new Exception("No valid rootpathtype found for specified pathType: " + pathType);
                //Cast to a list
                var paths = new List<RootPath>(rpt.RootPaths.Where(rp => rp.IsActive));
                if(paths.Count <= 0)
                    throw new Exception("No active rootpaths found for specified pathType: " + pathType);

                //Grab a random RootPath
                var idx = new Random().Next(paths.Count);
                return paths[idx];
            }
        }

        #endregion
    }

    /// <summary>
    /// Helper Enumerator to strongly-type the RootPathTypes
    /// </summary>
    public enum PathType
    {
        Index,
        Media,
        DEV
    }
}
