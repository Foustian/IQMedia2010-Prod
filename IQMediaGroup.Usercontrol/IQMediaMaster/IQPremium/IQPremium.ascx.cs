using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using System.Configuration;
using PMGSearch;
using System.Xml;
using System.IO;
using IQMediaGroup.Core.Enumeration;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;
using System.Text;
using System.Security.Policy;
using System.Net;
using InfoSoftGlobal;
using System.Text.RegularExpressions;
using System.Data;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Usercontrol.Base;
using System.Threading;
using System.Runtime.Serialization;
using System.Data.SqlTypes;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IQPremium
{
    //public class affiliateZoomResponse { public string solrResponse { get; set; } public string name { get; set; } }
    public class affiliateMsLineResponse { public string solrResponse { get; set; } public string name { get; set; } }

    public partial class IQPremium : BaseControl
    {
        #region Variable

        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        SSP_IQ_Dma_Name marketNational;

        string chartWidth = "100%";
        string chartHeight = "550";
        //  List<affiliateZoomResponse> listOfAffiliateResponse = null;
        ViewstateInformation _viewstateInformation = null;
        List<ChartZoomHistory> lstChartZoomHistory = null;
        SessionInformation _SessionInformation;


        #endregion

        #region Page Events

        protected override void OnLoad(EventArgs e)
        {
            _viewstateInformation = GetViewstateInformation();
            lblmsg.Text = string.Empty;//modal popup of Save search's label
            lblmsg.Visible = false;

            lblSavedSearchmsg.Visible = false; // saved search msg label
            lblSavedSearchmsg.Text = String.Empty;
            lblIQAgentmsg.Visible = false;

            ScriptManager _ToolkitScriptManager = Page.Master.FindControl("ScriptManager1") as ScriptManager;
            if (_ToolkitScriptManager != null)
                _ToolkitScriptManager.AsyncPostBackTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["IQPremiumPageTimeout"]);

            _SessionInformation = CommonFunctions.GetSessionInformation();

            if (!IsPostBack)
            {
                //hfcurrentTab.Value = string.Empty;
                aSearchTips.HRef = ConfigurationManager.AppSettings[CommonConstants.ConfigSearchTipsURL];
                lblmsg.Text = "<br/>";

                int _CurrentTime = DateTime.Now.Hour;
                int? _FromTime = null;
                int? _ToTime = null;
                if (_CurrentTime > 12)
                {
                    _FromTime = _CurrentTime - 12;
                    rdAMPMToDate.SelectedValue = "24";
                }
                else
                {
                    _FromTime = _CurrentTime;
                    rdAMPMToDate.SelectedValue = "12";
                }
                //_ToTime = _FromTime - 1;
                _ToTime = _FromTime;

                ddlEndTime.SelectedValue = _ToTime.ToString();
                txtStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
                txtEndDate.Text = System.DateTime.Now.ToShortDateString();
                txtStartDate.Attributes.Add("readonly", "true");
                txtEndDate.Attributes.Add("readonly", "true");

                if (_SessionInformation.IsiQPremiumRadio)
                {

                    chkRadio.Visible = true;
                    aRadio.Visible = true;
                    txtRadioStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
                    txtRadioEndDate.Text = System.DateTime.Now.ToShortDateString();
                    txtRadioStartDate.Attributes.Add("readonly", "true");
                    txtRadioEndDate.Attributes.Add("readonly", "true");
                    ddlRadioEndHour.SelectedValue = _ToTime.ToString();
                }
                else
                {
                    upRadioFilter.Visible = false;
                    upRadio.Visible = false;
                    //upradioChart.Visible = false;
                    chkRadio.Visible = false;
                    aRadio.Visible = false;
                    string _Script = "$('#tabRadio').css('display','none');"
                       + "$('#divRadioResult').css('display','none');"
                       + "$('#tabChartRadio').css('display','none');"
                       + "$('#divRadioChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideRadioTab", _Script, true);
                }


                if (_SessionInformation.IsiQPremiumNM)
                {
                    txtNewsStartDate.Attributes.Add("readonly", "true");
                    txtNewsEndDate.Attributes.Add("readonly", "true");
                    txtNewsStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
                    txtNewsEndDate.Text = System.DateTime.Now.ToShortDateString();
                    ddlNewsEndHour.SelectedValue = _ToTime.ToString();
                }
                else
                {
                    upNewsFilter.Visible = false;
                    upOnlineNews.Visible = false;
                    upNewsChart.Visible = false;
                    chkNews.Visible = false;
                    aOnlineNews.Visible = false;
                    string _Script = "$('#tabOnlineNews').css('display','none');"
                       + "$('#divOnlineNewsResult').css('display','none');"
                       + "$('#tabChartOL').css('display','none');"
                       + "$('#divOLChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNewsTab", _Script, true);

                }


                if (_SessionInformation.IsiQPremiumSM)
                {
                    txtSMStartDate.Attributes.Add("readonly", "true");
                    txtSMEndDate.Attributes.Add("readonly", "true");
                    txtSMStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
                    txtSMEndDate.Text = System.DateTime.Now.ToShortDateString();
                    ddlSMEndHour.SelectedValue = _ToTime.ToString();
                }
                else
                {
                    upSMFilter.Visible = false;
                    upSocialMedia.Visible = false;
                    upSocialMediaChart.Visible = false;
                    chkSocialMedia.Visible = false;
                    aSocialMedia.Visible = false;
                    string _Script = "$('#tabSocialMedia').css('display','none');"
                       + "$('#divSocialMediaResult').css('display','none');"
                       + "$('#tabChartSM').css('display','none');"
                       + "$('#divSMChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSocialMediaTab", _Script, true);
                }

                if (_SessionInformation.IsiQPremiumTwitter)
                {
                    txtTwitterStartDate.Attributes.Add("readonly", "true");
                    txtTwitterEndDate.Attributes.Add("readonly", "true");
                    txtTwitterStartDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
                    txtTwitterEndDate.Text = System.DateTime.Now.ToShortDateString();
                    ddlTwitterEndHour.SelectedValue = _ToTime.ToString();
                }
                else
                {
                    upTwitterFilter.Visible = false;
                    upTwitter.Visible = false;
                    upTwitterChart.Visible = false;
                    chkTwitter.Visible = false;
                    aTwitter.Visible = false;

                    string _Script = "$('#tabTwitter').css('display','none');"
                       + "$('#divTwitterResult').css('display','none');"
                       + "$('#tabChartTwitter').css('display','none');"
                       + "$('#divTwitterChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTwitterTab", _Script, true);
                }

                if (_SessionInformation.IsiQPremiumAgent)
                {
                    liIsIQAgent.Visible = true;
                }
                else
                {
                    liIsIQAgent.Visible = false;
                }

                BindTwitterSortDropDown();
                BindMediaCategoryDropDown();
                GetClientSentimentSettings();
                LoadDefaultSearch();
                //ucCustomPagerSavedSearch.CurrentPage = 0;
                LoadSavedSearch();
                GetClientCompeteRights();

                // commnted filter update on 14-12-2012
                //upFilter.Update();
            }
            if ((Request["__EventTarget"] == upGrid.ClientID || Request["__EventTarget"] == upOnlineNews.ClientID || Request["__EventTarget"] == upSocialMedia.ClientID || Request["__EventTarget"] == upTwitter.ClientID) && Request["__EventArgument"] != "Tab")
            {

                if (_viewstateInformation != null)
                {
                    if (_viewstateInformation.listchartZoomHistory == null)
                    {
                        _viewstateInformation.listchartZoomHistory = new List<ChartZoomHistory>();
                    }
                    lstChartZoomHistory = _viewstateInformation.listchartZoomHistory;

                    if (_viewstateInformation.listOnlineNewsChartZoomHistory == null)
                    {
                        _viewstateInformation.listOnlineNewsChartZoomHistory = new List<OnlineNewsChartZoomHistory>();
                    }

                    if (_viewstateInformation.listSocialMediaChartZoomHistory == null)
                    {
                        _viewstateInformation.listSocialMediaChartZoomHistory = new List<SocialMediaChartZoomHistory>();
                    }
                    if (_viewstateInformation.listTwitterChartZoomHistory == null)
                    {
                        _viewstateInformation.listTwitterChartZoomHistory = new List<ChartZoomHistory>();
                    }


                }
                else
                {
                    _viewstateInformation = new ViewstateInformation();
                    _viewstateInformation.listchartZoomHistory = new List<ChartZoomHistory>();
                    _viewstateInformation.listOnlineNewsChartZoomHistory = new List<OnlineNewsChartZoomHistory>();
                    _viewstateInformation.listSocialMediaChartZoomHistory = new List<SocialMediaChartZoomHistory>();
                    _viewstateInformation.listTwitterChartZoomHistory = new List<ChartZoomHistory>();
                }


                List<string> postbackParam = Request["__EventArgument"].Split(',').ToList();
                if (postbackParam.Count > 0)
                {

                    if (postbackParam[0].ToLower() == "zoomin")
                    {
                        if (postbackParam[1].ToLower() == "divlinechart")
                        {
                            ChartZoomHistory chrtzoomhistory = new ChartZoomHistory();
                            //chrtzoomhistory.ID = lstChartZoomHistory.Count;
                            chrtzoomhistory.ID = _viewstateInformation.listchartZoomHistory.Count;
                            chrtzoomhistory.StartDate = Convert.ToDateTime(postbackParam[2]);
                            chrtzoomhistory.EndDate = Convert.ToDateTime(postbackParam[3]);
                            _viewstateInformation.listchartZoomHistory.Add(chrtzoomhistory);
                        }
                        else if (postbackParam[1].ToLower() == "divnewschart")
                        {
                            OnlineNewsChartZoomHistory onlineNewsChartZoomHistory = new OnlineNewsChartZoomHistory();
                            onlineNewsChartZoomHistory.ID = _viewstateInformation.listOnlineNewsChartZoomHistory.Count;
                            onlineNewsChartZoomHistory.StartDate = Convert.ToDateTime(postbackParam[2]);
                            onlineNewsChartZoomHistory.EndDate = Convert.ToDateTime(postbackParam[3]);
                            _viewstateInformation.listOnlineNewsChartZoomHistory.Add(onlineNewsChartZoomHistory);
                        }
                        else if (postbackParam[1].ToLower() == "divsocialmediachart")
                        {
                            SocialMediaChartZoomHistory socialMediaChartZoomHistory = new SocialMediaChartZoomHistory();
                            socialMediaChartZoomHistory.ID = _viewstateInformation.listSocialMediaChartZoomHistory.Count;
                            socialMediaChartZoomHistory.StartDate = Convert.ToDateTime(postbackParam[2]);
                            socialMediaChartZoomHistory.EndDate = Convert.ToDateTime(postbackParam[3]);
                            _viewstateInformation.listSocialMediaChartZoomHistory.Add(socialMediaChartZoomHistory);
                        }
                        else if (postbackParam[1].ToLower() == "divtwitterchart2")
                        {
                            ChartZoomHistory twitterChartZoomHistory = new ChartZoomHistory();
                            twitterChartZoomHistory.ID = _viewstateInformation.listTwitterChartZoomHistory.Count;
                            twitterChartZoomHistory.StartDate = Convert.ToDateTime(postbackParam[2]);
                            twitterChartZoomHistory.EndDate = Convert.ToDateTime(postbackParam[3]);
                            _viewstateInformation.listTwitterChartZoomHistory.Add(twitterChartZoomHistory);
                        }


                    }
                    else if (postbackParam[0].ToLower() == "zoomout")
                    {
                        if (postbackParam[1].ToLower() == "divlinechart")
                        {
                            _viewstateInformation.listchartZoomHistory.RemoveAt(_viewstateInformation.listchartZoomHistory.Count - 1);
                        }
                        else if (postbackParam[1].ToLower() == "divnewschart")
                        {
                            _viewstateInformation.listOnlineNewsChartZoomHistory.RemoveAt(_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1);
                        }
                        else if (postbackParam[1].ToLower() == "divsocialmediachart")
                        {
                            _viewstateInformation.listSocialMediaChartZoomHistory.RemoveAt(_viewstateInformation.listSocialMediaChartZoomHistory.Count - 1);
                        }
                        else if (postbackParam[1].ToLower() == "divtwitterchart2")
                        {
                            _viewstateInformation.listTwitterChartZoomHistory.RemoveAt(_viewstateInformation.listTwitterChartZoomHistory.Count - 1);
                        }

                    }
                    else if (postbackParam[0].ToLower() == "reset")
                    {
                        if (postbackParam[1].ToLower() == "divlinechart")
                        {
                            _viewstateInformation.listchartZoomHistory = null;
                        }
                        else if (postbackParam[1].ToLower() == "divnewschart")
                        {
                            _viewstateInformation.listOnlineNewsChartZoomHistory = null;
                        }
                        else if (postbackParam[1].ToLower() == "divsocialmediachart")
                        {
                            _viewstateInformation.listSocialMediaChartZoomHistory = null;
                        }
                        else if (postbackParam[1].ToLower() == "divtwitterchart2")
                        {
                            _viewstateInformation.listTwitterChartZoomHistory = null;
                        }
                    }
                    SetViewstateInformation(_viewstateInformation);
                    var currentContext = HttpContext.Current;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult(false);", true);

                    DateTime _FromDate = new DateTime();
                    DateTime _ToDate = new DateTime();
                    /* int _FromTime = 0;
                     int _ToTime = 0;*/


                    if (postbackParam[1].ToLower() == "divlinechart")
                    {
                        ucCustomPager.CurrentPage = 0;
                        if (_viewstateInformation != null && _viewstateInformation.searchRequestTV != null)
                        {

                            SearchRequest searchRequest = (SearchRequest)((SearchRequest)_viewstateInformation.searchRequestTV).SearchRequest_CloneObject();
                            if (_viewstateInformation != null && _viewstateInformation.listchartZoomHistory != null && _viewstateInformation.listchartZoomHistory.Count > 0)
                            {
                                searchRequest.StartDate = Convert.ToDateTime(_viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].StartDate);
                                searchRequest.EndDate = Convert.ToDateTime(_viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].EndDate);
                            }
                            else
                            {

                                if (rbDuration1.Checked)
                                {
                                    if (ddlDuration.SelectedValue == "all")
                                    {
                                        _ToDate = DateTime.Now;
                                        _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                                    }
                                    else
                                    {
                                        string[] _SelectedDuration = ddlDuration.SelectedValue.Split(',');
                                        if (_SelectedDuration.Length > 1)
                                        {
                                            _ToDate = DateTime.Now;
                                            int Time = -Math.Abs(Convert.ToInt32(_SelectedDuration[1]));
                                            switch (_SelectedDuration[0])
                                            {
                                                case "hour":
                                                    _FromDate = DateTime.Now.AddHours(Time);
                                                    break;
                                                case "day":
                                                    _FromDate = DateTime.Now.AddDays(Time);
                                                    break;
                                                case "month":
                                                    _FromDate = DateTime.Now.AddMonths(Time);
                                                    break;
                                                case "year":
                                                    _FromDate = DateTime.Now.AddYears(Time);
                                                    break;
                                                default:
                                                    _FromDate = DateTime.Now.AddHours(Time);
                                                    break;
                                            }
                                        }
                                    }

                                    searchRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                                    searchRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);
                                }
                                else
                                {
                                    int _FromTime = (Convert.ToInt32(rdAmPmFromDate.SelectedValue) - 12) + Convert.ToInt32(ddlStartTime.SelectedValue);
                                    int _ToTime = (Convert.ToInt32(rdAMPMToDate.SelectedValue) - 12) + Convert.ToInt32(ddlEndTime.SelectedValue);

                                    if (_FromTime == 24)
                                    {
                                        _FromTime = 12;
                                    }

                                    if (_ToTime == 24)
                                    {
                                        _ToTime = 12;
                                    }

                                    _FromDate = Convert.ToDateTime(txtStartDate.Text);
                                    _ToDate = Convert.ToDateTime(txtEndDate.Text);

                                    searchRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                                    searchRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                                }
                            }

                            /*searchRequest.StartDate = Convert.ToDateTime(_viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].StartDate);
                            searchRequest.EndDate = Convert.ToDateTime(_viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].EndDate);*/
                            searchRequest.PageNumber = (int)ucOnlineNewsPager.CurrentPage;

                            //_viewstateInformation.searchRequestTV = searchRequest;
                            //SetViewstateInformation(_viewstateInformation);

                            GetSearchResult(searchRequest, false);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTVTab", "ChangeTab('tabTV','divGridTab',0,1);", true);
                            upGrid.Update();
                        }
                    }
                    else if (postbackParam[1].ToLower() == "divnewschart")
                    {
                        ucOnlineNewsPager.CurrentPage = 0;
                        if (_viewstateInformation != null && _viewstateInformation.searchRequestOnlineNews != null)
                        {
                            SearchNewsRequest searchNewsRequest = (SearchNewsRequest)(_viewstateInformation.searchRequestOnlineNews as SearchNewsRequest).SearchNewsRequest_CloneObject();

                            if (_viewstateInformation != null && _viewstateInformation.listOnlineNewsChartZoomHistory != null && _viewstateInformation.listOnlineNewsChartZoomHistory.Count > 0)
                            {
                                searchNewsRequest.StartDate = Convert.ToDateTime(_viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].StartDate);
                                searchNewsRequest.EndDate = Convert.ToDateTime(_viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].EndDate);
                            }
                            else
                            {
                                if (rbNewsDuration.Checked)
                                {
                                    if (ddlNewsDuration.SelectedValue == "all")
                                    {
                                        _ToDate = DateTime.Now;
                                        _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                                    }
                                    else
                                    {
                                        string[] _SelectedNewsDuration = ddlNewsDuration.SelectedValue.Split(',');
                                        if (_SelectedNewsDuration.Length > 1)
                                        {
                                            _ToDate = DateTime.Now;
                                            int Time = -Math.Abs(Convert.ToInt32(_SelectedNewsDuration[1]));
                                            switch (_SelectedNewsDuration[0])
                                            {
                                                case "hour":
                                                    _FromDate = DateTime.Now.AddHours(Time);
                                                    break;
                                                case "day":
                                                    _FromDate = DateTime.Now.AddDays(Time);
                                                    break;
                                                case "month":
                                                    _FromDate = DateTime.Now.AddMonths(Time);
                                                    break;
                                                case "year":
                                                    _FromDate = DateTime.Now.AddYears(Time);
                                                    break;
                                                default:
                                                    _FromDate = DateTime.Now.AddHours(Time);
                                                    break;
                                            }
                                        }
                                    }

                                    searchNewsRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                                    searchNewsRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);
                                }
                                else
                                {
                                    int _FromTime = (Convert.ToInt32(rbNewsStart.SelectedValue) - 12) + Convert.ToInt32(ddlNewsStartHour.SelectedValue);
                                    int _ToTime = (Convert.ToInt32(rbNewsEnd.SelectedValue) - 12) + Convert.ToInt32(ddlNewsEndHour.SelectedValue);

                                    if (_FromTime == 24)
                                    {
                                        _FromTime = 12;
                                    }

                                    if (_ToTime == 24)
                                    {
                                        _ToTime = 12;
                                    }

                                    _FromDate = Convert.ToDateTime(txtNewsStartDate.Text);
                                    _ToDate = Convert.ToDateTime(txtNewsEndDate.Text);

                                    searchNewsRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                                    searchNewsRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                                }
                            }
                            searchNewsRequest.PageNumber = (int)ucOnlineNewsPager.CurrentPage;

                            //_viewstateInformation.searchRequestOnlineNews = searchNewsRequest;
                            //SetViewstateInformation(_viewstateInformation);

                            SearchNewsSection(searchNewsRequest, false);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLTab", "ChangeTab('tabOnlineNews','divGridTab',2,1);", true);
                            upOnlineNews.Update();
                        }
                    }
                    else if (postbackParam[1].ToLower() == "divsocialmediachart")
                    {
                        ucSMPager.CurrentPage = 0;
                        if (_viewstateInformation != null && _viewstateInformation.searchRequestSM != null)
                        {
                            SearchSMRequest searchSMRequest = (SearchSMRequest)((SearchSMRequest)_viewstateInformation.searchRequestSM).SearchSMRequest_CloneObject();
                            if (_viewstateInformation != null && _viewstateInformation.listSocialMediaChartZoomHistory != null && _viewstateInformation.listSocialMediaChartZoomHistory.Count > 0)
                            {
                                searchSMRequest.StartDate = Convert.ToDateTime(_viewstateInformation.listSocialMediaChartZoomHistory[_viewstateInformation.listSocialMediaChartZoomHistory.Count - 1].StartDate);
                                searchSMRequest.EndDate = Convert.ToDateTime(_viewstateInformation.listSocialMediaChartZoomHistory[_viewstateInformation.listSocialMediaChartZoomHistory.Count - 1].EndDate);
                            }
                            else
                            {
                                if (rbSMDuration.Checked)
                                {
                                    if (ddlSMDuration.SelectedValue == "all")
                                    {
                                        _ToDate = DateTime.Now;
                                        _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                                    }
                                    else
                                    {
                                        string[] selectedSMDuration = ddlSMDuration.SelectedValue.Split(',');
                                        if (selectedSMDuration.Length > 1)
                                        {
                                            _ToDate = DateTime.Now;
                                            int Time = -Math.Abs(Convert.ToInt32(selectedSMDuration[1]));
                                            switch (selectedSMDuration[0])
                                            {
                                                case "hour":
                                                    _FromDate = DateTime.Now.AddHours(Time);
                                                    break;
                                                case "day":
                                                    _FromDate = DateTime.Now.AddDays(Time);
                                                    break;
                                                case "month":
                                                    _FromDate = DateTime.Now.AddMonths(Time);
                                                    break;
                                                case "year":
                                                    _FromDate = DateTime.Now.AddYears(Time);
                                                    break;
                                                default:
                                                    _FromDate = DateTime.Now.AddHours(Time);
                                                    break;
                                            }
                                        }
                                    }

                                    searchSMRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                                    searchSMRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);
                                }
                                else
                                {
                                    int _FromTime = (Convert.ToInt32(rbSMStart.SelectedValue) - 12) + Convert.ToInt32(ddlSMStartHour.SelectedValue);
                                    int _ToTime = (Convert.ToInt32(rbSMEnd.SelectedValue) - 12) + Convert.ToInt32(ddlSMEndHour.SelectedValue);

                                    if (_FromTime == 24)
                                    {
                                        _FromTime = 12;
                                    }

                                    if (_ToTime == 24)
                                    {
                                        _ToTime = 12;
                                    }

                                    _FromDate = Convert.ToDateTime(txtSMStartDate.Text);
                                    _ToDate = Convert.ToDateTime(txtSMEndDate.Text);

                                    searchSMRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                                    searchSMRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                                }
                            }
                            searchSMRequest.PageNumber = (int)ucSMPager.CurrentPage;

                            // _viewstateInformation.searchRequestSM = searchSMRequest;
                            //SetViewstateInformation(_viewstateInformation);

                            SearchSocialMediaSection(searchSMRequest, false);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMTab", "ChangeTab('tabSocialMedia','divGridTab',3,1);", true);
                            upSocialMedia.Update();
                        }
                    }
                    else if (postbackParam[1].ToLower() == "divtwitterchart2")
                    {
                        ucTwitterPager.CurrentPage = 0;
                        if (_viewstateInformation != null && _viewstateInformation.searchRequestTwitter != null)
                        {
                            SearchTwitterRequest searchTwitterRequest = (SearchTwitterRequest)((SearchTwitterRequest)_viewstateInformation.searchRequestTwitter).SearchTwitterRequest_CloneObject();
                            if (_viewstateInformation != null && _viewstateInformation.listTwitterChartZoomHistory != null && _viewstateInformation.listTwitterChartZoomHistory.Count > 0)
                            {
                                searchTwitterRequest.StartDate = Convert.ToDateTime(_viewstateInformation.listTwitterChartZoomHistory[_viewstateInformation.listTwitterChartZoomHistory.Count - 1].StartDate);
                                searchTwitterRequest.EndDate = Convert.ToDateTime(_viewstateInformation.listTwitterChartZoomHistory[_viewstateInformation.listTwitterChartZoomHistory.Count - 1].EndDate);
                            }
                            else
                            {
                                if (rbTwitterDuration.Checked)
                                {
                                    if (ddlTwitterDuration.SelectedValue == "all")
                                    {
                                        _ToDate = DateTime.Now;
                                        _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                                    }
                                    else
                                    {
                                        string[] selectedTwitterDuration = ddlTwitterDuration.SelectedValue.Split(',');
                                        if (selectedTwitterDuration.Length > 1)
                                        {
                                            _ToDate = DateTime.Now;
                                            int Time = -Math.Abs(Convert.ToInt32(selectedTwitterDuration[1]));
                                            switch (selectedTwitterDuration[0])
                                            {
                                                case "hour":
                                                    _FromDate = DateTime.Now.AddHours(Time);
                                                    break;
                                                case "day":
                                                    _FromDate = DateTime.Now.AddDays(Time);
                                                    break;
                                                case "month":
                                                    _FromDate = DateTime.Now.AddMonths(Time);
                                                    break;
                                                case "year":
                                                    _FromDate = DateTime.Now.AddYears(Time);
                                                    break;
                                                default:
                                                    _FromDate = DateTime.Now.AddHours(Time);
                                                    break;
                                            }
                                        }
                                    }

                                    searchTwitterRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                                    searchTwitterRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);
                                }
                                else
                                {
                                    int _FromTime = (Convert.ToInt32(rbTwitterStart.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterStartHour.SelectedValue);
                                    int _ToTime = (Convert.ToInt32(rbTwitterEnd.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterEndHour.SelectedValue);

                                    if (_FromTime == 24)
                                    {
                                        _FromTime = 12;
                                    }

                                    if (_ToTime == 24)
                                    {
                                        _ToTime = 12;
                                    }

                                    _FromDate = Convert.ToDateTime(txtTwitterStartDate.Text);
                                    _ToDate = Convert.ToDateTime(txtTwitterEndDate.Text);

                                    searchTwitterRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                                    searchTwitterRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                                }
                            }
                            searchTwitterRequest.PageNumber = (int)ucTwitterPager.CurrentPage;

                            // _viewstateInformation.searchRequestSM = searchSMRequest;
                            //SetViewstateInformation(_viewstateInformation);

                            SearchTwitterSection(searchTwitterRequest, false);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMTab", "ChangeTab('tabTwitter','divGridTab',4,1);", true);
                        }
                    }

                }
            }
            else if (Request["__EventTarget"] != null && Request["__EventTarget"] == upRadio.ClientID && Request["__EventArgument"] == "Tab")
            {
                hfRadioStatus.Value = "1";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showRadioTab", "ChangeTab('tabRadio','divGridTab',1,1);", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showRadioChartTab", "ChangeTab('tabChartRadio','divChartTab',1,1);", true);
                BindRadioGrid();
            }
            else if (Request["__EventTarget"] != null && Request["__EventTarget"] == upOnlineNews.ClientID && Request["__EventArgument"] == "Tab")
            {


                hfOnlineNewsStatus.Value = "1";
                //hfTVStatus.Value = "0";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLTab", "ChangeTab('tabOnlineNews','divGridTab',2,1);", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLChartTab", "ChangeTab('tabChartOL','divChartTab',2,1);", true);
                SearchNewsSection((SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews, true);

            }
            else if (Request["__EventTarget"] != null && Request["__EventTarget"] == upGrid.ClientID && Request["__EventArgument"] == "Tab")
            {

                //hfOnlineNewsStatus.Value = "0";
                hfTVStatus.Value = "1";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTVTab", "ChangeTab('tabTV','divGridTab',0,1);", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTVChartTab", "ChangeTab('tabChartTV','divChartTab',0,1);", true);
                GetSearchResult((SearchRequest)_viewstateInformation.searchRequestTV, true);

            }
            else if (Request["__EventTarget"] != null && Request["__EventTarget"] == upSocialMedia.ClientID && Request["__EventArgument"] == "Tab")
            {
                hfSocialMediaStatus.Value = "1";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMTab", "ChangeTab('tabSocialMedia','divGridTab',3,1);", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMChartTab", "ChangeTab('tabChartSM','divChartTab',3,1);", true);
                SearchSocialMediaSection((SearchSMRequest)_viewstateInformation.searchRequestSM, true);

            }
            else if (Request["__EventTarget"] != null && Request["__EventTarget"] == upTwitter.ClientID && Request["__EventArgument"] == "Tab")
            {
                hfTwitterMediaStatus.Value = "1";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "ChangeTab('tabTwitter','divGridTab',4,1);", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterChartTab", "ChangeTab('tabChartTwitter','divChartTab',4,1);", true);
                SearchTwitterSection((SearchTwitterRequest)_viewstateInformation.searchRequestTwitter, true);
            }

            else if (Request["__EventTarget"] == upMainSearch.ClientID && Request["__EventArgument"] != null)
            {

                string[] _EventArgs = Request["__EventArgument"].Split(',');
                if (_EventArgs.Length > 1 && _EventArgs[0] == "Filter")
                {
                    if (_EventArgs[1] == upTVFilter.ID)
                    {
                        BindStatSkedProgData();
                        hdnTVData.Value = "1";
                        divMainTVFilter.Visible = true;
                        #region TV Filter

                        #region Set All Dma

                        #region Region Vise

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetRegion", "$('#" + ddlMarket.ClientID + " option').eq(0).attr('selected', 'selected');", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetmarketRegion", "CheckAllCheckBox(divMainRegionFilter);", true);

                        #endregion

                        #region Rank Vise

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetmarketrank", "CheckAllCheckBox(divMarketRankFilter);", true);

                        #endregion

                        #endregion

                        #region Set All Affils

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetTVStation", "CheckAllCheckBox(divStationFilter);", true);

                        #endregion

                        #region Set All Category

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetTVCategory", "CheckAllCheckBox(divCategoryFilter);", true);

                        #endregion


                        #region Set all to OFF
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTitle", "SetFilterStatus('divProgramFilterStatus','imgProgramFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTime", "SetFilterStatus('divTimeFilterStatus','imgTimeFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('divStationFilterStatus','imgStatusFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCat", "SetFilterStatus('divCategoryFilterStatus','imgCategoryFilter','OFF');", true);

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTV", "SetFilterStatus('divTVFilterStatus',null,'');", true);

                        #endregion


                        #endregion
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowTvFilter", "$('#" + divMainTVFilter.ClientID + "').show();", true);
                        upTVFilter.Update();
                    }

                    if (_EventArgs[1] == upRadioFilter.ID)
                    {
                        GetRadioStations();
                        hdnRadioData.Value = "1";
                        divMainRadioFilter.Visible = true;

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowRadioFilter", "$('#" + divMainRadioFilter.ClientID + "').show();", true);
                        upRadioFilter.Update();
                    }
                    else if (_EventArgs[1] == upNewsFilter.ID)
                    {
                        BindOnlineNewsCheckBoxes();
                        hdnONLINENEWSData.Value = "1";
                        divMainOnlineNewsFilter.Visible = true;
                        #region Online News Filter

                        #region Category
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONCategory", "CheckAllCheckBox(divNewsCategory);", true);

                        #endregion

                        #region Publication Category

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONPubCategory", "CheckAllCheckBox(divShowNewsPublicationCategory);", true);

                        #endregion

                        #region Genre

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONGenre", "CheckAllCheckBox(divNewsGenreFilter);", true);

                        #endregion

                        #region News Region


                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONNewsRegion", "CheckAllCheckBox(divNewsRegionFilter);", true);

                        #endregion

                        #region Set All to OFF
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsTime", "SetFilterStatus('divNewsTimeFilterStatus','imgNewsTimeFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPub", "SetFilterStatus('divNewsPublicationStatus','imgShowNewsPublicationFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsCat", "SetFilterStatus('divNewsCategoryStatus','imgShowNewsCategoryFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsGenre", "SetFilterStatus('divNewsGenreFilterStatus','imgNewsGenreFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsRegion", "SetFilterStatus('divNewsRegionFilterStatus','imgNewsRegionStatusFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPCat", "SetFilterStatus('divShowNewsPublicationCategoryStatus','imgShowNewsPublicationCategoryFilter','OFF');", true);

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNews", "SetFilterStatus('divOnlineNewsFilterStatus',null,'');", true);

                        #endregion

                        #endregion
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNewsFilter", "$('#" + divMainOnlineNewsFilter.ClientID + "').show();", true);
                        upNewsFilter.Update();
                    }
                    else if (_EventArgs[1] == upSMFilter.ID)
                    {
                        BindSocialMediaCheckBoxes();
                        hdnSocialMediaData.Value = "1";
                        divMainSMFilter.Visible = true;
                        #region Social Media Filter

                        #region Source Category
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceCategory", "CheckAllCheckBox(divSMCategory);", true);
                        #endregion

                        #region Source Tyoe
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceType", "CheckAllCheckBox(divSMType);", true);
                        #endregion

                        #region Source Rank

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceRank", "CheckAllCheckBox(divSMRank);", true);
                        #endregion

                        #region Set all to OFF
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMSource", "SetFilterStatus('divSMSourceStatus','imgShowSMSourceFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMTime", "SetFilterStatus('divSMTimeFilterStatus','imgSMTimeFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMCategory", "SetFilterStatus('divSMCategoryStatus','imgShowSMCategoryFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMType", "SetFilterStatus('divSMTypeStatus','imgShowSMTypeFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMRank", "SetFilterStatus('divSMRankStatus','imgShowSMRankFilter','OFF');", true);

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSocialMedia", "SetFilterStatus('divSMFilterStatus',null,'');", true);

                        #endregion

                        #endregion
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSMFilter", "$('#" + divMainSMFilter.ClientID + "').show();", true);
                        upSMFilter.Update();
                    }
                }
            }

            lblTVMsg.Visible = false;
            lblSMMsg.Visible = false;
            lblNewsMsg.Visible = false;
            lblTwitterMsg.Visible = false;
        }

        #endregion

        #region Button Events

        protected void btnSearch_click(object sender, EventArgs e)
        {
            try
            {
                if (!chkTV.Checked && !chkNews.Checked && !chkSocialMedia.Checked && !chkTwitter.Checked && !chkRadio.Checked)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCheckBoxValidation", "alert('Atleast one Filter must be selected');", true);
                    return;
                }
                Logger.Info("\n\n\nSearch Click Event Start");

                if (_viewstateInformation != null)
                {
                    _viewstateInformation.listchartZoomHistory = null;
                    _viewstateInformation.listOnlineNewsChartZoomHistory = null;
                    _viewstateInformation.listSocialMediaChartZoomHistory = null;
                    _viewstateInformation.listTwitterChartZoomHistory = null;
                    _viewstateInformation.SortExpression = string.Empty;
                    _viewstateInformation.SortExpressionOnlineNews = string.Empty;
                    _viewstateInformation.SortExpressionSocialMedia = string.Empty;
                    _viewstateInformation.SortExpressionTwitter = string.Empty;
                    SetViewstateInformation(_viewstateInformation);
                }
                ucCustomPager.CurrentPage = 0;
                ucOnlineNewsPager.CurrentPage = 0;
                ucSMPager.CurrentPage = 0;
                ucTwitterPager.CurrentPage = 0;
                ucRadioPager.CurrentPage = 0;

                if (ValidateCheckBoxes())
                {
                    SaveRequestObject();

                    SetFilters();

                    switch (GetGridToBind())
                    {
                        case 0: hfTVStatus.Value = "1";
                            GetSearchResult((SearchRequest)_viewstateInformation.searchRequestTV, true);

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTVTab", "ChangeTab('tabTV','divGridTab',0,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTVChartTab", "ChangeTab('tabChartTV','divChartTab',0,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');HideDiv('divRadioResultInner');HideDiv('divRadioChartInner');", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetOtherTabStatus", "SetNewsFilterStatus(0);SetSMFilterStatus(0);SetTwitterFilterStatus(0);SetRadioFilterStatus(0);", true);


                            break;

                        case 1: hfRadioStatus.Value = "1";
                            BindRadioGrid();
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showRadioTab", "ChangeTab('tabRadio','divGridTab',1,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "shoRadioChartTab", "ChangeTab('tabChartRadio','divChartTab',1,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetRadioStatus", "SetTVFilterStatus(0);SetNewsFilterStatus(0);SetSMFilterStatus(0);SetTwitterFilterStatus(0);", true);

                            break;



                        case 2: hfOnlineNewsStatus.Value = "1";
                            SearchNewsSection((SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews, true);

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLTab", "ChangeTab('tabOnlineNews','divGridTab',2,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLChartTab", "ChangeTab('tabChartOL','divChartTab',2,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');HideDiv('divRadioResultInner');HideDiv('divRadioChartInner');", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetTVStatus", "SetTVFilterStatus(0);SetSMFilterStatus(0);SetTwitterFilterStatus(0);SetRadioFilterStatus(0);", true);
                            break;

                        case 3: hfSocialMediaStatus.Value = "1";
                            SearchSocialMediaSection((SearchSMRequest)_viewstateInformation.searchRequestSM, true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMTab", "ChangeTab('tabSocialMedia','divGridTab',3,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMChartTab", "ChangeTab('tabChartSM','divChartTab',3,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');HideDiv('divRadioResultInner');HideDiv('divRadioChartInner');", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetSMStatus", "SetTVFilterStatus(0);SetNewsFilterStatus(0);SetTwitterFilterStatus(0);SetRadioFilterStatus(0);", true);
                            break;

                        case 4: hfTwitterMediaStatus.Value = "1";
                            SearchTwitterSection((SearchTwitterRequest)_viewstateInformation.searchRequestTwitter, true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "ChangeTab('tabTwitter','divGridTab',4,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterChartTab", "ChangeTab('tabChartTwitter','divChartTab',4,1);", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');HideDiv('divRadioResultInner');HideDiv('divRadioChartInner');", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetTwitterStatus", "SetTVFilterStatus(0);SetNewsFilterStatus(0);SetSMFilterStatus(0);SetRadioFilterStatus(0);", true);

                            break;


                        default: break;
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show('slow');$(\"#divChartResult\").show('slow');ShowHideDivResult(false);ShowHideDivChart(false);", true);

                }


                Logger.Info("Search Click Event End");

            }
            catch (Exception ex)
            {

                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        //protected void ShowValidationMessage()
        //{
        //    try
        //    {
        //        string validateString = string.Empty;
        //        string validatesubString = string.Empty;
        //        string InvalidTimemsg = "Start Time can not be greater than To Time";
        //        if (chkTV.Checked && (!isdmaChecked || !isaffilChecked || !isCategoryChecked || !IsTVTimeIntervalValid))
        //        {
        //            validateString = "TV: ";
        //            if (!isdmaChecked || !isaffilChecked || !isCategoryChecked)
        //                validateString += "Please Select atleast one ";
        //            if (!isdmaChecked)
        //            {
        //                validatesubString = "Market";
        //            }
        //            if (!isaffilChecked)
        //            {
        //                validatesubString = string.IsNullOrWhiteSpace(validatesubString) ? "Station" : validatesubString + ",Station ";
        //            }

        //            if (!isCategoryChecked)
        //            {
        //                validatesubString = string.IsNullOrWhiteSpace(validatesubString) ? "Category" : validatesubString + ",Category";
        //            }

        //            if (!IsTVTimeIntervalValid)
        //            {
        //                validatesubString = validatesubString + "\\n" + InvalidTimemsg;
        //            }
        //        }
        //        string validateOnlineNewsString = string.Empty;
        //        string validateOnlinenewssubString = string.Empty;

        //        if ((chkNews.Checked && _SessionInformation.IsiQPremiumNM) && (!isOnlineNewsCategoryChecked || !isOnlineNewsGenreChecked || !isOnlineNewsPublicationCategoryChecked || !isOnlineNewsRegionChecked || !IsNewsTimeIntervalValid))
        //        {

        //            validateOnlineNewsString = "Online News: ";
        //            if (!isOnlineNewsCategoryChecked || !isOnlineNewsGenreChecked || !isOnlineNewsPublicationCategoryChecked || !isOnlineNewsRegionChecked)
        //                validateOnlineNewsString += "Please Select Atleast one ";

        //            if (!isOnlineNewsCategoryChecked)
        //            {
        //                validateOnlinenewssubString = "News Category";
        //            }

        //            if (!isOnlineNewsPublicationCategoryChecked)
        //            {
        //                validateOnlinenewssubString = string.IsNullOrWhiteSpace(validateOnlinenewssubString) ? "Publication Category" : validateOnlinenewssubString + ",Publication Category";
        //            }

        //            if (!isOnlineNewsGenreChecked)
        //            {
        //                validateOnlinenewssubString = string.IsNullOrWhiteSpace(validateOnlinenewssubString) ? "Genre" : validateOnlinenewssubString + ",Genre";
        //            }

        //            if (!isOnlineNewsRegionChecked)
        //            {
        //                validateOnlinenewssubString = string.IsNullOrWhiteSpace(validateOnlinenewssubString) ? "Region" : validateOnlinenewssubString + ",Region";
        //            }

        //            if (!IsNewsTimeIntervalValid)
        //            {
        //                validateOnlinenewssubString = validateOnlinenewssubString + "\\n" + InvalidTimemsg;
        //            }
        //        }

        //        string validateSocialMediaString = string.Empty;
        //        string validateSocialMediasubString = string.Empty;
        //        if ((chkSocialMedia.Checked && _SessionInformation.IsiQPremiumSM) && (!isSMCategoryChecked || !isSMSourceTypeChecked || !isSmSourceRankChecked || !IsSocialTimeIntervalValid))
        //        {
        //            validateSocialMediaString = "Social Media: ";
        //            if (!isSMCategoryChecked || !isSMSourceTypeChecked || !isSmSourceRankChecked)
        //            {
        //                validateSocialMediaString += "Please Select Atleast one ";
        //                if (!isSMCategoryChecked)
        //                {
        //                    validateSocialMediasubString = "Source Category";
        //                }

        //                if (!isSMSourceTypeChecked)
        //                {
        //                    validateSocialMediasubString = string.IsNullOrWhiteSpace(validateOnlinenewssubString) ? "Source Category" : validateOnlinenewssubString + ",Source Type";
        //                }

        //                if (!isSmSourceRankChecked)
        //                {
        //                    validateSocialMediasubString = string.IsNullOrWhiteSpace(validateOnlinenewssubString) ? "Source Rank" : validateOnlinenewssubString + ",Source Rank";
        //                }
        //            }
        //            if (!IsSocialTimeIntervalValid)
        //            {
        //                validateSocialMediasubString = validateSocialMediasubString + "\\n" + InvalidTimemsg;
        //            }
        //        }

        //        string validateTwitterString = string.Empty;
        //        if ((chkTwitter.Checked && _SessionInformation.IsiQPremiumTwitter) && (!IsTwiiterTimeIntervalValid || !IsTwiiterFriendRangeValid || !IsTwiiterFolloweRangeValid || !IsTwiiterScoreRangeValid))
        //        {
        //            validateTwitterString = "Twitter: ";
        //            if (!IsTwiiterTimeIntervalValid)
        //            {
        //                validateTwitterString += "\\n" + InvalidTimemsg;
        //            }

        //            if (!IsTwiiterFriendRangeValid)
        //            {
        //                validateTwitterString += "\\n" + "Invalid Range for Friends Count";
        //            }

        //            if (!IsTwiiterFolloweRangeValid)
        //            {
        //                validateTwitterString += "\\n" + "Invalid Range for Followers Count";
        //            }

        //            if (!IsTwiiterScoreRangeValid)
        //            {
        //                validateTwitterString += "\\n" + "Invalid Range for Klout Score";
        //            }

        //        }

        //        string validateRadioString = string.Empty;
        //        if ((chkRadio.Checked && _SessionInformation.IsiQPremiumRadio) && (!IsRadioTimeIntervalValid || !IsRadioMarketChecked))
        //        {
        //            validateRadioString = "Radio:\\n ";

        //            if (!IsRadioTimeIntervalValid)
        //            {
        //                validateRadioString += InvalidTimemsg;
        //            }

        //            if (!IsRadioMarketChecked)
        //            {
        //                validateRadioString += " \\n Please Select atleast one ";
        //                validateRadioString += "\\n" + "Market";
        //            }

        //        }
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ValidateCheckBox", "javascript:alert('" + validateString + validatesubString + "\\n" + validateOnlineNewsString + validateOnlinenewssubString + "\\n" + validateSocialMediaString + validateSocialMediasubString + "\\n" + validateTwitterString + "\\n" + validateRadioString + "');", true);
        //    }
        //    catch (Exception ex)
        //    {

        //        this.WriteException(ex);
        //        Response.Redirect(CommonConstants.CustomErrorPage);
        //    }
        //}

        protected void LbtnRawMediaPlay_Command(object sender, CommandEventArgs e)
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

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetDivIFrameStyle", "SetDivIFrameDiv();", true);
                IframeRawMediaH.RawMediaID = new Guid(Convert.ToString(e.CommandArgument));
                IframeRawMediaH.IsUGC = false;
                IframeRawMediaH.SearchTerm = txtSearch.Text;

                IframeRawMediaH.InitializePlayer();

                //mpePlayer.Show();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPlayerModal", "ShowModal('diviframe');", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CCCallBack", "RegisterCCCallback(0);", true);
                upVideo.Update();

                if (hfcurrentTab.Value == "0")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowTVTab", "showTVTab();", true);
                }
                else if (hfcurrentTab.Value == "4")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowRadioTab", "showRadioTab();", true);
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult(false);", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnPostback_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (_viewstateInformation == null)
                    _viewstateInformation = GetViewstateInformation();




                List<SSP_IQ_Dma_Name> _ListOfMarketSelected = new List<SSP_IQ_Dma_Name>();
                List<SSP_IQ_Class> _ListOfProgramSelected = new List<SSP_IQ_Class>();
                List<String> _ListOfAffilsSelected = new List<String>();
                List<String> _ListOfNewsCategorySelected = new List<String>();
                List<String> _ListOfNewsRegionSelected = new List<String>();
                List<String> _ListOfNewsGenreSelected = new List<String>();
                List<String> _ListOfNewsPublicationCategorySelected = new List<String>();
                List<String> _ListOfSMSourceCategorySelected = new List<String>();
                List<String> _ListOFSMSourceTypeSelected = new List<String>();
                List<String> _ListOFSMSourceRankSelected = new List<String>();
                List<IQ_STATION> _ListOFRadioMarket = new List<IQ_STATION>();

                DateTime? _FromDate = null;
                DateTime? _ToDate = null;
                string _DayDuration = string.Empty;

                DateTime? _NewsFromDate = null;
                DateTime? _NewsToDate = null;
                string _NewsDayDuration = string.Empty;

                DateTime? _SMFromDate = null;
                DateTime? _SMToDate = null;
                string _SMDayDuration = string.Empty;

                DateTime? _TwitterFromDate = null;
                DateTime? _TwitterToDate = null;
                string _TwitterDayDuration = string.Empty;

                DateTime? _RadioFromDate = null;
                DateTime? _RadioToDate = null;
                string _RadioDayDuration = string.Empty;

                int _FromTime = 0, _ToTime = 0, _NewsFromTime = 0, _NewsToTime = 0, _SMFromTime = 0, _SMToTime = 0, _TwitterFromTime = 0, _TwitterToTime = 0, _RadioFromTime = 0, _RadioToTime = 0;

                if (chkIsIQAgent.Checked && _SessionInformation.IsiQPremiumAgent && String.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    lblmsg.Text = "Search term is mandatory for IQAgent";
                    lblmsg.Visible = true;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideUpdatebtn", "$('#" + btnUpdate.ClientID + "').hide();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSubmitbtn", "$('#" + btnSubmit.ClientID + "').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNote", "$('#" + lblNote.ClientID + "').hide();", true);
                    return;
                }

                if (chkTV.Checked)
                {
                    #region market Filter Region

                    if (ddlMarket.SelectedIndex == 0)
                    {
                        #region Region Filter

                        if (!chlkRegionSelectAll.Checked || _viewstateInformation.IsAllDmaAllowed == false)
                        {
                            foreach (RepeaterItem rptitm in rptregion.Items)
                            {
                                Repeater rptDmaList = (Repeater)rptitm.FindControl("rptDma");
                                foreach (RepeaterItem repeaterItem in rptDmaList.Items)
                                {
                                    HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                    if (chkboxDma.Checked)
                                    {
                                        string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                        if (SelctedDMANameAndNum.Length > 1)
                                        {
                                            SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                            IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                            IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                            _ListOfMarketSelected.Add(IQ_DMA);
                                        }

                                    }
                                }
                            }
                            if (chkRegionNational.Visible && chkRegionNational.Checked)
                            {
                                SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                IQ_DMA.IQ_Dma_Name = chkRegionNational.Value;
                                IQ_DMA.IQ_Dma_Num = "000";
                                _ListOfMarketSelected.Add(IQ_DMA);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Rank Filter

                        if (!chkRankFIlterSelectAll.Checked || _viewstateInformation.IsAllDmaAllowed == false)
                        {
                            foreach (RepeaterItem repeaterItem in rptTop10.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop20.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop30.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop40.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop50.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop60.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop80.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop100.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop150.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop210.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            if (chkNational.Visible && chkNational.Checked)
                            {
                                SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                IQ_DMA.IQ_Dma_Name = chkNational.Value;
                                IQ_DMA.IQ_Dma_Num = "000";
                                _ListOfMarketSelected.Add(IQ_DMA);
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Affiliate Region
                    /*if (!chkAffilAll.Checked || _ViewstateInformation.IsAllStationAllowed == false)
                {
                    foreach (ListItem chk in cblAI.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                    foreach (ListItem chk in cblFJ.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                    foreach (ListItem chk in cblKO.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                    foreach (ListItem chk in cblPT.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                    foreach (ListItem chk in cblUZ.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                }*/


                    if (!chkAffilAll.Checked || _viewstateInformation.IsAllStationAllowed == false)
                    {
                        foreach (RepeaterItem rptitm in rptTVStationSubMaster.Items)
                        {
                            Repeater rptTVStationChild = (Repeater)rptitm.FindControl("rptTVStationChild");
                            foreach (RepeaterItem repeaterItem in rptTVStationChild.Items)
                            {
                                HtmlInputCheckBox chkTVStation = (HtmlInputCheckBox)repeaterItem.FindControl("chkTVStation");
                                if (chkTVStation.Checked)
                                {
                                    _ListOfAffilsSelected.Add(chkTVStation.Value);
                                }
                            }
                        }
                    }
                    #endregion

                    #region Category
                    if (!chkCategoryAll.Checked || _viewstateInformation.IsAllClassAllowed == false)
                    {
                        foreach (RepeaterItem rptItem in rptCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkClass");
                            if (chk.Checked)
                            {

                                Label label = (Label)rptItem.FindControl("lblClass");
                                SSP_IQ_Class IQClass = new SSP_IQ_Class();
                                IQClass.IQ_Class = label.Text;
                                IQClass.IQ_Class_Num = chk.Value;
                                _ListOfProgramSelected.Add(IQClass);
                            }
                        }
                    }

                    #endregion

                    #region Duration Calculation

                    _FromTime = (Convert.ToInt32(rdAmPmFromDate.SelectedValue) - 12) + Convert.ToInt32(ddlStartTime.SelectedValue);
                    _ToTime = (Convert.ToInt32(rdAMPMToDate.SelectedValue) - 12) + Convert.ToInt32(ddlEndTime.SelectedValue);

                    if (_FromTime == 24)
                    {
                        _FromTime = 12;
                    }

                    if (_ToTime == 24)
                    {
                        _ToTime = 12;
                    }

                    if (rbDuration1.Checked)
                    {
                        _DayDuration = ddlDuration.SelectedValue;
                        //if (ddlDuration.SelectedValue == "0") // 3 Months
                        //{
                        //    _DayDuration = "3";
                        //}
                        //else if (ddlDuration.SelectedValue == "1") // 6 Months
                        //{
                        //    _DayDuration = "6";
                        //}
                        //else if (ddlDuration.SelectedValue == "2") // 1 Year
                        //{
                        //    _DayDuration = "12";
                        //}
                        //else
                        //{
                        //    _DayDuration = "All";
                        //}
                    }
                    else
                    {
                        _FromDate = Convert.ToDateTime(txtStartDate.Text);
                        _ToDate = Convert.ToDateTime(txtEndDate.Text);
                    }

                    #endregion

                    /*savedSearchRequest.tv.tvDuration.FromDate = _FromDate;
                    savedSearchRequest.tv.tvDuration.ToDate = _ToDate;
                    savedSearchRequest.tv.ProgramTitle = txProgramTitle.Text.Trim();
                    savedSearchRequest.tv.Appearing = txtAppearing.Text.Trim();
                    savedSearchRequest.tv.iQDmaSet.listofIQDma = _ListOfMarketSelected;
                    savedSearchRequest.tv.stationAffiliateSet.listOfIQStationID = _ListOfAffilsSelected;
                    savedSearchRequest.tv.iQClassSet.listofIQCLass = _ListOfProgramSelected;*/

                }

                if (chkNews.Checked && _SessionInformation.IsiQPremiumNM)
                {
                    #region News Filters

                    #region List Filters
                    if (!chkNewsCategorySelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsCategory");
                            if (chk.Checked)
                            {
                                _ListOfNewsCategorySelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkNewsRegionAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsRegion.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsregion");
                            if (chk.Checked)
                            {
                                _ListOfNewsRegionSelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkNewsGenreSelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsGenre.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsGenre");
                            if (chk.Checked)
                            {
                                _ListOfNewsGenreSelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkNewsPublicationCategory.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsPublicationCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsPublicationCategory");
                            if (chk.Checked)
                            {
                                _ListOfNewsPublicationCategorySelected.Add(chk.Value);
                            }
                        }
                    }
                    #endregion

                    #region News Time Filter

                    _NewsFromTime = (Convert.ToInt32(rbNewsStart.SelectedValue) - 12) + Convert.ToInt32(ddlNewsStartHour.SelectedValue);
                    _NewsToTime = (Convert.ToInt32(rbNewsEnd.SelectedValue) - 12) + Convert.ToInt32(ddlNewsEndHour.SelectedValue);

                    if (_NewsFromTime == 24)
                    {
                        _NewsFromTime = 12;
                    }

                    if (_NewsToTime == 24)
                    {
                        _NewsToTime = 12;
                    }

                    if (rbNewsDuration.Checked)
                    {
                        _NewsDayDuration = ddlNewsDuration.SelectedValue;
                        //if (ddlNewsDuration.SelectedValue == "0") // 3 Months
                        //{
                        //    _NewsDayDuration = "3";
                        //}
                        //else if (ddlNewsDuration.SelectedValue == "1") // 6 Months
                        //{
                        //    _NewsDayDuration = "6";
                        //}
                        //else if (ddlNewsDuration.SelectedValue == "2") // 1 Year
                        //{
                        //    _NewsDayDuration = "12";
                        //}
                        //else
                        //{
                        //    _NewsDayDuration = "All";
                        //}
                    }
                    else
                    {
                        _NewsFromDate = Convert.ToDateTime(txtNewsStartDate.Text);
                        _NewsToDate = Convert.ToDateTime(txtNewsEndDate.Text);
                    }

                    #endregion

                    #endregion

                    /*savedSearchRequest.news.NewsDuration.FromDate = _NewsFromDate;
                    savedSearchRequest.news.NewsDuration.ToDate = _NewsToDate;
                    savedSearchRequest.news.Publication = txtNewsPublication.Text.Trim();
                    savedSearchRequest.news.newsCategory_Set.listOfNewsCategory = _ListOfNewsCategorySelected;
                    savedSearchRequest.news.publicationCategory_Set.listofPublicationCategory = _ListOfNewsPublicationCategorySelected;
                    savedSearchRequest.news.genre_Set.listOfGenre = _ListOfNewsGenreSelected;
                    savedSearchRequest.news.region_Set.listOfRegion = _ListOfNewsRegionSelected;*/
                }

                if (chkSocialMedia.Checked && _SessionInformation.IsiQPremiumSM)
                {
                    #region Social Media Filter

                    if (!chkSMCategorySelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptSMCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMCategory");
                            if (chk.Checked)
                            {
                                _ListOfSMSourceCategorySelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkSMTypeSelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptSMType.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMType");
                            if (chk.Checked)
                            {
                                _ListOFSMSourceTypeSelected.Add(chk.Value);
                            }
                        }
                    }


                    if (!chkSMRankSelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptSMRank.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMRank");
                            if (chk.Checked)
                            {
                                _ListOFSMSourceRankSelected.Add(chk.Value);
                            }
                        }
                    }

                    #region Social Media Time Filter

                    _SMFromTime = (Convert.ToInt32(rbSMStart.SelectedValue) - 12) + Convert.ToInt32(ddlSMStartHour.SelectedValue);
                    _SMToTime = (Convert.ToInt32(rbSMEnd.SelectedValue) - 12) + Convert.ToInt32(ddlSMEndHour.SelectedValue);

                    if (_SMFromTime == 24)
                    {
                        _SMFromTime = 12;
                    }

                    if (_SMToTime == 24)
                    {
                        _SMToTime = 12;
                    }

                    _SMFromDate = null;
                    _SMToDate = null;
                    _SMDayDuration = string.Empty;

                    if (rbSMDuration.Checked)
                    {
                        _SMDayDuration = ddlSMDuration.SelectedValue;
                    }
                    else
                    {
                        _SMFromDate = Convert.ToDateTime(txtSMStartDate.Text);
                        _SMToDate = Convert.ToDateTime(txtSMEndDate.Text);
                    }

                    #endregion

                    #endregion

                    /*savedSearchRequest.SMStartDate = _SMFromDate;
                    savedSearchRequest.SMEndDate = _SMToDate;
                    savedSearchRequest.SMSource = txtSMSource.Text.Trim();
                    savedSearchRequest.SMAuthor = txtSMAuthor.Text.Trim();
                    savedSearchRequest.SMTitle = txtSMTitle.Text.Trim();
                    savedSearchRequest.SMListOfSourceCategory = _ListOfSMSourceCategorySelected;
                    savedSearchRequest.SMListOfSourceType = _ListOFSMSourceTypeSelected;
                    savedSearchRequest.SMListOfSourceRank = _ListOFSMSourceRankSelected;*/

                }

                if (chkTwitter.Checked && _SessionInformation.IsiQPremiumTwitter)
                {
                    #region Twitter Time Filter

                    if (rbTwitterDuration.Checked)
                    {
                        _TwitterDayDuration = ddlTwitterDuration.SelectedValue;
                    }
                    else
                    {
                        _TwitterFromTime = (Convert.ToInt32(rbTwitterStart.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterStartHour.SelectedValue);
                        _TwitterToTime = (Convert.ToInt32(rbTwitterEnd.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterEndHour.SelectedValue);

                        if (_TwitterFromTime == 24)
                        {
                            _TwitterFromTime = 12;
                        }

                        if (_TwitterToTime == 24)
                        {
                            _TwitterToTime = 12;
                        }

                        _TwitterFromDate = Convert.ToDateTime(txtTwitterStartDate.Text);
                        _TwitterToDate = Convert.ToDateTime(txtTwitterEndDate.Text);
                    }

                    #endregion

                    /*savedSearchRequest.TwitterStartDate = _TwitterFromDate;
                    savedSearchRequest.TwitterEndDate = _TwitterToDate;
                    savedSearchRequest.TwitterActor = txtTweetActor.Text.Trim();
                    savedSearchRequest.TwitterFollowersCountFrom = Convert.ToInt64(txtFollowerCountFrom.Text.Trim());
                    savedSearchRequest.TwitterFollowersCountTo = Convert.ToInt64(txtFollowerCountTo.Text.Trim());
                    savedSearchRequest.TwitterFriendsCountFrom = Convert.ToInt64(txtFriendsCountFrom.Text.Trim());
                    savedSearchRequest.TwitterFriendsCountTo = Convert.ToInt64(txtFriendsCountTo.Text.Trim());
                    savedSearchRequest.TwitterKloutScoreFrom = Convert.ToInt64(txtKloutScoreFrom.Text.Trim());
                    savedSearchRequest.TwitterKloutScoreTo = Convert.ToInt64(txtKloutScoreTo.Text.Trim());*/
                }

                if (chkRadio.Checked && _SessionInformation.IsiQPremiumRadio)
                {
                    #region Radio Market Filters

                    if (!chkRadioMarketAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptRadioMarket.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkDma");
                            if (chk.Checked)
                            {
                                string[] numNameOFMarket = chk.Value.Split('#');
                                IQ_STATION iq_Station = new IQ_STATION();
                                iq_Station.dma_name = numNameOFMarket[0];
                                iq_Station.dma_num = numNameOFMarket[1];
                                _ListOFRadioMarket.Add(iq_Station);
                            }
                        }
                    }


                    #endregion

                    #region Radio Time Filter

                    _RadioFromTime = (Convert.ToInt32(rbRadioStart.SelectedValue) - 12) + Convert.ToInt32(ddlRadioStartHour.SelectedValue);
                    _RadioToTime = (Convert.ToInt32(rbRadioEnd.SelectedValue) - 12) + Convert.ToInt32(ddlRadioEndHour.SelectedValue);

                    if (_RadioFromTime == 24)
                    {
                        _RadioFromTime = 12;
                    }

                    if (_RadioToTime == 24)
                    {
                        _RadioToTime = 12;
                    }

                    if (rbRadioDuration.Checked)
                    {
                        _RadioDayDuration = ddlRadioDuration.SelectedValue;
                    }
                    else
                    {
                        _RadioFromDate = Convert.ToDateTime(txtRadioStartDate.Text);
                        _RadioToDate = Convert.ToDateTime(txtRadioEndDate.Text);
                    }

                    #endregion

                    /* savedSearchRequest.RadioStartDate = _RadioFromDate;
                    savedSearchRequest.RadioEndDate = _RadioToDate;
                    savedSearchRequest.RadioListOfMarket = _ListOFRadioMarket;*/

                }

                #region Generate Xml
                XDocument xmlDocument = new XDocument(new XElement("SearchRequest",
                    new XElement("SearchTerm", txtSearch.Text),
                    !chkTV.Checked ? null :
                    new XElement("TV",
                        new XElement("ProgramTitle", txProgramTitle.Text),
                        new XElement("Appearing", txtAppearing.Text),
                        rbDuration2.Checked ? new XElement("TimeZone", ddlTimeZone.SelectedItem.Text) : null,

                        rbDuration1.Checked ?
                        new XElement("Duration",
                            new XElement("DayDuration", _DayDuration)) :
                        _FromDate != null && _ToDate != null ?
                        new XElement("Duration",
                            new XElement("FromDate", new DateTime(_FromDate.Value.Year, _FromDate.Value.Month, _FromDate.Value.Day, _FromTime, 0, 0).ToString()),
                            new XElement("ToDate", new DateTime(_ToDate.Value.Year, _ToDate.Value.Month, _ToDate.Value.Day, _ToTime, 0, 0).ToString())) : null,

                        _ListOfMarketSelected.Count == 0 ?
                        new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "true"), new XAttribute("SelectionMethod", ddlMarket.SelectedValue)) :
                        new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "false"), new XAttribute("SelectionMethod", ddlMarket.SelectedValue),
                        from SSP_IQ_Dma_Name market in _ListOfMarketSelected
                        select new XElement("IQ_Dma",
                            new XElement("num", market.IQ_Dma_Num),
                            new XElement("name", market.IQ_Dma_Name))),

                        _ListOfAffilsSelected.Count == 0 ?
                        new XElement("Station_Affiliate_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("Station_Affiliate_Set", new XAttribute("IsAllowAll", "false"),
                        from string affil in _ListOfAffilsSelected
                        select new XElement("IQ_Station_ID", affil)),

                        _ListOfProgramSelected.Count == 0 ?
                        new XElement("IQ_Class_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("IQ_Class_Set", new XAttribute("IsAllowAll", "false"),
                        from SSP_IQ_Class program in _ListOfProgramSelected
                        select new XElement("IQ_Class",
                            new XElement("num", program.IQ_Class_Num),
                            new XElement("name", program.IQ_Class)))
                        ),

                        !chkRadio.Checked ? null :
                        new XElement("Radio",
                            rbRadioDuration.Checked ?
                        new XElement("Duration",
                            new XElement("DayDuration", _RadioDayDuration)) :
                        _RadioFromDate != null && _RadioToDate != null ?
                        new XElement("Duration",
                            new XElement("FromDate", new DateTime(_RadioFromDate.Value.Year, _RadioFromDate.Value.Month, _RadioFromDate.Value.Day, _RadioFromTime, 0, 0).ToString()),
                            new XElement("ToDate", new DateTime(_RadioToDate.Value.Year, _RadioToDate.Value.Month, _RadioToDate.Value.Day, _RadioToTime, 0, 0).ToString())) : null,

                            _ListOFRadioMarket.Count == 0 ?
                        new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "false"),
                        from IQ_STATION radioMarket in _ListOFRadioMarket
                        select new XElement("IQ_Dma",
                            new XElement("num", radioMarket.dma_num),
                            new XElement("name", radioMarket.dma_name)))),

                    !chkNews.Checked ? null :
                    new XElement("News",
                        new XElement("Publication", txtNewsPublication.Text),
                        rbNewsDuration.Checked ?
                        new XElement("Duration",
                            new XElement("DayDuration", _NewsDayDuration)) :
                        _NewsFromDate != null && _NewsToDate != null ?
                        new XElement("Duration",
                            new XElement("FromDate", new DateTime(_NewsFromDate.Value.Year, _NewsFromDate.Value.Month, _NewsFromDate.Value.Day, _NewsFromTime, 0, 0).ToString()),
                            new XElement("ToDate", new DateTime(_NewsToDate.Value.Year, _NewsToDate.Value.Month, _NewsToDate.Value.Day, _NewsToTime, 0, 0).ToString())) : null,
                        _ListOfNewsCategorySelected.Count == 0 ?
                        new XElement("NewsCategory_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("NewsCategory_Set", new XAttribute("IsAllowAll", "false"),
                        from string newscategory in _ListOfNewsCategorySelected
                        select new XElement("NewsCategory", newscategory)),

                        _ListOfNewsPublicationCategorySelected.Count == 0 ?
                        new XElement("PublicationCategory_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("PublicationCategory_Set", new XAttribute("IsAllowAll", "false"),
                        from string publicationcategory in _ListOfNewsPublicationCategorySelected
                        select new XElement("PublicationCategory", publicationcategory)),

                        _ListOfNewsGenreSelected.Count == 0 ?
                        new XElement("Genre_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("Genre_Set", new XAttribute("IsAllowAll", "false"),
                        from string genre in _ListOfNewsGenreSelected
                        select new XElement("Genre", genre)),

                        _ListOfNewsRegionSelected.Count == 0 ?
                        new XElement("Region_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("Region_Set", new XAttribute("IsAllowAll", "false"),
                        from string region in _ListOfNewsRegionSelected
                        select new XElement("Region", region))
                      ),

                    !chkSocialMedia.Checked ? null :
                    new XElement("SocialMedia",
                        rbSMDuration.Checked ?
                        new XElement("Duration",
                            new XElement("DayDuration", _SMDayDuration)) :
                        _SMFromDate != null && _SMToDate != null ?
                        new XElement("Duration",
                            new XElement("FromDate", new DateTime(_SMFromDate.Value.Year, _SMFromDate.Value.Month, _SMFromDate.Value.Day, _SMFromTime, 0, 0).ToString()),
                            new XElement("ToDate", new DateTime(_SMToDate.Value.Year, _SMToDate.Value.Month, _SMToDate.Value.Day, _SMToTime, 0, 0).ToString())) : null,
                        new XElement("Source", txtSMSource.Text),
                        new XElement("Author", txtSMAuthor.Text),
                        new XElement("Title", txtSMTitle.Text),

                        _ListOFSMSourceRankSelected.Count == 0 ?
                        new XElement("SourceRank_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("SourceRank_Set", new XAttribute("IsAllowAll", "false"),
                            from string sourcerank in _ListOFSMSourceRankSelected
                            select new XElement("SourceRank", sourcerank)),

                        _ListOfSMSourceCategorySelected.Count == 0 ?
                        new XElement("SourceCategory_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("SourceCategory_Set", new XAttribute("IsAllowAll", "false"),
                            from string sourcecategory in _ListOfSMSourceCategorySelected
                            select new XElement("SourceCategory", sourcecategory)),

                        _ListOFSMSourceTypeSelected.Count == 0 ?
                        new XElement("SourceType_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("SourceType_Set", new XAttribute("IsAllowAll", "false"),
                            from string sourcetype in _ListOFSMSourceTypeSelected
                            select new XElement("SourceType", sourcetype))
                    ),

                    !chkTwitter.Checked || !_SessionInformation.IsiQPremiumTwitter ? null :
                        new XElement("Twitter",
                            rbTwitterDuration.Checked ?
                            new XElement("Duration",
                                new XElement("DayDuration", _TwitterDayDuration)) :
                                _TwitterFromDate != null && _TwitterToDate != null ?
                            new XElement("Duration",
                                new XElement("FromDate", new DateTime(_TwitterFromDate.Value.Year, _TwitterFromDate.Value.Month, _TwitterFromDate.Value.Day, _TwitterFromTime, 0, 0).ToString()),
                                new XElement("ToDate", new DateTime(_TwitterToDate.Value.Year, _TwitterToDate.Value.Month, _TwitterToDate.Value.Day, _TwitterToTime, 0, 0).ToString())) : null,
                            new XElement("Actor", txtTweetActor.Text),

                            (!string.IsNullOrWhiteSpace(txtFriendsCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFriendsCountTo.Text)) ?
                             new XElement("ActorFriendsRange",
                                 new XElement("From", txtFriendsCountFrom.Text),
                                 new XElement("To", txtFriendsCountTo.Text)) : null,

                            (!string.IsNullOrWhiteSpace(txtFollowerCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFollowerCountTo.Text)) ?
                             new XElement("ActorFollowersRange",
                                 new XElement("From", txtFollowerCountFrom.Text),
                                 new XElement("To", txtFollowerCountTo.Text)) : null,

                            (!string.IsNullOrWhiteSpace(txtKloutScoreFrom.Text) && !string.IsNullOrWhiteSpace(txtKloutScoreTo.Text)) ?
                             new XElement("KloutScoreRange",
                                 new XElement("From", txtKloutScoreFrom.Text),
                                 new XElement("To", txtKloutScoreTo.Text)) : null
                     )
                 ));
                #endregion

                int OutStatus = 0;
                int iQAgentStatus = 0;
                string OutTitle = string.Empty;
                int ID = 0;
                IIQCustomer_SavedSearchController _IIQCustomer_SavedSearchController = _ControllerFactory.CreateObject<IIQCustomer_SavedSearchController>();
                _IIQCustomer_SavedSearchController.InsertCustomerSearch(new Guid(_SessionInformation.CustomerGUID), xmlDocument, txtTitle.Text.Trim(), txtDescription.Text.Trim(), new Guid(ddlCategory.SelectedValue), Convert.ToBoolean(chkIsDefaultSearch.Checked), Convert.ToBoolean(chkIsIQAgent.Checked && _SessionInformation.IsiQPremiumAgent), new Guid(_SessionInformation.ClientGUID), out OutStatus, out OutTitle, out ID, out iQAgentStatus);
                if (OutStatus == 0)
                {
                    if (ID > 0)
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Search Request Saved Successfully');", true);
                        if (iQAgentStatus < 0)
                        {
                            lblIQAgentmsg.Text = "Total no. of iQAgent exceeds the limit.";
                            lblIQAgentmsg.Visible = true;
                            lblIQAgentmsg.ForeColor = System.Drawing.Color.Red;
                        }
                        lblSavedSearchmsg.Text = "Search Request Saved Successfully.";
                        lblSavedSearchmsg.Visible = true;
                        lblSavedSearchmsg.ForeColor = System.Drawing.Color.Green;


                        SaveRequestObject();
                        SetFilters();

                        if (_viewstateInformation.LoadedSavedSearch == null)
                            _viewstateInformation.LoadedSavedSearch = new SavedSearch();
                        _viewstateInformation.LoadedSavedSearch.ID = ID;
                        _viewstateInformation.LoadedSavedSearch.Title = txtTitle.Text.Trim();
                        _viewstateInformation.LoadedSavedSearch.IsIQAgent = (iQAgentStatus > 0) ? true : false;
                        _viewstateInformation.LoadedSavedSearch.CustomerGUID = new Guid(_SessionInformation.CustomerGUID);
                        _viewstateInformation.CurrentPageSavedSearch = 0;
                        SetViewstateInformation(_viewstateInformation);

                        txtTitle.Text = string.Empty;
                        txtDescription.Text = string.Empty;
                        ddlCategory.SelectedIndex = 0;
                        chkIsDefaultSearch.Checked = false;
                        //mpSaveSearch.Hide();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveSearchModal", "closeModal('pnlSaveSearch');", true);

                        LoadSavedSearch();
                        upSavedSearh.Update();
                        upMainSearch.Update();
                    }
                    else
                    {
                        lblmsg.Text = "An error occur, please try again.";
                        lblmsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSubmitButton", "$('#ctl00_Content_Data_ucIQpremium_btnSubmit').show();", true);
                    }
                }
                else if (OutStatus == -1)
                {
                    lblmsg.Text = "search with same title already exist, Please try different title.";
                    lblmsg.Visible = true;
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSubmitButton", "$('#ctl00_Content_Data_ucIQpremium_btnSubmit').show();", true);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideUpdatebtn", "$('#" + btnUpdate.ClientID + "').hide();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSubmitbtn", "$('#" + btnSubmit.ClientID + "').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNote", "$('#" + lblNote.ClientID + "').hide();", true);
                }
                else if (OutStatus == -2)
                {
                    lblmsg.Text = "search with same criteria already exist , with title : " + OutTitle + "";
                    lblmsg.Visible = true;
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSubmitButton", "$('#ctl00_Content_Data_ucIQpremium_btnSubmit').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideUpdatebtn", "$('#" + btnUpdate.ClientID + "').hide();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSubmitbtn", "$('#" + btnSubmit.ClientID + "').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNote", "$('#" + lblNote.ClientID + "').hide();", true);
                }
                else
                {
                    lblmsg.Text = "An error occured, please try again!";
                    lblmsg.Visible = true;
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSubmitButton", "$('#ctl00_Content_Data_ucIQpremium_btnSubmit').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideUpdatebtn", "$('#" + btnUpdate.ClientID + "').hide();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSubmitbtn", "$('#" + btnSubmit.ClientID + "').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNote", "$('#" + lblNote.ClientID + "').hide();", true);
                }


            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {


                List<SSP_IQ_Dma_Name> _ListOfMarketSelected = new List<SSP_IQ_Dma_Name>();
                List<SSP_IQ_Class> _ListOfProgramSelected = new List<SSP_IQ_Class>();
                List<String> _ListOfAffilsSelected = new List<String>();
                List<String> _ListOfNewsCategorySelected = new List<String>();
                List<String> _ListOfNewsRegionSelected = new List<String>();
                List<String> _ListOfNewsGenreSelected = new List<String>();
                List<String> _ListOfNewsPublicationCategorySelected = new List<String>();
                List<String> _ListOfSMSourceCategorySelected = new List<String>();
                List<String> _ListOFSMSourceTypeSelected = new List<String>();
                List<String> _ListOFSMSourceRankSelected = new List<String>();
                List<IQ_STATION> _ListOFRadioMarket = new List<IQ_STATION>();
                SavedSearchRequest savedSearchRequestThis = new SavedSearchRequest();
                savedSearchRequestThis.SearchTerm = txtSearch.Text.Trim();


                SavedSearch savedSearch = new SavedSearch();

                DateTime? _FromDate = null;
                DateTime? _ToDate = null;
                string _DayDuration = string.Empty;

                DateTime? _NewsFromDate = null;
                DateTime? _NewsToDate = null;
                string _NewsDayDuration = string.Empty;


                DateTime? _SMFromDate = null;
                DateTime? _SMToDate = null;
                string _SMDayDuration = string.Empty;

                DateTime? _TwitterFromDate = null;
                DateTime? _TwitterToDate = null;
                string _TwitterDayDuration = string.Empty;

                DateTime? _RadioFromDate = null;
                DateTime? _RadioToDate = null;
                string _RadioDayDuration = string.Empty;


                int _FromTime = 0, _ToTime = 0, _NewsFromTime = 0, _NewsToTime = 0, _SMFromTime = 0, _SMToTime = 0, _TwitterFromTime = 0, _TwitterToTime = 0, _RadioFromTime = 0, _RadioToTime = 0;

                if (chkIsIQAgent.Checked && _SessionInformation.IsiQPremiumAgent && String.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    lblmsg.Text = "Search term is mandatory for IQAgent";
                    lblmsg.Visible = true;
                    lblNote.Text = "Note: All Text and Filter Setting are included in the \"Update\"";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showUpdateButton", "$('#ctl00_Content_Data_ucIQpremium_btnUpdate').show();$('#spnSaveSearchHeader').text('Edit Search');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSubmitbtn", "$('#" + btnSubmit.ClientID + "').hide();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNote", "$('#" + lblNote.ClientID + "').css(\"display\",\"block\");", true);

                    return;
                }

                if (chkTV.Checked)
                {
                    #region market Filter Region

                    if (ddlMarket.SelectedIndex == 0)
                    {
                        #region Region Filter

                        if (!chlkRegionSelectAll.Checked || _viewstateInformation.IsAllDmaAllowed == false)
                        {
                            foreach (RepeaterItem rptitm in rptregion.Items)
                            {
                                Repeater rptDmaList = (Repeater)rptitm.FindControl("rptDma");
                                foreach (RepeaterItem repeaterItem in rptDmaList.Items)
                                {
                                    HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                    if (chkboxDma.Checked)
                                    {
                                        string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                        if (SelctedDMANameAndNum.Length > 1)
                                        {
                                            SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                            IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                            IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                            _ListOfMarketSelected.Add(IQ_DMA);
                                        }

                                    }
                                }
                            }
                            if (chkRegionNational.Visible && chkRegionNational.Checked)
                            {
                                SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                IQ_DMA.IQ_Dma_Name = chkRegionNational.Value;
                                IQ_DMA.IQ_Dma_Num = "000";
                                _ListOfMarketSelected.Add(IQ_DMA);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Rank Filter

                        if (!chkRankFIlterSelectAll.Checked || _viewstateInformation.IsAllDmaAllowed == false)
                        {
                            foreach (RepeaterItem repeaterItem in rptTop10.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop20.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop30.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop40.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop50.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop60.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop80.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop100.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop150.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop210.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                    if (SelctedDMANameAndNum.Length > 1)
                                    {
                                        SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                        IQ_DMA.IQ_Dma_Name = SelctedDMANameAndNum[0];
                                        IQ_DMA.IQ_Dma_Num = SelctedDMANameAndNum[1];
                                        _ListOfMarketSelected.Add(IQ_DMA);
                                    }
                                }
                            }

                            if (chkNational.Visible && chkNational.Checked)
                            {
                                SSP_IQ_Dma_Name IQ_DMA = new SSP_IQ_Dma_Name();
                                IQ_DMA.IQ_Dma_Name = chkNational.Value;
                                IQ_DMA.IQ_Dma_Num = "000";
                                _ListOfMarketSelected.Add(IQ_DMA);
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Affiliate Region
                    /*if (!chkAffilAll.Checked || _ViewstateInformation.IsAllStationAllowed == false)
                {
                    foreach (ListItem chk in cblAI.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                    foreach (ListItem chk in cblFJ.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                    foreach (ListItem chk in cblKO.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                    foreach (ListItem chk in cblPT.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                    foreach (ListItem chk in cblUZ.Items)
                    {
                        if (chk.Selected)
                        {
                            SSP_Station_Affil IQAffil = new SSP_Station_Affil();
                            IQAffil.Station_Affil = chk.Text;
                            IQAffil.Station_Affil_Num = chk.Value;
                            ListOfAffilsSelected.Add(IQAffil);
                        }
                    }

                }*/


                    if (!chkAffilAll.Checked || _viewstateInformation.IsAllStationAllowed == false)
                    {
                        foreach (RepeaterItem rptitm in rptTVStationSubMaster.Items)
                        {
                            Repeater rptTVStationChild = (Repeater)rptitm.FindControl("rptTVStationChild");
                            foreach (RepeaterItem repeaterItem in rptTVStationChild.Items)
                            {
                                HtmlInputCheckBox chkTVStation = (HtmlInputCheckBox)repeaterItem.FindControl("chkTVStation");
                                if (chkTVStation.Checked)
                                {
                                    _ListOfAffilsSelected.Add(chkTVStation.Value);
                                }
                            }
                        }
                    }
                    #endregion

                    #region Category
                    if (!chkCategoryAll.Checked || _viewstateInformation.IsAllClassAllowed == false)
                    {
                        foreach (RepeaterItem rptItem in rptCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkClass");
                            if (chk.Checked)
                            {
                                Label label = (Label)rptItem.FindControl("lblClass");
                                SSP_IQ_Class IQClass = new SSP_IQ_Class();
                                IQClass.IQ_Class = label.Text;
                                IQClass.IQ_Class_Num = chk.Value;
                                _ListOfProgramSelected.Add(IQClass);
                            }
                        }
                    }

                    #endregion

                    #region Duration Calculation

                    _FromTime = (Convert.ToInt32(rdAmPmFromDate.SelectedValue) - 12) + Convert.ToInt32(ddlStartTime.SelectedValue);
                    _ToTime = (Convert.ToInt32(rdAMPMToDate.SelectedValue) - 12) + Convert.ToInt32(ddlEndTime.SelectedValue);

                    if (_FromTime == 24)
                    {
                        _FromTime = 12;
                    }

                    if (_ToTime == 24)
                    {
                        _ToTime = 12;
                    }

                    _FromDate = null;
                    _ToDate = null;
                    _DayDuration = string.Empty;

                    if (rbDuration1.Checked)
                    {
                        _DayDuration = ddlDuration.SelectedValue;
                        //if (ddlDuration.SelectedValue == "0") // 3 Months
                        //{
                        //    _DayDuration = "3";
                        //}
                        //else if (ddlDuration.SelectedValue == "1") // 6 Months
                        //{
                        //    _DayDuration = "6";
                        //}
                        //else if (ddlDuration.SelectedValue == "2") // 1 Year
                        //{
                        //    _DayDuration = "12";
                        //}
                        //else
                        //{
                        //    _DayDuration = "All";
                        //}
                    }
                    else
                    {
                        _FromDate = Convert.ToDateTime(txtStartDate.Text);
                        _ToDate = Convert.ToDateTime(txtEndDate.Text);
                    }

                    #endregion

                    #region Saved search's current request

                    savedSearchRequestThis.tv = new TV();

                    savedSearchRequestThis.tv.tvDuration = new Duration();
                    if (rbDuration1.Checked)
                    {
                        savedSearchRequestThis.tv.tvDuration.DayDuration = _DayDuration;
                    }
                    else
                    {

                        savedSearchRequestThis.tv.tvDuration.FromDate = new DateTime(_FromDate.Value.Year, _FromDate.Value.Month, _FromDate.Value.Day, _FromTime, 0, 0).ToString();
                        //Convert.ToString(_FromDate);
                        savedSearchRequestThis.tv.tvDuration.ToDate = new DateTime(_ToDate.Value.Year, _ToDate.Value.Month, _ToDate.Value.Day, _ToTime, 0, 0).ToString();
                        // Convert.ToString(_ToDate);
                    }

                    savedSearchRequestThis.tv.ProgramTitle = txProgramTitle.Text.Trim();
                    savedSearchRequestThis.tv.Appearing = txtAppearing.Text.Trim();

                    var listIQ_Dma = (List<IQ_Dma>)(from dma in _ListOfMarketSelected
                                                    select new IQ_Dma
                                                    {
                                                        name = dma.IQ_Dma_Name,
                                                        num = dma.IQ_Dma_Num
                                                    }).ToList();

                    savedSearchRequestThis.tv.iQDmaSet = new IQ_Dma_Set();
                    savedSearchRequestThis.tv.iQDmaSet.listofIQDma = new List<IQ_Dma>();
                    savedSearchRequestThis.tv.iQDmaSet.listofIQDma.AddRange(listIQ_Dma);
                    savedSearchRequestThis.tv.iQDmaSet.IsAllowAll = (listIQ_Dma.Count > 0 ? false : true);
                    savedSearchRequestThis.tv.iQDmaSet.SelectionMethod = ddlMarket.SelectedValue;

                    savedSearchRequestThis.tv.stationAffiliateSet = new Station_Affiliate_Set();
                    savedSearchRequestThis.tv.stationAffiliateSet.listOfIQStationID = new List<string>();
                    savedSearchRequestThis.tv.stationAffiliateSet.listOfIQStationID = _ListOfAffilsSelected;
                    savedSearchRequestThis.tv.stationAffiliateSet.IsAllowAll = (_ListOfAffilsSelected.Count > 0 ? false : true);

                    var listofIQClass = (List<IQ_Class>)(from iqclass in _ListOfProgramSelected
                                                         select new IQ_Class
                                                         {
                                                             name = iqclass.IQ_Class,
                                                             num = iqclass.IQ_Class_Num
                                                         }).ToList();

                    savedSearchRequestThis.tv.iQClassSet = new IQ_Class_Set();
                    savedSearchRequestThis.tv.iQClassSet.listofIQCLass = new List<IQ_Class>();
                    savedSearchRequestThis.tv.iQClassSet.listofIQCLass.AddRange(listofIQClass);
                    savedSearchRequestThis.tv.iQClassSet.IsAllowAll = (listofIQClass.Count > 0 ? false : true);

                    #endregion

                }

                if (chkNews.Checked && _SessionInformation.IsiQPremiumNM)
                {
                    #region News Filters

                    #region List Filters
                    if (!chkNewsCategorySelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsCategory");
                            if (chk.Checked)
                            {
                                _ListOfNewsCategorySelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkNewsRegionAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsRegion.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsregion");
                            if (chk.Checked)
                            {
                                _ListOfNewsRegionSelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkNewsGenreSelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsGenre.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsGenre");
                            if (chk.Checked)
                            {
                                _ListOfNewsGenreSelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkNewsPublicationCategory.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsPublicationCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsPublicationCategory");
                            if (chk.Checked)
                            {
                                _ListOfNewsPublicationCategorySelected.Add(chk.Value);
                            }
                        }
                    }
                    #endregion

                    #region News Time Filter

                    _NewsFromTime = (Convert.ToInt32(rbNewsStart.SelectedValue) - 12) + Convert.ToInt32(ddlNewsStartHour.SelectedValue);
                    _NewsToTime = (Convert.ToInt32(rbNewsEnd.SelectedValue) - 12) + Convert.ToInt32(ddlNewsEndHour.SelectedValue);

                    if (_NewsFromTime == 24)
                    {
                        _NewsFromTime = 12;
                    }

                    if (_NewsToTime == 24)
                    {
                        _NewsToTime = 12;
                    }

                    _NewsFromDate = null;
                    _NewsToDate = null;
                    _NewsDayDuration = string.Empty;

                    if (rbNewsDuration.Checked)
                    {
                        _NewsDayDuration = ddlNewsDuration.SelectedValue;
                        //if (ddlNewsDuration.SelectedValue == "0") // 3 Months
                        //{
                        //    _NewsDayDuration = "3";
                        //}
                        //else if (ddlNewsDuration.SelectedValue == "1") // 6 Months
                        //{
                        //    _NewsDayDuration = "6";
                        //}
                        //else if (ddlNewsDuration.SelectedValue == "2") // 1 Year
                        //{
                        //    _NewsDayDuration = "12";
                        //}
                        //else
                        //{
                        //    _NewsDayDuration = "All";
                        //}
                    }
                    else
                    {
                        _NewsFromDate = Convert.ToDateTime(txtNewsStartDate.Text);
                        _NewsToDate = Convert.ToDateTime(txtNewsEndDate.Text);
                    }

                    #endregion

                    #endregion

                    #region Saved search's current request

                    savedSearchRequestThis.news = new Core.HelperClasses.News();

                    savedSearchRequestThis.news.NewsDuration = new Duration();
                    if (rbNewsDuration.Checked)
                    {
                        savedSearchRequestThis.news.NewsDuration.DayDuration = _NewsDayDuration;
                    }
                    else
                    {
                        savedSearchRequestThis.news.NewsDuration.FromDate = new DateTime(_NewsFromDate.Value.Year, _NewsFromDate.Value.Month, _NewsFromDate.Value.Day, _NewsFromTime, 0, 0).ToString();
                        //Convert.ToString(_NewsFromDate);
                        savedSearchRequestThis.news.NewsDuration.ToDate = new DateTime(_NewsToDate.Value.Year, _NewsToDate.Value.Month, _NewsToDate.Value.Day, _NewsToTime, 0, 0).ToString();
                        //Convert.ToString(_NewsToDate);
                    }

                    savedSearchRequestThis.news.Publication = txtNewsPublication.Text.Trim();

                    savedSearchRequestThis.news.newsCategory_Set = new NewsCategory_Set();
                    savedSearchRequestThis.news.newsCategory_Set.listOfNewsCategory = new List<string>();
                    savedSearchRequestThis.news.newsCategory_Set.listOfNewsCategory = _ListOfNewsCategorySelected;
                    savedSearchRequestThis.news.newsCategory_Set.IsAllowAll = (_ListOfNewsCategorySelected.Count > 0 ? false : true);


                    savedSearchRequestThis.news.publicationCategory_Set = new PublicationCategory_Set();
                    savedSearchRequestThis.news.publicationCategory_Set.listofPublicationCategory = new List<string>();
                    savedSearchRequestThis.news.publicationCategory_Set.listofPublicationCategory = _ListOfNewsPublicationCategorySelected;
                    savedSearchRequestThis.news.publicationCategory_Set.IsAllowAll = (_ListOfNewsPublicationCategorySelected.Count > 0 ? false : true);

                    savedSearchRequestThis.news.genre_Set = new Genre_Set();
                    savedSearchRequestThis.news.genre_Set.listOfGenre = new List<string>();
                    savedSearchRequestThis.news.genre_Set.listOfGenre = _ListOfNewsGenreSelected;
                    savedSearchRequestThis.news.genre_Set.IsAllowAll = (_ListOfNewsGenreSelected.Count > 0 ? false : true);

                    savedSearchRequestThis.news.region_Set = new Region_Set();
                    savedSearchRequestThis.news.region_Set.listOfRegion = new List<string>();
                    savedSearchRequestThis.news.region_Set.listOfRegion = _ListOfNewsRegionSelected;
                    savedSearchRequestThis.news.region_Set.IsAllowAll = (_ListOfNewsRegionSelected.Count > 0 ? false : true);

                    #endregion
                }

                if (chkSocialMedia.Checked && _SessionInformation.IsiQPremiumSM)
                {
                    #region Social Media Filter

                    if (!chkSMCategorySelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptSMCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMCategory");
                            if (chk.Checked)
                            {
                                _ListOfSMSourceCategorySelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkSMTypeSelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptSMType.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMType");
                            if (chk.Checked)
                            {
                                _ListOFSMSourceTypeSelected.Add(chk.Value);
                            }
                        }
                    }

                    if (!chkSMRankSelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptSMRank.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMRank");
                            if (chk.Checked)
                            {
                                _ListOFSMSourceRankSelected.Add(chk.Value);
                            }
                        }
                    }

                    #region Social Media Time Filter

                    _SMFromTime = (Convert.ToInt32(rbSMStart.SelectedValue) - 12) + Convert.ToInt32(ddlSMStartHour.SelectedValue);
                    _SMToTime = (Convert.ToInt32(rbSMEnd.SelectedValue) - 12) + Convert.ToInt32(ddlSMEndHour.SelectedValue);

                    if (_SMFromTime == 24)
                    {
                        _SMFromTime = 12;
                    }

                    if (_SMToTime == 24)
                    {
                        _SMToTime = 12;
                    }

                    _SMFromDate = null;
                    _SMToDate = null;
                    _SMDayDuration = string.Empty;

                    if (rbSMDuration.Checked)
                    {
                        _SMDayDuration = ddlSMDuration.SelectedValue;
                    }
                    else
                    {
                        _SMFromDate = Convert.ToDateTime(txtSMStartDate.Text);
                        _SMToDate = Convert.ToDateTime(txtSMEndDate.Text);
                    }

                    #endregion

                    #endregion
                    #region Saved search's current request

                    savedSearchRequestThis.socialMedia = new Core.HelperClasses.SocialMediaElement();

                    savedSearchRequestThis.socialMedia.smDuration = new Duration();
                    if (rbSMDuration.Checked)
                    {
                        savedSearchRequestThis.socialMedia.smDuration.DayDuration = _SMDayDuration;
                    }
                    else
                    {
                        savedSearchRequestThis.socialMedia.smDuration.FromDate = new DateTime(_SMFromDate.Value.Year, _SMFromDate.Value.Month, _SMFromDate.Value.Day, _SMFromTime, 0, 0).ToString();
                        //Convert.ToString(_SMFromDate);
                        savedSearchRequestThis.socialMedia.smDuration.ToDate = new DateTime(_SMToDate.Value.Year, _SMToDate.Value.Month, _SMToDate.Value.Day, _SMToTime, 0, 0).ToString();
                        //Convert.ToString(_SMToDate);
                    }

                    savedSearchRequestThis.socialMedia.Source = txtSMSource.Text.Trim();
                    savedSearchRequestThis.socialMedia.Author = txtSMAuthor.Text.Trim();
                    savedSearchRequestThis.socialMedia.Title = txtSMTitle.Text.Trim();


                    savedSearchRequestThis.socialMedia.sourceType_Set = new SourceType_Set();
                    savedSearchRequestThis.socialMedia.sourceType_Set.listOfSourceType = new List<string>();
                    savedSearchRequestThis.socialMedia.sourceType_Set.listOfSourceType = _ListOFSMSourceTypeSelected;
                    savedSearchRequestThis.socialMedia.sourceType_Set.IsAllowAll = (_ListOFSMSourceTypeSelected.Count > 0 ? false : true);

                    savedSearchRequestThis.socialMedia.sourceCategory_Set = new SourceCategory_Set();
                    savedSearchRequestThis.socialMedia.sourceCategory_Set.listOfSourceCategory = new List<string>();
                    savedSearchRequestThis.socialMedia.sourceCategory_Set.listOfSourceCategory = _ListOfSMSourceCategorySelected;
                    savedSearchRequestThis.socialMedia.sourceCategory_Set.IsAllowAll = (_ListOfSMSourceCategorySelected.Count > 0 ? false : true);

                    savedSearchRequestThis.socialMedia.sourceRank_Set = new SourceRank_Set();
                    savedSearchRequestThis.socialMedia.sourceRank_Set.listofsourceRank = new List<string>();
                    savedSearchRequestThis.socialMedia.sourceRank_Set.listofsourceRank = _ListOFSMSourceRankSelected;
                    savedSearchRequestThis.socialMedia.sourceRank_Set.IsAllowAll = (_ListOFSMSourceRankSelected.Count > 0 ? false : true);

                    #endregion
                }

                if (chkTwitter.Checked && _SessionInformation.IsiQPremiumTwitter)
                {
                    #region Twitter Time Filter

                    if (rbTwitterDuration.Checked)
                    {
                        _TwitterDayDuration = ddlTwitterDuration.SelectedValue;
                    }
                    else
                    {
                        _TwitterFromTime = (Convert.ToInt32(rbTwitterStart.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterStartHour.SelectedValue);
                        _TwitterToTime = (Convert.ToInt32(rbTwitterEnd.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterEndHour.SelectedValue);

                        if (_TwitterFromTime == 24)
                        {
                            _TwitterFromTime = 12;
                        }

                        if (_TwitterToTime == 24)
                        {
                            _TwitterToTime = 12;
                        }

                        _TwitterFromDate = Convert.ToDateTime(txtTwitterStartDate.Text);
                        _TwitterToDate = Convert.ToDateTime(txtTwitterEndDate.Text);
                    }

                    #endregion

                    #region Saved search's current request

                    savedSearchRequestThis.twitter = new Core.HelperClasses.Twitter();

                    savedSearchRequestThis.twitter.twitterDuration = new Duration();
                    if (rbTwitterDuration.Checked)
                    {
                        savedSearchRequestThis.twitter.twitterDuration.DayDuration = _TwitterDayDuration;
                    }
                    else
                    {
                        savedSearchRequestThis.twitter.twitterDuration.FromDate = new DateTime(_TwitterFromDate.Value.Year, _TwitterFromDate.Value.Month, _TwitterFromDate.Value.Day, _TwitterFromTime, 0, 0).ToString();
                        // Convert.ToString(_TwitterFromDate);
                        savedSearchRequestThis.twitter.twitterDuration.ToDate = new DateTime(_TwitterToDate.Value.Year, _TwitterToDate.Value.Month, _TwitterToDate.Value.Day, _TwitterToTime, 0, 0).ToString();
                        //Convert.ToString(_TwitterToDate);
                    }

                    savedSearchRequestThis.twitter.Actor = txtTweetActor.Text.Trim();

                    if (!string.IsNullOrWhiteSpace(txtFollowerCountFrom.Text.Trim()) && !string.IsNullOrWhiteSpace(txtFollowerCountTo.Text.Trim()))
                    {
                        savedSearchRequestThis.twitter.actorFollowersRange = new ActorFollowersRange();
                        savedSearchRequestThis.twitter.actorFollowersRange.From = Convert.ToInt32(txtFollowerCountFrom.Text.Trim());
                        savedSearchRequestThis.twitter.actorFollowersRange.To = Convert.ToInt32(txtFollowerCountTo.Text.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(txtFriendsCountFrom.Text.Trim()) && !string.IsNullOrWhiteSpace(txtFriendsCountTo.Text.Trim()))
                    {
                        savedSearchRequestThis.twitter.actorFriendsRange = new ActorFriendsRange();
                        savedSearchRequestThis.twitter.actorFriendsRange.From = Convert.ToInt32(txtFriendsCountFrom.Text.Trim());
                        savedSearchRequestThis.twitter.actorFriendsRange.To = Convert.ToInt32(txtFriendsCountTo.Text.Trim());
                    }

                    if (!string.IsNullOrWhiteSpace(txtKloutScoreFrom.Text.Trim()) && !string.IsNullOrWhiteSpace(txtKloutScoreTo.Text.Trim()))
                    {
                        savedSearchRequestThis.twitter.kloutScoreRange = new KloutScoreRange();
                        savedSearchRequestThis.twitter.kloutScoreRange.From = Convert.ToInt32(txtKloutScoreFrom.Text.Trim());
                        savedSearchRequestThis.twitter.kloutScoreRange.To = Convert.ToInt32(txtKloutScoreTo.Text.Trim());
                    }

                    #endregion
                }

                if (chkRadio.Checked && _SessionInformation.IsiQPremiumRadio)
                {
                    #region Radio Market Filters

                    if (!chkRadioMarketAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptRadioMarket.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkDma");
                            if (chk.Checked)
                            {
                                string[] numNameOFMarket = chk.Value.Split('#');
                                IQ_STATION iq_Station = new IQ_STATION();
                                iq_Station.dma_name = numNameOFMarket[0];
                                iq_Station.dma_num = numNameOFMarket[1];
                                _ListOFRadioMarket.Add(iq_Station);
                            }
                        }
                    }


                    #endregion

                    #region Radio Time Filter

                    _RadioFromTime = (Convert.ToInt32(rbRadioStart.SelectedValue) - 12) + Convert.ToInt32(ddlRadioStartHour.SelectedValue);
                    _RadioToTime = (Convert.ToInt32(rbRadioEnd.SelectedValue) - 12) + Convert.ToInt32(ddlRadioEndHour.SelectedValue);

                    if (_RadioFromTime == 24)
                    {
                        _RadioFromTime = 12;
                    }

                    if (_RadioToTime == 24)
                    {
                        _RadioToTime = 12;
                    }

                    if (rbRadioDuration.Checked)
                    {
                        _RadioDayDuration = ddlRadioDuration.SelectedValue;
                    }
                    else
                    {
                        _RadioFromDate = Convert.ToDateTime(txtRadioStartDate.Text);
                        _RadioToDate = Convert.ToDateTime(txtRadioEndDate.Text);
                    }

                    #endregion

                    #region Saved search's current request

                    savedSearchRequestThis.radio = new Core.HelperClasses.Radio();

                    savedSearchRequestThis.radio.radioDuration = new Duration();
                    if (rbRadioDuration.Checked)
                    {
                        savedSearchRequestThis.radio.radioDuration.DayDuration = _RadioDayDuration;
                    }
                    else
                    {
                        savedSearchRequestThis.radio.radioDuration.FromDate = new DateTime(_RadioFromDate.Value.Year, _RadioFromDate.Value.Month, _RadioFromDate.Value.Day, _RadioFromTime, 0, 0).ToString();
                        //Convert.ToString(_RadioFromDate);
                        savedSearchRequestThis.radio.radioDuration.ToDate = new DateTime(_RadioToDate.Value.Year, _RadioToDate.Value.Month, _RadioToDate.Value.Day, _RadioToTime, 0, 0).ToString();
                        //Convert.ToString(_RadioToDate);
                    }

                    savedSearchRequestThis.radio.radioIQDmaSet = new IQ_Dma_Set();
                    var iqdmasetRadio = (from iqdma in _ListOFRadioMarket
                                         select new IQ_Dma
                                         {
                                             name = iqdma.dma_name,
                                             num = iqdma.dma_num
                                         }).ToList();
                    savedSearchRequestThis.radio.radioIQDmaSet.listofIQDma = new List<IQ_Dma>();
                    savedSearchRequestThis.radio.radioIQDmaSet.listofIQDma.AddRange(iqdmasetRadio);
                    savedSearchRequestThis.radio.radioIQDmaSet.IsAllowAll = (iqdmasetRadio.Count > 0 ? false : true);



                    #endregion
                }

                #region Generate Xml
                XDocument xmlDocument = new XDocument(new XElement("SearchRequest",
                    new XElement("SearchTerm", txtSearch.Text),
                    !chkTV.Checked ? null :
                    new XElement("TV",
                        new XElement("ProgramTitle", txProgramTitle.Text),
                        new XElement("Appearing", txtAppearing.Text),
                        rbDuration2.Checked ? new XElement("TimeZone", ddlTimeZone.SelectedItem.Text) : null,

                        rbDuration1.Checked ?
                        new XElement("Duration",
                            new XElement("DayDuration", _DayDuration)) :
                        _FromDate != null && _ToDate != null ?
                        new XElement("Duration",
                            new XElement("FromDate", new DateTime(_FromDate.Value.Year, _FromDate.Value.Month, _FromDate.Value.Day, _FromTime, 0, 0).ToString()),
                            new XElement("ToDate", new DateTime(_ToDate.Value.Year, _ToDate.Value.Month, _ToDate.Value.Day, _ToTime, 0, 0).ToString())) : null,

                        _ListOfMarketSelected.Count == 0 ?
                        new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "true"), new XAttribute("SelectionMethod", ddlMarket.SelectedValue)) :
                        new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "false"), new XAttribute("SelectionMethod", ddlMarket.SelectedValue),
                        from SSP_IQ_Dma_Name market in _ListOfMarketSelected
                        select new XElement("IQ_Dma",
                            new XElement("num", market.IQ_Dma_Num),
                            new XElement("name", market.IQ_Dma_Name))),

                        _ListOfAffilsSelected.Count == 0 ?
                        new XElement("Station_Affiliate_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("Station_Affiliate_Set", new XAttribute("IsAllowAll", "false"),
                        from string affil in _ListOfAffilsSelected
                        select new XElement("IQ_Station_ID", affil)),

                        _ListOfProgramSelected.Count == 0 ?
                        new XElement("IQ_Class_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("IQ_Class_Set", new XAttribute("IsAllowAll", "false"),
                        from SSP_IQ_Class program in _ListOfProgramSelected
                        select new XElement("IQ_Class",
                            new XElement("num", program.IQ_Class_Num),
                            new XElement("name", program.IQ_Class)))
                        ),

                        !chkRadio.Checked ? null :
                        new XElement("Radio",
                            rbRadioDuration.Checked ?
                        new XElement("Duration",
                            new XElement("DayDuration", _RadioDayDuration)) :
                        _RadioFromDate != null && _RadioToDate != null ?
                        new XElement("Duration",
                            new XElement("FromDate", new DateTime(_RadioFromDate.Value.Year, _RadioFromDate.Value.Month, _RadioFromDate.Value.Day, _RadioFromTime, 0, 0).ToString()),
                            new XElement("ToDate", new DateTime(_RadioToDate.Value.Year, _RadioToDate.Value.Month, _RadioToDate.Value.Day, _RadioToTime, 0, 0).ToString())) : null,

                            _ListOFRadioMarket.Count == 0 ?
                        new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "false"),
                        from IQ_STATION radioMarket in _ListOFRadioMarket
                        select new XElement("IQ_Dma",
                            new XElement("num", radioMarket.dma_num),
                            new XElement("name", radioMarket.dma_name)))),


                    !chkNews.Checked ? null :
                    new XElement("News",
                        new XElement("Publication", txtNewsPublication.Text),
                        rbNewsDuration.Checked ?
                        new XElement("Duration",
                            new XElement("DayDuration", _NewsDayDuration)) :
                        _NewsFromDate != null && _NewsToDate != null ?
                        new XElement("Duration",
                            new XElement("FromDate", new DateTime(_NewsFromDate.Value.Year, _NewsFromDate.Value.Month, _NewsFromDate.Value.Day, _NewsFromTime, 0, 0).ToString()),
                            new XElement("ToDate", new DateTime(_NewsToDate.Value.Year, _NewsToDate.Value.Month, _NewsToDate.Value.Day, _NewsToTime, 0, 0).ToString())) : null,
                        _ListOfNewsCategorySelected.Count == 0 ?
                        new XElement("NewsCategory_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("NewsCategory_Set", new XAttribute("IsAllowAll", "false"),
                        from string newscategory in _ListOfNewsCategorySelected
                        select new XElement("NewsCategory", newscategory)),

                        _ListOfNewsPublicationCategorySelected.Count == 0 ?
                        new XElement("PublicationCategory_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("PublicationCategory_Set", new XAttribute("IsAllowAll", "false"),
                        from string publicationcategory in _ListOfNewsPublicationCategorySelected
                        select new XElement("PublicationCategory", publicationcategory)),

                        _ListOfNewsGenreSelected.Count == 0 ?
                        new XElement("Genre_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("Genre_Set", new XAttribute("IsAllowAll", "false"),
                        from string genre in _ListOfNewsGenreSelected
                        select new XElement("Genre", genre)),

                        _ListOfNewsRegionSelected.Count == 0 ?
                        new XElement("Region_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("Region_Set", new XAttribute("IsAllowAll", "false"),
                        from string region in _ListOfNewsRegionSelected
                        select new XElement("Region", region))
                        ),

                    !chkSocialMedia.Checked ? null :
                    new XElement("SocialMedia",
                        rbSMDuration.Checked ?
                        new XElement("Duration",
                            new XElement("DayDuration", _SMDayDuration)) :
                        _SMFromDate != null && _SMToDate != null ?
                        new XElement("Duration",
                            new XElement("FromDate", new DateTime(_SMFromDate.Value.Year, _SMFromDate.Value.Month, _SMFromDate.Value.Day, _SMFromTime, 0, 0).ToString()),
                            new XElement("ToDate", new DateTime(_SMToDate.Value.Year, _SMToDate.Value.Month, _SMToDate.Value.Day, _SMToTime, 0, 0).ToString())) : null,
                        new XElement("Source", txtSMSource.Text),
                        new XElement("Author", txtSMAuthor.Text),
                        new XElement("Title", txtSMTitle.Text),

                        _ListOFSMSourceRankSelected.Count == 0 ?
                        new XElement("SourceRank_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("SourceRank_Set", new XAttribute("IsAllowAll", "false"),
                            from string sourcerank in _ListOFSMSourceRankSelected
                            select new XElement("SourceRank", sourcerank)),

                        _ListOfSMSourceCategorySelected.Count == 0 ?
                        new XElement("SourceCategory_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("SourceCategory_Set", new XAttribute("IsAllowAll", "false"),
                            from string sourcecategory in _ListOfSMSourceCategorySelected
                            select new XElement("SourceCategory", sourcecategory)),

                        _ListOFSMSourceTypeSelected.Count == 0 ?
                        new XElement("SourceType_Set", new XAttribute("IsAllowAll", "true")) :
                        new XElement("SourceType_Set", new XAttribute("IsAllowAll", "false"),
                            from string sourcetype in _ListOFSMSourceTypeSelected
                            select new XElement("SourceType", sourcetype))
                    ),

                    !chkTwitter.Checked || !_SessionInformation.IsiQPremiumTwitter ? null :
                        new XElement("Twitter",
                            rbTwitterDuration.Checked ?
                            new XElement("Duration",
                                new XElement("DayDuration", _TwitterDayDuration)) :
                                _TwitterFromDate != null && _TwitterToDate != null ?
                            new XElement("Duration",
                                new XElement("FromDate", new DateTime(_TwitterFromDate.Value.Year, _TwitterFromDate.Value.Month, _TwitterFromDate.Value.Day, _TwitterFromTime, 0, 0).ToString()),
                                new XElement("ToDate", new DateTime(_TwitterToDate.Value.Year, _TwitterToDate.Value.Month, _TwitterToDate.Value.Day, _TwitterToTime, 0, 0).ToString())) : null,
                            new XElement("Actor", txtTweetActor.Text),

                            (!string.IsNullOrWhiteSpace(txtFriendsCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFriendsCountTo.Text)) ?
                             new XElement("ActorFriendsRange",
                                 new XElement("From", txtFriendsCountFrom.Text),
                                 new XElement("To", txtFriendsCountTo.Text)) : null,

                            (!string.IsNullOrWhiteSpace(txtFollowerCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFollowerCountTo.Text)) ?
                             new XElement("ActorFollowersRange",
                                 new XElement("From", txtFollowerCountFrom.Text),
                                 new XElement("To", txtFollowerCountTo.Text)) : null,

                            (!string.IsNullOrWhiteSpace(txtKloutScoreFrom.Text) && !string.IsNullOrWhiteSpace(txtKloutScoreTo.Text)) ?
                             new XElement("KloutScoreRange",
                                 new XElement("From", txtKloutScoreFrom.Text),
                                 new XElement("To", txtKloutScoreTo.Text)) : null
                     )
                 ));
                #endregion

                savedSearch.IQPremiumSearchRequestXml = Convert.ToString(xmlDocument);
                savedSearch.Title = txtTitle.Text.Trim();
                savedSearch.Description = txtDescription.Text.Trim();
                savedSearch.CustomerGUID = new Guid(_SessionInformation.CustomerGUID);
                savedSearch.CategoryGuid = new Guid(ddlCategory.SelectedValue);
                savedSearch.IsDefualtSearch = chkIsDefaultSearch.Checked;
                savedSearch.ID = _viewstateInformation.ListEditSavedSearch.ID;
                savedSearch.IsIQAgent = chkIsIQAgent.Checked;




                int OutStatus = 0;
                int iQAgentStatus = 0;
                string OutTitle = string.Empty;
                bool isSearchTermEqual = false;

                IIQCustomer_SavedSearchController _IIQCustomer_SavedSearchController = _ControllerFactory.CreateObject<IIQCustomer_SavedSearchController>();

                if (chkIsIQAgent.Checked)
                {
                    String searchTerm = _IIQCustomer_SavedSearchController.GetSearchTermByCustomerSavedSearchID(savedSearch.ID, new Guid(_SessionInformation.ClientGUID));
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        SavedSearchRequest savedSearchRequestOther = new SavedSearchRequest();
                        savedSearchRequestOther.tv = new IQMediaGroup.Core.HelperClasses.TV();
                        savedSearchRequestOther.radio = new IQMediaGroup.Core.HelperClasses.Radio();
                        savedSearchRequestOther.news = new IQMediaGroup.Core.HelperClasses.News();
                        savedSearchRequestOther.socialMedia = new Core.HelperClasses.SocialMediaElement();
                        savedSearchRequestOther.twitter = new Core.HelperClasses.Twitter();

                        savedSearchRequestOther = (SavedSearchRequest)CommonFunctions.MakeDeserialiazation(searchTerm, savedSearchRequestOther);


                        if (savedSearchRequestThis.Equals(savedSearchRequestOther))
                        {
                            isSearchTermEqual = true;
                        }
                    }
                }


                _IIQCustomer_SavedSearchController.UpdateCustomerSearch(savedSearch, new Guid(_SessionInformation.ClientGUID), isSearchTermEqual, out OutStatus, out OutTitle, out iQAgentStatus);
                if (OutStatus == 0)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Search Request Updated Successfully');", true);
                    if (iQAgentStatus < 0)
                    {
                        lblIQAgentmsg.Text = "Total no. of iQAgent exceeds the limit.";
                        lblIQAgentmsg.Visible = true;
                        lblIQAgentmsg.ForeColor = System.Drawing.Color.Red;
                    }
                    lblSavedSearchmsg.Text = "Search Request Updated Successfully.";
                    lblSavedSearchmsg.Visible = true;
                    lblSavedSearchmsg.ForeColor = System.Drawing.Color.Green;

                    SaveRequestObject();
                    SetFilters();

                    if (_viewstateInformation.LoadedSavedSearch == null)
                        _viewstateInformation.LoadedSavedSearch = new SavedSearch();

                    _viewstateInformation.LoadedSavedSearch.ID = _viewstateInformation.ListEditSavedSearch.ID;
                    _viewstateInformation.LoadedSavedSearch.Title = txtTitle.Text.Trim();
                    _viewstateInformation.LoadedSavedSearch.IsIQAgent = (iQAgentStatus > 0) ? true : false;
                    _viewstateInformation.LoadedSavedSearch.CustomerGUID = new Guid(_SessionInformation.CustomerGUID);
                    _viewstateInformation.CurrentPageSavedSearch = 0;
                    SetViewstateInformation(_viewstateInformation);


                    txtTitle.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    ddlCategory.SelectedIndex = 0;
                    chkIsDefaultSearch.Checked = false;


                    //mpSaveSearch.Hide();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveSearchModal", "closeModal('pnlSaveSearch');", true);
                    LoadSavedSearch();
                    upSavedSearh.Update();
                    upMainSearch.Update();
                }
                else if (OutStatus == -1)
                {
                    lblmsg.Text = "search with same title already exist, Please try different title.";
                    lblmsg.Visible = true;
                    lblNote.Text = "Note: All Text and Filter Setting are included in the \"Update\"";
                    //lblNote.Attributes.Add("style", "display:block");
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showUpdateButton", "$('#ctl00_Content_Data_ucIQpremium_btnUpdate').show();$('#spnSaveSearchHeader').text('Edit Search');", true);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSubmitbtn", "$('#" + btnSubmit.ClientID + "').hide();", true);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNote", "$('#" + lblNote.ClientID + "').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNote", "$('#" + lblNote.ClientID + "').css(\"display\",\"block\");", true);
                    // commnted filter update on 14-12-2012
                    //upFilter.Update();


                }
                else if (OutStatus == -2)
                {
                    lblmsg.Text = "search with same criteria already exist , with title : " + OutTitle + "";
                    lblmsg.Visible = true;
                    lblNote.Text = "Note: All Text and Filter Setting are included in the \"Update\"";
                    //lblNote.Attributes.Add("style", "display:block");
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showUpdateButton", "$('#ctl00_Content_Data_ucIQpremium_btnUpdate').show();$('#spnSaveSearchHeader').text('Edit Search');", true);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSubmitbtn", "$('#" + btnSubmit.ClientID + "').hide();", true);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNote", "$('#" + lblNote.ClientID + "').show();", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNote", "$('#" + lblNote.ClientID + "').css(\"display\",\"block\");", true);

                    // commnted filter update on 14-12-2012
                    //upFilter.Update();
                }


            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btncsv_click(object sender, EventArgs e)
        {
            try
            {

                Response.Clear();

                Response.Buffer = true;



                Response.Charset = "";

                Response.ContentType = "application/text";
                if ((sender as Button).ID == "btncsv")
                {
                    Response.AddHeader("content-disposition", "attachment;filename=TVChart" + DateTime.Now.Ticks + ".csv");
                    Response.Output.Write(hfsvcData.Value);

                }
                else if ((sender as Button).ID == "btnCSVNews")
                {
                    Response.AddHeader("content-disposition", "attachment;filename=OnlineNewsChart" + DateTime.Now.Ticks + ".csv");
                    Response.Output.Write(hfcsvNewsData.Value);
                }
                else if ((sender as Button).ID == btnCSVSM.ID)
                {
                    Response.AddHeader("content-disposition", "attachment;filename=SocialMediaChart" + DateTime.Now.Ticks + ".csv");
                    Response.Output.Write(hfcsvSMData.Value);
                }
                else if ((sender as Button).ID == btnCSVTwitter.ID)
                {
                    Response.AddHeader("content-disposition", "attachment;filename=TwitterChart" + DateTime.Now.Ticks + ".csv");
                    Response.Output.Write(hfcsvTwitterData.Value);
                }

                Response.Flush();

                Response.End();
                /*Response.Clear();

                Response.Buffer = true;

                Response.AddHeader("content-disposition",

                 "attachment;filename=CSVData.csv");


                Response.Charset = "";

                Response.ContentType = "application/text";

                string csvdata = hfsvcData.Value;

                Response.Output.Write(csvdata);

                Response.Flush();

                Response.End();*/
            }
            catch (ThreadAbortException exthread)
            {

            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        protected void btnSaveArticle_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnArticleType.Value == "Twitter")
                {
                    IArchiveTweetsController _IArchiveTweetsController = _ControllerFactory.CreateObject<IArchiveTweetsController>();
                    ArchiveTweets _ArchiveTweets = new ArchiveTweets();

                    Guid? _NullCategoryGUID = null;

                    TwitterResult _TwitterResult = (TwitterResult)_viewstateInformation.TweeterResult;

                    _ArchiveTweets.Title = txtArticleTitle.Text;
                    _ArchiveTweets.Keywords = txtKeywords.Text;
                    _ArchiveTweets.Description = txtADescription.Text;
                    _ArchiveTweets.CustomerGuid = new Guid(_SessionInformation.CustomerGUID);
                    _ArchiveTweets.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                    _ArchiveTweets.CategoryGuid = new Guid(ddlPCategory.SelectedValue);
                    _ArchiveTweets.SubCategory1Guid = ddlSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory1.SelectedValue);
                    _ArchiveTweets.SubCategory2Guid = ddlSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory2.SelectedValue);
                    _ArchiveTweets.SubCategory3Guid = ddlSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory3.SelectedValue);
                    _ArchiveTweets.Tweet_ID = Convert.ToInt64(hdnSaveArticleID.Value);
                    _ArchiveTweets.Actor_DisplayName = _TwitterResult.actor_displayName;
                    _ArchiveTweets.Actor_PreferredUserName = _TwitterResult.actor_prefferedUserName;
                    _ArchiveTweets.Actor_FollowersCount = _TwitterResult.followers_count;
                    _ArchiveTweets.Actor_FriendsCount = _TwitterResult.friends_count;
                    _ArchiveTweets.Actor_link = _TwitterResult.actor_link;
                    _ArchiveTweets.Tweet_Body = _TwitterResult.tweet_body;
                    _ArchiveTweets.Actor_Image = _TwitterResult.actor_image;
                    _ArchiveTweets.Tweet_PostedDateTime = Convert.ToDateTime(_TwitterResult.tweet_postedDateTime);
                    _ArchiveTweets.gnip_Klout_Score = _TwitterResult.Klout_score;
                    _ArchiveTweets.Rating = Convert.ToInt16(txtArticleRate.Text);

                    string _Result = _IArchiveTweetsController.InsertArchiveTweet(_ArchiveTweets);

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
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticleModal", "ShowModal('pnlSaveArticle');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Tweet Saved Successfully');", true);

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticleModal", "closeModal('pnlSaveArticle');", true);
                    }

                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnSaveArticle1_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnSaveArticleType.Value == "NM")
                {

                    IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();

                    Guid? _NullCategoryGUID = null;
                    List<ArchiveNM> _ListOfArchiveNM = new List<ArchiveNM>();
                    foreach (GridViewRow gvRow in gvOnlineNews.Rows)
                    {
                        if (gvRow.FindControl("chkSave") != null
                                && ((HtmlInputCheckBox)gvRow.FindControl("chkSave")).Checked)
                        {
                            ArchiveNM _ArchiveNM = new ArchiveNM();

                            _ArchiveNM.Title = gvRow.Cells[2].Text;
                            _ArchiveNM.CustomerGuid = new Guid(_SessionInformation.CustomerGUID);
                            _ArchiveNM.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                            _ArchiveNM.CategoryGuid = new Guid(ddlArticlePCategory.SelectedValue);
                            _ArchiveNM.SubCategory1Guid = ddlArticleSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory1.SelectedValue);
                            _ArchiveNM.SubCategory2Guid = ddlArticleSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory2.SelectedValue);
                            _ArchiveNM.SubCategory3Guid = ddlArticleSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory3.SelectedValue);
                            _ArchiveNM.ArticleID = ((HtmlInputCheckBox)gvRow.FindControl("chkSave")).Value;
                            _ArchiveNM.Url = (gvRow.FindControl("imgArticleButton") as HyperLink).NavigateUrl;

                            _ArchiveNM.Publication = string.IsNullOrWhiteSpace((gvRow.FindControl("aPublication") as HtmlAnchor).HRef)
                                    ? string.Empty : new Uri((gvRow.FindControl("aPublication") as HtmlAnchor).HRef).ToString();

                            _ArchiveNM.CompeteUrl = string.IsNullOrWhiteSpace((gvRow.FindControl("aPublication") as HtmlAnchor).HRef)
                                    ? string.Empty : new Uri((gvRow.FindControl("aPublication") as HtmlAnchor).HRef).Host.Replace("www.", string.Empty);

                            _ArchiveNM.Harvest_Time = Convert.ToDateTime((gvRow.FindControl("hfOnlineNewsHarvestDT") as Label).Text);

                            _ListOfArchiveNM.Add(_ArchiveNM);
                        }
                    }

                    Uri newsSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrNewsUrl]);
                    SearchEngine _SearchEngine = new SearchEngine(newsSearchURI);
                    SearchNewsRequest _SearchNewsRequest = new SearchNewsRequest();
                    _SearchNewsRequest.IsShowContent = true;
                    _SearchNewsRequest.IDs = _ListOfArchiveNM.Select(a => a.ArticleID).ToList();
                    SearchNewsResults _searchNewsResults = _SearchEngine.SearchNews(_SearchNewsRequest);
                    if (_searchNewsResults.newsResults != null && _searchNewsResults.newsResults.Count > 0)
                    {
                        foreach (ArchiveNM _ArchiveNM in _ListOfArchiveNM)
                        {
                            _ArchiveNM.Content = _searchNewsResults.newsResults.Find(a => a.ID.Equals(_ArchiveNM.ArticleID)).Content;
                        }
                    }

                    int Status = 0;
                    int RecordsInserted = 0;

                    XDocument _XDocument = _IArchiveNMController.GenerateListToXML(_ListOfArchiveNM);
                    System.Data.SqlTypes.SqlXml _SqlXML = new System.Data.SqlTypes.SqlXml(_XDocument.CreateReader());
                    string _Result = _IArchiveNMController.InsertArchiveNMByList(_SqlXML, out Status, out RecordsInserted);

                    if (Status != 0)
                    {
                        lblSaveArticleErrMsg.Text = "An error occured, please try again.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticle1Modal", "ShowModal('pnlSaveArticle1');", true);
                    }
                    else
                    {
                        if (RecordsInserted < _ListOfArchiveNM.Count())
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticle1Alert", "alert('" + RecordsInserted.ToString() + " article(s) Saved Successfully, " + (_ListOfArchiveNM.Count() - RecordsInserted).ToString() + " article(s) can not be saved, because it is already saved.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticle1Alert", "alert('Article Saved Successfully');", true);
                        }
                        var newsGeneratePDFsvc = new NewsGeneratePDFWebServiceClient();
                        newsGeneratePDFsvc.WakeupService();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticle1Modal", "closeModal('pnlSaveArticle1');", true);
                    }
                }
                else if (hdnSaveArticleType.Value == "SM")
                {
                    ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();

                    Guid? _NullCategoryGUID = null;
                    List<ArchiveSM> _ListOfArchiveSM = new List<ArchiveSM>();
                    foreach (GridViewRow gvRow in gvSocialMedia.Rows)
                    {
                        if (gvRow.FindControl("chkSave") != null
                                && ((HtmlInputCheckBox)gvRow.FindControl("chkSave")).Checked)
                        {
                            ArchiveSM _ArchiveSM = new ArchiveSM();

                            _ArchiveSM.Title = gvRow.Cells[2].Text;
                            _ArchiveSM.CustomerGuid = new Guid(_SessionInformation.CustomerGUID);
                            _ArchiveSM.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                            _ArchiveSM.CategoryGuid = new Guid(ddlArticlePCategory.SelectedValue);
                            _ArchiveSM.SubCategory1Guid = ddlArticleSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory1.SelectedValue);
                            _ArchiveSM.SubCategory2Guid = ddlArticleSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory2.SelectedValue);
                            _ArchiveSM.SubCategory3Guid = ddlArticleSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory3.SelectedValue);
                            _ArchiveSM.ArticleID = ((HtmlInputCheckBox)gvRow.FindControl("chkSave")).Value;
                            _ArchiveSM.Url = (gvRow.FindControl("imgSMArticleButton") as HyperLink).NavigateUrl;
                            _ArchiveSM.Publication = string.IsNullOrWhiteSpace((gvRow.FindControl("aPublication") as HtmlAnchor).HRef)
                                    ? string.Empty : new Uri((gvRow.FindControl("aPublication") as HtmlAnchor).HRef).Host.Replace("www.", string.Empty);
                            _ArchiveSM.Harvest_Time = Convert.ToDateTime((gvRow.FindControl("hfsocialMediaHarvestDT") as Label).Text);
                            _ArchiveSM.FeedClass = ((gvRow.FindControl("lblfeedClass") as Label).Text);

                            _ArchiveSM.homeLink = string.IsNullOrWhiteSpace((gvRow.FindControl("aPublication") as HtmlAnchor).HRef)
                                    ? string.Empty : new Uri((gvRow.FindControl("aPublication") as HtmlAnchor).HRef).ToString();

                            _ListOfArchiveSM.Add(_ArchiveSM);
                        }
                    }


                    Uri newsSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrSMUrl]);
                    SearchEngine _SearchEngine = new SearchEngine(newsSearchURI);
                    SearchSMRequest _SearchSMRequest = new SearchSMRequest();
                    _SearchSMRequest.isShowContent = true;
                    _SearchSMRequest.ids = _ListOfArchiveSM.Select(a => a.ArticleID).ToList();
                    SearchSMResult _SearchSMResult = _ISocialMediaController.GetSocialMediaGridData(_SearchSMRequest, _SearchEngine);
                    if (_SearchSMResult.smResults != null && _SearchSMResult.smResults.Count > 0)
                    {
                        foreach (ArchiveSM _ArchiveSM in _ListOfArchiveSM)
                        {
                            _ArchiveSM.Content = _SearchSMResult.smResults.Find(a => a.id.Equals(_ArchiveSM.ArticleID)).content;
                        }
                    }

                    int Status = 0;
                    int RecordsInserted = 0;

                    XDocument _XDocument = _ISocialMediaController.GenerateListToXML(_ListOfArchiveSM);
                    System.Data.SqlTypes.SqlXml _SqlXML = new System.Data.SqlTypes.SqlXml(_XDocument.CreateReader());
                    string _Result = _ISocialMediaController.InsertArchiveSMByList(_SqlXML, out Status, out RecordsInserted);


                    if (Status != 0)
                    {
                        lblSaveArticleErrMsg.Text = "An error occured, please try again.";
                        //mdlpopupSaveArticle.Show();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticle1Modal", "ShowModal('pnlSaveArticle1');", true);
                    }
                    else
                    {
                        if (RecordsInserted < _ListOfArchiveSM.Count())
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticle1Alert", "alert('" + RecordsInserted.ToString() + " article(s) Saved Successfully, " + (_ListOfArchiveSM.Count() - RecordsInserted).ToString() + " article(s) can not be saved, because it is already saved.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticle1Alert", "alert('Article Saved Successfully');", true);
                        }
                        var socialGeneratePDFsvc = new SocialGeneratePDFWebServiceClient();
                        socialGeneratePDFsvc.WakeupService();
                        //mdlpopupSaveArticle.Hide();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticle1Modal", "closeModal('pnlSaveArticle1');", true);
                    }

                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void btnResetAll_click(object sender, EventArgs e)
        {
            try
            {
                int _CurrentTime = DateTime.Now.Hour;
                int? _FromTime = null;
                int? _ToTime = null;
                string _Script = string.Empty;
                if (_CurrentTime > 12)
                {
                    _FromTime = _CurrentTime - 12;

                    _Script = "$('#" + rdAMPMToDate.ClientID + " option').eq(24).attr('selected', 'selected');";
                    if (_SessionInformation.IsiQPremiumNM)
                    {
                        _Script += "$('#" + rbNewsEnd.ClientID + " option').eq(24).attr('selected', 'selected');";
                    }
                    if (_SessionInformation.IsiQPremiumSM)
                    {
                        _Script += "$('#" + rbSMEnd.ClientID + " option').eq(24).attr('selected', 'selected');";
                    }
                    if (_SessionInformation.IsiQPremiumTwitter)
                    {
                        _Script += "$('#" + rbTwitterEnd.ClientID + " option').eq(24).attr('selected', 'selected');";
                    }
                    if (_SessionInformation.IsiQPremiumRadio)
                    {
                        _Script += "$('#" + rbRadioEnd.ClientID + " option').eq(24).attr('selected', 'selected');";
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetSMTimer", _Script, true);

                }
                else
                {
                    _FromTime = _CurrentTime;
                    _Script = "$('#" + rdAMPMToDate.ClientID + " option').eq(12).attr('selected', 'selected');";
                    if (_SessionInformation.IsiQPremiumNM)
                    {
                        _Script += "$('#" + rbNewsEnd.ClientID + " option').eq(12).attr('selected', 'selected');";
                    }
                    if (_SessionInformation.IsiQPremiumSM)
                    {
                        _Script += "$('#" + rbSMEnd.ClientID + " option').eq(12).attr('selected', 'selected');";
                    }
                    if (_SessionInformation.IsiQPremiumTwitter)
                    {
                        _Script += "$('#" + rbTwitterEnd.ClientID + " option').eq(12).attr('selected', 'selected');";
                    }

                    if (_SessionInformation.IsiQPremiumRadio)
                    {
                        _Script += "$('#" + rbRadioEnd.ClientID + " option').eq(12).attr('selected', 'selected');";
                    }
                }
                //_ToTime = _FromTime - 1;
                _ToTime = _FromTime;

                #region TV Filter

                #region Time FIlter

                //rbDuration2.Checked = false;
                //rbDuration1.Checked = true;

                //ddlDuration.SelectedIndex = 0;

                _Script = "$('#" + rbDuration1.ClientID + "').attr('checked', true);" +
                    "$('#" + rdAmPmFromDate.ClientID + " option').eq(12).attr('selected', 'selected');" +
                     "$('#" + txtStartDate.ClientID + "').val('" + DateTime.Today.AddDays(-1).ToShortDateString() + "');" +
                    //"$find('CalendarExtenderFromDate').set_selectedDate('" + DateTime.Today.AddDays(-1).ToShortDateString() + "');" +
                    "$('#" + txtEndDate.ClientID + "').val('" + System.DateTime.Now.ToShortDateString() + "');" +
                    "$('#" + ddlStartTime.ClientID + "').val('0');" +
                    "$('#" + ddlEndTime.ClientID + "').val('" + _ToTime.ToString() + "');" +
                    "$('#" + ddlDuration.ClientID + "').attr('disabled', false);" +
                    "$('#" + rbDuration2.ClientID + "').attr('checked', false);" +
                    "$('#" + ddlDuration.ClientID + " option').eq(0).attr('selected', 'selected');" +
                    "$('#divInterval').hide();";

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTVTimer", _Script, true);

                #endregion

                #region Program Filter

                txProgramTitle.Text = string.Empty;
                txtAppearing.Text = string.Empty;

                _Script = "$('#" + txProgramTitle.ClientID + "').val('');" +
                   "$('#" + txtAppearing.ClientID + "').val('');" +
                   "$('#" + txtSearch.ClientID + "').val('');";

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTVProgramFilter", _Script, true);

                #endregion

                #region Market Filter

                #region Region Wise

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetRegion", "$('#" + ddlMarket.ClientID + " option').eq(0).attr('selected', 'selected');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetmarketRegion", "CheckAllCheckBox(divMainRegionFilter);", true);

                #endregion

                #region Rank Wise

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetmarketrank", "CheckAllCheckBox(divMarketRankFilter);", true);

                #endregion

                #endregion

                #region Category Filter
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetTVCategory", "CheckAllCheckBox(divCategoryFilter);", true);

                #endregion

                #region Station Filter

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetTVStation", "CheckAllCheckBox(divStationFilter);", true);

                #endregion

                #region Set all to OFF
                //divProgramFilterStatus.InnerText = "OFF";
                //divProgramFilterStatus.Style.Add("color", "black");
                //imgProgramFilter.Src = "~/images/filter.png";

                //divTimeFilterStatus.InnerText = "OFF";
                //divTimeFilterStatus.Style.Add("color", "black");
                //imgTimeFilter.Src = "~/images/filter.png";

                //divMarketFilterStatus.InnerText = "OFF";
                //divMarketFilterStatus.Style.Add("color", "black");
                //imgMarketFilter.Src = "~/images/filter.png";

                //divStationFilterStatus.InnerText = "OFF";
                //divStationFilterStatus.Style.Add("color", "black");
                //imgStatusFilter.Src = "~/images/filter.png";

                //divCategoryFilterStatus.InnerText = "OFF";
                //divCategoryFilterStatus.Style.Add("color", "black");
                //imgCategoryFilter.Src = "~/images/filter.png";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTitle", "SetFilterStatus('divProgramFilterStatus','imgProgramFilter','OFF');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTime", "SetFilterStatus('divTimeFilterStatus','imgTimeFilter','OFF');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','OFF');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('divStationFilterStatus','imgStatusFilter','OFF');", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCat", "SetFilterStatus('divCategoryFilterStatus','imgCategoryFilter','OFF');", true);

                //divTVFilterStatus.InnerText = String.Empty;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTV", "SetFilterStatus('divTVFilterStatus',null,'');", true);


                #endregion



                #endregion

                if (_SessionInformation.IsiQPremiumNM)
                {
                    #region Online News Filter

                    #region Time Filter

                    //rbNewsInterval.Checked = false;
                    //rbNewsDuration.Checked = true;
                    //ddlNewsDuration.SelectedIndex = 0;


                    _Script = "$('#" + rbNewsDuration.ClientID + "').attr('checked', true);" +
                        "$('#" + rbNewsStart.ClientID + " option').eq(12).attr('selected', 'selected');" +
                        "$('#" + txtNewsStartDate.ClientID + "').val('" + DateTime.Today.AddDays(-1).ToShortDateString() + "');" +
                        "$('#" + txtNewsEndDate.ClientID + "').val('" + System.DateTime.Now.ToShortDateString() + "');" +
                        "$('#" + ddlNewsEndHour.ClientID + "').val('" + _ToTime.ToString() + "');" +
                        "$('#" + ddlNewsStartHour.ClientID + "').val('0');" +
                        "$('#" + ddlNewsDuration.ClientID + "').attr('disabled', false);" +
                       "$('#" + rbNewsInterval.ClientID + "').attr('checked', false);" +
                       "$('#" + ddlNewsDuration.ClientID + " option').eq(0).attr('selected', 'selected');" +
                       "$('#divNewsInterval').hide();";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetNewsTimer", _Script, true);

                    #endregion

                    #region Source

                    //txtNewsPublication.Text = string.Empty;

                    _Script = "$('#" + txtNewsPublication.ClientID + "').val('');";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetNewsSource", _Script, true);


                    #endregion

                    #region Category
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONCategory", "CheckAllCheckBox(divNewsCategory);", true);


                    #endregion

                    #region Publication Category

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONPubCategory", "CheckAllCheckBox(divShowNewsPublicationCategory);", true);

                    #endregion


                    #region Genre

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONGenre", "CheckAllCheckBox(divNewsGenreFilter);", true);

                    #endregion

                    #region News Region


                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONNewsRegion", "CheckAllCheckBox(divNewsRegionFilter);", true);

                    #endregion

                    #region Set All to OFF

                    //divNewsTimeFilterStatus.InnerText = "OFF";
                    //divNewsTimeFilterStatus.Style.Add("color", "black");
                    //imgNewsTimeFilter.Src = "~/images/filter.png";

                    //divNewsPublicationStatus.InnerText = "OFF";
                    //divNewsPublicationStatus.Style.Add("color", "black");
                    //imgShowNewsPublicationFilter.Src = "~/images/filter.png";

                    //divNewsCategoryStatus.InnerText = "OFF";
                    //divNewsCategoryStatus.Style.Add("color", "black");
                    //imgShowNewsCategoryFilter.Src = "~/images/filter.png";

                    //divNewsGenreFilterStatus.InnerText = "OFF";
                    //divNewsGenreFilterStatus.Style.Add("color", "black");
                    //imgNewsGenreFilter.Src = "~/images/filter.png";

                    //divNewsRegionFilterStatus.InnerText = "OFF";
                    //divNewsRegionFilterStatus.Style.Add("color", "black");
                    //imgNewsRegionStatusFilter.Src = "~/images/filter.png";

                    //divShowNewsPublicationCategoryStatus.InnerText = "OFF";
                    //divShowNewsPublicationCategoryStatus.Style.Add("color", "black");
                    //imgShowNewsPublicationCategoryFilter.Src = "~/images/filter.png";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsTime", "SetFilterStatus('divNewsTimeFilterStatus','imgNewsTimeFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPub", "SetFilterStatus('divNewsPublicationStatus','imgShowNewsPublicationFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsCat", "SetFilterStatus('divNewsCategoryStatus','imgShowNewsCategoryFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsGenre", "SetFilterStatus('divNewsGenreFilterStatus','imgNewsGenreFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsRegion", "SetFilterStatus('divNewsRegionFilterStatus','imgNewsRegionStatusFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPCat", "SetFilterStatus('divShowNewsPublicationCategoryStatus','imgShowNewsPublicationCategoryFilter','OFF');", true);


                    //divOnlineNewsFilterStatus.InnerText = String.Empty;

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNews", "SetFilterStatus('divOnlineNewsFilterStatus',null,'');", true);

                    #endregion

                    #endregion
                }

                if (_SessionInformation.IsiQPremiumSM)
                {
                    #region Social Media Filter

                    #region Time Filter

                    _Script = "$('#" + rbSMDuration.ClientID + "').attr('checked', true);" +
                        "$('#" + rbSMStart.ClientID + " option').eq(12).attr('selected', 'selected');" +
                        "$('#" + txtSMStartDate.ClientID + "').val('" + DateTime.Today.AddDays(-1).ToShortDateString() + "');" +
                        "$('#" + txtSMEndDate.ClientID + "').val('" + System.DateTime.Now.ToShortDateString() + "');" +
                        "$('#" + ddlSMEndHour.ClientID + "').val('" + _ToTime.ToString() + "');" +
                        "$('#" + ddlSMStartHour.ClientID + "').val('0');" +
                        "$('#" + ddlSMDuration.ClientID + "').attr('disabled', false);" +
                       "$('#" + rbSMInterval.ClientID + "').attr('checked', false);" +
                       "$('#" + ddlSMDuration.ClientID + " option').eq(0).attr('selected', 'selected');" +
                       "$('#divSMInterval').hide();";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetSMTimer", _Script, true);

                    #endregion

                    #region Source

                    _Script = "$('#" + txtSMAuthor.ClientID + "').val('');" +
                        "$('#" + txtSMSource.ClientID + "').val('');" +
                        "$('#" + txtSMTitle.ClientID + "').val('');";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetSMSource", _Script, true);

                    #endregion

                    #region Rank

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceRank", "CheckAllCheckBox(divSMRank);", true);

                    #endregion

                    #region Source Category
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceCategory", "CheckAllCheckBox(divSMCategory);", true);
                    #endregion

                    #region Source Type
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceType", "CheckAllCheckBox(divSMType);", true);
                    #endregion

                    #region Set all to OFF
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMSource", "SetFilterStatus('divSMSourceStatus','imgShowSMSourceFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMTime", "SetFilterStatus('divSMTimeFilterStatus','imgSMTimeFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMCategory", "SetFilterStatus('divSMCategoryStatus','imgShowSMCategoryFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMType", "SetFilterStatus('divSMTypeStatus','imgShowSMTypeFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMRank", "SetFilterStatus('divSMRankStatus','imgShowSMRankFilter','OFF');", true);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSocialMedia", "SetFilterStatus('divSMFilterStatus',null,'');", true);

                    #endregion

                    #endregion
                }

                if (_SessionInformation.IsiQPremiumTwitter)
                {
                    #region Time Filter

                    _Script = "$('#" + rbTwitterDuration.ClientID + "').attr('checked', true);" +
                        "$('#" + rbTwitterStart.ClientID + " option').eq(12).attr('selected', 'selected');" +
                        "$('#" + txtTwitterStartDate.ClientID + "').val('" + DateTime.Today.AddDays(-1).ToShortDateString() + "');" +
                        "$('#" + txtTwitterEndDate.ClientID + "').val('" + System.DateTime.Now.ToShortDateString() + "');" +
                        "$('#" + ddlTwitterEndHour.ClientID + "').val('" + _ToTime.ToString() + "');" +
                        "$('#" + ddlTwitterStartHour.ClientID + "').val('0');" +
                        "$('#" + ddlTwitterDuration.ClientID + "').attr('disabled', false);" +
                       "$('#" + rbTwitterInterval.ClientID + "').attr('checked', false);" +
                       "$('#" + ddlTwitterDuration.ClientID + " option').eq(0).attr('selected', 'selected');" +
                       "$('#divSMInterval').hide();";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTwitterTimer", _Script, true);

                    #endregion

                    #region Source Filter

                    _Script = "$('#" + txtTweetActor.ClientID + "').val('');";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTwitterSource", _Script, true);

                    #endregion

                    #region Count Filter
                    _Script = "$('#" + txtFollowerCountFrom.ClientID + "').val('');"
                        + "$('#" + txtFollowerCountTo.ClientID + "').val('');"
                        + "$('#" + txtFriendsCountFrom.ClientID + "').val('');"
                        + "$('#" + txtFriendsCountTo.ClientID + "').val('');"
                        + "$('#" + txtKloutScoreFrom.ClientID + "').val('');"
                        + "$('#" + txtKloutScoreTo.ClientID + "').val('');";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTwitterCount", _Script, true);

                    #endregion

                    #region Set all to OFF

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterSource", "SetFilterStatus('divTwitterSourceFilterStatus','imgTwitterSourceFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterTime", "SetFilterStatus('divTwitterTimeFilterStatus','imgTwitterTimeFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterCount", "SetFilterStatus('divTwitterCountFilterStatus','imgTwitterCountFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterScore", "SetFilterStatus('divTwitterScoreFilterStatus','imgTwitterScoreFilter','OFF');", true);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitter", "SetFilterStatus('divTwitterFilterStatus',null,'');", true);

                    #endregion
                }

                if (_SessionInformation.IsiQPremiumRadio)
                {
                    #region Radio Filter

                    #region Time Filter

                    _Script = "$('#" + rbRadioDuration.ClientID + "').attr('checked', true);" +
                        "$('#" + rbRadioStart.ClientID + " option').eq(12).attr('selected', 'selected');" +
                        "$('#" + txtRadioStartDate.ClientID + "').val('" + DateTime.Today.AddDays(-1).ToShortDateString() + "');" +
                        "$('#" + txtRadioEndDate.ClientID + "').val('" + System.DateTime.Now.ToShortDateString() + "');" +
                        "$('#" + ddlRadioEndHour.ClientID + "').val('" + _ToTime.ToString() + "');" +
                        "$('#" + ddlRadioStartHour.ClientID + "').val('0');" +
                        "$('#" + ddlRadioDuration.ClientID + "').attr('disabled', false);" +
                       "$('#" + rbRadioInterval.ClientID + "').attr('checked', false);" +
                       "$('#" + ddlRadioDuration.ClientID + " option').eq(0).attr('selected', 'selected');" +
                       "$('#divRadioInterval').hide();";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetRadioTimer", _Script, true);

                    #endregion


                    #region Radio Matrket Category
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONMarketCategory", "CheckAllCheckBox(divRadioMarketFilter);", true);
                    #endregion

                    #region Set all to OFF

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioTime", "SetFilterStatus('divRadioTimeFilterStatus','imgShowRadioTimeFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioMarket", "SetFilterStatus('divRadioMarketFilterStatus','imgShowRadioMarketFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadio", "SetFilterStatus('divRadioFilterStatus',null,'');", true);
                    #endregion

                    #endregion
                }

                #region Grid and Chart to Null


                rptTV.DataSource = null;
                rptTV.DataBind();
                rptTV.Visible = false;
                divNoResults.Visible = false;

                divLineChart.InnerHtml = String.Empty;

                _Script = "$('#tabTV').css('display','none');"
                        + "$('#divTVResult').css('display','none');"
                        + "$('#tabChartTV').css('display','none');"
                        + "$('#divTVChart').css('display','none');";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTVTab", _Script, true);

                if (_SessionInformation.IsiQPremiumNM)
                {
                    gvOnlineNews.DataSource = null;
                    gvOnlineNews.DataBind();

                    divNewsChart.InnerHtml = String.Empty;
                    _Script = "$('#tabOnlineNews').css('display','none');"
                           + "$('#divOnlineNewsResult').css('display','none');"
                           + "$('#tabChartOL').css('display','none');"
                           + "$('#divOLChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNewsTab", _Script, true);
                }

                if (_SessionInformation.IsiQPremiumSM)
                {
                    gvSocialMedia.DataSource = null;
                    gvSocialMedia.DataBind();

                    divSocialMediaChart.InnerHtml = string.Empty;
                    _Script = "$('#tabSocialMedia').css('display','none');"
                           + "$('#divSocialMediaResult').css('display','none');"
                           + "$('#tabChartSM').css('display','none');"
                           + "$('#divSMChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSocialMediaTab", _Script, true);
                }

                if (_SessionInformation.IsiQPremiumTwitter)
                {
                    dlTweets.DataSource = null;
                    dlTweets.DataBind();
                    divNoResultTwitter.Visible = false;

                    divTwitterChart2.InnerHtml = string.Empty;
                    _Script = "$('#tabTwitter').css('display','none');"
                           + "$('#divTwitterResult').css('display','none');"
                           + "$('#tabChartTwitter').css('display','none');"
                           + "$('#divTwitterChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTwitterTab", _Script, true);
                }

                if (_SessionInformation.IsiQPremiumRadio)
                {
                    grvRadioStations.DataSource = null;
                    grvRadioStations.DataBind();

                    //divradi.InnerHtml = string.Empty;
                    _Script = "$('#tabRadio').css('display','none');"
                           + "$('#divRadioResult').css('display','none');"
                           + "$('#tabChartRadio').css('display','none');"
                           + "$('#divRadioChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideRadioTab", _Script, true);
                }

                //upGrid.Update();
                //upChart.Update();
                #endregion


                _viewstateInformation = GetViewstateInformation();
                //if (_viewstateInformation.SavedSearchSelectedIndex != null)
                //    (gvSavedSearch.Rows[_viewstateInformation.SavedSearchSelectedIndex.Value].FindControl("btnEditSavedSearch") as ImageButton).Visible = false;
                if (gvSavedSearch.Rows.Count > 0)
                    (gvSavedSearch.Rows[0].FindControl("btnEditSavedSearch") as ImageButton).Visible = false;


                gvSavedSearch.SelectedIndex = -1;
                _viewstateInformation.LoadedSavedSearch = null;
                _viewstateInformation.CurrentPageSavedSearch = 0;
                LoadSavedSearch();
                //_viewstateInformation.SavedSearchSelectedIndex = null;
                //_viewstateInformation.SavedSearchSelectedPage = null;
                SetViewstateInformation(_viewstateInformation);
                // commnted filter update on 14-12-2012
                //upFilter.Update();

            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvSavedSearch_Command(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditSearch")
                {
                    IIQCustomer_SavedSearchController _IIQCustomer_SavedSearchController = _ControllerFactory.CreateObject<IIQCustomer_SavedSearchController>();
                    SavedSearch lstsavedSearch = _IIQCustomer_SavedSearchController.GetDataByID(Convert.ToInt32(e.CommandArgument)).FirstOrDefault();

                    if (lstsavedSearch != null)
                    {
                        _viewstateInformation = GetViewstateInformation();
                        _viewstateInformation.ListEditSavedSearch = lstsavedSearch;
                        SetViewstateInformation(_viewstateInformation);

                        txtTitle.Text = lstsavedSearch.Title;
                        txtDescription.Text = lstsavedSearch.Description;
                        ddlCategory.SelectedValue = Convert.ToString(lstsavedSearch.CategoryGuid);
                        chkIsDefaultSearch.Checked = lstsavedSearch.IsDefualtSearch;
                        chkIsIQAgent.Checked = lstsavedSearch.IsIQAgent;
                        lblmsg.Text = string.Empty;
                        lblmsg.Visible = false;

                        /*btnSubmit.Attributes.Add("style", "display:none");
                        btnUpdate.Attributes.Add("style", "display:inline");*/


                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowUpdatebtn", "$('#" + btnUpdate.ClientID + "').show();", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSubmitbtn", "$('#" + btnSubmit.ClientID + "').hide();", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNote", "$('#" + lblNote.ClientID + "').css(\"display\",\"block\");", true);


                        lblNote.Text = "Note: All Text and Filter Setting are included in the \"Update\"";
                        //lblNote.Attributes.Add("style", "display:block");
                        //mpSaveSearch.Show();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveSearchModal", "ShowModal('pnlSaveSearch');", true);
                        upEditSaveSearch.Update();


                    }
                    // commnted filter update on 14-12-2012
                    //upFilter.Update();
                }
                else if (e.CommandName == "LoadSearch")
                {
                    //Clear All Grid and Chart
                    ClearAllGridChart();
                    IIQCustomer_SavedSearchController _IIQCustomer_SavedSearchController = _ControllerFactory.CreateObject<IIQCustomer_SavedSearchController>();
                    SavedSearch lstsavedSearch = _IIQCustomer_SavedSearchController.GetDataByID(Convert.ToInt32(e.CommandArgument)).FirstOrDefault();

                    if (lstsavedSearch != null)
                    {
                        if (!CheckForRightByXML(Convert.ToString(lstsavedSearch.IQPremiumSearchRequestXml)))
                        {
                            lblSavedSearchmsg.Text = "You do not have rights to view complete search.";
                            lblSavedSearchmsg.ForeColor = System.Drawing.Color.Red;
                            lblSavedSearchmsg.Visible = true;
                        }
                        FillFilterByXML(Convert.ToString(lstsavedSearch.IQPremiumSearchRequestXml));

                        LinkButton _LinkButton = e.CommandSource as LinkButton;
                        if (_LinkButton != null)
                        {
                            gvSavedSearch.SelectedIndex = (_LinkButton.NamingContainer as GridViewRow).RowIndex;
                            if (new Guid(((_LinkButton.NamingContainer as GridViewRow).FindControl("hfSavedSearchCustomerGUID") as HiddenField).Value)
                                == new Guid(_SessionInformation.CustomerGUID))
                            {
                                (_LinkButton.NamingContainer as GridViewRow).FindControl("btnEditSavedSearch").Visible = true;
                                (_LinkButton.NamingContainer as GridViewRow).FindControl("btnRemoveSavedSearch").Visible = true;
                            }
                            else
                            {
                                (_LinkButton.NamingContainer as GridViewRow).FindControl("btnEditSavedSearch").Visible = false;
                                (_LinkButton.NamingContainer as GridViewRow).FindControl("btnRemoveSavedSearch").Visible = false;
                            }


                            SaveRequestObject();


                        }
                        else
                        {

                            if (!chkTV.Checked && !chkNews.Checked && !chkSocialMedia.Checked && !chkTwitter.Checked && !chkRadio.Checked)
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideTabDiv", "HideDiv('tabTV');HideDiv('tabOnlineNews');HideDiv('tabSocialMedia');HideDiv('tabTwitter');HideDiv('tabChartTV');HideDiv('tabChartOL');HideDiv('tabChartSM');HideDiv('tabChartTwitter');", true);
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideResultDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');", true);

                                _viewstateInformation.LoadedSavedSearch = lstsavedSearch;
                                _viewstateInformation.CurrentPageSavedSearch = 0;
                                //_viewstateInformation.SavedSearchSelectedIndex = gvSavedSearch.SelectedIndex;
                                //_viewstateInformation.SavedSearchSelectedPage = _viewstateInformation.CurrentPageSavedSearch;
                                LoadSavedSearch();
                                SetViewstateInformation(_viewstateInformation);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSavedSearch", "$('#liSavedSearch').show();", true);
                                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCheckBoxValidation", "alert('Atleast one Filter must be selected');", true);
                                return;
                            }

                            SaveRequestObject();

                            ImageButton _ImageButton = e.CommandSource as ImageButton;
                            gvSavedSearch.SelectedIndex = (_ImageButton.NamingContainer as GridViewRow).RowIndex;
                            //(_ImageButton.NamingContainer as GridViewRow).FindControl("btnEditSavedSearch").Visible = true;

                            if (new Guid(((_ImageButton.NamingContainer as GridViewRow).FindControl("hfSavedSearchCustomerGUID") as HiddenField).Value)
                                == new Guid(_SessionInformation.CustomerGUID))
                            {
                                (_ImageButton.NamingContainer as GridViewRow).FindControl("btnEditSavedSearch").Visible = true;
                                (_ImageButton.NamingContainer as GridViewRow).FindControl("btnRemoveSavedSearch").Visible = true;

                            }
                            else
                            {
                                (_ImageButton.NamingContainer as GridViewRow).FindControl("btnEditSavedSearch").Visible = false;
                                (_ImageButton.NamingContainer as GridViewRow).FindControl("btnRemoveSavedSearch").Visible = false;
                            }


                            if (ValidateCheckBoxes())
                            {

                                if (_viewstateInformation != null)
                                {
                                    _viewstateInformation.listchartZoomHistory = null;
                                    _viewstateInformation.listOnlineNewsChartZoomHistory = null;
                                    _viewstateInformation.listSocialMediaChartZoomHistory = null;
                                    _viewstateInformation.listTwitterChartZoomHistory = null;
                                    SetViewstateInformation(_viewstateInformation);
                                }
                                ucCustomPager.CurrentPage = 0;
                                ucOnlineNewsPager.CurrentPage = 0;
                                ucSMPager.CurrentPage = 0;
                                ucTwitterPager.CurrentPage = 0;
                                ucRadioPager.CurrentPage = 0;

                                switch (GetGridToBind())
                                {
                                    case 0: hfTVStatus.Value = "1"; GetSearchResult((SearchRequest)_viewstateInformation.searchRequestTV, true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');", true);

                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTVTab", "ChangeTab('tabTV','divGridTab',0,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTVChartTab", "ChangeTab('tabChartTV','divChartTab',0,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetOtherTabStatus", "SetNewsFilterStatus(0);SetSMFilterStatus(0);SetTwitterFilterStatus(0);SetRadioFilterStatus(0);", true);
                                        break;

                                    case 1: hfRadioStatus.Value = "1";
                                        BindRadioGrid();
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showRadioTab", "ChangeTab('tabRadio','divGridTab',1,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "shoRadioChartTab", "ChangeTab('tabChartRadio','divChartTab',1,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetRadioStatus", "SetTVFilterStatus(0);SetNewsFilterStatus(0);SetSMFilterStatus(0);SetTwitterFilterStatus(0);", true);

                                        break;

                                    case 2: hfOnlineNewsStatus.Value = "1"; SearchNewsSection((SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews, true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');", true);

                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLTab", "ChangeTab('tabOnlineNews','divGridTab',2,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLChartTab", "ChangeTab('tabChartOL','divChartTab',2,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetOtherTabStatus", "SetTVFilterStatus(0);SetSMFilterStatus(0);SetTwitterFilterStatus(0);SetRadioFilterStatus(0);", true);
                                        break;
                                    case 3: hfSocialMediaStatus.Value = "1"; SearchSocialMediaSection((SearchSMRequest)_viewstateInformation.searchRequestSM, true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divTwitterResultInner');HideDiv('divTwitterChartInner');", true);

                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMTab", "ChangeTab('tabSocialMedia','divGridTab',3,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMChartTab", "ChangeTab('tabChartSM','divChartTab',3,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetOtherTabStatus", "SetTVFilterStatus(0);SetNewsFilterStatus(0);SetTwitterFilterStatus(0);SetRadioFilterStatus(0);", true);
                                        break;
                                    case 4: hfTwitterMediaStatus.Value = "1";
                                        SearchTwitterSection((SearchTwitterRequest)_viewstateInformation.searchRequestTwitter, true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "ChangeTab('tabTwitter','divGridTab',4,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterChartTab", "ChangeTab('tabChartTwitter','divChartTab',4,1);", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideOtherDiv", "HideDiv('divTVResultInner');HideDiv('divTVChartInner');HideDiv('divOnlineNewsResultInner');HideDiv('divOLChartInner');HideDiv('divSocialMediaResultInner');HideDiv('divSMChartInner');", true);
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetOtherTabStatus", "SetTVFilterStatus(0);SetNewsFilterStatus(0);SetSMFilterStatus(0);SetRadioFilterStatus(0);", true);

                                        break;



                                    default: break;
                                }

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show('slow');$(\"#divChartResult\").show('slow');ShowHideDivResult(false);ShowHideDivChart(false);", true);
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "divTab", "$(\"#tabs\").show('slow');", true);
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "divTabChart", "$(\"#chartTab\").show('slow');", true);
                            }
                            //upTab.Update();
                            //upChartTab.Update();
                        }
                        _viewstateInformation.LoadedSavedSearch = lstsavedSearch;
                        _viewstateInformation.CurrentPageSavedSearch = 0;
                        //_viewstateInformation.SavedSearchSelectedIndex = gvSavedSearch.SelectedIndex;
                        //_viewstateInformation.SavedSearchSelectedPage = _viewstateInformation.CurrentPageSavedSearch;
                        LoadSavedSearch();
                        SetViewstateInformation(_viewstateInformation);
                    }

                    // commnted filter update on 14-12-2012

                }
                else if (e.CommandName == "DeleteSearch")
                {
                    IIQCustomer_SavedSearchController _IIQCustomer_SavedSearchController = _ControllerFactory.CreateObject<IIQCustomer_SavedSearchController>();
                    String listofID = String.Empty;

                    String result = _IIQCustomer_SavedSearchController.DeleteCustomerSavedSearch(e.CommandArgument.ToString());

                    if (Convert.ToInt32(result) > 0)
                    {
                        if (((e.CommandSource as ImageButton).NamingContainer as GridViewRow).RowIndex == 0 && _viewstateInformation.LoadedSavedSearch != null)
                        //if (((e.CommandSource as ImageButton).NamingContainer as GridViewRow).RowIndex == _viewstateInformation.SavedSearchSelectedIndex && _viewstateInformation.CurrentPageSavedSearch == _viewstateInformation.SavedSearchSelectedPage)
                        {
                            //_viewstateInformation.SavedSearchSelectedIndex = null;
                            //_viewstateInformation.SavedSearchSelectedPage = null;
                            _viewstateInformation.LoadedSavedSearch = null;
                            _viewstateInformation.CurrentPageSavedSearch = 0;
                            SetViewstateInformation(_viewstateInformation);
                            gvSavedSearch.SelectedIndex = -1;
                        }
                        lblSavedSearchmsg.Text = "Record deleted successfully";
                        lblSavedSearchmsg.ForeColor = System.Drawing.Color.Green;
                        lblSavedSearchmsg.Visible = true;
                        LoadSavedSearch();

                    }
                    else
                    {
                        lblSavedSearchmsg.Text = "Some error occurred, Please Try again.";
                        lblSavedSearchmsg.ForeColor = System.Drawing.Color.Red;
                        lblSavedSearchmsg.Visible = true;
                    }
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSavedSearch", "$('#liSavedSearch').show();", true);
                    // commnted filter update on 14-12-2012
                }
                else if (e.CommandName == "Pager")
                {
                    if (e.CommandArgument != null)
                    {
                        if (e.CommandArgument.ToString() == "First")
                            _viewstateInformation.CurrentPageSavedSearch = 0;
                        else if (e.CommandArgument.ToString() == "Next")
                            _viewstateInformation.CurrentPageSavedSearch += 1;
                        else if (e.CommandArgument.ToString() == "Prev")
                            _viewstateInformation.CurrentPageSavedSearch -= 1;
                        else if (e.CommandArgument.ToString() == "Last")
                            _viewstateInformation.CurrentPageSavedSearch = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(_viewstateInformation.TotalRecordsCountSavedSearch) / gvSavedSearch.PageSize)) - 1;

                        LoadSavedSearch();

                        if (_viewstateInformation.LoadedSavedSearch != null)
                        {
                            gvSavedSearch.SelectedIndex = 0;
                        }
                        //if (_viewstateInformation.SavedSearchSelectedIndex != null)
                        //{
                        //    if (_viewstateInformation.SavedSearchSelectedPage == _viewstateInformation.CurrentPageSavedSearch && _viewstateInformation.SavedSearchSelectedIndex.Value < gvSavedSearch.PageSize)
                        //    {
                        //        gvSavedSearch.SelectedIndex = _viewstateInformation.SavedSearchSelectedIndex.Value;
                        //    }
                        //    else
                        //    {
                        //        gvSavedSearch.SelectedIndex = -1;
                        //    }
                        //}

                        SetViewstateInformation(_viewstateInformation);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSavedSearch", "$('#liSavedSearch').show();", true);
                    }
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvSavedSearch_DataBound(object sender, EventArgs e)
        {
            try
            {
                _viewstateInformation = GetViewstateInformation();
                foreach (GridViewRow _GridViewRow in gvSavedSearch.Rows)
                {
                    LinkButton _lnkbtnTitle = (LinkButton)_GridViewRow.FindControl("lnkbtnTitle");
                    if (_viewstateInformation.LoadedSavedSearch != null && Convert.ToInt32(_lnkbtnTitle.CommandArgument) == _viewstateInformation.LoadedSavedSearch.ID)
                    {
                        gvSavedSearch.SelectedIndex = _GridViewRow.RowIndex;

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSavedSearch", "$('#liSavedSearch').show();", true);
                        SetViewstateInformation(_viewstateInformation);
                    }
                    else
                        (_GridViewRow.FindControl("btnEditSavedSearch") as ImageButton).Visible = false;
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvSavedSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null && ((IQMediaGroup.Core.HelperClasses.SavedSearch)(e.Row.DataItem)).IsIQAgent)
                {
                    e.Row.FindControl("imgIQAgent").Visible = true;
                }
                else
                {
                    e.Row.FindControl("imgIQAgent").Visible = false;
                }

                if (new Guid((e.Row.FindControl("hfSavedSearchCustomerGUID") as HiddenField).Value) != Guid.Empty
                              && new Guid((e.Row.FindControl("hfSavedSearchCustomerGUID") as HiddenField).Value)
                                  == new Guid(_SessionInformation.CustomerGUID))
                {
                    e.Row.FindControl("btnEditSavedSearch").Visible = true;
                    e.Row.FindControl("btnRemoveSavedSearch").Visible = true;
                }
                else
                {
                    e.Row.FindControl("btnEditSavedSearch").Visible = false;
                    e.Row.FindControl("btnRemoveSavedSearch").Visible = false;
                }
            }


        }

        protected void dlTweets_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            if (e.CommandName == "SaveTweet")
            {
                DataListItem _dlTweetItem = ((e.CommandSource as LinkButton).NamingContainer as DataListItem);
                TwitterResult _TweeterResult = new TwitterResult();
                _TweeterResult.tweet_id = Convert.ToInt64(e.CommandArgument.ToString());
                _TweeterResult.tweet_body = (_dlTweetItem.FindControl("lblTweetBody") as Label).Text;
                _TweeterResult.actor_displayName = (_dlTweetItem.FindControl("lblDisplayName") as Label).Text;
                _TweeterResult.actor_link = (_dlTweetItem.FindControl("aActorLink") as HtmlAnchor).HRef;
                _TweeterResult.friends_count = Convert.ToInt64((_dlTweetItem.FindControl("lblActorFriends") as Label).Text.Replace(",", string.Empty));
                _TweeterResult.followers_count = Convert.ToInt64((_dlTweetItem.FindControl("lblActorFollowers") as Label).Text.Replace(",", string.Empty));
                _TweeterResult.actor_image = (_dlTweetItem.FindControl("imgActor") as Image).ImageUrl;
                _TweeterResult.Klout_score = Convert.ToInt64((_dlTweetItem.FindControl("lblKloutScore") as Label).Text);
                _TweeterResult.tweet_postedDateTime = (_dlTweetItem.FindControl("lblPostedDateTime") as Label).Text;
                _TweeterResult.actor_prefferedUserName = (_dlTweetItem.FindControl("lblPrefferedUserName") as Label).Text;
                _viewstateInformation.TweeterResult = _TweeterResult;
                SetViewstateInformation(_viewstateInformation);
                txtArticleRate.Text = "1";
                hdnArticleType.Value = "Twitter";
                hdnSaveArticleID.Value = e.CommandArgument.ToString();
                upSaveArticle.Update();


                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenArticlePopup", "OpenSaveArticlePopup(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "showTwitterTab();", true);

            }
        }

        #endregion

        #region Methods

        protected void GetClientCompeteRights()
        {
            IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
            _viewstateInformation.IsCompeteData = Convert.ToBoolean(_IClientRoleController.GetClientRoleByClientGUIDRoleName(new Guid(_SessionInformation.ClientGUID), RolesName.CompeteData.ToString()));
            SetViewstateInformation(_viewstateInformation);
        }

        protected void GetClientSentimentSettings()
        {
            try
            {
                if (_SessionInformation.IsiQPremiumSentiment)
                {

                    float? TVLowThreshold, TVHighThreshold, NMLowThreshold, NMHighThreshold;
                    float? SMLowThreshold, SMHighThreshold, TwitterLowThreshold, TwitterHighThreshold;
                    IIQClient_CustomSettingsController _IIQClient_CustomSettingsController = _ControllerFactory.CreateObject<IIQClient_CustomSettingsController>();
                    _IIQClient_CustomSettingsController.GetSentimentSettingsByClientGuid(new Guid(_SessionInformation.ClientGUID),
                                                                                            out TVLowThreshold,
                                                                                            out TVHighThreshold,
                                                                                            out NMLowThreshold,
                                                                                            out NMHighThreshold,
                                                                                            out SMLowThreshold,
                                                                                            out SMHighThreshold,
                                                                                            out TwitterLowThreshold,
                                                                                            out TwitterHighThreshold);

                    _viewstateInformation.TVLowThreshold = TVLowThreshold;
                    _viewstateInformation.TVHighThreshold = TVHighThreshold;
                    _viewstateInformation.NMLowThreshold = NMLowThreshold;
                    _viewstateInformation.NMHighThreshold = NMHighThreshold;
                    _viewstateInformation.SMLowThreshold = SMLowThreshold;
                    _viewstateInformation.SMHighThreshold = SMHighThreshold;
                    _viewstateInformation.TwitterLowThreshold = TwitterLowThreshold;
                    _viewstateInformation.TwitterHighThreshold = TwitterHighThreshold;
                    SetViewstateInformation(_viewstateInformation);
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void ClearAllGridChart()
        {
            #region Grid and Chart to Null


            rptTV.DataSource = null;
            rptTV.DataBind();
            rptTV.Visible = false;
            divNoResults.Visible = false;
            divLineChart.InnerHtml = String.Empty;

            string _Script = "$('#tabTV').css('display','none');"
                + "$('#divTVResult').css('display','none');"
                + "$('#tabChartTV').css('display','none');"
                + "$('#divTVChart').css('display','none');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTVTab", _Script, true);
            upGrid.Update();
            upChart.Update();


            grvRadioStations.DataSource = null;
            grvRadioStations.DataBind();
            grvRadioStations.Visible = false;

            _Script = "$('#tabRadio').css('display','none');"
                           + "$('#divRadioResult').css('display','none');"
                           + "$('#tabChartRadio').css('display','none');"
                           + "$('#divRadioChart').css('display','none');";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideRadioTab", _Script, true);
            upRadio.Update();


            gvOnlineNews.DataSource = null;
            gvOnlineNews.DataBind();
            divNewsChart.InnerHtml = String.Empty;

            _Script = "$('#tabOnlineNews').css('display','none');"
                + "$('#divOnlineNewsResult').css('display','none');"
                + "$('#tabChartOL').css('display','none');"
                + "$('#divOLChart').css('display','none');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNewsTab", _Script, true);
            upNewsChart.Update();
            upOnlineNews.Update();



            gvSocialMedia.DataSource = null;
            gvSocialMedia.DataBind();
            divSocialMediaChart.InnerHtml = string.Empty;

            _Script = "$('#tabSocialMedia').css('display','none');"
                + "$('#divSocialMediaResult').css('display','none');"
                + "$('#tabChartSM').css('display','none');"
                + "$('#divSMChart').css('display','none');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSocialMediaTab", _Script, true);
            upSocialMedia.Update();
            upSocialMediaChart.Update();


            dlTweets.DataSource = null;
            dlTweets.DataBind();
            divNoResultTwitter.Visible = false;

            divTwitterChart2.InnerHtml = string.Empty;
            _Script = "$('#tabTwitter').css('display','none');"
                   + "$('#divTwitterResult').css('display','none');"
                   + "$('#tabChartTwitter').css('display','none');"
                   + "$('#divTwitterChart').css('display','none');";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTwitterTab", _Script, true);
            upTwitter.Update();
            upTwitterChart.Update();


            #endregion
        }

        protected void CheckAllCheckBox(Control cnt)
        {
            try
            {
                if (cnt.HasControls())
                {
                    foreach (Control insidecontrol in cnt.Controls)
                    {
                        CheckAllCheckBox(insidecontrol);
                        /*if (insidecontrol.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                        {
                            ((CheckBox)insidecontrol).Checked = true;
                        }*/
                    }
                }
                else if (cnt.GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
                {
                    ((CheckBox)cnt).Checked = true;
                }
                else if (cnt.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlInputCheckBox")
                {
                    ((HtmlInputCheckBox)cnt).Checked = true;

                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        private void SetFilters()
        {
            try
            {
                bool isTVFilterON = false;
                bool isOnlineNewsFilterON = false;
                bool isSocialMediaFilterON = false;
                bool isTwitterFilterON = false;
                bool isRadioFilterON = false;

                #region TV Filter
                if (chkTV.Checked)
                {
                    #region Program Title Status
                    if (!string.IsNullOrWhiteSpace(txProgramTitle.Text) || !string.IsNullOrWhiteSpace(txtAppearing.Text))
                    {
                        //divProgramFilterStatus.InnerText = "ON";
                        //divProgramFilterStatus.Style.Add("color", "red");
                        //imgProgramFilter.Src = "~/images/filter-Selected.png";
                        isTVFilterON = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTitle", "SetFilterStatus('divProgramFilterStatus','imgProgramFilter','ON');", true);

                    }
                    else
                    {
                        //divProgramFilterStatus.InnerText = "OFF";
                        //divProgramFilterStatus.Style.Add("color", "black");
                        //imgProgramFilter.Src = "~/images/filter.png";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTitle", "SetFilterStatus('divProgramFilterStatus','imgProgramFilter','OFF');", true);
                    }
                    #endregion

                    #region Time Filter Status
                    if (rbDuration1.Checked)
                    {
                        if (ddlDuration.SelectedIndex > 0)
                        {
                            //divTimeFilterStatus.InnerText = "ON";
                            //divTimeFilterStatus.Style.Add("color", "red");
                            //imgTimeFilter.Src = "~/images/filter-Selected.png";
                            isTVFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVDuration", "SetFilterStatus('divTimeFilterStatus','imgTimeFilter','ON');", true);
                        }
                        else
                        {
                            //divTimeFilterStatus.InnerText = "OFF";
                            //divTimeFilterStatus.Style.Add("color", "black");
                            //imgTimeFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVDuration", "SetFilterStatus('divTimeFilterStatus','imgTimeFilter','OFF');", true);
                        }
                    }
                    else
                    {
                        //divTimeFilterStatus.InnerText = "ON";
                        //divTimeFilterStatus.Style.Add("color", "red");
                        //imgTimeFilter.Src = "~/images/filter-Selected.png";
                        isTVFilterON = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVDuration", "SetFilterStatus('divTimeFilterStatus','imgTimeFilter','ON');", true);
                    }

                    #endregion

                    #region Market Filter Status
                    if (ddlMarket.SelectedIndex == 0)
                    {
                        #region Set Filter Status
                        if (chlkRegionSelectAll.Checked)
                        {
                            //divMarketFilterStatus.InnerText = "OFF";
                            //divMarketFilterStatus.Style.Add("color", "black");
                            //imgMarketFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVRegion", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','OFF');", true);
                        }
                        else
                        {
                            //divMarketFilterStatus.InnerText = "ON";
                            //divMarketFilterStatus.Style.Add("color", "red");
                            //imgMarketFilter.Src = "~/images/filter-Selected.png";
                            isTVFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVRegion", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','ON');", true);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Set Filter Status
                        if (chkRankFIlterSelectAll.Checked)
                        {
                            //divMarketFilterStatus.InnerText = "OFF";
                            //divMarketFilterStatus.Style.Add("color", "black");
                            //imgMarketFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVRegion", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','OFF');", true);
                        }
                        else
                        {
                            //divMarketFilterStatus.InnerText = "ON";
                            //divMarketFilterStatus.Style.Add("color", "red");
                            //imgMarketFilter.Src = "~/images/filter-Selected.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVRegion", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','ON');", true);
                            isTVFilterON = true;
                        }
                        #endregion
                    }

                    #endregion

                    #region Station Filter Status
                    if (chkAffilAll.Checked)
                    {
                        //divStationFilterStatus.InnerText = "OFF";
                        //divStationFilterStatus.Style.Add("color", "black");
                        //imgStatusFilter.Src = "~/images/filter.png";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('divStationFilterStatus','imgStatusFilter','OFF');", true);
                    }
                    else
                    {
                        //divStationFilterStatus.InnerText = "ON";
                        //divStationFilterStatus.Style.Add("color", "red");
                        //imgStatusFilter.Src = "~/images/filter-Selected.png";
                        isTVFilterON = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('divStationFilterStatus','imgStatusFilter','ON');", true);
                    }

                    #endregion

                    #region Category Filter Status
                    if (chkCategoryAll.Checked)
                    {
                        //divCategoryFilterStatus.InnerText = "OFF";
                        //divCategoryFilterStatus.Style.Add("color", "black");
                        //imgCategoryFilter.Src = "~/images/filter.png";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCategory", "SetFilterStatus('divCategoryFilterStatus','imgCategoryFilter','OFF');", true);

                    }
                    else
                    {
                        //divCategoryFilterStatus.InnerText = "ON";
                        //divCategoryFilterStatus.Style.Add("color", "red");
                        //imgCategoryFilter.Src = "~/images/filter-Selected.png";
                        isTVFilterON = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCategory", "SetFilterStatus('divCategoryFilterStatus','imgCategoryFilter','ON');", true);
                    }
                    #endregion



                }
                else
                {
                    isTVFilterON = false;

                    if (divMainTVFilter.Visible)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTitle", "SetFilterStatus('divProgramFilterStatus','imgProgramFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTime", "SetFilterStatus('divTimeFilterStatus','imgTimeFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('divStationFilterStatus','imgStatusFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCat", "SetFilterStatus('divCategoryFilterStatus','imgCategoryFilter','OFF');", true);
                    }
                }

                if (divMainTVFilter.Visible)
                {
                    if (isTVFilterON)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTV", "SetFilterStatus('divTVFilterStatus',null,'ON');", true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTV", "SetFilterStatus('divTVFilterStatus',null,'');", true);
                    }
                }
                #endregion

                #region Online News Filter Status
                if (_SessionInformation.IsiQPremiumNM)
                {
                    if (chkNews.Checked)
                    {
                        #region Time Filter Status

                        if ((rbNewsDuration.Checked && ddlNewsDuration.SelectedIndex > 0) || rbNewsInterval.Checked)
                        {
                            //divNewsTimeFilterStatus.InnerText = "ON";
                            //divNewsTimeFilterStatus.Style.Add("color", "red");
                            //imgNewsTimeFilter.Src = "~/images/filter-Selected.png";
                            isOnlineNewsFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsTime", "SetFilterStatus('divNewsTimeFilterStatus','imgNewsTimeFilter','ON');", true);


                        }
                        else
                        {
                            //divNewsTimeFilterStatus.InnerText = "OFF";
                            //divNewsTimeFilterStatus.Style.Add("color", "black");
                            //imgNewsTimeFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsTime", "SetFilterStatus('divNewsTimeFilterStatus','imgNewsTimeFilter','OFF');", true);
                        }

                        #endregion

                        #region News Source Filter
                        if (!string.IsNullOrWhiteSpace(txtNewsPublication.Text.Trim()))
                        {
                            //divNewsPublicationStatus.InnerText = "ON";
                            //divNewsPublicationStatus.Style.Add("color", "red");
                            //imgShowNewsPublicationFilter.Src = "~/images/filter-Selected.png";
                            isOnlineNewsFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPub", "SetFilterStatus('divNewsPublicationStatus','imgShowNewsPublicationFilter','ON');", true);

                        }
                        else
                        {
                            //divNewsPublicationStatus.InnerText = "OFF";
                            //divNewsPublicationStatus.Style.Add("color", "black");
                            //imgShowNewsPublicationFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPub", "SetFilterStatus('divNewsPublicationStatus','imgShowNewsPublicationFilter','OFF');", true);
                        }

                        #endregion

                        #region Set News Category Filter Status
                        if (chkNewsCategorySelectAll.Checked)
                        {
                            //divNewsCategoryStatus.InnerText = "OFF";
                            //divNewsCategoryStatus.Style.Add("color", "black");
                            //imgShowNewsCategoryFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsCat", "SetFilterStatus('divNewsCategoryStatus','imgShowNewsCategoryFilter','OFF');", true);
                        }
                        else
                        {
                            //divNewsCategoryStatus.InnerText = "ON";
                            //divNewsCategoryStatus.Style.Add("color", "red");
                            //imgShowNewsCategoryFilter.Src = "~/images/filter-Selected.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsCat", "SetFilterStatus('divNewsCategoryStatus','imgShowNewsCategoryFilter','ON');", true);
                            isOnlineNewsFilterON = true;
                        }
                        #endregion

                        #region Set News Genre Filter Status
                        if (chkNewsGenreSelectAll.Checked)
                        {
                            //divNewsGenreFilterStatus.InnerText = "OFF";
                            //divNewsGenreFilterStatus.Style.Add("color", "black");
                            //imgNewsGenreFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsGenre", "SetFilterStatus('divNewsGenreFilterStatus','imgNewsGenreFilter','OFF');", true);

                        }
                        else
                        {
                            //divNewsGenreFilterStatus.InnerText = "ON";
                            //divNewsGenreFilterStatus.Style.Add("color", "red");
                            //imgNewsGenreFilter.Src = "~/images/filter-Selected.png";
                            isOnlineNewsFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsGenre", "SetFilterStatus('divNewsGenreFilterStatus','imgNewsGenreFilter','ON');", true);
                        }
                        #endregion

                        #region Set News Region Filter Status
                        if (chkNewsRegionAll.Checked)
                        {
                            //divNewsRegionFilterStatus.InnerText = "OFF";
                            //divNewsRegionFilterStatus.Style.Add("color", "black");
                            //imgNewsRegionStatusFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsRegion", "SetFilterStatus('divNewsRegionFilterStatus','imgNewsRegionStatusFilter','OFF');", true);
                        }
                        else
                        {
                            //divNewsRegionFilterStatus.InnerText = "ON";
                            //divNewsRegionFilterStatus.Style.Add("color", "red");
                            //imgNewsRegionStatusFilter.Src = "~/images/filter-Selected.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsRegion", "SetFilterStatus('divNewsRegionFilterStatus','imgNewsRegionStatusFilter','ON');", true);
                            isOnlineNewsFilterON = true;
                        }
                        #endregion

                        #region Set News Publication Filter Status
                        if (chkNewsPublicationCategory.Checked)
                        {
                            //divShowNewsPublicationCategoryStatus.InnerText = "OFF";
                            //divShowNewsPublicationCategoryStatus.Style.Add("color", "black");
                            //imgShowNewsPublicationCategoryFilter.Src = "~/images/filter.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPCat", "SetFilterStatus('divShowNewsPublicationCategoryStatus','imgShowNewsPublicationCategoryFilter','OFF');", true);
                        }
                        else
                        {
                            //divShowNewsPublicationCategoryStatus.InnerText = "ON";
                            //divShowNewsPublicationCategoryStatus.Style.Add("color", "red");
                            //imgShowNewsPublicationCategoryFilter.Src = "~/images/filter-Selected.png";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPCat", "SetFilterStatus('divShowNewsPublicationCategoryStatus','imgShowNewsPublicationCategoryFilter','ON');", true);
                            isOnlineNewsFilterON = true;
                        }
                        #endregion
                    }
                    else
                    {
                        isOnlineNewsFilterON = false;

                        if (divMainOnlineNewsFilter.Visible)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsTime", "SetFilterStatus('divNewsTimeFilterStatus','imgNewsTimeFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPub", "SetFilterStatus('divNewsPublicationStatus','imgShowNewsPublicationFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsCat", "SetFilterStatus('divNewsCategoryStatus','imgShowNewsCategoryFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsGenre", "SetFilterStatus('divNewsGenreFilterStatus','imgNewsGenreFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsRegion", "SetFilterStatus('divNewsRegionFilterStatus','imgNewsRegionStatusFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPCat", "SetFilterStatus('divShowNewsPublicationCategoryStatus','imgShowNewsPublicationCategoryFilter','OFF');", true);
                        }
                    }


                    if (divMainOnlineNewsFilter.Visible)
                    {
                        if (isOnlineNewsFilterON)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNews", "SetFilterStatus('divOnlineNewsFilterStatus',null,'ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNews", "SetFilterStatus('divOnlineNewsFilterStatus',null,'');", true);
                        }
                    }
                }
                #endregion

                #region Social Media Filter Status

                if (_SessionInformation.IsiQPremiumSM)
                {
                    if (chkSocialMedia.Checked)
                    {
                        #region Time Filter Status

                        if ((rbSMDuration.Checked && ddlSMDuration.SelectedIndex > 0) || rbSMInterval.Checked)
                        {
                            isSocialMediaFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMTime", "SetFilterStatus('divSMTimeFilterStatus','imgSMTimeFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMTime", "SetFilterStatus('divNewsTimeFilterStatus','imgNewsTimeFilter','OFF');", true);
                        }

                        #endregion

                        #region Source Filter Status
                        if (!string.IsNullOrWhiteSpace(txtSMSource.Text) || !string.IsNullOrWhiteSpace(txtSMAuthor.Text) || !string.IsNullOrWhiteSpace(txtSMTitle.Text))
                        {
                            isSocialMediaFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMSource", "SetFilterStatus('divSMSourceStatus','imgShowSMSourceFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMSource", "SetFilterStatus('divSMSourceStatus','imgShowSMSourceFilter','OFF');", true);
                        }
                        #endregion

                        #region Source Category Filter Status
                        if (!chkSMCategorySelectAll.Checked)
                        {
                            isSocialMediaFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMCategory", "SetFilterStatus('divSMCategoryStatus','imgShowSMCategoryFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMCategory", "SetFilterStatus('divSMCategoryStatus','imgShowSMCategoryFilter','OFF');", true);
                        }
                        #endregion

                        #region Source Type Filter Status
                        if (!chkSMTypeSelectAll.Checked)
                        {
                            isSocialMediaFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMType", "SetFilterStatus('divSMTypeStatus','imgShowSMTypeFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMType", "SetFilterStatus('divSMTypeStatus','imgShowSMTypeFilter','OFF');", true);
                        }
                        #endregion

                        #region Source Rank Filter Status
                        if (!chkSMRankSelectAll.Checked)
                        {
                            isSocialMediaFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMRank", "SetFilterStatus('divSMRankStatus','imgShowSMRankFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMRank", "SetFilterStatus('divSMRankStatus','imgShowSMRankFilter','OFF');", true);
                        }

                        #endregion


                    }
                    else
                    {
                        isSocialMediaFilterON = false;

                        if (divMainSMFilter.Visible)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMSource", "SetFilterStatus('divSMSourceStatus','imgShowSMSourceFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMTime", "SetFilterStatus('divSMTimeFilterStatus','imgSMTimeFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMCategory", "SetFilterStatus('divSMCategoryStatus','imgShowSMCategoryFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMType", "SetFilterStatus('divSMTypeStatus','imgShowSMTypeFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMRank", "SetFilterStatus('divSMRankStatus','imgShowSMRankFilter','OFF');", true);
                        }
                    }

                    if (divMainSMFilter.Visible)
                    {
                        if (isSocialMediaFilterON)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSocialMedia", "SetFilterStatus('divSMFilterStatus',null,'ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSocialMedia", "SetFilterStatus('divSMFilterStatus',null,'');", true);
                        }
                    }
                }

                #endregion

                #region Twitter Filter Status

                if (_SessionInformation.IsiQPremiumTwitter)
                {
                    if (chkTwitter.Checked)
                    {
                        #region Time Filter Status

                        if ((rbTwitterDuration.Checked && ddlTwitterDuration.SelectedIndex > 0) || rbTwitterInterval.Checked)
                        {
                            isTwitterFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterTime", "SetFilterStatus('divTwitterTimeFilterStatus','imgTwitterTimeFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterTime", "SetFilterStatus('divTwitterTimeFilterStatus','imgTwitterTimeFilter','OFF');", true);
                        }

                        #endregion

                        #region Source Filter Status
                        if (!string.IsNullOrWhiteSpace(txtTweetActor.Text))
                        {
                            isTwitterFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterSource", "SetFilterStatus('divTwitterSourceFilterStatus','imgTwitterSourceFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterSource", "SetFilterStatus('divTwitterSourceFilterStatus','imgTwitterSourceFilter','OFF');", true);
                        }
                        #endregion

                        #region Count Filter Status

                        if ((!string.IsNullOrWhiteSpace(txtFollowerCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFollowerCountTo.Text))
                                || (!string.IsNullOrWhiteSpace(txtFriendsCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFriendsCountTo.Text)))
                        {
                            isTwitterFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterCount", "SetFilterStatus('divTwitterCountFilterStatus','imgTwitterCountFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterCount", "SetFilterStatus('divTwitterCountFilterStatus','imgTwitterCountFilter','OFF');", true);
                        }

                        #endregion

                        #region Score Filter Status
                        if (!string.IsNullOrWhiteSpace(txtKloutScoreFrom.Text) && !string.IsNullOrWhiteSpace(txtKloutScoreTo.Text))
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterScore", "SetFilterStatus('divTwitterScoreFilterStatus','imgTwitterScoreFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterScore", "SetFilterStatus('divTwitterScoreFilterStatus','imgTwitterScoreFilter','OFF');", true);
                        }

                        #endregion
                    }
                    else
                    {
                        isTwitterFilterON = false;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterSource", "SetFilterStatus('divTwitterSourceFilterStatus','imgTwitterSourceFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterTime", "SetFilterStatus('divTwitterTimeFilterStatus','imgTwitterTimeFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterCount", "SetFilterStatus('divTwitterCountFilterStatus','imgTwitterCountFilter','OFF');", true);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterScore", "SetFilterStatus('divTwitterScoreFilterStatus','imgTwitterScoreFilter','OFF');", true);

                    }

                    if (isTwitterFilterON)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitter", "SetFilterStatus('divTwitterFilterStatus',null,'ON');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitter", "SetFilterStatus('divTwitterFilterStatus',null,'');", true);
                    }

                }

                #endregion

                #region Radio Filter Status

                if (_SessionInformation.IsiQPremiumRadio)
                {
                    if (chkRadio.Checked)
                    {
                        #region Time Filter Status

                        if ((rbRadioDuration.Checked && ddlRadioDuration.SelectedIndex > 0) || rbRadioInterval.Checked)
                        {
                            isRadioFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioTime", "SetFilterStatus('divRadioTimeFilterStatus','imgShowRadioTimeFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioTime", "SetFilterStatus('divRadioTimeFilterStatus','imgShowRadioTimeFilter','OFF');", true);
                        }

                        #endregion

                        #region Radio Market Filter Status
                        if (!chkRadioMarketAll.Checked)
                        {
                            isRadioFilterON = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioType", "SetFilterStatus('divRadioMarketFilterStatus','imgShowRadioMarketFilter','ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioType", "SetFilterStatus('divRadioMarketFilterStatus','imgShowRadioMarketFilter','OFF');", true);
                        }
                        #endregion

                        if (isRadioFilterON)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadio", "SetFilterStatus('divRadioFilterStatus',null,'ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadio", "SetFilterStatus('divRadioFilterStatus',null,'');", true);
                        }


                    }
                    else
                    {
                        isRadioFilterON = false;

                        if (divMainRadioFilter.Visible)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioTime", "SetFilterStatus('divRadioTimeFilterStatus','imgShowRadioTimeFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioTime", "SetFilterStatus('divRadioTimeFilterStatus','imgShowRadioTimeFilter','OFF');", true);

                        }
                    }

                    if (divMainRadioFilter.Visible)
                    {
                        if (isRadioFilterON)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadio", "SetFilterStatus('divRadioFilterStatus',null,'ON');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadio", "SetFilterStatus('divRadioFilterStatus',null,'');", true);
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private bool ValidateCheckBoxes()
        {
            try
            {

                bool ReturnValue = true;
                string ValidationMessgae = string.Empty;
                string InvalidTimemsg = "Start Time can not be greater than To Time";
                if (chkTV.Checked)
                {
                    string TVMessage = string.Empty;
                    bool isdmaChecked = false;
                    bool isaffilChecked = false;
                    bool isCategoryChecked = false;

                    #region market Filter Region

                    if (ddlMarket.SelectedIndex == 0)
                    {
                        #region Region Filter
                        if (!chlkRegionSelectAll.Checked || _viewstateInformation.IsAllDmaAllowed == false)
                        {

                            foreach (RepeaterItem rptitm in rptregion.Items)
                            {
                                Repeater rptDmaList = (Repeater)rptitm.FindControl("rptDma");
                                foreach (RepeaterItem repeaterItem in rptDmaList.Items)
                                {
                                    HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                    if (chkboxDma.Checked)
                                    {
                                        isdmaChecked = true;
                                        break;
                                    }
                                }


                            }
                            if (chkRegionNational.Visible && chkRegionNational.Checked)
                            {
                                isdmaChecked = true;
                            }
                        }
                        else
                        {
                            isdmaChecked = true;
                        }


                        #endregion
                    }
                    else
                    {
                        #region Rank Filter
                        List<string> lstMarket = new List<string>();
                        if (!chkRankFIlterSelectAll.Checked || _viewstateInformation.IsAllDmaAllowed == false)
                        {
                            foreach (RepeaterItem repeaterItem in rptTop210.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop150.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop100.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop80.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;

                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop60.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop50.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop40.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop30.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop20.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop10.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chk.Checked)
                                {
                                    isdmaChecked = true; break;
                                }
                            }

                            if (chkNational.Visible && chkNational.Checked)
                            {
                                isdmaChecked = true;
                            }


                        }
                        else
                        {
                            isdmaChecked = true;
                        }

                        #endregion
                    }
                    #endregion

                    #region Affiliate Region
                    if (!chkAffilAll.Checked || _viewstateInformation.IsAllStationAllowed == false)
                    {

                        /*foreach (ListItem chk in cblAI.Items)
                        {
                            if (chk.Selected)
                            {
                                isaffilChecked = true; break;
                            }
                        }

                        foreach (ListItem chk in cblFJ.Items)
                        {
                            if (chk.Selected)
                            {
                                isaffilChecked = true; break;
                            }
                        }

                        foreach (ListItem chk in cblKO.Items)
                        {
                            if (chk.Selected)
                            {
                                isaffilChecked = true; break;
                            }
                        }

                        foreach (ListItem chk in cblPT.Items)
                        {
                            if (chk.Selected)
                            {
                                isaffilChecked = true; break;
                            }
                        }

                        foreach (ListItem chk in cblUZ.Items)
                        {
                            if (chk.Selected)
                            {
                                isaffilChecked = true; break;
                            }
                        }*/

                        foreach (RepeaterItem rptitm in rptTVStationSubMaster.Items)
                        {
                            Repeater rptTVStationChild = (Repeater)rptitm.FindControl("rptTVStationChild");
                            foreach (RepeaterItem repeaterItem in rptTVStationChild.Items)
                            {
                                HtmlInputCheckBox chkTVStation = (HtmlInputCheckBox)repeaterItem.FindControl("chkTVStation");
                                if (chkTVStation.Checked)
                                {
                                    isaffilChecked = true; break;
                                }
                            }
                        }
                    }
                    else
                    {
                        isaffilChecked = true;
                    }



                    #endregion

                    #region Category
                    if (!chkCategoryAll.Checked || _viewstateInformation.IsAllClassAllowed == false)
                    {
                        foreach (RepeaterItem rptItem in rptCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkClass");
                            if (chk.Checked)
                            {
                                isCategoryChecked = true; break;
                            }
                        }
                    }
                    else
                    {
                        isCategoryChecked = true;
                    }

                    #endregion


                    if (!isdmaChecked || !isaffilChecked || !isCategoryChecked)
                    {
                        ReturnValue = false;
                        ValidationMessgae = "TV: Please Select atleast one ";
                        TVMessage = !isdmaChecked ? "Market" : string.Empty;
                        TVMessage += !isaffilChecked ? " Station" : string.Empty;
                        TVMessage += !isCategoryChecked ? " Category" : string.Empty;
                        TVMessage = TVMessage.Trim().Replace(" ", ", ");
                        ValidationMessgae = ValidationMessgae + TVMessage;
                    }

                    if (rbDuration2.Checked)
                    {
                        int _FromTime = (Convert.ToInt32(rdAmPmFromDate.SelectedValue) - 12) + Convert.ToInt32(ddlStartTime.SelectedValue);
                        int _ToTime = (Convert.ToInt32(rdAMPMToDate.SelectedValue) - 12) + Convert.ToInt32(ddlEndTime.SelectedValue);

                        _FromTime = _FromTime == 24 ? 12 : _FromTime;
                        _ToTime = _ToTime == 24 ? 12 : _ToTime;


                        DateTime _FromDate = Convert.ToDateTime(txtStartDate.Text);
                        DateTime _ToDate = Convert.ToDateTime(txtEndDate.Text);
                        _FromDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        _ToDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                        if (_FromDate > _ToDate)
                        {
                            ReturnValue = false;
                            ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                            ValidationMessgae = string.IsNullOrEmpty(TVMessage) ? ValidationMessgae + "TV: " : ValidationMessgae;
                            ValidationMessgae += InvalidTimemsg;
                        }
                    }

                }


                if (chkNews.Checked && _SessionInformation.IsiQPremiumNM)
                {
                    string NewsMessage = string.Empty;
                    bool isOnlineNewsCategoryChecked = false;
                    bool isOnlineNewsPublicationCategoryChecked = false;
                    bool isOnlineNewsGenreChecked = false;
                    bool isOnlineNewsRegionChecked = false;

                    #region Online News Category Region

                    if (!chkNewsCategorySelectAll.Checked)
                    {


                        foreach (RepeaterItem rptitem in rptNewsCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsCategory");
                            if (chk != null)
                            {
                                if (chk.Checked)
                                {
                                    isOnlineNewsCategoryChecked = true;
                                    break;
                                }
                            }
                        }



                    }
                    else
                    {
                        isOnlineNewsCategoryChecked = true;
                    }

                    #endregion

                    #region Online News Genre Region
                    if (!chkNewsGenreSelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsGenre.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsGenre");
                            if (chk != null)
                            {
                                if (chk.Checked)
                                {
                                    isOnlineNewsGenreChecked = true; break;
                                }
                            }
                        }


                    }
                    else
                    {
                        isOnlineNewsGenreChecked = true;
                    }

                    #endregion

                    #region Online "News Region" Region
                    if (!chkNewsRegionAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsRegion.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsregion");
                            if (chk != null)
                            {
                                if (chk.Checked)
                                {
                                    isOnlineNewsRegionChecked = true; break;
                                }
                            }
                        }
                    }
                    else
                    {
                        isOnlineNewsRegionChecked = true;
                    }

                    #endregion

                    #region Publication Category Region
                    if (!chkNewsPublicationCategory.Checked)
                    {

                        foreach (RepeaterItem rptitem in rptNewsPublicationCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsPublicationCategory");
                            if (chk != null)
                            {
                                if (chk.Checked)
                                {
                                    isOnlineNewsPublicationCategoryChecked = true; break;

                                }
                            }
                        }

                    }
                    else
                    {
                        isOnlineNewsPublicationCategoryChecked = true;
                    }
                    #endregion
                    if (!isOnlineNewsCategoryChecked || !isOnlineNewsGenreChecked || !isOnlineNewsPublicationCategoryChecked || !isOnlineNewsRegionChecked)
                    {
                        ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n\\n" : ValidationMessgae;
                        ValidationMessgae += "Online News: Please Select atleast one ";
                        NewsMessage = !isOnlineNewsCategoryChecked ? "News Category" : NewsMessage;
                        NewsMessage += !isOnlineNewsPublicationCategoryChecked ? "\tPublication Category" : string.Empty;
                        NewsMessage += !isOnlineNewsGenreChecked ? "\tGenre" : string.Empty;
                        NewsMessage += !isOnlineNewsRegionChecked ? "\tRegion" : string.Empty;
                        NewsMessage = NewsMessage.Trim().Replace("\t", ", ");
                        ValidationMessgae = ValidationMessgae + NewsMessage;
                        ReturnValue = false;
                    }

                    if (rbNewsInterval.Checked)
                    {
                        int _FromTime = (Convert.ToInt32(rbNewsStart.SelectedValue) - 12) + Convert.ToInt32(ddlNewsStartHour.SelectedValue);
                        int _ToTime = (Convert.ToInt32(rbNewsEnd.SelectedValue) - 12) + Convert.ToInt32(ddlNewsEndHour.SelectedValue);

                        _FromTime = _FromTime == 24 ? 12 : _FromTime;
                        _ToTime = _ToTime == 24 ? 12 : _ToTime;


                        DateTime _FromDate = Convert.ToDateTime(txtNewsStartDate.Text);
                        DateTime _ToDate = Convert.ToDateTime(txtNewsEndDate.Text);
                        _FromDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        _ToDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                        if (_FromDate > _ToDate)
                        {
                            ReturnValue = false;
                            ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                            ValidationMessgae = string.IsNullOrEmpty(NewsMessage) ? ValidationMessgae + "\\nOnline News: " : ValidationMessgae;
                            ValidationMessgae = ValidationMessgae + InvalidTimemsg;
                        }
                    }

                }

                if (chkSocialMedia.Checked && _SessionInformation.IsiQPremiumSM)
                {
                    string SMMessage = string.Empty;
                    bool isSMCategoryChecked = false;
                    bool isSMSourceTypeChecked = false;
                    bool isSmSourceRankChecked = false;

                    #region Source Category
                    foreach (RepeaterItem rptItem in rptSMCategory.Items)
                    {

                        HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkSMCategory");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                isSMCategoryChecked = true; break;
                            }

                        }
                    }
                    #endregion

                    #region Source Type
                    foreach (RepeaterItem rptItem in rptSMType.Items)
                    {

                        HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkSMType");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                isSMSourceTypeChecked = true; break;
                            }

                        }
                    }
                    #endregion

                    #region Sourcr Rank
                    foreach (RepeaterItem rptItem in rptSMRank.Items)
                    {

                        HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkSMRank");
                        if (chk != null)
                        {
                            if (chk.Checked)
                            {
                                isSmSourceRankChecked = true; break;
                            }

                        }
                    }
                    #endregion

                    if (!isSMCategoryChecked || !isSMSourceTypeChecked || !isSmSourceRankChecked)
                    {
                        ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n\\n" : ValidationMessgae;
                        ValidationMessgae += "Social Media: Please Select atleast one ";
                        SMMessage = !isSMCategoryChecked ? "Source Category" : string.Empty;
                        SMMessage += !isSMSourceTypeChecked ? "\tSource Type" : string.Empty;
                        SMMessage += !isSmSourceRankChecked ? "\tSource Rank" : string.Empty;
                        SMMessage = SMMessage.Trim().Replace("\t", ", ");
                        ValidationMessgae = ValidationMessgae + SMMessage;
                        ReturnValue = false;
                    }

                    if (rbSMInterval.Checked)
                    {
                        int _FromTime = (Convert.ToInt32(rbSMStart.SelectedValue) - 12) + Convert.ToInt32(ddlSMStartHour.SelectedValue);
                        int _ToTime = (Convert.ToInt32(rbSMEnd.SelectedValue) - 12) + Convert.ToInt32(ddlSMEndHour.SelectedValue);

                        _FromTime = _FromTime == 24 ? 12 : _FromTime;
                        _ToTime = _ToTime == 24 ? 12 : _ToTime;


                        DateTime _FromDate = Convert.ToDateTime(txtSMStartDate.Text);
                        DateTime _ToDate = Convert.ToDateTime(txtSMEndDate.Text);
                        _FromDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        _ToDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                        if (_FromDate > _ToDate)
                        {
                            ReturnValue = false;
                            ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                            ValidationMessgae = string.IsNullOrEmpty(SMMessage) ? ValidationMessgae + "\\nSocial Media: " : ValidationMessgae;
                            ValidationMessgae = ValidationMessgae + InvalidTimemsg;
                        }
                    }
                }

                if (chkTwitter.Checked && _SessionInformation.IsiQPremiumTwitter)
                {
                    string TwitterMessage = string.Empty;

                    if (rbTwitterInterval.Checked)
                    {
                        int _FromTime = (Convert.ToInt32(rbSMStart.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterStartHour.SelectedValue);
                        int _ToTime = (Convert.ToInt32(rbSMEnd.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterEndHour.SelectedValue);

                        _FromTime = _FromTime == 24 ? 12 : _FromTime;
                        _ToTime = _ToTime == 24 ? 12 : _ToTime;


                        DateTime _FromDate = Convert.ToDateTime(txtTwitterStartDate.Text);
                        DateTime _ToDate = Convert.ToDateTime(txtTwitterEndDate.Text);
                        _FromDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        _ToDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                        if (_FromDate > _ToDate)
                        {
                            ReturnValue = false;
                            ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n\\n" : ValidationMessgae;
                            TwitterMessage = "Twitter: ";
                            ValidationMessgae = ValidationMessgae + TwitterMessage + InvalidTimemsg;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(txtFriendsCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFriendsCountTo.Text))
                    {
                        Int64 FriendRangeFrom = Convert.ToInt64(txtFriendsCountFrom.Text);
                        Int64 FriendRangeTo = Convert.ToInt64(txtFriendsCountTo.Text);

                        if (FriendRangeFrom > FriendRangeTo)
                        {

                            ReturnValue = false;
                            ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                            ValidationMessgae = string.IsNullOrEmpty(TwitterMessage) ? ValidationMessgae + "\\nTwitter: " : ValidationMessgae;
                            TwitterMessage = "Twitter: ";
                            ValidationMessgae = ValidationMessgae + "Invalid Range for Friends Count";
                        }
                    }
                    else if ((string.IsNullOrWhiteSpace(txtFriendsCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFriendsCountTo.Text)) || (!string.IsNullOrWhiteSpace(txtFriendsCountFrom.Text) && string.IsNullOrWhiteSpace(txtFriendsCountTo.Text)))
                    {
                        ReturnValue = false;
                        ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                        ValidationMessgae = string.IsNullOrEmpty(TwitterMessage) ? ValidationMessgae + "\\nTwitter: " : ValidationMessgae;
                        TwitterMessage = "Twitter: ";
                        ValidationMessgae = ValidationMessgae + "Invalid Range for Friends Count";
                    }

                    if (!string.IsNullOrWhiteSpace(txtFollowerCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFollowerCountTo.Text))
                    {
                        Int64 FollowerRangeFrom = Convert.ToInt64(txtFollowerCountFrom.Text);
                        Int64 FollowerRangeTo = Convert.ToInt64(txtFollowerCountTo.Text);

                        if (FollowerRangeFrom > FollowerRangeTo)
                        {
                            ReturnValue = false;
                            ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                            ValidationMessgae = string.IsNullOrEmpty(TwitterMessage) ? ValidationMessgae + "\\nTwitter: " : ValidationMessgae;
                            TwitterMessage = "Twitter: ";
                            ValidationMessgae = ValidationMessgae + "Invalid Range for Followers Count";
                        }
                    }
                    else if ((string.IsNullOrWhiteSpace(txtFollowerCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFollowerCountTo.Text)) || (!string.IsNullOrWhiteSpace(txtFollowerCountFrom.Text) && string.IsNullOrWhiteSpace(txtFollowerCountTo.Text)))
                    {
                        ReturnValue = false;
                        ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                        ValidationMessgae = string.IsNullOrEmpty(TwitterMessage) ? ValidationMessgae + "\\nTwitter: " : ValidationMessgae;
                        TwitterMessage = "Twitter: ";
                        ValidationMessgae = ValidationMessgae + "Invalid Range for Followers Count";
                    }

                    if (!string.IsNullOrWhiteSpace(txtKloutScoreFrom.Text) && !string.IsNullOrWhiteSpace(txtKloutScoreTo.Text))
                    {
                        Int64 ScoreRangeFrom = Convert.ToInt64(txtKloutScoreFrom.Text);
                        Int64 ScoreRangeTo = Convert.ToInt64(txtKloutScoreTo.Text);

                        if (ScoreRangeFrom > ScoreRangeTo)
                        {
                            ReturnValue = false;
                            ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                            ValidationMessgae = string.IsNullOrEmpty(TwitterMessage) ? ValidationMessgae + "\\nTwitter: " : ValidationMessgae;
                            TwitterMessage = "Twitter: ";
                            ValidationMessgae = ValidationMessgae + "Invalid Range for Klout Score";
                        }
                    }
                    else if ((string.IsNullOrWhiteSpace(txtKloutScoreFrom.Text) && !string.IsNullOrWhiteSpace(txtKloutScoreTo.Text)) || (!string.IsNullOrWhiteSpace(txtKloutScoreFrom.Text) && string.IsNullOrWhiteSpace(txtKloutScoreTo.Text)))
                    {
                        ReturnValue = false;
                        ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                        ValidationMessgae = string.IsNullOrEmpty(TwitterMessage) ? ValidationMessgae + "\\nTwitter: " : ValidationMessgae;
                        TwitterMessage = "Twitter: ";
                        ValidationMessgae = ValidationMessgae + "Invalid Range for Klout Score";
                    }
                }

                if (chkRadio.Checked && _SessionInformation.IsiQPremiumRadio)
                {
                    string RadioMessage = string.Empty;
                    bool IsRadioMarketChecked = false;

                    #region Radio Market
                    if (!chkRadioMarketAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptRadioMarket.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkDma");
                            if (chk != null)
                            {
                                if (chk.Checked)
                                {
                                    IsRadioMarketChecked = true; break;

                                }
                            }
                        }

                    }
                    else
                    {
                        IsRadioMarketChecked = true;
                    }

                    if (!IsRadioMarketChecked)
                    {
                        ReturnValue = false;
                        RadioMessage = "Radio: ";
                        ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n\\n" : ValidationMessgae;
                        ValidationMessgae += RadioMessage + "Please Select atleast one Market";
                    }
                    #endregion
                    if (rbRadioInterval.Checked)
                    {
                        int _FromTime = (Convert.ToInt32(rbRadioStart.SelectedValue) - 12) + Convert.ToInt32(ddlRadioStartHour.SelectedValue);
                        int _ToTime = (Convert.ToInt32(rbRadioEnd.SelectedValue) - 12) + Convert.ToInt32(ddlRadioEndHour.SelectedValue);

                        _FromTime = _FromTime == 24 ? 12 : _FromTime;
                        _ToTime = _ToTime == 24 ? 12 : _ToTime;


                        DateTime _FromDate = Convert.ToDateTime(txtRadioStartDate.Text);
                        DateTime _ToDate = Convert.ToDateTime(txtRadioEndDate.Text);
                        _FromDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        _ToDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                        if (_FromDate > _ToDate)
                        {
                            ReturnValue = false;

                            ValidationMessgae = !string.IsNullOrEmpty(ValidationMessgae) ? ValidationMessgae + "\\n" : ValidationMessgae;
                            ValidationMessgae = string.IsNullOrEmpty(RadioMessage) ? ValidationMessgae + "\\nRadio: " : ValidationMessgae;
                            ValidationMessgae = ValidationMessgae + InvalidTimemsg;
                        }
                    }
                }

                if (!ReturnValue && !string.IsNullOrWhiteSpace(ValidationMessgae))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ValidateCheckBox", "javascript:alert('" + ValidationMessgae + "');", true);
                }

                return ReturnValue;
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
                return false;
            }
        }

        private void BindMediaCategoryDropDown()
        {

            try
            {

                string _ClientGUID = _SessionInformation.ClientGUID;
                List<CustomCategory> _ListofCustomCategory = new List<CustomCategory>();

                ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                _ListofCustomCategory = _ICustomCategoryController.SelectByClientGUID(new Guid(_ClientGUID));

                if (_ListofCustomCategory != null && _ListofCustomCategory.Count > 0)
                {
                    ddlCategory.DataTextField = "CategoryName";
                    ddlCategory.DataValueField = "CategoryGUID";
                    ddlCategory.DataSource = _ListofCustomCategory;
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new ListItem("Select Category", "0"));

                    if (_SessionInformation.IsiQPremiumTwitter)
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

                    if (_SessionInformation.IsiQPremiumSM || _SessionInformation.IsiQPremiumNM)
                    {
                        ddlArticlePCategory.DataTextField = "CategoryName";
                        ddlArticlePCategory.DataValueField = "CategoryGUID";
                        ddlArticlePCategory.DataSource = _ListofCustomCategory;
                        ddlArticlePCategory.DataBind();

                        ddlArticlePCategory.Items.Insert(0, new ListItem("<Blank>", "0"));

                        ddlArticleSubCategory1.DataTextField = "CategoryName";
                        ddlArticleSubCategory1.DataValueField = "CategoryGUID";
                        ddlArticleSubCategory1.DataSource = _ListofCustomCategory;
                        ddlArticleSubCategory1.DataBind();

                        ddlArticleSubCategory1.Items.Insert(0, new ListItem("<Blank>", "0"));

                        ddlArticleSubCategory2.DataTextField = "CategoryName";
                        ddlArticleSubCategory2.DataValueField = "CategoryGUID";
                        ddlArticleSubCategory2.DataSource = _ListofCustomCategory;
                        ddlArticleSubCategory2.DataBind();

                        ddlArticleSubCategory2.Items.Insert(0, new ListItem("<Blank>", "0"));

                        ddlArticleSubCategory3.DataTextField = "CategoryName";
                        ddlArticleSubCategory3.DataValueField = "CategoryGUID";
                        ddlArticleSubCategory3.DataSource = _ListofCustomCategory;
                        ddlArticleSubCategory3.DataBind();

                        ddlArticleSubCategory3.Items.Insert(0, new ListItem("<Blank>", "0"));
                    }


                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void BindTwitterSortDropDown()
        {
            Dictionary<string, string> _TwitterSortIterms = null;
            string ConfigTwitterSortSettings = ConfigurationManager.AppSettings["TwitterSortSettings"];
            _TwitterSortIterms = ConfigTwitterSortSettings.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(part => part.Split('='))
               .ToDictionary(split => split[0], split => split[1]);

            ddlTwitterSortExp.DataSource = _TwitterSortIterms;
            ddlTwitterSortExp.DataTextField = "Value";
            ddlTwitterSortExp.DataValueField = "Key";
            ddlTwitterSortExp.DataBind();
        }

        public void BindSocialMediaCheckBoxes()
        {
            try
            {
                ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
                Core.HelperClasses.SocialMedia _SocialMedia = _ISocialMediaController.GetSocialMediaFilterData();

                rptSMCategory.DataSource = _SocialMedia.ListOFSourceCategory;
                rptSMCategory.DataBind();

                rptSMType.DataSource = _SocialMedia.ListOFSourceType;
                rptSMType.DataBind();

                rptSMRank.DataSource = Enumerable.Range(1, 10).ToList();
                rptSMRank.DataBind();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void BindOnlineNewsCheckBoxes()
        {
            try
            {
                INB_NewsCategoryController _INB_NewsCategoryController = _ControllerFactory.CreateObject<INB_NewsCategoryController>();
                List<NB_NewsCategory> listNBNewsCategory = _INB_NewsCategoryController.GetAllNewsCategory();

                rptNewsCategory.DataSource = listNBNewsCategory;
                rptNewsCategory.DataBind();

                INB_PublicationCategoryController _INB_PublicationCategoryController = _ControllerFactory.CreateObject<INB_PublicationCategoryController>();
                List<NB_PublicationCategory> listNBPublicationCategory = _INB_PublicationCategoryController.GetAllPublicationCategory();

                _viewstateInformation.listPublicationCategory = listNBPublicationCategory;
                SetViewstateInformation(_viewstateInformation);

                rptNewsPublicationCategory.DataSource = listNBPublicationCategory;
                rptNewsPublicationCategory.DataBind();

                INB_GenreController _INB_GenreController = _ControllerFactory.CreateObject<INB_GenreController>();
                List<NB_Genre> listNBGenre = _INB_GenreController.GetAllGenre();

                rptNewsGenre.DataSource = listNBGenre;
                rptNewsGenre.DataBind();

                INB_RegionController _INB_RegionController = _ControllerFactory.CreateObject<INB_RegionController>();
                List<NB_Region> listNBRegion = _INB_RegionController.GetAllRegion();

                rptNewsRegion.DataSource = listNBRegion;
                rptNewsRegion.DataBind();

            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void BindStatSkedProgData()
        {
            try
            {
                IIQRegionController _IIQRegionController = _ControllerFactory.CreateObject<IIQRegionController>();
                DataSet dsIQRegion = _IIQRegionController.GetAllRegion();

                MasterIQ_Station _MasterIQ_Station = null;
                IIQ_STATIONController _IIQ_STATIONController = _ControllerFactory.CreateObject<IIQ_STATIONController>();


                Boolean IsAllDmaAllowed = true;
                Boolean IsAllStationAllowed = true;
                Boolean IsAllClassAllowed = true;
                _MasterIQ_Station = _IIQ_STATIONController.GetAllDetailWithRegion(new Guid(_SessionInformation.ClientGUID), out IsAllDmaAllowed, out IsAllStationAllowed, out IsAllClassAllowed);

                // ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _viewstateInformation.IsAllDmaAllowed = IsAllDmaAllowed;// m getting it true here. 
                _viewstateInformation.IsAllStationAllowed = IsAllStationAllowed;
                _viewstateInformation.IsAllClassAllowed = IsAllClassAllowed;
                SetViewstateInformation(_viewstateInformation);

                var _MasterStatSkedProgBindigSource = from market in _MasterIQ_Station._ListofMarket
                                                      select new
                                                      {
                                                          IQ_Dma_Num = market.IQ_Dma_Name + '#' + market.IQ_Dma_Num,
                                                          Name_Num = Convert.ToInt32(market.IQ_Dma_Num).ToString() + " - " + market.IQ_Dma_Name,
                                                          iqdmaNum = Convert.ToInt32(market.IQ_Dma_Num)
                                                      };

                #region Market Repeater Bind

                if (_MasterIQ_Station._ListofMarket.Count > 0)
                {

                    #region Rank CheckboxList Bind

                    rptTop10.DataSource = (from p in _MasterStatSkedProgBindigSource
                                           where p.iqdmaNum >= 1 && p.iqdmaNum <= 10
                                           select p).ToList();
                    rptTop10.DataBind();


                    rptTop20.DataSource = (from p in _MasterStatSkedProgBindigSource
                                           where p.iqdmaNum >= 11 && p.iqdmaNum <= 20
                                           select p).ToList();// _MasterStatSkedProgBindigSource.Skip(10).Take(10);
                    rptTop20.DataBind();


                    rptTop30.DataSource = (from p in _MasterStatSkedProgBindigSource
                                           where p.iqdmaNum >= 21 && p.iqdmaNum <= 30
                                           select p).ToList(); //_MasterStatSkedProgBindigSource.Skip(20).Take(10);
                    rptTop30.DataBind();

                    rptTop40.DataSource = (from p in _MasterStatSkedProgBindigSource
                                           where p.iqdmaNum >= 31 && p.iqdmaNum <= 40
                                           select p).ToList(); //_MasterStatSkedProgBindigSource.Skip(30).Take(10);
                    rptTop40.DataBind();


                    rptTop50.DataSource = (from p in _MasterStatSkedProgBindigSource
                                           where p.iqdmaNum >= 41 && p.iqdmaNum <= 50
                                           select p).ToList(); //_MasterStatSkedProgBindigSource.Skip(40).Take(10);
                    rptTop50.DataBind();


                    rptTop60.DataSource = (from p in _MasterStatSkedProgBindigSource
                                           where p.iqdmaNum >= 51 && p.iqdmaNum <= 60
                                           select p).ToList(); //_MasterStatSkedProgBindigSource.Skip(50).Take(10);
                    rptTop60.DataBind();


                    rptTop80.DataSource = (from p in _MasterStatSkedProgBindigSource
                                           where p.iqdmaNum >= 61 && p.iqdmaNum <= 80
                                           select p).ToList(); //_MasterStatSkedProgBindigSource.Skip(60).Take(20);
                    rptTop80.DataBind();


                    rptTop100.DataSource = (from p in _MasterStatSkedProgBindigSource
                                            where p.iqdmaNum >= 81 && p.iqdmaNum <= 100
                                            select p).ToList(); //_MasterStatSkedProgBindigSource.Skip(80).Take(20);
                    rptTop100.DataBind();


                    rptTop150.DataSource = (from p in _MasterStatSkedProgBindigSource
                                            where p.iqdmaNum >= 101 && p.iqdmaNum <= 150
                                            select p).ToList(); //_MasterStatSkedProgBindigSource.Skip(100).Take(50);
                    rptTop150.DataBind();

                    rptTop210.DataSource = (from p in _MasterStatSkedProgBindigSource
                                            where p.iqdmaNum >= 151 && p.iqdmaNum <= 210
                                            select p).ToList(); //_MasterStatSkedProgBindigSource.Skip(150).Take(60);
                    rptTop210.DataBind();

                    marketNational = _MasterIQ_Station._ListofMarket.FirstOrDefault(m => m.IQ_Dma_Num == "000");
                    if (marketNational != null)
                    {
                        chkNational.Visible = true;
                    }
                    else
                    {
                        chkNational.Visible = false;
                    }
                    chkRankFIlterSelectAll.Checked = true;

                    #endregion

                    #region Region CheckBoxList Bind

                    if (dsIQRegion != null && dsIQRegion.Tables.Count > 0 && dsIQRegion.Tables[0].Rows.Count > 0)
                    {
                        rptregion.DataSource = dsIQRegion.Tables[0];
                        rptregion.DataBind();

                        for (int i = 0; i < dsIQRegion.Tables[0].Rows.Count; i++)
                        {
                            int regionID = Convert.ToInt32(dsIQRegion.Tables[0].Rows[i]["ID"]);

                            var _ListofRegionWiseMarket = from market in _MasterIQ_Station._ListofMarket
                                                          where market._RegionID.Equals(regionID)
                                                          orderby market.IQ_Dma_Name
                                                          select new
                                                          {
                                                              IQ_Dma_Num = market.IQ_Dma_Num,
                                                              IQ_Dma_Name = market.IQ_Dma_Name,
                                                              Name_Num = Convert.ToInt32(market.IQ_Dma_Num).ToString() + " - " + market.IQ_Dma_Name
                                                          };

                            Repeater rptDmaList = (Repeater)rptregion.Items[i].FindControl("rptDma");
                            rptDmaList.DataSource = _ListofRegionWiseMarket;
                            rptDmaList.DataBind();

                            if (marketNational != null)
                            {
                                chkRegionNational.Visible = true;
                            }
                            else
                            {
                                chkRegionNational.Visible = false;
                            }
                        }


                    }

                    #endregion


                }

                #endregion

                #region TV Station Repeater Bind


                var distincStationID = _MasterIQ_Station._ListofAffil.Select(x => x.Station_Affil_Cat_Num).Distinct().ToList();
                rptTVStationSubMaster.DataSource = distincStationID;
                rptTVStationSubMaster.DataBind();

                for (int i = 0; i < distincStationID.Count; i++)
                {
                    ((CheckBox)rptTVStationSubMaster.Items[i].FindControl("chkStationSubMaster")).Text = _MasterIQ_Station._ListofAffil.Where(x => x.Station_Affil_Cat_Num.Equals(distincStationID[i])).First().Station_Affil_Cat_Name;
                    ((HiddenField)rptTVStationSubMaster.Items[i].FindControl("hfAffilNum")).Value = Convert.ToString(distincStationID[i]);

                    var listofStation = _MasterIQ_Station._ListofAffil.Where(x => x.Station_Affil_Cat_Num.Equals(distincStationID[i])).OrderBy(o => o.Station_Call_Sign);

                    //((System.Web.UI.HtmlControls.HtmlImage)rptTVStationSubMaster.Items[i].FindControl("imgTVFilterSecectSubMaster")).Attributes.Add("onclick", "OnImageExpandCollapseClick('" + ((System.Web.UI.HtmlControls.HtmlImage)rptTVStationSubMaster.Items[i].FindControl("imgTVFilterSecectSubMaster")).ClientID + "', '" + rptTVStationSubMaster.Items[i].FindControl("divCheckBox").ClientID + "');");

                    Repeater rptTVStationChild = (Repeater)rptTVStationSubMaster.Items[i].FindControl("rptTVStationChild");
                    rptTVStationChild.DataSource = listofStation;
                    rptTVStationChild.DataBind();


                    //CheckBoxList chkTVStationChild = (CheckBoxList)rptTVStationSubMaster.Items[i].FindControl("chkTVStationChild");
                    //chkTVStationChild.DataTextField = "Station_Call_Sign";
                    //chkTVStationChild.DataValueField = "IQ_Station_ID";
                    //chkTVStationChild.DataSource = listofStation;
                    //chkTVStationChild.DataBind();

                    //((CheckBox)rptTVStationSubMaster.Items[i].FindControl("chkStationSubMaster")).Text = ((CheckBox)rptTVStationSubMaster.Items[i].FindControl("chkStationSubMaster")).Text;// +" (" + listofStation.Count() + ")";

                    //((CheckBox)rptTVStationSubMaster.Items[i].FindControl("chkStationSubMaster")).Attributes.Add("onclick", "RegionMainCheckBoxClick(this," + rptTVStationSubMaster.Items[i].FindControl("divCheckBox").ClientID + "," + chkAffilAll.ClientID + ",divAffil);");

                    //foreach (RepeaterItem repeaterItem in rptTVStationChild.Items)
                    //{
                    //    HtmlInputCheckBox chkboxTVStation = (HtmlInputCheckBox)repeaterItem.FindControl("chkTVStation");
                    //    chkboxTVStation.Attributes.Add("onclick", "chkMainRegion(this," + rptTVStationSubMaster.Items[i].FindControl("divCheckBox").ClientID + "," + ((CheckBox)rptTVStationSubMaster.Items[i].FindControl("chkStationSubMaster")).ClientID + "," + chkAffilAll.ClientID + ",divAffil);");
                    //}
                    //chkRegionNational.Attributes.Add("onclick", "MasterSelectallCheckUncheck(this.checked,ctl00_Content_Data_ucIQpremium_chlkRegionSelectAll, divRegionFilter);");
                    //if (marketNational != null)
                    //{
                    //    chkRegionNational.Checked = true;
                    //}
                    //else
                    //{
                    //    chkRegionNational.Visible = false;
                    //}



                }



                #endregion

                #region Category Repeater Bind
                if (_MasterIQ_Station._ListofType.Count > 0)
                {
                    rptCategory.DataSource = _MasterIQ_Station._ListofType;
                    rptCategory.DataBind();
                }
                #endregion

            }
            catch (Exception ex)
            {

                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void GetRadioStations()
        {
            try
            {
                IIQ_STATIONController _IRL_STATIONController = _ControllerFactory.CreateObject<IIQ_STATIONController>();
                List<IQ_STATION> _ListOfRLStation = _IRL_STATIONController.SelectAllRadioStations();

                List<string> _ListOfDMAName = _ListOfRLStation.Select(row => row.dma_name).Distinct().ToList();


                IQ_STATIONComparer iQ_STATIONComparer = new IQ_STATIONComparer();
                List<IQ_STATION> listOFIQStation = _ListOfRLStation.Distinct(iQ_STATIONComparer).ToList();



                _viewstateInformation = GetViewstateInformation();
                _viewstateInformation.ListOfRadioStations = _ListOfRLStation;
                SetViewstateInformation(_viewstateInformation);

                rptRadioMarket.DataSource = listOFIQStation;
                rptRadioMarket.DataBind();

                foreach (RepeaterItem rptitem in rptRadioMarket.Items)
                {
                    HtmlInputCheckBox chkRadioStation = (HtmlInputCheckBox)rptitem.FindControl("chkDma");
                    chkRadioStation.Checked = true;
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void GetSearchResult(SearchRequest searchRequest, Boolean needToBindChart)
        {
            try
            {

                /* if (!chkTV.Checked)
                 {

                     grvTV.DataSource = null;
                     grvTV.DataBind();
                     ucCustomPager.Visible = false;
                     return;
                 }*/
                Logger.Info("Get Search Results for TV Request Starts");

                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                //SearchRequest searchRequest = (SearchRequest)_viewstateInformation.searchRequestTV;
                var currentContext = HttpContext.Current;

                if ((_viewstateInformation != null && _viewstateInformation.listchartZoomHistory != null && _viewstateInformation.listchartZoomHistory.Count > 0) || !needToBindChart)
                {
                    Task[] tasks = new Task[1]
                        {
                            Task.Factory.StartNew(() => BindGrid(searchRequest,_SearchEngine,currentContext))    
                        };
                    try
                    {
                        Task.WaitAll(tasks);
                    }
                    catch (Exception ex)
                    {
                        this.WriteException(ex);
                        lblTVMsg.Visible = true;
                        lblTVMsg.Text = CommonConstants.MsgSolrSearchUA;
                    }
                }
                else
                {
                    Task[] tasks = new Task[2]
                        {
                            Task.Factory.StartNew(() => BindGrid(searchRequest,_SearchEngine,currentContext)),
                            Task.Factory.StartNew(() => BindChart(searchRequest,_SearchEngine,currentContext))
                        };
                    try
                    {
                        Task.WaitAll(tasks);
                    }
                    catch (Exception ex)
                    {
                        this.WriteException(ex);
                        lblTVMsg.Visible = true;
                        lblTVMsg.Text = CommonConstants.MsgSolrSearchUA;
                    }

                    upChart.Update();
                }
                //Block until all tasks complete.

                if (_viewstateInformation != null && (string.IsNullOrEmpty(_viewstateInformation.SortExpression) || _viewstateInformation.IsSortDirecitonAsc == null))
                {
                    _viewstateInformation.SortExpression = "datetime";
                    _viewstateInformation.IsSortDirecitonAsc = false;
                    SetViewstateInformation(_viewstateInformation);
                    SortClipDirection("datetime", false, rptTV);
                }
                else
                {
                    SortClipDirection(_viewstateInformation.SortExpression, _viewstateInformation.IsSortDirecitonAsc, rptTV);
                }

                Logger.Info("Get Search Results for TV Request Ends");

                upGrid.Update();
                // Continue on this thread...
                // commnted filter update on 14-12-2012
                //upFilter.Update();
            }
            catch (System.TimeoutException ex)
            {
                throw new MyException(CommonConstants.MsgBasicSearchUA);
            }
            catch (ThreadAbortException _exthread)
            {

            }
            catch (Exception ex)
            {
                throw new MyException(ex.ToString());
            }
        }

        public void LoadDefaultSearch()
        {
            try
            {

                IIQCustomer_SavedSearchController _IIQCustomer_SavedSearchController = _ControllerFactory.CreateObject<IIQCustomer_SavedSearchController>();
                List<SavedSearch> _ListOfSavedSearch = _IIQCustomer_SavedSearchController.GetDefaultSearchByCustomerGuid(new Guid(_SessionInformation.CustomerGUID));
                if (_ListOfSavedSearch != null && _ListOfSavedSearch.Count > 0)
                {
                    _ListOfSavedSearch[0].CustomerGUID = new Guid(_SessionInformation.CustomerGUID);
                    _viewstateInformation.LoadedSavedSearch = _ListOfSavedSearch[0];
                    SetViewstateInformation(_viewstateInformation);
                    FillFilterByXML(_ListOfSavedSearch[0].IQPremiumSearchRequestXml);
                    SaveRequestObject();
                }
                else
                {
                    FillFilterByXML(string.Empty);
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);

            }
        }

        public void FillFilterByXML(string xmlData)
        {
            try
            {
                if (!string.IsNullOrEmpty(xmlData))
                {
                    XDocument _xdoc = XDocument.Parse(xmlData);

                    #region Set SearchTerm
                    if (_xdoc.Root.Element("SearchTerm") != null && !string.IsNullOrEmpty(_xdoc.Root.Element("SearchTerm").Value))
                    {
                        txtSearch.Text = _xdoc.Root.Element("SearchTerm").Value;
                    }
                    else
                    {
                        txtSearch.Text = string.Empty;
                    }
                    #endregion

                    if (_xdoc.Root.Element("TV") != null)
                    {
                        if (hdnTVData.Value != "1")
                        {
                            BindStatSkedProgData();
                            hdnTVData.Value = "1";
                            divMainTVFilter.Visible = true;
                        }
                        #region TV Filter

                        #region ProgramTitle and Appearing
                        if (_xdoc.Descendants("TV").Elements("ProgramTitle").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("TV").Elements("ProgramTitle").FirstOrDefault().Value))
                        {
                            txProgramTitle.Text = _xdoc.Descendants("TV").Elements("ProgramTitle").FirstOrDefault().Value;
                        }
                        else
                        {
                            txProgramTitle.Text = string.Empty;
                        }
                        if (_xdoc.Descendants("TV").Elements("Appearing").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("TV").Elements("Appearing").FirstOrDefault().Value))
                        {
                            txtAppearing.Text = _xdoc.Descendants("TV").Elements("Appearing").FirstOrDefault().Value;
                        }
                        else
                        {
                            txtAppearing.Text = string.Empty;
                        }
                        #endregion

                        #region Set TimeFilter
                        DateTime FromDate = new DateTime();
                        if (_xdoc.Descendants("TV").Descendants("FromDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("TV").Descendants("FromDate").FirstOrDefault().Value, out FromDate))
                        {
                            txtStartDate.Text = FromDate.Date.ToShortDateString();
                            int Hour = FromDate.Hour < 12 ? FromDate.Hour : FromDate.Hour - 12;
                            ddlStartTime.SelectedValue = FromDate.Hour == 12 ? "0" : Hour.ToString();
                            rbDuration1.Checked = false;
                            rbDuration2.Checked = true;
                            rdAmPmFromDate.SelectedIndex = FromDate.Hour < 12 ? 0 : 1;
                        }

                        DateTime ToDate = new DateTime();
                        if (_xdoc.Descendants("TV").Descendants("ToDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("TV").Descendants("ToDate").FirstOrDefault().Value, out ToDate))
                        {
                            txtEndDate.Text = ToDate.Date.ToShortDateString();
                            int Hour = ToDate.Hour < 12 ? ToDate.Hour : ToDate.Hour - 12;
                            ddlEndTime.SelectedValue = ToDate.Hour == 12 ? "0" : Hour.ToString();
                            rbDuration1.Checked = false;
                            rbDuration2.Checked = true;
                            rdAMPMToDate.SelectedIndex = ToDate.Hour < 12 ? 0 : 1;
                        }

                        if (_xdoc.Descendants("TV").Descendants("DayDuration").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("TV").Descendants("DayDuration").FirstOrDefault().Value))
                        {
                            if (ddlDuration.Items.FindByValue(_xdoc.Descendants("TV").Descendants("DayDuration").FirstOrDefault().Value) != null)
                            {
                                ddlDuration.SelectedValue = _xdoc.Descendants("TV").Descendants("DayDuration").FirstOrDefault().Value;
                            }

                            //string _SelectedDuration = _xdoc.Descendants("TV").Descendants("DayDuration").FirstOrDefault().Value.ToLower();
                            //switch (_SelectedDuration)
                            //{
                            //    case "3":
                            //        ddlDuration.SelectedIndex = 0;
                            //        break;
                            //    case "6":
                            //        ddlDuration.SelectedIndex = 1;
                            //        break;
                            //    case "12":
                            //        ddlDuration.SelectedIndex = 2;
                            //        break;
                            //    case "all":
                            //        ddlDuration.SelectedIndex = 3;
                            //        break;
                            //    default:
                            //        ddlDuration.SelectedIndex = 0;
                            //        break;
                            //}
                            rbDuration1.Checked = true;
                            rbDuration2.Checked = false;
                        }

                        if (_xdoc.Descendants("TV").Elements("TimeZone").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("TV").Elements("TimeZone").FirstOrDefault().Value))
                        {
                            switch (_xdoc.Descendants("TV").Elements("TimeZone").FirstOrDefault().Value.ToUpper())
                            {
                                case "ALL":
                                    ddlTimeZone.SelectedIndex = 0;
                                    break;
                                case "EST":
                                    ddlTimeZone.SelectedIndex = 1;
                                    break;
                                case "CST":
                                    ddlTimeZone.SelectedIndex = 2;
                                    break;
                                case "MST":
                                    ddlTimeZone.SelectedIndex = 3;
                                    break;
                                case "PST":
                                    ddlTimeZone.SelectedIndex = 4;
                                    break;
                                default:
                                    ddlTimeZone.SelectedIndex = 0;
                                    break;

                            }
                        }
                        #endregion


                        #region Set Station Affiliate Filter
                        List<int> _ListOfTotalStationRepeaterItemSelected = Enumerable.Repeat(0, rptTVStationSubMaster.Items.Count).ToList();
                        int index = 0;
                        Boolean isAllTVStationSelected = true;
                        if (_xdoc.Descendants("Station_Affiliate_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("Station_Affiliate_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                        {
                            foreach (RepeaterItem rptitm in rptTVStationSubMaster.Items)
                            {
                                Repeater rptTVStationChild = (Repeater)rptitm.FindControl("rptTVStationChild");
                                ((CheckBox)rptitm.FindControl("chkStationSubMaster")).Checked = false;
                                foreach (RepeaterItem repeaterItem in rptTVStationChild.Items)
                                {
                                    HtmlInputCheckBox chkTVStation = (HtmlInputCheckBox)repeaterItem.FindControl("chkTVStation");
                                    chkTVStation.Checked = false;
                                }
                            }


                            foreach (XElement Xelem in _xdoc.Descendants("Station_Affiliate_Set").Elements("IQ_Station_ID"))
                            {
                                index = 0;
                                foreach (RepeaterItem rptitm in rptTVStationSubMaster.Items)
                                {
                                    bool IsChecked = false;

                                    Repeater rptTVStationChild = (Repeater)rptitm.FindControl("rptTVStationChild");
                                    foreach (RepeaterItem repeaterItem in rptTVStationChild.Items)
                                    {
                                        HtmlInputCheckBox chkTVStation = (HtmlInputCheckBox)repeaterItem.FindControl("chkTVStation");
                                        if (chkTVStation.Value.Trim().ToLower() == Xelem.Value.Trim().ToLower())
                                        {
                                            chkTVStation.Checked = true;
                                            _ListOfTotalStationRepeaterItemSelected[index]++;
                                            IsChecked = true;
                                            break;
                                        }
                                    }

                                    index++;

                                    if (IsChecked)
                                        break;
                                }
                            }

                            index = 0;
                            foreach (RepeaterItem rptitm in rptTVStationSubMaster.Items)
                            {
                                Repeater rptTVStationChild = (Repeater)rptitm.FindControl("rptTVStationChild");

                                CheckBox chkSubStationCheck = (CheckBox)rptitm.FindControl("chkStationSubMaster");
                                if (rptTVStationChild.Items.Count > 0 && rptTVStationChild.Items.Count == _ListOfTotalStationRepeaterItemSelected[index])
                                { chkSubStationCheck.Checked = true; }
                                else
                                { isAllTVStationSelected = false; }
                                index++;
                            }

                            if (isAllTVStationSelected)
                            { chkAffilAll.Checked = true; }
                            else { chkAffilAll.Checked = false; }
                        }
                        else
                        {
                            chkAffilAll.Checked = true;
                            foreach (RepeaterItem rptitm in rptTVStationSubMaster.Items)
                            {

                                ((CheckBox)rptitm.FindControl("chkStationSubMaster")).Checked = true;

                                Repeater rptTVStationChild = (Repeater)rptitm.FindControl("rptTVStationChild");
                                foreach (RepeaterItem repeaterItem in rptTVStationChild.Items)
                                {
                                    HtmlInputCheckBox chkTVStation = (HtmlInputCheckBox)repeaterItem.FindControl("chkTVStation");
                                    chkTVStation.Checked = true;
                                }
                            }
                        }
                        #endregion

                        #region Set IQ Class Filter
                        Boolean isAllTVCategoryChecked = true;
                        if (_xdoc.Descendants("IQ_Class_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("IQ_Class_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                        {
                            foreach (RepeaterItem rptItem in rptCategory.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkClass");
                                chk.Checked = false;
                            }

                            foreach (RepeaterItem rptItem in rptCategory.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkClass");
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Class").Elements("num"))
                                {
                                    if (chk.Value == Xelem.Value)
                                    {
                                        chk.Checked = true;
                                        break;
                                    }
                                }

                                if (!chk.Checked)
                                {
                                    isAllTVCategoryChecked = false;
                                }
                            }
                            if (isAllTVCategoryChecked)
                            { chkCategoryAll.Checked = true; }
                            else { chkCategoryAll.Checked = false; }

                        }
                        else
                        {
                            foreach (RepeaterItem rptItem in rptCategory.Items)
                            {
                                HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkClass");
                                chk.Checked = true;
                            }
                            chkCategoryAll.Checked = true;
                        }
                        #endregion

                        #region Set Market Filter

                        if (_xdoc.Descendants("IQ_Dma_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("IQ_Dma_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                        {
                            ddlMarket.SelectedValue = _xdoc.Descendants("IQ_Dma_Set").ElementAt(0).Attribute("SelectionMethod").Value;
                            chlkRegionSelectAll.Checked = false;
                            chkRankFIlterSelectAll.Checked = false;
                            //if (ddlMarket.SelectedIndex == 0)
                            //{


                            #region Region Filter
                            Boolean isAllMarketRegionSelected = true;
                            List<int> _ListOfTotalMarketRepeaterItemSelected = Enumerable.Repeat(0, rptregion.Items.Count).ToList();

                            for (int i = 0; i < rptregion.Items.Count; i++)
                            {
                                Repeater rptDmaList = (Repeater)rptregion.Items[i].FindControl("rptDma");
                                ((CheckBox)rptregion.Items[i].FindControl("chkRegion")).Checked = false;
                                foreach (RepeaterItem repeaterItem in rptDmaList.Items)
                                {
                                    HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                    chkboxDma.Checked = false;
                                }
                            }

                            index = 0;
                            foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                            {
                                index = 0;
                                foreach (RepeaterItem rptitm in rptregion.Items)
                                {
                                    bool isChecked = false;
                                    Repeater rptDmaList = (Repeater)rptitm.FindControl("rptDma");

                                    foreach (RepeaterItem repeaterItem in rptDmaList.Items)
                                    {
                                        HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                        string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');

                                        if (SelctedDMANameAndNum[0] == Xelem.Value)
                                        {
                                            chkboxDma.Checked = true;
                                            isChecked = true;
                                            _ListOfTotalMarketRepeaterItemSelected[index]++;
                                            break;
                                        }
                                    }
                                    index++;

                                    if (isChecked)
                                        break;
                                }
                            }


                            index = 0;
                            foreach (RepeaterItem rptitm in rptregion.Items)
                            {
                                Repeater rptDma = (Repeater)rptitm.FindControl("rptDma");
                                CheckBox chkSubMarketCheck = (CheckBox)rptitm.FindControl("chkRegion");
                                if (rptDma.Items.Count == _ListOfTotalMarketRepeaterItemSelected[index])
                                { chkSubMarketCheck.Checked = true; }
                                else { isAllMarketRegionSelected = false; }
                                index++;
                            }

                            if (_xdoc.Descendants("IQ_Dma").Elements("name").FirstOrDefault(e => e.Value == "National") != null)
                            {
                                chkRegionNational.Checked = true;
                            }
                            else
                            {
                                chkRegionNational.Checked = false;
                            }

                            if (isAllMarketRegionSelected)
                            {
                                if (chkRegionNational.Visible)
                                {
                                    if (chkRegionNational.Checked)
                                    {
                                        chlkRegionSelectAll.Checked = true;
                                    }
                                    else { chlkRegionSelectAll.Checked = false; }
                                }
                                else
                                {
                                    chlkRegionSelectAll.Checked = true;
                                }
                            }
                            else
                            { chlkRegionSelectAll.Checked = false; }
                            #endregion
                            //}
                            //else
                            //{
                            #region Rank Filter

                            foreach (RepeaterItem repeaterItem in rptTop10.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop20.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop30.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop40.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop50.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop60.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop80.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop100.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop150.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }
                            foreach (RepeaterItem repeaterItem in rptTop210.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = false;
                            }

                            Boolean isAllRankFilterChecked = true;
                            //foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                            //{
                            //bool IsChecked = false;
                            bool IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop10.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }

                            }

                            if (IsAllSelected)
                                chkTop10.Checked = true;

                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop20.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }
                            }

                            if (IsAllSelected)
                                chkTop20.Checked = true;


                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop30.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }
                            }

                            if (IsAllSelected)
                                chkTop30.Checked = true;


                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop40.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }
                            }

                            if (IsAllSelected)
                                chkTop40.Checked = true;


                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop50.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }
                            }

                            if (IsAllSelected)
                                chkTop50.Checked = true;


                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop60.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }
                            }

                            if (IsAllSelected)
                                chkTop60.Checked = true;


                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop80.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }
                            }

                            if (IsAllSelected)
                                chkTop80.Checked = true;

                            //if (IsChecked)
                            //    continue;

                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop100.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }

                            }

                            if (IsAllSelected)
                                chkTop100.Checked = true;

                            //if (IsChecked)
                            //    continue;

                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop150.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }
                            }

                            if (IsAllSelected)
                                chkTop150.Checked = true;

                            //if (IsChecked)
                            //    continue;

                            IsAllSelected = true;
                            foreach (RepeaterItem repeaterItem in rptTop210.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');
                                foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                                {
                                    if (SelctedDMANameAndNum[0] == Xelem.Value)
                                    {
                                        chkboxDma.Checked = true;
                                        break;
                                    }
                                }

                                if (!chkboxDma.Checked)
                                {
                                    IsAllSelected = false;
                                    isAllRankFilterChecked = false;
                                }
                            }

                            if (IsAllSelected)
                                chkTop210.Checked = true;
                            //}
                            if (_xdoc.Descendants("IQ_Dma").Elements("name").FirstOrDefault(e => e.Value == "National") != null)
                            {
                                chkNational.Checked = true;
                            }
                            else
                            {
                                chkNational.Checked = false;
                            }

                            if (isAllRankFilterChecked && (!chkNational.Visible || chkNational.Checked))
                            { chkRankFIlterSelectAll.Checked = true; }
                            else
                            { chkRankFIlterSelectAll.Checked = false; }
                            #endregion
                            //}
                        }
                        else
                        {
                            #region Region Vise

                            chlkRegionSelectAll.Checked = true;
                            for (int i = 0; i < rptregion.Items.Count; i++)
                            {
                                Repeater rptDmaList = (Repeater)rptregion.Items[i].FindControl("rptDma");
                                //if (rptDmaList.Items.Count > 0)
                                //{
                                ((CheckBox)rptregion.Items[i].FindControl("chkRegion")).Checked = true;
                                foreach (RepeaterItem repeaterItem in rptDmaList.Items)
                                {
                                    HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                    //chkboxDma.Attributes.Add("onclick", "chkMainRegion(this," + ((CheckBoxList)rptregion.Items[i].FindControl("cblregion")).ClientID + "," + ((CheckBox)rptregion.Items[i].FindControl("chkRegion")).ClientID + "," + chlkRegionSelectAll.ClientID + ",divRegionFilter);");
                                    //chkboxDma.Attributes.Add("onclick", "chkMainRegion(this," + rptregion.Items[i].FindControl("divCheckBox").ClientID + "," + ((CheckBox)rptregion.Items[i].FindControl("chkRegion")).ClientID + "," + chlkRegionSelectAll.ClientID + ");");
                                    chkboxDma.Checked = true;
                                }
                                //}
                            }

                            if (chkRegionNational.Visible)
                            {
                                chkRegionNational.Checked = true;
                            }
                            else
                            {
                                chkRegionNational.Checked = false;
                            }

                            #endregion

                            #region Rank Vise
                            foreach (RepeaterItem repeaterItem in rptTop10.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop10.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop20.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop20.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop30.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop30.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop40.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop40.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop50.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop50.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop60.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop60.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop80.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop80.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop100.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop100.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop150.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop150.Checked = true;

                            foreach (RepeaterItem repeaterItem in rptTop210.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                chkboxDma.Checked = true;
                            }
                            chkTop210.Checked = true;


                            if (chkNational.Visible)
                            {
                                chkNational.Checked = true;
                            }
                            else
                            {
                                chkNational.Checked = false;
                            }

                            chkRankFIlterSelectAll.Checked = true;


                            #endregion
                        }
                        #endregion

                        #endregion

                        chkTV.Checked = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowTvFilter", "$('#" + divMainTVFilter.ClientID + "').show();", true);
                        upTVFilter.Update();
                    }
                    else
                    {
                        chkTV.Checked = false;

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTvFilter", "$('#" + divMainTVFilter.ClientID + "').hide();", true);
                        if (divMainTVFilter.Visible)
                        {
                            #region TV Filter

                            #region Time FIlter

                            string _Script = "$('#" + rbDuration1.ClientID + "').attr('checked', true);" +
                                "$('#" + rbDuration2.ClientID + "').attr('checked', false);" +
                                "$('#" + ddlDuration.ClientID + " option').eq(0).attr('selected', 'selected');";

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTVTimer", _Script, true);

                            #endregion

                            #region Program Filter

                            _Script = "$('#" + txProgramTitle.ClientID + "').val('');" +
                            "$('#" + txtAppearing.ClientID + "').val('');";

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTVProgramFilter", _Script, true);

                            #endregion

                            #region Set All Dma

                            #region Region Vise

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetRegion", "$('#" + ddlMarket.ClientID + " option').eq(0).attr('selected', 'selected');", true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetmarketRegion", "CheckAllCheckBox(divMainRegionFilter);", true);

                            #endregion

                            #region Rank Vise

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetmarketrank", "CheckAllCheckBox(divMarketRankFilter);", true);

                            #endregion

                            #endregion

                            #region Set All Affils

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetTVStation", "CheckAllCheckBox(divStationFilter);", true);

                            #endregion

                            #region Set All Category

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetTVCategory", "CheckAllCheckBox(divCategoryFilter);", true);

                            #endregion


                            #region Set all to OFF
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTitle", "SetFilterStatus('divProgramFilterStatus','imgProgramFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTime", "SetFilterStatus('divTimeFilterStatus','imgTimeFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('divStationFilterStatus','imgStatusFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCat", "SetFilterStatus('divCategoryFilterStatus','imgCategoryFilter','OFF');", true);

                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTV", "SetFilterStatus('divTVFilterStatus',null,'');", true);

                            #endregion


                            #endregion
                        }
                    }

                    if (_SessionInformation.IsiQPremiumRadio)
                    {
                        if (_xdoc.Root.Element("Radio") != null)
                        {
                            if (hdnRadioData.Value != "1")
                            {
                                GetRadioStations();
                                hdnRadioData.Value = "1";
                                divMainRadioFilter.Visible = true;
                            }

                            chkRadio.Checked = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowRadioFilter", "$('#" + divMainRadioFilter.ClientID + "').show();", true);

                            #region Set Radio Time Filter

                            DateTime _RadioFromDate = new DateTime();
                            if (_xdoc.Descendants("Radio").Descendants("FromDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("Radio").Descendants("FromDate").FirstOrDefault().Value, out _RadioFromDate))
                            {
                                txtRadioStartDate.Text = _RadioFromDate.Date.ToShortDateString();
                                int Hour = _RadioFromDate.Hour < 12 ? _RadioFromDate.Hour : _RadioFromDate.Hour - 12;
                                ddlRadioStartHour.SelectedValue = _RadioFromDate.Hour == 12 ? "0" : Hour.ToString();
                                rbRadioInterval.Checked = true;
                                rbRadioDuration.Checked = false;
                                rbRadioStart.SelectedIndex = _RadioFromDate.Hour < 12 ? 0 : 1;
                            }

                            DateTime _RadioToDate = new DateTime();
                            if (_xdoc.Descendants("Radio").Descendants("ToDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("Radio").Descendants("ToDate").FirstOrDefault().Value, out _RadioToDate))
                            {
                                txtRadioEndDate.Text = _RadioToDate.Date.ToShortDateString();
                                int Hour = _RadioToDate.Hour < 12 ? _RadioToDate.Hour : _RadioToDate.Hour - 12;
                                ddlRadioEndHour.SelectedValue = _RadioToDate.Hour == 12 ? "0" : Hour.ToString();
                                rbRadioInterval.Checked = true;
                                rbRadioDuration.Checked = false;
                                rbRadioEnd.SelectedIndex = _RadioToDate.Hour < 12 ? 0 : 1;
                            }

                            if (_xdoc.Descendants("Radio").Descendants("DayDuration").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("Radio").Descendants("DayDuration").FirstOrDefault().Value))
                            {
                                if (ddlRadioDuration.Items.FindByValue(_xdoc.Descendants("Radio").Descendants("DayDuration").FirstOrDefault().Value) != null)
                                {
                                    ddlRadioDuration.SelectedValue = _xdoc.Descendants("Radio").Descendants("DayDuration").FirstOrDefault().Value;
                                }
                                rbRadioDuration.Checked = true;
                                rbRadioInterval.Checked = false;
                            }
                            #endregion

                            #region Set Radio Market Filter
                            Boolean isRadioMarketAllChecked = true;
                            if (_xdoc.Root.Element("Radio").Descendants("IQ_Dma_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Root.Element("Radio").Descendants("IQ_Dma_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                            {
                                chkRadioMarketAll.Checked = false;

                                foreach (RepeaterItem rptitem in rptRadioMarket.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkDma");
                                    chk.Checked = false;
                                }



                                foreach (RepeaterItem repeaterItem in rptRadioMarket.Items)
                                {
                                    HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                    foreach (XElement Xelem in _xdoc.Root.Element("Radio").Descendants("IQ_Dma").Elements("name"))
                                    {

                                        string[] SelctedDMANameAndNum = chkboxDma.Value.Split('#');

                                        if (SelctedDMANameAndNum[0] == Xelem.Value)
                                        {
                                            chkboxDma.Checked = true;
                                            break;
                                        }


                                    }
                                    if (!chkboxDma.Checked)
                                    { isRadioMarketAllChecked = false; }


                                }


                                if (isRadioMarketAllChecked)
                                { chkRadioMarketAll.Checked = true; }
                                else
                                { chkRadioMarketAll.Checked = false; }
                            }
                            else
                            {
                                chkRadioMarketAll.Checked = true;
                                foreach (RepeaterItem rptitem in rptRadioMarket.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkDma");
                                    chk.Checked = true;
                                }
                            }


                            #endregion
                            upRadioFilter.Update();
                        }
                        else
                        {
                            chkRadio.Checked = false;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideRadioFilter", "$('#" + divMainRadioFilter.ClientID + "').hide();", true);

                            if (divMainRadioFilter.Visible)
                            {
                                #region RadioFilter

                                #region Time Filter

                                string _Script = "$('#" + rbRadioDuration.ClientID + "').attr('checked', true);" +
                                   "$('#" + rbRadioInterval.ClientID + "').attr('checked', false);" +
                                   "$('#" + ddlRadioDuration.ClientID + " option').eq(0).attr('selected', 'selected');";

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetRadioTimer", _Script, true);

                                #endregion





                                #region Radio Market
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceCategory", "CheckAllCheckBox(divRadioMarket);", true);
                                #endregion

                                #region Set all to OFF

                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioTime", "SetFilterStatus('divRadioTimeFilterStatus','imgShowRadioTimeFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadioMarket", "SetFilterStatus('divRadioMarketFilterStatus','imgShowRadioMarketFilter','OFF');", true);


                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterRadio", "SetFilterStatus('divRadioFilterStatus',null,'');", true);

                                #endregion

                                #endregion
                            }
                        }

                    }


                    if (_SessionInformation.IsiQPremiumNM)
                    {
                        if (_xdoc.Root.Element("News") != null)
                        {
                            if (hdnONLINENEWSData.Value != "1")
                            {
                                BindOnlineNewsCheckBoxes();
                                hdnONLINENEWSData.Value = "1";
                                divMainOnlineNewsFilter.Visible = true;
                            }

                            chkNews.Checked = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNewsFilter", "$('#" + divMainOnlineNewsFilter.ClientID + "').show();", true);
                            #region News Filters

                            #region Set News Publication
                            if (_xdoc.Descendants("News").Elements("Publication").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("News").Elements("Publication").FirstOrDefault().Value))
                            {
                                txtNewsPublication.Text = _xdoc.Descendants("News").Elements("Publication").FirstOrDefault().Value;
                            }
                            else
                            {
                                txtNewsPublication.Text = string.Empty;
                            }
                            #endregion

                            #region Set News Time Filter

                            DateTime _NewsFromDate = new DateTime();
                            if (_xdoc.Descendants("News").Descendants("FromDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("News").Descendants("FromDate").FirstOrDefault().Value, out _NewsFromDate))
                            {
                                txtNewsStartDate.Text = _NewsFromDate.Date.ToShortDateString();
                                int Hour = _NewsFromDate.Hour < 12 ? _NewsFromDate.Hour : _NewsFromDate.Hour - 12;
                                ddlNewsStartHour.SelectedValue = _NewsFromDate.Hour == 12 ? "0" : Hour.ToString();
                                rbNewsInterval.Checked = true;
                                rbNewsDuration.Checked = false;
                                rbNewsStart.SelectedIndex = _NewsFromDate.Hour < 12 ? 0 : 1;
                            }

                            DateTime _NewsToDate = new DateTime();
                            if (_xdoc.Descendants("News").Descendants("ToDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("News").Descendants("ToDate").FirstOrDefault().Value, out _NewsToDate))
                            {
                                txtNewsEndDate.Text = _NewsToDate.Date.ToShortDateString();
                                int Hour = _NewsToDate.Hour < 12 ? _NewsToDate.Hour : _NewsToDate.Hour - 12;
                                ddlNewsEndHour.SelectedValue = _NewsToDate.Hour == 12 ? "0" : Hour.ToString();
                                rbNewsInterval.Checked = true;
                                rbNewsDuration.Checked = false;
                                rbNewsEnd.SelectedIndex = _NewsToDate.Hour < 12 ? 0 : 1;
                            }

                            if (_xdoc.Descendants("News").Descendants("DayDuration").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("News").Descendants("DayDuration").FirstOrDefault().Value))
                            {
                                if (ddlNewsDuration.Items.FindByValue(_xdoc.Descendants("News").Descendants("DayDuration").FirstOrDefault().Value) != null)
                                {
                                    ddlNewsDuration.SelectedValue = _xdoc.Descendants("News").Descendants("DayDuration").FirstOrDefault().Value;
                                }
                                rbNewsDuration.Checked = true;
                                rbNewsInterval.Checked = false;
                            }
                            #endregion

                            #region Set News Categoy Filter

                            Boolean isNewsCategoryAllChecked = true;
                            if (_xdoc.Descendants("NewsCategory_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("NewsCategory_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                            {
                                chkNewsCategorySelectAll.Checked = false;

                                foreach (RepeaterItem rptitem in rptNewsCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsCategory");
                                    chk.Checked = false;
                                }

                                foreach (RepeaterItem rptitem in rptNewsCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsCategory");
                                    foreach (XElement Xelem in _xdoc.Descendants("NewsCategory_Set").Elements("NewsCategory"))
                                    {
                                        if (chk.Value.ToLower().Trim() == Xelem.Value.ToLower().Trim())
                                        {
                                            chk.Checked = true;
                                            break;
                                        }

                                    }
                                    if (!chk.Checked)
                                    { isNewsCategoryAllChecked = false; }
                                }
                                if (isNewsCategoryAllChecked)
                                { chkNewsCategorySelectAll.Checked = true; }
                                else { chkNewsCategorySelectAll.Checked = false; }


                            }
                            else
                            {
                                chkNewsCategorySelectAll.Checked = true;
                                foreach (RepeaterItem rptitem in rptNewsCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsCategory");
                                    chk.Checked = true;
                                }
                            }

                            #endregion

                            #region Set News Publication Category Filter
                            Boolean isNewsPublicatoinCategoryAllChecked = true;
                            if (_xdoc.Descendants("PublicationCategory_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("PublicationCategory_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                            {
                                chkNewsPublicationCategory.Checked = false;

                                foreach (RepeaterItem rptitem in rptNewsPublicationCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsPublicationCategory");
                                    chk.Checked = false;
                                }

                                foreach (RepeaterItem rptitem in rptNewsPublicationCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsPublicationCategory");
                                    foreach (XElement Xelem in _xdoc.Descendants("PublicationCategory_Set").Elements("PublicationCategory"))
                                    {
                                        if (chk.Value.ToLower().Trim() == Xelem.Value.ToLower().Trim())
                                        {
                                            chk.Checked = true;
                                            break;
                                        }
                                    }
                                    if (!chk.Checked)
                                    { isNewsPublicatoinCategoryAllChecked = false; }
                                }
                                if (isNewsPublicatoinCategoryAllChecked)
                                { chkNewsPublicationCategory.Checked = true; }
                                else
                                { chkNewsPublicationCategory.Checked = false; }
                            }
                            else
                            {
                                chkNewsPublicationCategory.Checked = true;
                                foreach (RepeaterItem rptitem in rptNewsPublicationCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsPublicationCategory");
                                    chk.Checked = true;
                                }
                            }

                            #endregion

                            #region Set News Genre Filter
                            Boolean isAllNewsGenreAllChecked = true;
                            if (_xdoc.Descendants("Genre_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("Genre_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                            {
                                chkNewsGenreSelectAll.Checked = false;

                                foreach (RepeaterItem rptitem in rptNewsGenre.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsGenre");
                                    chk.Checked = false;
                                }

                                foreach (RepeaterItem rptitem in rptNewsGenre.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsGenre");
                                    foreach (XElement Xelem in _xdoc.Descendants("Genre_Set").Elements("Genre"))
                                    {
                                        if (chk.Value.ToLower().Trim() == Xelem.Value.ToLower().Trim())
                                        {
                                            chk.Checked = true;
                                            break;
                                        }

                                    }
                                    if (!chk.Checked)
                                    { isAllNewsGenreAllChecked = false; }
                                }

                                if (isAllNewsGenreAllChecked)
                                { chkNewsGenreSelectAll.Checked = true; }
                                else
                                { chkNewsGenreSelectAll.Checked = false; }
                            }
                            else
                            {
                                chkNewsGenreSelectAll.Checked = true;
                                foreach (RepeaterItem rptitem in rptNewsGenre.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsGenre");
                                    chk.Checked = true;
                                }
                            }

                            #endregion

                            #region Set News Region Filter
                            Boolean isNewsRegionAllChecked = true;
                            if (_xdoc.Descendants("Region_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("Region_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                            {
                                chkNewsRegionAll.Checked = false;

                                foreach (RepeaterItem rptitem in rptNewsRegion.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsregion");
                                    chk.Checked = false;
                                }

                                foreach (RepeaterItem rptitem in rptNewsRegion.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsregion");
                                    foreach (XElement Xelem in _xdoc.Descendants("Region_Set").Elements("Region"))
                                    {
                                        if (chk.Value.ToLower().Trim() == Xelem.Value.ToLower().Trim())
                                        {
                                            chk.Checked = true;
                                            break;
                                        }
                                    }

                                    if (!chk.Checked)
                                    { isNewsRegionAllChecked = false; }
                                }
                                if (isNewsRegionAllChecked)
                                { chkNewsRegionAll.Checked = true; }
                                else
                                { chkNewsRegionAll.Checked = false; }
                            }
                            else
                            {
                                chkNewsRegionAll.Checked = true;
                                foreach (RepeaterItem rptitem in rptNewsRegion.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsregion");
                                    chk.Checked = true;
                                }
                            }

                            #endregion

                            #endregion
                            upNewsFilter.Update();
                        }
                        else
                        {
                            chkNews.Checked = false;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNewsFilter", "$('#" + divMainOnlineNewsFilter.ClientID + "').hide();", true);
                            if (divMainOnlineNewsFilter.Visible)
                            {
                                #region Online News Filter

                                #region Time Filter

                                string _Script = "$('#" + rbNewsDuration.ClientID + "').attr('checked', true);" +
                                   "$('#" + rbNewsInterval.ClientID + "').attr('checked', false);" +
                                   "$('#" + ddlNewsDuration.ClientID + " option').eq(0).attr('selected', 'selected');";

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetNewsTimer", _Script, true);

                                #endregion

                                #region Source

                                _Script = "$('#" + txtNewsPublication.ClientID + "').val('');";

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetNewsSource", _Script, true);

                                #endregion

                                #region Category
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONCategory", "CheckAllCheckBox(divNewsCategory);", true);

                                #endregion

                                #region Publication Category

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONPubCategory", "CheckAllCheckBox(divShowNewsPublicationCategory);", true);

                                #endregion


                                #region Genre

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONGenre", "CheckAllCheckBox(divNewsGenreFilter);", true);

                                #endregion

                                #region News Region


                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONNewsRegion", "CheckAllCheckBox(divNewsRegionFilter);", true);

                                #endregion

                                #region Set All to OFF
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsTime", "SetFilterStatus('divNewsTimeFilterStatus','imgNewsTimeFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPub", "SetFilterStatus('divNewsPublicationStatus','imgShowNewsPublicationFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsCat", "SetFilterStatus('divNewsCategoryStatus','imgShowNewsCategoryFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsGenre", "SetFilterStatus('divNewsGenreFilterStatus','imgNewsGenreFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsRegion", "SetFilterStatus('divNewsRegionFilterStatus','imgNewsRegionStatusFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNewsPCat", "SetFilterStatus('divShowNewsPublicationCategoryStatus','imgShowNewsPublicationCategoryFilter','OFF');", true);

                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterNews", "SetFilterStatus('divOnlineNewsFilterStatus',null,'');", true);

                                #endregion

                                #endregion
                            }
                        }
                    }

                    if (_SessionInformation.IsiQPremiumSM)
                    {
                        if (_xdoc.Root.Element("SocialMedia") != null)
                        {
                            if (hdnSocialMediaData.Value != "1")
                            {
                                hdnSocialMediaData.Value = "1";
                                BindSocialMediaCheckBoxes();
                                divMainSMFilter.Visible = true;
                            }

                            #region Social Media Filter

                            #region Set Social Media Time Filter

                            DateTime _SMFromDate = new DateTime();
                            if (_xdoc.Descendants("SocialMedia").Descendants("FromDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("SocialMedia").Descendants("FromDate").FirstOrDefault().Value, out _SMFromDate))
                            {
                                txtSMStartDate.Text = _SMFromDate.Date.ToShortDateString();
                                int Hour = _SMFromDate.Hour < 12 ? _SMFromDate.Hour : _SMFromDate.Hour - 12;
                                ddlSMStartHour.SelectedValue = _SMFromDate.Hour == 12 ? "0" : Hour.ToString();
                                rbSMInterval.Checked = true;
                                rbSMDuration.Checked = false;
                                rbSMStart.SelectedIndex = _SMFromDate.Hour < 12 ? 0 : 1;
                            }

                            DateTime _SMToDate = new DateTime();
                            if (_xdoc.Descendants("SocialMedia").Descendants("ToDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("SocialMedia").Descendants("ToDate").FirstOrDefault().Value, out _SMToDate))
                            {
                                txtSMEndDate.Text = _SMToDate.Date.ToShortDateString();
                                int Hour = _SMToDate.Hour < 12 ? _SMToDate.Hour : _SMToDate.Hour - 12;
                                ddlSMEndHour.SelectedValue = _SMToDate.Hour == 12 ? "0" : Hour.ToString();
                                rbSMInterval.Checked = true;
                                rbSMDuration.Checked = false;
                                rbSMEnd.SelectedIndex = _SMToDate.Hour < 12 ? 0 : 1;
                            }

                            if (_xdoc.Descendants("SocialMedia").Descendants("DayDuration").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("SocialMedia").Descendants("DayDuration").FirstOrDefault().Value))
                            {
                                if (ddlSMDuration.Items.FindByValue(_xdoc.Descendants("SocialMedia").Descendants("DayDuration").FirstOrDefault().Value) != null)
                                {
                                    ddlSMDuration.SelectedValue = _xdoc.Descendants("SocialMedia").Descendants("DayDuration").FirstOrDefault().Value;
                                }
                                rbSMDuration.Checked = true;
                                rbSMInterval.Checked = false;
                            }
                            #endregion

                            #region Set Social Media Source

                            if (_xdoc.Descendants("SocialMedia").Elements("Source").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("SocialMedia").Elements("Source").FirstOrDefault().Value))
                            {
                                txtSMSource.Text = _xdoc.Descendants("SocialMedia").Elements("Source").FirstOrDefault().Value;
                            }
                            else
                            {
                                txtSMSource.Text = string.Empty;
                            }

                            if (_xdoc.Descendants("SocialMedia").Elements("Title").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("SocialMedia").Elements("Title").FirstOrDefault().Value))
                            {
                                txtSMTitle.Text = _xdoc.Descendants("SocialMedia").Elements("Title").FirstOrDefault().Value;
                            }
                            else
                            {
                                txtSMTitle.Text = string.Empty;
                            }

                            if (_xdoc.Descendants("SocialMedia").Elements("Author").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("SocialMedia").Elements("Author").FirstOrDefault().Value))
                            {
                                txtSMAuthor.Text = _xdoc.Descendants("SocialMedia").Elements("Author").FirstOrDefault().Value;
                            }
                            else
                            {
                                txtSMAuthor.Text = string.Empty;
                            }

                            #endregion

                            #region Set Social Media Source Rank Filter

                            Boolean isSMRankAllChecked = true;
                            if (_xdoc.Descendants("SourceRank_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("SourceRank_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                            {
                                chkSMRankSelectAll.Checked = false;

                                foreach (RepeaterItem rptitem in rptSMRank.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMRank");
                                    chk.Checked = false;
                                }

                                foreach (RepeaterItem rptitem in rptSMRank.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMRank");
                                    foreach (XElement Xelem in _xdoc.Descendants("SourceRank_Set").Elements("SourceRank"))
                                    {
                                        if (chk.Value.ToLower().Trim() == Xelem.Value.ToLower().Trim())
                                        {
                                            chk.Checked = true;
                                            break;
                                        }
                                    }

                                    if (!chk.Checked)
                                    { isSMRankAllChecked = false; }
                                }
                                if (isSMRankAllChecked)
                                { chkSMRankSelectAll.Checked = true; }
                                else
                                { chkSMRankSelectAll.Checked = false; }
                            }
                            else
                            {
                                chkSMRankSelectAll.Checked = true;
                                foreach (RepeaterItem rptitem in rptSMRank.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMRank");
                                    chk.Checked = true;
                                }
                            }

                            #endregion

                            #region Set Social Media Source Category Filter

                            Boolean isSMCategoryAllChecked = true;
                            if (_xdoc.Descendants("SourceCategory_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("SourceCategory_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                            {
                                chkSMCategorySelectAll.Checked = false;

                                foreach (RepeaterItem rptitem in rptSMCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMCategory");
                                    chk.Checked = false;
                                }

                                foreach (RepeaterItem rptitem in rptSMCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMCategory");
                                    foreach (XElement Xelem in _xdoc.Descendants("SourceCategory_Set").Elements("SourceCategory"))
                                    {
                                        if (chk.Value.ToLower().Trim() == Xelem.Value.ToLower().Trim())
                                        {
                                            chk.Checked = true;
                                            break;
                                        }
                                    }

                                    if (!chk.Checked)
                                    { isSMCategoryAllChecked = false; }
                                }
                                if (isSMCategoryAllChecked)
                                { chkSMCategorySelectAll.Checked = true; }
                                else
                                { chkSMCategorySelectAll.Checked = false; }
                            }
                            else
                            {
                                chkSMCategorySelectAll.Checked = true;
                                foreach (RepeaterItem rptitem in rptSMCategory.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMCategory");
                                    chk.Checked = true;
                                }
                            }

                            #endregion

                            #region Set Social Media Source Type Filter

                            Boolean isSMTypeAllChecked = true;
                            if (_xdoc.Descendants("SourceType_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("SourceType_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                            {
                                chkSMTypeSelectAll.Checked = false;

                                foreach (RepeaterItem rptitem in rptSMType.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMType");
                                    chk.Checked = false;
                                }

                                foreach (RepeaterItem rptitem in rptSMType.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMType");
                                    foreach (XElement Xelem in _xdoc.Descendants("SourceType_Set").Elements("SourceType"))
                                    {
                                        if (chk.Value.ToLower().Trim() == Xelem.Value.ToLower().Trim())
                                        {
                                            chk.Checked = true;
                                            break;
                                        }
                                    }

                                    if (!chk.Checked)
                                    { isSMTypeAllChecked = false; }
                                }
                                if (isSMTypeAllChecked)
                                { chkSMTypeSelectAll.Checked = true; }
                                else
                                { chkSMTypeSelectAll.Checked = false; }
                            }
                            else
                            {
                                chkSMTypeSelectAll.Checked = true;
                                foreach (RepeaterItem rptitem in rptSMType.Items)
                                {
                                    HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMType");
                                    chk.Checked = true;
                                }
                            }

                            #endregion


                            #endregion

                            chkSocialMedia.Checked = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSocialMediaFilter", "$('#" + divMainSMFilter.ClientID + "').show();", true);
                            upSMFilter.Update();
                        }
                        else
                        {
                            chkSocialMedia.Checked = false;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSocialMediaFilter", "$('#" + divMainSMFilter.ClientID + "').hide();", true);

                            if (divMainSMFilter.Visible)
                            {
                                #region Social Media Filter

                                #region Time Filter

                                string _Script = "$('#" + rbSMDuration.ClientID + "').attr('checked', true);" +
                                   "$('#" + rbSMInterval.ClientID + "').attr('checked', false);" +
                                   "$('#" + ddlSMDuration.ClientID + " option').eq(0).attr('selected', 'selected');";

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetSMTimer", _Script, true);

                                #endregion

                                #region Source

                                _Script = "$('#" + txtSMAuthor.ClientID + "').val('');" +
                                    "$('#" + txtSMSource.ClientID + "').val('');" +
                                    "$('#" + txtSMTitle.ClientID + "').val('');";

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetSMSource", _Script, true);

                                #endregion

                                #region Rank

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceRank", "CheckAllCheckBox(divSMRank);", true);

                                #endregion

                                #region Source Category
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceCategory", "CheckAllCheckBox(divSMCategory);", true);
                                #endregion

                                #region Source Rank
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetONSourceType", "CheckAllCheckBox(divSMType);", true);
                                #endregion

                                #region Set all to OFF
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMSource", "SetFilterStatus('divSMSourceStatus','imgShowSMSourceFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMTime", "SetFilterStatus('divSMTimeFilterStatus','imgSMTimeFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMCategory", "SetFilterStatus('divSMCategoryStatus','imgShowSMCategoryFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMType", "SetFilterStatus('divSMTypeStatus','imgShowSMTypeFilter','OFF');", true);
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSMRank", "SetFilterStatus('divSMRankStatus','imgShowSMRankFilter','OFF');", true);

                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterSocialMedia", "SetFilterStatus('divSMFilterStatus',null,'');", true);

                                #endregion

                                #endregion
                            }
                        }
                    }

                    if (_SessionInformation.IsiQPremiumTwitter)
                    {
                        if (_xdoc.Root.Element("Twitter") != null)
                        {
                            #region Twitter Filter

                            #region Set Twitter Time Filter

                            DateTime _TwitterFromDate = new DateTime();
                            if (_xdoc.Descendants("Twitter").Descendants("FromDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("Twitter").Descendants("FromDate").FirstOrDefault().Value, out _TwitterFromDate))
                            {
                                txtTwitterStartDate.Text = _TwitterFromDate.Date.ToShortDateString();
                                int Hour = _TwitterFromDate.Hour < 12 ? _TwitterFromDate.Hour : _TwitterFromDate.Hour - 12;
                                ddlTwitterStartHour.SelectedValue = _TwitterFromDate.Hour == 12 ? "0" : Hour.ToString();
                                rbTwitterInterval.Checked = true;
                                rbTwitterDuration.Checked = false;
                                rbTwitterStart.SelectedIndex = _TwitterFromDate.Hour < 12 ? 0 : 1;
                            }

                            DateTime _TwitterToDate = new DateTime();
                            if (_xdoc.Descendants("Twitter").Descendants("ToDate").FirstOrDefault() != null && DateTime.TryParse(_xdoc.Descendants("Twitter").Descendants("ToDate").FirstOrDefault().Value, out _TwitterToDate))
                            {
                                txtTwitterEndDate.Text = _TwitterToDate.Date.ToShortDateString();
                                int Hour = _TwitterToDate.Hour < 12 ? _TwitterToDate.Hour : _TwitterToDate.Hour - 12;
                                ddlTwitterEndHour.SelectedValue = _TwitterToDate.Hour == 12 ? "0" : Hour.ToString();
                                rbTwitterInterval.Checked = true;
                                rbTwitterDuration.Checked = false;
                                rbTwitterEnd.SelectedIndex = _TwitterToDate.Hour < 12 ? 0 : 1;
                            }

                            if (_xdoc.Descendants("Twitter").Descendants("DayDuration").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("Twitter").Descendants("DayDuration").FirstOrDefault().Value))
                            {
                                if (ddlTwitterDuration.Items.FindByValue(_xdoc.Descendants("Twitter").Descendants("DayDuration").FirstOrDefault().Value) != null)
                                {
                                    ddlTwitterDuration.SelectedValue = _xdoc.Descendants("Twitter").Descendants("DayDuration").FirstOrDefault().Value;
                                }
                                rbTwitterDuration.Checked = true;
                                rbTwitterInterval.Checked = false;
                            }
                            #endregion

                            #region Set Twitter Source Filter

                            if (_xdoc.Descendants("Twitter").Elements("Actor").FirstOrDefault() != null && !string.IsNullOrEmpty(_xdoc.Descendants("Twitter").Elements("Actor").FirstOrDefault().Value))
                            {
                                txtTweetActor.Text = _xdoc.Descendants("Twitter").Elements("Actor").FirstOrDefault().Value;
                            }
                            else
                            {
                                txtTweetActor.Text = string.Empty;
                            }

                            #endregion

                            #region Set Twitter Count Filter

                            Int64 _TwitterFriendRangeFrom;
                            Int64 _TwitterFriendRangeTo;
                            if (_xdoc.Descendants("Twitter").Descendants("ActorFriendsRange").FirstOrDefault() != null)
                            {
                                if (_xdoc.Descendants("ActorFriendsRange").Descendants("From").FirstOrDefault() != null && Int64.TryParse(_xdoc.Descendants("ActorFriendsRange").Descendants("From").FirstOrDefault().Value, out _TwitterFriendRangeFrom))
                                {
                                    txtFriendsCountFrom.Text = _TwitterFriendRangeFrom.ToString();
                                }
                                else
                                {
                                    txtFriendsCountFrom.Text = string.Empty;
                                }

                                if (_xdoc.Descendants("ActorFriendsRange").Descendants("To").FirstOrDefault() != null && Int64.TryParse(_xdoc.Descendants("ActorFriendsRange").Descendants("To").FirstOrDefault().Value, out _TwitterFriendRangeTo))
                                {
                                    txtFriendsCountTo.Text = _TwitterFriendRangeTo.ToString();
                                }
                                else
                                {
                                    txtFriendsCountTo.Text = string.Empty;
                                }
                            }
                            else
                            {
                                txtFriendsCountFrom.Text = string.Empty;
                                txtFriendsCountTo.Text = string.Empty;
                            }

                            Int64 _TwitterFollowerRangeFrom;
                            Int64 _TwitterFollowerRangeTo;
                            if (_xdoc.Descendants("Twitter").Descendants("ActorFollowersRange").FirstOrDefault() != null)
                            {
                                if (_xdoc.Descendants("ActorFollowersRange").Descendants("From").FirstOrDefault() != null && Int64.TryParse(_xdoc.Descendants("ActorFollowersRange").Descendants("From").FirstOrDefault().Value, out _TwitterFollowerRangeFrom))
                                {
                                    txtFollowerCountFrom.Text = _TwitterFollowerRangeFrom.ToString();
                                }
                                else
                                {
                                    txtFollowerCountFrom.Text = string.Empty;
                                }

                                if (_xdoc.Descendants("ActorFollowersRange").Descendants("To").FirstOrDefault() != null && Int64.TryParse(_xdoc.Descendants("ActorFollowersRange").Descendants("To").FirstOrDefault().Value, out _TwitterFollowerRangeTo))
                                {
                                    txtFollowerCountTo.Text = _TwitterFollowerRangeTo.ToString();
                                }
                                else
                                {
                                    txtFollowerCountTo.Text = string.Empty;
                                }
                            }
                            else
                            {
                                txtFollowerCountFrom.Text = string.Empty;
                                txtFollowerCountTo.Text = string.Empty;
                            }

                            Int64 _TwitterKloutScoreFrom;
                            Int64 _TwitterKloutScoreTo;
                            if (_xdoc.Descendants("Twitter").Descendants("KloutScoreRange").FirstOrDefault() != null)
                            {
                                if (_xdoc.Descendants("KloutScoreRange").Descendants("From").FirstOrDefault() != null && Int64.TryParse(_xdoc.Descendants("KloutScoreRange").Descendants("From").FirstOrDefault().Value, out _TwitterKloutScoreFrom))
                                {
                                    txtKloutScoreFrom.Text = _TwitterKloutScoreFrom.ToString();
                                }
                                else
                                {
                                    txtKloutScoreFrom.Text = string.Empty;
                                }

                                if (_xdoc.Descendants("KloutScoreRange").Descendants("To").FirstOrDefault() != null && Int64.TryParse(_xdoc.Descendants("KloutScoreRange").Descendants("To").FirstOrDefault().Value, out _TwitterKloutScoreTo))
                                {
                                    txtKloutScoreTo.Text = _TwitterKloutScoreTo.ToString();
                                }
                                else
                                {
                                    txtKloutScoreTo.Text = string.Empty;
                                }
                            }
                            else
                            {
                                txtKloutScoreFrom.Text = string.Empty;
                                txtKloutScoreTo.Text = string.Empty;
                            }

                            #endregion



                            #endregion

                            chkTwitter.Checked = true;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowTwitterFilter", "$('#" + divMainTwitterFilter.ClientID + "').show();", true);
                            upTwitterFilter.Update();
                        }
                        else
                        {
                            chkTwitter.Checked = false;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTwitterFilter", "$('#" + divMainTwitterFilter.ClientID + "').hide();", true);

                            #region Twitter Filter

                            #region Time Filter
                            int _ToTime = DateTime.Now.Hour;
                            string _Script = "$('#" + rbTwitterDuration.ClientID + "').attr('checked', true);" +
                                "$('#" + rbTwitterStart.ClientID + " option').eq(12).attr('selected', 'selected');" +
                                "$('#" + txtTwitterStartDate.ClientID + "').val('" + DateTime.Today.AddDays(-1).ToShortDateString() + "');" +
                                "$('#" + txtTwitterEndDate.ClientID + "').val('" + System.DateTime.Now.ToShortDateString() + "');" +
                                "$('#" + ddlTwitterEndHour.ClientID + "').val('" + (_ToTime > 12 ? (_ToTime - 12).ToString() : _ToTime.ToString()) + "');" +
                                "$('#" + ddlTwitterStartHour.ClientID + "').val('0');" +
                                "$('#" + ddlTwitterDuration.ClientID + "').attr('disabled', false);" +
                               "$('#" + rbTwitterInterval.ClientID + "').attr('checked', false);" +
                               "$('#" + ddlTwitterDuration.ClientID + " option').eq(0).attr('selected', 'selected');" +
                               "$('#divSMInterval').hide();";

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTwitterTimer", _Script, true);

                            #endregion

                            #region Source Filter

                            _Script = "$('#" + txtTweetActor.ClientID + "').val('');";

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTwitterSource", _Script, true);

                            #endregion

                            #region Count Filter
                            _Script = "$('#" + txtFollowerCountFrom.ClientID + "').val('');"
                                + "$('#" + txtFollowerCountTo.ClientID + "').val('');"
                                + "$('#" + txtFriendsCountFrom.ClientID + "').val('');"
                                + "$('#" + txtFriendsCountTo.ClientID + "').val('');"
                                + "$('#" + txtKloutScoreFrom.ClientID + "').val('');"
                                + "$('#" + txtKloutScoreTo.ClientID + "').val('');";

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTwitterCount", _Script, true);

                            #endregion

                            #region Set all to OFF

                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterSource", "SetFilterStatus('divTwitterSourceFilterStatus','imgTwitterSourceFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterTime", "SetFilterStatus('divTwitterTimeFilterStatus','imgTwitterTimeFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterCount", "SetFilterStatus('divTwitterCountFilterStatus','imgTwitterCountFilter','OFF');", true);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitterScore", "SetFilterStatus('divTwitterScoreFilterStatus','imgTwitterScoreFilter','OFF');", true);

                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTwitter", "SetFilterStatus('divTwitterFilterStatus',null,'');", true);

                            #endregion

                            #endregion

                        }
                    }

                    upMainSearch.Update();
                    SetFilters();

                }
                else
                {
                    BindStatSkedProgData();
                    hdnTVData.Value = "1";
                    divMainTVFilter.Visible = true;
                    chkSocialMedia.Checked = false;
                    chkNews.Checked = false;
                    chkTV.Checked = true;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowFilters", "$('#" + divMainTVFilter.ClientID + "').show();$('#" + divMainOnlineNewsFilter.ClientID + "').hide();$('#" + divMainSMFilter.ClientID + "').hide();", true);
                    txtSearch.Text = string.Empty;

                    #region TV Filter

                    #region Time FIlter

                    string _Script = "$('#" + rbDuration1.ClientID + "').attr('checked', true);" +
                        "$('#" + rbDuration2.ClientID + "').attr('checked', false);" +
                        "$('#" + ddlDuration.ClientID + " option').eq(0).attr('selected', 'selected');";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTVTimer", _Script, true);

                    #endregion

                    #region Program Filter

                    _Script = "$('#" + txProgramTitle.ClientID + "').val('');" +
                    "$('#" + txtAppearing.ClientID + "').val('');";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetTVProgramFilter", _Script, true);

                    #endregion

                    #region Set All Dma

                    #region Region Vise

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResetRegion", "$('#" + ddlMarket.ClientID + " option').eq(0).attr('selected', 'selected');", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetmarketRegion", "CheckAllCheckBox(divMainRegionFilter);", true);

                    #endregion

                    #region Rank Vise

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetmarketrank", "CheckAllCheckBox(divMarketRankFilter);", true);

                    #endregion

                    #endregion

                    #region Set All Affils

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetTVStation", "CheckAllCheckBox(divStationFilter);", true);

                    #endregion

                    #region Set All Category

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "resetTVCategory", "CheckAllCheckBox(divCategoryFilter);", true);

                    #endregion


                    #region Set all to OFF
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTitle", "SetFilterStatus('divProgramFilterStatus','imgProgramFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVTime", "SetFilterStatus('divTimeFilterStatus','imgTimeFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('divMarketFilterStatus','imgMarketFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('divStationFilterStatus','imgStatusFilter','OFF');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCat", "SetFilterStatus('divCategoryFilterStatus','imgCategoryFilter','OFF');", true);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTV", "SetFilterStatus('divTVFilterStatus',null,'');", true);

                    #endregion


                    #endregion

                    upTVFilter.Update();
                    upMainSearch.Update();


                }
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public bool CheckForRightByXML(string xmlData)
        {
            bool hasAllRights = true;
            try
            {

                if (!string.IsNullOrEmpty(xmlData))
                {

                    XDocument xDoc = XDocument.Parse(xmlData);
                    if (xDoc.Root.Element("News") != null && !_SessionInformation.IsiQPremiumNM)
                    {
                        hasAllRights = false;
                    }

                    if (xDoc.Root.Element("SocialMedia") != null && !_SessionInformation.IsiQPremiumSM)
                    {
                        hasAllRights = false;
                    }

                    if (xDoc.Root.Element("Twitter") != null && !_SessionInformation.IsiQPremiumTwitter)
                    {
                        hasAllRights = false;
                    }

                }


            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
            return hasAllRights;
        }

        public void LoadSavedSearch()
        {
            try
            {

                if (_viewstateInformation.CurrentPageSavedSearch == null)
                    _viewstateInformation.CurrentPageSavedSearch = 0;

                int TotalRecords = 0;
                int? DefualtSavedSearchID = _viewstateInformation.LoadedSavedSearch == null ? null : (int?)_viewstateInformation.LoadedSavedSearch.ID;
                IIQCustomer_SavedSearchController _IIQCustomer_SavedSearchController = _ControllerFactory.CreateObject<IIQCustomer_SavedSearchController>();
                List<SavedSearch> listOfSavedSearch = _IIQCustomer_SavedSearchController.GetSavedSearchBasedOnClientGUID(new Guid(_SessionInformation.CustomerGUID), new Guid(_SessionInformation.ClientGUID), _viewstateInformation.CurrentPageSavedSearch.Value, gvSavedSearch.PageSize, DefualtSavedSearchID, out TotalRecords);


                if (_viewstateInformation.LoadedSavedSearch != null)
                {
                    listOfSavedSearch.Insert(0, _viewstateInformation.LoadedSavedSearch);
                    gvSavedSearch.DataSource = listOfSavedSearch;
                }
                else
                    gvSavedSearch.DataSource = listOfSavedSearch;
                gvSavedSearch.DataBind();

                _viewstateInformation.TotalRecordsCountSavedSearch = TotalRecords;

                SetViewstateInformation(_viewstateInformation);

                if (_viewstateInformation.TotalRecordsCountSavedSearch > 0)
                {
                    //ucCustomPagerSavedSearch.Visible = true;
                    if (Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(_viewstateInformation.TotalRecordsCountSavedSearch) / gvSavedSearch.PageSize)) - 1 == _viewstateInformation.CurrentPageSavedSearch)
                    {
                        (gvSavedSearch.FooterRow.FindControl("btnNext") as LinkButton).Visible = false;
                        (gvSavedSearch.FooterRow.FindControl("btnLast") as LinkButton).Visible = false;
                    }
                    else
                    {
                        (gvSavedSearch.FooterRow.FindControl("btnNext") as LinkButton).Visible = true;
                        (gvSavedSearch.FooterRow.FindControl("btnLast") as LinkButton).Visible = true;
                    }

                    if (_viewstateInformation.CurrentPageSavedSearch == 0)
                    {
                        (gvSavedSearch.FooterRow.FindControl("btnFirst") as LinkButton).Visible = false;
                        (gvSavedSearch.FooterRow.FindControl("btnPrev") as LinkButton).Visible = false;
                    }
                    else
                    {
                        (gvSavedSearch.FooterRow.FindControl("btnFirst") as LinkButton).Visible = true;
                        (gvSavedSearch.FooterRow.FindControl("btnPrev") as LinkButton).Visible = true;
                    }
                }

                upSavedSearh.Update();

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void SearchNewsSection(SearchNewsRequest searchNewsRequest, Boolean needToBindNewsChart)
        {
            try
            {
                /*if (!chkNews.Checked)
                {
                    gvOnlineNews.DataSource = null;
                    gvOnlineNews.DataBind();
                    ucOnlineNewsPager.Visible = false;
                    return;
                }*/

                Uri newsSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrNewsUrl]);
                SearchEngine _SearchEngine = new SearchEngine(newsSearchURI);
                //SearchNewsRequest searchNewsRequest = (SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews;

                var currentContext = HttpContext.Current;

                if ((_viewstateInformation != null && _viewstateInformation.listOnlineNewsChartZoomHistory != null && _viewstateInformation.listOnlineNewsChartZoomHistory.Count > 0) || !needToBindNewsChart)
                {
                    BinNewsGrid(searchNewsRequest, _SearchEngine, currentContext);
                }
                else
                {
                    Task[] tasks = new Task[2]
                    {
                        Task.Factory.StartNew(() => BinNewsGrid(searchNewsRequest,_SearchEngine,currentContext)),
                        Task.Factory.StartNew(() => BindNewsChart(searchNewsRequest,_SearchEngine,currentContext))

                    };

                    try
                    {
                        Task.WaitAll(tasks);
                    }
                    catch (Exception ex)
                    {
                        this.WriteException(ex);
                        lblNewsMsg.Visible = true;
                        lblNewsMsg.Text = CommonConstants.MsgSolrSearchUA;
                    }

                    upNewsChart.Update();
                }

                if (_viewstateInformation != null && (string.IsNullOrEmpty(_viewstateInformation.SortExpressionOnlineNews) || _viewstateInformation.IsOnlineNewsSortDirecitonAsc == null))
                {
                    _viewstateInformation.SortExpressionOnlineNews = "date";
                    _viewstateInformation.IsOnlineNewsSortDirecitonAsc = false;
                    SetViewstateInformation(_viewstateInformation);
                    SortClipDirection("date", false, gvOnlineNews);
                }
                else
                {
                    SortClipDirection(_viewstateInformation.SortExpressionOnlineNews, _viewstateInformation.IsOnlineNewsSortDirecitonAsc, gvOnlineNews);
                }

                upOnlineNews.Update();
                // commnted filter update on 14-12-2012
                //upFilter.Update();

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void SearchSocialMediaSection(SearchSMRequest searchSMRequest, Boolean needToBindSMChart)
        {
            try
            {
                Uri smSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrSMUrl]);
                SearchEngine searchEngine = new SearchEngine(smSearchURI);
                //SearchSMRequest searchSMRequest = (SearchSMRequest)_viewstateInformation.searchRequestSM;

                var currentContext = HttpContext.Current;
                if ((_viewstateInformation != null && _viewstateInformation.listSocialMediaChartZoomHistory != null && _viewstateInformation.listSocialMediaChartZoomHistory.Count > 0) || !needToBindSMChart)
                {
                    BindSMGrid(searchSMRequest, searchEngine, currentContext);
                }
                else
                {
                    Task[] tasks = new Task[2]
                     {
                         Task.Factory.StartNew(() => BindSMGrid(searchSMRequest,searchEngine,currentContext)),
                         Task.Factory.StartNew(() => BindSMChart(searchSMRequest,searchEngine,currentContext))
                     };

                    try
                    {
                        Task.WaitAll(tasks);
                    }
                    catch (Exception ex)
                    {
                        this.WriteException(ex);
                        lblSMMsg.Visible = true;
                        lblSMMsg.Text = CommonConstants.MsgSolrSearchUA;
                    }
                    upSocialMediaChart.Update();
                }

                if (_viewstateInformation != null && (string.IsNullOrEmpty(_viewstateInformation.SortExpressionSocialMedia) || _viewstateInformation.IsSocialMediaSortDirecitonAsc == null))
                {
                    _viewstateInformation.SortExpressionSocialMedia = "date";
                    _viewstateInformation.IsSocialMediaSortDirecitonAsc = false;
                    SetViewstateInformation(_viewstateInformation);
                    SortClipDirection("date", false, gvSocialMedia);
                }
                else
                {
                    SortClipDirection(_viewstateInformation.SortExpressionSocialMedia, _viewstateInformation.IsSocialMediaSortDirecitonAsc, gvSocialMedia);
                }

                upSocialMedia.Update();
                // commnted filter update on 14-12-2012
                //upFilter.Update();

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void SearchTwitterSection(SearchTwitterRequest searchTwitterRequest, Boolean needToBindTwitterChart)
        {
            try
            {
                Uri twitterSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrTwitterUrl]);
                SearchEngine searchEngine = new SearchEngine(twitterSearchURI);

                var currentContext = HttpContext.Current;

                if (String.IsNullOrWhiteSpace(_viewstateInformation.SortExpressionTwitter))
                {
                    ddlTwitterSortExp.SelectedIndex = 0;
                    rbTwitterSortDir.SelectedIndex = 1;
                }

                if ((_viewstateInformation != null && _viewstateInformation.listTwitterChartZoomHistory != null && _viewstateInformation.listTwitterChartZoomHistory.Count > 0) || !needToBindTwitterChart)
                {
                    BindTwitterGrid(searchTwitterRequest, searchEngine, currentContext);
                }
                else
                {
                    Task[] tasks = new Task[2]
                     {
                         Task.Factory.StartNew(() => BindTwitterGrid(searchTwitterRequest, searchEngine, currentContext)),
                         Task.Factory.StartNew(() => BindTwitterChart(searchTwitterRequest,searchEngine,currentContext))
                     };

                    try
                    {
                        Task.WaitAll(tasks);
                    }
                    catch (Exception ex)
                    {
                        this.WriteException(ex);
                        lblTwitterMsg.Visible = true;
                        lblTwitterMsg.Text = CommonConstants.MsgSolrSearchUA;
                    }
                    upTwitterChart.Update();
                }


                if (_viewstateInformation != null && (string.IsNullOrEmpty(_viewstateInformation.SortExpressionTwitter) || _viewstateInformation.IsTwitterSortDirecitonAsc == null))
                {
                    _viewstateInformation.SortExpressionSocialMedia = "date";
                    _viewstateInformation.IsSocialMediaSortDirecitonAsc = false;
                    SetViewstateInformation(_viewstateInformation);
                }


                upTwitter.Update();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void BindSMGrid(SearchSMRequest searchSMRequest, SearchEngine searchEngine, HttpContext currentContext)
        {
            try
            {
                ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
                SearchSMResult searchSMResult = _ISocialMediaController.GetSocialMediaGridData(searchSMRequest, searchEngine);

                XmlDocument _XmlDocument = new XmlDocument();

                string searchResponse = string.Empty;
                _XmlDocument.LoadXml(searchSMResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            searchResponse = _XmlDocument.InnerXml;
                            searchResponse = searchResponse.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    searchResponse = null;
                }


                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                _ISearchLogController.InsertSearchLog(currentContext,
                                            SearchType.IQPremium_SM.ToString(),
                                            searchSMRequest.SearchTerm,
                                            searchSMRequest.PageNumber,
                                            searchSMRequest.PageSize,
                                            0,
                                            searchSMRequest.StartDate,
                                            searchSMRequest.EndDate,
                                            searchResponse,
                                            searchEngine.Url.ToString());

                if (searchSMResult != null)
                {
                    gvSocialMedia.DataSource = searchSMResult.smResults;
                    gvSocialMedia.DataBind();
                }

                gvSocialMedia.Visible = true;
                ucSMPager.TotalRecords = searchSMResult.TotalResults;
                ucSMPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                ucSMPager.BindDataList();

                if (searchSMResult != null && searchSMResult.smResults != null && searchSMResult.smResults.Count > 0)
                {
                    ucSMPager.Visible = true;
                    lblSMChart.Visible = true;
                    spnSMChartProgramFound.Visible = true;

                    if (!_viewstateInformation.IsCompeteData)
                    {
                        gvSocialMedia.Columns[7].Visible = false;
                        gvSocialMedia.Columns[8].Visible = false;
                        if (!_SessionInformation.IsiQPremiumSentiment)
                        {
                            gvSocialMedia.Columns[10].Visible = false;
                            gvSocialMedia.Columns[0].HeaderStyle.Width = Unit.Percentage(8);
                            gvSocialMedia.Columns[1].HeaderStyle.Width = Unit.Percentage(13);
                            gvSocialMedia.Columns[2].HeaderStyle.Width = Unit.Percentage(23);
                            gvSocialMedia.Columns[3].HeaderStyle.Width = Unit.Percentage(14);
                            gvSocialMedia.Columns[4].HeaderStyle.Width = Unit.Percentage(10);
                            gvSocialMedia.Columns[5].HeaderStyle.Width = Unit.Percentage(10);
                            gvSocialMedia.Columns[6].HeaderStyle.Width = Unit.Percentage(7);
                            gvSocialMedia.Columns[9].HeaderStyle.Width = Unit.Percentage(10);
                            gvSocialMedia.Columns[11].HeaderStyle.Width = Unit.Percentage(5);
                        }
                        else
                        {
                            gvSocialMedia.Columns[0].HeaderStyle.Width = Unit.Percentage(8);
                            gvSocialMedia.Columns[1].HeaderStyle.Width = Unit.Percentage(13);
                            gvSocialMedia.Columns[2].HeaderStyle.Width = Unit.Percentage(17);
                            gvSocialMedia.Columns[3].HeaderStyle.Width = Unit.Percentage(14);
                            gvSocialMedia.Columns[4].HeaderStyle.Width = Unit.Percentage(10);
                            gvSocialMedia.Columns[5].HeaderStyle.Width = Unit.Percentage(8);
                            gvSocialMedia.Columns[6].HeaderStyle.Width = Unit.Percentage(6);
                            gvSocialMedia.Columns[9].HeaderStyle.Width = Unit.Percentage(9);
                            gvSocialMedia.Columns[10].HeaderStyle.Width = Unit.Percentage(10);
                            gvSocialMedia.Columns[11].HeaderStyle.Width = Unit.Percentage(5);
                        }

                    }
                    else
                    {

                        /*var distinctDisplayUrl = searchSMResult.smResults.Select(a => !string.IsNullOrWhiteSpace(a.homeLink) ? new Uri(a.homeLink).Host.Replace("www.", "") : string.Empty).Distinct().ToList();

                        var displyUrlXml = new XElement("list",
                                                from string websiteurl in distinctDisplayUrl
                                                select new XElement("item", new XAttribute("url", websiteurl)));*/


                        List<SMResult> lstSMResults = searchSMResult.smResults.Select(a => new SMResult()
                                                                                    {
                                                                                        homeLink = !string.IsNullOrWhiteSpace(a.homeLink) ? new Uri(a.homeLink).Host.Replace("www.", "") : string.Empty,
                                                                                        feedClass = !string.IsNullOrWhiteSpace(a.feedClass) ? a.feedClass : string.Empty
                                                                                    }
                                                                                    ).GroupBy(h => h.homeLink)
                                                                                       .Select(s => s.First()).ToList();

                        var displyUrlXml = new XElement("list",
                                                from SMResult smres in lstSMResults
                                                select new XElement("item", new XAttribute("url", smres.homeLink), new XAttribute("sourceCategory", smres.feedClass)));

                        IIQ_CompeteAllController _IIQ_CompeteAllController = _ControllerFactory.CreateObject<IIQ_CompeteAllController>();
                        List<IQ_CompeteAll> _ListOfIQ_CompeteAll = _IIQ_CompeteAllController.GetArtileAdShareValueByClientGuidAndXml(new Guid(_SessionInformation.ClientGUID), displyUrlXml.ToString(), Comete_MediaType.SM.ToString());

                        foreach (GridViewRow _GridViewRow in gvSocialMedia.Rows)
                        {
                            string href = !string.IsNullOrWhiteSpace((_GridViewRow.FindControl("aPublication") as HtmlAnchor).HRef) ? new Uri((_GridViewRow.FindControl("aPublication") as HtmlAnchor).HRef).Host.Replace("www.", "") : string.Empty;
                            IQ_CompeteAll _IQ_CompeteAll = _ListOfIQ_CompeteAll.Find(a => a.CompeteURL.Equals(href));

                            (_GridViewRow.FindControl("caud") as HtmlTableCell).InnerHtml = _IQ_CompeteAll == null || (_IQ_CompeteAll.c_uniq_visitor == null || !_IQ_CompeteAll.IsUrlFound) ? "NA" : string.Format("{0:n0}", _IQ_CompeteAll.c_uniq_visitor);
                            if ((_IQ_CompeteAll != null && (_IQ_CompeteAll.c_uniq_visitor == -1)))
                            {
                                (_GridViewRow.FindControl("caud") as HtmlTableCell).InnerHtml = "";
                            }
                            (_GridViewRow.FindControl("ccmpt") as HtmlTableCell).InnerHtml = (_IQ_CompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\"  title=\"Powered by Compete\" />" : "");
                            //_GridViewRow.Cells[7].Text = _IQ_CompeteAll == null || _IQ_CompeteAll.c_uniq_visitor == null ? "NA" : string.Format("{0:n0}", _IQ_CompeteAll.c_uniq_visitor) + (_IQ_CompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\" title=\"Powered by Compete\" />" : "");
                            _GridViewRow.Cells[8].Text = _IQ_CompeteAll == null || (_IQ_CompeteAll.IQ_AdShare_Value == null || !_IQ_CompeteAll.IsUrlFound) ? "NA" : string.Format("{0:C}", _IQ_CompeteAll.IQ_AdShare_Value); //+ (_IQ_CompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\" title=\"Powered by Compete\" />" : ""); 

                            if ((_IQ_CompeteAll != null && (_IQ_CompeteAll.IQ_AdShare_Value == -1)))
                            {
                                _GridViewRow.Cells[8].Text = "";
                            }


                        }

                        if (!_SessionInformation.IsiQPremiumSentiment)
                        {
                            gvSocialMedia.Columns[10].Visible = false;
                            gvSocialMedia.Columns[0].HeaderStyle.Width = Unit.Percentage(6);
                            gvSocialMedia.Columns[1].HeaderStyle.Width = Unit.Percentage(13);
                            gvSocialMedia.Columns[2].HeaderStyle.Width = Unit.Percentage(16);
                            gvSocialMedia.Columns[3].HeaderStyle.Width = Unit.Percentage(9);
                            gvSocialMedia.Columns[4].HeaderStyle.Width = Unit.Percentage(9);
                            gvSocialMedia.Columns[5].HeaderStyle.Width = Unit.Percentage(8);
                            gvSocialMedia.Columns[6].HeaderStyle.Width = Unit.Percentage(6);
                            gvSocialMedia.Columns[7].HeaderStyle.Width = Unit.Percentage(10);
                            gvSocialMedia.Columns[8].HeaderStyle.Width = Unit.Percentage(10);
                            gvSocialMedia.Columns[9].HeaderStyle.Width = Unit.Percentage(8);
                            gvSocialMedia.Columns[11].HeaderStyle.Width = Unit.Percentage(5);
                        }
                    }
                }
                else
                {
                    ucSMPager.Visible = false;
                    lblSMChart.Visible = false;
                    spnSMChartProgramFound.Visible = true;
                }


                lblSMChart.Text = string.IsNullOrWhiteSpace(Convert.ToString(searchSMResult.TotalResults)) ? "0" : searchSMResult.TotalResults.ToString("#,#");

                if (_viewstateInformation != null && _viewstateInformation.listSocialMediaChartZoomHistory != null && _viewstateInformation.listSocialMediaChartZoomHistory.Count > 0)
                {
                    spnSMChart.Visible = true;
                }
                else
                {
                    spnSMChart.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        public void BindSMChart(SearchSMRequest searchSMRequest, SearchEngine searchEngine, HttpContext currentContext)
        {
            try
            {
                ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
                Boolean isError = false;
                String jsonForChart = _ISocialMediaController.GetSocialMediaChartData(searchSMRequest, searchEngine, currentContext, out isError);

                if (!string.IsNullOrWhiteSpace(jsonForChart))
                {
                    FusionCharts.SetDataFormat("json");
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]))
                    {
                        divSocialMediaChart.InnerHtml = FusionCharts.RenderChart("http://localhost:2281/fusionchart/ZoomLine.swf", "", jsonForChart, "divSocialMediaChart", chartWidth, chartHeight, false, false);
                    }
                    else
                    {
                        divSocialMediaChart.InnerHtml = FusionCharts.RenderChart("http://" + Request.Url.Host + "/fusionchart/ZoomLine.swf?v=1.1", "", jsonForChart, "divSocialMediaChart", chartWidth, chartHeight, false, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void BindTwitterGrid(SearchTwitterRequest searchTwitterRequest, SearchEngine searchEngine, HttpContext currentContext)
        {
            try
            {
                bool isError;
                SearchTwitterResult _SearchTwitterResult = searchEngine.SearchTwitter(searchTwitterRequest, false, out isError);
                XmlDocument _XmlDocument = new XmlDocument();

                string searchResponse = string.Empty;
                _XmlDocument.LoadXml(_SearchTwitterResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            searchResponse = _XmlDocument.InnerXml;
                            searchResponse = searchResponse.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    searchResponse = null;
                }

                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                _ISearchLogController.InsertSearchLog(currentContext, SearchType.IQPremium_NM.ToString(), searchTwitterRequest.SearchTerm, searchTwitterRequest.PageNumber, searchTwitterRequest.PageSize, 0, searchTwitterRequest.StartDate, searchTwitterRequest.EndDate, searchResponse, searchEngine.Url.ToString());

                if (_SearchTwitterResult != null && _SearchTwitterResult.TwitterResults != null)
                {
                    dlTweets.DataSource = _SearchTwitterResult.TwitterResults;
                    dlTweets.DataBind();
                    dlTweets.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;border:1px solid #999999;");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "$(\"div[id='datalistInner']:last\").css(\"border\", \"none\");", true);
                }

                dlTweets.Visible = true;
                ucTwitterPager.TotalRecords = _SearchTwitterResult.TotalResults;
                ucTwitterPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                ucTwitterPager.BindDataList();


                if (_SearchTwitterResult != null && _SearchTwitterResult.TwitterResults != null && _SearchTwitterResult.TwitterResults.Count > 0)
                {
                    ucTwitterPager.Visible = true;
                    spnTwitterChartProgramFound.Visible = true;
                    lblTwitterChart.Visible = true;
                    divNoResultTwitter.Visible = false;
                    pnlTwitterSort.Visible = true;

                    if (!_SessionInformation.IsiQPremiumSentiment)
                    {
                        foreach (DataListItem _DataListItem in dlTweets.Items)
                        {
                            if (_DataListItem.ItemType == ListItemType.Item || _DataListItem.ItemType == ListItemType.AlternatingItem)
                            {
                                _DataListItem.FindControl("divSentiment").Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    ucTwitterPager.Visible = false;
                    spnTwitterChartProgramFound.Visible = false;
                    lblTwitterChart.Visible = false;
                    divNoResultTwitter.Visible = true;
                    pnlTwitterSort.Visible = false;
                }

                lblTwitterChart.Text = string.IsNullOrWhiteSpace(Convert.ToString(_SearchTwitterResult.TotalResults)) ? "0" : _SearchTwitterResult.TotalResults.ToString("#,#");

                if (_viewstateInformation != null && _viewstateInformation.listTwitterChartZoomHistory != null && _viewstateInformation.listTwitterChartZoomHistory.Count > 0)
                {
                    spnTwitterChart.Visible = true;
                }
                else
                {
                    spnTwitterChart.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void BindTwitterChart(SearchTwitterRequest searchTwitterRequest, SearchEngine searchEngine, HttpContext currentContext)
        {
            try
            {
                Boolean isError = false;
                SearchTwitterResult searchTwitterResult = searchEngine.SearchTwitter(searchTwitterRequest, true, out isError);
                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                string searchResponse = string.Empty;
                if (isError)
                {
                    XmlDocument _XmlDocument = new XmlDocument();

                    _XmlDocument.LoadXml(searchTwitterResult.ResponseXml);

                    XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                    if (_XmlNodeList.Count > 0)
                    {
                        XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                        foreach (XmlAttribute item in _XmlAttributeCollection)
                        {
                            if (item.Name == "status")
                            {
                                searchResponse = _XmlDocument.InnerXml;
                                searchResponse = searchResponse.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                            }
                        }
                    }
                    else
                    {
                        searchResponse = null;
                    }
                }
                else
                {
                    jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchTwitterResult.ResponseXml);
                    searchResponse = null;
                }

                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                _ISearchLogController.InsertSearchLog(currentContext, SearchType.IQPremium_Twitter.ToString(), searchTwitterRequest.SearchTerm, searchTwitterRequest.PageNumber, searchTwitterRequest.PageSize, 0, searchTwitterRequest.StartDate, searchTwitterRequest.EndDate, searchResponse, searchEngine.Url.ToString());


                _viewstateInformation = GetViewstateInformation();
                ZoomChart zoomChartTweets = new ZoomChart();

                zoomChartTweets.Dataset = new List<Dataset>();
                zoomChartTweets.Categories = new List<Category>();

                if (!isError)
                {
                    string totalcount = Convert.ToString(jsonData["facet_counts"]["facet_ranges"][searchTwitterRequest.FacetRange]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);

                    Category category = new Category();
                    category.Category1 = new List<Category2>();
                    zoomChartTweets.Categories.Add(category);

                    Dataset datasetValue = new Dataset();

                    datasetValue.Seriesname = "Tweets";
                    datasetValue.Data = new List<Datum>();

                    zoomChartTweets.Dataset.Add(datasetValue);

                    string[] facetData = totalcount.Split(',');

                    for (int i = 0; i < facetData.Length; i = i + 2)
                    {
                        datasetValue.Data.Add(new Datum() { Value = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty)) });
                        category.Category1.Add(new Category2() { Label = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt") });
                    }
                }

                string chartTitle = string.Empty;
                zoomChartTweets.Chart = new Chart
                {
                    numVisibleLabels = ConfigurationManager.AppSettings[CommonConstants.ConfigZoomChartNoOfLables],
                    caption = string.Empty,
                    subcaption = "",
                    exportEnabled = "1",
                    exportAtClient = "1",
                    exportHandler = "fcBatchExporterTwitter",
                    allowpinmode = "0",
                    drawAnchors = "0",
                    bgColor = "FFFFFF",
                    showBorder = "0",
                    yaxisname = "Tweets",
                    chartRightMargin = ConfigurationManager.AppSettings[CommonConstants.ConfigChartRightMargin],
                    formatnumberscale = "0"

                };


                string jsonForChart = CommonFunctions.SearializeJson(zoomChartTweets);

                if (!string.IsNullOrWhiteSpace(jsonForChart))
                {
                    FusionCharts.SetDataFormat("json");
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]))
                    {
                        divTwitterChart2.InnerHtml = FusionCharts.RenderChart("http://localhost:2281/fusionchart/ZoomLine.swf", "", jsonForChart, "divTwitterChart2", chartWidth, chartHeight, false, false);

                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "loadZoomChart", "LoadZoomChart('" + jsonForChart + "');", true);
                    }
                    else
                    {
                        divTwitterChart2.InnerHtml = FusionCharts.RenderChart("http://" + Request.Url.Host + "/fusionchart/ZoomLine.swf?v=1.1", "", jsonForChart, "divTwitterChart2", chartWidth, chartHeight, false, false);
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void BindGrid(SearchRequest _SearchRequest, SearchEngine _SearchEngine, HttpContext _HttpContext)
        {
            try
            {

                Logger.Info("Bind Grid for TV Request Starts");

                Logger.Info("PMG Request Start");

                SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest);

                Logger.Info("PMG Request End");

                Logger.Info("Parse Result Object Start");

                string _Responce = string.Empty;
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.LoadXml(_SearchResult.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            _Responce = _XmlDocument.InnerXml;
                            _Responce = _Responce.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    _Responce = null;
                }


                List<RawMedia> _ListOfRawMedia = new List<RawMedia>();

                bool _IsPmgSearchTotalHitsFromConfig = false;


                if (ConfigurationManager.AppSettings["PMGSearchTotalHitsFromConfig"] != null)
                {
                    bool.TryParse(ConfigurationManager.AppSettings["PMGSearchTotalHitsFromConfig"], out _IsPmgSearchTotalHitsFromConfig);
                }

                if (_IsPmgSearchTotalHitsFromConfig == true)
                {
                    int _MaxPMGHitsCount = 100;
                    if (ConfigurationManager.AppSettings["PMGMaxListCount"] != null)
                    {
                        int.TryParse(ConfigurationManager.AppSettings["PMGMaxListCount"], out _MaxPMGHitsCount);
                    }

                    _SearchResult.TotalHitCount = _MaxPMGHitsCount;
                }


                //lblNoOfRawMedia.Text = _SearchResult.TotalHitCount.ToString();                
                //StringBuilder iqcckeyList = new StringBuilder();
                XDocument xDoc = new XDocument(new XElement("list"));



                foreach (Hit _Hit in _SearchResult.Hits)
                {
                    xDoc.Root.Add(new XElement("item", new XAttribute("iq_cc_key", _Hit.Iqcckey), new XAttribute("iq_dma", _Hit.IQDmaNum)));
                    RawMedia _RawMedia = new RawMedia();
                    _RawMedia.RawMediaID = new Guid(_Hit.Guid);
                    _RawMedia.Hits = _Hit.TotalNoOfOccurrence;
                    _RawMedia.IQ_Dma_Name = _Hit.Market;
                    _RawMedia.DateTime = new DateTime(_Hit.Timestamp.Year, _Hit.Timestamp.Month, _Hit.Timestamp.Day, (_Hit.Hour / 100), 0, 0);
                    _RawMedia.IQ_CC_Key = _Hit.Iqcckey;
                    _RawMedia.Affiliate = _Hit.Affiliate;
                    _RawMedia.Title120 = _Hit.Title120;
                    _RawMedia.PositiveSentiment = _Hit.Sentiments.PositiveSentiment;
                    _RawMedia.NegativeSentiment = _Hit.Sentiments.NegativeSentiment;
                    //_RawMedia.FullSentiment = _Hit.Sentiments.FullSentiment;


                    //iqcckeyList.Append(",'" + _Hit.Iqcckey + "'");
                    if (File.Exists(_HttpContext.Server.MapPath("~/StationLogoImages/" + _Hit.StationId + ".gif")))
                    {
                        _RawMedia.StationLogo = "http://" + _HttpContext.Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _Hit.StationId + ".gif";
                    }
                    else if (File.Exists(_HttpContext.Server.MapPath("~/StationLogoImages/" + _Hit.StationId + ".jpg")))
                    {
                        _RawMedia.StationLogo = "http://" + _HttpContext.Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _Hit.StationId + ".jpg";
                    }

                    _ListOfRawMedia.Add(_RawMedia);
                }

                Logger.Info("Parse Result Object End");

                if (Convert.ToString(xDoc).Length > 0)
                {
                    Logger.Info("Get NielSen Data For TV Start");

                    IIQNielsenSquadController _IIQNielsenSquadController = _ControllerFactory.CreateObject<IIQNielsenSquadController>();
                    _ListOfRawMedia = _IIQNielsenSquadController.GetNielsenDataByXML(xDoc, _ListOfRawMedia, new Guid(_SessionInformation.ClientGUID));

                    Logger.Info("Get NielSen Data For TV End");
                }

                Logger.Info("Bind TV List to Grid Start");

                rptTV.DataSource = _ListOfRawMedia;
                rptTV.DataBind();




                lblNoOfRadioRawMedia.Text = string.IsNullOrWhiteSpace(Convert.ToString(_SearchResult.TotalHitCount)) ? "0" : _SearchResult.TotalHitCount.ToString("#,#");

                if (_ListOfRawMedia.Count > 0)
                {
                    rptTV.Visible = true;
                    divNoResults.Visible = false;
                    spnTotalProgramHeader.Visible = true;
                    lblNoOfRadioRawMedia.Visible = true;

                    if (!_SessionInformation.IsiQPremiumSentiment)
                    {
                        foreach (RepeaterItem _RepeaterItem in rptTV.Items)
                        {
                            if (_RepeaterItem.ItemType == ListItemType.Item || _RepeaterItem.ItemType == ListItemType.AlternatingItem)
                            {
                                _RepeaterItem.FindControl("divSentiment").Visible = false;
                                (_RepeaterItem.FindControl("divMarket") as HtmlGenericControl).Attributes.Add("style", "width:15%");
                                (_RepeaterItem.FindControl("divTitle") as HtmlGenericControl).Attributes.Add("style", "width:27%");
                                (_RepeaterItem.FindControl("divAdShare") as HtmlGenericControl).Attributes.Add("style", "width:10%");
                                (_RepeaterItem.FindControl("divHits") as HtmlGenericControl).Attributes.Add("style", "width:9%");
                            }
                        }

                        rptTV.Controls[0].Controls[0].FindControl("divHeaderSentiment").Visible = false;
                        (rptTV.Controls[0].Controls[0].FindControl("divHeaderMarket") as HtmlGenericControl).Attributes.Add("style", "width:15%");
                        (rptTV.Controls[0].Controls[0].FindControl("divHeaderTitle") as HtmlGenericControl).Attributes.Add("style", "width:27%");
                        (rptTV.Controls[0].Controls[0].FindControl("divHeaderAdShare") as HtmlGenericControl).Attributes.Add("style", "width:10%");
                        (rptTV.Controls[0].Controls[0].FindControl("divHeaderHits") as HtmlGenericControl).Attributes.Add("style", "width:9%");
                    }
                }
                else
                {
                    rptTV.Visible = false;
                    divNoResults.Visible = true;
                    spnTotalProgramHeader.Visible = false;
                    lblNoOfRadioRawMedia.Visible = false;
                }
                //}

                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                _ISearchLogController.InsertSearchLog(_HttpContext, SearchType.IQPremium_TV.ToString(), _SearchRequest.Terms, _SearchRequest.PageNumber, _SearchRequest.PageSize, _SearchRequest.MaxHighlights, _SearchRequest.StartDate, _SearchRequest.EndDate, _Responce, _SearchEngine.Url.ToString());

                ucCustomPager.TotalRecords = _SearchResult.TotalHitCount;
                ucCustomPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                //ucCustomPager.CurrentPage = _ViewstateInformation._CurrentClipPage;
                ucCustomPager.BindDataList();

                if (_viewstateInformation != null && _viewstateInformation.listchartZoomHistory != null && _viewstateInformation.listchartZoomHistory.Count > 0)
                {
                    spnChartActive.Visible = true;
                }
                else
                {
                    spnChartActive.Visible = false;
                }

                if (_ListOfRawMedia.Count > 0)
                {
                    ucCustomPager.Visible = true;
                }
                else
                {
                    ucCustomPager.Visible = false;
                }

                Logger.Info("Bind Grid for TV Request End");
            }
            catch (Exception ex)
            {
                lblNewsMsg.Visible = true;
                lblNewsMsg.Text = CommonConstants.MsgSolrSearchUA;
                this.WriteException(ex);
                // Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void BindChart(SearchRequest _SearchRequest, SearchEngine _SearchEngine, HttpContext currentContext)
        {
            try
            {
                Logger.Info("Bind Chart for TV Request Start");
                Boolean isError = false;
                SearchResult _SearchResult = _SearchEngine.SearchTVChart(_SearchRequest, out isError);

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();

                string searchResponse = string.Empty;
                if (isError)
                {
                    XmlDocument _XmlDocument = new XmlDocument();


                    _XmlDocument.LoadXml(_SearchResult.ResponseXml);

                    XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                    if (_XmlNodeList.Count > 0)
                    {
                        XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                        foreach (XmlAttribute item in _XmlAttributeCollection)
                        {
                            if (item.Name == "status")
                            {
                                searchResponse = _XmlDocument.InnerXml;
                                searchResponse = searchResponse.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                            }
                        }
                    }
                    else
                    {
                        searchResponse = null;
                    }
                }
                else
                {
                    jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(_SearchResult.ResponseXml);
                    searchResponse = null;
                }

                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                _ISearchLogController.InsertSearchLog(currentContext, SearchType.IQPremium_TV.ToString(), _SearchRequest.Terms, _SearchRequest.PageNumber, _SearchRequest.PageSize, _SearchRequest.MaxHighlights, _SearchRequest.StartDate, _SearchRequest.EndDate, searchResponse, _SearchEngine.Url.ToString());
                //((Newtonsoft.Json.Linq.JObject)jsonData)["response"]["docs"]
                //Newtonsoft.Json.Linq.JObject jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(res.ResponseXml);




                _viewstateInformation = GetViewstateInformation();
                ZoomChart zoomChartNews = new ZoomChart();

                zoomChartNews.Dataset = new List<Dataset>();



                zoomChartNews.Categories = new List<Category>();
                Category category = new Category();
                category.Category1 = new List<Category2>();
                zoomChartNews.Categories.Add(category);

                if (!isError)
                {
                    List<String> allStationCategoryNum = new List<String>();
                    Dictionary<String, String> dictNumName = new Dictionary<string, string>();

                    foreach (KeyValuePair<Dictionary<String, String>, List<String>> kvp in _SearchRequest.AffilForFacet)
                    {
                        if (kvp.Value.Count > 0)
                        {
                            foreach (KeyValuePair<String, String> kvpstationNumName in kvp.Key)
                            {
                                dictNumName.Add(kvpstationNumName.Key, kvpstationNumName.Value);
                                allStationCategoryNum.Add(kvpstationNumName.Key);
                            }
                        }
                    }

                    foreach (string AffilcategoryID in allStationCategoryNum)
                    {
                        string totalcount = Convert.ToString(jsonData["facet_counts"]["facet_ranges"][AffilcategoryID]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);

                        string[] facetData = totalcount.Split(',');
                        category.Category1 = new List<Category2>(10);

                        /*string catName = ((Dictionary<String, String>)from finalDict in
                                                                          (from p in request.AffilForFacet
                                                                           select p.Key)
                                                                      select finalDict)[AffilcategoryID];*/
                        string catName = Convert.ToString(dictNumName.Where(a => a.Key.Equals(AffilcategoryID)).First().Value);// Select(s => s.Value));
                        //Convert.ToString(((Dictionary<String, String>)request.AffilForFacet.Select(s => s.Key)).Where(x => x.Key.Equals(AffilcategoryID)).First().Value;// Select(v => v.Value));

                        Dataset datasetValue = new Dataset();

                        datasetValue.Seriesname = catName;
                        datasetValue.Data = new List<Datum>();

                        zoomChartNews.Dataset.Add(datasetValue);

                        for (int i = 0; i < facetData.Length; i = i + 2)
                        {
                            datasetValue.Data.Add(new Datum() { Value = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty)) });
                            category.Category1.Add(new Category2() { Label = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt") });
                        }
                    }

                }

                string chartTitle = string.Empty;
                zoomChartNews.Chart = new Chart
                {
                    numVisibleLabels = ConfigurationManager.AppSettings[CommonConstants.ConfigZoomChartNoOfLables],
                    caption = string.Empty,
                    subcaption = "",
                    exportEnabled = "1",
                    exportAtClient = "1",
                    exportHandler = "fcBatchExporter",
                    allowpinmode = "0",
                    drawAnchors = "0",
                    bgColor = "FFFFFF",
                    showBorder = "0",
                    yaxisname = "Programs",
                    chartRightMargin = ConfigurationManager.AppSettings[CommonConstants.ConfigChartRightMargin],
                    formatnumberscale = "0"
                };

                /* ChartData chartData = new ChartData();
                 chartData.data = listOfData;
                 chartData.Chart = new Chart { caption = "test Caption", numberprefix = "$", showvalues = "0", xAxisName = "date", yAxisName = "hits" };*/

                string jsonForChart = CommonFunctions.SearializeJson(zoomChartNews);
                // jsonForChart = CommonFunctions.SearializeJson(File.ReadAllText(Server.MapPath("~/1.json")));

                //string jsonForZoomChart = CommonFunctions.SearializeJson(zoomChart);

                if (!string.IsNullOrWhiteSpace(jsonForChart))
                {
                    FusionCharts.SetDataFormat("json");
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]))
                    {
                        divLineChart.InnerHtml = FusionCharts.RenderChart("http://localhost:2281/fusionchart/ZoomLine.swf", "", jsonForChart, "divLineChart", chartWidth, chartHeight, false, true);
                    }
                    else
                    {
                        divLineChart.InnerHtml = FusionCharts.RenderChart("http://" + Request.Url.Host + "/fusionchart/ZoomLine.swf?v=1.1", "", jsonForChart, "divLineChart", chartWidth, chartHeight, false, true);
                    }
                }

                Logger.Info("Bind Chart for TV Request End");
            }
            catch (Exception ex)
            {

                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void BinNewsGrid(SearchNewsRequest _searchNewsRequest, SearchEngine _SearchEngine, HttpContext currentContext)
        {

            try
            {
                SearchNewsResults _searchNewsResults = _SearchEngine.SearchNews(_searchNewsRequest);
                XmlDocument _XmlDocument = new XmlDocument();

                string searchResponse = string.Empty;
                _XmlDocument.LoadXml(_searchNewsResults.ResponseXml);

                XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                if (_XmlNodeList.Count > 0)
                {
                    XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                    foreach (XmlAttribute item in _XmlAttributeCollection)
                    {
                        if (item.Name == "status")
                        {
                            searchResponse = _XmlDocument.InnerXml;
                            searchResponse = searchResponse.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        }
                    }
                }
                else
                {
                    searchResponse = null;
                }

                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                _ISearchLogController.InsertSearchLog(currentContext, SearchType.IQPremium_NM.ToString(), _searchNewsRequest.SearchTerm, _searchNewsRequest.PageNumber, _searchNewsRequest.PageSize, 0, _searchNewsRequest.StartDate, _searchNewsRequest.EndDate, searchResponse, _SearchEngine.Url.ToString());

                if (_searchNewsResults != null && _searchNewsResults.newsResults != null)
                {
                    gvOnlineNews.DataSource = _searchNewsResults.newsResults;
                    gvOnlineNews.DataBind();
                }

                gvOnlineNews.Visible = true;
                ucOnlineNewsPager.TotalRecords = _searchNewsResults.TotalResults;
                ucOnlineNewsPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                ucOnlineNewsPager.BindDataList();

                if (_searchNewsResults != null && _searchNewsResults.newsResults != null && _searchNewsResults.newsResults.Count > 0)
                {
                    ucOnlineNewsPager.Visible = true;
                    spnNewsChartProgramFound.Visible = true;
                    lblNewsChart.Visible = true;

                    if (!_viewstateInformation.IsCompeteData)
                    {
                        gvOnlineNews.Columns[6].Visible = false;
                        gvOnlineNews.Columns[7].Visible = false;
                        if (!_SessionInformation.IsiQPremiumSentiment)
                        {
                            gvOnlineNews.Columns[9].Visible = false;
                            gvOnlineNews.Columns[0].HeaderStyle.Width = Unit.Percentage(8);
                            gvOnlineNews.Columns[1].HeaderStyle.Width = Unit.Percentage(17);
                            gvOnlineNews.Columns[2].HeaderStyle.Width = Unit.Percentage(26);
                            gvOnlineNews.Columns[3].HeaderStyle.Width = Unit.Percentage(14);
                            gvOnlineNews.Columns[4].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[5].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[8].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[10].HeaderStyle.Width = Unit.Percentage(5);
                        }
                        else
                        {
                            gvOnlineNews.Columns[0].HeaderStyle.Width = Unit.Percentage(8);
                            gvOnlineNews.Columns[1].HeaderStyle.Width = Unit.Percentage(13);
                            gvOnlineNews.Columns[2].HeaderStyle.Width = Unit.Percentage(20);
                            gvOnlineNews.Columns[3].HeaderStyle.Width = Unit.Percentage(14);
                            gvOnlineNews.Columns[4].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[5].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[8].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[9].HeaderStyle.Width = Unit.Percentage(9);
                            gvOnlineNews.Columns[10].HeaderStyle.Width = Unit.Percentage(5);
                        }
                    }
                    else
                    {

                        var distinctDisplayUrl = _searchNewsResults.newsResults.Select(a => !string.IsNullOrWhiteSpace(a.publication) ? new Uri(a.publication).Host.Replace("www.", "") : string.Empty).Distinct().ToList();

                        var displyUrlXml = new XElement("list",
                                                from string websiteurl in distinctDisplayUrl
                                                select new XElement("item", new XAttribute("url", websiteurl)));

                        IIQ_CompeteAllController _IIQ_CompeteAllController = _ControllerFactory.CreateObject<IIQ_CompeteAllController>();
                        List<IQ_CompeteAll> _ListOfIQ_CompeteAll = _IIQ_CompeteAllController.GetArtileAdShareValueByClientGuidAndXml(new Guid(_SessionInformation.ClientGUID), displyUrlXml.ToString(), Comete_MediaType.NM.ToString());

                        foreach (GridViewRow _GridViewRow in gvOnlineNews.Rows)
                        {
                            string href = !string.IsNullOrWhiteSpace((_GridViewRow.FindControl("aPublication") as HtmlAnchor).HRef) ? new Uri((_GridViewRow.FindControl("aPublication") as HtmlAnchor).HRef).Host.Replace("www.", "") : string.Empty;
                            IQ_CompeteAll _IQ_CompeteAll = _ListOfIQ_CompeteAll.Find(a => a.CompeteURL.Equals(href));



                            (_GridViewRow.FindControl("caud") as HtmlTableCell).InnerHtml = (_IQ_CompeteAll == null || (_IQ_CompeteAll.c_uniq_visitor == null || !_IQ_CompeteAll.IsUrlFound)) ? "NA" : string.Format("{0:n0}", _IQ_CompeteAll.c_uniq_visitor);
                            if ((_IQ_CompeteAll != null && (_IQ_CompeteAll.c_uniq_visitor == -1)))
                            {
                                (_GridViewRow.FindControl("caud") as HtmlTableCell).InnerHtml = "";
                            }

                            (_GridViewRow.FindControl("ccmpt") as HtmlTableCell).InnerHtml = (_IQ_CompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\"  title=\"Powered by Compete\" />" : "");

                            _GridViewRow.Cells[7].Text = (_IQ_CompeteAll == null || (_IQ_CompeteAll.IQ_AdShare_Value == null || !_IQ_CompeteAll.IsUrlFound)) ? "NA" : string.Format("{0:C}", _IQ_CompeteAll.IQ_AdShare_Value.Value);// +(_IQ_CompeteAll.IsCompeteAll ? "<img src=\"../Images/compete.png\" style=\"width:14px\" />" : "");
                            if ((_IQ_CompeteAll != null && (_IQ_CompeteAll.IQ_AdShare_Value == -1)))
                            {
                                _GridViewRow.Cells[7].Text = "";
                            }
                        }

                        if (!_SessionInformation.IsiQPremiumSentiment)
                        {
                            gvOnlineNews.Columns[9].Visible = false;
                            gvOnlineNews.Columns[0].HeaderStyle.Width = Unit.Percentage(6);
                            gvOnlineNews.Columns[1].HeaderStyle.Width = Unit.Percentage(13);
                            gvOnlineNews.Columns[2].HeaderStyle.Width = Unit.Percentage(17);
                            gvOnlineNews.Columns[3].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[4].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[5].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[6].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[7].HeaderStyle.Width = Unit.Percentage(10);
                            gvOnlineNews.Columns[8].HeaderStyle.Width = Unit.Percentage(9);
                            gvOnlineNews.Columns[10].HeaderStyle.Width = Unit.Percentage(5);
                        }
                    }
                }
                else
                {
                    ucOnlineNewsPager.Visible = false;
                    spnNewsChartProgramFound.Visible = false;
                    lblNewsChart.Visible = false;
                }

                lblNewsChart.Text = string.IsNullOrWhiteSpace(Convert.ToString(_searchNewsResults.TotalResults)) ? "0" : _searchNewsResults.TotalResults.ToString("#,#");


                if (_viewstateInformation != null && _viewstateInformation.listOnlineNewsChartZoomHistory != null && _viewstateInformation.listOnlineNewsChartZoomHistory.Count > 0)
                {
                    spnNewsChart.Visible = true;
                }
                else
                {
                    spnNewsChart.Visible = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void BindNewsChart(SearchNewsRequest _searchNewsRequest, SearchEngine _SearchEngine, HttpContext currentContext)
        {
            try
            {
                Boolean isError = false;
                String newsChartData = _SearchEngine.SearchNewsChart(_searchNewsRequest, out isError);
                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();


                string searchResponse = string.Empty;
                if (isError)
                {
                    XmlDocument _XmlDocument = new XmlDocument();

                    _XmlDocument.LoadXml(newsChartData);

                    XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                    if (_XmlNodeList.Count > 0)
                    {
                        XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                        foreach (XmlAttribute item in _XmlAttributeCollection)
                        {
                            if (item.Name == "status")
                            {
                                searchResponse = _XmlDocument.InnerXml;
                                searchResponse = searchResponse.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                            }
                        }
                    }
                    else
                    {
                        searchResponse = null;
                    }
                }
                else
                {
                    jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(newsChartData);
                    searchResponse = null;
                }

                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                _ISearchLogController.InsertSearchLog(currentContext, SearchType.IQPremium_NM.ToString(), _searchNewsRequest.SearchTerm, _searchNewsRequest.PageNumber, _searchNewsRequest.PageSize, 0, _searchNewsRequest.StartDate, _searchNewsRequest.EndDate, searchResponse, _SearchEngine.Url.ToString());


                _viewstateInformation = GetViewstateInformation();
                ZoomChart zoomChartNews = new ZoomChart();

                zoomChartNews.Dataset = new List<Dataset>();

                zoomChartNews.Categories = new List<Category>();
                Category category = new Category();
                category.Category1 = new List<Category2>();
                zoomChartNews.Categories.Add(category);

                if (!isError && _viewstateInformation != null && _viewstateInformation.listPublicationCategory != null)
                {
                    List<NB_PublicationCategory> listNBPublicationCategory = _viewstateInformation.listPublicationCategory;

                    foreach (NB_PublicationCategory pCategory in _viewstateInformation.listPublicationCategory)
                    {
                        if (_searchNewsRequest.PublicationCategory != null
                            && (_searchNewsRequest.PublicationCategory.Count <= 0
                            || _searchNewsRequest.PublicationCategory.Contains(Convert.ToInt32(pCategory.ID))))
                        {
                            string totalcount = Convert.ToString(jsonData["facet_counts"]["facet_ranges"][pCategory.ID.ToString()]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);

                            string[] facetData = totalcount.Split(',');
                            category.Category1 = new List<Category2>(10);



                            string catName = _viewstateInformation.listPublicationCategory.Where(s => s.ID.Equals(pCategory.ID)).First().Name;

                            Dataset datasetValue = new Dataset();

                            datasetValue.Seriesname = catName;
                            datasetValue.Data = new List<Datum>();

                            zoomChartNews.Dataset.Add(datasetValue);

                            for (int i = 0; i < facetData.Length; i = i + 2)
                            {

                                datasetValue.Data.Add(new Datum() { Value = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty)) });

                                category.Category1.Add(new Category2() { Label = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt") });
                            }
                        }
                    }
                }

                string chartTitle = string.Empty;
                zoomChartNews.Chart = new Chart
                {
                    numVisibleLabels = ConfigurationManager.AppSettings[CommonConstants.ConfigZoomChartNoOfLables],
                    caption = string.Empty,
                    subcaption = "",
                    exportEnabled = "1",
                    exportAtClient = "1",
                    exportHandler = "fcBatchExporterNews",
                    allowpinmode = "0",
                    drawAnchors = "0",
                    bgColor = "FFFFFF",
                    showBorder = "0",
                    yaxisname = "Article",
                    chartRightMargin = ConfigurationManager.AppSettings[CommonConstants.ConfigChartRightMargin],
                    formatnumberscale = "0"

                };


                string jsonForChart = CommonFunctions.SearializeJson(zoomChartNews);

                if (!string.IsNullOrWhiteSpace(jsonForChart))
                {
                    FusionCharts.SetDataFormat("json");
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]))
                    {
                        divNewsChart.InnerHtml = FusionCharts.RenderChart("http://localhost:2281/fusionchart/ZoomLine.swf", "", jsonForChart, "divNewsChart", chartWidth, chartHeight, false, false);

                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "loadZoomChart", "LoadZoomChart('" + jsonForChart + "');", true);
                    }
                    else
                    {
                        divNewsChart.InnerHtml = FusionCharts.RenderChart("http://" + Request.Url.Host + "/fusionchart/ZoomLine.swf?v=1.1", "", jsonForChart, "divNewsChart", chartWidth, chartHeight, false, false);
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void BindRadioGrid()
        {
            try
            {
                _viewstateInformation = GetViewstateInformation();
                SearchRadioRequest searchRadioRequest = (SearchRadioRequest)_viewstateInformation.searchRequestRadio;


                if (!String.IsNullOrEmpty(Convert.ToString(searchRadioRequest.IQStationIDXML)))
                {
                    IRL_GUIDSController _IRL_GUIDSController = _ControllerFactory.CreateObject<IRL_GUIDSController>();
                    Int64 _TotalRecordsCount = 0;

                    List<RadioStation> _ListOfRadioStation = _IRL_GUIDSController.GetAllRL_GUIDSByRadioStationsByXML(searchRadioRequest.IQStationIDXML,
                                                                                                            (int)ucRadioPager.CurrentPage,
                                                                                                            searchRadioRequest.PageSize,
                                                                                                            _viewstateInformation.SortExpressionRadio,
                                                                                                            _viewstateInformation.IsRadioSortDirecitonAsc, searchRadioRequest.FromDate,
                                                                                                            searchRadioRequest.ToDate,
                                                                                                            out _TotalRecordsCount);

                    /*if (_viewstateInformation.ListOfRadioStation == null)
                    {
                        _viewstateInformation.ListOfRadioStation = _ListOfRadioStation;
                    }
                    else
                    {
                        _viewstateInformation.ListOfRadioStation.AddRange(_ListOfRadioStation);
                    }*/

                    grvRadioStations.DataSource = _ListOfRadioStation;
                    grvRadioStations.DataBind();
                    grvRadioStations.Visible = true;
                    SortClipDirection(_viewstateInformation.SortExpressionRadio, _viewstateInformation.IsRadioSortDirecitonAsc, grvRadioStations);
                    if (_ListOfRadioStation.Count > 0)
                    {
                        lblRadioChart.Text = Convert.ToString(_TotalRecordsCount);
                        ucRadioPager.TotalRecords = _TotalRecordsCount;
                        ucRadioPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                        ucRadioPager.BindDataList();
                        ucRadioPager.Visible = true;
                        spnRadioFound.Visible = true;
                        lblRadioChart.Visible = true;

                    }
                    else
                    {
                        ucRadioPager.Visible = false;
                        spnRadioFound.Visible = false;
                        lblRadioChart.Visible = false;
                    }

                    _viewstateInformation.TotalRadioStationsCountDB = _TotalRecordsCount;

                    lblRadioChart.Text = _TotalRecordsCount.ToString();

                    SetViewstateInformation(_viewstateInformation);
                }
                else
                {
                    grvRadioStations.DataSource = null;
                    grvRadioStations.DataBind();


                }
                upRadio.Update();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected int GetGridToBind()
        {
            try
            {
                if (chkTV.Checked)
                {
                    //tabTV.Visible = true;
                    //divTVResult.Visible = true;
                    //tabChartTV.Visible = true;
                    //divTVChart.Visible = true;

                    string _Script = "$('#tabTV').css('display','block');"
                        + "$('#divTVResult').css('display','block');"
                        + "$('#tabChartTV').css('display','block');"
                        + "$('#divTVChart').css('display','block');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DisplayTVTab", _Script, true);

                }
                else
                {
                    //tabTV.Visible = false;
                    //divTVResult.Visible = false;
                    //tabChartTV.Visible = false;
                    //divTVChart.Visible = false;

                    string _Script = "$('#tabTV').css('display','none');"
                        + "$('#divTVResult').css('display','none');"
                        + "$('#tabChartTV').css('display','none');"
                        + "$('#divTVChart').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTVTab", _Script, true);
                }


                //Display Radio Tab
                if (_SessionInformation.IsiQPremiumRadio)
                {
                    if (chkRadio.Checked)
                    {
                        //tabOnlineNews.Visible = true;
                        //divOnlineNewsResult.Visible = true;
                        //tabChartOL.Visible = true;
                        //divOLChart.Visible = true;


                        string _Script = "$('#tabRadio').css('display','block');"
                            + "$('#divRadioResult').css('display','block');"
                            + "$('#tabChartRadio').css('display','block');"
                            + "$('#divRadioChart').css('display','block');";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DisplayRadioTab", _Script, true);

                    }
                    else
                    {
                        string _Script = "$('#tabRadio').css('display','none');"
                            + "$('#divRadioResult').css('display','none');"
                            + "$('#tabChartRadio').css('display','none');"
                            + "$('#divRadioChart').css('display','none');";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideRadioTab", _Script, true);


                    }
                }


                if (_SessionInformation.IsiQPremiumNM)
                {
                    if (chkNews.Checked)
                    {
                        //tabOnlineNews.Visible = true;
                        //divOnlineNewsResult.Visible = true;
                        //tabChartOL.Visible = true;
                        //divOLChart.Visible = true;


                        string _Script = "$('#tabOnlineNews').css('display','block');"
                            + "$('#divOnlineNewsResult').css('display','block');"
                            + "$('#tabChartOL').css('display','block');"
                            + "$('#divOLChart').css('display','block');";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DisplayNewsTab", _Script, true);

                    }
                    else
                    {
                        //tabOnlineNews.Visible = false;
                        //divOnlineNewsResult.Visible = false;
                        //tabChartOL.Visible = false;
                        //divOLChart.Visible = false;


                        string _Script = "$('#tabOnlineNews').css('display','none');"
                            + "$('#divOnlineNewsResult').css('display','none');"
                            + "$('#tabChartOL').css('display','none');"
                            + "$('#divOLChart').css('display','none');";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNewsTab", _Script, true);


                    }
                }

                if (_SessionInformation.IsiQPremiumSM)
                {
                    if (chkSocialMedia.Checked)
                    {
                        string _Script = "$('#tabSocialMedia').css('display','block');"
                            + "$('#divSocialMediaResult').css('display','block');"
                            + "$('#tabChartSM').css('display','block');"
                            + "$('#divSMChart').css('display','block');";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DisplaySocialMediaTab", _Script, true);

                    }
                    else
                    {
                        string _Script = "$('#tabSocialMedia').css('display','none');"
                             + "$('#divSocialMediaResult').css('display','none');"
                             + "$('#tabChartSM').css('display','none');"
                             + "$('#divSMChart').css('display','none');";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSocialMediaTab", _Script, true);
                    }
                }

                if (_SessionInformation.IsiQPremiumTwitter)
                {
                    if (chkTwitter.Checked)
                    {
                        string _Script = "$('#tabTwitter').css('display','block');"
                            + "$('#divTwitterResult').css('display','block');"
                            + "$('#tabChartTwitter').css('display','block');"
                            + "$('#divTwitterChart').css('display','block');";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "DisplayTwitterTab", _Script, true);
                    }
                    else
                    {
                        string _Script = "$('#tabTwitter').css('display','none');"
                             + "$('#divTwitterResult').css('display','none');"
                             + "$('#tabChartTwitter').css('display','none');"
                             + "$('#divTwitterChart').css('display','none');";

                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTwitterTab", _Script, true);
                    }
                }


                //upChartTab.Update();
                //upTab.Update();

                int gridNumber = CheckifCheckBoxchecked();
                if (!String.IsNullOrWhiteSpace(hfcurrentTab.Value) && gridNumber > 0)
                {
                    if (gridNumber == 1) //TV
                    {
                        return 0;
                    }
                    else if (gridNumber == 2)  // Radio
                    {
                        return 1;
                    }
                    else if (gridNumber == 3)  // Online News
                    {
                        return 2;
                    }
                    else if (gridNumber == 4) // Social Media 
                    {
                        return 3;
                    }
                    else if (gridNumber == 5) // Twitter
                    {
                        return 4;
                    }
                }
                else
                {
                    if (chkTV.Checked)
                    {
                        return 0;
                    }
                    if (chkRadio.Checked)
                        return 1;

                    if (chkNews.Checked)
                    {
                        return 2;
                    }
                    if (chkSocialMedia.Checked)
                    {
                        return 3;
                    }

                    if (chkTwitter.Checked)
                    {
                        return 4;
                    }


                    /*if (chkSocialMedia.Checked)
                    {
                        return 2;
                    }
                    if (chkTwitter.Checked)
                    {
                        return 3;
                    }
                    if (chkRadio.Checked)
                    {
                        return 4;
                    }*/
                }

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
            return 5;
        }

        protected int CheckifCheckBoxchecked()
        {
            //here hfcurrentTab.Value
            // 0 for TV
            // 1 for Radio
            // 2 for Online News
            // 3 for Social Media
            // 4 for Twitter
            int returnValue = 0;
            switch (hfcurrentTab.Value)
            {
                case "0":
                    if (chkTV.Checked)
                    {
                        returnValue = 1;
                    }
                    else
                    {
                        returnValue = 0;
                    }
                    break;

                case "1":
                    if (chkRadio.Checked && _SessionInformation.IsiQPremiumRadio)
                    {
                        returnValue = 2;
                    }
                    else
                    {
                        returnValue = 0;
                    }
                    break;

                case "2":
                    if (chkNews.Checked && _SessionInformation.IsiQPremiumNM)
                    {
                        returnValue = 3;
                    }
                    else
                    {
                        returnValue = 0;
                    }
                    break;

                case "3":
                    if (chkSocialMedia.Checked && _SessionInformation.IsiQPremiumSM)
                    {
                        returnValue = 4;
                    }
                    else
                    {
                        returnValue = 0;
                    }
                    break;

                case "4":
                    if (chkTwitter.Checked && _SessionInformation.IsiQPremiumTwitter)
                    {
                        returnValue = 5;
                    }
                    else
                    {
                        returnValue = 0;
                    }
                    break;




                default:
                    returnValue = 0;
                    break;

            }

            return returnValue;
        }

        public void SaveRequestObject()
        {
            try
            {
                Logger.Info("Save Request Object To ViewState Starts");
                SearchRequest _SearchRequest = null;
                SearchNewsRequest _searchNewsRequest = null;
                SearchSMRequest searchSMRequest = null;
                SearchTwitterRequest searchTwitterRequest = null;
                SearchRadioRequest searchRadioRequest = null;

                if (chkTV.Checked)
                {
                    #region Save TV Request
                    //_viewstateInformation.IsAllStationAllowed = true;
                    //_viewstateInformation.IsAllDmaAllowed = true;
                    //_viewstateInformation.IsAllClassAllowed = true;

                    _SearchRequest = new SearchRequest();

                    int _PMGMaxHighlights = 20;

                    if (ConfigurationManager.AppSettings["PMGMaxHighlights"] != null)
                    {
                        int.TryParse(ConfigurationManager.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                    }

                    _SearchRequest.Terms = txtSearch.Text.Trim();
                    if (ucCustomPager.CurrentPage != null)
                        _SearchRequest.PageNumber = ucCustomPager.CurrentPage.Value;
                    else
                        _SearchRequest.PageNumber = 0;
                    _SearchRequest.PageSize = 10;
                    _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                    _SearchRequest.SortFields = "datetime-";

                    if (!string.IsNullOrWhiteSpace(txProgramTitle.Text))
                    {
                        _SearchRequest.Title120 = "\"" + txProgramTitle.Text + "\"";
                    }

                    _SearchRequest.Appearing = txtAppearing.Text;

                    _SearchRequest.TimeZone = ddlTimeZone.SelectedItem.Text;

                    if (_SessionInformation.IsiQPremiumSentiment)
                    {
                        _SearchRequest.IsSentiment = true;
                        _SearchRequest.LowThreshold = _viewstateInformation.TVLowThreshold;
                        _SearchRequest.HighThreshold = _viewstateInformation.TVHighThreshold;
                        _SearchRequest.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                    }
                    else
                        _SearchRequest.IsSentiment = false;

                    List<string> _ListOfMarket = new List<string>();
                    List<string> _ListOfStations = new List<string>();
                    List<string> _ListOfIQClassNum = new List<string>();

                    #region market Filter Region

                    if (ddlMarket.SelectedIndex == 0)
                    {
                        #region Region Filter
                        if (!chlkRegionSelectAll.Checked || _viewstateInformation.IsAllDmaAllowed == false)
                        {

                            foreach (RepeaterItem rptitm in rptregion.Items)
                            {
                                Repeater rptDmaList = (Repeater)rptitm.FindControl("rptDma");
                                foreach (RepeaterItem repeaterItem in rptDmaList.Items)
                                {
                                    HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                    if (chkboxDma.Checked)
                                    {
                                        string[] DmaNameNum = chkboxDma.Value.Split('#');
                                        if (DmaNameNum.Length > 0)
                                        {
                                            _ListOfMarket.Add(DmaNameNum[0]);
                                        }
                                    }
                                    //chkboxDma.Attributes.Add("onclick", "chkMainRegion(this," + ((CheckBoxList)rptregion.Items[i].FindControl("cblregion")).ClientID + "," + ((CheckBox)rptregion.Items[i].FindControl("chkRegion")).ClientID + "," + chlkRegionSelectAll.ClientID + ",divRegionFilter);");

                                }


                            }
                            if (chkRegionNational.Visible && chkRegionNational.Checked)
                            {
                                _ListOfMarket.Add(chkRegionNational.Value);
                            }
                        }

                        #region Set Filter Status
                        if (chlkRegionSelectAll.Checked || _ListOfMarket.Count() == 0)
                        {
                            //divMarketFilterStatus.InnerText = "OFF";
                            //divMarketFilterStatus.Style.Add("color", "black");
                            //imgMarketFilter.Src = "~/images/filter.png";
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('" + divMarketFilterStatus.ClientID + "','" + imgMarketFilter.ClientID + "','OFF')", true);
                        }
                        else
                        {
                            //divMarketFilterStatus.InnerText = "ON";
                            //divMarketFilterStatus.Style.Add("color", "red");
                            //imgMarketFilter.Src = "~/images/filter-Selected.png";
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('" + divMarketFilterStatus.ClientID + "','" + imgMarketFilter.ClientID + "','ON')", true);
                        }
                        #endregion


                        #endregion
                    }
                    else
                    {
                        #region Rank Filter
                        List<string> lstMarket = new List<string>();
                        if (!chkRankFIlterSelectAll.Checked || _viewstateInformation.IsAllDmaAllowed == false)
                        {
                            foreach (RepeaterItem repeaterItem in rptTop210.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop150.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop100.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop80.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop60.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop50.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop40.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop30.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop20.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            foreach (RepeaterItem repeaterItem in rptTop10.Items)
                            {
                                HtmlInputCheckBox chkboxDma = (HtmlInputCheckBox)repeaterItem.FindControl("chkDma");
                                if (chkboxDma.Checked)
                                {
                                    string[] DmaNameNum = chkboxDma.Value.Split('#');
                                    if (DmaNameNum.Length > 0)
                                    {
                                        lstMarket.Add(DmaNameNum[0]);
                                    }
                                }
                            }

                            if (chkNational.Visible && chkNational.Checked)
                            {
                                lstMarket.Add(chkNational.Value);
                            }
                        }

                        _ListOfMarket = lstMarket.Select(x => x).Distinct().ToList();


                        #region Set Filter Status
                        if (chkRankFIlterSelectAll.Checked || _ListOfMarket.Count() == 0)
                        {
                            //divMarketFilterStatus.InnerText = "OFF";
                            //divMarketFilterStatus.Style.Add("color", "black");
                            //imgMarketFilter.Src = "~/images/filter.png";
                            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('" + divMarketFilterStatus.ClientID + "','" + imgMarketFilter.ClientID + "','OFF')", true);
                        }
                        else
                        {
                            //divMarketFilterStatus.InnerText = "ON";
                            //divMarketFilterStatus.Style.Add("color", "red");
                            //imgMarketFilter.Src = "~/images/filter-Selected.png";
                            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVMarket", "SetFilterStatus('" + divMarketFilterStatus.ClientID + "','" + imgMarketFilter.ClientID + "','ON')", true);
                        }
                        #endregion
                        #endregion
                    }
                    #endregion

                    #region Affiliate Region

                    Dictionary<Dictionary<String, String>, List<String>> lstaffilforFacet = new Dictionary<Dictionary<String, String>, List<String>>();
                    foreach (RepeaterItem rptitm in rptTVStationSubMaster.Items)
                    {

                        List<String> childStationids = new List<string>();
                        Dictionary<String, String> dictValue = new Dictionary<string, string>();
                        Repeater rptTVStationChild = (Repeater)rptitm.FindControl("rptTVStationChild");
                        foreach (RepeaterItem repeaterItem in rptTVStationChild.Items)
                        {

                            HtmlInputCheckBox chkTVStation = (HtmlInputCheckBox)repeaterItem.FindControl("chkTVStation");
                            if (chkAffilAll.Checked || chkTVStation.Checked)
                            {
                                childStationids.Add(chkTVStation.Value);
                                _ListOfStations.Add(chkTVStation.Value);
                            }

                        }

                        dictValue.Add(((HiddenField)rptitm.FindControl("hfAffilNum")).Value, ((CheckBox)rptitm.FindControl("chkStationSubMaster")).Text);
                        lstaffilforFacet.Add(dictValue, childStationids);
                    }

                    _SearchRequest.AffilForFacet = lstaffilforFacet;

                    #region Station Filter Status
                    if (chkAffilAll.Checked || _ListOfStations.Count() == 0)
                    {
                        //divStationFilterStatus.InnerText = "OFF";
                        //divStationFilterStatus.Style.Add("color", "black");
                        //imgStatusFilter.Src = "~/images/filter.png";
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('" + divStationFilterStatus.ClientID + "','" + imgStatusFilter.ClientID + "','OFF')", true);
                    }
                    else
                    {
                        //divStationFilterStatus.InnerText = "ON";
                        //divStationFilterStatus.Style.Add("color", "red");
                        //imgStatusFilter.Src = "~/images/filter-Selected.png";
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVStation", "SetFilterStatus('" + divStationFilterStatus.ClientID + "','" + imgStatusFilter.ClientID + "','ON')", true);
                    }


                    #endregion
                    #endregion

                    #region Category
                    if (!chkCategoryAll.Checked || _viewstateInformation.IsAllClassAllowed == false)
                    {
                        foreach (RepeaterItem rptItem in rptCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptItem.FindControl("chkClass");
                            if (chk.Checked)
                            {
                                _ListOfIQClassNum.Add(chk.Value);
                            }
                        }
                    }

                    #region Category Filter Status
                    if (chkCategoryAll.Checked || _ListOfIQClassNum.Count() == 0)
                    {
                        //divCategoryFilterStatus.InnerText = "OFF";
                        //divCategoryFilterStatus.Style.Add("color", "black");
                        //imgCategoryFilter.Src = "~/images/filter.png";
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCat", "SetFilterStatus('" + divCategoryFilterStatus.ClientID + "','" + imgCategoryFilter.ClientID + "','OFF')", true);

                    }
                    else
                    {
                        //divCategoryFilterStatus.InnerText = "ON";
                        //divCategoryFilterStatus.Style.Add("color", "red");
                        //imgCategoryFilter.Src = "~/images/filter-Selected.png";
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetFilterTVCat", "SetFilterStatus('" + divCategoryFilterStatus.ClientID + "','" + imgCategoryFilter.ClientID + "','ON')", true);
                    }
                    #endregion

                    #endregion

                    _SearchRequest.Stations = _ListOfStations;
                    _SearchRequest.IQDmaName = _ListOfMarket;
                    _SearchRequest.IQClassNum = _ListOfIQClassNum;



                    DateTime _FromDate = new DateTime();
                    DateTime _ToDate = new DateTime();

                    /*if (_viewstateInformation != null && _viewstateInformation.listchartZoomHistory != null && _viewstateInformation.listchartZoomHistory.Count > 0)
                    {
                        _SearchRequest.StartDate = Convert.ToDateTime(_viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].StartDate);
                        _SearchRequest.EndDate = Convert.ToDateTime(_viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].EndDate);
                    }
                    else
                    {*/

                    int _FromTime = (Convert.ToInt32(rdAmPmFromDate.SelectedValue) - 12) + Convert.ToInt32(ddlStartTime.SelectedValue);
                    int _ToTime = (Convert.ToInt32(rdAMPMToDate.SelectedValue) - 12) + Convert.ToInt32(ddlEndTime.SelectedValue);

                    if (_FromTime == 24)
                    {
                        _FromTime = 12;
                    }

                    if (_ToTime == 24)
                    {
                        _ToTime = 12;
                    }

                    if (rbDuration1.Checked)
                    {
                        _FromTime = 0;
                        _ToTime = DateTime.Now.Hour;

                        if (ddlDuration.SelectedValue == "all")
                        {
                            _ToDate = DateTime.Now;
                            _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                        }
                        else
                        {
                            string[] _SelectedDuration = ddlDuration.SelectedValue.Split(',');
                            if (_SelectedDuration.Length > 1)
                            {
                                _ToDate = DateTime.Now;
                                int Time = -Math.Abs(Convert.ToInt32(_SelectedDuration[1]));
                                switch (_SelectedDuration[0])
                                {
                                    case "hour":
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                    case "day":
                                        _FromDate = DateTime.Now.AddDays(Time);
                                        break;
                                    case "month":
                                        _FromDate = DateTime.Now.AddMonths(Time);
                                        break;
                                    case "year":
                                        _FromDate = DateTime.Now.AddYears(Time);
                                        break;
                                    default:
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                }
                            }
                        }


                        _SearchRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                        _SearchRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);
                    }
                    else
                    {


                        _FromDate = Convert.ToDateTime(txtStartDate.Text);
                        _ToDate = Convert.ToDateTime(txtEndDate.Text);
                        _SearchRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        _SearchRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                    }




                    /* }*/
                    #region Set Facet Params

                    _SearchRequest.Facet = true;
                    _SearchRequest.FacetRangeOther = "all";
                    _SearchRequest.FacetRangeStarts = _SearchRequest.StartDate;
                    _SearchRequest.FacetRangeEnds = _SearchRequest.EndDate;
                    DateTime startdt = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                    DateTime enddt = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);


                    //_SearchRequest.FacetRangeGap = RangeGap.HOUR;
                    TimeSpan dateDiff = (enddt - startdt);

                    if (dateDiff.Days < 7)
                    {
                        _SearchRequest.FacetRangeGap = RangeGap.HOUR;
                    }
                    else
                    {
                        _SearchRequest.FacetRangeGap = RangeGap.DAY;
                    }

                    _SearchRequest.FacetRangeGapDuration = 1;
                    _SearchRequest.FacetRange = "RL_Station_DateTime_DT";

                    _SearchRequest.wt = ReponseType.json;

                    #endregion Set Facet Params

                    #endregion
                }


                if (chkNews.Checked && _SessionInformation.IsiQPremiumNM)
                {
                    #region Save Online News Request

                    _searchNewsRequest = new SearchNewsRequest();

                    _searchNewsRequest.SortFields = "date-";


                    _searchNewsRequest.PageSize = 10;
                    if (ucOnlineNewsPager.CurrentPage != null)
                        _searchNewsRequest.PageNumber = ucOnlineNewsPager.CurrentPage.Value;
                    else
                        _searchNewsRequest.PageNumber = 0;
                    if (!string.IsNullOrWhiteSpace(txtNewsPublication.Text))
                    {
                        _searchNewsRequest.Source = "\"" + txtNewsPublication.Text + "\"";
                    }

                    if (!string.IsNullOrWhiteSpace(txtSearch.Text.Trim()))
                    {
                        _searchNewsRequest.SearchTerm = txtSearch.Text.Trim();
                    }

                    if (_SessionInformation.IsiQPremiumSentiment)
                    {
                        _searchNewsRequest.IsSentiment = true;
                        _searchNewsRequest.LowThreshold = _viewstateInformation.NMLowThreshold;
                        _searchNewsRequest.HighThreshold = _viewstateInformation.NMHighThreshold;
                        _searchNewsRequest.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                    }

                    _searchNewsRequest.IsReturnHighlight = true;

                    DateTime _FromDate = new DateTime();
                    DateTime _ToDate = new DateTime();
                    /*if (_viewstateInformation != null && _viewstateInformation.listOnlineNewsChartZoomHistory != null && _viewstateInformation.listOnlineNewsChartZoomHistory.Count > 0)
                    {
                        _searchNewsRequest.StartDate = Convert.ToDateTime(_viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].StartDate);
                        _searchNewsRequest.EndDate = Convert.ToDateTime(_viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].EndDate);
                    }
                    else
                    {*/

                    int _FromTime = (Convert.ToInt32(rbNewsStart.SelectedValue) - 12) + Convert.ToInt32(ddlNewsStartHour.SelectedValue);
                    int _ToTime = (Convert.ToInt32(rbNewsEnd.SelectedValue) - 12) + Convert.ToInt32(ddlNewsEndHour.SelectedValue);

                    if (_FromTime == 24)
                    {
                        _FromTime = 12;
                    }

                    if (_ToTime == 24)
                    {
                        _ToTime = 12;
                    }

                    if (rbNewsDuration.Checked)
                    {
                        _FromTime = 0;
                        _ToTime = DateTime.Now.Hour;

                        if (ddlNewsDuration.SelectedValue == "all")
                        {
                            _ToDate = DateTime.Now;
                            _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                        }
                        else
                        {
                            string[] _SelectedNewsDuration = ddlNewsDuration.SelectedValue.Split(',');
                            if (_SelectedNewsDuration.Length > 1)
                            {
                                _ToDate = DateTime.Now;
                                int Time = -Math.Abs(Convert.ToInt32(_SelectedNewsDuration[1]));
                                switch (_SelectedNewsDuration[0])
                                {
                                    case "hour":
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                    case "day":
                                        _FromDate = DateTime.Now.AddDays(Time);
                                        break;
                                    case "month":
                                        _FromDate = DateTime.Now.AddMonths(Time);
                                        break;
                                    case "year":
                                        _FromDate = DateTime.Now.AddYears(Time);
                                        break;
                                    default:
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                }
                            }
                        }

                        _searchNewsRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                        _searchNewsRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);
                    }
                    else
                    {
                        _FromDate = Convert.ToDateTime(txtNewsStartDate.Text);
                        _ToDate = Convert.ToDateTime(txtNewsEndDate.Text);

                        _searchNewsRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        _searchNewsRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                    }




                    /*}*/

                    List<String> newsCategoryList = new List<String>();
                    if (!chkNewsCategorySelectAll.Checked)
                    {


                        foreach (RepeaterItem rptitem in rptNewsCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsCategory");
                            if (chk != null && chk.Checked)
                            {
                                newsCategoryList.Add(chk.Value);
                            }
                        }
                        _searchNewsRequest.NewsCategory = newsCategoryList;


                    }

                    //#region Set News Category Filter Status
                    //if (chkNewsCategorySelectAll.Checked || newsCategoryList.Count() == 0)
                    //{
                    //    divNewsCategoryStatus.InnerText = "OFF";
                    //    divNewsCategoryStatus.Style.Add("color", "black");
                    //    imgShowNewsCategoryFilter.Src = "~/images/filter.png";
                    //}
                    //else
                    //{
                    //    divNewsCategoryStatus.InnerText = "ON";
                    //    divNewsCategoryStatus.Style.Add("color", "red");
                    //    imgShowNewsCategoryFilter.Src = "~/images/filter-Selected.png";
                    //}
                    //#endregion

                    List<String> GenreList = new List<string>();
                    if (!chkNewsGenreSelectAll.Checked)
                    {

                        foreach (RepeaterItem rptitem in rptNewsGenre.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsGenre");
                            if (chk != null && chk.Checked)
                            {
                                GenreList.Add(chk.Value);
                            }
                        }

                        _searchNewsRequest.Genre = GenreList;
                    }

                    //#region Set News Genre Filter Status
                    //if (chkNewsGenreSelectAll.Checked || GenreList.Count() == 0)
                    //{
                    //    divNewsGenreFilterStatus.InnerText = "OFF";
                    //    divNewsGenreFilterStatus.Style.Add("color", "black");
                    //    imgNewsGenreFilter.Src = "~/images/filter.png";
                    //}
                    //else
                    //{
                    //    divNewsGenreFilterStatus.InnerText = "ON";
                    //    divNewsGenreFilterStatus.Style.Add("color", "red");
                    //    imgNewsGenreFilter.Src = "~/images/filter-Selected.png";
                    //}
                    //#endregion

                    List<String> newsRegionList = new List<string>();

                    if (!chkNewsRegionAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptNewsRegion.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsregion");
                            if (chk != null && chk.Checked)
                            {
                                newsRegionList.Add(chk.Value);
                            }
                        }

                        _searchNewsRequest.NewsRegion = newsRegionList;
                    }

                    //#region Set News Region Filter Status
                    //if (chkNewsRegionAll.Checked || newsRegionList.Count() == 0)
                    //{
                    //    divNewsRegionFilterStatus.InnerText = "OFF";
                    //    divNewsRegionFilterStatus.Style.Add("color", "black");
                    //    imgNewsRegionStatusFilter.Src = "~/images/filter.png";
                    //}
                    //else
                    //{
                    //    divNewsRegionFilterStatus.InnerText = "ON";
                    //    divNewsRegionFilterStatus.Style.Add("color", "red");
                    //    imgNewsRegionStatusFilter.Src = "~/images/filter-Selected.png";
                    //}
                    //#endregion

                    List<int> publicationCategoryList = new List<int>();
                    List<string> _ListOfPubCat = new List<string>();

                    int publicationCategoryNumber = 0;
                    foreach (RepeaterItem rptitem in rptNewsPublicationCategory.Items)
                    {
                        HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsPublicationCategory");
                        if (!chkNewsPublicationCategory.Checked && chk != null && chk.Checked)
                        {
                            publicationCategoryNumber = Convert.ToInt32(chk.Value);
                            publicationCategoryList.Add(publicationCategoryNumber);
                        }
                        _ListOfPubCat.Add(chk.Value);
                    }
                    _searchNewsRequest.PublicationCategory = publicationCategoryList;

                    //List<NB_PublicationCategory> publicationCategoryListFacetRange = new List<NB_PublicationCategory>();
                    //publicationCategoryListFacetRange = _viewstateInformation.listPublicationCategory;
                    /*int publicationCategoryNumberFacetRange = 0;

                    foreach (RepeaterItem rptitem in rptNewsPublicationCategory.Items)
                    {

                        HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkNewsPublicationCategory");
                        if (chk != null)
                        {
                            publicationCategoryNumberFacetRange = Convert.ToInt32(chk.Value); //publicationCategoryNumberFacetRange + 1;
                            publicationCategoryListFacetRange.Add(publicationCategoryNumberFacetRange.ToString());
                        }
                    }*/
                    _searchNewsRequest.lstfacetRange = _ListOfPubCat;

                    //_viewstateInformation = GetViewstateInformation();
                    //_viewstateInformation.listPublicationCategory = publicationCategoryListFacetRange;
                    //SetViewstateInformation(_viewstateInformation);


                    //#region Set News Publication Filter Status
                    //if (chkNewsPublicationCategory.Checked || publicationCategoryList.Count() == 0)
                    //{
                    //    divShowNewsPublicationCategoryStatus.InnerText = "OFF";
                    //    divShowNewsPublicationCategoryStatus.Style.Add("color", "black");
                    //    imgShowNewsPublicationCategoryFilter.Src = "~/images/filter.png";
                    //}
                    //else
                    //{
                    //    divShowNewsPublicationCategoryStatus.InnerText = "ON";
                    //    divShowNewsPublicationCategoryStatus.Style.Add("color", "red");
                    //    imgShowNewsPublicationCategoryFilter.Src = "~/images/filter-Selected.png";
                    //}
                    //#endregion

                    _searchNewsRequest.PublicationCategory = publicationCategoryList;

                    #region Set Facet Params

                    _searchNewsRequest.Facet = true;
                    // _searchNewsRequest.FacetRangeOther = "all";
                    _searchNewsRequest.FacetRangeStarts = _searchNewsRequest.StartDate;
                    _searchNewsRequest.FacetRangeEnds = _searchNewsRequest.EndDate;
                    DateTime startdt = (DateTime)_searchNewsRequest.StartDate;// new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                    DateTime enddt = (DateTime)_searchNewsRequest.EndDate;// new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                    //TimeSpan? t = startdt.Subtract(enddt);

                    //  _searchNewsRequest.FacetRangeGap = RangeGap.HOUR;
                    TimeSpan dateDiff = (enddt - startdt);
                    if (dateDiff.Days < 7)
                    {
                        _searchNewsRequest.FacetRangeGap = RangeGap.HOUR;
                    }
                    else
                    {
                        _searchNewsRequest.FacetRangeGap = RangeGap.DAY;
                    }

                    _searchNewsRequest.FacetRangeGapDuration = 1;
                    // _searchNewsRequest.FacetRange = "harvest_time_DT";

                    _searchNewsRequest.wt = ReponseType.json;

                    #endregion Set Facet Params
                    #endregion
                }

                if (chkSocialMedia.Checked && _SessionInformation.IsiQPremiumSM)
                {
                    #region Save Social Media Request

                    searchSMRequest = new SearchSMRequest();
                    searchSMRequest.SortFields = "date-";


                    searchSMRequest.PageSize = 10;
                    if (ucSMPager.CurrentPage != null)
                        searchSMRequest.PageNumber = ucSMPager.CurrentPage.Value;
                    else
                        searchSMRequest.PageNumber = 0;


                    if (!string.IsNullOrWhiteSpace(txtSearch.Text.Trim()))
                    {
                        searchSMRequest.SearchTerm = txtSearch.Text.Trim();
                    }

                    if (!string.IsNullOrWhiteSpace(txtSMSource.Text.Trim()))
                    {
                        searchSMRequest.SocialMediaSource = txtSMSource.Text.Trim();
                    }

                    if (!string.IsNullOrWhiteSpace(txtSMAuthor.Text.Trim()))
                    {
                        searchSMRequest.Author = txtSMAuthor.Text.Trim();
                    }

                    if (!string.IsNullOrWhiteSpace(txtSMTitle.Text.Trim()))
                    {
                        searchSMRequest.Title = txtSMTitle.Text.Trim();
                    }
                    if (_SessionInformation.IsiQPremiumSentiment)
                    {
                        searchSMRequest.IsSentiment = true;
                        searchSMRequest.LowThreshold = _viewstateInformation.SMLowThreshold;
                        searchSMRequest.HighThreshold = _viewstateInformation.SMHighThreshold;
                        searchSMRequest.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                    }

                    searchSMRequest.IsReturnHighlight = true;

                    DateTime _FromDate = new DateTime();
                    DateTime _ToDate = new DateTime();

                    int _FromTime = (Convert.ToInt32(rbSMStart.SelectedValue) - 12) + Convert.ToInt32(ddlSMStartHour.SelectedValue);
                    int _ToTime = (Convert.ToInt32(rbSMEnd.SelectedValue) - 12) + Convert.ToInt32(ddlSMEndHour.SelectedValue);

                    if (_FromTime == 24)
                    {
                        _FromTime = 12;
                    }

                    if (_ToTime == 24)
                    {
                        _ToTime = 12;
                    }

                    if (rbSMDuration.Checked)
                    {
                        _FromTime = 0;
                        _ToTime = DateTime.Now.Hour;

                        if (ddlSMDuration.SelectedValue == "all")
                        {
                            _ToDate = DateTime.Now;
                            _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                        }
                        else
                        {
                            string[] _SelectedSMDuration = ddlSMDuration.SelectedValue.Split(',');
                            if (_SelectedSMDuration.Length > 1)
                            {
                                _ToDate = DateTime.Now;
                                int Time = -Math.Abs(Convert.ToInt32(_SelectedSMDuration[1]));
                                switch (_SelectedSMDuration[0])
                                {
                                    case "hour":
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                    case "day":
                                        _FromDate = DateTime.Now.AddDays(Time);
                                        break;
                                    case "month":
                                        _FromDate = DateTime.Now.AddMonths(Time);
                                        break;
                                    case "year":
                                        _FromDate = DateTime.Now.AddYears(Time);
                                        break;
                                    default:
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                }
                            }
                        }

                        searchSMRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                        searchSMRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);

                    }
                    else
                    {
                        _FromDate = Convert.ToDateTime(txtSMStartDate.Text);
                        _ToDate = Convert.ToDateTime(txtSMEndDate.Text);

                        searchSMRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        searchSMRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                    }

                    #region Source Category
                    List<String> smSourceCategoryList = new List<String>();
                    if (!chkSMCategorySelectAll.Checked)
                    {


                        foreach (RepeaterItem rptitem in rptSMCategory.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMCategory");
                            if (chk != null && chk.Checked)
                            {
                                smSourceCategoryList.Add("\"" + chk.Value + "\"");
                            }
                        }
                        searchSMRequest.SourceCategory = smSourceCategoryList;
                    }
                    #endregion

                    #region Source Type

                    List<String> smSourceTypeList = new List<String>();
                    //if (!chkSMTypeSelectAll.Checked)
                    //{
                    foreach (RepeaterItem rptitem in rptSMType.Items)
                    {
                        HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMType");
                        if (chk != null && chk.Checked)
                        {
                            if (chkSMTypeSelectAll.Checked || chk.Checked)
                                smSourceTypeList.Add("\"" + chk.Value + "\"");
                        }
                    }
                    searchSMRequest.SourceType = smSourceTypeList;
                    //}


                    #endregion


                    #region Source Rank

                    List<String> smSourceCategoryRank = new List<String>();
                    if (!chkSMRankSelectAll.Checked)
                    {
                        foreach (RepeaterItem rptitem in rptSMRank.Items)
                        {
                            HtmlInputCheckBox chk = (HtmlInputCheckBox)rptitem.FindControl("chkSMRank");
                            if (chk != null && chk.Checked)
                            {
                                smSourceCategoryRank.Add(chk.Value);
                            }
                        }
                        searchSMRequest.SourceRank = smSourceCategoryRank;
                    }

                    #endregion

                    #region Set Facet Params

                    searchSMRequest.Facet = true;

                    searchSMRequest.FacetRangeStarts = searchSMRequest.StartDate;
                    searchSMRequest.FacetRangeEnds = searchSMRequest.EndDate;
                    DateTime startdt = (DateTime)searchSMRequest.StartDate;
                    DateTime enddt = (DateTime)searchSMRequest.EndDate;

                    TimeSpan dateDiff = (enddt - startdt);
                    if (dateDiff.Days < 7)
                    {
                        searchSMRequest.FacetRangeGap = RangeGap.HOUR;
                    }
                    else
                    {
                        searchSMRequest.FacetRangeGap = RangeGap.DAY;
                    }

                    searchSMRequest.FacetRangeGapDuration = 1;

                    searchSMRequest.wt = ReponseType.json;

                    #endregion Set Facet Params

                    #endregion
                }

                if (chkTwitter.Checked && _SessionInformation.IsiQPremiumTwitter)
                {
                    #region Save Twitter Request
                    searchTwitterRequest = new SearchTwitterRequest();

                    searchTwitterRequest.SearchTerm = txtSearch.Text.Trim();
                    if (ucTwitterPager.CurrentPage != null)
                        searchTwitterRequest.PageNumber = ucTwitterPager.CurrentPage.Value;
                    else
                        searchTwitterRequest.PageNumber = 0;

                    searchTwitterRequest.PageSize = 15;

                    searchTwitterRequest.SortFields = "date-";

                    if (!string.IsNullOrWhiteSpace(txtTweetActor.Text))
                    {
                        searchTwitterRequest.ActorDisplayName = txtTweetActor.Text.Trim();
                    }

                    DateTime _FromDate = new DateTime();
                    DateTime _ToDate = new DateTime();

                    if (_SessionInformation.IsiQPremiumSentiment)
                    {
                        searchTwitterRequest.IsSentiment = true;
                        searchTwitterRequest.LowThreshold = _viewstateInformation.TwitterLowThreshold;
                        searchTwitterRequest.HighThreshold = _viewstateInformation.TwitterHighThreshold;
                        searchTwitterRequest.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                    }
                    if (rbTwitterDuration.Checked)
                    {
                        if (ddlTwitterDuration.SelectedValue == "all")
                        {
                            _ToDate = DateTime.Now;
                            _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                        }
                        else
                        {
                            string[] _SelectedNewsDuration = ddlTwitterDuration.SelectedValue.Split(',');
                            if (_SelectedNewsDuration.Length > 1)
                            {
                                _ToDate = DateTime.Now;
                                int Time = -Math.Abs(Convert.ToInt32(_SelectedNewsDuration[1]));
                                switch (_SelectedNewsDuration[0])
                                {
                                    case "hour":
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                    case "day":
                                        _FromDate = DateTime.Now.AddDays(Time);
                                        break;
                                    case "month":
                                        _FromDate = DateTime.Now.AddMonths(Time);
                                        break;
                                    case "year":
                                        _FromDate = DateTime.Now.AddYears(Time);
                                        break;
                                    default:
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                }
                            }
                        }

                        searchTwitterRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                        searchTwitterRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);
                    }
                    else
                    {
                        _FromDate = Convert.ToDateTime(txtTwitterStartDate.Text);
                        _ToDate = Convert.ToDateTime(txtTwitterEndDate.Text);

                        int _FromTime = (Convert.ToInt32(rbTwitterStart.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterStartHour.SelectedValue);
                        int _ToTime = (Convert.ToInt32(rbTwitterEnd.SelectedValue) - 12) + Convert.ToInt32(ddlTwitterEndHour.SelectedValue);

                        if (_FromTime == 24)
                        {
                            _FromTime = 12;
                        }

                        if (_ToTime == 24)
                        {
                            _ToTime = 12;
                        }

                        searchTwitterRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        searchTwitterRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                    }

                    if (!string.IsNullOrWhiteSpace(txtFollowerCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFollowerCountTo.Text))
                    {
                        searchTwitterRequest.FollowersRangeFrom = Convert.ToInt64(txtFollowerCountFrom.Text);
                        searchTwitterRequest.FollowersRangeTo = Convert.ToInt64(txtFollowerCountTo.Text);
                    }

                    if (!string.IsNullOrWhiteSpace(txtFriendsCountFrom.Text) && !string.IsNullOrWhiteSpace(txtFriendsCountTo.Text))
                    {
                        searchTwitterRequest.FriendsRangeFrom = Convert.ToInt64(txtFriendsCountFrom.Text);
                        searchTwitterRequest.FriendsRangeTo = Convert.ToInt64(txtFriendsCountTo.Text);
                    }

                    if (!string.IsNullOrWhiteSpace(txtKloutScoreFrom.Text) && !string.IsNullOrWhiteSpace(txtKloutScoreTo.Text))
                    {
                        searchTwitterRequest.KloutRangeFrom = Convert.ToInt64(txtKloutScoreFrom.Text);
                        searchTwitterRequest.KloutRangeTo = Convert.ToInt64(txtKloutScoreTo.Text);
                    }

                    #endregion

                    #region Set Facet Params

                    searchTwitterRequest.Facet = true;

                    searchTwitterRequest.FacetRangeStarts = searchTwitterRequest.StartDate;
                    searchTwitterRequest.FacetRangeEnds = searchTwitterRequest.EndDate;
                    searchTwitterRequest.FacetRange = "tweet_postedDatetime";
                    DateTime startdt = (DateTime)searchTwitterRequest.StartDate;
                    DateTime enddt = (DateTime)searchTwitterRequest.EndDate;

                    TimeSpan dateDiff = (enddt - startdt);
                    if (dateDiff.Days < 7)
                    {
                        searchTwitterRequest.FacetRangeGap = RangeGap.HOUR;
                    }
                    else
                    {
                        searchTwitterRequest.FacetRangeGap = RangeGap.DAY;
                    }

                    searchTwitterRequest.FacetRangeGapDuration = 1;

                    searchTwitterRequest.wt = ReponseType.json;

                    #endregion Set Facet Params

                }

                if (chkRadio.Checked && _SessionInformation.IsiQPremiumRadio)
                {
                    #region Save Radio Request
                    searchRadioRequest = new SearchRadioRequest();

                    DateTime _FromDate = new DateTime();
                    DateTime _ToDate = new DateTime();

                    int _FromTime = (Convert.ToInt32(rbRadioStart.SelectedValue) - 12) + Convert.ToInt32(ddlRadioStartHour.SelectedValue);
                    int _ToTime = (Convert.ToInt32(rbRadioEnd.SelectedValue) - 12) + Convert.ToInt32(ddlRadioEndHour.SelectedValue);

                    if (_FromTime == 24)
                    {
                        _FromTime = 12;
                    }

                    if (_ToTime == 24)
                    {
                        _ToTime = 12;
                    }
                    searchRadioRequest.PageSize = 10;

                    if (rbRadioDuration.Checked)
                    {
                        _FromTime = 0;
                        _ToTime = DateTime.Now.Hour;

                        if (ddlRadioDuration.SelectedValue == "all")
                        {
                            _ToDate = DateTime.Now;
                            _FromDate = Convert.ToDateTime(ConfigurationManager.AppSettings["IQPremiumDefaultStartDate"]);
                        }
                        else
                        {
                            string[] _SelectedDuration = ddlRadioDuration.SelectedValue.Split(',');
                            if (_SelectedDuration.Length > 1)
                            {
                                _ToDate = DateTime.Now;
                                int Time = -Math.Abs(Convert.ToInt32(_SelectedDuration[1]));
                                switch (_SelectedDuration[0])
                                {
                                    case "hour":
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                    case "day":
                                        _FromDate = DateTime.Now.AddDays(Time);
                                        break;
                                    case "month":
                                        _FromDate = DateTime.Now.AddMonths(Time);
                                        break;
                                    case "year":
                                        _FromDate = DateTime.Now.AddYears(Time);
                                        break;
                                    default:
                                        _FromDate = DateTime.Now.AddHours(Time);
                                        break;
                                }
                            }
                        }


                        _FromDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromDate.Hour, 0, 0);
                        _ToDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToDate.Hour, 0, 0);
                    }
                    else
                    {


                        _FromDate = Convert.ToDateTime(txtRadioStartDate.Text);
                        _ToDate = Convert.ToDateTime(txtRadioEndDate.Text);
                        _FromDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                        _ToDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);
                    }

                    searchRadioRequest.FromDate = _FromDate;
                    searchRadioRequest.ToDate = _ToDate;

                    _viewstateInformation.SortExpressionRadio = "DateTime";
                    _viewstateInformation.IsRadioSortDirecitonAsc = false;
                    List<string> _ListOfDmaName = new List<string>();

                    for (int _RadioStationsCount = 0; _RadioStationsCount < rptRadioMarket.Items.Count; _RadioStationsCount++)
                    {
                        HtmlInputCheckBox chkRadioStation = (HtmlInputCheckBox)rptRadioMarket.Items[_RadioStationsCount].FindControl("chkDma");
                        if (chkRadioStation.Checked)
                        {
                            string[] nameNum = Convert.ToString(chkRadioStation.Value).Split('#');
                            if (nameNum.Count() > 0)
                                _ListOfDmaName.Add(nameNum[0]);

                        }

                    }

                    List<IQ_STATION> _ListOfRadioStations = _viewstateInformation.ListOfRadioStations;
                    List<IQ_STATION> _ListOfSelectedRadioStations = _ListOfRadioStations.FindAll(
                                               delegate(IQ_STATION _Main_RL_STATION)
                                               {
                                                   string _dmaname = _ListOfDmaName.Find(delegate(string _TempDmaName) { return _TempDmaName == _Main_RL_STATION.dma_name; });
                                                   return (!string.IsNullOrEmpty(_dmaname));
                                               }
                                           );

                    string _CSIQCCKey = String.Join(",", _ListOfSelectedRadioStations.Select(x => x.IQ_Station_ID.ToString()).ToArray());

                    XDocument xDocIQCCKey = new XDocument(
                               new XElement
                                   ("IQ_Station_ID_Set",
                                            _ListOfSelectedRadioStations.Select(x => new XElement("IQ_Station_ID", x.IQ_Station_ID.Replace("'", string.Empty)))
                                   )

                           );

                    //string _CSIQCCKey = string.Empty;

                    List<string> _ListOfIQCCKey = new List<string>();

                    /* for (DateTime _IndexFromDate = _FromDate; _IndexFromDate <= _ToDate; )
                     {
                         string IQCCKey;
                         if (DateTime.Now.IsDaylightSavingTime())
                         {
                             var _IQCCKey = from _RadioStations in _ListOfSelectedRadioStations
                                            select IQCCKey = "'" + (_RadioStations.IQ_Station_ID.ToString() + "_" + _IndexFromDate.AddHours(((-1) * (Convert.ToDouble(_RadioStations.gmt_adj))) - Convert.ToDouble(_RadioStations.dst_adj)).ToString("yyyyMMdd") + "_" + _IndexFromDate.AddHours(((-1) * Convert.ToInt32(_RadioStations.gmt_adj)) - Convert.ToDouble(_RadioStations.dst_adj)).Hour.ToString().PadLeft(2, '0') + "00") + "'";

                             _ListOfIQCCKey = new List<string>(_IQCCKey);
                         }
                         else
                         {
                             var _IQCCKey = from _RadioStations in _ListOfSelectedRadioStations
                                            select IQCCKey = "'" + (_RadioStations.IQ_Station_ID.ToString() + "_" + _IndexFromDate.AddHours((-1) * (Convert.ToDouble(_RadioStations.gmt_adj))).ToString("yyyyMMdd") + "_" + _IndexFromDate.AddHours((-1) * Convert.ToInt32(_RadioStations.gmt_adj)).Hour.ToString().PadLeft(2, '0') + "00") + "'";

                             _ListOfIQCCKey = new List<string>(_IQCCKey);
                         }

                         if (String.IsNullOrEmpty(_CSIQCCKey))
                         {
                             _CSIQCCKey = string.Join(",", _ListOfIQCCKey.ToArray());
                         }
                         else
                         {
                             _CSIQCCKey = _CSIQCCKey + "," + string.Join(",", _ListOfIQCCKey.ToArray());
                         }

                         _IndexFromDate = _IndexFromDate.AddHours(1);
                     }*/

                    System.IO.Stream st = new System.IO.MemoryStream();

                    System.Xml.XmlDocument D = new System.Xml.XmlDocument();

                    D.LoadXml(xDocIQCCKey.ToString());

                    D.Save(st);

                    SqlXml sx = new SqlXml(st);

                    searchRadioRequest.IQStationIDXML = sx;

                    #endregion
                }


                #region Comparision
                if (Page.IsPostBack)
                {
                    if (chkTV.Checked && _viewstateInformation.searchRequestTV == null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (!chkTV.Checked && _viewstateInformation.searchRequestTV != null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (_SearchRequest != null && _viewstateInformation.searchRequestTV != null && !_SearchRequest.Equals((SearchRequest)_viewstateInformation.searchRequestTV))
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }

                    if (chkNews.Checked && _viewstateInformation.searchRequestOnlineNews == null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (!chkNews.Checked && _viewstateInformation.searchRequestOnlineNews != null)
                    {
                        ResetSavedSearchGridSelectedIndex();

                    }
                    else if (_searchNewsRequest != null && _viewstateInformation.searchRequestOnlineNews != null && !_searchNewsRequest.Equals((SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews))
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }

                    if (chkSocialMedia.Checked && _viewstateInformation.searchRequestSM == null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (!chkSocialMedia.Checked && _viewstateInformation.searchRequestSM != null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (searchSMRequest != null && _viewstateInformation.searchRequestSM != null && !searchSMRequest.Equals((SearchSMRequest)_viewstateInformation.searchRequestSM))
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }

                    if (chkTwitter.Checked && _viewstateInformation.searchRequestTwitter == null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (!chkTwitter.Checked && _viewstateInformation.searchRequestTwitter != null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (searchTwitterRequest != null && _viewstateInformation.searchRequestTwitter != null && !searchTwitterRequest.Equals((SearchTwitterRequest)_viewstateInformation.searchRequestTwitter))
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }

                    if (chkRadio.Checked && _viewstateInformation.searchRequestRadio == null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (!chkRadio.Checked && _viewstateInformation.searchRequestRadio != null)
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                    else if (searchRadioRequest != null && _viewstateInformation.searchRequestRadio != null && !searchRadioRequest.Equals((SearchRadioRequest)_viewstateInformation.searchRequestRadio))
                    {
                        ResetSavedSearchGridSelectedIndex();
                    }
                }

                _viewstateInformation.searchRequestTV = _SearchRequest;
                _viewstateInformation.searchRequestOnlineNews = _searchNewsRequest;
                _viewstateInformation.searchRequestSM = searchSMRequest;
                _viewstateInformation.searchRequestTwitter = searchTwitterRequest;
                _viewstateInformation.searchRequestRadio = searchRadioRequest;



                #endregion

                SetViewstateInformation(_viewstateInformation);
                // commnted filter update on 14-12-2012
                //upFilter.Update();
                Logger.Info("Save Request Object To ViewState Ends");
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }


        private void ResetSavedSearchGridSelectedIndex()
        {
            try
            {
                gvSavedSearch.SelectedIndex = -1;
                //if (_viewstateInformation.SavedSearchSelectedIndex != null)
                //    (gvSavedSearch.Rows[_viewstateInformation.SavedSearchSelectedIndex.Value].FindControl("btnEditSavedSearch") as ImageButton).Visible = false;

                if (_viewstateInformation.LoadedSavedSearch != null)
                    (gvSavedSearch.Rows[0].FindControl("btnEditSavedSearch") as ImageButton).Visible = false;

                _viewstateInformation.LoadedSavedSearch = null;
                //_viewstateInformation.SavedSearchSelectedIndex = null;
                //_viewstateInformation.SavedSearchSelectedPage = null;
                _viewstateInformation.CurrentPageSavedSearch = 0;
                SetViewstateInformation(_viewstateInformation);
                LoadSavedSearch();
                upSavedSearh.Update();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Validations

        protected void CVFromDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (Convert.ToDateTime(txtStartDate.Text) > System.DateTime.Now)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion Validations

        #region Pagination
        protected void ucCustomPager_PageIndexChange(object sender, EventArgs e)// int currentpageNumber)
        {
            try
            {
                if (_viewstateInformation != null && _viewstateInformation.searchRequestTV != null)
                {
                    if (_viewstateInformation.listchartZoomHistory != null && _viewstateInformation.listchartZoomHistory.Count > 0)
                    {
                        SearchRequest searchRequest = (SearchRequest)((SearchRequest)_viewstateInformation.searchRequestTV).SearchRequest_CloneObject();
                        searchRequest.StartDate = _viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].StartDate;
                        searchRequest.EndDate = _viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].EndDate;
                        searchRequest.PageNumber = (int)ucCustomPager.CurrentPage;
                        GetSearchResult(searchRequest, false);
                    }
                    else
                    {
                        SearchRequest searchRequest = (SearchRequest)_viewstateInformation.searchRequestTV;
                        searchRequest.PageNumber = (int)ucCustomPager.CurrentPage;
                        _viewstateInformation.searchRequestTV = searchRequest;
                        SetViewstateInformation(_viewstateInformation);
                        GetSearchResult(searchRequest, false);
                    }


                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowTVTab", "showTVTab();", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult(false);", true);
                    // commnted filter update on 14-12-2012
                    //upFilter.Update();
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void ucOnlineNewsPager_PageIndexChange(object sender, EventArgs e)// int currentpageNumber)
        {
            try
            {
                if (_viewstateInformation != null && _viewstateInformation.searchRequestOnlineNews != null)
                {
                    if (_viewstateInformation.listOnlineNewsChartZoomHistory != null && _viewstateInformation.listOnlineNewsChartZoomHistory.Count > 0)
                    {
                        SearchNewsRequest searchNewsRequest = (SearchNewsRequest)((SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews).SearchNewsRequest_CloneObject();
                        searchNewsRequest.StartDate = _viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].StartDate;
                        searchNewsRequest.EndDate = _viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].EndDate;
                        searchNewsRequest.PageNumber = (int)ucOnlineNewsPager.CurrentPage;
                        SearchNewsSection(searchNewsRequest, false);

                    }
                    else
                    {
                        SearchNewsRequest searchNewsRequest = (SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews;
                        searchNewsRequest.PageNumber = (int)ucOnlineNewsPager.CurrentPage;
                        _viewstateInformation.searchRequestOnlineNews = searchNewsRequest;
                        SetViewstateInformation(_viewstateInformation);
                        SearchNewsSection(searchNewsRequest, false);
                    }


                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLTab", "ChangeTab('tabOnlineNews','divGridTab',2,1);", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult(false);", true);

                    // commnted filter update on 14-12-2012
                    //upFilter.Update();
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void ucSMPager_PageIndexChange(object sender, EventArgs e)
        {
            try
            {
                if (_viewstateInformation != null && _viewstateInformation.searchRequestSM != null)
                {
                    if (_viewstateInformation.listSocialMediaChartZoomHistory != null && _viewstateInformation.listSocialMediaChartZoomHistory.Count > 0)
                    {
                        SearchSMRequest _SearchSMRequest = (SearchSMRequest)((SearchSMRequest)_viewstateInformation.searchRequestSM).SearchSMRequest_CloneObject();
                        _SearchSMRequest.StartDate = _viewstateInformation.listSocialMediaChartZoomHistory[_viewstateInformation.listSocialMediaChartZoomHistory.Count - 1].StartDate;
                        _SearchSMRequest.EndDate = _viewstateInformation.listSocialMediaChartZoomHistory[_viewstateInformation.listSocialMediaChartZoomHistory.Count - 1].EndDate;
                        _SearchSMRequest.PageNumber = (int)ucSMPager.CurrentPage;
                        SearchSocialMediaSection(_SearchSMRequest, false);
                    }
                    else
                    {
                        SearchSMRequest _SearchSMRequest = (SearchSMRequest)_viewstateInformation.searchRequestSM;
                        _SearchSMRequest.PageNumber = (int)ucSMPager.CurrentPage;
                        _viewstateInformation.searchRequestSM = _SearchSMRequest;
                        SetViewstateInformation(_viewstateInformation);

                        SearchSocialMediaSection(_SearchSMRequest, false);
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMTab", "ChangeTab('tabSocialMedia','divGridTab',3,1);", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult(false);", true);
                }
                // commnted filter update on 14-12-2012
                //upFilter.Update();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void ucTwitterPager_PageIndexChange(object sender, EventArgs e)
        {
            try
            {
                if (_viewstateInformation != null && _viewstateInformation.searchRequestTwitter != null)
                {
                    if (_viewstateInformation.listTwitterChartZoomHistory != null && _viewstateInformation.listTwitterChartZoomHistory.Count > 0)
                    {
                        SearchTwitterRequest _SearchTwitterRequest = (SearchTwitterRequest)(((SearchTwitterRequest)_viewstateInformation.searchRequestTwitter).SearchTwitterRequest_CloneObject());
                        _SearchTwitterRequest.StartDate = _viewstateInformation.listTwitterChartZoomHistory[_viewstateInformation.listTwitterChartZoomHistory.Count - 1].StartDate;
                        _SearchTwitterRequest.EndDate = _viewstateInformation.listTwitterChartZoomHistory[_viewstateInformation.listTwitterChartZoomHistory.Count - 1].EndDate;
                        _SearchTwitterRequest.PageNumber = (int)ucTwitterPager.CurrentPage;
                        SearchTwitterSection(_SearchTwitterRequest, false);
                    }
                    else
                    {
                        SearchTwitterRequest _SearchTwitterRequest = (SearchTwitterRequest)_viewstateInformation.searchRequestTwitter;
                        _SearchTwitterRequest.PageNumber = (int)ucTwitterPager.CurrentPage;
                        _viewstateInformation.searchRequestTwitter = _SearchTwitterRequest;
                        SetViewstateInformation(_viewstateInformation);

                        SearchTwitterSection(_SearchTwitterRequest, false);
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "showTwitterTab();", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult(false);", true);
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void ucRadioPager_PageIndexChange(object sender, EventArgs e)
        {
            try
            {
                BindRadioGrid();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        #endregion

        #region Sorting
        private void SortClipDirection(string sortField, bool sortDirectionAsc, GridView gvView)
        {


            GridViewRow gridViewHeaderSearchRow = gvView.HeaderRow;

            if (gridViewHeaderSearchRow != null)
            {
                foreach (TableCell headerSearchCell in gridViewHeaderSearchRow.Cells)
                {
                    if (headerSearchCell.HasControls())
                    {
                        LinkButton headerSearchButton = headerSearchCell.Controls[0] as LinkButton;

                        if (headerSearchButton != null)
                        {
                            HtmlGenericControl divSearch = new HtmlGenericControl("div");

                            Label headerSearchText = new Label();
                            headerSearchText.Text = headerSearchButton.Text;

                            divSearch.Controls.Add(headerSearchText);


                            if (headerSearchButton.CommandArgument == sortField)
                            {
                                Image headerSearchImage = new Image();

                                if (sortDirectionAsc)
                                {
                                    headerSearchImage.Attributes.Add("style", "padding-left:3px");
                                    headerSearchImage.ImageUrl = "~/Images/arrow-up.gif";
                                }
                                else
                                {
                                    headerSearchImage.Attributes.Add("style", "padding-left:3px");
                                    headerSearchImage.ImageUrl = "~/Images/arrow-down.gif";
                                }
                                divSearch.Controls.Add(headerSearchImage);
                            }
                            headerSearchButton.Controls.Clear();
                            headerSearchButton.Controls.Add(divSearch);
                        }
                    }
                }
            }
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();", true);
            if (gvView.ID == rptTV.ID)
            {
                upGrid.Update();
            }
            else if (gvView.ID == gvOnlineNews.ID)
            {
                upOnlineNews.Update();
            }
            else if (gvView.ID == gvSocialMedia.ID)
            {
                upSocialMedia.Update();
            }
        }

        protected void GvOnlineNews_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_viewstateInformation.SortExpressionOnlineNews))
                {
                    if (_viewstateInformation.SortExpressionOnlineNews.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_viewstateInformation.IsOnlineNewsSortDirecitonAsc == true)
                        {
                            _viewstateInformation.IsOnlineNewsSortDirecitonAsc = false;
                        }
                        else
                        {
                            _viewstateInformation.IsOnlineNewsSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _viewstateInformation.SortExpressionOnlineNews = e.SortExpression;
                        _viewstateInformation.IsOnlineNewsSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _viewstateInformation.SortExpressionOnlineNews = e.SortExpression;
                    _viewstateInformation.IsOnlineNewsSortDirecitonAsc = true;
                }

                SetViewstateInformation(_viewstateInformation);
                ucOnlineNewsPager.CurrentPage = 0;

                if (_viewstateInformation != null && _viewstateInformation.searchRequestOnlineNews != null)
                {
                    SearchNewsRequest searchNewsRequest;
                    if (_viewstateInformation.listOnlineNewsChartZoomHistory != null && _viewstateInformation.listOnlineNewsChartZoomHistory.Count > 0)
                    {
                        searchNewsRequest = (SearchNewsRequest)((SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews).SearchNewsRequest_CloneObject();
                        searchNewsRequest.StartDate = _viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].StartDate;
                        searchNewsRequest.EndDate = _viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].EndDate;
                    }
                    else
                    {
                        searchNewsRequest = (SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews;
                        _viewstateInformation.searchRequestOnlineNews = searchNewsRequest;
                    }

                    if (_viewstateInformation != null && !string.IsNullOrEmpty(_viewstateInformation.SortExpressionOnlineNews))
                    {
                        if (!_viewstateInformation.IsOnlineNewsSortDirecitonAsc)
                        {
                            searchNewsRequest.SortFields = _viewstateInformation.SortExpressionOnlineNews + "-";
                        }
                        else
                        {
                            searchNewsRequest.SortFields = _viewstateInformation.SortExpressionOnlineNews;
                        }
                    }

                    searchNewsRequest.PageNumber = 0;

                    SetViewstateInformation(_viewstateInformation);

                    SearchNewsSection(searchNewsRequest, false);
                    //SortClipDirection(e.SortExpression, _viewstateInformation.IsSortDirecitonAsc, gvOnlineNews);

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "shwOLTab", "showOnlineNewsTab();", true);
                }


                /*if (_viewstateInformation != null && _viewstateInformation.searchRequestOnlineNews != null)
                {
                    // SearchNewsRequest searchNewsRequest = (SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews;

                    SearchNewsSection(false);
                    SortClipDirection(e.SortExpression, _viewstateInformation.IsOnlineNewsSortDirecitonAsc, gvOnlineNews);
                }*/

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GvSocialMedia_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_viewstateInformation.SortExpressionSocialMedia))
                {
                    if (_viewstateInformation.SortExpressionSocialMedia.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_viewstateInformation.IsSocialMediaSortDirecitonAsc == true)
                        {
                            _viewstateInformation.IsSocialMediaSortDirecitonAsc = false;
                        }
                        else
                        {
                            _viewstateInformation.IsSocialMediaSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _viewstateInformation.SortExpressionSocialMedia = e.SortExpression;
                        _viewstateInformation.IsSocialMediaSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _viewstateInformation.SortExpressionSocialMedia = e.SortExpression;
                    _viewstateInformation.IsSocialMediaSortDirecitonAsc = true;
                }

                SetViewstateInformation(_viewstateInformation);
                ucSMPager.CurrentPage = 0;

                if (_viewstateInformation != null && _viewstateInformation.searchRequestSM != null)
                {
                    SearchSMRequest searchSMRequest;
                    if (_viewstateInformation.listSocialMediaChartZoomHistory != null && _viewstateInformation.listSocialMediaChartZoomHistory.Count > 0)
                    {
                        searchSMRequest = (SearchSMRequest)((SearchSMRequest)_viewstateInformation.searchRequestSM).SearchSMRequest_CloneObject();
                        searchSMRequest.StartDate = _viewstateInformation.listSocialMediaChartZoomHistory[_viewstateInformation.listSocialMediaChartZoomHistory.Count - 1].StartDate;
                        searchSMRequest.EndDate = _viewstateInformation.listSocialMediaChartZoomHistory[_viewstateInformation.listSocialMediaChartZoomHistory.Count - 1].EndDate;
                    }
                    else
                    {
                        searchSMRequest = (SearchSMRequest)_viewstateInformation.searchRequestSM;
                        _viewstateInformation.searchRequestSM = searchSMRequest;
                    }

                    if (_viewstateInformation != null && !string.IsNullOrEmpty(_viewstateInformation.SortExpressionSocialMedia))
                    {
                        if (!_viewstateInformation.IsSocialMediaSortDirecitonAsc)
                        {
                            searchSMRequest.SortFields = _viewstateInformation.SortExpressionSocialMedia + "-";
                        }
                        else
                        {
                            searchSMRequest.SortFields = _viewstateInformation.SortExpressionSocialMedia;
                        }
                    }

                    searchSMRequest.PageNumber = 0;

                    SetViewstateInformation(_viewstateInformation);

                    SearchSocialMediaSection(searchSMRequest, false);

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "shwSMTab", "showSocialMediaTab();", true);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void grvRadioStations_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_viewstateInformation.SortExpressionRadio))
                {
                    if (_viewstateInformation.SortExpressionRadio.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_viewstateInformation.IsRadioSortDirecitonAsc == true)
                        {
                            _viewstateInformation.IsRadioSortDirecitonAsc = false;
                        }
                        else
                        {
                            _viewstateInformation.IsRadioSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _viewstateInformation.SortExpressionRadio = e.SortExpression;
                        _viewstateInformation.IsRadioSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _viewstateInformation.SortExpressionRadio = e.SortExpression;
                    _viewstateInformation.IsRadioSortDirecitonAsc = true;
                }

                SetViewstateInformation(_viewstateInformation);
                ucRadioPager.CurrentPage = 0;

                if (_viewstateInformation != null && _viewstateInformation.searchRequestRadio != null)
                {
                    SearchRadioRequest searchRadioRequest = new SearchRadioRequest();
                    /*if (_viewstateInformation.listOnlineNewsChartZoomHistory != null && _viewstateInformation.listOnlineNewsChartZoomHistory.Count > 0)
                    {
                        searchNewsRequest = (SearchNewsRequest)((SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews).SearchNewsRequest_CloneObject();
                        searchNewsRequest.StartDate = _viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].StartDate;
                        searchNewsRequest.EndDate = _viewstateInformation.listOnlineNewsChartZoomHistory[_viewstateInformation.listOnlineNewsChartZoomHistory.Count - 1].EndDate;
                    }
                    else
                    {
                        searchNewsRequest = (SearchNewsRequest)_viewstateInformation.searchRequestOnlineNews;
                        _viewstateInformation.searchRequestOnlineNews = searchNewsRequest;
                    }*/

                    /*if (_viewstateInformation != null && !string.IsNullOrEmpty(_viewstateInformation.SortExpressionRadio))
                    {
                        if (!_viewstateInformation.IsOnlineNewsSortDirecitonAsc)
                        {
                            searchRadioRequest.SortExpression = _viewstateInformation.SortExpressionOnlineNews + "-";
                        }
                        else
                        {
                            searchRadioRequest.SortExpression = _viewstateInformation.SortExpressionOnlineNews;
                        }
                    }*/

                    searchRadioRequest.PageNumber = 0;

                    SetViewstateInformation(_viewstateInformation);
                    BindRadioGrid();
                    // SearchNewsSection(searchNewsRequest, false);


                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "shwRadioTab", "showRadioTab();", true);
                }




            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void rptTV_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "SORT")
                {
                    if (!string.IsNullOrEmpty(_viewstateInformation.SortExpression))
                    {
                        if (_viewstateInformation.SortExpression.ToLower() == e.CommandArgument.ToString().ToLower())
                        {
                            if (_viewstateInformation.IsSortDirecitonAsc == true)
                            {
                                _viewstateInformation.IsSortDirecitonAsc = false;
                            }
                            else
                            {
                                _viewstateInformation.IsSortDirecitonAsc = true;
                            }
                        }
                        else
                        {
                            _viewstateInformation.SortExpression = e.CommandArgument.ToString();
                            _viewstateInformation.IsSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _viewstateInformation.SortExpression = e.CommandArgument.ToString();
                        _viewstateInformation.IsSortDirecitonAsc = true;
                    }

                    SetViewstateInformation(_viewstateInformation);
                    ucCustomPager.CurrentPage = 0;

                    if (_viewstateInformation != null && _viewstateInformation.searchRequestTV != null)
                    {
                        SearchRequest searchRequest;
                        if (_viewstateInformation.listchartZoomHistory != null && _viewstateInformation.listchartZoomHistory.Count > 0)
                        {
                            searchRequest = (SearchRequest)((SearchRequest)_viewstateInformation.searchRequestTV).SearchRequest_CloneObject();
                            searchRequest.StartDate = _viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].StartDate;
                            searchRequest.EndDate = _viewstateInformation.listchartZoomHistory[_viewstateInformation.listchartZoomHistory.Count - 1].EndDate;
                        }
                        else
                        {
                            searchRequest = (SearchRequest)_viewstateInformation.searchRequestTV;
                            _viewstateInformation.searchRequestTV = searchRequest;
                        }

                        if (_viewstateInformation != null && !string.IsNullOrEmpty(_viewstateInformation.SortExpression))
                        {
                            if (!_viewstateInformation.IsSortDirecitonAsc)
                            {
                                searchRequest.SortFields = _viewstateInformation.SortExpression + "-";
                            }
                            else
                            {
                                searchRequest.SortFields = _viewstateInformation.SortExpression;
                            }
                        }

                        searchRequest.PageNumber = 0;

                        SetViewstateInformation(_viewstateInformation);

                        GetSearchResult(searchRequest, false);
                        //SortClipDirection(_viewstateInformation.SortExpression, _viewstateInformation.IsSortDirecitonAsc, rptTV);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTVTab", "ChangeTab('tabTV','divGridTab',0,1);", true);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnTwiiterSort_Click(object sender, EventArgs e)
        {
            try
            {
                _viewstateInformation.SortExpressionTwitter = ddlTwitterSortExp.SelectedValue;
                _viewstateInformation.IsTwitterSortDirecitonAsc = Convert.ToBoolean(rbTwitterSortDir.SelectedValue);
                if (_viewstateInformation != null && _viewstateInformation.searchRequestTwitter != null)
                {
                    SearchTwitterRequest searchTwitterRequest;

                    if (_viewstateInformation.listTwitterChartZoomHistory != null && _viewstateInformation.listTwitterChartZoomHistory.Count > 0)
                    {
                        searchTwitterRequest = (SearchTwitterRequest)(((SearchTwitterRequest)_viewstateInformation.searchRequestTwitter).SearchTwitterRequest_CloneObject());
                        searchTwitterRequest.StartDate = _viewstateInformation.listTwitterChartZoomHistory[_viewstateInformation.listTwitterChartZoomHistory.Count - 1].StartDate;
                        searchTwitterRequest.EndDate = _viewstateInformation.listTwitterChartZoomHistory[_viewstateInformation.listTwitterChartZoomHistory.Count - 1].EndDate;
                    }
                    else
                    {
                        searchTwitterRequest = (SearchTwitterRequest)_viewstateInformation.searchRequestTwitter;
                        _viewstateInformation.searchRequestTwitter = searchTwitterRequest;
                    }

                    ucTwitterPager.CurrentPage = 0;
                    searchTwitterRequest.PageNumber = 0;

                    if (!_viewstateInformation.IsTwitterSortDirecitonAsc)
                    {
                        searchTwitterRequest.SortFields = _viewstateInformation.SortExpressionTwitter + "-";
                    }
                    else
                    {
                        searchTwitterRequest.SortFields = _viewstateInformation.SortExpressionTwitter;
                    }
                    SetViewstateInformation(_viewstateInformation);

                    SearchTwitterSection(searchTwitterRequest, false);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "showTwitterTab();", true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void SortClipDirection(string sortField, bool sortDirectionAsc, Repeater rptView)
        {
            RepeaterItem RptHeaderRow = rptView.Controls[0] as RepeaterItem;
            if (RptHeaderRow != null)
            {
                if (RptHeaderRow.HasControls())
                {
                    for (int i = 0; i < RptHeaderRow.Controls.Count; i++)
                    {
                        LinkButton headerSearchButton = RptHeaderRow.Controls[i] as LinkButton;
                        if (headerSearchButton != null)
                        {
                            HtmlGenericControl divSearch = new HtmlGenericControl("span");

                            Literal headerSearchText = new Literal();
                            headerSearchText.Text = headerSearchButton.Text;

                            divSearch.Controls.Add(headerSearchText);


                            if (headerSearchButton.CommandArgument.ToLower() == sortField.ToLower())
                            {
                                Image headerSearchImage = new Image();

                                if (sortDirectionAsc)
                                {
                                    headerSearchImage.Attributes.Add("style", "padding-left:3px");
                                    headerSearchImage.ImageUrl = "~/Images/arrow-up.gif";
                                }
                                else
                                {
                                    headerSearchImage.Attributes.Add("style", "padding-left:3px");
                                    headerSearchImage.ImageUrl = "~/Images/arrow-down.gif";
                                }
                                divSearch.Controls.Add(headerSearchImage);
                            }
                            headerSearchButton.Controls.Clear();
                            headerSearchButton.Controls.Add(divSearch);
                        }
                    }
                }
            }
        }

        #endregion

    }


    #region Support Classes

    /* public class ChartData
     {
         public Chart Chart { get; set; }
         public List<Data> data;
     }*/
    [Serializable]
    [DataContract]
    public class Chart
    {
        /*public string caption { get; set; }        
        public string xAxisName { get; set; }
        public string yAxisName { get; set; }
        public string showvalues { get; set; }
        public string numberprefix { get; set; }*/
        /*[DataMember(Name = "compactdatamode")]
        public string compactdatamode { get; set; }

        [DataMember(Name = "dataseparator")]
        public string dataseparator { get; set; }
         
          
         * 
         * [DataMember(Name = "axis")]
        public string axis { get; set; }

        [DataMember(Name = "numberprefix")]
        public string numberprefix { get; set; }

        [DataMember(Name = "formatnumberscale")]
        public string formatnumberscale { get; set; }        

        [DataMember(Name = "enableiconmousecursors")]
        public string enableiconmousecursors { get; set; }

        [DataMember(Name = "dynamicaxis")]
        public string dynamicaxis { get; set; }

        [DataMember(Name = "palette")]
        public string palette { get; set; }
         * 
         * [DataMember(Name = "showExportDataMenuItem")]
        public string showExportDataMenuItem { get; set; }

         */

        [DataMember(Name = "showBorder")]
        public string showBorder { get; set; }

        [DataMember(Name = "bgColor")]
        public string bgColor { get; set; }


        [DataMember(Name = "caption")]
        public string caption { get; set; }

        [DataMember(Name = "subcaption")]
        public string subcaption { get; set; }

        [DataMember(Name = "formatnumberscale")]
        public string formatnumberscale { get; set; }

        [DataMember(Name = "numVisibleLabels")]
        public string numVisibleLabels { get; set; }


        [DataMember(Name = "exportEnabled")]
        public string exportEnabled { get; set; }

        [DataMember(Name = "exportAtClient")]
        public string exportAtClient { get; set; }

        [DataMember(Name = "exportHandler")]
        public string exportHandler { get; set; }

        [DataMember(Name = "drawAnchors")]
        public string drawAnchors { get; set; }

        [DataMember(Name = "allowpinmode")]
        public string allowpinmode { get; set; }

        [DataMember(Name = "yaxisname")]
        public string yaxisname { get; set; }

        [DataMember(Name = "chartRightMargin")]
        public string chartRightMargin { get; set; }

    }

    [Serializable]
    [DataContract]
    class ZoomChart
    {

        [DataMember(Name = "chart")]
        public Chart Chart { get; set; }

        [DataMember(Name = "categories")]
        public List<Category> Categories { get; set; }

        [DataMember(Name = "dataset")]
        public List<Dataset> Dataset { get; set; }
    }

    [Serializable]
    [DataContract]
    class Dataset
    {

        [DataMember(Name = "seriesname")]
        public string Seriesname { get; set; }

        [DataMember(Name = "data")]
        public List<Datum> Data { get; set; }
    }
    [Serializable]
    [DataContract]
    class Category
    {

        [DataMember(Name = "category")]
        public List<Category2> Category1 { get; set; }
    }

    [Serializable]
    [DataContract]
    class Category2 : IEqualityComparer<Category2>
    {

        [DataMember(Name = "label")]
        public string Label { get; set; }

        public bool Equals(Category2 x, Category2 y)
        {
            return x.Label.Equals(y.Label);
        }

        public int GetHashCode(Category2 obj)
        {
            return obj.Label.GetHashCode();
        }


    }
    [Serializable]
    [DataContract]
    class Datum
    {

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
    /*  public class Data
      {
          public string label { get; set; }
          public string value { get; set; }
      }

      public class ZoomChart
      {
          public Chart chart { get; set; }
          public categories categories { get; set; }
          public dataset dataset { get; set; }
      }

      public class categories
      {
          public categoryList categoryList { get; set; }
      }

      public class categoryList
      {
          public List<category> category { get; set; }
      }

      public class category
      {
          public string label { get; set; }
      }

      public class dataset
      {        
          public List<data> data { get; set; }
      }

      public class data
      {
          public string seriesname { get; set; } 
          [DataMember(Name="")]
          public List<value> value { get; set; }
      }

      public class value
      {
          [DataMember(Name="value")]
          public string dataValue { get; set; }
      }*/



    //public class ZoomChart
    //{
    //    public Chart chart { get; set; }
    //    public List<categories> categories { get; set; }
    //    public List<dataset> dataset { get; set; }
    //}

    //public class categories
    //{
    //    public string category { get; set; }
    //}

    //public class dataset
    //{
    //    public string seriesname { get; set; }
    //    public string data { get; set; }
    //}

    //public class SolrResponse
    //{
    //    public string status { get; set; }
    //    public string QTime { get; set; }
    //    public string QTime { get; set; }
    //}

    //public class Params
    //{
    //    public string facet { get; set; }
    //    public string facet { get; set; }
    //}

    [DataContract(Name = "__type")]
    public class response
    {

        public facet_counts facet_counts { get; set; }
    }


    public class facet_counts
    {
        public facet_ranges facet_ranges { get; set; }
    }


    public class facet_ranges
    {

        public List<child> lstChild { get; set; }
    }

    [DataContract(Name = "__type")]
    public class child
    {
        [DataMember(Name = "__type")]
        public string clsName { get; set; }
        [DataMember(Name = "__type")]
        public List<counts> counts { get; set; }
    }

    [DataContract(Name = "__type")]
    public class counts
    {
        [DataMember(Name = "__type")]
        public string label { get; set; }
        [DataMember(Name = "__type")]
        public string value { get; set; }
    }

    class IQ_STATIONComparer : IEqualityComparer<IQ_STATION>
    {
        #region IEqualityComparer<IQ_STATION> Members

        public bool Equals(IQ_STATION x, IQ_STATION y)
        {
            return x.dma_name.Equals(y.dma_name);
        }

        public int GetHashCode(IQ_STATION obj)
        {
            return obj.dma_name.GetHashCode();
        }

        #endregion
    }

    #endregion
}


