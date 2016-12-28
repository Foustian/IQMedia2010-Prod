using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Domain;
using System.Configuration;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.CoreServices.Serializers;

namespace IQMediaGroup.CoreServices.Commands
{
    public class UpdateFiveMinStagingCC : ICommand
    {

        #region ICommand Member

        public string _iqcckey { get; private set; }
        public Guid? _recordFileGUID { get; private set; }
        public int? _lastCCTxtSeg { get; private set; }
        public string _CCTxtFilename { get; private set; }
        public string _CCTxtStatus { get; private set; }

        public UpdateFiveMinStagingCC(object iqcckey, object recordFileGUID, object lastCCTxtSeg, object CCTxtFilename, object CCTxtStatus)
        {
            _iqcckey = (iqcckey is NullParameter) ? null : (string)iqcckey;
            _recordFileGUID = (recordFileGUID is NullParameter) ? null : (Guid?)recordFileGUID;
            _lastCCTxtSeg = (lastCCTxtSeg is NullParameter) ? null : (int?)lastCCTxtSeg;
            _CCTxtFilename = (CCTxtFilename is NullParameter) ? null : (string)CCTxtFilename;
            _CCTxtStatus = (CCTxtStatus is NullParameter) ? null : (string)CCTxtStatus;

        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            var updateFiveMinStagingCCOutput = new UpdateFiveMinStagingCCOutput();
            Log4NetLogger.Info("Update FiveMinStaging Request Started");
            try
            {
                if (_iqcckey == null)
                    throw new ArgumentException("invalid or missing iqcckey");

                if (_recordFileGUID == null)
                    throw new ArgumentException("invalid or missing recordFileGUID");

                if (_lastCCTxtSeg == null)
                    throw new ArgumentException("invalid or missing lastCCTxtSeg");

                if (_CCTxtFilename == null)
                    throw new ArgumentException("invalid or missing CCTxtFilename");

                if (_CCTxtStatus == null)
                    throw new ArgumentException("invalid or missing CCTxtStatus");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Log4NetLogger.Info("Update FiveMinStagingCC Input iqcckey:" + _iqcckey + ", recordFileGUID:" + _recordFileGUID + ", lastCCTxtSeg:" + _lastCCTxtSeg + ", CCTxtFilename:" + _CCTxtFilename + ", CCTxtStatus:" + _CCTxtStatus);
                }

                var fiveMinStagingLogic = (FiveMinStagingLogic)LogicFactory.GetLogic(LogicType.FiveMinStaging);
                updateFiveMinStagingCCOutput = fiveMinStagingLogic.UpdateFiveMinStagingCC(_recordFileGUID.Value, _lastCCTxtSeg.Value, _CCTxtFilename, _CCTxtStatus, _iqcckey);

            }
            catch (ArgumentException ex)
            {
                updateFiveMinStagingCCOutput.status = 1;
                updateFiveMinStagingCCOutput.message = ex.Message;

            }
            catch (Exception ex)
            {
                Log4NetLogger.Info("Error : " + ex.Message + " stack : " + ex.StackTrace);
                updateFiveMinStagingCCOutput.status = 1;
                updateFiveMinStagingCCOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(updateFiveMinStagingCCOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("Update FiveMinStagingCC Output " + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("Update FiveMinStagingCC Request Ended");
            HttpResponse.Output.Write(xmlResult);
        }

        #endregion
    }
}