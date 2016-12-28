using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.IOS.Util;
using System.IO;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Services.IOS.Web.Serializers;
using System.Configuration;
using IQMediaGroup.Logic.IOS;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class UploadVideo : ICommand
    {
        public void Execute(HttpRequest request, HttpResponse response)
        {
            string jsonResult = string.Empty;
            UploadVideoOutput uploadVideoOutput = new UploadVideoOutput();
            try
            {
                Log4NetLogger.Info("IOS Upload Video Request Started");

                UploadVideoInput uploadVideoInput = new UploadVideoInput();
                try
                {
                    StreamReader StreamReader = new StreamReader(request.InputStream);
                    string input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        Log4NetLogger.Debug(input);
                    }
                    uploadVideoInput = (UploadVideoInput)Serializer.JsonDeserialize(input, uploadVideoInput.GetType());
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (!uploadVideoInput.UID.HasValue)
                    throw new ArgumentException("Invalid or missing UID.");

                if (!uploadVideoInput.CatID.HasValue)
                    throw new ArgumentException("Invalid or missing CatID.");

                if (!uploadVideoInput.DT.HasValue)
                    throw new ArgumentException("Invalid or missing DT.");

                if (string.IsNullOrEmpty(uploadVideoInput.TimeZone))
                    throw new ArgumentException("Invalid or missing TimeZone.");

                if (string.IsNullOrEmpty(uploadVideoInput.FileName))
                    throw new ArgumentException("Invalid or missing FileName.");


                var videoLogic = (VideoLogic)LogicFactory.GetLogic(LogicType.Video);
                uploadVideoOutput = videoLogic.InsertVideoUpload(uploadVideoInput);

            }
            catch (CustomException ex)
            {
                uploadVideoOutput.Message = ex.Message;
                uploadVideoOutput.Status = -2;

                Log4NetLogger.Error("Upload Video - CustomException: ", ex);
            }
            catch (ArgumentException ex)
            {
                uploadVideoOutput.Message = ex.Message;
                uploadVideoOutput.Status = -1;

                Log4NetLogger.Error("Upload Video - ArgumentException: ", ex);
            }
            catch (Exception ex)
            {
                uploadVideoOutput.Message = "An error occurred, please try again.";
                uploadVideoOutput.Status = -3;

                Log4NetLogger.Error("Upload Video - Exception: ", ex);
            }

            jsonResult = Serializer.JsonSearialize(uploadVideoOutput);

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