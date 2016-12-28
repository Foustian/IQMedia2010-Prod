using System;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Logic;
using System.Net;
using System.IO;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class ExportClipForMicrosite : ICommand
    {
        public Guid? _ClipGUID { get; private set; }
        public Guid? _ClientGUID { get; private set; }
        public bool? _IsUGC { get; private set; }

        public ExportClipForMicrosite(object ClipGUID, object ClientGUID, object IsUGC)
        {
            _ClipGUID = (ClipGUID is NullParameter) ? null : (Guid?)ClipGUID;
            _ClientGUID = (ClientGUID is NullParameter) ? null : (Guid?)ClientGUID;
            _IsUGC = (IsUGC is NullParameter) ? null : (bool?)IsUGC;
        }

        public void Execute(HttpRequest request, HttpResponse HttpResponse)
        {
            string _JSONResult = string.Empty;
            CLogger.Debug("ExportClipForMicrosite Request Started");

            try
            {
                if (!_ClipGUID.HasValue)
                {
                    throw new ArgumentException("Invalid or missing Clip Guid.");
                }

                if (!_ClientGUID.HasValue)
                {
                    throw new ArgumentException("Invalid or missing Client Guid.");
                }

                if (!_IsUGC.HasValue)
                {
                    throw new ArgumentException("Invalid or missing IsUGC.");
                }

                CLogger.Debug("{Request Parameters: ClipGUID : " + _ClipGUID + " ClientGUID : " + _ClientGUID + " IsUGC : " + _IsUGC);

                ClientLogic clientLogic = (ClientLogic)LogicFactory.GetLogic(LogicType.Client);
                var hasAccess = clientLogic.HasMicrositeAccess(_ClientGUID.Value);

                if (hasAccess)
                {
                    if (_IsUGC.Value)
                    {
                        CLogger.Debug("ExportUGCRawClip will be called");

                        ExportUGCRawClipLogic _UGCRawClipExportLogic = (ExportUGCRawClipLogic)LogicFactory.GetLogic(LogicType.UGCRawClipExport);
                        _JSONResult = _UGCRawClipExportLogic.InsertIQService_UGCRawClipExport(_ClipGUID);
                    }
                    else
                    {
                        CLogger.Debug("ExportClip will be called");

                        string _Uri = System.Configuration.ConfigurationManager.AppSettings["ExportClip"] + "?fid=" + _ClipGUID;

                        HttpWebRequest _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(_Uri);
                        _httpWebRequest.KeepAlive = false;

                        if ((_httpWebRequest.GetResponse().ContentLength > 0))
                        {
                            using (StreamReader _StreamReader = new StreamReader(_httpWebRequest.GetResponse().GetResponseStream()))
                            {
                                _JSONResult = Serializer.Searialize(_StreamReader.ReadToEnd());
                                _StreamReader.Dispose();
                            }
                        }
                        else
                        {
                            _JSONResult = Serializer.Searialize("No response from ExportClip");
                        }
                    }
                }
                else
                {
                    _JSONResult = Serializer.Searialize("{\"Status\":1,\"Message\":\"" + "Client is not authenticated" + "\"}");
                }
            }
            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace + "inner exception :" + ex.InnerException);
                _JSONResult = Serializer.Searialize("An error occurred, please try again");
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("ExportClipForMicrosite Request Ended");
            HttpResponse.Output.Write(_JSONResult);
        }
    }
}