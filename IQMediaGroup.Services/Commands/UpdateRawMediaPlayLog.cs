using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Logic;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Domain;
using System.Configuration;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class UpdateRawMediaPlayLog : ICommand
    {
        public Int64? _LogID { get; private set; }
        public Int16? _SecondsPlayed { get; private set; }

        public UpdateRawMediaPlayLog(object LogID, object SecondsPlayed)
        {
            _LogID = (LogID is NullParameter) ? null : (Int64?)LogID;
            _SecondsPlayed = (SecondsPlayed is NullParameter) ? null : (Int16?)SecondsPlayed;
        }

        public void Execute(HttpRequest request, HttpResponse response)
        {
            var output = new CommandOutput();

            try
            {
                if (!_LogID.HasValue || !_SecondsPlayed.HasValue)
                {
                    throw new ArgumentException("Invalid or missing arguments");
                }

                bool logPlays;
                Boolean.TryParse(ConfigurationManager.AppSettings["LogRawMediaPlays"], out logPlays);
                if (logPlays)
                {
                    var logic = (TrackingLogic)LogicFactory.GetLogic(LogicType.Tracking);
                    if (logic.UpdateLog(_LogID.Value, _SecondsPlayed.Value) > 0)
                    {
                        output.Status = 1;
                    }
                    else
                    {
                        output.Status = -3;
                        output.Message = String.Format(String.Format("No logs were updated with ID = {0}.", _LogID));
                    }
                }
                else
                {
                    output.Status = 0;
                    output.Message = "Logging disabled";
                }
            }
            catch (ArgumentException ex)
            {
                CLogger.Error(String.Format("UpdateRawMediaPlayLog({0}, {1})", _LogID, _SecondsPlayed), ex);
                output.Status = -2;
                output.Message = "Invalid or missing input";
            }
            catch (Exception ex)
            {
                CLogger.Error(String.Format("UpdateRawMediaPlayLog({0}, {1})", _LogID, _SecondsPlayed), ex);
                output.Status = -1;
                output.Message = "Error";
            }

            response.Write(Serializer.Searialize(output));
        }
    }
}