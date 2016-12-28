using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Domain;
using System.IO;
using System.Configuration;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class EmailService : ICommand
    {

        #region ICommand Members

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {

            string _JSONResult = string.Empty;
            string _JSONRequest = string.Empty;
            try
            {
                CLogger.Debug("Email Request Started");
                EmaiInput _EmailInput = new EmaiInput();

                StreamReader StreamReader = new StreamReader(HttpRequest.InputStream);

                try
                {
                    _JSONRequest = StreamReader.ReadToEnd();
                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug(_JSONRequest);
                    }
                    _EmailInput = (EmaiInput)Serializer.Deserialize(_JSONRequest, _EmailInput.GetType());                    
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                ValidationLogic ValidationLogic = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);

                bool IsValidate = ValidationLogic.ValidateEmailInput(_EmailInput);

                if (IsValidate)
                {
                    EmailServiceLogic _objEmailServiceLogic = (EmailServiceLogic)LogicFactory.GetLogic(LogicType.EmailService);

                    _JSONResult = _objEmailServiceLogic.SendEmail(_EmailInput);

                    _JSONResult = Serializer.Searialize(_JSONResult);
                }
                else
                {
                    _JSONResult = Serializer.Searialize("Invalid Input");
                }


            }
            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace + "Inner Exception : " + ex.InnerException);
                _JSONResult = Serializer.Searialize("1");
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug(_JSONResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("Email Request Ended");

            HttpResponse.Output.Write(_JSONResult);

        }

        #endregion
    }
}