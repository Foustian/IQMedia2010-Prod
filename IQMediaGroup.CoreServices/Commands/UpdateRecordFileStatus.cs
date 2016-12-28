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

    public class UpdateRecordFileStatus : ICommand
    {
        public Guid? _GUID { get; private set; }
        public String _Status { get; private set; }

        public UpdateRecordFileStatus(object guid, object Status)
        {
            _GUID = (guid is NullParameter) ? null : (Guid?)(guid);
            _Status = (Status is NullParameter) ? null : Convert.ToString(Status);
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string xmlResult = string.Empty;
            Logger.LogInfo("UpdateRecordFileStatus Request Started");
            var recordFileUpdateOutput = new RecordFileUpdateOutput();
            try
            {
                if (_GUID == null || !_GUID.HasValue)
                {
                    throw new ArgumentException("Invalid or Missing GUID");

                }
                if (string.IsNullOrWhiteSpace(_Status))
                {
                    throw new ArgumentException("Invalid or Missing Status");
                }
                if (!Enum.GetNames(typeof(IQCoreRecordFileStatus)).Contains(_Status.ToUpper()))
                {
                    throw new ArgumentException("Invalid Status");
                }
                else
                {
                    var recordFileLogic = (RecordFileLogic)LogicFactory.GetLogic(LogicType.RecordFile);
                    if (recordFileLogic.UpdateRecordFileStatus(_GUID, _Status.ToUpper()))
                    {
                        recordFileUpdateOutput.Status = 0;
                        recordFileUpdateOutput.Message = "Success";
                    }
                    else
                    {
                        recordFileUpdateOutput.Status = 1;
                        recordFileUpdateOutput.Message = "Record not updated.";
                    }
                }
            }
            catch (ArgumentException ex)
            {
                recordFileUpdateOutput.Status = 1;
                recordFileUpdateOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error : " + ex.Message + " stack : " + ex.StackTrace);
                recordFileUpdateOutput.Status = 1;
                recordFileUpdateOutput.Message = ex.Message;
            }

            xmlResult = Serializers.Serializer.SerializeToXml(recordFileUpdateOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Logger.LogInfo("UpdateRecordFileStatus Output :" + xmlResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/xml";
            Log4NetLogger.Info("UpdateRecordFileStatus Request Ended.");
            HttpResponse.Output.Write(xmlResult);

        }

        public enum IQCoreRecordFileStatus
        {
            CLIPPING,
            DELETED,
            DOUBLE,
            DUPLICATE,
            NEW,
            QUEUED,
            READY,
            WEBDELETED

        }
    }
}