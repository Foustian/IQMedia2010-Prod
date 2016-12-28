using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Reports.Controller.Factory;
using IQMediaGroup.Reports.Usercontrol.Base;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Reports.Controller.Interface;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using PMGSearch;
using System.Configuration;
using System.ComponentModel;



namespace IQMediaGroup.Reports.Usercontrol.IQMediaMaster.Report
{
    public partial class Report : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        SessionInformation _SessionInformation;
        ViewstateInformation _ViewstateInformation;

        public bool isEmailActive
        {
            get
            {
                return _isEmailActive;
            }
            set
            {
                _isEmailActive = value;
                if (value)
                {
                    btnReportEmail.Visible = true;

                }
                else
                {
                    btnReportEmail.Visible = false;
                }
            }
        }public bool _isEmailActive = false;

        [DefaultValue(false)]
        public bool IsCompeteData { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                _SessionInformation = CommonFunctions.GetSessionInformation();
                _ViewstateInformation = GetViewstateInformation();
                if (Page.IsPostBack)
                {
                    lblReportErrorMsg.Text = string.Empty;
                    lblReportMsg.Text = string.Empty;
                    lblReportErrorMsg.Visible = false;
                    lblError.Text = string.Empty;

                    if (Request["__EventTarget"] == upReport.ClientID && Request["__EventArgument"] != null)
                    {
                        PlayRawMedia(Request["__EventArgument"]);
                    }

                }
                else
                {
                    BindMediaCategoryDropDown();
                }

            }
            catch (Exception ex)
            {
                lblReportErrorMsg.Text = "An Error Occured, Please Try Again!!";
                divReportResult.Visible = false;
                lblReportErrorMsg.Visible = true;
            }
        }

        #region Methods
        public void FetchReportXml(string p_ReportXml, Guid p_ClientGuid, string p_ReportTypeName, string p_ReportIdentity, string p_EncReportID)
        {
            try
            {
                hfReportID.Value = p_EncReportID;
                if (_ViewstateInformation == null)
                    _ViewstateInformation = GetViewstateInformation();

                if (_SessionInformation == null)
                    _SessionInformation = CommonFunctions.GetSessionInformation();

                _ViewstateInformation.MyIQTVReportResult = string.Empty;
                _ViewstateInformation.MyIQNewsReportResult = string.Empty;
                _ViewstateInformation.MyIQSMReportResult = string.Empty;
                _ViewstateInformation.IsIQAgentTVResultShow = false;
                _ViewstateInformation.IsIQAgentNMResultShow = false;
                _ViewstateInformation.IsIQAgentSMResultShow = false;
                _ViewstateInformation.IsIQAgentTwitterResultShow = false;
                SetViewstateInformation(_ViewstateInformation);

                if (!string.IsNullOrWhiteSpace(p_ReportXml))
                {
                    XDocument _ReportDoc = XDocument.Parse(p_ReportXml);

                    if (_ReportDoc.Descendants("SearchRequest") != null)
                    {
                        DateTime FromDate = new DateTime();
                        DateTime ToDate = new DateTime();
                        int IQAgentSearchRequestID;
                        string Query_Name = string.Empty;
                        string SearchTerm = string.Empty;

                        //if (_ReportDoc.Root.Element("Media").Descendants("Type").FirstOrDefault() != null)
                        //{
                        if ((_ReportDoc.Descendants("SearchRequest").Descendants("ID").FirstOrDefault() != null && int.TryParse(_ReportDoc.Descendants("SearchRequest").Descendants("ID").FirstOrDefault().Value, out IQAgentSearchRequestID)))
                        {
                            if ((_ReportDoc.Descendants("SearchRequest").Descendants("StartDate").FirstOrDefault() != null && DateTime.TryParse(_ReportDoc.Descendants("SearchRequest").Descendants("StartDate").FirstOrDefault().Value, out FromDate)) && (_ReportDoc.Descendants("SearchRequest").Descendants("EndDate").FirstOrDefault() != null && DateTime.TryParse(_ReportDoc.Descendants("SearchRequest").Descendants("EndDate").FirstOrDefault().Value, out ToDate)))
                            {
                                lblReportHeader.Text = p_ReportTypeName + " - " + Query_Name + " - " + string.Format("{0:MM/dd/yyyy hh:mm tt}", FromDate) + " To " + string.Format("{0:MM/dd/yyyy hh:mm tt}", ToDate);

                                lblReportErrorMsg.Visible = false;

                                foreach (XElement _Element in _ReportDoc.Descendants("Media_Set").Descendants("Media"))
                                {
                                    int _NoOfRecordsToDisplay;
                                    if (Request.QueryString["source"] != null && Request.QueryString["source"].ToLower() == "email")
                                        _NoOfRecordsToDisplay = GetNoOfRecordToDisplayInEmail(_Element);// Convert.ToInt32(_Element.Element("NoOfRecordsToDisplayInEmail").Value);
                                    else
                                        _NoOfRecordsToDisplay = GetNoOfRecordToDisplay(_Element);// Convert.ToInt32(_Element.Element("NoOfRecordsToDisplay").Value);

                                    System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
                                    div.TagName = CommonConstants.HTMLDiv;
                                    if (_Element.Element("Type").Value == Report_ReportType.TV.ToString())
                                    {
                                        IQMediaGroup.Reports.Controller.Interface.IIQAgentResultsController _IIQAgentResultsController = _ControllerFactory.CreateObject<IQMediaGroup.Reports.Controller.Interface.IIQAgentResultsController>();
                                        List<IQAgentResults> _ListOfIQAgentResults = _IIQAgentResultsController.GetIQAgentResultBySearchDate(p_ClientGuid, IQAgentSearchRequestID, FromDate, ToDate, _NoOfRecordsToDisplay, _SessionInformation.IsNielSenData, out Query_Name, out SearchTerm);                                        //_ViewstateInformation.Report_ReportType = Report_ReportType.TV;
                                        _ViewstateInformation.MyIQTVReportResult = CommonFunctions.SearializeJson(_ListOfIQAgentResults);
                                        _ViewstateInformation._ClipSearchTerm = SearchTerm;

                                        _ViewstateInformation.TVReportNoOfRecordsDisplayInEmail = GetNoOfRecordToDisplayInEmail(_Element);// Convert.ToInt32(_Element.Element("NoOfRecordsToDisplayInEmail").Value);
                                        _ViewstateInformation.IsIQAgentTVResultShow = true;

                                        SetViewstateInformation(_ViewstateInformation);
                                        div.InnerHtml = GetTVReportHtml();
                                        divTVReport.Controls.Add(div);
                                        divReportResult.Visible = true;
                                    }

                                    else if (_Element.Element("Type").Value == Report_ReportType.NM.ToString())
                                    {
                                        INMController _INMController = _ControllerFactory.CreateObject<INMController>();
                                        List<NewsResult> _ListOfNewsResult = _INMController.GetIQAgent_NMResultBySearchDate(p_ClientGuid, IQAgentSearchRequestID, FromDate, ToDate, _NoOfRecordsToDisplay, IsCompeteData, out Query_Name);
                                        //_ViewstateInformation.Report_ReportType = Report_ReportType.NEWS;
                                        _ViewstateInformation.MyIQNewsReportResult = CommonFunctions.SearializeJson(_ListOfNewsResult);

                                        _ViewstateInformation.NMReportNoOfRecordsDisplayInEmail = GetNoOfRecordToDisplayInEmail(_Element); //Convert.ToInt32(_Element.Element("NoOfRecordsToDisplayInEmail").Value);
                                        _ViewstateInformation.IsIQAgentNMResultShow = true;
                                        SetViewstateInformation(_ViewstateInformation);
                                        div.InnerHtml = GetNEWSReportHtml();
                                        divNewsReport.Controls.Add(div);
                                        divReportResult.Visible = true;
                                    }

                                    else if (_Element.Element("Type").Value == Report_ReportType.SM.ToString())
                                    {
                                        ISMController _ISMController = _ControllerFactory.CreateObject<ISMController>();
                                        List<SMResult> _ListOfSMResult = _ISMController.GetIQAgent_SMResultBySearchDate(p_ClientGuid, IQAgentSearchRequestID, FromDate, ToDate, _NoOfRecordsToDisplay, IsCompeteData, out Query_Name);
                                        //_ViewstateInformation.Report_ReportType = Report_ReportType.SM;
                                        _ViewstateInformation.MyIQSMReportResult = CommonFunctions.SearializeJson(_ListOfSMResult);

                                        _ViewstateInformation.SMReportNoOfRecordsDisplayInEmail = GetNoOfRecordToDisplayInEmail(_Element);// Convert.ToInt32(_Element.Element("NoOfRecordsToDisplayInEmail").Value);
                                        _ViewstateInformation.IsIQAgentSMResultShow = true;
                                        SetViewstateInformation(_ViewstateInformation);
                                        div.InnerHtml = GetSMReportHtml();
                                        divSocialReport.Controls.Add(div);
                                        divReportResult.Visible = true;
                                    }
                                    else if (_Element.Element("Type").Value == Report_ReportType.TW.ToString())
                                    {
                                        ITwitterController _ITwitterController = _ControllerFactory.CreateObject<ITwitterController>();
                                        List<TwitterResult> _ListOfSTwitterResult = _ITwitterController.GetIQAgent_TwitterResultBySearchDate(p_ClientGuid, IQAgentSearchRequestID, FromDate, ToDate, _NoOfRecordsToDisplay, out Query_Name);
                                        //_ViewstateInformation.Report_ReportType = Report_ReportType.SM;
                                        _ViewstateInformation.MyIQTwitterReportResult = CommonFunctions.SearializeJson(_ListOfSTwitterResult);

                                        _ViewstateInformation.TWReportNoOfRecordsDisplayInEmail = GetNoOfRecordToDisplayInEmail(_Element); //Convert.ToInt32(_Element.Element("NoOfRecordsToDisplayInEmail").Value);
                                        _ViewstateInformation.IsIQAgentTwitterResultShow = true;
                                        SetViewstateInformation(_ViewstateInformation);
                                        div.InnerHtml = GetTWReportHtml();
                                        divTwitterReport.Controls.Add(div);
                                        divReportResult.Visible = true;
                                    }
                                }
                            }
                            else
                            {
                                lblReportErrorMsg.Text = "An Error Occured, Please Try Again!!";
                                divReportResult.Visible = false;
                                lblReportErrorMsg.Visible = true;
                            }
                        }
                        //}
                    }
                    else
                    {
                        lblReportErrorMsg.Text = "An Error Occured, Please Try Again!!";
                        divReportResult.Visible = false;
                        lblReportErrorMsg.Visible = true;
                    }
                }
                else
                {
                    lblReportErrorMsg.Text = "An Error Occured, Please Try Again!!";
                    divReportResult.Visible = false;
                    lblReportErrorMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblReportErrorMsg.Text = "An Error Occured, Please Try Again!!";
                divReportResult.Visible = false;
                lblReportErrorMsg.Visible = true;
            }
        }

        private string GetTVReportHtml(bool IsEmailOrDownload = false, int LimitRecords = 0)
        {
            try
            {

                List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();
                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQTVReportResult))
                    _ListOfIQAgentResults = (List<IQAgentResults>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTVReportResult, _ListOfIQAgentResults.GetType());

                StringBuilder _TVReportBuilder = new StringBuilder();

                _TVReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportTVResult');\">");
                _TVReportBuilder.Append("TV");
                _TVReportBuilder.Append("</div>");
                _TVReportBuilder.Append("<div id=\"divReportTVResult\" class=\"clear\">");

                if (_ListOfIQAgentResults.Count > 0)
                {

                    _TVReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"width: 100%;\" rules=\"all\" class=\"grid grid-iq\">");
                    _TVReportBuilder.Append("<tr>");
                    //_TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:3%;\">No#</th>");

                    if (_SessionInformation.IsNielSenData)
                    {
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:6%;\"></th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:22%;\">Program</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:12%;\">DateTime</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:11%;\">Station</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:22%;\">Market</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:11%;\">Audience</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:9%;\">iQ Media Value</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:7%;\">Hits</th>");
                    }
                    else
                    {
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:7%;\"></th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:30%;\">Program</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:22%;\">DateTime</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:12%;\">Station</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:22%;\">Market</th>");
                        _TVReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:7%;\">Hits</th>");
                    }
                    _TVReportBuilder.Append("</tr>");
                    for (int i = 0; i < _ListOfIQAgentResults.Count; i++)
                    {
                        if (LimitRecords != 0 && i >= LimitRecords)
                            break;

                        IQAgentResults _IQAgentResults = _ListOfIQAgentResults[i];

                        if (File.Exists(Server.MapPath("~/StationLogoImages/" + _IQAgentResults.Rl_Station + ".gif")))
                        {
                            _IQAgentResults.StationLogo = "http://" + Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _IQAgentResults.Rl_Station + ".gif";
                        }
                        else if (File.Exists(Server.MapPath("~/StationLogoImages/" + _IQAgentResults.Rl_Station + ".jpg")))
                        {
                            _IQAgentResults.StationLogo = "http://" + Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _IQAgentResults.Rl_Station + ".jpg";
                        }

                        _TVReportBuilder.Append("<tr>");
                        //_TVReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", ++i);
                        if (_SessionInformation != null && _SessionInformation.ClientGUID != null && !IsEmailOrDownload)
                        {
                            _TVReportBuilder.AppendFormat("<td class=\"center\"><a target=\"_blank\" style=\"border:0;cursor:pointer;\" onclick=\"PlayVideo('{0}')\" >Play</a></td>", _IQAgentResults.RL_VideoGUID);
                        }
                        else
                        {
                            //_TVReportBuilder.AppendFormat("<td class=\"center\"><a target=\"_blank\"  href=\"{0}\" style=\"border:0\" >Play</a></td>", string.IsNullOrWhiteSpace(_IQAgentResults.IQAgentResultUrl) ? "javascript:;" : ConfigurationManager.AppSettings["IQAgentIFrameUrl"] + _IQAgentResults.IQAgentResultUrl);                            
                            _TVReportBuilder.AppendFormat("<td class=\"center\">" + (!string.IsNullOrWhiteSpace(_IQAgentResults.IQAgentResultUrl) ? "<a target=\"_blank\"  href=\" " + ConfigurationManager.AppSettings["IQAgentIFrameUrl"] + _IQAgentResults.IQAgentResultUrl + "\" style=\"border:0\" >Play</a>" : "") + "</td>");

                        }
                        _TVReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _IQAgentResults.Title120);
                        _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:MM/dd/yyyy hh:mm tt}", _IQAgentResults.RL_Date));
                        //_TVReportBuilder.AppendFormat("<td class=\"center\"><a href=\"javascript:;\" style=\"border:0\"><img style=\"border:0\" alt=\"\" src=\"{0}\" /></a></td>", _IQAgentResults.StationLogo);
                        _TVReportBuilder.AppendFormat("<td class=\"center\"><img style=\"border:0\" alt=\"\" src=\"{0}\" /></td>", _IQAgentResults.StationLogo);
                        _TVReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _IQAgentResults.RL_Market);

                        if (_SessionInformation.IsNielSenData)
                        {
                            _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", _IQAgentResults.AUDIENCE == null ? "NA" : string.Format("{0:n0}", _IQAgentResults.AUDIENCE));
                            _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", (string.IsNullOrWhiteSpace(_IQAgentResults.SQAD_SHAREVALUE) ? "NA" : _IQAgentResults.SQAD_SHAREVALUE));
                        }
                        _TVReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", _IQAgentResults.Number_Hits);
                        _TVReportBuilder.Append("</tr>");
                    }

                    _TVReportBuilder.Append("</table>");
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
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        private string GetNEWSReportHtml(bool IsEmailOrDownload = false, int LimitRecords = 0)
        {
            try
            {
                List<NewsResult> _ListOfNewsResult = new List<NewsResult>();

                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQNewsReportResult))
                    _ListOfNewsResult = (List<NewsResult>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQNewsReportResult, _ListOfNewsResult.GetType());

                StringBuilder _NewsReportBuilder = new StringBuilder();

                _NewsReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportNewsResult');\">");
                _NewsReportBuilder.Append("NEWS");
                _NewsReportBuilder.Append("</div>");
                _NewsReportBuilder.Append("<div id=\"divReportNewsResult\" class=\"clear\">");

                if (_ListOfNewsResult.Count > 0)
                {

                    _NewsReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"width: 100%;\" rules=\"all\" class=\"grid grid-iq\">");
                    _NewsReportBuilder.Append("<tr>");
                    if (IsCompeteData)
                    {
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:6%;\">Article</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:12%;\">Publication</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:28%;\">Title</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:10%;\">Date Time</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:11%;\">Category</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:14%;\">Genre</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:13%;\">Audience</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:6%;\">iQ Media Value</th>");
                    }
                    else
                    {
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:6%;\">Article</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:17%;\">Publication</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:33%;\">Title</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:18%;\">Date Time</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:11%;\">Category</th>");
                        _NewsReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:14%;\">Genre</th>");
                    }
                    _NewsReportBuilder.Append("</tr>");

                    for (int i = 0; i < _ListOfNewsResult.Count; i++)
                    {
                        if (LimitRecords != 0 && i >= LimitRecords)
                            break;

                        NewsResult _NewsResult = _ListOfNewsResult[i];
                        _NewsReportBuilder.Append("<tr>");
                        if (_SessionInformation != null && _SessionInformation.ClientGUID != null && !IsEmailOrDownload)
                        {
                            _NewsReportBuilder.AppendFormat("<td class=\"center\"><img  onclick=\"ShowArticle('{0}','{1}','{2}');\" style=\"border:0;cursor:pointer;\" alt=\"\" src=\"../Images/NewsRead.png\" /></td>", _NewsResult.Article, _NewsResult.ID, Report_ReportType.NM.ToString());
                        }
                        else
                        {
                            _NewsReportBuilder.AppendFormat("<td class=\"center\"><a target=\"_blank\" style=\"border:0;\" href=\"{0}\">View</a></td>", _NewsResult.Article);
                        }
                        _NewsReportBuilder.AppendFormat("<td class=\"left\"><a target=\"_blank\"  href=\"{0}\" style=\"border:0\" >{0}</a></td>", string.IsNullOrWhiteSpace(_NewsResult.publication) ? "javascript:;" : _NewsResult.publication);
                        _NewsReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _NewsResult.Title);
                        _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", string.Format("{0:MM/dd/yyyy hh:mm tt}", _NewsResult.date));
                        _NewsReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _NewsResult.Category);
                        _NewsReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _NewsResult.Genre);

                        if (IsCompeteData)
                        {

                            string cUniqueVisitorValue = string.Empty;
                            string iqAdshareValue = string.Empty;
                            cUniqueVisitorValue = ((_NewsResult.C_uniq_visitor == null || !_NewsResult.IsUrlFound) ? "NA" : string.Format("{0:n0}", _NewsResult.C_uniq_visitor));
                            if ((_NewsResult != null && (_NewsResult.C_uniq_visitor == -1)))
                            {
                                cUniqueVisitorValue = string.Empty;
                            }

                            iqAdshareValue = ((_NewsResult.IQ_AdShare_Value == null || !_NewsResult.IsUrlFound) ? "NA" : string.Format("{0:C}", _NewsResult.IQ_AdShare_Value.Value));
                            if ((_NewsResult != null && (_NewsResult.IQ_AdShare_Value == -1)))
                            {
                                iqAdshareValue = string.Empty;
                            }

                            _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}&nbsp;&nbsp;{1}</td>", cUniqueVisitorValue, (_NewsResult.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\"  title=\"Powered by Compete\" />" : string.Empty));
                            /*_NewsReportBuilder.Append("<td class=\"right\"><table style=\"margin-top:-6px;\"><tr>");
                            _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", cUniqueVisitorValue);//(_NewsResult.C_uniq_visitor == null || !_NewsResult.IsUrlFound) ? "NA" : string.Format("{0:n0}", _NewsResult.C_uniq_visitor));
                            _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", (_NewsResult.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\"  title=\"Powered by Compete\" />" : ""));
                            _NewsReportBuilder.Append("</tr></table></td>");*/
                            _NewsReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", iqAdshareValue);//((_NewsResult.IQ_AdShare_Value == null || !_NewsResult.IsUrlFound) ? "NA" : string.Format("{0:C}", _NewsResult.IQ_AdShare_Value.Value)));
                        }
                        _NewsReportBuilder.Append("</tr>");
                    }

                    _NewsReportBuilder.Append("</table>");
                }
                else
                {
                    _NewsReportBuilder.Append("<div class=\"padding5\">");
                    _NewsReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width:100%\" id=\"gvNEWSReportSummary\" class=\"grid grid-iq\">");
                    _NewsReportBuilder.Append("<tr><td colspan=\"2\">No Results Found</td></tr>");
                    _NewsReportBuilder.Append("</table>");
                    _NewsReportBuilder.Append("</div>");
                }
                _NewsReportBuilder.Append("</div>");
                return _NewsReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        private string GetSMReportHtml(bool IsEmailOrDownload = false, int LimitRecords = 0)
        {
            try
            {

                StringBuilder _SMReportBuilder = new StringBuilder();

                _SMReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportSMResult');\">");
                _SMReportBuilder.Append("Social Media");
                _SMReportBuilder.Append("</div>");
                _SMReportBuilder.Append("<div id=\"divReportSMResult\" class=\"clear\">");

                List<SMResult> _ListOfSMResult = new List<SMResult>();

                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQSMReportResult))
                {
                    _ListOfSMResult = (List<SMResult>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQSMReportResult, _ListOfSMResult.GetType());
                }

                if (_ListOfSMResult.Count > 0)
                {

                    _SMReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" style=\"max-width: 100%;border-collapse:collapse;word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;\" rules=\"all\" class=\"grid grid-iq\">");
                    _SMReportBuilder.Append("<tr>");
                    if (IsCompeteData)
                    {
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:6%;\">Article</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:19%;\">Publication</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:22%;\">Title</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:10%;\">Date Time</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:10%;\">Category</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:7%;\">Type</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:6%;\">Rank</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:11%;\">Audience</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:9%;\">iQ Media Value</th>");
                    }
                    else
                    {
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th center\" scope=\"col\" style=\"width:7%;\">Article</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:14%;\">Publication</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:29%;\">Title</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:18%;\">Date Time</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:13%;\">Category</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-left\" scope=\"col\" style=\"width:13%;\">Type</th>");
                        _SMReportBuilder.Append("<th valign=\"top\" class=\"grid-th-right\" scope=\"col\" style=\"width:6%;\">Rank</th>");
                    }
                    _SMReportBuilder.Append("</tr>");

                    for (int i = 0; i < _ListOfSMResult.Count; i++)
                    {
                        if (LimitRecords != 0 && i >= LimitRecords)
                            break;

                        SMResult _SMResult = _ListOfSMResult[i];

                        _SMReportBuilder.Append("<tr>");
                        if (_SessionInformation != null && _SessionInformation.ClientGUID != null && !IsEmailOrDownload)
                        {
                            _SMReportBuilder.AppendFormat("<td class=\"center\"><img  onclick=\"ShowArticle('{0}','{1}','{2}');\" style=\"border:0;cursor:pointer;\" alt=\"\" src=\"../Images/NewsRead.png\" /></td>", _SMResult.link, _SMResult.id, Report_ReportType.SM.ToString());
                        }
                        else
                        {
                            _SMReportBuilder.AppendFormat("<td class=\"center\"><a target=\"_blank\" style=\"border:0;\" href=\"{0}\">View</a></td>", _SMResult.link);
                        }
                        _SMReportBuilder.AppendFormat("<td class=\"left\"><a target=\"_blank\"  href=\"{0}\" style=\"border:0;\" >{0}</a></td>", string.IsNullOrWhiteSpace(_SMResult.homeLink) ? "javascript:;" : _SMResult.homeLink);
                        _SMReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _SMResult.description);
                        _SMReportBuilder.AppendFormat("<td class=\"right\">{0}<br/>{1}</td>", Convert.ToDateTime(_SMResult.itemHarvestDate_DT).ToString("MM/dd/yyyy"), Convert.ToDateTime(_SMResult.itemHarvestDate_DT).ToString("hh:mm tt"));
                        _SMReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _SMResult.feedCategories);
                        _SMReportBuilder.AppendFormat("<td class=\"left\">{0}</td>", _SMResult.feedClass);
                        _SMReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", _SMResult.feedRank);

                        if (IsCompeteData)
                        {
                            string cUniqVisitorValue = string.Empty;
                            string iqAdshareValue = string.Empty;
                            cUniqVisitorValue = (_SMResult.C_uniq_visitor == null || !_SMResult.IsUrlFound) ? "NA" : string.Format("{0:n0}", _SMResult.C_uniq_visitor);
                            if ((_SMResult != null && (_SMResult.C_uniq_visitor == -1)))
                            {
                                cUniqVisitorValue = string.Empty;
                            }

                            iqAdshareValue = ((_SMResult.IQ_AdShare_Value == null || !_SMResult.IsUrlFound) ? "NA" : string.Format("{0:C}", _SMResult.IQ_AdShare_Value.Value));
                            if ((_SMResult != null && (_SMResult.IQ_AdShare_Value == -1)))
                            {
                                iqAdshareValue = string.Empty;
                            }

                            _SMReportBuilder.AppendFormat("<td class=\"right\">{0}&nbsp;&nbsp;{1}</td>", cUniqVisitorValue, (_SMResult.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\"  title=\"Powered by Compete\" />" : string.Empty));
                            //_SMReportBuilder.Append("<td class=\"right\"><table style=\"margin-top:-6px;\"><tr>");
                            //_SMReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", cUniqVisitorValue);//(_SMResult.C_uniq_visitor == null || !_SMResult.IsUrlFound) ? "NA" : string.Format("{0:n0}", _SMResult.C_uniq_visitor));
                            //_SMReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", (_SMResult.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\"  title=\"Powered by Compete\" />" : ""));
                            //_SMReportBuilder.Append("</tr></table></td>");
                            _SMReportBuilder.AppendFormat("<td class=\"right\">{0}</td>", iqAdshareValue);//((_SMResult.IQ_AdShare_Value == null || !_SMResult.IsUrlFound) ? "NA" : string.Format("{0:C}", _SMResult.IQ_AdShare_Value.Value)));
                        }
                        _SMReportBuilder.Append("</tr>");
                    }

                    _SMReportBuilder.Append("</table>");
                }
                else
                {
                    _SMReportBuilder.Append("<div class=\"padding5\">");
                    _SMReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width:100%\" id=\"gvSMReportSummary\" class=\"grid grid-iq\">");
                    _SMReportBuilder.Append("<tr><td colspan=\"2\">No Results Found</td></tr>");
                    _SMReportBuilder.Append("</table>");
                    _SMReportBuilder.Append("</div>");
                }
                _SMReportBuilder.Append("</div>");
                return _SMReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        private string GetTWReportHtml(bool IsEmailOrDownload = false, int LimitRecords = 0)
        {
            try
            {

                StringBuilder _TwitterReportBuilder = new StringBuilder();

                _TwitterReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportTWResult');\">");
                _TwitterReportBuilder.Append("Twitter");
                _TwitterReportBuilder.Append("</div>");
                _TwitterReportBuilder.Append("<div id=\"divReportTWResult\" class=\"clear\" style=\"word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;border:1px solid #999999;\">");

                List<TwitterResult> _ListOfTwitterResult = new List<TwitterResult>();

                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQTwitterReportResult))
                {
                    _ListOfTwitterResult = (List<TwitterResult>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTwitterReportResult, _ListOfTwitterResult.GetType());
                }

                if (_ListOfTwitterResult.Count > 0)
                {
                    _TwitterReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width: 100%;\">");
                    for (int i = 0; i < _ListOfTwitterResult.Count; i++)
                    {
                        if (LimitRecords != 0 && i >= LimitRecords)
                            break;

                        TwitterResult _TwitterResult = _ListOfTwitterResult[i];

                        _TwitterReportBuilder.Append("<tr>");

                        _TwitterReportBuilder.AppendFormat("<td>"
                                                      + "<div class=\"clear TweetInnerDiv\" id=\"datalistInner\">"
                                                         + "<div class=\"float-left TweetBodyDivIQP borderBoxSizing\">"
                                                             + "<div class=\"clear\">"
                                                                 + "<div class=\"float-left TweetActorDisplayName\">"
                                                                 + "<a target=\"_blank\" id=\"aActorLink\"  " + (!string.IsNullOrWhiteSpace(_TwitterResult.actor_link) ? "href=\"{0}\"" : string.Empty) + ">"
                                                                        + " <span id=\"lblDisplayName\">{1}</span>"
                                                                      + "</a>&nbsp;<span class=\"TweetSubdivFont\">@</span><span class=\"TweetSubdivFont\" id=\"lblPrefferedUserName\">{2}</span>"
                                                                     + "<br></div>"

                                                                 + "<div class=\"float-right\">"
                                                                     + "<div class=\"float-left TweetSubdivFont\">"
                                                                         + "Klout Score:&nbsp;"
                                                                          + "<span id=\"lblKloutScore\">{3}</span>&nbsp;&nbsp;&nbsp;&nbsp;</div>"
                                                                      + "<div class=\"float-left TweetSubdivFont\">Followers:&nbsp;"
                                                                          + "<span id=\"lblActorFollowers\">{4}</span>&nbsp;&nbsp;&nbsp;&nbsp;</div>"
                                                                     + "<div class=\"float-left TweetSubdivFont\">Friends:&nbsp;"
                                                                         + "<span id=\"lblActorFriends\">{5}</span></div></div></div>"

                                                             + "<div class=\"clear PaddingTopBottom1p TweetBodyText\">"
                                                                  + "<div class=\"div75pleft\">"
                                                                      + "<span id=\"lblTweetBody\">{6}</span></div>"

                                                                  + "<div class=\"TweetSubdivFont float-right\"><span id=\"lblPostedDateTime\">{7}</span></div></div></div>"

                                                         + "<div class=\"float-right IQPremiumTweetImageDiv center\">"
                                                              + "<a target=\"_blank\" id=\"aActorLinkimage\" href=\"{0}\">"
                                                                  + "<img style=\"border-width:0px;\" src=\"{8}\" id=\"cimgActor\"></a><br>"

                                                             , _TwitterResult.actor_link, _TwitterResult.actor_displayName, _TwitterResult.actor_prefferedUserName,
                                                             _TwitterResult.Klout_score, _TwitterResult.followers_count, _TwitterResult.friends_count, _TwitterResult.tweet_body,
                                                             _TwitterResult.tweet_postedDateTime, _TwitterResult.actor_image);

                        /*if (!IsEmailOrDownload)
                        {
                            _TwitterReportBuilder.AppendFormat("<a onclick=\"SaveTweet('{0}','TW')\" id=\"lnlSaveTweet\" class=\"cursor\">Save Tweet</a></div></div></td>", _TwitterResult.tweet_id);
                        }
                        else
                        {*/
                        _TwitterReportBuilder.Append("</div></div></td>");
                        //}
                        _TwitterReportBuilder.Append("</tr>");

                    }

                    _TwitterReportBuilder.Append("</table>");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "$(\"div[id='datalistInner']:last\").css(\"border\", \"none\");", true);
                }
                else
                {
                    _TwitterReportBuilder.Append("<div class=\"padding5\">");
                    _TwitterReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width:100%\" id=\"gvTwitterReportSummary\" class=\"grid grid-iq\">");
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
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        private string GetTWReportHtmlForEmail(bool IsEmailOrDownload = false, int LimitRecords = 0)
        {
            try
            {

                StringBuilder _TwitterReportBuilder = new StringBuilder();

                _TwitterReportBuilder.Append("<div class=\"reportHeader clear\" style=\"margin:5px 0px;text-indent:5px;font-weight:bold;\" onclick=\"ShowHideDiv('divReportTWResult');\">");
                _TwitterReportBuilder.Append("Twitter");
                _TwitterReportBuilder.Append("</div>");
                _TwitterReportBuilder.Append("<div id=\"divReportTWResult\" class=\"clear\" style=\"word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;border:1px solid #999999;\">");

                List<TwitterResult> _ListOfTwitterResult = new List<TwitterResult>();

                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQTwitterReportResult))
                {
                    _ListOfTwitterResult = (List<TwitterResult>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTwitterReportResult, _ListOfTwitterResult.GetType());
                }

                if (_ListOfTwitterResult.Count > 0)
                {
                    _TwitterReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"2\" border=\"0\" style=\"width: 100%;\">");
                    for (int i = 0; i < _ListOfTwitterResult.Count; i++)
                    {
                        if (LimitRecords != 0 && i >= LimitRecords)
                            break;

                        TwitterResult _TwitterResult = _ListOfTwitterResult[i];

                        _TwitterReportBuilder.Append("<tr>");

                        /* _TwitterReportBuilder.AppendFormat("<td>"
                             + (_ListOfTwitterResult.Count == i + 1 ? "<div style=\"clear: both;overflow: hidden;padding: 5px;\" id=\"datalistInner\">" : "<div style=\"clear: both;overflow: hidden;border-bottom: 1px solid #999999;padding: 5px;\">")
                                                         + "<div align=\"left\" style=\"float: left;width: 85%;vertical-align: top;box-sizing: border-box;-moz-box-sizing: border-box;-webkit-box-sizing: border-box;\">"
                                                             + "<div style=\"clear: both;overflow: hidden;\">"
                                                                 + "<div align=\"left\" style=\"float: left;font-size:14px;width:250px;line-height:20px;\">"
                                                                 + "<a target=\"_blank\"  " + (!string.IsNullOrWhiteSpace(_TwitterResult.actor_link) ? "href=\"{0}\"" : string.Empty) + ">"
                                                                        + " <div align=\"left\" style=\"float: left;margin-right: 5px;\">{1}</div>"
                                                                      + "</a>&nbsp;<div align=\"left\" style=\"float:left;\"><div align=\"left\" style=\"font-size: 11px; color: #999999;float: left;\">@</div><div align=\"left\" style=\"font-size: 11px; color: #999999;float: left;\">{2}</div></div>"
                                                                     + "<br></div>"

                                                                 + "<div align=\"left\" style=\"float: left;line-height:20px;\">"
                                                                     + "<div align=\"left\" style=\"float: left;font-size: 11px; color: #999999;\">"
                                                                         + "Klout Score:&nbsp;"
                                                                          + "<span>{3}</span>&nbsp;&nbsp;&nbsp;&nbsp;</div>"
                                                                      + "<div  align=\"left\" style=\"float: left;font-size: 11px; color: #999999;\">Followers:&nbsp;"
                                                                          + "<span>{4}</span>&nbsp;&nbsp;&nbsp;&nbsp;</div>"
                                                                     + "<div align=\"left\" style=\"float: left;font-size: 11px; color: #999999;\">Friends:&nbsp;"
                                                                         + "<span>{5}</span></div></div></div>"

                                                             + "<div style=\"clear: both; padding: 1% 0%;color: #535353;\">"
                                                                  + "<div align=\"left\" style=\" float: left;width: 75%;line-height:20px;\">"
                                                                      + "<span>{6}</span></div>"

                                                                  + "<div align=\"right\" style=\" font-size: 11px; color: #999999;float: right;line-height:20px;\"><span>{7}</span></div></div></div>"

                                                         + "<div align=\"right\" style=\"float: right;width: 12%;text-align: center;\">"
                                                              + "<a target=\"_blank\" href=\"{0}\">"
                                                                  + "<img style=\"border-width:0px;\" src=\"{8}\"></a><br>"

                                                             , _TwitterResult.actor_link, _TwitterResult.actor_displayName, _TwitterResult.actor_prefferedUserName,
                                                             _TwitterResult.Klout_score, _TwitterResult.followers_count, _TwitterResult.friends_count, _TwitterResult.tweet_body,
                                                             _TwitterResult.tweet_postedDateTime, _TwitterResult.actor_image);*/

                        _TwitterReportBuilder.AppendFormat("<td>"
                           + (LimitRecords == i + 1 ? "<table style=\"clear: both; overflow: hidden; width:100%;\">" : "<table style=\"clear: both; overflow: hidden; border-bottom: 1px solid #999999;width:100%;\">")
                                                       + "<tr><td align=\"left\" style=\"float: left; width: 85%; vertical-align: top; box-sizing: border-box; -moz-box-sizing: border-box; -webkit-box-sizing: border-box;\"><table style=\"width:100%;\"><tr><td style=\"float: left; font-size: 14px; width: 250px; line-height: 20px;width:300px;\"><table><tr><td>"

                                                               + "<a target=\"_blank\"  " + (!string.IsNullOrWhiteSpace(_TwitterResult.actor_link) ? "href=\"{0}\"" : string.Empty) + ">"
                                                                      + "{1}"
                                                                    + "</a>&nbsp;&nbsp;@{2}&nbsp;</td>"
                                                                   + "</tr></table></td>"

                                                               + "<td align=\"right\" style=\"line-height: 20px;width: 320px;\"><table><tr><td align=\"right\" style=\"font-size: 11px; color: #999999;width:90px;\">"
                                                                       + "Klout Score:&nbsp;{3}&nbsp;&nbsp;&nbsp;&nbsp;</td>"
                                                                    + "<td align=\"right\" style=\"font-size: 11px; color: #999999;width:101px;\">Followers:&nbsp; {4}&nbsp;&nbsp;&nbsp;&nbsp;</td>"

                                                                + "<td align=\"right\" style=\"font-size: 11px; color: #999999;width:78px;\">Friends:&nbsp; {5}</td>"

                                                                + "</tr></table></td></tr>"

                                                                + "<tr><td style=\"float: left; line-height: 20px;\"><span>{6}</span></td><td align=\"right\" style=\"font-size: 11px; color: #999999; line-height: 20px;\"><span>{7}</span></td></tr></table></td>"
                                                           + "<td style=\"float: right; width: 12%; text-align: center;\"><a target=\"_blank\" href=\"{0}\"><img style=\"border-width: 0px;\" src=\"{8}\"></a><br>"

                                                           , _TwitterResult.actor_link, _TwitterResult.actor_displayName, _TwitterResult.actor_prefferedUserName,
                                                           _TwitterResult.Klout_score, _TwitterResult.followers_count, _TwitterResult.friends_count, _TwitterResult.tweet_body,
                                                           _TwitterResult.tweet_postedDateTime, _TwitterResult.actor_image);
                        _TwitterReportBuilder.Append("</td></tr></table>");

                        _TwitterReportBuilder.Append("</tr>");

                    }

                    _TwitterReportBuilder.Append("</table>");
                }
                else
                {
                    _TwitterReportBuilder.Append("<div style=\"padding:5px;\">");
                    _TwitterReportBuilder.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" style=\"width:100%\" id=\"gvTwitterReportSummary\" class=\"grid grid-iq\">");
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
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        private string GetTVReportCsv()
        {
            try
            {

                List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();
                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQTVReportResult))
                    _ListOfIQAgentResults = (List<IQAgentResults>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTVReportResult, _ListOfIQAgentResults.GetType());

                StringBuilder _TVReportBuilder = new StringBuilder();

                _TVReportBuilder.Append("TV\r\n");

                if (_ListOfIQAgentResults.Count > 0)
                {
                    if (_SessionInformation.IsNielSenData)
                    {
                        _TVReportBuilder.Append("Program,DateTime,Market,Audience,iQ Media Value,Hits");
                    }
                    else
                    {
                        _TVReportBuilder.Append("Program,DateTime,Market,Hits");
                    }
                    _TVReportBuilder.Append("\r\n");

                    foreach (IQAgentResults _IQAgentResults in _ListOfIQAgentResults)
                    {
                        _TVReportBuilder.AppendFormat("\"{0}\",", _IQAgentResults.IQAgentResultUrl);
                        _TVReportBuilder.AppendFormat("\"{0}\",", _IQAgentResults.Title120.Replace("\"", "\"\""));
                        _TVReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:MM/dd/yyyy hh:mm tt}", _IQAgentResults.RL_Date));
                        _TVReportBuilder.AppendFormat("\"{0}\",", _IQAgentResults.RL_Market);
                        if (_SessionInformation.IsNielSenData)
                        {
                            _TVReportBuilder.AppendFormat("\"{0}\"", _IQAgentResults.AUDIENCE == null ? "NA" : string.Format("{0:n0}", _IQAgentResults.AUDIENCE));
                            _TVReportBuilder.AppendFormat("\"{0}\"", (string.IsNullOrWhiteSpace(_IQAgentResults.SQAD_SHAREVALUE) ? "NA" : _IQAgentResults.SQAD_SHAREVALUE));
                        }
                        _TVReportBuilder.AppendFormat("\"{0}\"", _IQAgentResults.Number_Hits);
                        _TVReportBuilder.Append("\r\n");
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
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        private string GetNEWSReportCsv()
        {
            try
            {

                List<NewsResult> _ListOfNewsResult = new List<NewsResult>();

                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQNewsReportResult))
                    _ListOfNewsResult = (List<NewsResult>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQNewsReportResult, _ListOfNewsResult.GetType());

                StringBuilder _NewsReportBuilder = new StringBuilder();

                _NewsReportBuilder.Append("NEWS\r\n");

                if (_ListOfNewsResult.Count > 0)
                {
                    if (IsCompeteData)
                    {
                        _NewsReportBuilder.Append("Article,Publication,Title,Date Time,Category,Genre,Audience,iQ Media Value");
                    }
                    else
                    {
                        _NewsReportBuilder.Append("Article,Publication,Title,Date Time,Category,Genre");
                    }
                    _NewsReportBuilder.Append("\r\n");

                    foreach (NewsResult _NewsResult in _ListOfNewsResult)
                    {

                        _NewsReportBuilder.AppendFormat("\"{0}\",", _NewsResult.Article);
                        _NewsReportBuilder.AppendFormat("\"{0}\",", _NewsResult.publication);
                        _NewsReportBuilder.AppendFormat("\"{0}\",", _NewsResult.Title.Replace("\"", "\"\""));
                        _NewsReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:MM/dd/yyyy hh:mm tt}", _NewsResult.date));
                        _NewsReportBuilder.AppendFormat("\"{0}\",", _NewsResult.Category);
                        _NewsReportBuilder.AppendFormat("\"{0}\"", _NewsResult.Genre);
                        if (IsCompeteData)
                        {
                            string cUniqVisitorValue = string.Empty;
                            string iqAdshareValue = string.Empty;
                            cUniqVisitorValue = ((_NewsResult.C_uniq_visitor == null || !_NewsResult.IsUrlFound) ? "NA" : string.Format("{0:n0}", _NewsResult.C_uniq_visitor));
                            if ((_NewsResult != null && (_NewsResult.C_uniq_visitor == -1)))
                            {
                                cUniqVisitorValue = string.Empty;
                            }

                            iqAdshareValue = ((_NewsResult.IQ_AdShare_Value == null || !_NewsResult.IsUrlFound) ? "NA" : string.Format("{0:C}", _NewsResult.IQ_AdShare_Value.Value));
                            if ((_NewsResult != null && (_NewsResult.IQ_AdShare_Value == -1)))
                            {
                                iqAdshareValue = string.Empty;
                            }

                            _NewsReportBuilder.AppendFormat("\"{0}{1}\",", cUniqVisitorValue, (_NewsResult.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\"  title=\"Powered by Compete\" />" : ""));
                            _NewsReportBuilder.AppendFormat("\"{0}\"", iqAdshareValue);
                        }
                        _NewsReportBuilder.Append("\r\n");
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
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        private string GetSMReportCsv()
        {
            try
            {

                List<SMResult> _ListOfSMResult = new List<SMResult>();

                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQSMReportResult))
                    _ListOfSMResult = (List<SMResult>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQSMReportResult, _ListOfSMResult.GetType());

                StringBuilder _SMReportBuilder = new StringBuilder();

                _SMReportBuilder.Append("Social Media\r\n");

                if (_ListOfSMResult.Count > 0)
                {
                    if (IsCompeteData)
                    {
                        _SMReportBuilder.Append("Article,Publication,Title,Date Time,Category,Type,Rank,Audience,iQ Media Value");
                    }
                    else
                    {
                        _SMReportBuilder.Append("Article,Publication,Title,Date Time,Category,Type,Rank");
                    }
                    _SMReportBuilder.Append("\r\n");

                    foreach (SMResult _SMResult in _ListOfSMResult)
                    {
                        _SMReportBuilder.AppendFormat("\"{0}\",", _SMResult.link);
                        _SMReportBuilder.AppendFormat("\"{0}\",", _SMResult.homeLink);
                        _SMReportBuilder.AppendFormat("\"{0}\",", _SMResult.description.Replace("\"", "\"\""));
                        _SMReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:MM/dd/yyyy hh:mm tt}", _SMResult.itemHarvestDate_DT));
                        _SMReportBuilder.AppendFormat("\"{0}\",", _SMResult.feedCategories);
                        _SMReportBuilder.AppendFormat("\"{0}\",", _SMResult.feedClass);
                        _SMReportBuilder.AppendFormat("\"{0}\"", _SMResult.feedRank);
                        if (IsCompeteData)
                        {
                            string cUniqueVisitorValue = string.Empty;
                            string iqAdshareValue = string.Empty;
                            cUniqueVisitorValue = ((_SMResult.C_uniq_visitor == null || !_SMResult.IsUrlFound) ? "NA" : string.Format("{0:n0}", _SMResult.C_uniq_visitor));
                            if ((_SMResult != null && (_SMResult.C_uniq_visitor == -1)))
                            {
                                cUniqueVisitorValue = string.Empty;
                            }

                            iqAdshareValue = ((_SMResult.IQ_AdShare_Value == null || !_SMResult.IsUrlFound) ? "NA" : string.Format("{0:C}", _SMResult.IQ_AdShare_Value.Value));
                            if ((_SMResult != null && (_SMResult.IQ_AdShare_Value == -1)))
                            {
                                iqAdshareValue = string.Empty;
                            }

                            _SMReportBuilder.AppendFormat("\"{0}{1}\",", cUniqueVisitorValue, (_SMResult.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\"  title=\"Powered by Compete\" />" : ""));
                            _SMReportBuilder.AppendFormat("\"{0}\"", iqAdshareValue);
                        }
                        _SMReportBuilder.Append("\r\n");
                    }
                    _SMReportBuilder.Append("\r\n");
                }
                else
                {
                    _SMReportBuilder.Append("No Results Found");
                    _SMReportBuilder.Append("\r\n");
                }
                return _SMReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        private string GetTWReportCsv()
        {
            try
            {

                List<TwitterResult> _ListOfTwitterResult = new List<TwitterResult>();

                if (!string.IsNullOrEmpty(_ViewstateInformation.MyIQTwitterReportResult))
                    _ListOfTwitterResult = (List<TwitterResult>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTwitterReportResult, _ListOfTwitterResult.GetType());

                StringBuilder _TWReportBuilder = new StringBuilder();

                _TWReportBuilder.Append("Social Media\r\n");

                if (_ListOfTwitterResult.Count > 0)
                {
                    _TWReportBuilder.Append("Actor Link,Actor Display Name,Actor Preffered Name,Klout Score,Followers Count,Friends Count,Body,Tweet Posted Datetime,Actor Image,Tweet ID");
                    _TWReportBuilder.Append("\r\n");

                    foreach (TwitterResult _TwitterResult in _ListOfTwitterResult)
                    {
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.actor_link);
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.actor_displayName);
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.actor_prefferedUserName);
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.Klout_score);
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.followers_count);
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.friends_count);
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.tweet_body);
                        _TWReportBuilder.AppendFormat("\"{0}\",", string.Format("{0:MM/dd/yyyy hh:mm tt}", _TwitterResult.tweet_postedDateTime));
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.actor_image);
                        _TWReportBuilder.AppendFormat("\"{0}\",", _TwitterResult.tweet_id);

                        _TWReportBuilder.Append("\r\n");


                    }
                    _TWReportBuilder.Append("\r\n");
                }
                else
                {
                    _TWReportBuilder.Append("No Results Found");
                    _TWReportBuilder.Append("\r\n");
                }
                return _TWReportBuilder.ToString();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
                return string.Empty;
            }
        }

        #endregion

        #region Email

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
                    EmailContent = GenerateEmailHTML();
                    foreach (string mailAddress in mailAddresses)
                    {
                        if (mailAddress.Length != 0)
                        {
                            if (validateEmails(mailAddress))
                            {
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
                    BindReportFromReportType();
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
                    lblReportMsg.Text = "Email Sent Successfully.";
                    lblReportMsg.ForeColor = System.Drawing.Color.Green;
                    upReport.Update();
                    ClearEmailFrom();
                }

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblError.Text = "Some Error Occured, Please Try Again!!";
            }
        }

        protected void ClearEmailFrom()
        {
            txtFriendsEmail.Text = "";
            //txtYourEmail.Text = "";
            txtMessage.Text = "";
            txtSubject.Text = "";
        }

        /// <summary>
        /// This Function  Validates the User Email Address
        /// </summary>
        /// <param name="_UserEmail">Contains User's Email Address</param>
        /// <returns>True if validate else false</returns>
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

        public string GenerateEmailHTML()
        {
            try
            {
                _ViewstateInformation = GetViewstateInformation();
                var sb = new StringBuilder();

                sb.Append("<html>");
                sb.Append("<head>");
                /*sb.AppendFormat("<link href=\"http://{0}/Css/my-style.css\" rel=\"stylesheet\" type=\"text/css\" />", Request.ServerVariables["HTTP_HOST"]);
                sb.AppendFormat("<link href=\"http://{0}/Css/fonts/stylesheet.css\" rel=\"stylesheet\" type=\"text/css\" />", Request.ServerVariables["HTTP_HOST"]);*/
                StreamReader style1 = new StreamReader(Server.MapPath("/Css/my-style.css"));
                string style1data = style1.ReadToEnd();
                sb.Append("<style type=\"text/css\">" + style1data + "</style>");
                sb.Append("</head>");
                sb.Append("<body>");

                sb.AppendFormat("<a href=\"{0}{1}\" >", ConfigurationManager.AppSettings["ReportUrl"], HttpUtility.UrlEncode(hfReportID.Value));
                divReportHeader.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                sb.Append("</a>");

                if (_ViewstateInformation.IsIQAgentTVResultShow)
                {
                    sb.Append(GetTVReportHtml(true, _ViewstateInformation.TVReportNoOfRecordsDisplayInEmail));
                }
                if (_ViewstateInformation.IsIQAgentNMResultShow)
                {
                    sb.Append("<br />");
                    sb.Append(GetNEWSReportHtml(true, _ViewstateInformation.NMReportNoOfRecordsDisplayInEmail));
                }
                if (_ViewstateInformation.IsIQAgentSMResultShow)
                {
                    sb.Append("<br />");
                    sb.Append(GetSMReportHtml(true, _ViewstateInformation.SMReportNoOfRecordsDisplayInEmail));
                }

                if (_ViewstateInformation.IsIQAgentTwitterResultShow)
                {
                    sb.Append("<br />");
                    sb.Append(GetTWReportHtmlForEmail(true, _ViewstateInformation.TWReportNoOfRecordsDisplayInEmail));
                }

                sb.Append("</body>");
                sb.Append("</html>");

                return Convert.ToString(sb);
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        #endregion

        #region Download
        protected void btnReportPdfDownload_Click(object sender, EventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<head>");
                sb.Append("<link href=\"../../Css/my-style.css\" rel=\"stylesheet\" type=\"text/css\" />");
                sb.Append("<link href=\"../../Css/fonts/stylesheet.css\" rel=\"stylesheet\" type=\"text/css\" />");
                //sb.Append("<script src=\"../../Script/jquery-1.8.3.min.js\" type=\"text/javascript\"></script>");
                //sb.Append("<script src=\"../../Script/wkhtmltopdf.tablesplit.js\" type=\"text/javascript\"></script>");
                sb.Append("</head>");
                sb.Append("<body>");
                divReportHeader.RenderControl(new HtmlTextWriter(new StringWriter(sb)));
                if (_ViewstateInformation.IsIQAgentTVResultShow == true)
                { sb.Append(GetTVReportHtml(true)); }
                if (_ViewstateInformation.IsIQAgentNMResultShow == true)
                { sb.Append(GetNEWSReportHtml(true)); }
                if (_ViewstateInformation.IsIQAgentSMResultShow == true)
                { sb.Append(GetSMReportHtml(true)); }
                if (_ViewstateInformation.IsIQAgentTwitterResultShow == true)
                { sb.Append(GetTWReportHtml(true)); }
                //sb.Append("<script type=\"text/javascript\">var pdfPage = {width: 8.26, height: 11.79, margins: {top: 0.393701, left: 0.393701, right: 0.393701, bottom: 0.393701 }};var splitThreshold = 33;var splitClassName = 'grid-iq';$(window).load(function () {var dpi = $('<div id=\"dpi\"></div>').css({height: '1in', width: '1in',top: '-100%', left: '-100%',position: 'absolute'}).appendTo('body').height();var pageHeight = Math.ceil((pdfPage.height - pdfPage.margins.top - pdfPage.margins.bottom) * dpi);var $body = $('body');var tablesModified = true;var offsetCorrection = 0;while (tablesModified) {tablesModified = false;$('table.'+splitClassName).each(function(){var $t = $(this);var copy = $t.clone();copy.find('tbody > tr').remove();var $cbody = copy.find('tbody');var found = false;$t.removeClass(splitClassName); var newOffsetCorrection = offsetCorrection;$('tbody tr', $t).each(function(){var $tr = $(this);var top = $tr.offset().top;var ctop = offsetCorrection + top;var pageEnd = (Math.floor(ctop/pageHeight)+1)*pageHeight;if (ctop >= (pageEnd - splitThreshold)) {$tr.detach().appendTo($cbody);if (!found) {newOffsetCorrection += (pageEnd - ctop);}found = true;}});if (!found) return;offsetCorrection = newOffsetCorrection;tablesModified = true;var $br = $('<div style=\"height: 10px;\"></div>').css('page-break-before', 'always');$br.insertAfter($t);copy.insertAfter($br);});}});</script>");
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
                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + lblReportHeader.Text.Replace("/", "-") + ".pdf" + "\"");

                    // Set the ContentType
                    Response.ContentType = "Application/pdf";

                    // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                    Response.WriteFile(OutputFile);

                    // End the response
                    Response.End();
                }
                else
                {
                    lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                    lblReportMsg.ForeColor = System.Drawing.Color.Red;
                }

            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnReportCsvDownload_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder _DownloarStr = new StringBuilder();

                _DownloarStr.AppendFormat("{0}", lblReportHeader.Text);
                _DownloarStr.Append("\r\n");

                if (_ViewstateInformation.IsIQAgentTVResultShow)
                {
                    _DownloarStr.Append(GetTVReportCsv());
                }

                if (_ViewstateInformation.IsIQAgentNMResultShow)
                {
                    _DownloarStr.Append(GetNEWSReportCsv());
                }

                if (_ViewstateInformation.IsIQAgentSMResultShow)
                {
                    _DownloarStr.Append(GetSMReportCsv());
                }

                if (_ViewstateInformation.IsIQAgentTwitterResultShow)
                {
                    _DownloarStr.Append(GetTWReportCsv());
                }

                Response.ClearContent();

                // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + lblReportHeader.Text.Replace("/", "-") + ".csv" + "\"");

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
                lblReportMsg.Text = "Some Error Occured, Please Try Again!!";
                lblReportMsg.ForeColor = System.Drawing.Color.Red;
            }
        }
        #endregion

        #region Save and Print Article
        protected void btnSaveArticle_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnArticleType.Value == Report_ReportType.NM.ToString())
                {

                    IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();
                    ArchiveNM _ArchiveNM = new ArchiveNM();


                    Guid? _NullCategoryGUID = null;

                    _ArchiveNM.Title = txtArticleTitle.Text;
                    _ArchiveNM.Keywords = txtKeywords.Text;
                    _ArchiveNM.Description = txtADescription.Text;
                    _ArchiveNM.CustomerGuid = new Guid(_SessionInformation.CustomerGUID);
                    _ArchiveNM.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                    _ArchiveNM.CategoryGuid = new Guid(ddlPCategory.SelectedValue);
                    _ArchiveNM.SubCategory1Guid = ddlSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory1.SelectedValue);
                    _ArchiveNM.SubCategory2Guid = ddlSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory2.SelectedValue);
                    _ArchiveNM.SubCategory3Guid = ddlSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory3.SelectedValue);
                    _ArchiveNM.ArticleID = hfarticleID.Value;

                    _ArchiveNM.Rating = Convert.ToInt16(txtArticleRate.Text);


                    Uri newsSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrNewsUrl]);
                    SearchEngine _SearchEngine = new SearchEngine(newsSearchURI);
                    SearchNewsRequest _SearchNewsRequest = new SearchNewsRequest();
                    _SearchNewsRequest.IsShowContent = true;
                    _SearchNewsRequest.IDs = new List<String> { "_" + _ArchiveNM.ArticleID };
                    SearchNewsResults _searchNewsResults = _SearchEngine.SearchNews(_SearchNewsRequest);
                    if (_searchNewsResults.newsResults != null && _searchNewsResults.newsResults.Count > 0)
                    {
                        _ArchiveNM.Content = _searchNewsResults.newsResults[0].Content;
                        _ArchiveNM.Harvest_Time = Convert.ToDateTime(_searchNewsResults.newsResults[0].date);
                        _ArchiveNM.Url = _searchNewsResults.newsResults[0].Article;
                    }

                    string _Result = _IArchiveNMController.InsertArchiveNM(_ArchiveNM);

                    if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) <= 0)
                    {
                        if (Convert.ToInt32(_Result) == -1)
                        {
                            lblSaveArticleMsg.Text = "Article is already saved.";
                        }
                        else
                        {
                            lblSaveArticleMsg.Text = "An error occur, please try again.";
                        }
                        //mdlpopupSaveArticle.Show();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticleModal", "ShowModal('pnlSaveArticle');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Article Saved Successfully');", true);
                        var newsGeneratePDFsvc = new NewsGeneratePDFWebServiceClient();
                        newsGeneratePDFsvc.WakeupService();
                        //mdlpopupSaveArticle.Hide();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticleModal", "closeModal('pnlSaveArticle');", true);

                        //lblNewsMsg.Text = "Article Saved Successfully.";
                        //upOnlineNews.Update();
                    }
                }

                if (hdnArticleType.Value == Report_ReportType.SM.ToString())
                {

                    ISMController _ISMController = _ControllerFactory.CreateObject<ISMController>();
                    ArchiveSM _ArchiveSM = new ArchiveSM();


                    Guid? _NullCategoryGUID = null;

                    _ArchiveSM.Title = txtArticleTitle.Text;
                    _ArchiveSM.Keywords = txtKeywords.Text;
                    _ArchiveSM.Description = txtADescription.Text;
                    _ArchiveSM.CustomerGuid = new Guid(_SessionInformation.CustomerGUID);
                    _ArchiveSM.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                    _ArchiveSM.CategoryGuid = new Guid(ddlPCategory.SelectedValue);
                    _ArchiveSM.SubCategory1Guid = ddlSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory1.SelectedValue);
                    _ArchiveSM.SubCategory2Guid = ddlSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory2.SelectedValue);
                    _ArchiveSM.SubCategory3Guid = ddlSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory3.SelectedValue);
                    _ArchiveSM.ArticleID = hfarticleID.Value;

                    _ArchiveSM.Rating = Convert.ToInt16(txtArticleRate.Text);

                    Uri newsSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrSMUrl]);
                    SearchEngine _SearchEngine = new SearchEngine(newsSearchURI);
                    SearchSMRequest _SearchSMRequest = new SearchSMRequest();
                    _SearchSMRequest.isShowContent = true;
                    _SearchSMRequest.ids = new List<String> { _ArchiveSM.ArticleID };
                    SearchSMResult _SearchSMResult = _SearchEngine.SearchSocialMedia(_SearchSMRequest);
                    if (_SearchSMResult.smResults != null && _SearchSMResult.smResults.Count > 0)
                    {
                        _ArchiveSM.Content = _SearchSMResult.smResults[0].content;
                        _ArchiveSM.Url = _SearchSMResult.smResults[0].link;
                        _ArchiveSM.Harvest_Time = Convert.ToDateTime(_SearchSMResult.smResults[0].itemHarvestDate_DT);
                    }

                    string _Result = _ISMController.InsertArchiveSM(_ArchiveSM);

                    if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) <= 0)
                    {
                        if (Convert.ToInt32(_Result) == -1)
                        {
                            lblSaveArticleMsg.Text = "Article is already saved.";
                        }
                        else
                        {
                            lblSaveArticleMsg.Text = "An error occur, please try again.";
                        }
                        //mdlpopupSaveArticle.Show();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticleModal", "ShowModal('pnlSaveArticle');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Article Saved Successfully');", true);
                        var socialGeneratePDFsvc = new SocialGeneratePDFWebServiceClient();
                        socialGeneratePDFsvc.WakeupService();
                        //mdlpopupSaveArticle.Hide();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticleModal", "closeModal('pnlSaveArticle');", true);
                    }
                }

                if (hdnArticleType.Value == Report_ReportType.TW.ToString())
                {
                    List<TwitterResult> _ListOfTwitterResult = new List<TwitterResult>();

                    if (!string.IsNullOrWhiteSpace(_ViewstateInformation.MyIQTwitterReportResult))
                    {
                        _ListOfTwitterResult = (List<TwitterResult>)CommonFunctions.DeserializeJson(_ViewstateInformation.MyIQTwitterReportResult, _ListOfTwitterResult.GetType());
                    }

                    TwitterResult twitterResult = null;
                    if (!string.IsNullOrWhiteSpace(hfarticleID.Value))
                    {
                        if (_ListOfTwitterResult != null && _ListOfTwitterResult.Count > 0)
                        {
                            twitterResult = _ListOfTwitterResult.Where(tw => tw.tweet_id.Equals(Convert.ToInt64(hfarticleID.Value))).FirstOrDefault();
                        }

                        if (twitterResult != null)
                        {
                            ITwitterController _ITwitterController = _ControllerFactory.CreateObject<ITwitterController>();
                            ArchiveTweets _ArchiveTweets = new ArchiveTweets();

                            Guid? _NullCategoryGUID = null;

                            _ArchiveTweets.Title = txtArticleTitle.Text;
                            _ArchiveTweets.Keywords = txtKeywords.Text;
                            _ArchiveTweets.Description = txtADescription.Text;
                            _ArchiveTweets.CustomerGuid = new Guid(_SessionInformation.CustomerGUID);
                            _ArchiveTweets.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                            _ArchiveTweets.CategoryGuid = new Guid(ddlPCategory.SelectedValue);
                            _ArchiveTweets.SubCategory1Guid = ddlSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory1.SelectedValue);
                            _ArchiveTweets.SubCategory2Guid = ddlSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory2.SelectedValue);
                            _ArchiveTweets.SubCategory3Guid = ddlSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory3.SelectedValue);
                            _ArchiveTweets.Tweet_ID = Convert.ToInt64(hfarticleID.Value);
                            _ArchiveTweets.Actor_DisplayName = twitterResult.actor_displayName;
                            _ArchiveTweets.Actor_PreferredUserName = twitterResult.actor_prefferedUserName;
                            _ArchiveTweets.Actor_FollowersCount = twitterResult.followers_count;
                            _ArchiveTweets.Actor_FriendsCount = twitterResult.friends_count;
                            _ArchiveTweets.Actor_link = twitterResult.actor_link;
                            _ArchiveTweets.Tweet_Body = twitterResult.tweet_body; ;
                            _ArchiveTweets.Actor_Image = twitterResult.actor_image;
                            _ArchiveTweets.Tweet_PostedDateTime = Convert.ToDateTime(twitterResult.tweet_postedDateTime);
                            _ArchiveTweets.gnip_Klout_Score = twitterResult.Klout_score;
                            _ArchiveTweets.Rating = Convert.ToInt16(txtArticleRate.Text);

                            string _Result = _ITwitterController.InsertArchiveTweet(_ArchiveTweets);

                            if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) <= 0)
                            {
                                if (Convert.ToInt32(_Result) == -1)
                                {
                                    lblSaveArticleMsg.Text = "Tweet is already saved.";
                                }
                                else
                                {
                                    lblSaveArticleMsg.Text = "An error occur, please try again.";
                                }
                                lblSaveArticleMsg.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticleModal", "ShowModal('pnlSaveArticle');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Tweet Saved Successfully');", true);
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticleModal", "closeModal('pnlSaveArticle');", true);
                                //SetControlforPopUp(1);
                            }

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "displayTwitterTab();", true);
                        }
                        else
                        {
                            lblSaveArticleMsg.Text = "An error occur, please try again.";
                        }
                    }
                    else
                    {
                        lblSaveArticleMsg.Text = "An error occur, please try again.";
                    }
                }
                BindReportFromReportType();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                lblSaveArticleMsg.Text = "An error occur, please try again.";
            }
        }

        private void BindMediaCategoryDropDown()
        {

            try
            {
                if (_SessionInformation != null && _SessionInformation.ClientGUID != null)
                {
                    string _ClientGUID = _SessionInformation.ClientGUID;
                    List<CustomCategory> _ListofCustomCategory = new List<CustomCategory>();

                    ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                    _ListofCustomCategory = _ICustomCategoryController.SelectByClientGUID(new Guid(_ClientGUID));

                    if (_ListofCustomCategory != null && _ListofCustomCategory.Count > 0)
                    {
                        /*ddlCategory.DataTextField = "CategoryName";
                        ddlCategory.DataValueField = "CategoryGUID";
                        ddlCategory.DataSource = _ListofCustomCategory;
                        ddlCategory.DataBind();
                        ddlCategory.Items.Insert(0, new ListItem("Select Category", "0"));*/

                        if (_SessionInformation.IsiQPremiumSM || _SessionInformation.IsiQPremiumNM)
                        {
                            ddlPCategory.DataTextField = "CategoryName";
                            ddlPCategory.DataValueField = "CategoryGUID";
                            ddlPCategory.DataSource = _ListofCustomCategory;
                            ddlPCategory.DataBind();

                            ddlPCategory.Items.Insert(0, new ListItem("<Blank>", "0"));

                            ddlSubCategory1.DataTextField = "CategoryName";
                            ddlSubCategory1.DataValueField = "CategoryGUID";
                            ddlSubCategory1.DataSource = _ListofCustomCategory;
                            ddlSubCategory1.DataBind();

                            ddlSubCategory1.Items.Insert(0, new ListItem("<Blank>", "0"));

                            ddlSubCategory2.DataTextField = "CategoryName";
                            ddlSubCategory2.DataValueField = "CategoryGUID";
                            ddlSubCategory2.DataSource = _ListofCustomCategory;
                            ddlSubCategory2.DataBind();

                            ddlSubCategory2.Items.Insert(0, new ListItem("<Blank>", "0"));

                            ddlSubCategory3.DataTextField = "CategoryName";
                            ddlSubCategory3.DataValueField = "CategoryGUID";
                            ddlSubCategory3.DataSource = _ListofCustomCategory;
                            ddlSubCategory3.DataBind();

                            ddlSubCategory3.Items.Insert(0, new ListItem("<Blank>", "0"));
                        }


                    }
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        #endregion

        #region Play Video

        protected void PlayRawMedia(string p_RawMediaID)
        {
            try
            {
                /* if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]))
                 {
                     ClipFrame.Attributes.Add("src", "http://localhost:2281/IFrameRawMediaH/Default.aspx?RawMediaID=" + e.CommandArgument.ToString() + "&SearchTerm=" + txtSearch.Text + "&IsUGC=false");
                 }
                 else
                 {
                     ClipFrame.Attributes.Add("src", "http://" + Request.Url.Host.ToString() + "/IFrameRawMediaH/Default.aspx?RawMediaID=" + e.CommandArgument.ToString() + "&SearchTerm=" + HttpUtility.UrlEncode(txtSearch.Text) + "&IsUGC=false");
                 }

                 ClipFrame.Visible = true;*/

                _ViewstateInformation = GetViewstateInformation();

                IframeRawMediaH.RawMediaID = new Guid(p_RawMediaID);
                IframeRawMediaH.IsUGC = false;
                IframeRawMediaH.SearchTerm = _ViewstateInformation._ClipSearchTerm;

                IframeRawMediaH.InitializePlayer();
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "SetWidthOfPlayerPopup", "SetPlayerPopupWidth();", true);

                BindReportFromReportType();

                upVideo.Update();

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPlayerPopup", "ShowModal('diviframe');", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        private void BindReportFromReportType()
        {
            try
            {


                if (_ViewstateInformation.IsIQAgentTVResultShow)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
                    div.TagName = CommonConstants.HTMLDiv;
                    div.InnerHtml = GetTVReportHtml();
                    divTVReport.Controls.Add(div);
                }
                if (_ViewstateInformation.IsIQAgentNMResultShow)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
                    div.TagName = CommonConstants.HTMLDiv;
                    div.InnerHtml = GetNEWSReportHtml();
                    divNewsReport.Controls.Add(div);
                }

                if (_ViewstateInformation.IsIQAgentSMResultShow)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
                    div.TagName = CommonConstants.HTMLDiv;
                    div.InnerHtml = GetSMReportHtml();
                    divSocialReport.Controls.Add(div);
                }

                if (_ViewstateInformation.IsIQAgentTwitterResultShow)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
                    div.TagName = CommonConstants.HTMLDiv;
                    div.InnerHtml = GetTWReportHtml();
                    divSocialReport.Controls.Add(div);
                }


                upReport.Update();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private Int32 GetNoOfRecordToDisplayInEmail(XElement xelem)
        {
            try
            {
                if (xelem.Element("NoOfRecordsToDisplayInEmail") == null)
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["NoOfRecordsToDisplayInEmailDefault"]);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(xelem.Element("NoOfRecordsToDisplayInEmail").Value))
                    {
                        return Convert.ToInt32(xelem.Element("NoOfRecordsToDisplayInEmail").Value);
                    }
                    else
                    {
                        return Convert.ToInt32(ConfigurationManager.AppSettings["NoOfRecordsToDisplayInEmailDefault"]);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private Int32 GetNoOfRecordToDisplay(XElement xelem)
        {
            try
            {
                if (xelem.Element("NoOfRecordsToDisplay") == null)
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["NoOfRecordsToDisplayDefault"]);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(xelem.Element("NoOfRecordsToDisplay").Value))
                    {
                        return Convert.ToInt32(xelem.Element("NoOfRecordsToDisplay").Value);
                    }
                    else
                    {
                        return Convert.ToInt32(ConfigurationManager.AppSettings["NoOfRecordsToDisplayDefault"]);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void SetControlforPopUp(int popupType)
        {
            /*//vlSummerySaveArticle.Visible = false;
            lblSaveArticleMsg.Visible = false;
            txtArticleTitle.Text = string.Empty;
            txtADescription.Text = string.Empty;
            txtKeywords.Text = string.Empty;
            ddlPCategory.SelectedIndex = 0;
            ddlSubCategory1.SelectedIndex = 0;
            ddlSubCategory2.SelectedIndex = 0;
            ddlSubCategory3.SelectedIndex = 0;
            txtArticleRate.Text = "1";
            chkArticlePreferred.Checked = false;
            if (popupType == 1)
            {
                spnSaveArticleTitle.InnerText = "Tweet Details";
            }
            else
            {
                spnSaveArticleTitle.InnerText = "Article Details";
            }*/
        }
    }
}
