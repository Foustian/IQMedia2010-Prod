using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.IOS.Util;
using System.IO;
using IQMediaGroup.Services.IOS.Web.Serializers;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Logic.IOS;
using System.Configuration;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class UpdateUploadStatus : ICommand
    {
        public void Execute(HttpRequest request, HttpResponse response)
        {
            string jsonResult = string.Empty;
            UpdateUploadStatusOutput updateUploadStatusOutput = new UpdateUploadStatusOutput();
            try
            {
                Log4NetLogger.Info("IOS Upload Video Request Started");

                UpdateUploadStatusInput updateUploadStatusInput = new UpdateUploadStatusInput();
                try
                {
                    StreamReader StreamReader = new StreamReader(request.InputStream);
                    string input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        Log4NetLogger.Debug(input);
                    }
                    updateUploadStatusInput = (UpdateUploadStatusInput)Serializer.JsonDeserialize(input, updateUploadStatusInput.GetType());
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (!updateUploadStatusInput.ID.HasValue)
                    throw new ArgumentException("Invalid or missing ID.");

                if (string.IsNullOrEmpty(updateUploadStatusInput.Status))
                    throw new ArgumentException("Invalid or missing Status.");


                var videoLogic = (VideoLogic)LogicFactory.GetLogic(LogicType.Video);
                updateUploadStatusOutput = videoLogic.UpdateUploadTrackingStatus(updateUploadStatusInput);

            }
            catch (CustomException ex)
            {
                updateUploadStatusOutput.Message = ex.Message;
                updateUploadStatusOutput.Status = -2;

                Log4NetLogger.Error("Upload Video - CustomException: ", ex);
            }
            catch (ArgumentException ex)
            {
                updateUploadStatusOutput.Message = ex.Message;
                updateUploadStatusOutput.Status = -1;

                Log4NetLogger.Error("Upload Video - ArgumentException: ", ex);
            }
            catch (Exception ex)
            {
                updateUploadStatusOutput.Message = "An error occurred, please try again.";
                updateUploadStatusOutput.Status = -3;

                Log4NetLogger.Error("Upload Video - Exception: ", ex);
            }

            jsonResult = Serializer.JsonSearialize(updateUploadStatusOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("Upload Video - Output : " + jsonResult);
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/json";
            Log4NetLogger.Info("IOS Upload Video Request Ended");

            response.Output.Write(jsonResult);
        }
    }
}