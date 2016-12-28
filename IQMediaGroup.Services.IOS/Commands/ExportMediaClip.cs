using System;
using System.Web;
using IQMediaGroup.Common.IOS.Util;
using IQMediaGroup.Logic.IOS;
using IQMediaGroup.Domain.IOS;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class ExportMediaClip : BaseCommand, ICommand
    {
        public Guid? _clipGUID;

        public ExportMediaClip(object fid)
        {
            _clipGUID = (fid is NullParameter) ? null : (Guid?)fid;
        }


        #region ICommand Region

        public void Execute(HttpRequest request, HttpResponse response)
        {
            var output = new MediaClipExportOutput();

            try
            {
                Log4NetLogger.Info("ExportMediaClip request started.");

                if (!IQMedia.Web.Common.Authentication.IsAuthenticated)
                {
                    throw new System.Security.Authentication.AuthenticationException();
                }

                if (!_clipGUID.HasValue)
                {
                    throw new ArgumentException("Invalid or missing Clip Guid.");
                }

                output.Status = 0;
                output.Message = "Success";

                var clpLogic = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);

                if (clpLogic.QueueIOSExport(_clipGUID.Value))
                {
                    Log4NetLogger.Info("Clip successfully queued for Media Clip Export: " + _clipGUID);
                }
                else
                {
                    Log4NetLogger.Info("Clip already queued OR exported for Media Clip Export: " + _clipGUID);
                }
            }
            catch (System.Security.Authentication.AuthenticationException)
            {
                Log4NetLogger.Error("Authentication Faiiled.");

                output.Status = -1;
                output.Message = "Access denied.";
            }            
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);

                output.Status = -2;
                output.Message = "Error.";
            }

            var outputStr = Serializers.Serializer.JsonSearialize(output);

            Log4NetLogger.Info(outputStr);

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/json";
            Log4NetLogger.Info("ExportMediaClip Request Ended");

            response.Output.Write(outputStr);
        }

        #endregion
    }
}