using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.IO;
using IQMediaGroup.ExportClip.Serializers;
using IQMediaGroup.ExportClip.Domain;
using System.Xml.Linq;
using IQMediaGroup.ExportClip.Logic;
using System.Configuration;


namespace IQMediaGroup.ExportClip.Commands
{
    public class RemoteExportClip : ICommand
    {
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string StrRemoteExportClipInput = string.Empty;
            string StrRemoteExportClipOutput = string.Empty;
            RemoteExportClipOutput _RemoteExportClipOutput = new RemoteExportClipOutput();

            Log4NetLogger.Info("Remote Export Clip Request Started.");

            try
            {
                StreamReader _StreamReader = new StreamReader(HttpRequest.InputStream);
                RemoteExportClipInput _RemoteExportClipInput = new RemoteExportClipInput();

                try
                {
                    StrRemoteExportClipInput = _StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {

                        Log4NetLogger.Info("Remote ExportClip Input : " + StrRemoteExportClipInput);
                    }

                    _RemoteExportClipInput = (RemoteExportClipInput)Serializer.DeserialiazeXml(StrRemoteExportClipInput, _RemoteExportClipInput);
                }
                catch (Exception)
                {
                    throw new CustomException("An error occurred in parsing input.");
                }


                var _ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);
                bool isValidate = _ValidationLogic.ValidateInput(_RemoteExportClipInput);

                if (!isValidate)
                {
                    _RemoteExportClipOutput.Message = "Invalid Input";
                    _RemoteExportClipOutput.Status = 1;
                    StrRemoteExportClipOutput = Serializer.SerializeToXml(_RemoteExportClipOutput);
                }
                else
                {
                    RemoteExportClipLogic _RemoteExportClipLogic = (RemoteExportClipLogic)LogicFactory.GetLogic(LogicType.RemoteExportClip);
                    string ClipInfo = _RemoteExportClipInput.ClipInfo == null ? string.Empty : Serializer.SerializeToXml(_RemoteExportClipInput.ClipInfo);
                    _RemoteExportClipOutput = _RemoteExportClipLogic.InsertRemoteExportClip(_RemoteExportClipInput.ClipGUID,_RemoteExportClipInput.ClipFTPLocation, ClipInfo);
                    StrRemoteExportClipOutput = Serializer.SerializeToXml(_RemoteExportClipOutput);
                }
            }
            catch (CustomException _CustomException)
            {
                _RemoteExportClipOutput.Message = _CustomException.Message;
                _RemoteExportClipOutput.Status = 1;
                StrRemoteExportClipOutput = Serializer.SerializeToXml(_RemoteExportClipOutput);
            }
            catch (Exception ex)
            {

                _RemoteExportClipOutput.Message = "An error occurred, please try again.";
                _RemoteExportClipOutput.Status = 1;
                StrRemoteExportClipOutput = Serializer.SerializeToXml(_RemoteExportClipOutput);

                Log4NetLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("Remote ExportClip Output : " + StrRemoteExportClipOutput);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";

            Log4NetLogger.Info("Remote ExportClip Request Completed.");

            HttpResponse.Output.Write(StrRemoteExportClipOutput);
        }
    }
}