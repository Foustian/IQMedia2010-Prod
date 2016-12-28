using System;
using System.Web;
using IQMedia.Services.SMS.Logic;
using IQMedia.Services.SMS.Domain;
using System.Configuration;
namespace IQMedia.Services.SMS.Commands
{
    public class GetSubscriptionURL : ICommand
    {
        public Int64? _HubSpotID { get; private set; }
        public Int64? _SearchRequestID { get; private set; }
        public string _Format { get; private set; }
        public string _Action { get; set; }

        public GetSubscriptionURL(object HubSpotID, object SearchRequestID, object Format, object Action)
        {
            _HubSpotID = (HubSpotID is NullParameter) ? null : (Int64?)HubSpotID;
            _SearchRequestID = (SearchRequestID is NullParameter) ? null : (Int64?)SearchRequestID;
            _Format = (Format is NullParameter) ? "json" : (String)Format;
            _Action = (Action is NullParameter) ? "s" : (String)Action;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            SubscriptionOutput subsOutput = new SubscriptionOutput();

            try
            {
                if (!_HubSpotID.HasValue || !_SearchRequestID.HasValue)
                {
                    throw new ArgumentException("Invalid or missing input.");
                }

                ValidationLogic valLgc = (ValidationLogic)LogicFactory.GetLogic(LogicType.Validation);

                if (valLgc.ValidateSubscriptionInput(_HubSpotID, _SearchRequestID, _Action))
                {
                    SubscriptionLogic subsLgc = (SubscriptionLogic)LogicFactory.GetLogic(LogicType.Subscription);

                    var subsURL = subsLgc.GetEncryptedSubscriptionURL(ConfigurationManager.AppSettings["SubscriptionURL"], _HubSpotID.Value, _SearchRequestID.Value, _Action);
                    subsOutput.SubscriptionURL =subsURL;
                    subsOutput.Message = "Success";
                    subsOutput.Status = 0;
                }
                else
                {
                    subsOutput.Status = 1;
                    subsOutput.Message = "Invalid Input.";
                }

            }
            catch (ArgumentException ex)
            {
                subsOutput.Status = 1;
                subsOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                subsOutput.Status = 1;
                subsOutput.Message = "An error occurred, please try again.";
            }

            if (string.Compare(_Format,"xml",true)==0)
            {
                HttpResponse.Output.Write(Serializers.Serializer.SerializeToXml(subsOutput));
            }
            else
            {
                HttpResponse.Output.Write(Serializers.Serializer.Serialize(subsOutput));
            }
        }
    }
}