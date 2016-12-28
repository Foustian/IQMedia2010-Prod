using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Logic;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Commands
{
    public class UpdateRootPathStatus : ICommand
    {

        private readonly Int64? _rootpathID;
        private readonly int? _status;

        public UpdateRootPathStatus(object rootpathID, object status)
        {
            _rootpathID = (rootpathID is NullParameter) ? null : (Int64?)rootpathID;
            _status = (status is NullParameter) ? null : (int?)status;

        }
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            Log4NetLogger.Info("Update Root Path Request Started.");
            string xmlResult = string.Empty;
            var updateRootPathStatusOutput = new UpdateRootPathStatusOutput();
            try
            {
                if (_rootpathID == null)
                    throw new ArgumentException("ivalid or missing rootpathID");

                if (_status == null || (_status != 0 && _status!=1))
                    throw new ArgumentException("ivalid or missing status");

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    Logger.LogInfo("Update Root Path Input rootpathID: " + _rootpathID + ", status:" + _status);
                }

                var rootPathLogic = (RootPathLogic)LogicFactory.GetLogic(LogicType.RootPath);
                updateRootPathStatusOutput = rootPathLogic.UpdateRootPathStatus(_rootpathID.Value, Convert.ToBoolean(_status));
                
            }
            catch (ArgumentException ex)
            {
                updateRootPathStatusOutput.status = 1;
                updateRootPathStatusOutput.message = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                updateRootPathStatusOutput.message = "An error occurred, Please try again.";
                updateRootPathStatusOutput.status = 1;


            }
            xmlResult = Serializers.Serializer.SerializeToXml(updateRootPathStatusOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Update Root Path Output :" + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("Update Root Path Status Request Completed.");
            HttpResponse.Output.Write(xmlResult);
            
        }
    }
}