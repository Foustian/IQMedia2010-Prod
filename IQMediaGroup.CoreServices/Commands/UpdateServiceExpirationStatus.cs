using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.CoreServices.Serializers;
using System.Configuration;

namespace IQMediaGroup.CoreServices.Commands
{
    public class UpdateServiceExpirationStatus : ICommand
    {
        public Guid? _GUID { get; private set; }
        public string _Status { get; private set; }

        public UpdateServiceExpirationStatus(object guid, object status)
        {
            _GUID = (guid is NullParameter) ? null : (Guid?)guid;
            _Status = (status is NullParameter) ? null : Convert.ToString(status);
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            var updateServiceExpirationOutput = new UpdateServiceExpirationOutput();
            Logger.LogInfo("Update ServiceExpirationStatus Request Started");
            try
            {
                if (_GUID == null)
                    throw new ArgumentException("invalid or missing guid");

                if (string.IsNullOrWhiteSpace(_Status))
                    throw new ArgumentException("invalid or missing status");

                if (!Enum.GetNames(typeof(IQServiceExpirationStatus)).Contains(_Status.ToUpper()))
                {
                    throw new ArgumentException("Invalid Status");
                }
                else
                {
                    var serviceExpirationLogic = (ServiceExpirationLogic)LogicFactory.GetLogic(LogicType.ServiceExpiration);
                    if (serviceExpirationLogic.UpdateIQServiceExpirationStatus(_Status.ToUpper(), _GUID))
                    {
                        updateServiceExpirationOutput.status = 0;
                        updateServiceExpirationOutput.message = "Success";
                    }
                    else
                    {
                        updateServiceExpirationOutput.status = 1;
                        updateServiceExpirationOutput.message = "Record not updated";
                    }
                }

            }
            catch (ArgumentException ex)
            {
                updateServiceExpirationOutput.status = 1;
                updateServiceExpirationOutput.message = ex.Message;
            }
            catch (Exception ex)
            {
                updateServiceExpirationOutput.status = 1;
                updateServiceExpirationOutput.message = "An error occurred, please try again.";
            }

            xmlResult = Serializer.SerializeToXml(updateServiceExpirationOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("Update ServiceExpirationStatus  Output " + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Logger.LogInfo("Update ServiceExpirationStatus Request Ended");
            HttpResponse.Output.Write(xmlResult);

        }

        public enum IQServiceExpirationStatus
        {
            CLIPPING,
            DELETED,
            DOUBLE,
            DUPLICATE,
            NEW,
            QUEUED,
            READY,
            WEBDELETED,            
            CLIP_EXISTS,
            FILE_MISSING
        }


    }
}
