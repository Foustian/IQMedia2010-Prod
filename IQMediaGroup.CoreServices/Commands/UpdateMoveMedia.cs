using System.Web;
using System;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.CoreServices.Logic;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Commands
{
    public class UpdateMoveMedia : ICommand
    {
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            var output = new UpdateMoveMediaOutput();
            var svcName = "UpdateMoveMedia";

            try
            {
                var mm = IQMediaGroup.CoreServices.Util.CommonFunctions.InitializeRequest<UpdateMoveMediaInput>(HttpRequest, svcName);

                if (mm.MoveMedia.ID <= 0 && (mm.MoveMedia.OriginRPID <= 0 || mm.MoveMedia.RecordFileGUID == Guid.Empty))
                {
                    throw new ArgumentException("Invalid or missing ID or RecordfileGUID and OriginRPID");
                }

                if (mm.UpdateOrigin)
                {                   
                    if (string.IsNullOrWhiteSpace(mm.MoveMedia.OriginStatus))
                    {
                        throw new ArgumentException("Invalid or missing Origin Status");
                    }
                }
                else
                {
                    if (mm.MoveMedia.DestinationRPID <= 0)
                    {
                        throw new ArgumentException("Invalid or missing Destination RPID");
                    }                   

                    if (string.IsNullOrWhiteSpace(mm.MoveMedia.DestinationLocation))
                    {
                        throw new ArgumentException("Invalid or missing Destination Location");
                    }

                    if (string.IsNullOrWhiteSpace(mm.MoveMedia.DestinationStatus))
                    {
                        throw new ArgumentException("Invalid or missing Destination Status");
                    }

                    if (string.IsNullOrWhiteSpace(mm.MoveMedia.DestinationSite))
                    {
                        throw new ArgumentException("Invalid or missing Destination Site");
                    }
                }

                var mmLgc = (MoveMediaLogic)LogicFactory.GetLogic(LogicType.MoveMedia);
                int affectedrecs = mmLgc.UpdateMoveMedia(mm.MoveMedia, mm.UpdateOrigin);

                if (affectedrecs>0)
                {
                    output.Status = 0;
                    output.Message = "Success";
                }
                else
                {
                    output.Status = 1;
                    output.Message = "Record couldn't be updated.";
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