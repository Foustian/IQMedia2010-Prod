using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.IOS.Util;
using IQMediaGroup.Services.IOS.Web.Serializers;
using IQMediaGroup.Logic.IOS;
using IQMediaGroup.Domain.IOS;
using System.IO;
using System.Configuration;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class IsRegd : BaseCommand, ICommand
    {
        public void Execute(HttpRequest request, HttpResponse response)
        {
            string jsonResult = string.Empty;
            IsRegisteredOutput isRegisteredOutput = new IsRegisteredOutput();
            try
            {
                Log4NetLogger.Info("IOS IsRegistered Request Started");
                
                MobileLogInInput mobileLogInInput = new MobileLogInInput();
                try
                {
                    StreamReader StreamReader = new StreamReader(request.InputStream);
                    string input = StreamReader.ReadToEnd();

                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        Log4NetLogger.Debug(input);
                    }
                    mobileLogInInput = (MobileLogInInput)Serializer.Deserialize(input, mobileLogInInput.GetType());

                    Log4NetLogger.Debug("IsRegd - UDID : " + mobileLogInInput.UID);
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (string.IsNullOrEmpty(mobileLogInInput.UID))
                    throw new ArgumentException("Invalid or missing UDID.");

                if (string.IsNullOrEmpty(mobileLogInInput.Version))
                    throw new ArgumentException("Invalid or missing Version.");

                var customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                isRegisteredOutput = customerLogic.IsCustomerRegistered(mobileLogInInput);

                Log4NetLogger.Debug("IsRegd - UDID : " + mobileLogInInput.UID + " isRegd : " + isRegisteredOutput.IsRegistered + " staus : "+ isRegisteredOutput.Status);

            }
            catch (ArgumentException ex)
            {
                isRegisteredOutput.Message = ex.Message;
                isRegisteredOutput.Status = -1;

                Log4NetLogger.Error("IsRegd - ArgumentException: ", ex);
            }
            catch (CustomException ex)
            {
                Log4NetLogger.Error(ex);
                isRegisteredOutput.Status = -2;
                isRegisteredOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                isRegisteredOutput.Message = "An error occurred, please try again.";
                isRegisteredOutput.Status = -3;

                Log4NetLogger.Error("IsRegd - Exception: ", ex);
            }

            jsonResult = Serializer.Searialize(isRegisteredOutput);
            
            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("IsRegd - Output : "+jsonResult);
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/json";
            Log4NetLogger.Info("IOS Is Registered Request Ended");

            response.Output.Write(jsonResult);
        }
    }
}