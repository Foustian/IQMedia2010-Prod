using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Util;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Commands
{
    public class InsertMoveMedia : ICommand
    {
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            var output = new InsertMoveMediaOutput();
            var svcName = "InsertMoveMedia";
            try
            {
                var mm = IQMediaGroup.CoreServices.Util.CommonFunctions.InitializeRequest<InsertMoveMediaInput>(HttpRequest, svcName);

                if (mm.MoveMedia.OriginRPID <= 0)
                {
                    throw new ArgumentException("Invalid or missing Origin RPID");
                }

                if (mm.MoveMedia.RecordFileGUID == null || mm.MoveMedia.RecordFileGUID == Guid.Empty)
                {
                    throw new ArgumentException("Invalid or missing RecordfileGUID");
                }

                if (string.IsNullOrWhiteSpace(mm.MoveMedia.OriginLocation))
                {
                    throw new ArgumentException("Invalid or missing Origin Location");
                }

                if (string.IsNullOrWhiteSpace(mm.MoveMedia.OriginStatus))
                {
                    throw new ArgumentException("Invalid or missing Origin Status");
                }

                if (string.IsNullOrWhiteSpace(mm.MoveMedia.OriginSite))
                {
                    throw new ArgumentException("Invalid or missing Origin Site");
                }

                var mmLgc = (MoveMediaLogic)LogicFactory.GetLogic(LogicType.MoveMedia);
                /*
                Possible values of OutStatus are:
                1 (Default value) - record not inserted
                -1 - record exist with same RecordfileGUID and OriginRPID
                0 - record inserted successfully
                */
                int outStatus = 1;
                Int64 id=mmLgc.InsertMoveMedia(mm.MoveMedia, out outStatus);

                if (outStatus==0 && id>0)
                {
                    output.ID = id;
                    output.Status = 0;
                    output.Message = "Success";
                }
                else if(outStatus==-1)
                {
                    output.Status = 2;
                    output.Message = "Record is already existed.";
                }
                else
                {
                    output.Status = 1;
                    output.Message = "Record couldn't be inserted.";
                }
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
                output.Status = -2;
                output.Message = ex.Message;
            }

            IQMediaGroup.CoreServices.Util.CommonFunctions.ReturnResponse(HttpResponse, output, svcName);
        }
    }
}