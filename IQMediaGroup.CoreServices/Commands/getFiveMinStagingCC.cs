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
    public class getFiveMinStagingCC : ICommand
    {
        #region ICommand Members
        private string _iqcckey;

        public getFiveMinStagingCC(object iqcckey)
        {
            _iqcckey = (iqcckey is NullParameter) ? null : (string)iqcckey;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            Log4NetLogger.Info("Insert Five Min Staging with CC Request Started.");
            var fiveMinStagingCCOutput = new FiveMinStagingCCOutput();

            try
            {
                if (_iqcckey == null)
                    throw new ArgumentException("invalid or missing iqcckey");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Log4NetLogger.Info("Get FiveMinStagingCC Input iqcckey: " + _iqcckey);
                }

                var fiveMinStagingLogic = (FiveMinStagingLogic)LogicFactory.GetLogic(LogicType.FiveMinStaging);
                fiveMinStagingCCOutput = fiveMinStagingLogic.GetFiveMinStagingCC(_iqcckey);
            }
            catch (ArgumentException ex)
            {
                fiveMinStagingCCOutput.status = 1;
                fiveMinStagingCCOutput.message = ex.Message;

            }
            catch (Exception ex)
            {

                Log4NetLogger.Info("Error : " + ex.Message + " stack : " + ex.StackTrace);
                fiveMinStagingCCOutput.status = 1;
                fiveMinStagingCCOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(fiveMinStagingCCOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("Get FiveMinStaging Output " + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("Get FiveMinStaging Request Ended");
            HttpResponse.Output.Write(xmlResult);

        }
        #endregion
    }
}