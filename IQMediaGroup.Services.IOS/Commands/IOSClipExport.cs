using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.IOS.Util;
using IQMediaGroup.Services.IOS.Web.Serializers;
using IQMediaGroup.Logic.IOS;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
   public class IOSClipExport : BaseCommand, ICommand
    {
        public Guid? clipGUID;
        string jsonResult = string.Empty;

        public IOSClipExport(object p_clipGUID)
        {
            clipGUID = (p_clipGUID is NullParameter) ? null : (Guid?)p_clipGUID;
        }


        #region ICommand Region

        public void Execute(HttpRequest request, HttpResponse response)
        {
            try
            {
                Log4NetLogger.Info("IOS Clip Export Request Started");

                if (!clipGUID.HasValue)
                    throw new ArgumentException("Invalid or missing Clip Guid.");

                var clpLogic = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);
                if(clpLogic.QueueIOSExport(clipGUID.Value))
                    jsonResult = Serializer.Searialize("Clip Successfully Queued for IOS Export: " + clipGUID);
                else
                    jsonResult = Serializer.Searialize("Clip Already Queued OR Exported for IOS Export: " + clipGUID);

            }
            catch (ArgumentException ex)
            {
                jsonResult = Serializer.Searialize(ex.Message);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("Error : " + ex.Message + " stack : " + ex.StackTrace, ex);
                jsonResult = Serializer.Searialize("An error occurred, please try again.");
            }

            Log4NetLogger.Info(jsonResult);

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/json";
            Log4NetLogger.Info("IOS Clip Export Request Ended");

            response.Output.Write(jsonResult);
        }

        #endregion
    }
}