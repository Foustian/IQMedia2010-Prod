using System;
using System.Web;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;
using IQMediaGroup.CoreServices.Logic;

namespace IQMediaGroup.CoreServices.Commands
{
    public class GetMoveMediaList : ICommand
    {
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            var svcName = "GetMoveMediaList";
            var output = new GetMoveMediaListOutput();

            try
            {
                var mm = IQMediaGroup.CoreServices.Util.CommonFunctions.InitializeRequest<GetMoveMediaListInput>(HttpRequest, svcName);

                if (string.IsNullOrWhiteSpace(mm.OriginSite) && string.IsNullOrWhiteSpace(mm.DestinationSite) && string.IsNullOrWhiteSpace(mm.OriginStatus) && string.IsNullOrWhiteSpace(mm.DestinationStatus))
                {
                    throw new ArgumentException("Atleast one search parameter must exist.");
                }

                if (mm.NumRecords <= 0)
                {
                    mm.NumRecords = 10;
                }

                var mmLgc = (MoveMediaLogic)LogicFactory.GetLogic(LogicType.MoveMedia);
                output.MoveMediaList = mmLgc.GetMoveMedia(mm);
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