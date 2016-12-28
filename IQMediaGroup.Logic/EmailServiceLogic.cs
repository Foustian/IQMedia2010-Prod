using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using System.Data.Objects;
using System.Configuration;
using System.Web;
using System.IO;
using IQMediaGroup.Common.Util;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace IQMediaGroup.Logic
{
    public class EmailServiceLogic : BaseLogic, ILogic
    {


        public string SendEmail(EmaiInput _EmailInput)
        {
            
            try
            {

                string EmailContent = string.Empty;
                string ServiceType = string.Empty;

                string _EmailFromat = GetEmailFormat();

                string EmailImage = string.Empty;
                if (!string.IsNullOrEmpty(_EmailInput._imagePath) /* && File.Exists(ConfigurationManager.AppSettings["ImagePath"] + _EmailInput.FileID + ".jpg")*/)
                {
                    EmailImage = _EmailInput._imagePath;
                }
                else
                {
                    ArchiveClipService _ArchiveClipService = Context.GetArchiveClipDataAndImageContentByClipID(new Guid(_EmailInput.FileID)).Single();
                    if (!string.IsNullOrEmpty(_ArchiveClipService.ThumbnailImagePath))
                    {
                        EmailImage = _ArchiveClipService.ThumbnailImagePath;
                    }
                    else
                    {
                        EmailImage = ConfigurationManager.AppSettings["NoImageURL"];
                    }
                }

                string[] mailAddresses = _EmailInput.To.Split(';');
                foreach (string mailAddress in mailAddresses)
                {
                    ServiceType = _EmailInput.PageName;
                    EmailContent = string.Format(_EmailFromat, 
                                                ConfigurationSettings.AppSettings["ClipURL"], 
                                                HttpContext.Current.Server.UrlEncode(CommonFunctions.Encrypt(mailAddress, ConfigurationManager.AppSettings["EncryptionKey"])),
                                                EmailImage, 
                                                _EmailInput.FileName, 
                                                ConfigurationSettings.AppSettings["iOSURL"], 
                                                _EmailInput.FileID);
                                                //,ConfigurationSettings.AppSettings["IOSAppDownloadLocation"]);
                    string WholeEmailBody = CommonFunctions.EmailSend(_EmailInput.From, mailAddress, _EmailInput.Subject, _EmailInput.Body, EmailContent);
                    MailLog(WholeEmailBody, _EmailInput.From, mailAddress, ServiceType);
                }
                return "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void MailLog(string _EmailBody, string From, string To, string ServiceType)
        {
            try
            {
                string _FileContent = string.Empty;

                _FileContent = "<EmailLog>";
                _FileContent += "<EmailContent>" + _EmailBody + "</EmailContent>";
                _FileContent += "</EmailLog>";

                string _Result = string.Empty;

                ObjectParameter _objparameter = new ObjectParameter("OutboundReportingKey", typeof(Int64));
                ObjectResult _objresult = Context.InsertOutBoundReporting(string.Empty, From, To, _FileContent, ServiceType, _objparameter);
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }

        }

        public string GetEmailFormat()
        {
            try
            {

                string strEmailBody = string.Empty;

                strEmailBody += "<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\" style=\"font-family:Verdana;font-size:11px;\">";
                strEmailBody += "<tr>";
                strEmailBody += "<th>Image</th>";
                strEmailBody += "<th style=\"width:150px;\" align=\"center\">Title</th>";
                strEmailBody += "<th>Clip URL</th>";
                strEmailBody += "</tr>";

                strEmailBody += "<tr>";
                strEmailBody += "<td><a href=\"{0}{5}&amp;TE={1}&amp;\">" + "<img src=\"{2}\" id=\"imgClip\" height=\"100\" width=\"100\" /></a></td>";
                strEmailBody += "<td style=\"width:150px;\" align=\"center\">{3}</td>";
                strEmailBody += "<td><a href=\"{0}{5}&amp;TE={1}&amp;\">{0}{5}&amp;TE={1}&amp;</a></td>";
                strEmailBody += "</tr>";
                //added for iPad/iPhone Link
                //strEmailBody += "<tr><td colspan=\"1\"><a href=\"{5}ClipID={7}&amp;BaseUrl={6}\">Click here for iPad/iPhone</a></td></tr>";
                //strEmailBody += "<tr><td colspan=\"1\"><a href=\"{7}\" target=\"_blank\" >Click here to install IQMedia application</a></td></tr>";
                strEmailBody += "</table>";

                return strEmailBody;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

       
    }
}
