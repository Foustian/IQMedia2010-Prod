using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Common.IOS.Util;

namespace IQMediaGroup.Logic.IOS
{
    public class ClipLogic : BaseLogic, ILogic
    {

        public string GetClipIOSLocation(Guid? clipGUID)
        {
            try
            {
                var iosFileLocation = Context.GetClipIOSLocation(clipGUID).FirstOrDefault();
                if (iosFileLocation != null && !string.IsNullOrWhiteSpace(iosFileLocation.IOSFileLocation))
                {
                    return (iosFileLocation.StreamSuffixPath.Substring(0,iosFileLocation.StreamSuffixPath.LastIndexOf("/")) + "/" + iosFileLocation.AppName + new Uri(iosFileLocation.StreamSuffixPath + iosFileLocation.IOSFileLocation).LocalPath);

                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetRecordFileLocationByGuid(Guid? RecordfileGuid)
        {
            var iosFileLocation = Context.GetRecordFileLocationByGuid(RecordfileGuid).FirstOrDefault();
            if (iosFileLocation != null && !string.IsNullOrWhiteSpace(iosFileLocation.IOSFileLocation))
            {
                return "http://" + new Uri(iosFileLocation.StreamSuffixPath).Host + "/" + iosFileLocation.AppName + new Uri(iosFileLocation.StreamSuffixPath + iosFileLocation.IOSFileLocation).LocalPath;

            }

            return string.Empty;
        }


        public bool QueueIOSExport(Guid p_clipGuid)
        {

            try
            {
                // check for if clip is already queued / in process / exported , and then not enqueue again.
                var clipexport = Context.IQIOSService_Export.Where(e => e.ClipGuid == p_clipGuid && (string.Compare(e.Status, "QUEUED", true) == 0 || string.Compare(e.Status, "IN_PROCESS", true) == 0 || string.Compare(e.Status, "EXPORTED", true) == 0 || string.Compare(e.Status,"FILE_LOCATION_NOT_FOUND") == 0)).FirstOrDefault();
                if (clipexport == null)
                {
                    IQIOSService_Export iQIOSService_Export = new IQIOSService_Export()
                    {
                        ID = Guid.NewGuid(),
                        ClipGuid = p_clipGuid,
                        Status = "QUEUED",
                        DateQueued = DateTime.Now,
                        LastModified = DateTime.Now

                    };
                    Context.IQIOSService_Export.AddObject(iQIOSService_Export);
                    Context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
