using System;
using IQMediaGroup.Common.IOS.Util;
using IQMediaGroup.Logic;
using IQMediaGroup.Logic.IOS;


namespace IQMediaGroup.Services.IOS.ExportClip.Service
{
    public class IOSExportWebService : IIOSExportWebService
    {
        //public bool EnqueueTask(Guid clipGuid, string outputExt, string outputPath = null, string outputDimensions = null)
        //{
        //    var clpLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Service);
        //    return clpLgc.EnqueueClipForExport(clipGuid, outputExt, outputPath, outputDimensions);
        //}

        public void WakeupService()
        {
            //Forcefully tell export to run.
            Log4NetLogger.Info("Export kicked off by WCF Service.");
            ExportService.Instance.EnqueueTasks();
        }
    }
}
