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
    /// Summary description for email
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")] 
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)] 
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EmailService : System.Web.Services.WebService
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        [WebMethod]
        /*public string HelloWorld()
        {
            return "Hello World";
        }*/
        public string SendEmail(string From, string To, string Subject, string Body, string URL, string FileName, string _imagePath,string FileID,string PageName)
        {
            try
            {
                
                string EmailContent = string.Empty;
                string ServiceType = string.Empty;
                 
                string[] mailAddresses = To.Split(';');
                foreach (string mailAddress in mailAddresses)
                {
                    if (string.IsNullOrEmpty(_imagePath))
                    {
                        ServiceType = "myIQ";
                        IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                        EmailContent = _IArchiveClipController.EmailContent(FileID,mailAddress,ServiceType);
                    }
                    else
                    {
                        if (PageName == "iqBasic")
                        {
                            ServiceType = "iqBasic";
                            IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                            EmailContent = _IArchiveClipController.EmailContent(URL, FileName, _imagePath, FileID, mailAddress, ServiceType);
                        }
                        else
                        {
                            ServiceType = "ClipPlayer";
                            IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                            EmailContent = _IArchiveClipController.EmailContent(URL, FileName, _imagePath, FileID,mailAddress,ServiceType);
                        }
                    }

                    string WholeEmailBody = IQMediaGroup.Core.HelperClasses.CommonFunctions.EmailSend(From, mailAddress, Subject, Body, EmailContent);

                    MailLog(WholeEmailBody, From, mailAddress, ServiceType);
                   

                }

                return "Success!!";

            }
            catch (Exception ex)
            {
                return "Email Sending Failed!!, Error:"+ex.Message;
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
                IOutboundReportingController _IOutboundReportingController = _ControllerFactory.CreateObject<IOutboundReportingController>();
                OutboundReporting _OutboundReporting = new OutboundReporting();
                _OutboundReporting.Query_Name = "";
                _OutboundReporting.FromEmailAddress = From;
                _OutboundReporting.ToEmailAddress = To;
                _OutboundReporting.MailContent = _FileContent;
                _OutboundReporting.ServiceType = ServiceType;
                _Result = _IOutboundReportingController.InsertOutboundReportingLog(_OutboundReporting);
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }

        }

        //public void SendEmail()
        //{
        //    try
        //    {

        //        SmtpClient _SmtpClient = new SmtpClient();
        //        string MailUserName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServerUser"));
        //        string MailPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServerPassword"));

        //        NetworkCredential _NetworkCredential = new NetworkCredential(MailUserName, MailPassword);
        //        _SmtpClient.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("sSMTPPort"));
        //        _SmtpClient.Credentials = _NetworkCredential;
        //        _SmtpClient.Host = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServer"));

        //        MailMessage _MailMessage = new MailMessage();
        //        _MailMessage.From = new MailAddress(txtYourEmail.Text);

        //        string toEmailAddresses = txtFriendsEmail.Text;

        //        string[] mailAddresses = toEmailAddresses.Split(";".ToCharArray());
        //        string IncorrectMessages = string.Empty;

        //        if (mailAddresses.Length > 0)
        //        {

        //            foreach (string mailAddress in mailAddresses)
        //            {
        //                if (mailAddress.Length != 0)
        //                {
        //                    if (validateEmails(mailAddress))
        //                    {
        //                        _MailMessage.To.Add(new MailAddress(mailAddress));
        //                    }
        //                    else
        //                    {
        //                        if (!string.IsNullOrEmpty(IncorrectMessages))
        //                        {
        //                            IncorrectMessages = IncorrectMessages + "," + mailAddress;
        //                        }
        //                        else
        //                        {
        //                            IncorrectMessages = mailAddress;
        //                        }

        //                    }
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(IncorrectMessages))
        //            {
        //                lblError.Text = "Following email address invalid " + IncorrectMessages;
        //            }
        //        }
        //        else
        //        {
        //            lblError.Text = "Please enter valid email address";
        //        }

        //        //_MailMessage.To.Add(new MailAddress(txtFriendsEmail.Text));
        //        string strEmailBody = string.Empty;
        //        ViewstateInformation _ViewstateInformation = GetViewstateInformation();
        //        strEmailBody += "<html>";
        //        strEmailBody += "<body style=\"font-family:Verdana;font-size:11px;\">";
        //        if (txtMessage.Text.Trim() == string.Empty)
        //        {
        //            strEmailBody += "Hi," + "<br />";
        //            strEmailBody += "Please Check out following IQMedia Clip(s) <br />";
        //            if (!string.IsNullOrEmpty(_ViewstateInformation.MailMessage))
        //            {
        //                strEmailBody += _ViewstateInformation.MailMessage + "<br />";
        //            }
        //            strEmailBody += "Thanks,<br /> IQMedia Corp <br /> www.iqmediacorp.com";
        //        }
        //        else
        //        {
        //            strEmailBody += txtMessage.Text.Replace("\r\n", "<br />") + _ViewstateInformation.MailMessage + "<br />";
        //            strEmailBody += "Thanks,<br /> IQMedia Corp <br /> www.iqmediacorp.com";
        //        }
        //        strEmailBody += "</body>";
        //        strEmailBody += "</html>";

        //        _MailMessage.Body = strEmailBody;
        //        _MailMessage.Subject = txtSubject.Text;
        //        _MailMessage.IsBodyHtml = true;

        //        if (_MailMessage.To.Count > 0)
        //        {
        //            _SmtpClient.Send(_MailMessage);
        //            MailLog(strEmailBody);
        //        }

        //        if (!string.IsNullOrEmpty(IncorrectMessages))
        //        {
        //            mdlpopupEmail.Show();
        //            lblError.Text = "Following email address invalid " + IncorrectMessages;
        //            lblError.Visible = true;
        //        }

        //    }
        //    catch (Exception _Exception)
        //    {
        //        this.WriteException(_Exception);
        //        Response.Redirect(CommonConstants.CustomErrorPage);
        //    }
        //}

    }
}
