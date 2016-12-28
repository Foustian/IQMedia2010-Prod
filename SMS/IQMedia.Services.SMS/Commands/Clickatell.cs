using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMedia.Services.SMS.Logic;
using IQMedia.Services.SMS.Domain;

namespace IQMedia.Services.SMS.Commands
{
    public class Clickatell : ICommand
    {
        

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _response = string.Empty;
            Log4NetLogger.Info("Clickatell Service Started");
            try
            {
                ClickatelInput clickatelInput = new ClickatelInput();

                Log4NetLogger.Info("api_id" + HttpRequest["api_id"]);
                Log4NetLogger.Info("from" + HttpRequest["from"]);
                Log4NetLogger.Info("to" + HttpRequest["to"]);
                Log4NetLogger.Info("timestamp" + HttpRequest["timestamp"]);
                Log4NetLogger.Info("text" + HttpRequest["text"]);
                Log4NetLogger.Info("charset" + HttpRequest["charset"]);
                Log4NetLogger.Info("udh" + HttpRequest["udh"]);
                Log4NetLogger.Info("moMsgId" + HttpRequest["moMsgId"]);

                DateTime _date;

                clickatelInput.CustomerPhoneNo = Convert.ToString(HttpRequest["from"]);
                clickatelInput.ReceivedDateTime = DateTime.TryParse(Convert.ToString(HttpRequest["timestamp"]), out _date) ? (DateTime?)_date : null;
                clickatelInput.MsgText = Convert.ToString(HttpRequest["text"]);
                clickatelInput.MesssagId = Convert.ToString(HttpRequest["moMsgId"]);

                ClickatellLogic clickatellLogic = (ClickatellLogic)LogicFactory.GetLogic(LogicType.Clickatell);
                if (clickatellLogic.InsertClickatell(clickatelInput))
                {
                    _response = "Clickatell request inserted successfully";
                }
                else
                {
                    _response = "Clickatell request not inserted";
                }

            }
            catch (ArgumentException ex)
            {
                Log4NetLogger.Error("invalid argument",ex);
                _response = ex.Message;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("An error occurred",ex);
                _response = "An error occurred, please try again.";
            }

            HttpResponse.Output.Write(_response);

            Log4NetLogger.Info("Clickatell Service Ended");
            
        }
    }
}