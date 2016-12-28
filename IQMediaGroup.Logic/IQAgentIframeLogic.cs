using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
using System.Web;
using IQMediaGroup.Common.Util;
using System.Data.Objects;
using IQMediaGroup.Domain;

namespace IQMediaGroup.Logic
{
    public class IQAgentIframeLogic : BaseLogic, ILogic
    {
        public string GetIQAgentIFrameLink(Guid? rawMediaID, DateTime? date)//  string plainText, byte[] Key, byte[] IV)
        {
            try
            {


                string plainText = rawMediaID + "&" + date;
                byte[] encodedByte = CommonFunctions.AesEncryptStringToBytes(plainText);
                string _IQAgentIframeURL = ConfigurationManager.AppSettings["IQAgentIframeUrl"] + HttpContext.Current.Server.UrlEncode(Convert.ToBase64String(encodedByte));

                return _IQAgentIframeURL;


            }
            catch (Exception exception)
            {

                throw exception;
            }


        }

        public IQAgentIframeOutput InsertIQAgentIframe(Guid? rawMediaGuid, DateTime? expiryDate, Int64? iQAgentResultID, string dataModelType)
        {
            try
            {
                IQAgentIframe iQAgentIframe = Context.InsertIQAgentIframe(rawMediaGuid, expiryDate, iQAgentResultID, dataModelType).FirstOrDefault();
                var iQAgentIframeOutput = new IQAgentIframeOutput();
                if (iQAgentIframe != null)
                {

                    if (iQAgentIframe.Status)
                    {
                        iQAgentIframeOutput.Message = "Fail - Video file is invalid";
                        iQAgentIframeOutput.Status = 1;

                    }
                    else
                    {

                        byte[] encodedByte = CommonFunctions.AesEncryptStringToBytes(Convert.ToString(iQAgentIframe.IQAgentIframeID));
                        string iQAgentIfameURL = HttpContext.Current.Server.UrlEncode(Convert.ToBase64String(encodedByte));
                        iQAgentIframeOutput.IQAgentFrameURL = iQAgentIfameURL;
                        iQAgentIframeOutput.Message = "Success";
                        iQAgentIframeOutput.Status = 0;
                    }

                }



                return iQAgentIframeOutput;
            }
            catch (Exception exception)
            {

                throw exception;
            }


        }

    }
}
