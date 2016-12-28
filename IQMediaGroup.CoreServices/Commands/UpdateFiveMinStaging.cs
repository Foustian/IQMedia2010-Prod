using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Serializers;


namespace IQMediaGroup.CoreServices.Commands
{
    public class UpdateFiveMinStaging :ICommand
    {
        public string _iqcckey { get; private set; }
        public Guid? _recordFileGUID { get; private set; }
        public int? _lastMediaSeg { get; private set; }
        public string _mediaFilename { get; private set; }
        public string _mediaStatus { get; private set; }

        public UpdateFiveMinStaging(object iqcckey, object recordFileGUID, object lastMediaSeg, object mediaFilename, object mediaStatus)
        {
            _iqcckey = (iqcckey is NullParameter) ? null : (string)iqcckey;
            _recordFileGUID = (recordFileGUID is NullParameter) ? null : (Guid?)recordFileGUID;
            _lastMediaSeg = (lastMediaSeg is NullParameter) ? null : (int?)lastMediaSeg;
            _mediaFilename = (mediaFilename is NullParameter) ? null : (string)mediaFilename;
            _mediaStatus = (mediaStatus is NullParameter) ? null : (string)mediaStatus;

        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            var updateFiveMinStagingOutput = new UpdateFiveMinStagingOutput();
            Logger.LogInfo("Update FiveMinStaging Request Started");
            try
            {
                if (_iqcckey == null)
                    throw new ArgumentException("invalid or missing iqcckey");

                if (_recordFileGUID == null)
                    throw new ArgumentException("invalid or missing recordFileGUID");

                if (_lastMediaSeg == null)
                    throw new ArgumentException("invalid or missing lastMediaSeg");

                if (_mediaFilename == null)
                    throw new ArgumentException("invalid or missing mediaFilename");

                if (_mediaStatus == null)
                    throw new ArgumentException("invalid or missing mediaStatus");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("Update FiveMinStaging Input iqcckey:" + _iqcckey + ", recordFileGUID:" + _recordFileGUID + ", lastMediaSeg:" + _lastMediaSeg + ", mediaFilename:" + _mediaStatus + ", mediaStatus:"+_mediaStatus);
                }

                var fiveMinStagingLogic = (FiveMinStagingLogic)LogicFactory.GetLogic(LogicType.FiveMinStaging);
                updateFiveMinStagingOutput = fiveMinStagingLogic.UpdateFiveMinStaging(_recordFileGUID.Value,_lastMediaSeg.Value,_mediaFilename,_mediaStatus,_iqcckey);

            }
            catch (ArgumentException ex)
            {
                updateFiveMinStagingOutput.status = 1;
                updateFiveMinStagingOutput.message = ex.Message;

            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                updateFiveMinStagingOutput.status = 1;
                updateFiveMinStagingOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(updateFiveMinStagingOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Update FiveMinStaging Output " + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Logger.LogInfo("Update FiveMinStaging Request Ended");
            HttpResponse.Output.Write(xmlResult);
        }
    }
}