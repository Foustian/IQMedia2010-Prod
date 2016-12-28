using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Services.Commands;
using IQMediaGroup.Logic;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Serializers;
using System.IO;
using IQMediaGroup.Domain;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class ExportUGCRawClip : ICommand
    {
        public Guid? _ClipID { get; private set; }

        public ExportUGCRawClip(object ClipID)
        {
            _ClipID = (ClipID is NullParameter) ? null : (Guid?)ClipID;
        }

        public void Execute(HttpRequest request, HttpResponse HttpResponse)
        {
            string _JSONResult = string.Empty;
            CLogger.Debug("UGC ClipExport Request Started");
            try
            {
                if (_ClipID != null)
                {
                    CLogger.Debug("{\"ClipID\":\"" + _ClipID + "\"");
                    ExportUGCRawClipLogic _UGCRawClipExportLogic = (ExportUGCRawClipLogic)LogicFactory.GetLogic(LogicType.UGCRawClipExport);
                    _JSONResult = _UGCRawClipExportLogic.InsertIQService_UGCRawClipExport(_ClipID); 
                }
                else
                {
                    _JSONResult = Serializer.Searialize("ClipID is not valid");
                }
            }
            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace);
                _JSONResult = Serializer.Searialize("An error occurred, please try again"+ex.Message+ex.StackTrace+ex.InnerException.Message);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("UGC ClipExport Request Ended");
            HttpResponse.Output.Write(_JSONResult);

        }
    }
}