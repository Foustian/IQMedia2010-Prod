using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Serializers;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Commands
{
    public class GetFiveMinStaging : ICommand
    {
        #region ICommand Members

        public string _iqcckey { get; private set; }

        public GetFiveMinStaging(object iqcckey)
        {
            _iqcckey = (iqcckey is NullParameter) ? null : (string)iqcckey;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            var fiveMinStagingOutput = new FiveMinStagingOutput();
            Logger.LogInfo("Get FiveMinStaging Request Started");
            try
            {
                if (_iqcckey == null)
                    throw new ArgumentException("invalid or missing iqcckey");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("Get FiveMinStaging Input iqcckey: " + _iqcckey);
                }

                var fiveMinStagingLogic = (FiveMinStagingLogic)LogicFactory.GetLogic(LogicType.FiveMinStaging);
                fiveMinStagingOutput = fiveMinStagingLogic.GetFiveMinStaging(_iqcckey);

            }
            catch (ArgumentException ex)
            {
                fiveMinStagingOutput.status = 1;
                fiveMinStagingOutput.message = ex.Message;

            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                fiveMinStagingOutput.status = 1;
                fiveMinStagingOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(fiveMinStagingOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Get FiveMinStaging Output " + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Logger.LogInfo("Get FiveMinStaging Request Ended");
            HttpResponse.Output.Write(xmlResult);
        }

        #endregion
    }
}