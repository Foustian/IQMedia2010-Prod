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
    public class InsertFiveMinStaging : ICommand
    {
         public string _iqcckey { get; private set; }

         public InsertFiveMinStaging(object iqcckey)
        {
            _iqcckey = (iqcckey is NullParameter) ? null : (string)iqcckey;
        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            var insertFiveMinStagingOutput = new InsertFiveMinStagingOutput();
            Logger.LogInfo("Insert FiveMinStaging Request Started");
            try
            {
                if (_iqcckey == null)
                    throw new ArgumentException("invalid or missing iqcckey");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("Insert FiveMinStaging Input iqcckey: " + _iqcckey);
                }

                var fiveMinStagingLogic = (FiveMinStagingLogic)LogicFactory.GetLogic(LogicType.FiveMinStaging);
                insertFiveMinStagingOutput = fiveMinStagingLogic.InsertFiveMinStaging(_iqcckey);

            }
            catch (ArgumentException ex)
            {
                insertFiveMinStagingOutput.status = 1;
                insertFiveMinStagingOutput.message = ex.Message;

            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                insertFiveMinStagingOutput.status = 1;
                insertFiveMinStagingOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(insertFiveMinStagingOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Insert FiveMinStaging Output " + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Logger.LogInfo("Insert FiveMinStaging Request Ended");
            HttpResponse.Output.Write(xmlResult);
        }

    }
}