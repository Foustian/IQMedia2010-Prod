using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Reports.Usercontrol.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Reports.Controller.Factory;
using System.Text;
using System.Threading;
using System.IO;
using IQMediaGroup.Reports.Controller.Interface;
using System.Configuration;
using System.Text.RegularExpressions;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Reports.Controller.Common;
using System.Web.UI.HtmlControls;

namespace IQMediaGroup.Reports.Usercontrol.IQMediaMaster.MyIQReport
{
    public partial class MyIQReport : BaseControl
    {
        SessionInformation _SessionInformation;
        ViewstateInformation _ViewstateInformation;
        List<Caption> _ListOfRCatpion = null;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        decimal iqAdSharevalueTV = 0;
        decimal iqAdSharevalueNM = 0;
        decimal iqAdSharevalueSM = 0;

        Int32 audienceTV = 0;
        Int32 audienceNM = 0;
        Int32 audienceSM = 0;


        protected override void OnLoad(EventArgs e)
        {
            _SessionInformation = CommonFunctions.GetSessionInformation();
            _ViewstateInformation = GetViewstateInformation();

            if (!Page.IsPostBack)
            {

                txtFromDate.Text = DateTime.Now.ToShortDateString();
                txtToDate.Text = DateTime.Now.ToShortDateString();
                if (!_SessionInformation.IsmyiQNM)
                {
                    chkReportNews.Visible = false;
                    spnOnlineNews.Visible = false;
                }

                if (!_SessionInformation.IsmyiQSM)
                {
                    chkReportSM.Visible = false;
                    spnSocialMedia.Visible = false;
                }

                if (!_SessionInformation.IsMyIQTwitter)
                {
                    chkReportTwitter.Visible = false;
                    spnTwitter.Visible = false;
                }

                btnReport.Visible = false;
                BindReportType();

                divReportResult.Visible = false;
                divTVReport.Visible = false;
                divNewsReport.Visible = false;
                divSocialMediaReport.Visible = false;
                divTwitterReport.Visible = false;

                GetClientCompeteRights();
                GetCustomCategoryByClientGUID();
            }

            if (Request.UserAgent.ToLower().Contains("android") && CheckVersion())
            {
                ScriptManager.RegisterClientScriptInclude(this.Page, Page.GetType(), "script5", this.Page.ResolveClientUrl("~/Script/") + "AndroidPlayer.js");
            }

            if (Request["__EventTarget"] != null && Request["__EventTarget"] == upMainGrid.ClientID && Request["__EventArgument"] != null)
            {
                PlayClip(Request["__EventArgument"]);
            }

            ScriptManager.RegisterStartupScript(upBtnReport, upBtnReport.GetType(), "ReportDocReady", "ReportDocReady();", true);
            chkReportSelectAll.Attributes.Add("onclick", "CheckUncheckAll('divReportSelection',this.id);");
            lblReportErrorMsg.Text = string.Empty;
            lblReportSuccessMsg.Text = string.Empty;


        }

        #region Report Events

        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                divReportResult.Visible = false;

                if (ValidateReportSearch())
                {
                    SetReportParams();

                    btnReportEmail.Visible = true;
                    btnReportCsvDownload.Visible = true;
                    btnReportPdfDownload.Visible = true;
                    divReportHeader.Visible = true;

                    if (_SessionInformation.ISCustomeHeader)
                    {
                        imgReportClientLogo.Src = "http://" + Request.Url.Host.ToString() + "/Images/CustomHeader/" + _SessionInformation.CustomeHeaderImage;
                    }


                    if (ddlCategory.SelectedIndex > 0)
                    {
                        lblReportHeader.Text = "Daily Report - " + ddlCategory.SelectedItem.Text + " - " + txtFromDate.Text + " To " + txtToDate.Text;
                    }
                    else
                    {
                        lblReportHeader.Text = "Daily Report - " + txtFromDate.Text + " To " + txtToDate.Text;
                    }


                    if (chkReportTV.Checked)
                    {
                        divTVReport.Visible = true;
                        BindReportClips();
                    }
                    else
                    {
                        divTVReport.Visible = false;
                    }

                    if (chkReportNews.Checked && _SessionInformation.IsmyiQNM)
                    {
                        divNewsReport.Visible = true;
                        BindReportNews();
                    }
                    else
                    {
                        divNewsReport.Visible = false;
                    }

                    if (chkReportSM.Checked && _SessionInformation.IsmyiQSM)
                    {
                        divSocialMediaReport.Visible = true;
                        BindReportSocialMedia();
                    }
                    else
                    {
                        divSocialMediaReport.Visible = false;
                    }

                    if (chkReportTwitter.Checked && _SessionInformation.IsMyIQTwitter)
                    {
                        divTwitterReport.Visible = true;
                        BindReportTwitter();
                    }
                    else
                    {
                        divTwitterReport.Visible = false;
                    }

                    StringBuilder sbSummary = new StringBuilder();

                    Int64 audienceTotal = 0;
                    decimal iqAdsharevalueTotal = 0;

                    if (_ViewstateInformation.IsNielSenData)
                    {
                        audienceTotal = string.IsNullOrWhiteSpace(Convert.ToString(audienceTV)) ? 0 : audienceTV;
                        iqAdsharevalueTotal = string.IsNullOrWhiteSpace(Convert.ToString(iqAdSharevalueTV)) ? 0 : iqAdSharevalueTV;
                    }


                    if (_ViewstateInformation.IsCompeteData)
                    {
                        audienceTotal = audienceTotal + (string.IsNullOrWhiteSpace(Convert.ToString(audienceNM)) ? 0 : audienceNM);
                        iqAdsharevalueTotal = iqAdsharevalueTotal + (string.IsNullOrWhiteSpace(Convert.ToString(iqAdSharevalueNM)) ? 0 : iqAdSharevalueNM);


                        audienceTotal = audienceTotal + (string.IsNullOrWhiteSpace(Convert.ToString(audienceSM)) ? 0 : audienceSM);
                        iqAdsharevalueTotal = iqAdsharevalueTotal + (string.IsNullOrWhiteSpace(Convert.ToString(iqAdSharevalueSM)) ? 0 : iqAdSharevalueSM);
                    }



                    if (_ViewstateInformation.IsNielSenData || _ViewstateInformation.IsCompeteData)
                    {
                        sbSummary.AppendFormat("<table style=\"margin:15px 0px 0 0;width:100%;\" class=\"grid grid-iq\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">");

                        sbSummary.Append("<tr>");
                        /*sbSummary.Append("<td style=\"width:12%;\" class=\"right\"></td>");
                        sbSummary.Append("<td style=\"width:6%;\" class=\"right\">&nbsp;</td>");
                        sbSummary.Append("<td style=\"width:20%;\" class=\"right\">&nbsp;</td>");*/
                        sbSummary.Append("<td style=\"width:78%;\" class=\"right\">Grand Total : </td>");
                        sbSummary.AppendFormat("<td style=\"width:8%;\" colspan=\"3\" class=\"right\">{0}</td>", string.Format("{0:n0}", audienceTotal));
                        sbSummary.AppendFormat("<td style=\"width:14%;\" class=\"right\">{0}</td>", string.Format("{0:C}", iqAdsharevalueTotal));
                        sbSummary.Append("</tr>");
                        sbSummary.AppendFormat("</table>");
                        divSummary.InnerHtml = Convert.ToString(sbSummary);
                    }
                    divReportResult.Visible = true;
                    //upMainGrid.Update();

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowReport", "$('#divReport').show();", true);
                }

                upMainGrid.Update();

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblReportErr.Text = CommonConstants.CommonErrMsg;//"An error occured, please try again!";
            }
        }

        protected void btnReportCsvDownload_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder _DownloarStr = new StringBuilder();

                _DownloarStr.AppendFormat("DAILY REPORT - {0}", Convert.ToString(_ViewstateInformation._MyIQReportParams.FromDate).Replace("/", "-") + "_To_" + Convert.ToString(_ViewstateInformation._MyIQReportParams.ToDate).Replace("/", "-"));
                _DownloarStr.Append("\r\n");

                if (chkReportTV.Checked)
                {
                    _DownloarStr.Append(GetTVReportCsv());
                }

                if (chkReportNews.Checked && _SessionInformation.IsmyiQNM)
                {
                    _DownloarStr.Append(GetNewsReportCsv());
                }

                if (chkReportSM.Checked && _SessionInformation.IsmyiQSM)
                {
                    _DownloarStr.Append(GetSocialMediaReportCsv());
                }

                if (chkReportTwitter.Checked && _SessionInformation.IsMyIQTwitter)
                {
                    _DownloarStr.Append(GetReportTwitterCsv());
                }

                if (_ViewstateInformation.IsNielSenData || _ViewstateInformation.IsCompeteData)
                {
                    Int64 audienceTotal = 0;
                    decimal iqAdsharevalueTotal = 0;

                    if (_ViewstateInformation.IsNielSenData)
                    {
                        audienceTotal = string.IsNullOrWhiteSpace(Convert.ToString(audienceTV)) ? 0 : audienceTV;
                        iqAdsharevalueTotal = string.IsNullOrWhiteSpace(Convert.ToString(iqAdSharevalueTV)) ? 0 : iqAdSharevalueTV;
                    }


                    if (_ViewstateInformation.IsCompeteData)
                    {
                        audienceTotal = audienceTotal + (string.IsNullOrWhiteSpace(Convert.ToString(audienceNM)) ? 0 : audienceNM);
                        iqAdsharevalueTotal = iqAdsharevalueTotal + (string.IsNullOrWhiteSpace(Convert.ToString(iqAdSharevalueNM)) ? 0 : iqAdSharevalueNM);


                        audienceTotal = audienceTotal + (string.IsNullOrWhiteSpace(Convert.ToString(audienceSM)) ? 0 : audienceSM);
                        iqAdsharevalueTotal = iqAdsharevalueTotal + (string.IsNullOrWhiteSpace(Convert.ToString(iqAdSharevalueSM)) ? 0 : iqAdSharevalueSM);
                    }


                    _DownloarStr.AppendFormat("\"{0}\",", "");
                    _DownloarStr.AppendFormat("\"{0}\",", "");
                    _DownloarStr.AppendFormat("\"{0}\",", "");
                    _DownloarStr.AppendFormat("\"{0}\",", "Grand Total");
                    _DownloarStr.AppendFormat("\"{0}\",", string.Format("{0:n0}", audienceTotal));
                    _DownloarStr.AppendFormat("\"{0}\",", string.Format("{0:C}", iqAdsharevalueTotal));

                }


                Response.ClearContent();

                // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Convert.ToDateTime(_ViewstateInformation._MyIQReportParams.FromDate).ToShortDateString().Replace("/", "-") + "_To_" + Convert.ToDateTime(_ViewstateInformation._MyIQReportParams.ToDate).ToShortDateString().Replace("/", "-") + "-Report.csv" + "\"");

                // Set the ContentType
                Response.ContentType = "text/csv";

                Response.Write(_DownloarStr.ToString());

                // End the response
                Response.End();

            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg; //"An error occured, please try again!";
            }
        }

        protected void btnReportPdfDownload_Click(object sender, EventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<head>");
                sb.Append("<link href=\"../../Css/my-style.css\" rel=\"stylesheet\" type=\"text/css\" />");
                sb.Append("</head>");
                sb.Append("<body>");

                if (_SessionInformation.ISCustomeHeader)
                {
                    sb.Append("<div><img src=\"http://" + Request.Url.Host.ToString() + "/Images/CustomHeader/" + _SessionInformation.CustomeHeaderImage + "\" alt=\"\" /></div>");                    
                    sb.Append("\r\n");
                }

                //divClientLogo.RenderControl(new HtmlTextWriter(new StringWriter(sb)));

                divReportHeader.RenderControl(new HtmlTextWriter(new StringWriter(sb)));

                if (chkReportTV.Checked)
                {
                    //divTVReport.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                    sb.Append(GetTVReportHtml(true));
                }

                if (chkReportNews.Checked && _SessionInformation.IsmyiQNM)
                {
                    //divNewsReport.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                    sb.Append(GetNewsReportHtml(true));
                }

                if (chkReportSM.Checked && _SessionInformation.IsmyiQSM)
                {
                    //divSocialMediaReport.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                    sb.Append(GetSocialMediaReportHtml(true));
                }

                if (chkReportTwitter.Checked && _SessionInformation.IsMyIQTwitter)
                {
                    //divSocialMediaReport.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                    sb.Append(GetReportTwitterHtml());
                }

                divSummary.RenderControl(new HtmlTextWriter(new StringWriter(sb)));

                sb.Append("</body>");
                sb.Append("</html>");
                string InputFile = Server.MapPath("../Pdf/Report/") + Guid.NewGuid() + ".html";
                string OutputFile = Server.MapPath("../Pdf/Report/") + Guid.NewGuid() + ".pdf";

                FileInfo infile = new FileInfo(InputFile);

                using (Stream stream = infile.OpenWrite())
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(sb.ToString());
                }

                if (CommonFunctions.GeneratePdf(InputFile, OutputFile))
                {
                    //infile.Delete();

                    Response.ClearContent();

                    // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Convert.ToDateTime(_ViewstateInformation._MyIQReportParams.FromDate).ToShortDateString().Replace("/", "-") + "_To_" + Convert.ToDateTime(_ViewstateInformation._MyIQReportParams.ToDate).ToShortDateString().Replace("/", "-") + "-Report.pdf" + "\"");

                    // Set the ContentType
                    Response.ContentType = "Application/pdf";

                    // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                    Response.WriteFile(OutputFile);

                    // End the response
                    Response.End();
                }
                else
                {
                    lblReportErrorMsg.Text = "Some Error Occured, Please Try Again!!";
                }

            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg; //"An error occured, please try again!";
            }
        }

        #region Bind Report TV Grid

        private void BindReportClips()
        {
            try
            {

                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                /*List<ArchiveClip> _ListOfArchilveClipCategoryGroup = _IArchiveClipController.GetArchiveClipReportGroupByCategory(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._MyIQReportParams.ReportDate.Value);

                List<ArchiveClip> _ListOfArchilveClipResult = new List<ArchiveClip>();

                foreach (ArchiveClip _ArchiveClip in _ListOfArchilveClipCategoryGroup)
                {
                    if (_ArchiveClip.CategoryGUID.HasValue)
                    {
                        List<ArchiveClip> _ListOfArchilveClip = _IArchiveClipController.GetArchiveClipByCategoryGuidAndDate(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation.TVReportSortExpression, _ViewstateInformation.IsTVReportSortDirecitonAsc, _ViewstateInformation._MyIQReportParams.ReportDate.Value, _ArchiveClip.CategoryGUID.Value, _ViewstateInformation.IsNielSenData);
                        foreach (ArchiveClip _ArchiveClipResult in _ListOfArchilveClip)
                        {
                            _ArchiveClipResult.CategoryName = _ArchiveClip.CategoryName;
                            _ArchiveClipResult.CategoryGUID = _ArchiveClip.CategoryGUID;
                        }
                        _ListOfArchilveClipResult.AddRange(_ListOfArchilveClip);
                    }
                }*/

                List<ArchiveClip> _ListOfArchilveClipResult = _IArchiveClipController.GetArchiveClipByDurationNCategoryGuid(new Guid(_SessionInformation.ClientGUID),
                                                                                               _ViewstateInformation.SocialMediaReportSortExpression,
                                                                                               _ViewstateInformation.IsSocialMediaReportSortDirecitonAsc,
                                                                                               _ViewstateInformation._MyIQReportParams.FromDate.Value,
                                                                                               _ViewstateInformation._MyIQReportParams.ToDate.Value,
                                                                                               (ddlCategory.SelectedIndex > 0 ? new Guid(ddlCategory.SelectedValue) : Guid.Empty),
                                                                                               _ViewstateInformation.IsCompeteData);

                _ViewstateInformation.MyIQTVReportResult = CommonFunctions.SearializeJson(_ListOfArchilveClipResult);

                SetViewstateInformation(_ViewstateInformation);

                divTVReport.InnerHtml = GetTVReportHtml();

                if (_ListOfArchilveClipResult != null && _ListOfArchilveClipResult.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowTVRerpot", "$('#divReportTVResult').show();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTVRerpot", "$('#divReportTVResult').hide();", true);
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
                //Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        private string GetTVReportHtml(bool IsEmailOrDownload = false)
        {
            try
            {
                StringBuilder _TVReportBuilder = new StringBuilder();
                decimal iqAdsharevalueSub = 0;
                Int32 audienceSub = 0;

                audienceTV = 0;
                iqAdSharevalueTV = 0;

                List<ArchiveClip> _ListOfArchilveArchiveClipResult = new List<ArchiveClip>();
                _ListOfArchilveArchiveClipResult = (List<ArchiveClip>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTVReportResult, _ListOfArchilveArchiveClipResult.GetType());

                _TVReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportTVResult');\">");
                _TVReportBuilder.Append("TV");
                _TVReportBuilder.Append("</div>");
                _TVReportBuilder.Append("<div id=\"divReportTVResult\" class=\"clear\">");

                if (_ListOfArchilveArchiveClipResult.Count > 0)
                {

                    List<ArchiveClip> _ListOfArchilveClipSummery = (from a in _ListOfArchilveArchiveClipResult
                                                                    group a by a.CategoryGUID into b
                                                                    where b.Count() > 0
                                                                    select new ArchiveClip
                                                                    {
                                                                        Total = b.Count(),
                                                                        CategoryGUID = b.Key,
                                                                        CategoryName = b.Max(c => c.CategoryName)

                                                                    }).ToList();

                    _TVReportBuilder.Append("<div class=\"padding5  clear\">");
                    _TVReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"width:50%\" id=\"gvTVReportSummery\" class=\"grid grid-iq\">");
                    _TVReportBuilder.Append("<tr valign=\"top\" align=\"center\" style=\"height:10px;\" class=\"grid-th\">");
                    _TVReportBuilder.Append("<th style=\"height:20px;width:70%;\" scope=\"col\" class=\"grid-th-left\">Category</th>");
                    _TVReportBuilder.Append("<th style=\"height:20px;width:30%;\" scope=\"col\" class=\"grid-th-right\">Total</th>");
                    _TVReportBuilder.Append("</tr>");


                    foreach (ArchiveClip _SummeryItem in _ListOfArchilveClipSummery)
                    {
                        _TVReportBuilder.Append("<tr>");
                        _TVReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _SummeryItem.CategoryName);
                        _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", _SummeryItem.Total);
                        _TVReportBuilder.Append("</tr>");
                    }

                    _TVReportBuilder.Append("</table>");
                    _TVReportBuilder.Append("</div>");

                    foreach (ArchiveClip _SummeryItem in _ListOfArchilveClipSummery)
                    {
                        audienceSub = 0;
                        iqAdsharevalueSub = 0;

                        _TVReportBuilder.AppendFormat("<div onclick=\"ShowHideDiv('TVReuslt{0}')\" style=\"margin:15px 0px 3px 15px;\" class=\"width100p ulheader cursor clear\" id=\"TVHandle{0}\">{1}</div>", _SummeryItem.CategoryGUID, _SummeryItem.CategoryName);
                        _TVReportBuilder.AppendFormat("<div id=\"TVReuslt{0}\">", _SummeryItem.CategoryGUID);

                        List<ArchiveClip> _ListOfArchiveClipCategory = _ListOfArchilveArchiveClipResult.Where(a => a.CategoryGUID == _SummeryItem.CategoryGUID).ToList();

                        _TVReportBuilder.AppendFormat("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"max-width: 100%;border-collapse:collapse;word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;\" id=\"grvReportTV_{0}\" rules=\"all\" class=\"grid grid-iq\">", _SummeryItem.CategoryName);
                        _TVReportBuilder.Append("<tr>");
                        if (_ViewstateInformation.IsNielSenData)
                        {
                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:7%;\">Clip</th>");
                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:100px;\">Image</th>");
                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:21%;\">Title</th>");

                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:13%;\">Air Date</th>");
                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:13%;\">Audience</th>");
                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:20%;\">iQ media Value</th>");
                        }
                        else
                        {
                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:7%;\">Clip</th>");
                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:100px;\">Image</th>");
                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:21%;\">Title</th>");

                            _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:10%;\">Air Date</th>");
                        }
                        _TVReportBuilder.Append("</tr>");
                        foreach (ArchiveClip _ArchiveClip in _ListOfArchiveClipCategory)
                        {
                            _TVReportBuilder.Append("<tr>");

                            if (_SessionInformation != null && _SessionInformation.ClientGUID != null && !IsEmailOrDownload)
                            {
                                _TVReportBuilder.AppendFormat("<td class=\"center\"><a target=\"_blank\" style=\"border:0;cursor:pointer;\" onclick=\"PlayClip('{0}')\" >Play</a></td>", _ArchiveClip.ClipID);
                                _TVReportBuilder.Append("<td class=\"center\">");
                                _TVReportBuilder.AppendFormat("<a target=\"_blank\" style=\"border:0;cursor:pointer;\" onclick=\"PlayClip('{2}')\" ><img align=\"middle\" width=\"100\" height=\"100\" style=\"height:100px;width:100px;border-width:0px;\" alt=\"{0}\" src=\"{1}&amp;eid={2}\" /></a>", _ArchiveClip.ClipTitle, ConfigurationManager.AppSettings["ClipGetPreview"], _ArchiveClip.ClipID);
                                _TVReportBuilder.Append("</td>");
                            }
                            else
                            {
                                _TVReportBuilder.AppendFormat("<td class=\"center\"><a target=\"_blank\"  href=\"http://{0}/ClipPlayer/Default.aspx?ClipID={1}&amp;PN={2}\" style=\"border:0\" >View Clip</a></td>", Request.Url.Host, _ArchiveClip.ClipID, HttpContext.Current.Server.UrlEncode(CommonFunctions.Encrypt("myIQ", ConfigurationManager.AppSettings["EncryptionKey"])));
                                _TVReportBuilder.Append("<td class=\"center\">");
                                _TVReportBuilder.AppendFormat("<a target=\"_blank\" href=\"http://{3}/ClipPlayer/Default.aspx?ClipID={2}&amp;PN={4}\" style=\"border:0;\" ><img align=\"middle\" width=\"100\" height=\"100\" style=\"height:100px;width:100px;border-width:0px;\" alt=\"{0}\" src=\"{1}&amp;eid={2}\" /></a>", _ArchiveClip.ClipTitle, ConfigurationManager.AppSettings["ClipGetPreview"], _ArchiveClip.ClipID, Request.Url.Host, HttpContext.Current.Server.UrlEncode(CommonFunctions.Encrypt("myIQ", ConfigurationManager.AppSettings["EncryptionKey"])));
                                _TVReportBuilder.Append("</td>");
                            }


                            _TVReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _ArchiveClip.ClipTitle);

                            _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:MM/dd/yyyy hh:mm tt}", _ArchiveClip.ClipDate));
                            if (_ViewstateInformation.IsNielSenData)
                            {
                                _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.IsNullOrEmpty(_ArchiveClip.Audience) ? "NA" : _ArchiveClip.Audience);
                                _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.IsNullOrEmpty(_ArchiveClip.Sqad_ShareValue) ? "NA" : string.Format("{0:N}{1}", _ArchiveClip.Sqad_ShareValue, _ArchiveClip.IsActualNielsen ? "(A)" : "(E)"));

                                audienceSub = audienceSub + (string.IsNullOrEmpty(_ArchiveClip.Audience) ? 0 : Convert.ToInt32(_ArchiveClip.Audience));
                                iqAdsharevalueSub = iqAdsharevalueSub + (string.IsNullOrEmpty(_ArchiveClip.Sqad_ShareValue) ? 0 : Convert.ToDecimal(_ArchiveClip.Sqad_ShareValue));


                            }
                            _TVReportBuilder.Append("</tr>");
                        }

                        if (_ViewstateInformation.IsNielSenData)
                        {
                            _TVReportBuilder.Append("<tr>");
                            _TVReportBuilder.Append("<td class=\"right\" colspan=\"4\">Sub Total : </td>");
                            _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:n0}", audienceSub));
                            _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", iqAdsharevalueSub);

                            _TVReportBuilder.Append("</tr>");
                        }

                        _TVReportBuilder.Append("</table>");
                        _TVReportBuilder.Append("</div>");

                        audienceTV = audienceTV + audienceSub;
                        iqAdSharevalueTV = iqAdSharevalueTV + iqAdsharevalueSub;

                    }

                    if (_ViewstateInformation.IsNielSenData)
                    {
                        _TVReportBuilder.AppendFormat("<table style=\"margin:15px 0 0 0;width:100%;\" class=\"grid grid-iq\" cellpadding=\"5\" cellspacing=\"0\" border=\"1\">");

                        _TVReportBuilder.Append("<tr>");
                        /*_TVReportBuilder.Append("<td style=\"width:7%\">&nbsp;</td>");
                        _TVReportBuilder.Append("<td style=\"width:100px\">&nbsp;</td>");
                        _TVReportBuilder.Append("<td style=\"width:21%\">&nbsp;</td>");*/
                        _TVReportBuilder.Append("<td style=\"width:67%\" class=\"right\">Total : </td>");
                        _TVReportBuilder.AppendFormat("<td style=\"width:13%\" class=\"right\">{0}</td>", string.Format("{0:n0}", audienceTV));
                        _TVReportBuilder.AppendFormat("<td style=\"width:20%\" class=\"right\">{0}</td>", iqAdSharevalueTV);
                        _TVReportBuilder.Append("</tr>");
                        _TVReportBuilder.AppendFormat("</table>");
                    }


                }
                else
                {
                    _TVReportBuilder.Append("<div class=\"padding5\">");
                    _TVReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width:100%\" id=\"gvTVReportSummery\" class=\"grid grid-iq\">");
                    _TVReportBuilder.Append("<tr><td colspan=\"2\">No Results Found</td></tr>");
                    _TVReportBuilder.Append("</table>");
                    _TVReportBuilder.Append("</div>");
                }
                _TVReportBuilder.Append("</div>");
                return _TVReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
            return string.Empty;

        }

        private string GetTVReportCsv()
        {
            try
            {
                Int32 audienceSub = 0;
                Decimal iqAdsharevalueSub = 0;
                audienceTV = 0;
                iqAdSharevalueTV = 0;

                List<ArchiveClip> _ListOfArchilveArchiveClipResult = new List<ArchiveClip>();
                _ListOfArchilveArchiveClipResult = (List<ArchiveClip>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTVReportResult, _ListOfArchilveArchiveClipResult.GetType());

                StringBuilder _TVReportBuilder = new StringBuilder();
                _TVReportBuilder.Append("TV\r\n");

                if (_ListOfArchilveArchiveClipResult.Count > 0)
                {

                    List<ArchiveClip> _ListOfArchilveClipSummery = (from a in _ListOfArchilveArchiveClipResult
                                                                    group a by a.CategoryGUID into b
                                                                    where b.Count() > 0
                                                                    select new ArchiveClip
                                                                    {
                                                                        Total = b.Count(),
                                                                        CategoryGUID = b.Key,
                                                                        CategoryName = b.Max(c => c.CategoryName)

                                                                    }).ToList();

                    _TVReportBuilder.Append("Category,Total");
                    _TVReportBuilder.Append("\r\n");

                    foreach (ArchiveClip _SummeryItem in _ListOfArchilveClipSummery)
                    {
                        _TVReportBuilder.AppendFormat("\"{0}\",{1}", _SummeryItem.CategoryName, _SummeryItem.Total);
                        _TVReportBuilder.Append("\r\n");
                    }

                    _TVReportBuilder.Append("\r\n");
                    foreach (ArchiveClip _SummeryItem in _ListOfArchilveClipSummery)
                    {
                        audienceSub = 0;
                        iqAdsharevalueSub = 0;

                        _TVReportBuilder.AppendFormat("\"{0}\"", _SummeryItem.CategoryName);
                        _TVReportBuilder.Append("\r\n");

                        List<ArchiveClip> _ListOfArchiveClipCategory = _ListOfArchilveArchiveClipResult.Where(a => a.CategoryGUID == _SummeryItem.CategoryGUID).ToList();

                        if (_ViewstateInformation.IsNielSenData)
                        {
                            _TVReportBuilder.Append("Clip,Title,Locat Air Date,Audience,iQ media Value");
                        }
                        else
                        {
                            _TVReportBuilder.Append("Clip,Title,Locat Air Date");
                        }
                        _TVReportBuilder.Append("\r\n");

                        foreach (ArchiveClip _ArchiveClip in _ListOfArchiveClipCategory)
                        {
                            _TVReportBuilder.AppendFormat("http://{0}/ClipPlayer/Default.aspx?ClipID={1}&amp;PN={2},", Request.Url.Host, _ArchiveClip.ClipID, HttpContext.Current.Server.UrlEncode(CommonFunctions.Encrypt("myIQ", ConfigurationManager.AppSettings["EncryptionKey"])));
                            _TVReportBuilder.AppendFormat("\"{0}\",", _ArchiveClip.ClipTitle);
                            _TVReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:MM/dd/yyyy hh:mm tt}", _ArchiveClip.ClipDate));
                            if (_ViewstateInformation.IsNielSenData)
                            {
                                _TVReportBuilder.AppendFormat("\"{0}\",", string.IsNullOrEmpty(_ArchiveClip.Audience) ? "NA" : _ArchiveClip.Audience);
                                _TVReportBuilder.AppendFormat("\"{0}\"", string.IsNullOrEmpty(_ArchiveClip.Sqad_ShareValue) ? "NA" : string.Format("{0:N}{1}", _ArchiveClip.Sqad_ShareValue, _ArchiveClip.IsActualNielsen ? "(A)" : "(E)"));
                            }
                            _TVReportBuilder.Append("\r\n");

                            audienceSub = audienceSub + (string.IsNullOrEmpty(_ArchiveClip.Audience) ? 0 : Convert.ToInt32(_ArchiveClip.Audience));
                            iqAdsharevalueSub = iqAdsharevalueSub + (string.IsNullOrEmpty(_ArchiveClip.Sqad_ShareValue) ? 0 : Convert.ToDecimal(_ArchiveClip.Sqad_ShareValue));

                        }
                        if (_ViewstateInformation.IsNielSenData)
                        {
                            _TVReportBuilder.AppendFormat("\"{0}\",", "");
                            _TVReportBuilder.AppendFormat("\"{0}\",", "");
                            _TVReportBuilder.AppendFormat("\"{0}\",", "Sub Total");
                            _TVReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:n0}", audienceSub));
                            _TVReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:C}", iqAdsharevalueSub));
                        }
                        audienceTV = audienceTV + audienceSub;
                        iqAdSharevalueTV = iqAdSharevalueTV + iqAdsharevalueSub;

                        _TVReportBuilder.Append("\r\n");
                    }
                    if (_ViewstateInformation.IsNielSenData)
                    {
                        _TVReportBuilder.AppendFormat("\"{0}\",", "");
                        _TVReportBuilder.AppendFormat("\"{0}\",", "");
                        _TVReportBuilder.AppendFormat("\"{0}\",", "Total");
                        _TVReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:n0}", audienceTV));
                        _TVReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:C}", iqAdSharevalueTV));
                    }
                    _TVReportBuilder.Append("\r\n");
                }
                else
                {
                    _TVReportBuilder.Append("No Results Found");
                    _TVReportBuilder.Append("\r\n");
                }
                return _TVReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
            return string.Empty;

        }

        #endregion

        #region Bind Report News Grid

        private void BindReportNews()
        {
            try
            {
                IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();
                /*List<ArchiveNM> _ListOfArchilveNMCategoryGroup = _IArchiveNMController.GetArchiveNMReportGroupByCategory(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._MyIQReportParams.ReportDate.Value);

                List<ArchiveNM> _ListOfArchilveNMResult = new List<ArchiveNM>();

                foreach (ArchiveNM _ArchiveNM in _ListOfArchilveNMCategoryGroup)
                {
                    List<ArchiveNM> _ListOfArchilveNM = _IArchiveNMController.GetArchiveNMByCategoryGuidAndDate(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation.NewsReportSortExpression, _ViewstateInformation.IsNewsReportSortDirecitonAsc, _ViewstateInformation._MyIQReportParams.ReportDate.Value, _ArchiveNM.CategoryGuid, _ViewstateInformation.IsCompeteData);
                    foreach (ArchiveNM _ArchiveNMResult in _ListOfArchilveNM)
                    {
                        _ArchiveNMResult.CategoryNames = _ArchiveNM.CategoryNames;
                        _ArchiveNMResult.CategoryGuid = _ArchiveNM.CategoryGuid;
                    }
                    _ListOfArchilveNMResult.AddRange(_ListOfArchilveNM);
                }*/

                List<ArchiveNM> _ListOfArchilveNMResult = _IArchiveNMController.GetArchiveNMByDurationNCategoryGuid(new Guid(_SessionInformation.ClientGUID),
                                                                                               _ViewstateInformation.SocialMediaReportSortExpression,
                                                                                               _ViewstateInformation.IsSocialMediaReportSortDirecitonAsc,
                                                                                               _ViewstateInformation._MyIQReportParams.FromDate.Value,
                                                                                               _ViewstateInformation._MyIQReportParams.ToDate.Value,
                                                                                               (ddlCategory.SelectedIndex > 0 ? new Guid(ddlCategory.SelectedValue) : Guid.Empty),
                                                                                               _ViewstateInformation.IsCompeteData);

                _ViewstateInformation.MyIQNewsReportResult = CommonFunctions.SearializeJson(_ListOfArchilveNMResult);

                SetViewstateInformation(_ViewstateInformation);

                divNewsReport.InnerHtml = GetNewsReportHtml();

                //gvNewsReportSummery.DataSource = _ListOfArchilveNMCategoryGroup;
                //gvNewsReportSummery.DataBind();

                //rptReportNews.DataSource = _ListOfArchilveNMCategoryGroup;
                //rptReportNews.DataBind();

                if (_ListOfArchilveNMResult != null && _ListOfArchilveNMResult.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNewsRerpot", "$('#divReportNewsResult').show();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNewsRerpot", "$('#divReportNewsResult').hide();", true);
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
        }

        private string GetNewsReportHtml(bool IsEmailOrDownload = false)
        {
            try
            {
                StringBuilder _NewsReportBuilder = new StringBuilder();
                Int32 audienceSub = 0;
                decimal iqAdsharevalueSub = 0;

                audienceNM = 0;
                iqAdSharevalueNM = 0;

                List<ArchiveNM> _ListOfArchilveNMResult = new List<ArchiveNM>();
                _ListOfArchilveNMResult = (List<ArchiveNM>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQNewsReportResult, _ListOfArchilveNMResult.GetType());

                _NewsReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportNewsResult');\">");
                _NewsReportBuilder.Append("Online News");
                _NewsReportBuilder.Append("</div>");
                _NewsReportBuilder.Append("<div id=\"divReportNewsResult\" class=\"clear\">");

                if (_ListOfArchilveNMResult.Count > 0)
                {

                    List<ArchiveNM> _ListOfArchilveNMSummery = (from a in _ListOfArchilveNMResult
                                                                group a by a.CategoryGuid into b
                                                                where b.Count() > 0
                                                                select new ArchiveNM
                                                                {
                                                                    Total = b.Count(),
                                                                    CategoryGuid = b.Key,
                                                                    CategoryNames = b.Max(c => c.CategoryNames)

                                                                }).ToList();

                    _NewsReportBuilder.Append("<div class=\"padding5 clear\">");
                    _NewsReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"width:50%\" id=\"gvNewsReportSummery\" class=\"grid grid-iq\">");
                    _NewsReportBuilder.Append("<tr valign=\"top\" align=\"center\" style=\"height:10px;\" class=\"grid-th\">");
                    _NewsReportBuilder.Append("<th style=\"height:20px;width:70%;\" scope=\"col\" class=\"grid-th-left\">Category</th>");
                    _NewsReportBuilder.Append("<th style=\"height:20px;width:30%;\" scope=\"col\" class=\"grid-th-right\">Total</th>");
                    _NewsReportBuilder.Append("</tr>");


                    foreach (ArchiveNM _SummeryItem in _ListOfArchilveNMSummery)
                    {
                        _NewsReportBuilder.Append("<tr>");
                        _NewsReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _SummeryItem.CategoryNames);
                        _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", _SummeryItem.Total);
                        _NewsReportBuilder.Append("</tr>");
                    }

                    _NewsReportBuilder.Append("</table>");
                    _NewsReportBuilder.Append("</div>");

                    foreach (ArchiveNM _SummeryItem in _ListOfArchilveNMSummery)
                    {
                        audienceSub = 0;
                        iqAdsharevalueSub = 0;

                        _NewsReportBuilder.AppendFormat("<div onclick=\"ShowHideDiv('NewsReuslt{0}')\" style=\"margin:15px 0 3px 15px\" class=\"width100p ulheader cursor clear\" id=\"NewsHandle{0}\">{1}</div>", _SummeryItem.CategoryGuid, _SummeryItem.CategoryNames);
                        _NewsReportBuilder.AppendFormat("<div id=\"NewsReuslt{0}\" >", _SummeryItem.CategoryGuid);

                        List<ArchiveNM> _ListOfArchiveNMCategory = _ListOfArchilveNMResult.Where(a => a.CategoryGuid == _SummeryItem.CategoryGuid).ToList();
                        _NewsReportBuilder.AppendFormat("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"width: 100%;\" id=\"grvReportNews_{0}\" rules=\"all\" class=\"grid grid-iq\">", _SummeryItem.CategoryNames);
                        _NewsReportBuilder.Append("<tr>");
                        if (_ViewstateInformation.IsCompeteData)
                        {
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:12%;\">Harvest Time</th>");
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:6%;\">Article</th>");
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:20%;\">Publication</th>");
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:40%;\">Title</th>");
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:8%;\">Audience</th>");
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:14%;\">iQ Media Value</th>");

                        }
                        else
                        {
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:17%;\">Harvest Time</th>");
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:15%;\">Article</th>");
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:26%;\">Publication</th>");
                            _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:42%;\">Title</th>");

                        }

                        _NewsReportBuilder.Append("</tr>");
                        foreach (ArchiveNM _ArchiveNM in _ListOfArchiveNMCategory)
                        {

                            _NewsReportBuilder.Append("<tr>");
                            _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:MM/dd/yyyy hh:mm tt}", _ArchiveNM.Harvest_Time));

                            if (_SessionInformation != null && _SessionInformation.ClientGUID != null && !IsEmailOrDownload)
                            {
                                _NewsReportBuilder.AppendFormat("<td class=\"center\"><img  onclick=\"ShowArticle('{0}','{1}','{2}');\" style=\"border:0;cursor:pointer;\" alt=\"\" src=\"../Images/NewsRead.png\" /></td>", _ArchiveNM.Url, _ArchiveNM.ArticleID, Report_ReportType.NM.ToString());
                            }
                            else
                            {
                                _NewsReportBuilder.AppendFormat("<td class=\"center\"><a target=\"_blank\" href=\"{0}\" style=\"border:0;\" >View Article</a></td>", _ArchiveNM.Url);
                            }
                            _NewsReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _ArchiveNM.Publication);
                            _NewsReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _ArchiveNM.Title);

                            if (_ViewstateInformation.IsCompeteData)
                            {
                                string cUniqVisitorValue = string.Empty;
                                string iqAdshareValue = string.Empty;

                                cUniqVisitorValue = (_ArchiveNM.c_uniq_visitor == null || !_ArchiveNM.IsUrlFound) ? "NA" : string.Format("{0:n0}", _ArchiveNM.c_uniq_visitor);
                                if ((_ArchiveNM != null && (_ArchiveNM.c_uniq_visitor == -1)))
                                {
                                    cUniqVisitorValue = string.Empty;
                                }

                                iqAdshareValue = ((_ArchiveNM.IQ_AdShare_Value == null || !_ArchiveNM.IsUrlFound) ? "NA" : string.Format("{0:C}", _ArchiveNM.IQ_AdShare_Value.Value));
                                if ((_ArchiveNM != null && (_ArchiveNM.c_uniq_visitor == -1)))
                                {
                                    iqAdshareValue = string.Empty;
                                }

                                _NewsReportBuilder.Append("<td class=\"right\"><table style=\"margin-top:-6px;\"><tr>");
                                _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", cUniqVisitorValue);//(_ArchiveNM.c_uniq_visitor == null || !_ArchiveNM.IsUrlFound) ? "NA" : string.Format("{0:n0}", _ArchiveNM.c_uniq_visitor));
                                _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", (_ArchiveNM.IsCompeteAll ? "<img src=\"http://" + Request.Url.Host + "/Images/compete.jpg\" style=\"width:14px\"  title=\"Powered by Compete\" />" : ""));
                                _NewsReportBuilder.Append("</tr></table></td>");
                                _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", iqAdshareValue);//((_ArchiveNM.IQ_AdShare_Value == null || !_ArchiveNM.IsUrlFound) ? "NA" : string.Format("{0:C}", _ArchiveNM.IQ_AdShare_Value.Value)));


                                audienceSub = audienceSub + ((_ArchiveNM.c_uniq_visitor != null && _ArchiveNM.c_uniq_visitor == -1) ? 0 : Convert.ToInt32(_ArchiveNM.c_uniq_visitor));
                                iqAdsharevalueSub = iqAdsharevalueSub + ((_ArchiveNM.IQ_AdShare_Value != null && _ArchiveNM.IQ_AdShare_Value == -1) ? 0 : Convert.ToDecimal(_ArchiveNM.IQ_AdShare_Value));


                            }
                            _NewsReportBuilder.Append("</tr>");

                        }

                        audienceNM = audienceNM + audienceSub;
                        iqAdSharevalueNM = iqAdSharevalueNM + iqAdsharevalueSub;

                        if (_ViewstateInformation.IsCompeteData)
                        {
                            _NewsReportBuilder.Append("<tr>");
                            _NewsReportBuilder.Append("<td colspan=\"4\" class=\"right\">Sub Total : </td>");
                            _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:n0}", audienceSub));
                            _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:C}", iqAdsharevalueSub));


                            _NewsReportBuilder.Append("</tr>");
                        }
                        _NewsReportBuilder.Append("</table>");
                        _NewsReportBuilder.Append("</div>");
                    }

                    if (_ViewstateInformation.IsCompeteData)
                    {
                        _NewsReportBuilder.AppendFormat("<table style=\"margin:15px 0px 0 0;width:100%;\" class=\"grid grid-iq width100p\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">");

                        _NewsReportBuilder.Append("<tr>");
                        /*_NewsReportBuilder.Append("<td style=\"width:12%;\" class=\"right\"></td>");
                        _NewsReportBuilder.Append("<td style=\"width:6%;\" class=\"right\">&nbsp;</td>");
                        _NewsReportBuilder.Append("<td style=\"width:20%;\" class=\"right\">&nbsp;</td>");*/
                        _NewsReportBuilder.Append("<td style=\"width:78%;\" class=\"right\">Total : </td>");
                        _NewsReportBuilder.AppendFormat("<td style=\"width:8%;\" colspan=\"3\" class=\"right\">{0}</td>", string.Format("{0:n0}", audienceNM));
                        _NewsReportBuilder.AppendFormat("<td style=\"width:14%;\" class=\"right\">{0}</td>", string.Format("{0:C}", iqAdSharevalueNM));
                        _NewsReportBuilder.Append("</tr>");
                        _NewsReportBuilder.AppendFormat("</table>");
                    }
                    _NewsReportBuilder.Append("</div>");
                }
                else
                {
                    _NewsReportBuilder.Append("<div class=\"padding5\">");
                    _NewsReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width:100%\" id=\"gvNewsReportSummery\" class=\"grid grid-iq\">");
                    _NewsReportBuilder.Append("<tr><td colspan=\"2\">No Results Found</td></tr>");
                    _NewsReportBuilder.Append("</table>");
                    _NewsReportBuilder.Append("</div>");
                }
                return _NewsReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
            return string.Empty;

        }

        private string GetNewsReportCsv()
        {
            try
            {
                audienceNM = 0;
                iqAdSharevalueNM = 0;

                List<ArchiveNM> _ListOfArchilveNMResult = new List<ArchiveNM>();
                _ListOfArchilveNMResult = (List<ArchiveNM>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQNewsReportResult, _ListOfArchilveNMResult.GetType());

                StringBuilder _NewsReportBuilder = new StringBuilder();

                _NewsReportBuilder.Append("Online News");
                _NewsReportBuilder.Append("\r\n");

                if (_ListOfArchilveNMResult.Count > 0)
                {

                    List<ArchiveNM> _ListOfArchilveNMSummery = (from a in _ListOfArchilveNMResult
                                                                group a by a.CategoryGuid into b
                                                                where b.Count() > 0
                                                                select new ArchiveNM
                                                                {
                                                                    Total = b.Count(),
                                                                    CategoryGuid = b.Key,
                                                                    CategoryNames = b.Max(c => c.CategoryNames)

                                                                }).ToList();

                    _NewsReportBuilder.Append("Category,Total");
                    _NewsReportBuilder.Append("\r\n");


                    foreach (ArchiveNM _SummeryItem in _ListOfArchilveNMSummery)
                    {
                        _NewsReportBuilder.AppendFormat("\"{0}\",{1}", _SummeryItem.CategoryNames, _SummeryItem.Total);
                        _NewsReportBuilder.Append("\r\n");
                    }

                    _NewsReportBuilder.Append("\r\n");

                    foreach (ArchiveNM _SummeryItem in _ListOfArchilveNMSummery)
                    {
                        Int32 audienceSub = 0;
                        Decimal iqAdsharevalueSub = 0;

                        _NewsReportBuilder.AppendFormat("\"{0}\"", _SummeryItem.CategoryNames);
                        _NewsReportBuilder.Append("\r\n");

                        List<ArchiveNM> _ListOfArchiveNMCategory = _ListOfArchilveNMResult.Where(a => a.CategoryGuid == _SummeryItem.CategoryGuid).ToList();
                        if (_ViewstateInformation.IsCompeteData)
                        {
                            _NewsReportBuilder.Append("Harvest Time,Article,Publication,Title,Audience,iQ Media Value");
                        }
                        else
                        {
                            _NewsReportBuilder.Append("Harvest Time,Article,Publication,Title");
                        }
                        _NewsReportBuilder.Append("\r\n");

                        foreach (ArchiveNM _ArchiveNM in _ListOfArchiveNMCategory)
                        {
                            _NewsReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:MM/dd/yyyy hh:mm tt}", _ArchiveNM.Harvest_Time));
                            _NewsReportBuilder.AppendFormat("\"{0}\",", _ArchiveNM.Url);
                            _NewsReportBuilder.AppendFormat("\"{0}\",", _ArchiveNM.Publication);
                            _NewsReportBuilder.AppendFormat("\"{0}\",", _ArchiveNM.Title);


                            if (_ViewstateInformation.IsCompeteData)
                            {
                                string cUniqVisitorValue = string.Empty;
                                string iqAdshareValue = string.Empty;

                                cUniqVisitorValue = (_ArchiveNM.c_uniq_visitor == null || !_ArchiveNM.IsUrlFound) ? "NA" : string.Format("{0:n0}", _ArchiveNM.c_uniq_visitor);
                                if ((_ArchiveNM != null && (_ArchiveNM.c_uniq_visitor == -1)))
                                {
                                    cUniqVisitorValue = string.Empty;
                                }

                                iqAdshareValue = ((_ArchiveNM.IQ_AdShare_Value == null || !_ArchiveNM.IsUrlFound) ? "NA" : string.Format("{0:C}", _ArchiveNM.IQ_AdShare_Value.Value));
                                if ((_ArchiveNM != null && (_ArchiveNM.c_uniq_visitor == -1)))
                                {
                                    iqAdshareValue = string.Empty;
                                }

                                _NewsReportBuilder.AppendFormat("\"{0}\",", cUniqVisitorValue);
                                _NewsReportBuilder.AppendFormat("\"{0}\"", iqAdshareValue);

                                audienceSub = audienceSub + ((_ArchiveNM.c_uniq_visitor != null && _ArchiveNM.c_uniq_visitor == -1) ? 0 : Convert.ToInt32(_ArchiveNM.c_uniq_visitor));
                                iqAdsharevalueSub = iqAdsharevalueSub + ((_ArchiveNM.IQ_AdShare_Value != null && _ArchiveNM.IQ_AdShare_Value == -1) ? 0 : Convert.ToDecimal(_ArchiveNM.IQ_AdShare_Value));

                            }

                            _NewsReportBuilder.Append("\r\n");
                        }

                        _NewsReportBuilder.Append("\r\n");
                        if (_ViewstateInformation.IsCompeteData)
                        {
                            _NewsReportBuilder.AppendFormat("\"{0}\",", "");
                            _NewsReportBuilder.AppendFormat("\"{0}\",", "");
                            _NewsReportBuilder.AppendFormat("\"{0}\",", "");
                            _NewsReportBuilder.AppendFormat("\"{0}\",", "Sub Total");
                            _NewsReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:n0}", audienceSub));
                            _NewsReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:C}", iqAdsharevalueSub));
                        }
                        audienceNM = audienceNM + audienceSub;
                        iqAdSharevalueNM = iqAdSharevalueNM + iqAdsharevalueSub;

                        _NewsReportBuilder.Append("\r\n");

                    }

                    _NewsReportBuilder.Append("\r\n");
                    if (_ViewstateInformation.IsCompeteData)
                    {
                        _NewsReportBuilder.AppendFormat("\"{0}\",", "");
                        _NewsReportBuilder.AppendFormat("\"{0}\",", "");
                        _NewsReportBuilder.AppendFormat("\"{0}\",", "");
                        _NewsReportBuilder.AppendFormat("\"{0}\",", "Total");
                        _NewsReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:n0}", audienceNM));
                        _NewsReportBuilder.AppendFormat("\"{0}\"", string.Format("{0:C}", iqAdSharevalueNM));
                    }
                    _NewsReportBuilder.Append("\r\n");
                }
                else
                {
                    _NewsReportBuilder.Append("No Results Found");
                    _NewsReportBuilder.Append("\r\n");
                }
                return _NewsReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;//"An error occured, please try again!";
            }
            return string.Empty;

        }

        #endregion

        #region Bind Report Social Media Grid

        private void BindReportSocialMedia()
        {
            try
            {
                ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
                /*List<ArchiveSM> _ListOfArchilveSMCategoryGroup = _ISocialMediaController.GetArchiveSMReportGroupByCategory(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._MyIQReportParams.ReportDate.Value);

                List<ArchiveSM> _ListOfArchilveSMResult = new List<ArchiveSM>();

                foreach (ArchiveSM _ArchiveSM in _ListOfArchilveSMCategoryGroup)
                {
                    List<ArchiveSM> _ListOfArchilveSM = _ISocialMediaController.GetArchiveSMByCategoryGuidAndDate(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation.SocialMediaReportSortExpression, _ViewstateInformation.IsSocialMediaReportSortDirecitonAsc, _ViewstateInformation._MyIQReportParams.ReportDate.Value, _ArchiveSM.CategoryGuid, _ViewstateInformation.IsCompeteData);
                    foreach (ArchiveSM _ArchiveSMResult in _ListOfArchilveSM)
                    {
                        _ArchiveSMResult.CategoryNames = _ArchiveSM.CategoryNames;
                        _ArchiveSMResult.CategoryGuid = _ArchiveSM.CategoryGuid;
                    }
                    _ListOfArchilveSMResult.AddRange(_ListOfArchilveSM);
                }*/

                List<ArchiveSM> _ListOfArchilveSMResult = _ISocialMediaController.GetArchiveSMByDurationNCategoryGuid(new Guid(_SessionInformation.ClientGUID),
                                                                                                _ViewstateInformation.SocialMediaReportSortExpression,
                                                                                                _ViewstateInformation.IsSocialMediaReportSortDirecitonAsc,
                                                                                                _ViewstateInformation._MyIQReportParams.FromDate.Value,
                                                                                                _ViewstateInformation._MyIQReportParams.ToDate.Value,
                                                                                                (ddlCategory.SelectedIndex > 0 ? new Guid(ddlCategory.SelectedValue) : Guid.Empty),
                                                                                                _ViewstateInformation.IsCompeteData);

                _ViewstateInformation.MyIQSMReportResult = CommonFunctions.SearializeJson(_ListOfArchilveSMResult);

                SetViewstateInformation(_ViewstateInformation);

                divSocialMediaReport.InnerHtml = GetSocialMediaReportHtml();

                //gvSMReportSummery.DataSource = _ListOfArchilveSMCategoryGroup;
                //gvSMReportSummery.DataBind();

                //rptReportSM.DataSource = _ListOfArchilveSMCategoryGroup;
                //rptReportSM.DataBind();

                if (_ListOfArchilveSMResult != null && _ListOfArchilveSMResult.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSMRerpot", "$('#divReportSocialMediaResult').show();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSMRerpot", "$('#divReportSocialMediaResult').hide();", true);
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
        }

        private string GetSocialMediaReportHtml(bool IsEmailOrDownload = false)
        {
            try
            {
                StringBuilder _SocialReportBuilder = new StringBuilder();

                Int32 audienceSub = 0;
                Decimal iqAdsharevalueSub = 0;

                audienceSM = 0;
                iqAdSharevalueSM = 0;

                List<ArchiveSM> _ListOfArchilveSMResult = new List<ArchiveSM>();
                _ListOfArchilveSMResult = (List<ArchiveSM>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQSMReportResult, _ListOfArchilveSMResult.GetType());

                _SocialReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportSocialMediaResult');\">");
                _SocialReportBuilder.Append("Social Media");
                _SocialReportBuilder.Append("</div>");
                _SocialReportBuilder.Append("<div id=\"divReportSocialMediaResult\" class=\"clear\">");

                if (_ListOfArchilveSMResult.Count > 0)
                {

                    List<ArchiveSM> _ListOfArchilveSMSummary = (from a in _ListOfArchilveSMResult
                                                                group a by a.CategoryGuid into b
                                                                where b.Count() > 0
                                                                select new ArchiveSM
                                                                {
                                                                    Total = b.Count(),
                                                                    CategoryGuid = b.Key,
                                                                    CategoryNames = b.Max(c => c.CategoryNames)

                                                                }).ToList();

                    _SocialReportBuilder.Append("<div class=\"padding5  clear\">");
                    _SocialReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"width:50%\" id=\"gvSMReportSummery\" class=\"grid grid-iq\">");
                    _SocialReportBuilder.Append("<tr valign=\"top\" align=\"center\" style=\"height:10px;\" class=\"grid-th\">");
                    _SocialReportBuilder.Append("<th style=\"height:20px;width:70%;\" scope=\"col\" class=\"grid-th-left\">Category</th>");
                    _SocialReportBuilder.Append("<th style=\"height:20px;width:30%;\" scope=\"col\" class=\"grid-th-right\">Total</th>");
                    _SocialReportBuilder.Append("</tr>");


                    foreach (ArchiveSM _SummaryItem in _ListOfArchilveSMSummary)
                    {
                        _SocialReportBuilder.Append("<tr>");
                        _SocialReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _SummaryItem.CategoryNames);
                        _SocialReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", _SummaryItem.Total);
                        _SocialReportBuilder.Append("</tr>");
                    }

                    _SocialReportBuilder.Append("</table>");
                    _SocialReportBuilder.Append("</div>");

                    foreach (ArchiveSM _SummaryItem in _ListOfArchilveSMSummary)
                    {
                        audienceSub = 0;
                        iqAdsharevalueSub = 0;

                        _SocialReportBuilder.AppendFormat("<div onclick=\"ShowHideDiv('SMReuslt{0}')\" style=\"margin:15px 0px 3px 15px\" class=\"width100p ulheader cursor clear\" id=\"SMHandle{0}\">{1}</div>", _SummaryItem.CategoryGuid, _SummaryItem.CategoryNames);
                        _SocialReportBuilder.AppendFormat("<div id=\"SMReuslt{0}\">", _SummaryItem.CategoryGuid);

                        List<ArchiveSM> _ListOfArchiveSMCategory = _ListOfArchilveSMResult.Where(a => a.CategoryGuid == _SummaryItem.CategoryGuid).ToList();

                        _SocialReportBuilder.AppendFormat("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"width: 100%;\" id=\"grvReportSM_{0}\" rules=\"all\" class=\"grid grid-iq\">", _SummaryItem.CategoryNames);
                        _SocialReportBuilder.Append("<tr>");
                        if (_ViewstateInformation.IsCompeteData)
                        {
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:12%;\">Harvest Time</th>");
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:6%;\">Article</th>");
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:20%;\">Publication</th>");
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:40%;\">Title</th>");

                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:8%;\">Audience</th>");
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:14%;\">iQ Media Value</th>");

                        }
                        else
                        {
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:17%;\">Harvest Time</th>");
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:15%;\">Article</th>");
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:26%;\">Publication</th>");
                            _SocialReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:42%;\">Title</th>");


                        }
                        _SocialReportBuilder.Append("</tr>");
                        foreach (ArchiveSM _ArchiveSM in _ListOfArchiveSMCategory)
                        {
                            _SocialReportBuilder.Append("<tr>");

                            _SocialReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:MM/dd/yyyy hh:mm tt}", _ArchiveSM.Harvest_Time));
                            if (_SessionInformation != null && _SessionInformation.ClientGUID != null && !IsEmailOrDownload)
                            {
                                _SocialReportBuilder.AppendFormat("<td class=\"center\"><img  onclick=\"ShowArticle('{0}','{1}','{2}');\" style=\"border:0;cursor:pointer;\" alt=\"\" src=\"../Images/NewsRead.png\" /></td>", _ArchiveSM.Url, _ArchiveSM.ArticleID, Report_ReportType.SM.ToString());
                            }
                            else
                            {
                                _SocialReportBuilder.AppendFormat("<td class=\"center\"><a target=\"_blank\" href=\"{0}\" style=\"border:0;\" >View Article</a></td>", _ArchiveSM.Url);
                            }
                            _SocialReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _ArchiveSM.homeLink);
                            _SocialReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _ArchiveSM.Title);

                            if (_ViewstateInformation.IsCompeteData)
                            {
                                string cUniqVisitorValue = string.Empty;
                                string iqAdshareValue = string.Empty; ;
                                cUniqVisitorValue = (_ArchiveSM.c_uniq_visitor == null || !_ArchiveSM.IsUrlFound) ? "NA" : string.Format("{0:n0}", _ArchiveSM.c_uniq_visitor);
                                if ((_ArchiveSM != null && (_ArchiveSM.c_uniq_visitor == -1)))
                                {
                                    cUniqVisitorValue = string.Empty;
                                }
                                iqAdshareValue = ((_ArchiveSM.IQ_AdShare_Value == null || !_ArchiveSM.IsUrlFound) ? "NA" : string.Format("{0:C}", _ArchiveSM.IQ_AdShare_Value.Value));
                                if ((_ArchiveSM != null && (_ArchiveSM.c_uniq_visitor == -1)))
                                {
                                    iqAdshareValue = string.Empty;
                                }

                                _SocialReportBuilder.Append("<td class=\"right\"><table style=\"margin-top:-6px;\"><tr>");
                                _SocialReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", cUniqVisitorValue);//(_ArchiveSM.c_uniq_visitor == null || !_ArchiveSM.IsUrlFound) ? "NA" : string.Format("{0:n0}", _ArchiveSM.c_uniq_visitor));
                                _SocialReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", (_ArchiveSM.IsCompleteAll ? "<img src=\"http://" + Request.Url.Host + "/Images/compete.jpg\" style=\"width:14px\"  title=\"Powered by Compete\" />" : ""));
                                _SocialReportBuilder.Append("</tr></table></td>");
                                _SocialReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", iqAdshareValue);//((_ArchiveSM.IQ_AdShare_Value == null || !_ArchiveSM.IsUrlFound) ? "NA" : string.Format("{0:C}", _ArchiveSM.IQ_AdShare_Value.Value)));

                                audienceSub = audienceSub + ((_ArchiveSM.c_uniq_visitor != null && _ArchiveSM.c_uniq_visitor == -1) ? 0 : Convert.ToInt32(_ArchiveSM.c_uniq_visitor));
                                iqAdsharevalueSub = iqAdsharevalueSub + ((_ArchiveSM.IQ_AdShare_Value != null && _ArchiveSM.IQ_AdShare_Value == -1) ? 0 : Convert.ToDecimal(_ArchiveSM.IQ_AdShare_Value));




                            }

                            _SocialReportBuilder.Append("</tr>");



                        }

                        audienceSM = audienceSM + audienceSub;
                        iqAdSharevalueSM = iqAdSharevalueSM + iqAdsharevalueSub;


                        if (_ViewstateInformation.IsCompeteData)
                        {
                            _SocialReportBuilder.Append("<tr>");
                            _SocialReportBuilder.Append("<td colspan=\"4\" class=\"right\">Sub Total : </td>");
                            _SocialReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:n0}", audienceSub));
                            _SocialReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:C}", iqAdsharevalueSub));

                            _SocialReportBuilder.Append("</tr>");
                        }

                        _SocialReportBuilder.Append("</table>");
                        _SocialReportBuilder.Append("</div>");
                    }

                    if (_ViewstateInformation.IsCompeteData)
                    {
                        _SocialReportBuilder.AppendFormat("<table style=\"margin:15px 0px 0 0;width:100%;\" class=\"grid grid-iq width100p\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">");

                        _SocialReportBuilder.Append("<tr>");
                        /*_SocialReportBuilder.Append("<td style=\"width:12%;\" class=\"right\"></td>");
                        _SocialReportBuilder.Append("<td style=\"width:6%;\" class=\"right\">&nbsp;</td>");
                        _SocialReportBuilder.Append("<td style=\"width:20%;\" class=\"right\">&nbsp;</td>");*/
                        _SocialReportBuilder.Append("<td style=\"width:78%;\" class=\"right\">Total :</td>");
                        _SocialReportBuilder.AppendFormat("<td style=\"width:8%;\" colspan=\"3\" class=\"right\">{0}</td>", string.Format("{0:n0}", audienceSM));
                        _SocialReportBuilder.AppendFormat("<td style=\"width:14%;\" class=\"right\">{0}</td>", string.Format("{0:C}", iqAdSharevalueSM));
                        _SocialReportBuilder.Append("</tr>");
                        _SocialReportBuilder.AppendFormat("</table>");
                    }


                    _SocialReportBuilder.Append("</div>");
                }
                else
                {
                    _SocialReportBuilder.Append("<div class=\"padding5\">");
                    _SocialReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width:100%\" id=\"gvSMReportSummery\" class=\"grid grid-iq\">");
                    _SocialReportBuilder.Append("<tr><td colspan=\"2\">No Results Found</td></tr>");
                    _SocialReportBuilder.Append("</table>");
                    _SocialReportBuilder.Append("</div>");
                }
                return _SocialReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
            return string.Empty;

        }

        private string GetSocialMediaReportCsv()
        {
            try
            {
                audienceSM = 0;
                iqAdSharevalueSM = 0;

                List<ArchiveSM> _ListOfArchilveSMResult = new List<ArchiveSM>();
                _ListOfArchilveSMResult = (List<ArchiveSM>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQSMReportResult, _ListOfArchilveSMResult.GetType());

                StringBuilder _SocialReportBuilder = new StringBuilder();

                _SocialReportBuilder.Append("Social Media");
                _SocialReportBuilder.Append("\r\n");

                if (_ListOfArchilveSMResult.Count > 0)
                {

                    List<ArchiveSM> _ListOfArchilveSMSummery = (from a in _ListOfArchilveSMResult
                                                                group a by a.CategoryGuid into b
                                                                where b.Count() > 0
                                                                select new ArchiveSM
                                                                {
                                                                    Total = b.Count(),
                                                                    CategoryGuid = b.Key,
                                                                    CategoryNames = b.Max(c => c.CategoryNames)

                                                                }).ToList();

                    _SocialReportBuilder.Append("Category,Total");
                    _SocialReportBuilder.Append("\r\n");


                    foreach (ArchiveSM _SummeryItem in _ListOfArchilveSMSummery)
                    {
                        _SocialReportBuilder.AppendFormat("\"{0}\",{1}", _SummeryItem.CategoryNames, _SummeryItem.Total);
                        _SocialReportBuilder.Append("\r\n");
                    }

                    _SocialReportBuilder.Append("\r\n");

                    foreach (ArchiveSM _SummeryItem in _ListOfArchilveSMSummery)
                    {
                        Int32 audienceSub = 0;
                        Decimal iqAdsharevalueSub = 0;

                        _SocialReportBuilder.AppendFormat("\"{0}\"", _SummeryItem.CategoryNames);
                        _SocialReportBuilder.Append("\r\n");

                        List<ArchiveSM> _ListOfArchiveSMCategory = _ListOfArchilveSMResult.Where(a => a.CategoryGuid == _SummeryItem.CategoryGuid).ToList();

                        if (_ViewstateInformation.IsCompeteData)
                        {
                            _SocialReportBuilder.Append("Harvest Time,Article,Publication,Title,Audience,iQ Media Value");
                        }
                        else
                        {
                            _SocialReportBuilder.Append("Harvest Time,Article,Publication,Title");
                        }
                        _SocialReportBuilder.Append("\r\n");

                        foreach (ArchiveSM _ArchiveSM in _ListOfArchiveSMCategory)
                        {
                            _SocialReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:MM/dd/yyyy hh:mm tt}", _ArchiveSM.Harvest_Time));
                            _SocialReportBuilder.AppendFormat("\"{0}\",", _ArchiveSM.Url);
                            _SocialReportBuilder.AppendFormat("\"{0}\",", _ArchiveSM.homeLink);
                            _SocialReportBuilder.AppendFormat("\"{0}\",", _ArchiveSM.Title);
                            if (_ViewstateInformation.IsCompeteData)
                            {
                                string cUniqVisitorValue = string.Empty;
                                string iqAdshareValue = string.Empty; ;
                                cUniqVisitorValue = (_ArchiveSM.c_uniq_visitor == null || !_ArchiveSM.IsUrlFound) ? "NA" : string.Format("{0:n0}", _ArchiveSM.c_uniq_visitor);
                                if ((_ArchiveSM != null && (_ArchiveSM.c_uniq_visitor == -1)))
                                {
                                    cUniqVisitorValue = string.Empty;
                                }
                                iqAdshareValue = ((_ArchiveSM.IQ_AdShare_Value == null || !_ArchiveSM.IsUrlFound) ? "NA" : string.Format("{0:C}", _ArchiveSM.IQ_AdShare_Value.Value));
                                if ((_ArchiveSM != null && (_ArchiveSM.c_uniq_visitor == -1)))
                                {
                                    iqAdshareValue = string.Empty;
                                }

                                _SocialReportBuilder.AppendFormat("\"{0}\",", cUniqVisitorValue);
                                _SocialReportBuilder.AppendFormat("\"{0}\"", iqAdshareValue);

                                audienceSub = audienceSub + ((_ArchiveSM.c_uniq_visitor != null && _ArchiveSM.c_uniq_visitor == -1) ? 0 : Convert.ToInt32(_ArchiveSM.c_uniq_visitor));
                                iqAdsharevalueSub = iqAdsharevalueSub + ((_ArchiveSM.IQ_AdShare_Value != null && _ArchiveSM.IQ_AdShare_Value == -1) ? 0 : Convert.ToDecimal(_ArchiveSM.IQ_AdShare_Value));

                            }

                            _SocialReportBuilder.Append("\r\n");
                        }
                        if (_ViewstateInformation.IsCompeteData)
                        {
                            _SocialReportBuilder.AppendFormat("\"{0}\",", "");
                            _SocialReportBuilder.AppendFormat("\"{0}\",", "");
                            _SocialReportBuilder.AppendFormat("\"{0}\",", "");
                            _SocialReportBuilder.AppendFormat("\"{0}\",", "Sub Total");
                            _SocialReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:n0}", audienceSub));
                            _SocialReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:C}", iqAdsharevalueSub));
                        }
                        audienceSM = audienceSM + audienceSub;
                        iqAdSharevalueSM = iqAdSharevalueSM + iqAdsharevalueSub;


                        _SocialReportBuilder.Append("\r\n");
                    }
                    _SocialReportBuilder.Append("\r\n");
                    if (_ViewstateInformation.IsCompeteData)
                    {
                        _SocialReportBuilder.AppendFormat("\"{0}\",", "");
                        _SocialReportBuilder.AppendFormat("\"{0}\",", "");
                        _SocialReportBuilder.AppendFormat("\"{0}\",", "");
                        _SocialReportBuilder.AppendFormat("\"{0}\",", "Total");
                        _SocialReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:n0}", audienceSM));
                        _SocialReportBuilder.AppendFormat("\"{0}\"", string.Format("{0:C}", iqAdSharevalueSM));
                    }
                    _SocialReportBuilder.Append("\r\n");

                }
                else
                {
                    _SocialReportBuilder.Append("No Results Found");
                    _SocialReportBuilder.Append("\r\n");
                }
                return _SocialReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
            return string.Empty;

        }

        #endregion

        #region Bind Report Twitter

        private void BindReportTwitter()
        {
            try
            {
                IArchiveTweetsController _IArchiveTweetsController = _ControllerFactory.CreateObject<IArchiveTweetsController>();
                /*List<ArchiveTweets> _ListOfArchiveTweetsCategoryGroup = _IArchiveTweetsController.GetArchiveTweetsReportGroupByCategory(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._MyIQReportParams.ReportDate.Value);

                List<ArchiveTweets> _ListOfArchiveTweetsResult = new List<ArchiveTweets>();

                foreach (ArchiveTweets _ArchiveTweets in _ListOfArchiveTweetsCategoryGroup)
                {
                    List<ArchiveTweets> _ListOfArchiveTweets = _IArchiveTweetsController.GetArchiveTweetsByCategoryGuidAndDate(new Guid(_SessionInformation.ClientGUID), string.Empty, false, _ViewstateInformation._MyIQReportParams.ReportDate.Value, _ArchiveTweets.CategoryGuid);
                    foreach (ArchiveTweets _ArchiveTweetsResult in _ListOfArchiveTweets)
                    {
                        _ArchiveTweetsResult.CategoryNames = _ArchiveTweets.CategoryNames;
                        _ArchiveTweetsResult.CategoryGuid = _ArchiveTweets.CategoryGuid;
                    }
                    _ListOfArchiveTweetsResult.AddRange(_ListOfArchiveTweets);
                }*/

                List<ArchiveTweets> _ListOfArchilveTweetsResult = _IArchiveTweetsController.GetArchiveTweetsByDurationNCategoryGuid(new Guid(_SessionInformation.ClientGUID),
                                                                                              _ViewstateInformation.SocialMediaReportSortExpression,
                                                                                              _ViewstateInformation.IsSocialMediaReportSortDirecitonAsc,
                                                                                              _ViewstateInformation._MyIQReportParams.FromDate.Value,
                                                                                              _ViewstateInformation._MyIQReportParams.ToDate.Value,
                                                                                              (ddlCategory.SelectedIndex > 0 ? new Guid(ddlCategory.SelectedValue) : Guid.Empty));

                _ViewstateInformation.MyIQTwitterReportResult = CommonFunctions.SearializeJson(_ListOfArchilveTweetsResult);

                SetViewstateInformation(_ViewstateInformation);

                divTwitterReport.InnerHtml = GetReportTwitterHtml();

                if (_ListOfArchilveTweetsResult != null && _ListOfArchilveTweetsResult.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowTwitterRerpot", "$('#divReportTwitterResult').show();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTwitterRerpot", "$('#divReportTwitterResult').hide();", true);
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
        }

        private string GetReportTwitterHtml(bool IsEmail = false)
        {
            try
            {
                StringBuilder _TwitterReportBuilder = new StringBuilder();

                List<ArchiveTweets> _ListOfArchiveTweetsResult = new List<ArchiveTweets>();
                _ListOfArchiveTweetsResult = (List<ArchiveTweets>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTwitterReportResult, _ListOfArchiveTweetsResult.GetType());

                _TwitterReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportTwitterResult');\">");
                _TwitterReportBuilder.Append("Twitter");
                _TwitterReportBuilder.Append("</div>");
                _TwitterReportBuilder.Append("<div id=\"divReportTwitterResult\" class=\"clear\">");

                if (_ListOfArchiveTweetsResult.Count > 0)
                {
                    List<ArchiveTweets> _ListOfArchilveTweetsSummery = (from a in _ListOfArchiveTweetsResult
                                                                        group a by a.CategoryGuid into b
                                                                        where b.Count() > 0
                                                                        select new ArchiveTweets
                                                                {
                                                                    Total = b.Count(),
                                                                    CategoryGuid = b.Key,
                                                                    CategoryNames = b.Max(c => c.CategoryNames)

                                                                }).ToList();

                    _TwitterReportBuilder.Append("<div class=\"padding5  clear\">");
                    _TwitterReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"width:50%\" id=\"gvTwitterReportSummery\" class=\"grid grid-iq\">");
                    _TwitterReportBuilder.Append("<tr valign=\"top\" align=\"center\" style=\"height:10px;\" class=\"grid-th\">");
                    _TwitterReportBuilder.Append("<th style=\"height:20px;width:70%;\" scope=\"col\" class=\"grid-th-left\">Category</th>");
                    _TwitterReportBuilder.Append("<th style=\"height:20px;width:30%;\" scope=\"col\" class=\"grid-th-right\">Total</th>");
                    _TwitterReportBuilder.Append("</tr>");


                    foreach (ArchiveTweets _SummeryItem in _ListOfArchilveTweetsSummery)
                    {
                        _TwitterReportBuilder.Append("<tr>");
                        _TwitterReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _SummeryItem.CategoryNames);
                        _TwitterReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", _SummeryItem.Total);
                        _TwitterReportBuilder.Append("</tr>");
                    }

                    _TwitterReportBuilder.Append("</table>");
                    _TwitterReportBuilder.Append("</div>");

                    foreach (ArchiveTweets _SummeryItem in _ListOfArchilveTweetsSummery)
                    {
                        _TwitterReportBuilder.AppendFormat("<div onclick=\"ShowHideDiv('TweetReuslt{0}')\" style=\"margin:15px 0px 3px 15px\" class=\"width100p ulheader cursor clear\" id=\"TweetHandle{0}\">{1}</div>", _SummeryItem.CategoryGuid, _SummeryItem.CategoryNames);
                        _TwitterReportBuilder.AppendFormat("<div id=\"TweetReuslt{0}\">", _SummeryItem.CategoryGuid);

                        List<ArchiveTweets> _ListOfArchiveTweetsCategory = _ListOfArchiveTweetsResult.Where(a => a.CategoryGuid == _SummeryItem.CategoryGuid).ToList();

                        _TwitterReportBuilder.Append("<table cellspacing=\"0\" border=\"0\" style=\"width: 100%; border: 1px solid #999999; word-break: break-all; word-wrap: break-word;\" >");

                        int CurrentRecord = 0;
                        foreach (ArchiveTweets _ArchiveTweets in _ListOfArchiveTweetsCategory)
                        {
                            CurrentRecord++;
                            _TwitterReportBuilder.Append("<tr>");

                            if (IsEmail)
                            {

                                _TwitterReportBuilder.AppendFormat("<td>"
                                   + (CurrentRecord == _ListOfArchiveTweetsCategory.Count() ? "<table style=\"clear: both; overflow: hidden; width:100%;\">" : "<table style=\"clear: both; overflow: hidden; border-bottom: 1px solid #999999;width:100%;\">")
                                                               + "<tr><td align=\"left\" style=\"float: left; width: 90%; vertical-align: top; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box;\"><table style=\"width:100%;\"><tr><td style=\"float: left; font-size: 14px; width: 250px; line-height: 20px;width:300px;\"><table><tr><td>"

                                                                       + "<a target=\"_blank\"  " + (!string.IsNullOrWhiteSpace(_ArchiveTweets.Actor_link) ? "href=\"{0}\"" : string.Empty) + ">"
                                                                              + "{1}"
                                                                            + "</a>&nbsp;&nbsp;{2}&nbsp;</td>"
                                                                           + "</tr></table></td>"

                                                                       + "<td align=\"right\" style=\"line-height: 20px;width: 360px;\"><table><tr><td align=\"right\" style=\"font-size: 11px; color: #999999;width:120px;\">"
                                                                               + "Klout Score:&nbsp;{3}&nbsp;&nbsp;&nbsp;&nbsp;</td>"
                                                                            + "<td align=\"right\" style=\"font-size: 11px; color: #999999;width:120px;\">Followers:&nbsp; {4}&nbsp;&nbsp;&nbsp;&nbsp;</td>"

                                                                        + "<td align=\"right\" style=\"font-size: 11px; color: #999999;width:120px;\">Friends:&nbsp; {5}</td>"

                                                                        + "</tr></table></td></tr>"

                                                                        + "<tr><td style=\"float: left; line-height: 20px;\"><span>{6}</span></td><td align=\"right\" style=\"font-size: 11px; color: #999999; line-height: 20px;\"><span>{7}</span></td></tr></table></td>"
                                                                   + "<td style=\"float: right; width: 7%; text-align: center;\"><a target=\"_blank\" href=\"{0}\"><img style=\"border-width: 0px;\" src=\"{8}\"></a><br>"

                                                                   , _ArchiveTweets.Actor_link, _ArchiveTweets.Actor_DisplayName, _ArchiveTweets.Actor_PreferredUserName,
                                                                   _ArchiveTweets.gnip_Klout_Score, string.Format("{0:n0}", _ArchiveTweets.Actor_FollowersCount), string.Format("{0:n0}", _ArchiveTweets.Actor_FriendsCount), _ArchiveTweets.Tweet_Body,
                                                                   _ArchiveTweets.Tweet_PostedDateTime, _ArchiveTweets.Actor_Image);
                                _TwitterReportBuilder.Append("</td></tr></table>");

                                _TwitterReportBuilder.Append("</tr>");
                            }
                            else
                            {
                                _TwitterReportBuilder.Append("<tr>");
                                _TwitterReportBuilder.Append("<td>");

                                // div TweetInnerDiv start
                                _TwitterReportBuilder.Append("<div  style=\"" + (CurrentRecord == _ListOfArchiveTweetsCategory.Count() ? "border-bottom:0px none;" : "border-bottom:1px solid #999999;") + ";padding:5px;clear:both;overflow:hidden;\" >");

                                //div TweetBodyDiv start
                                _TwitterReportBuilder.Append("<div style=\"padding-right:3%;vertical-align:top;width:89%;float:left;\">");

                                // div clear start
                                _TwitterReportBuilder.Append("<div style=\"clear:both;overflow:hidden;\">");

                                _TwitterReportBuilder.Append("<div style=\"float:left;font-size:14px;max-width:45%;\">");
                                _TwitterReportBuilder.AppendFormat("<a style=\"float:left;\" href=\"{1}\" target=\"_blank\"><span>{0}</span></a>", _ArchiveTweets.Actor_DisplayName, _ArchiveTweets.Actor_link);
                                _TwitterReportBuilder.AppendFormat("<div style=\"float:left;font-size:11px;color:#999999;\">&nbsp;&nbsp;{0}</div><br>", _ArchiveTweets.Actor_PreferredUserName);
                                _TwitterReportBuilder.Append("</div>");

                                // div float-right start
                                _TwitterReportBuilder.Append("<div style=\"float:right;\">");

                                _TwitterReportBuilder.Append("<div style=\"float:left;font-size:11px;color:#999999;\">");
                                _TwitterReportBuilder.AppendFormat("Klout Score:&nbsp;{0}&nbsp;&nbsp;&nbsp;&nbsp;", _ArchiveTweets.gnip_Klout_Score.ToString());
                                _TwitterReportBuilder.Append("</div>");

                                _TwitterReportBuilder.Append("<div style=\"float:left;font-size:11px;color:#999999;\">");
                                _TwitterReportBuilder.AppendFormat("Followers:&nbsp;{0}&nbsp;&nbsp;&nbsp;&nbsp;", string.Format("{0:n0}", _ArchiveTweets.Actor_FollowersCount));
                                _TwitterReportBuilder.Append("</div>");

                                _TwitterReportBuilder.Append("<div style=\"float:left;font-size:11px;color:#999999;\">");
                                _TwitterReportBuilder.AppendFormat("Friends:&nbsp;{0}", string.Format("{0:n0}", _ArchiveTweets.Actor_FriendsCount));
                                _TwitterReportBuilder.Append("</div>");

                                _TwitterReportBuilder.Append("</div>");
                                // div float-right end

                                _TwitterReportBuilder.Append("</div>");
                                // div clear end

                                _TwitterReportBuilder.Append("<div style=\"clear:both;padding:1% 0;color:#535353;\">");
                                _TwitterReportBuilder.AppendFormat("<div style=\"float:left;width:75%;\"><span>{0}</span></div>", _ArchiveTweets.Tweet_Body);
                                _TwitterReportBuilder.AppendFormat("<div style=\"float:right;font-size:11px;color:#999999;\"><span>1/23/2013 6:02:00 AM</span></div>", _ArchiveTweets.Tweet_PostedDateTime.ToString());
                                _TwitterReportBuilder.Append("</div>");

                                _TwitterReportBuilder.Append("</div>");
                                //div TweetBodyDiv end

                                // div TweetImageDiv Start
                                _TwitterReportBuilder.Append("<div style=\"float:left;text-align:center;width:8%;\">");
                                _TwitterReportBuilder.AppendFormat("<a target=\"_blank\" href=\"{0}\" >", _ArchiveTweets.Actor_link);
                                _TwitterReportBuilder.AppendFormat("<img style=\"border-width:0px;\" src=\"{0}\" />", _ArchiveTweets.Actor_Image);
                                _TwitterReportBuilder.Append("</a>");
                                _TwitterReportBuilder.Append("</div>");
                                // div TweetImageDiv End

                                _TwitterReportBuilder.Append("</div>");
                                //div TweetInnerDiv end

                                _TwitterReportBuilder.Append("</td>");
                                _TwitterReportBuilder.Append("</tr>");
                            }
                        }

                        _TwitterReportBuilder.Append("</table>");
                        _TwitterReportBuilder.Append("</div>");
                    }

                }
                else
                {
                    _TwitterReportBuilder.Append("<div class=\"padding5\">");
                    _TwitterReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width:100%\" id=\"gvTwitterReportSummery\" class=\"grid grid-iq\">");
                    _TwitterReportBuilder.Append("<tr><td colspan=\"2\">No Results Found</td></tr>");
                    _TwitterReportBuilder.Append("</table>");
                    _TwitterReportBuilder.Append("</div>");
                }

                _TwitterReportBuilder.Append("</div>");

                return _TwitterReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
            return string.Empty;
        }

        private string GetReportTwitterCsv()
        {
            try
            {
                StringBuilder _TwitterReportBuilder = new StringBuilder();

                List<ArchiveTweets> _ListOfArchiveTweetsResult = new List<ArchiveTweets>();
                _ListOfArchiveTweetsResult = (List<ArchiveTweets>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTwitterReportResult, _ListOfArchiveTweetsResult.GetType());

                _TwitterReportBuilder.Append("Twitter");
                _TwitterReportBuilder.Append("\r\n");

                if (_ListOfArchiveTweetsResult.Count > 0)
                {
                    List<ArchiveTweets> _ListOfArchilveTweetsSummery = (from a in _ListOfArchiveTweetsResult
                                                                        group a by a.CategoryGuid into b
                                                                        where b.Count() > 0
                                                                        select new ArchiveTweets
                                                                        {
                                                                            Total = b.Count(),
                                                                            CategoryGuid = b.Key,
                                                                            CategoryNames = b.Max(c => c.CategoryNames)

                                                                        }).ToList();
                    _TwitterReportBuilder.Append("Category,Total");
                    _TwitterReportBuilder.Append("\r\n");


                    foreach (ArchiveTweets _SummeryItem in _ListOfArchilveTweetsSummery)
                    {
                        _TwitterReportBuilder.AppendFormat("\"{0}\",{1}", _SummeryItem.CategoryNames, _SummeryItem.Total);
                        _TwitterReportBuilder.Append("\r\n");
                    }

                    _TwitterReportBuilder.Append("\r\n");

                    foreach (ArchiveTweets _SummeryItem in _ListOfArchilveTweetsSummery)
                    {
                        _TwitterReportBuilder.AppendFormat("\"{0}\"", _SummeryItem.CategoryNames);
                        _TwitterReportBuilder.Append("\r\n");

                        List<ArchiveTweets> _ListOfArchiveTweetsCategory = _ListOfArchiveTweetsResult.Where(a => a.CategoryGuid == _SummeryItem.CategoryGuid).ToList();


                        _TwitterReportBuilder.Append("Tweet Body,Tweet Posted Date,Actor Display Name,Actor Preffered Username,Actor Link,Klout Score,Followers,Friends");
                        _TwitterReportBuilder.Append("\r\n");

                        foreach (ArchiveTweets _ArchiveTweets in _ListOfArchiveTweetsCategory)
                        {

                            _TwitterReportBuilder.AppendFormat("\"{0}\",", _ArchiveTweets.Tweet_Body.Replace("\"", "\"\""));
                            _TwitterReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:MM/dd/yyyy hh:mm tt}", _ArchiveTweets.Tweet_PostedDateTime));
                            _TwitterReportBuilder.AppendFormat("\"{0}\",", _ArchiveTweets.Actor_DisplayName);
                            _TwitterReportBuilder.AppendFormat("\"{0}\",", _ArchiveTweets.Actor_PreferredUserName);
                            _TwitterReportBuilder.AppendFormat("\"{0}\",", _ArchiveTweets.Actor_link);
                            _TwitterReportBuilder.AppendFormat("\"{0}\",", _ArchiveTweets.gnip_Klout_Score.ToString());
                            _TwitterReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:n0}", _ArchiveTweets.Actor_FollowersCount));
                            _TwitterReportBuilder.AppendFormat("\"{0}\"", string.Format("{0:n0}", _ArchiveTweets.Actor_FriendsCount));

                            _TwitterReportBuilder.Append("\r\n");
                        }

                        _TwitterReportBuilder.Append("\r\n");
                    }
                    _TwitterReportBuilder.Append("\r\n");
                }
                else
                {
                    _TwitterReportBuilder.Append("No Results Found");
                    _TwitterReportBuilder.Append("\r\n");
                }
                return _TwitterReportBuilder.ToString();

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblReportErr.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
            return string.Empty;
        }

        #endregion

        private void SetReportParams()
        {
            MyIQReportParams _MyIQReportParams = new MyIQReportParams();

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && !string.IsNullOrWhiteSpace(txtToDate.Text))
            {

                _MyIQReportParams.FromDate = Convert.ToDateTime(txtFromDate.Text);
                _MyIQReportParams.ToDate = Convert.ToDateTime(txtToDate.Text);
                _MyIQReportParams.ReportType = ddlReportType.SelectedValue;
            }


            _ViewstateInformation._MyIQReportParams = _MyIQReportParams;
            _ViewstateInformation.TVReportSortExpression = string.Empty;
            _ViewstateInformation.NewsReportSortExpression = string.Empty;
            _ViewstateInformation.SocialMediaReportSortExpression = string.Empty;
            _ViewstateInformation.IsTVReportSortDirecitonAsc = false;
            _ViewstateInformation.IsNewsReportSortDirecitonAsc = false;
            _ViewstateInformation.IsSocialMediaReportSortDirecitonAsc = false;
            SetViewstateInformation(_ViewstateInformation);
        }

        private bool ValidateReportSearch()
        {
            lblReportErr.Text = "";
            bool validate = true;

            if (string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                validate = false;
                lblReportErr.Text += "Please enter From date.<br/>";
            }

            if (string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                validate = false;
                lblReportErr.Text += "Please enter To date.<br/>";
            }

            if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && !string.IsNullOrWhiteSpace(txtFromDate.Text))
            {
                if ((Convert.ToDateTime(txtToDate.Text) - Convert.ToDateTime(txtFromDate.Text)).TotalDays > Convert.ToInt16(ConfigurationManager.AppSettings["ReportMyIQMaxDuration"]))
                {
                    validate = false;
                    lblReportErr.Text += IQMediaGroup.Common.Config.ConfigSettings.MessagesSection.Messages.FirstOrDefault(a => a.Key == "ReportMyIQMaxDurationMessage").Value;
                }
            }
            if (!chkReportTV.Checked && !chkReportNews.Checked && !chkReportSM.Checked && !chkReportTwitter.Checked)
            {
                validate = false;
                lblReportErr.Text += "Atleast one filter must be selected.<br/>";
            }

            return validate;
        }

        #endregion

        private void BindReportType()
        {
            try
            {

                IIQ_ReportTypeController _IIQ_ReportTypeController = _ControllerFactory.CreateObject<IIQ_ReportTypeController>();
                List<IQ_ReportType> _ListOfIQ_ReportType = _IIQ_ReportTypeController.GetReportTypeByClientSettings(new Guid(_SessionInformation.ClientGUID), IQ_MasterReportType.MyIQ.ToString());
                if (_ListOfIQ_ReportType.Count > 0)
                {
                    ddlReportType.DataSource = _ListOfIQ_ReportType;
                    ddlReportType.DataValueField = "ID";
                    ddlReportType.DataTextField = "Name";
                    ddlReportType.DataBind();
                    ddlReportType.Items.Insert(0, new ListItem("Select", "0"));
                    btnReport.Visible = true;
                }
                else
                {
                    lblReportErr.Text = "No Report";
                    ddlReportType.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblErrorMessage.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
                //Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string EmailContent = string.Empty;

                string toEmailAddresses = txtFriendsEmail.Text;

                string[] mailAddresses = toEmailAddresses.Split(";".ToCharArray());
                string IncorrectMessages = string.Empty;

                if (mailAddresses.Length > 0)
                {
                    foreach (string mailAddress in mailAddresses)
                    {
                        if (mailAddress.Length != 0)
                        {
                            if (validateEmails(mailAddress))
                            {

                                var sb = new StringBuilder();

                                sb.Append("<html>");
                                sb.Append("<head>");
                                sb.AppendFormat("<link href=\"http://{0}/Css/my-style.css\" rel=\"stylesheet\" type=\"text/css\" />", Request.ServerVariables["HTTP_HOST"]);
                                sb.AppendFormat("<link href=\"http://{0}/Css/fonts/stylesheet.css\" rel=\"stylesheet\" type=\"text/css\" />", Request.ServerVariables["HTTP_HOST"]);
                                //sb.Append("<script src=\"../../Script/jquery-1.8.3.min.js\" type=\"text/javascript\"></script>");
                                //sb.Append("<script src=\"../../Script/wkhtmltopdf.tablesplit.js\" type=\"text/javascript\"></script>");
                                sb.Append("</head>");
                                sb.Append("<body>");

                                divClientLogo.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                                divReportHeader.RenderControl(new HtmlTextWriter(new StringWriter(sb)));

                                if (chkReportTV.Checked)
                                {
                                    //divTVReport.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                                    sb.Append(GetTVReportHtml(true));
                                    divTVReport.InnerHtml = GetTVReportHtml();
                                }

                                if (chkReportNews.Checked && _SessionInformation.IsmyiQNM)
                                {
                                    //divNewsReport.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                                    sb.Append(GetNewsReportHtml(true));
                                    divNewsReport.InnerHtml = GetNewsReportHtml();
                                }

                                if (chkReportSM.Checked && _SessionInformation.IsmyiQSM)
                                {
                                    //divSocialMediaReport.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                                    sb.Append(GetSocialMediaReportHtml(true));
                                    divSocialMediaReport.InnerHtml = GetSocialMediaReportHtml();
                                }

                                if (chkReportTwitter.Checked && _SessionInformation.IsMyIQTwitter)
                                {
                                    sb.Append(GetReportTwitterHtml(true));
                                    divTwitterReport.InnerHtml = GetReportTwitterHtml();
                                }

                                divSummary.RenderControl(new HtmlTextWriter(new StringWriter(sb)));

                                sb.Append("</body>");
                                sb.Append("</html>");

                                EmailContent = sb.ToString();

                                string WholeEmailBody = "";
                                WholeEmailBody = CommonFunctions.EmailSend(txtYourEmail.Text, mailAddress, txtSubject.Text, txtMessage.Text, EmailContent);

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "closeEmailPopUp", "closeModal('pnlMailPanel');", true);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(IncorrectMessages))
                                {
                                    IncorrectMessages = IncorrectMessages + "," + mailAddress;
                                }
                                else
                                {
                                    IncorrectMessages = mailAddress;
                                }

                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(IncorrectMessages))
                    {
                        lblError.Text = "Following email address invalid " + IncorrectMessages;
                    }
                }
                else
                {
                    lblError.Text = "Please enter valid email address";
                    lblError.Visible = true;
                }

                if (!string.IsNullOrEmpty(IncorrectMessages))
                {

                    //mdlpopupEmail.Show();
                    lblError.Text = "Following email address invalid " + IncorrectMessages;
                    lblError.Visible = true;
                }
                else
                {

                    lblReportSuccessMsg.Text = "Email Sent Successfully.";
                    upMainGrid.Update();
                    //mdlpopupEmail.Hide();
                    UpdateUpdatePanel(upMail);
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                lblError.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
            }
        }

        private bool validateEmails(string _UserEmail)
        {
            try
            {

                string _EmailPatern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

                Regex _Regex = new Regex(_EmailPatern);

                return _Regex.IsMatch(_UserEmail);

            }
            catch (Exception)
            {

                throw;
            }

        }

        protected void PlayClip(string _clipID)
        {
            try
            {
                if (Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod"))
                {
                    string baseURL = string.Empty;
                    if (ConfigurationManager.AppSettings["MyCliqMediaHost"].Contains(Context.Request.Url.Host))
                    {
                        baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
                    }
                    else
                    {
                        baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];
                    }

                    string scriptForIPad = "CheckForIOS('iqmedia://clipid=" + _clipID + "&baseurl=" + baseURL + "','" + ConfigurationManager.AppSettings["IOSAppURL"] + "');";

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "IOSCheck", scriptForIPad.ToString(), true);
                }
                else if (Request.UserAgent.ToLower().Contains("android") && CheckVersion())
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "AndroidVideo", "LoadHTML5Player('" + _clipID + "');", true);
                }
                else
                {
                    InitialClip(_clipID, "false");
                }

                if (chkReportTV.Checked)
                {
                    divTVReport.InnerHtml = GetTVReportHtml();
                }

                if (chkReportNews.Checked && _SessionInformation.IsmyiQNM)
                {
                    divNewsReport.InnerHtml = GetNewsReportHtml();
                }

                if (chkReportSM.Checked && _SessionInformation.IsmyiQSM)
                {
                    divSocialMediaReport.InnerHtml = GetSocialMediaReportHtml();
                }

                if (chkReportTwitter.Checked && _SessionInformation.IsMyIQTwitter)
                {
                    divTwitterReport.InnerHtml = GetReportTwitterHtml();
                }

                upMainGrid.Update();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private bool CheckVersion()
        {
            Version defaultAndroidVersion = new Version(ConfigurationManager.AppSettings[CommonConstants.ConfigAndroidDefaultVersion]);
            string useragent = Request.UserAgent.ToLower(); //"Mozilla/5.0 (Linux; U; Android 2.1-update1; en-gb; GT-I5801 Build/ECLAIR) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17";
            //Regex regex = new Regex(@"(?<=\bandroid\s\b)(\d+(?:\.\d+)+)");
            Regex regex = new Regex(ConfigurationManager.AppSettings[CommonConstants.ConfigAndroidVersionRegex]);
            string version = Convert.ToString(regex.Match(useragent));

            if (string.IsNullOrWhiteSpace(version))
            {
                return false;
            }
            else
            {
                try
                {
                    Version currentVersion = new Version(version);
                    if (currentVersion >= defaultAndroidVersion)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {

                    return false;
                }

            }
            return false;

        }

        private void InitialClip(string _clipID, string _playback)
        {
            try
            {
                divRawMedia.Visible = true;

                string baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];

                if (HttpContext.Current.Request.ServerVariables["Http_Host"] == "mycliqmedia")
                {
                    baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
                }

                divRawMedia.Controls.Clear();
                divRawMedia.InnerHtml = IQMediaPlayer.RenderClipPlayer(new Guid(_SessionInformation.ClientGUID), "", _clipID, "false", "myIQ", _SessionInformation.Email, baseURL, _SessionInformation.IsClientPlayerLogoActive, _SessionInformation.ClientPlayerLogoImage);

                if (_ListOfRCatpion != null)
                {
                    _ListOfRCatpion.Clear();
                }

                IClipController _IClipController = _ControllerFactory.CreateObject<IClipController>();
                _ListOfRCatpion = _IClipController.GetClipCaption(_clipID);

                AddCaption(true);
                IsVisibleCaption(true);

                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CCCallBack", "RegisterCCCallback(1);", true);

                divClipPlayer.Visible = true;
                upClipPlayer.Update();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPlayerModal", "ShowModal('divClipPlayer');", true);
                //mpeClipPlayer.Show();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Description:This function show Caption is visible or not.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Visible">visible true or false</param>
        public void IsVisibleCaption(bool p_Visible)
        {
            try
            {
                DivCaption.Visible = p_Visible;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Description:This function Add Caption.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="IsClip">Isclip true or false</param>
        private void AddCaption(bool IsClip)
        {
            DivCaption.Controls.Clear();

            if (_ListOfRCatpion != null && _ListOfRCatpion.Count > 0)
            {
                foreach (Caption _Caption in _ListOfRCatpion)
                {
                    if (_Caption.CaptionString != CommonConstants.XMLErrorMessage)
                    {
                        HtmlGenericControl _Div = new HtmlGenericControl();

                        _Div.TagName = "span";
                        _Div.Attributes.Add("class", "hit");
                        _Div.Attributes.Add("onclick", "setSeekPoint(" + _Caption.StartTime + ");");

                        HtmlGenericControl _DivTime = new HtmlGenericControl();

                        _DivTime.TagName = "span";
                        _DivTime.Attributes.Add("class", "boldgray");
                        _DivTime.InnerText = _Caption.StartDateTime;

                        HtmlGenericControl _DivCaptionString = new HtmlGenericControl();

                        _DivCaptionString.TagName = "span";
                        _DivCaptionString.Attributes.Add("class", "caption");

                        _DivCaptionString.InnerText = _Caption.CaptionString;

                        if (IsClip == false)
                        {
                            _Div.Controls.Add(_DivTime);
                        }

                        _Div.Controls.Add(_DivCaptionString);

                        DivCaption.Controls.Add(_Div);

                    }
                    else
                    {
                        DivCaption.InnerHtml = CommonConstants.XMLErrorMessage;
                    }
                }
            }
            else
            {
                DivCaption.InnerHtml = CommonConstants.NoResultsFound;
            }
        }

        public void ResetReport()
        {
            txtFromDate.Text = DateTime.Now.ToShortDateString();
            txtToDate.Text = DateTime.Now.ToShortDateString();
            ddlReportType.SelectedIndex = 0;
            if (!_SessionInformation.IsmyiQNM)
            {
                chkReportNews.Visible = false;
                spnOnlineNews.Visible = false;
            }

            if (!_SessionInformation.IsmyiQSM)
            {
                chkReportSM.Visible = false;
                spnSocialMedia.Visible = false;
            }

            if (!_SessionInformation.IsMyIQTwitter)
            {
                chkReportTwitter.Visible = false;
                spnTwitter.Visible = false;
            }

            divReportResult.Visible = false;
            divTVReport.Visible = false;
            divNewsReport.Visible = false;
            divSocialMediaReport.Visible = false;
            divTwitterReport.Visible = false;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideReportSelection", "$('#ReportFilters').hide();", true);
        }

        protected void GetClientCompeteRights()
        {
            try
            {
                IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                _ViewstateInformation.IsCompeteData = Convert.ToBoolean(_IRoleController.GetClientRoleByClientGUIDRoleName(new Guid(_SessionInformation.ClientGUID), RolesName.CompeteData.ToString()));
                _ViewstateInformation.IsNielSenData = Convert.ToBoolean(_IRoleController.GetClientRoleByClientGUIDRoleName(new Guid(_SessionInformation.ClientGUID), RolesName.NielsenData.ToString()));
                SetViewstateInformation(_ViewstateInformation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetCustomCategoryByClientGUID()
        {
            try
            {

                string _ClientGUID = _SessionInformation.ClientGUID;
                List<CustomCategory> _ListofCustomCategory = new List<CustomCategory>();

                ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                _ListofCustomCategory = _ICustomCategoryController.SelectByClientGUID(new Guid(_ClientGUID));

                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryGUID";

                ddlCategory.DataSource = _ListofCustomCategory;
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new ListItem("All", "0"));

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}