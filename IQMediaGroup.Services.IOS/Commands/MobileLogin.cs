using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Common.IOS.Util;
using IQMediaGroup.Logic.IOS;
using IQMediaGroup.Services.IOS.Web.Serializers;
using System.IO;
using System.Configuration;

namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class MobileLogin : BaseCommand, ICommand
    {
        
        public void Execute(HttpRequest request, HttpResponse response)
        {
            string jsonResult = string.Empty;
            MobileLogInOutput mobileLogInOutput = new MobileLogInOutput();
            try
            {
                Log4NetLogger.Info("IOS Mobile Login Request Started");

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

                    Log4NetLogger.Debug("Login - UserID : " + mobileLogInInput.UserID);
                    Log4NetLogger.Debug("Login - Pwd : " + mobileLogInInput.Password);
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                if (string.IsNullOrEmpty(mobileLogInInput.UserID))
                    throw new ArgumentException("Invalid or missing UserID.");

                if (string.IsNullOrEmpty(mobileLogInInput.Password))
                    throw new ArgumentException("Invalid or missing Password.");

                if (string.IsNullOrEmpty(mobileLogInInput.Application))
                    throw new ArgumentException("Invalid or missing Application.");

                if (string.IsNullOrEmpty(mobileLogInInput.Version))
                    throw new ArgumentException("Invalid or missing Version.");

                var UDID = Guid.NewGuid();

                mobileLogInInput.UID = UDID.ToString();

                var customerLogic = (CustomerLogic)LogicFactory.GetLogic(LogicType.Customer);
                mobileLogInOutput = customerLogic.MobileLogin(mobileLogInInput,Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordAttempts"]));

                mobileLogInOutput.UID = UDID.ToString();

                Log4NetLogger.Debug("MobileLogin - UDID : " + mobileLogInInput.UID + " mobilelogin : " + mobileLogInOutput.MobileLogin + " staus : " + mobileLogInOutput.Status);

            }
            catch (CustomException ex)
            {
                mobileLogInOutput.Message = ex.Message;
                mobileLogInOutput.Status = -2;

                Log4NetLogger.Error("MobileLogin - CustomException: ",ex);
            }
            catch (ArgumentException ex)
            {
                mobileLogInOutput.Message = ex.Message;
                mobileLogInOutput.Status = -1;

                Log4NetLogger.Error("MobileLogin - ArgumentException: ", ex);
            }
            catch (Exception ex)
            {                
                mobileLogInOutput.Message = "An error occurred, please try again.";
                mobileLogInOutput.Status = -3;

                Log4NetLogger.Error("MobileLogin - Exception: ", ex);
            }

            jsonResult = Serializer.Searialize(mobileLogInOutput);
            
            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                Log4NetLogger.Info("MobileLogin - Output : " + jsonResult);
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/json";
            Log4NetLogger.Info("IOS Mobile Login Request Ended");

            response.Output.Write(jsonResult);
        }
    }
}