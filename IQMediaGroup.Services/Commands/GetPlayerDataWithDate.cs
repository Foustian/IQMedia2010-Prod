using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain;
using IQMediaGroup.Logic;
using System.Security.Authentication;
using IQMediaGroup.Services.Serializers;
using IQMedia.Web.Common;
using System.Configuration;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Util;


namespace IQMediaGroup.Services.Commands
{
    public class GetPlayerDataWithDate : ICommand
    {
        public Guid? _ID { get; private set; }
        public string _Type { get; private set; }
        private String _EncryptedID;
        public DateTime? _Date { get; private set; }
        public GetPlayerDataWithDate(object ID, object Type,object Date)
        {

            if (ID is NullParameter)
            {
                _ID = null;
            }
            else
            {
                Guid originalGuid = Guid.Empty;
                if (Guid.TryParse(Convert.ToString(ID), out(originalGuid)))
                {
                    _ID = originalGuid;
                }
                else
                {
                    _EncryptedID = Convert.ToString(ID);
                }
            }


            _Type = (Type is NullParameter) ? null : (string)Type;
            _Date = (Date is NullParameter) ? null : (DateTime?)Date;         
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _XMLResult = string.Empty;
            
            try
            {
                CLogger.Debug("Get Player Data Request Started");
                /*if (!Authentication.IsAuthenticated)
                    throw new AuthenticationException();*/

                if (_ID == null && string.IsNullOrWhiteSpace(_EncryptedID))
                {
                    throw new ArgumentException("Invalid or missing MediaID");
                }

                if (string.IsNullOrEmpty(_Type))
                {
                    throw new ArgumentException("Invalid or missing Type");
                }

                if (!string.IsNullOrWhiteSpace(_EncryptedID))
                {
                    _ID = new Guid(IQMediaGroup.Common.Util.CommonFunctions.AesDecryptStringFromBytes(_EncryptedID));
                }

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    CLogger.Debug("{\"Type\":\"" + _Type + "\",\"GUID\":\"" + _ID + "\"}");
                }

                PlayerDataLogic PlayerDataLogic = (PlayerDataLogic)LogicFactory.GetLogic(LogicType.PlayerData);
                string _PmgUrl = BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.TV.ToString(),_Date);
                dynamic Result = PlayerDataLogic.GetPlayerData(_ID, _Type,_PmgUrl);

                

                if (Result != null)
                {
                    _XMLResult = Serializer.SerializeToXml(Result);
                }
                else
                {
                    _XMLResult = Serializer.SerializeToXml("No Results Found");
                }
                
            }
            catch (ArgumentException ArgumentException)
            {
                _XMLResult = Serializer.SerializeToXml(ArgumentException.Message);
            }
            catch (AuthenticationException)
            {
                _XMLResult = Serializer.SerializeToXml("User is not authenticated.");
            }
            catch (CustomException _CustomException)
            {
                _XMLResult = Serializer.Searialize(_CustomException.Message);
            }
            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + "\n Inner Exception" + ex.InnerException + "\n stack : " + ex.StackTrace);
                _XMLResult = Serializer.SerializeToXml("An error occurred, please try again");
            }


            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "text/xml";

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug(_XMLResult);
            }
            CLogger.Debug("Get Player Data Request Ended");
            HttpResponse.Output.Write(_XMLResult);
        }
    }
}