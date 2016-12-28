using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.CoreServices.Serializers;

namespace IQMediaGroup.CoreServices.Commands
{
    public class InsertFiveMinStagingCC : ICommand
    {

        private readonly string _iqcckey;

        public InsertFiveMinStagingCC(object iqcckey)
        {
            _iqcckey = (iqcckey is NullParameter) ? null : (string)iqcckey;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            Log4NetLogger.Info("Insert Five Min Staging with CC Request Started.");
            var insertFiveMinStagingCCOutput = new InsertFiveMinStagingCCOutput();

            try
            {
                if (_iqcckey == null)
                    throw new ArgumentException("invalid or missing iqcckey");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Log4NetLogger.Info("Insert FiveMinStagingCC Input iqcckey: " + _iqcckey);
                }

                var fiveMinStagingLogic = (FiveMinStagingLogic)LogicFactory.GetLogic(LogicType.FiveMinStaging);
                insertFiveMinStagingCCOutput = fiveMinStagingLogic.InsertFiveMinStagingwithCC(_iqcckey);
            }
            catch (ArgumentException ex)
            {
                insertFiveMinStagingCCOutput.status = 1;
                insertFiveMinStagingCCOutput.message = ex.Message;

            }
            catch (Exception ex)
            {

                Log4NetLogger.Info("Error : " + ex.Message + " stack : " + ex.StackTrace);
                insertFiveMinStagingCCOutput.status = 1;
                insertFiveMinStagingCCOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(insertFiveMinStagingCCOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("Insert FiveMinStagingCC Output " + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("Insert FiveMinStaging Request Ended");
            HttpResponse.Output.Write(xmlResult);
        }
    }
}