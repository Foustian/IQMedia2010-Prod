using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Logic.IOS;
using IQMediaGroup.Common.IOS.Util;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class UpdateRawMediaLog : BaseCommand, ICommand
    {
        public Int64? _LogID { get; private set; }
        public Int16? _PercentPlayed { get; private set; }

        public UpdateRawMediaLog(object LogID, object PercentPlayed)
        {
            _LogID = (LogID is NullParameter) ? null : (Int64?)LogID;
            _PercentPlayed = (PercentPlayed is NullParameter) ? null : (Int16?)PercentPlayed;
        }

        public void Execute(HttpRequest request, HttpResponse response)
        {
            try
            {
                if (_LogID.HasValue && _PercentPlayed.HasValue)
                {
                    var logic = (TrackingLogic)LogicFactory.GetLogic(LogicType.Tracking);
                    logic.UpdateLog(_LogID.Value, _PercentPlayed.Value);
                    response.Write("success=1");
                }
                else
                {
                    Log4NetLogger.Error("Invalid or missing input.");
                    response.Write("success=0");
                }
            }
            catch (InvalidOperationException ex)
            {
                Log4NetLogger.Error(String.Format("Invalid ID for raw media log ({0}).", _LogID), ex);
                response.Write("success=0");
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("UpdateRawMediaLog failed.", ex);
                response.Write("success=0");
            }
        }
    }
}