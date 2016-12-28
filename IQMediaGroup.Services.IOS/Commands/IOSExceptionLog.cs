using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.IOS.Util;
using System.IO;
using System.Configuration;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Services.IOS.Web.Serializers;
using IQMediaGroup.Logic.IOS;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class IOSExceptionLog : BaseCommand, ICommand
    {
        public void Execute(HttpRequest request, HttpResponse response)
        {
            string jsonResult = string.Empty;
            IOSExceptionLogOutput iOSExceptionLogOutput = new IOSExceptionLogOutput();
            try
            {
                Log4NetLogger.Info("IOS Exception Log Request Started");

                IOSExceptionLogInput iOSExceptionLogInput = new IOSExceptionLogInput();
                try
                {
                    StreamReader StreamReader = new StreamReader(request.InputStream);
                    string input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        Log4NetLogger.Debug(input);
                    }
                    iOSExceptionLogInput = (IOSExceptionLogInput)Serializer.Deserialize(input, iOSExceptionLogInput.GetType());
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (string.IsNullOrEmpty(iOSExceptionLogInput.Exception))
                    throw new ArgumentException("Invalid or missing Exception.");

                if (string.IsNullOrEmpty(iOSExceptionLogInput.UID))
                    throw new ArgumentException("Invalid or missing Unique ID.");

                if (string.IsNullOrEmpty(iOSExceptionLogInput.Application))
                    throw new ArgumentException("Invalid or missing Application.");

                var customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                iOSExceptionLogOutput = customerLogic.LogException(iOSExceptionLogInput);

                Log4NetLogger.Debug("IOS Exception Log - UDID : " + iOSExceptionLogInput.UID + " staus : " + iOSExceptionLogOutput.Status);

            }
            catch (CustomException ex)
            {
                iOSExceptionLogOutput.Message = ex.Message;
                iOSExceptionLogOutput.Status = -2;

                Log4NetLogger.Error("IOS Exception Log - CustomException: ", ex);
            }
            catch (ArgumentException ex)
            {
                iOSExceptionLogOutput.Message = ex.Message;
                iOSExceptionLogOutput.Status = -1;

                Log4NetLogger.Error("IOS Exception Log - ArgumentException: ", ex);
            }
            catch (Exception ex)
            {
                iOSExceptionLogOutput.Message = "An error occurred, please try again.";
                iOSExceptionLogOutput.Status = -3;

                Log4NetLogger.Error("IOS Exception Log - Exception: ", ex);
            }

            jsonResult = Serializer.Searialize(iOSExceptionLogOutput);

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("IOS Exception Log - Output : " + jsonResult);
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/json";
            Log4NetLogger.Info("IOS Exception Log Request Ended");

            response.Output.Write(jsonResult);
        }
    }
}