using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IQMedia.Common.Util;
using IQMedia.Domain;
using IQMedia.Service.Ingestion.Config;

namespace IQMedia.Service.Ingestion.Strategy
{
    public class AudioFileStrategy : StrategyBase
    {
        public AudioFileStrategy(string fileExt) : base(fileExt)
        {
        }

        public override void Process(string sourceFile)
        {
            Source source;
            DateTime dateRecorded;

            //TryParse returns to us the file's source and dateRecorded if successful.
            //If it fails, skip this file and return...
            if (!TryParseFile(sourceFile, out source, out dateRecorded))
            {
                Logger.Warning("The file is invalid and will be ignored: " + sourceFile);
                return;
            }
            //If we're ingesting files for an inactive source, throw a warning in the logs
            if (!source.IsActive)
                Logger.Warning(String.Format("Ingesting file '{0}' for inactive source '{1}'.", sourceFile, source.SourceID));

            //TODO: Use MediaInfo to validate file duration

            //Assign the destination location
            var destFile = Regex.Replace(sourceFile, ConfigSettings.Settings.SourcePattern,
                                         ConfigSettings.Settings.DestinationPattern);

            var recording = GetOrCreateRecording(source, dateRecorded);
            var rfList = new List<Recordfile>(recording.Recordfiles.Where(rf => rf.Location.Contains(_fileExt)));

            //Here is where we handle duplicate files. If an original exists, we mark it with status "DUPLICATE"
            //and we add a numerical suffix to the end of the new file.
            if (rfList.Count > 0)
            {
                Logger.Warning("Duplicate audio record(s) exist in database. Old recordfile(s) will be set to status 'DUPLICATE'.");
                foreach (var rf in rfList)
                    rf.Status = RecordfileStatus.DUPLICATE.ToString();

                //Add a numerical suffix to the current file incase it ends up on the same rootpath as the original
                //ex. If this is the 3rd FILE_2010_03_27.txt then it will be named FILE_2010_03_27-3.txt
                var idx = destFile.LastIndexOf('.');
                destFile = destFile.Substring(0, idx) + "-" + rfList.Count + destFile.Substring(idx);
            }

            //We have to declare type and path out here since we need to use them multiple times in
            //the object initializer and it's potentially expensive to call them twice...
            var rfType = RecordfileType.GetByExtension(_fileExt);
            var rPath = GetNextRootPath(PathType.Media);
            var newRecord = new Recordfile
            {
                StartOffset = 0,
                EndOffset = 3600,
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now,
                Location = destFile,
                RecordfileType = rfType,
                RecordfileTypeID = rfType.ID,
                RecordingID = recording.ID,
                RootPath = rPath,
                RootPathID = rPath.ID,
                Status = ConfigSettings.Settings.RecordingStatus_Audio
            };
            rfList.Add(newRecord);

            //Now move the files and update the DB
            if (CopyFile(sourceFile, newRecord.PhysicalPath.LocalPath))
            {
                //Attempt to store the recordfiles to the db...
                if (SaveRecordfiles(rfList))
                {
                    //Delete the local copy since we know it copied successfully
                    DeleteFile(sourceFile);
                    //We can leave the function now as we're done...
                    return;
                }
                //If we failed, delete the new file we just copied to the rootpath
                DeleteFile(newRecord.PhysicalPath.AbsolutePath);
            }
            //If we got here, there must have been a problem...
            Logger.Warning(String.Format("Could not save recordfile(s) for {0}.  The system has not committed the transaction.",
                sourceFile));
        }
    }
}
