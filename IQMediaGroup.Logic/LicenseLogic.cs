using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using IQMediaGroup.Common.Util;
using System.Configuration;
using System.Web;

namespace IQMediaGroup.Logic
{
    public class LicenseLogic : BaseLogic, ILogic
    {
        private Dictionary<string,object> GetIQLicenseDetailByClientGUID(Guid p_ClientGUID)
        {
            try
            {
                Dictionary<string,object> licenseDetail= Context.GetIQLicenseDetailByClientGUID(p_ClientGUID);

                IEnumerable<short> licenseEnumerable = Convert.ToString(licenseDetail["IQLicense"]).Split(',').Select(Int16.Parse).AsEnumerable();
                licenseDetail["IQLicense"] = licenseEnumerable;

                return licenseDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQLicenseUrlOutput GetLicenseUrl(IQLicenseUrlInput p_IQLicenseUrlInput)
        {
            try
            {
                Dictionary<string, object> licenseDetail = GetIQLicenseDetailByClientGUID(p_IQLicenseUrlInput.ClientGUID);
                IEnumerable<short> licenseEnumerable = (IEnumerable<short>)licenseDetail["IQLicense"];

                IQLicenseUrlOutput iQlicenseUrlOutput = new IQLicenseUrlOutput();
                iQlicenseUrlOutput.ArticleList = p_IQLicenseUrlInput.ArticleDetails;

                foreach (var article in iQlicenseUrlOutput.ArticleList)
                {
                    if (article.IQLicense > 0 && licenseEnumerable.Contains(article.IQLicense))
                    {
                        //article.Url = ConfigurationManager.AppSettings["LicenseBasicUrl"] + HttpContext.Current.Server.UrlEncode(CommonFunctions.EncryptLicenseStringAES(string.Format("{0}¶{1}¶{2}&u1=cliq40&u2={3}¶{4}", p_IQLicenseUrlInput.ClientGUID, p_IQLicenseUrlInput.EventName, p_IQLicenseUrlInput.InputUrl, p_ClientID, p_IQLicenseUrlInput.IQLicense)));
                        article.Url = ConfigurationManager.AppSettings["LicenseBasicUrl"] + HttpContext.Current.Server.UrlEncode(CommonFunctions.EncryptLicenseStringAES(licenseDetail["CustomerID"] + "¶" + p_IQLicenseUrlInput.EventName + "¶" + article.Url + "&u1=cliq40&u2=" + licenseDetail["ClientID"] + "¶"+article.IQLicense));
                    }
                    else
                    {
                        article.Url = null;
                    }
                }

                //iQLicenseUrlOutput.Url = ConfigurationManager.AppSettings["LicenseBasicUrl"] + HttpContext.Current.Server.UrlEncode(CommonFunctions.EncryptLicenseStringAES(string.Format("{0}¶{1}¶{2}&u1=cliq40&u2={3}¶{4}", p_IQLicenseUrlInput.ClientGUID, p_IQLicenseUrlInput.EventName, p_IQLicenseUrlInput.InputUrl, p_ClientID, p_IQLicenseUrlInput.IQLicense)));
                iQlicenseUrlOutput.Message ="Success";
                iQlicenseUrlOutput.Status = 0;
                return iQlicenseUrlOutput;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
