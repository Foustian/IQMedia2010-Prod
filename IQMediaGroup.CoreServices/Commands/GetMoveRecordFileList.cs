using System;
using System.Web;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Commands
{
    public class GetMoveRecordFileList : ICommand
    {
        public int? _RPID { get; private set; }
        public DateTime? _FromDate { get; private set; }
        public DateTime? _ToDate { get; private set; }
        public int? _NumRecords { get; private set; }

        public GetMoveRecordFileList(object RPID, object FromDate, object ToDate, object NumRecords)
        {
            _RPID = (RPID is NullParameter) ? null : (int?)RPID;
            _FromDate = (FromDate is NullParameter) ? null : (DateTime?)FromDate;
            _ToDate = (ToDate is NullParameter) ? null : (DateTime?)ToDate;
            _NumRecords = (NumRecords is NullParameter) ? null : (int?)NumRecords;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            var svcName = "GetMoveRecordFileList";
            Logger.LogInfo(svcName+" Request Started");

            var output = new GetMoveRecordFileListOutput();

            try
            {
                if (_RPID == null || _RPID <= 0)
                {
                    throw new ArgumentException("Invalid or missing RPID.");
                }

                if (_FromDate == null)
                {
                    throw new ArgumentException("Invalid or missing FromDate.");
                }

                if (_ToDate == null)
                {
                    throw new ArgumentException("Invalid or missing ToDate.");
                }

                if (_FromDate > _ToDate)
                {
                    throw new ArgumentException("Invalid or missing ToDate.");
                }

                if (_NumRecords == null || _NumRecords <= 0)
                {
                    _NumRecords = 10;
                }                

                var mmLgc = (MoveMediaLogic)LogicFactory.GetLogic(LogicType.MoveMedia);
                output.MoveRecordFileList = mmLgc.GetMoveMediaByRPID(_RPID.Value, _FromDate.Value, _ToDate.Value, _NumRecords.Value);
                output.Status = 0;
                output.Message = "Success";
            }
            catch (ArgumentException ex)
            {
                Logger.LogInfo("Error: " + svcName + " - " + ex.ToString());
                output.Status = -2;
                output.Message = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error: " + svcName + " - " + ex.ToString());
                output.Status = -3;
                output.Message = ex.Message;
            }
            
            IQMediaGroup.CoreServices.Util.CommonFunctions.ReturnResponse(HttpResponse, output, svcName);
        }
    }
}