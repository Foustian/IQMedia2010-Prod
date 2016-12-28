using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Logic;
using System.Configuration;


namespace IQMediaGroup.CoreServices.Commands
{
    public class GetMediaLocation : ICommand
    {
        public Guid? _GUID { get; private set; }

        public GetMediaLocation(object guid)
        {
            _GUID = (guid is NullParameter) ? null : (Guid?)(guid);
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            var getMediaLocationOutput = new GetMediaLocationOutput();
            var listOfMediaLocation = new List<MediaLocation>();
            Logger.LogInfo("GetMediaLocation Request Started");

            try
            {
                if (_GUID == null || !_GUID.HasValue)
                {
                    throw new ArgumentException("Invalid or missing station ID");
                }

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("GetMediaLocation Input GUID: " + _GUID);
                }
                var recordFileLogic = (RecordFileLogic)LogicFactory.GetLogic(LogicType.RecordFile);
                Boolean isClipExits = recordFileLogic.CheckForClipByRecordFileGUID(_GUID);

                if (isClipExits)
                {
                    getMediaLocationOutput.status = 2;
                    getMediaLocationOutput.message = "Clip Exists";
                }
                else
                {
                    listOfMediaLocation = recordFileLogic.GetMediaLocationByRecordFileGUID(_GUID);
                    MediaLocations mediaLocations = new MediaLocations();
                    mediaLocations.listofMediaLocationOutput = new List<MediaLocation>();
                    mediaLocations.listofMediaLocationOutput = listOfMediaLocation;
                    getMediaLocationOutput.status = 0;
                    getMediaLocationOutput.mediaLocations = mediaLocations;
                    getMediaLocationOutput.message = "Success";

                }
            }
            catch (ArgumentException ex)
            {
                getMediaLocationOutput.status = 1;
                getMediaLocationOutput.message = ex.Message;

            }
            catch (Exception exception)
            {
                Logger.LogInfo("Error : " + exception.Message + " stack : " + exception.StackTrace);
                getMediaLocationOutput.status = 1;
                getMediaLocationOutput.message = "An error occurred, Please try again.";
            }

            xmlResult = Serializers.Serializer.SerializeToXml(getMediaLocationOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("GetMediaLocation Output :" + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("GetMediaLocation Request Ended.");
            HttpResponse.Output.Write(xmlResult);

        }

    }
}