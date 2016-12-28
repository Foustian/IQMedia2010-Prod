using System;
using System.Web;
using IQMediaGroup.Domain.CaptureLog;
using IQMediaGroup.Logic.CaptureLog;
using IQMediaGroup.Common.Util;
using System.Configuration;

namespace IQMediaGroup.Services.CaptureLog.Commands
{
    public class Capturesvc : ICommand
    {

        public string _CaptureData { get; private set; }
        public string _Status { get; private set; }
        public string _Source { get; private set; }

        public Capturesvc(object CaptureData, object Status)
        {
            _CaptureData = (CaptureData is NullParameter) ? null : (string)CaptureData;
            _Status = (Status is NullParameter) ? null : (string)Status;
        }
        #region ICommand Members
        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            Log4NetLogger.Info("CaptureLog Service Started");
            
            string ReqResult = string.Empty;

            try
            {

                if (_Status == null)
                {
                    throw new ArgumentException("Invalid or missing Status");
                }

                CapturesvcLogic _CapturesvcLogic = (CapturesvcLogic)LogicFactory.GetLogic(LogicType.Capturesvc);
                _Source = HttpRequest.UserHostAddress;

                Log4NetLogger.Info("{\"CaptureData\":" + _CaptureData + ",\"Status\":" + _Status + ",\"Source\":" + _Source + "}");

                ReqResult = _CapturesvcLogic.InsertCaptureLog(_CaptureData, _Status, _Source);
                 
                Log4NetLogger.Info(ReqResult);
            }

            catch (CustomException _CustomException)
            {
                ReqResult = _CustomException.Message;
                Log4NetLogger.Error(ReqResult,_CustomException);
            }
            catch (Exception ex)
            {
                ReqResult = "An error occurred, please try again";
                Log4NetLogger.Error(ex.Message,ex);
            }

            Log4NetLogger.Info("CaptureLog Service Ended");

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";


            HttpResponse.Output.Write(ReqResult);
        }
        #endregion

    }
}