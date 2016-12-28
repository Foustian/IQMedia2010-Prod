using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.TVEyes.Domain;
using IQMedia.TVEyes.Common.Util;


namespace IQMedia.TVEyes.Logic
{
    public class MediaReadyForDownloadLogic : BaseLogic, ILogic
    {
        public string ArchiveTVEyesUpdateDownloadStatus(Int64 p_ID, string p_Result, string p_Media, string p_Package)
        {
            try
            {
                int? recordsupdated;
                if (string.Compare(p_Result, "Success",true) == 0)
                {
                    recordsupdated = Context.ArchiveTVEyesUpdateDownloadStatus(p_ID, DownloadStatus.Ready_For_Download.ToString(), p_Media, p_Package).FirstOrDefault();
                    if (recordsupdated != null && recordsupdated > 0)
                    {
                        DownloadClient downloadClient = new DownloadClient();
                        downloadClient.WakeupService();
                        return "Archive media is ready for download";
                    }
                    else
                    {
                        Logger.Warning("no records updated for id :" + p_ID);
                        return "Archive download status not updated";
                    }
                }
                else
                {
                    recordsupdated = Context.ArchiveTVEyesUpdateDownloadStatus(p_ID, DownloadStatus.Fail_Ready_For_Download.ToString(), p_Media, p_Package).FirstOrDefault();
                    if (recordsupdated != null && recordsupdated > 0)
                    {
                        return "Archive media status updated to failed for download";
                    }
                    else
                    {
                        Logger.Warning("no records updated for id :" + p_ID);
                        return "Archive download status not updated";
                    }
                }

                
            }
            catch (Exception _Exception)
            {
                Logger.Error("error occured while updating archive status", _Exception);
                throw _Exception;
            }


        }
    }

    public enum DownloadStatus
    {
        Ready_For_Download,
        Fail_Ready_For_Download
    }
}
