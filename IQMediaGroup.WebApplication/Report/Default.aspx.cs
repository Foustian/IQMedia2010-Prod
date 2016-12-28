using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Reports.Controller.Factory;
using System.Text;


namespace IQMediaGroup.WebApplication.Report
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    if (Request.QueryString["source"] != null && Request.QueryString["source"] == "email")
        //    {
        //        writer.Write(emailString);
        //    }           
        //}


        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                ScriptManager objScriptManager = (ScriptManager)this.Page.FindControl("SM1");
                objScriptManager.AsyncPostBackTimeout = 36000;
                lblReportErrorMsg.Text = string.Empty;
                if (!Page.IsPostBack)
                {
                    if (Request.QueryString["ID"] != null)
                    {
                        UTF8Encoding encoding = new UTF8Encoding();

                        IIQ_ReportController _IIQ_ReportController = _ControllerFactory.CreateObject<IIQ_ReportController>();
                        //IQ_Report _IQ_Report = _IIQ_ReportController.GetMentionReportXmlByMentionID(MentionID);
                        IQ_Report _IQ_Report = _IIQ_ReportController.GetReportXmlByReportGUID(new Guid(Request.QueryString["ID"]));
                        if (_IQ_Report != null)
                        {
                            /*switch (_IQ_Report._ReportTypeID)
                            {
                                case 1:*/
                            //IQMediaGroup.Reports.Usercontrol.IQMediaMaster.Report.Report Report1 =
                            //        (IQMediaGroup.Reports.Usercontrol.IQMediaMaster.Report.Report)LoadControl("~/UserControl/IQMediaMaster/Report/Report.ascx");

                            Report1.FetchReportXml(_IQ_Report.ReportRule, _IQ_Report.ClientGuid, _IQ_Report.ReportType, _IQ_Report.Identity, Request.QueryString["ID"]);
                            string emailString = Report1.GenerateEmailHTML();



                            if (Request.QueryString["source"] != null && Request.QueryString["source"].ToLower() == "email")
                            {
                                Response.Clear();
                                Response.Write(emailString);
                                Response.Flush();
                                Response.End();
                            }
                            else
                            {
                                Report1.Visible = true;
                            }


                            /*break;
                    }*/
                        }
                        else
                        {
                            lblReportErrorMsg.Text = "Invalid Request ID";
                        }
                    }
                    else
                    {
                        lblReportErrorMsg.Text = "You are not authorized to view this page";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }
    }
}