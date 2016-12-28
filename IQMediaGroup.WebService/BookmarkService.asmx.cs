using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net.Mail;
using System.Configuration;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.WebService
{
    /// <summary>
    /// Summary description for BookmarkService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BookmarkService : System.Web.Services.WebService
    {
                
        [WebMethod]
        public string BookmarkLink(string From, string To, string Subject, string Body, string URL, string FileName, string _imagePath, string FileID, string PageName)
        {
            try
            {

                string Name = HttpContext.Current.Request.Url.ToString();

                string[] _DomainName = Name.Split("/".ToCharArray());

                string _FinalString = _DomainName[0] + "//" + _DomainName[2];

                string _BookmarkURL = _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + FileID + "&TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(From, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"]));

                return _BookmarkURL;

            }
            catch (Exception ex)
            {
                return "Error:" + ex.Message;
            }

        }
    }
}
