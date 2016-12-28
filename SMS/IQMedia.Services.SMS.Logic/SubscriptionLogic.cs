using System;
using IQMediaGroup.Common.Util;
namespace IQMedia.Services.SMS.Logic
{
    public class SubscriptionLogic : ILogic
    {
        public string GetEncryptedSubscriptionURL(string p_baseURL, Int64 p_hubSpotID, Int64 p_searchRequestID, string p_action)
        {
            var encryptedID = CommonFunctions.EncryptStringAES(p_searchRequestID + "&" + p_hubSpotID, "pF6tvq4GexXSUaGFKXGqaFmiG2X6ihG3joVZ8RiSwpk=", "g3tqe+8V4H/JwMe0X69TGw==");

            return p_baseURL+"?ID="+System.Web.HttpUtility.UrlEncode(encryptedID)+"&Action="+p_action;
        }
    }
}
