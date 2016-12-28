using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using System.Threading;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using IQMediaGroup.Controller.Common;
using System.Xml.Linq;
using PMGSearch;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IQAgent
{
    public partial class IQAgent : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        ViewstateInformation _ViewstateInformation;
        SessionInformation _SessionInformation;
        List<RawMedia> _ListOfFinalClips = new List<RawMedia>();
        string _Email = string.Empty;
        string _Frequency = string.Empty;
        private string _ErrorMessage = "Notification already exists.";

        #region Page Events

        protected override void OnLoad(EventArgs e)
        {
            try
            {

                //hfIsTimeOut.Value = true.ToString().ToLower();
                //lblTimeOutMsg.Text = string.Empty;
                //lblTimeOutMsg.Style.Add("display", "none");

                _ViewstateInformation = GetViewstateInformation();
                _SessionInformation = CommonFunctions.GetSessionInformation();

                if (!IsPostBack)
                {
                    BindDropDown();
                    BindFrequencyDropDown(drpOption);
                    BindTwitterSortDropDown();
                    BindMediaCategoryDropDown();
                }

                if (Request["__EventTarget"] != null && Request["__EventTarget"] == upOnlineNews.ClientID && Request["__EventArgument"] == "Tab")
                {
                    hfOnlineNewsStatus.Value = "1";
                    //hfTVStatus.Value = "0";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setOLTab", "ChangeTab('tabOnlineNews','divGridTab',1,1);", true);
                    GetNewsResult();

                }
                else if (Request["__EventTarget"] != null && Request["__EventTarget"] == upTVGrid.ClientID && Request["__EventArgument"] == "Tab")
                {

                    //hfOnlineNewsStatus.Value = "0";
                    hfTVStatus.Value = "1";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setTVTab", "ChangeTab('tabTV','divGridTab',0,1);", true);
                    GetTVResult();

                }
                else if (Request["__EventTarget"] != null && Request["__EventTarget"] == upSocialMedia.ClientID && Request["__EventArgument"] == "Tab")
                {
                    hfSocialMediaStatus.Value = "1";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setSMTab", "ChangeTab('tabSocialMedia','divGridTab',2,1);", true);
                    GetSocialMediaResult();

                }
                else if (Request["__EventTarget"] != null && Request["__EventTarget"] == upTwitter.ClientID && Request["__EventArgument"] == "Tab")
                {
                    hfTwitterMediaStatus.Value = "1";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setTwitterTab", "ChangeTab('tabTwitter','divGridTab',3,1);", true);
                    GetTwitterResult();


                }

                lblSuccessMessage.Text = string.Empty;
                lblSuccessMessageNM.Text = string.Empty;
                lblSuccessMessageSM.Text = string.Empty;
                lblErrorMessage.Visible = false;
                lblNotificationError.Text = string.Empty;
                lblSuccessDelete.Text = string.Empty;

                SortClipDirection(_ViewstateInformation.SortExpression, _ViewstateInformation.IsSortDirecitonAsc, rptTV);
                SortClipDirection(_ViewstateInformation.SortExpressionSocialMedia, _ViewstateInformation.IsSocialMediaSortDirecitonAsc, gvSocialMedia);
                SortClipDirection(_ViewstateInformation.SortExpressionOnlineNews, _ViewstateInformation.IsOnlineNewsSortDirecitonAsc, gvOnlineNews);
                if (Request["__EventTarget"] != null && Request["__EventTarget"] != btnRefresh.ClientID && Request["__EventTarget"] != drpQueryName.ClientID && (string.IsNullOrWhiteSpace(Request["__EventTarget"]) || Request["__EventTarget"].Contains(this.ClientID) || Request["__EventTarget"].Contains(this.UniqueID)))
                {
                    SetTabVisiBility();
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        #endregion

        #region User Defined Events

        private void BindDropDown()
        {
            try
            {

                List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = new List<IQAgentSearchRequest>();
                ISearchRequestController _ISearchRequestController = _ControllerFactory.CreateObject<ISearchRequestController>();
                IQAgentSearchRequest _IQAgentSearchRequestInfo = new IQAgentSearchRequest();
                _IQAgentSearchRequestInfo.ClientGuid = new Guid(_SessionInformation.ClientGUID);
                _ListOfIQAgentSearchRequest = _ISearchRequestController.SelectByClientID(_IQAgentSearchRequestInfo.ClientGuid.Value);
                if (_ListOfIQAgentSearchRequest.Count > 0)
                {
                    drpQueryName.DataTextField = "Query_Name";
                    drpQueryName.DataValueField = "ID";
                    drpQueryName.DataSource = _ListOfIQAgentSearchRequest;
                    drpQueryName.DataBind();
                    drpQueryName.Items.Insert(0, new ListItem("Select", "0"));

                }

            }
            catch (Exception _Exception)
            {

                throw _Exception;
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
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void SetTabVisiBility()
        {
            try
            {
                if (!_ViewstateInformation.IsIQAgentTVResultShow)
                {

                    string _Script = "$('#tabTV').css('display','none');"
                        + "$('#divTVResult').css('display','none');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTVTab", _Script, true);

                    rptTV.DataSource = null;
                    rptTV.DataBind();
                    upTVGrid.Update();
                }
                else
                {
                    string _Script = "$('#tabTV').css('display','block');"
                        + "$('#divTVResult').css('display','block');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowTV", _Script, true);
                }

                if (!_ViewstateInformation.IsIQAgentNMResultShow)
                {
                    string _Script = "$('#tabOnlineNews').css('display','none');"
                    + "$('#divOnlineNewsResult').css('display','none');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideNewsTab", _Script, true);

                    gvOnlineNews.DataSource = null;
                    gvOnlineNews.DataBind();
                    upOnlineNews.Update();

                }
                else
                {
                    string _Script = "$('#tabOnlineNews').css('display','block');"
                    + "$('#divOnlineNewsResult').css('display','block');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowNewsTab", _Script, true);

                }

                if (!_ViewstateInformation.IsIQAgentSMResultShow)
                {
                    string _Script = "$('#tabSocialMedia').css('display','none');"
                     + "$('#divSocialMediaResult').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideSocialMediaTab", _Script, true);

                    gvSocialMedia.DataSource = null;
                    gvSocialMedia.DataBind();
                    upSocialMedia.Update();

                }
                else
                {
                    string _Script = "$('#tabSocialMedia').css('display','block');"
                     + "$('#divSocialMediaResult').css('display','block');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowSocialMediaTab", _Script, true);

                }

                if (!_ViewstateInformation.IsIQAgentTwitterResultShow)
                {

                    string _Script = "$('#tabTwitter').css('display','none');"
                    + "$('#divTwitterResult').css('display','none');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideTwitterTab", _Script, true);

                    dlTweets.DataSource = null;
                    dlTweets.DataBind();
                    upTwitter.Update();
                }
                else
                {
                    string _Script = "$('#tabTwitter').css('display','block');"
                    + "$('#divTwitterResult').css('display','block');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowTwitterTab", _Script, true);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ParseSearchRequestToShowTabs(string SearchTerm)
        {
            try
            {
                XDocument _xdoc = XDocument.Parse(SearchTerm);
                #region Set SearchTerm
                if (_xdoc.Root.Element("SearchTerm") != null && !string.IsNullOrEmpty(_xdoc.Root.Element("SearchTerm").Value))
                {
                    txtSearchMediaText.Text = _xdoc.Root.Element("SearchTerm").Value;
                }
                else
                {
                    txtSearchMediaText.Text = string.Empty;

                }
                #endregion
                if (_xdoc.Root.Element("TV") == null)
                {
                    _ViewstateInformation.IsIQAgentTVResultShow = false;
                }
                else
                {
                    _ViewstateInformation.IsIQAgentTVResultShow = true;
                }

                if (_xdoc.Root.Element("News") == null)
                {
                    _ViewstateInformation.IsIQAgentNMResultShow = false;
                }
                else
                {
                    _ViewstateInformation.IsIQAgentNMResultShow = true;
                }

                if (_xdoc.Root.Element("SocialMedia") == null)
                {
                    _ViewstateInformation.IsIQAgentSMResultShow = false;
                }
                else
                {
                    _ViewstateInformation.IsIQAgentSMResultShow = true;
                }

                if (_xdoc.Root.Element("Twitter") == null)
                {

                    _ViewstateInformation.IsIQAgentTwitterResultShow = false;
                }
                else
                {
                    _ViewstateInformation.IsIQAgentTwitterResultShow = true;
                }

                SetViewstateInformation(_ViewstateInformation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataFromDB()
        {
            try
            {
                if (_ViewstateInformation != null)
                {
                    _ViewstateInformation.SortExpression = "IQ_Local_Air_DateTime";
                    _ViewstateInformation.SortExpressionOnlineNews = "harvest_time";
                    _ViewstateInformation.SortExpressionSocialMedia = "itemHarvestDate_DT";
                    _ViewstateInformation.SortExpressionTwitter = string.Empty;
                    SetViewstateInformation(_ViewstateInformation);
                }

                ucCustomPager.CurrentPage = 0;
                ucOnlineNewsPager.CurrentPage = 0;
                ucSMPager.CurrentPage = 0;
                ucTwitterPager.CurrentPage = 0;

                switch (GetGridToBind())
                {
                    case 0: hfTVStatus.Value = "1";
                        GetTVResult();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setTVTab", "ChangeTab('tabTV','divGridTab',0,1);", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetOtherTabStatus", "SetNewsFilterStatus(0);SetSMFilterStatus(0);SetTwitterFilterStatus(0);", true);
                        break;

                    case 1: hfOnlineNewsStatus.Value = "1";
                        GetNewsResult();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setOLTab", "ChangeTab('tabOnlineNews','divGridTab',1,1);", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetTVStatus", "SetTVFilterStatus(0);SetSMFilterStatus(0);SetTwitterFilterStatus(0);", true);
                        break;

                    case 2: hfSocialMediaStatus.Value = "1";
                        GetSocialMediaResult();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setSMTab", "ChangeTab('tabSocialMedia','divGridTab',2,1);", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetSMStatus", "SetTVFilterStatus(0);SetNewsFilterStatus(0);SetTwitterFilterStatus(0);", true);
                        break;
                    case 3: hfTwitterMediaStatus.Value = "1";
                        GetTwitterResult();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setTwitterTab", "ChangeTab('tabTwitter','divGridTab',3,1);", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetTwitterStatus", "SetTVFilterStatus(0);SetNewsFilterStatus(0);SetSMFilterStatus(0);", true);

                        break;

                    default: break;
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show('slow');$(\"#divChartResult\").show('slow');ShowHideDivResult('divResult',false);ShowHideDivChart(false);", true);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void GetTwitterResult()
        {
            try
            {
                /*pnlTwitterSort.Visible = false;
                divNoResultTwitter.Visible = true;
                upTwitter.Update();*/

                try
                {
                    int _TotalRecords = 0;
                    IIQAgent_TwitterResultsController _IIQAgent_TwitterResultsController = _ControllerFactory.CreateObject<IIQAgent_TwitterResultsController>();
                    List<IQAgent_TwitterResult> _ListOfIQAgent_TwitterResult = _IIQAgent_TwitterResultsController.GetIQAgentTwitterResultsBySearchRequestID(Convert.ToInt32(drpQueryName.SelectedValue), ucTwitterPager.PageSize, ucTwitterPager.CurrentPage.Value, _ViewstateInformation.SortExpressionTwitter, _ViewstateInformation.IsTwitterSortDirecitonAsc, out _TotalRecords);

                    if (_ListOfIQAgent_TwitterResult.Count > 0)
                    {
                        dlTweets.DataSource = _ListOfIQAgent_TwitterResult;
                        dlTweets.DataBind();
                        dlTweets.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;border:1px solid #999999;");
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ValidateDatalist", "ValidateDataList('" + dlTweets.ClientID + "');", true);
                        dlTweets.Visible = true;
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", "$(\"div[id='datalistInner']:last\").css(\"border\", \"none\");", true);

                        ucTwitterPager.Visible = true;
                        spnTwitterChartProgramFound.Visible = true;
                        lblTwitterChart.Visible = true;

                        btnTwiiterSort.Visible = true;
                        ddlTwitterSortExp.Visible = true;
                        rbTwitterSortDir.Visible = true;

                        //btnRemoveSM.Visible = true;
                        lblTwitterChart.Text = _TotalRecords.ToString("#,#");

                        ucTwitterPager.TotalRecords = _TotalRecords;
                        ucTwitterPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                        ucTwitterPager.BindDataList();

                        divNoResultTwitter.Visible = false;
                    }
                    else
                    {
                        btnTwiiterSort.Visible = false;
                        ddlTwitterSortExp.Visible = false;
                        rbTwitterSortDir.Visible = false;

                        ucTwitterPager.Visible = false;
                        spnTwitterChartProgramFound.Visible = false;
                        lblTwitterChart.Visible = false;
                        divNoResultTwitter.Visible = true;
                        dlTweets.Visible = false;
                        //btnRemoveSM.Visible = false;
                    }

                    upTwitter.Update();

                }
                catch (Exception ex)
                {
                    this.WriteException(ex);
                    Response.Redirect(CommonConstants.CustomErrorPage);
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void GetSocialMediaResult()
        {
            try
            {
                int _TotalRecords = 0;
                IIQAgent_SMResultsController _IIQAgent_SMResultsController = _ControllerFactory.CreateObject<IIQAgent_SMResultsController>();
                List<IQAgent_SMResult> _ListOfIQAgent_SMResult = _IIQAgent_SMResultsController.GetIQAgentSMResultsBySearchRequestID(Convert.ToInt32(drpQueryName.SelectedValue), ucSMPager.PageSize, ucSMPager.CurrentPage.Value, _ViewstateInformation.SortDirectionSocialMedia, _ViewstateInformation.IsSocialMediaSortDirecitonAsc, out _TotalRecords);

                gvSocialMedia.DataSource = _ListOfIQAgent_SMResult;
                gvSocialMedia.DataBind();

                lblSMChart.Text = _TotalRecords.ToString("#,#");



                if (_ListOfIQAgent_SMResult.Count > 0)
                {
                    ucSMPager.Visible = true;
                    spnSMChartProgramFound.Visible = true;
                    lblSMChart.Visible = true;
                    btnRemoveSM.Visible = true;

                    ucSMPager.TotalRecords = _TotalRecords;
                    ucSMPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                    ucSMPager.BindDataList();
                }
                else
                {
                    ucSMPager.Visible = false;
                    spnSMChartProgramFound.Visible = false;
                    lblSMChart.Visible = false;
                    btnRemoveSM.Visible = false;
                }

                upSocialMedia.Update();

                SortClipDirection(_ViewstateInformation.SortExpressionSocialMedia, _ViewstateInformation.IsSocialMediaSortDirecitonAsc, gvSocialMedia);
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void GetNewsResult()
        {
            try
            {
                int _TotalRecords = 0;
                IIQAgent_NMResultsController _IIQAgent_NMResultsController = _ControllerFactory.CreateObject<IIQAgent_NMResultsController>();
                List<IQAgent_NMResult> _ListOfIQAgent_NMResult = _IIQAgent_NMResultsController.GetIQAgentNMResultsBySearchRequestID(Convert.ToInt32(drpQueryName.SelectedValue), ucOnlineNewsPager.PageSize, ucOnlineNewsPager.CurrentPage.Value, _ViewstateInformation.SortDirectionOnlineNews, _ViewstateInformation.IsOnlineNewsSortDirecitonAsc, out _TotalRecords);

                gvOnlineNews.DataSource = _ListOfIQAgent_NMResult;
                gvOnlineNews.DataBind();

                lblNewsChart.Text = _TotalRecords.ToString("#,#");



                if (_ListOfIQAgent_NMResult.Count > 0)
                {
                    ucOnlineNewsPager.Visible = true;
                    spnNewsChartProgramFound.Visible = true;
                    lblNewsChart.Visible = true;
                    btnRemoveNM.Visible = true;

                    ucOnlineNewsPager.TotalRecords = _TotalRecords;
                    ucOnlineNewsPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                    ucOnlineNewsPager.BindDataList();
                }
                else
                {
                    ucOnlineNewsPager.Visible = false;
                    spnNewsChartProgramFound.Visible = false;
                    lblNewsChart.Visible = false;
                    btnRemoveNM.Visible = false;
                }

                SortClipDirection(_ViewstateInformation.SortExpressionOnlineNews, _ViewstateInformation.IsOnlineNewsSortDirecitonAsc, gvOnlineNews);

                upOnlineNews.Update();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void GetTVResult()
        {
            try
            {
                IQAgentResults _IQAgentResults = new IQAgentResults();
                string _SearchTerm = string.Empty;
                int _TotalRecords = 0;


                _IQAgentResults.PageNo = ucCustomPager.CurrentPage.HasValue ? ucCustomPager.CurrentPage.Value : 0;
                _IQAgentResults.PageSize = ucCustomPager.PageSize;
                _IQAgentResults.SearchRequestID = Convert.ToInt32(drpQueryName.SelectedValue);
                _IQAgentResults.IsAscending = (_ViewstateInformation.IsSortDirecitonAsc != null) ? _ViewstateInformation.IsSortDirecitonAsc : false;
                _IQAgentResults.SortField = _ViewstateInformation.SortExpression;

                IIQAgentResultsController _IIQAgentResultsController = _ControllerFactory.CreateObject<IIQAgentResultsController>();
                List<IQAgentResults> _ListOfIQAgentResultsFromDB = _IIQAgentResultsController.SelectForParentChildRelationship(_IQAgentResults, out _TotalRecords);

                //txtSearchMediaText.Text = _SearchTerm;

                foreach (IQAgentResults _IQAgentResultsObj in _ListOfIQAgentResultsFromDB)
                {
                    if (File.Exists(Server.MapPath("~/StationLogoImages/" + _IQAgentResultsObj.Rl_Station + ".gif")))
                    {
                        _IQAgentResultsObj.StationLogo = "http://" + Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _IQAgentResultsObj.Rl_Station + ".gif";
                    }
                    else if (File.Exists(Server.MapPath("~/StationLogoImages/" + _IQAgentResultsObj.Rl_Station + ".jpg")))
                    {
                        _IQAgentResultsObj.StationLogo = "http://" + Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _IQAgentResultsObj.Rl_Station + ".jpg";
                    }
                }


                rptTV.DataSource = _ListOfIQAgentResultsFromDB;
                rptTV.DataBind();

                lblNoOfRadioRawMedia.Text = _TotalRecords.ToString("#,#");


                if (rptTV.Items.Count > 0)
                {
                    btnRemove.Visible = true;
                    ucCustomPager.Visible = true;
                    rptTV.Visible = true;
                    divNoResults.Visible = false;
                    spnTotalProgramHeader.Visible = true;
                    lblNoOfRadioRawMedia.Visible = true;

                    ucCustomPager.TotalRecords = _TotalRecords;
                    ucCustomPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                    ucCustomPager.BindDataList();
                }
                else
                {
                    btnRemove.Visible = false;
                    ucCustomPager.Visible = false;
                    rptTV.Visible = false;
                    divNoResults.Visible = true;
                    spnTotalProgramHeader.Visible = false;
                    lblNoOfRadioRawMedia.Visible = false;
                }

                SortClipDirection(_ViewstateInformation.SortExpression, _ViewstateInformation.IsSortDirecitonAsc, rptTV);

                upTVGrid.Update();
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        private int GetGridToBind()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfcurrentTab.Value))
                {
                    switch (hfcurrentTab.Value)
                    {
                        case "0":
                            if (_ViewstateInformation.IsIQAgentTVResultShow)
                                return 0;
                            break;
                        case "1":
                            if (_ViewstateInformation.IsIQAgentNMResultShow)
                                return 1;
                            break;
                        case "2":
                            if (_ViewstateInformation.IsIQAgentSMResultShow)
                                return 2;
                            break;
                        case "3":
                            if (_ViewstateInformation.IsIQAgentTwitterResultShow)
                                return 3;
                            break;
                    }
                }

                if (_ViewstateInformation.IsIQAgentTVResultShow)
                    return 0;
                else if (_ViewstateInformation.IsIQAgentNMResultShow)
                    return 1;
                else if (_ViewstateInformation.IsIQAgentSMResultShow)
                    return 2;
                else if (_ViewstateInformation.IsIQAgentTwitterResultShow)
                    return 3;

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

            return 0;
        }

        private void BindNotification()
        {
            try
            {
                List<IQNotificationSettings> _ListOfIQNotificationSettings = new List<IQNotificationSettings>();
                //string _Result = string.Empty;

                IIQNotificationSettingsController _IIQNotificationSettingsController = _ControllerFactory.CreateObject<IIQNotificationSettingsController>();
                IQNotificationSettings _IQNotificationSettings = new IQNotificationSettings();
                _IQNotificationSettings.SearchRequestID = Convert.ToInt32(drpQueryName.SelectedValue);
                _ListOfIQNotificationSettings = _IIQNotificationSettingsController.GetIQNotificationSettings(_IQNotificationSettings);
                gvIQNotification.DataSource = _ListOfIQNotificationSettings;
                gvIQNotification.DataBind();
                if (_ListOfIQNotificationSettings.Count > 0)
                {
                    btnRemoveNotification.Visible = true;
                }
                else
                {
                    btnRemoveNotification.Visible = false;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void ClearFields()
        {
            try
            {
                txtEmail.Text = "";
                drpOption.SelectedIndex = 0;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindTwitterSortDropDown()
        {
            try
            {
                Dictionary<string, string> _TwitterSortIterms = null;
                string ConfigTwitterSortSettings = ConfigurationManager.AppSettings["TwitterSortSettingsiQAgent"];
                _TwitterSortIterms = ConfigTwitterSortSettings.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(part => part.Split('='))
                   .ToDictionary(split => split[0], split => split[1]);

                ddlTwitterSortExp.DataSource = _TwitterSortIterms;
                ddlTwitterSortExp.DataTextField = "Value";
                ddlTwitterSortExp.DataValueField = "Key";
                ddlTwitterSortExp.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindFrequencyDropDown(DropDownList drp)
        {
            try
            {
                drp.Items.Clear();
                drp.Items.Insert(0, new ListItem(NotificationFrequency.Immediate.ToString(), "0"));
                drp.Items.Insert(1, new ListItem(NotificationFrequency.Hourly.ToString(), "1"));
                drp.Items.Insert(2, new ListItem(NotificationFrequency.OnceDay.ToString(), "2"));
                drp.Items.Insert(3, new ListItem(NotificationFrequency.OnceWeek.ToString(), "3"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Clip Events

        protected void LbtnRawMediaPlay_Command(object sender, CommandEventArgs e)
        {
            try
            {
                IframeRawMediaH.RawMediaID = new Guid(Convert.ToString(e.CommandArgument));
                IframeRawMediaH.IsUGC = false;
                IframeRawMediaH.SearchTerm = txtSearchMediaText.Text;

                IframeRawMediaH.InitializePlayer();

                upVideo.Update();

                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "ShowTVTab", "displayTVTab();", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "SetWidthOfPlayerPopup", "SetPlayerPopupWidth();", true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "ShowPlayerPopup", "ShowModal('diviframe');", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowPlayerPopup", "$('#btnOpenPlayerPopup').click();", true);


            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void rptTV_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlImage _ExpandCollapseImg = e.Item.FindControl("ExpandImg") as HtmlImage;
                    if (_ExpandCollapseImg != null)
                    {
                        Panel _Panel = e.Item.FindControl("pnlGridChildIQAgentResults") as Panel;
                        if (_Panel != null)
                        {
                            _ExpandCollapseImg.Attributes.Add("onclick", "expandcollapse('" + _Panel.ClientID + "',this);");
                        }
                    }

                    IQAgentResults _IQAgentResults = e.Item.DataItem as IQAgentResults;

                    if (_IQAgentResults != null)
                    {
                        List<IQAgentResults> _ListOfIQAgentResults = _IQAgentResults.ChildResults;

                        if (_ListOfIQAgentResults.Count > 0)
                        {
                            foreach (IQAgentResults _IQAgentResultsObj in _ListOfIQAgentResults)
                            {
                                if (File.Exists(Server.MapPath("~/StationLogoImages/" + _IQAgentResultsObj.Rl_Station + ".gif")))
                                {
                                    _IQAgentResultsObj.StationLogo = "http://" + Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _IQAgentResultsObj.Rl_Station + ".gif";
                                }
                                else if (File.Exists(Server.MapPath("~/StationLogoImages/" + _IQAgentResultsObj.Rl_Station + ".jpg")))
                                {
                                    _IQAgentResultsObj.StationLogo = "http://" + Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _IQAgentResultsObj.Rl_Station + ".jpg";
                                }
                            }

                            Repeater _GridViewChild = e.Item.FindControl("rptChildTV") as Repeater;

                            if (_GridViewChild != null)
                            {
                                _GridViewChild.DataSource = _ListOfIQAgentResults;
                                _GridViewChild.DataBind();
                            }
                        }
                        else
                        {
                            Repeater _GridViewChild = e.Item.FindControl("rptChildTV") as Repeater;
                            HtmlImage _HtmlImage = e.Item.FindControl("ExpandImg") as HtmlImage;
                            AjaxControlToolkit.CollapsiblePanelExtender _CollapsiblePanelExtender = e.Item.FindControl("cpnlextIQAgentResults") as AjaxControlToolkit.CollapsiblePanelExtender;

                            if (_GridViewChild != null)
                            {
                                _GridViewChild.Visible = false;
                            }

                            if (_HtmlImage != null)
                            {
                                _HtmlImage.Visible = false;
                            }

                            if (_CollapsiblePanelExtender != null)
                            {
                                _CollapsiblePanelExtender.Enabled = false;
                            }
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

        #region Notification Events

        protected void btnRemoveNotification_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;
                for (int Count = 0; Count < gvIQNotification.Rows.Count; Count++)
                {
                    //CheckBox chkDelete = (CheckBox)grvRawMedia.Rows[Count].FindControl("chkDelete");
                    HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)gvIQNotification.Rows[Count].FindControl("chkDelete");
                    HiddenField hfIQNotificationKey = (HiddenField)gvIQNotification.Rows[Count].FindControl("hfIQNotificationKey");
                    if (chkDelete.Checked == true)
                    {
                        strKeys = strKeys + chkDelete.Value + ",";
                    }
                }
                if (strKeys.Length > 0)
                {
                    strKeys = strKeys.Substring(0, strKeys.Length - 1);
                    string _Result = string.Empty;
                    IIQNotificationSettingsController _IIQNotificationSettingsController = _ControllerFactory.CreateObject<IIQNotificationSettingsController>();
                    _Result = _IIQNotificationSettingsController.DeleteNotificationSettings(strKeys);
                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessDelete.Text = "Record(s) deleted Successfully.";
                        //BindRawMediaGrid();
                        BindNotification();
                    }
                    else
                    {
                        //BindRawMediaGrid();
                        BindNotification();
                    }
                }
                ShowCurrentTab();
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string _Result = string.Empty;
                IIQNotificationSettingsController _IIQNotificationSettingsController = _ControllerFactory.CreateObject<IIQNotificationSettingsController>();
                IQNotificationSettings _IQNotificationSettings = new IQNotificationSettings();
                _IQNotificationSettings.TypeofEntry = NotificationType.Email.ToString();
                _IQNotificationSettings.Notification_Address = txtEmail.Text;
                _IQNotificationSettings.Frequency = Convert.ToString(drpOption.SelectedItem);
                _IQNotificationSettings.SearchRequestID = Convert.ToInt32(drpQueryName.SelectedValue);
                _Result = _IIQNotificationSettingsController.InsertNotificationSettings(_IQNotificationSettings);
                if (_Result == "-2")
                {
                    lblNotificationError.Text = _ErrorMessage;
                    lblNotificationError.Visible = true;
                }
                else if (!string.IsNullOrEmpty(_Result) && _Result != "-1")
                {
                    gvIQNotification.EditIndex = -1;
                    BindNotification();
                    ClearFields();
                }
                else if (string.IsNullOrEmpty(_Result))
                {
                    lblNotificationError.Text = "";
                }
                else
                {
                    lblNotificationError.Text = "IQNotification limit exceeds.";
                }
                ShowCurrentTab();
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #region Notification Grid Events

        protected void gvIQNotification_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                Label lblEmail = (Label)gvIQNotification.Rows[e.NewEditIndex].FindControl("lblEmail");
                _Email = lblEmail.Text;

                Label lblFrequency = (Label)gvIQNotification.Rows[e.NewEditIndex].FindControl("lblFrequency");
                _Frequency = lblFrequency.Text;

                gvIQNotification.EditIndex = e.NewEditIndex;
                //BindClientRole();
                //ApplyGridHeaderFilter();
                BindNotification();
                ShowCurrentTab();

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvIQNotification_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvIQNotification.EditIndex = -1;
                //BindClientRole();
                //ApplyGridHeaderFilter();
                BindNotification();
                ShowCurrentTab();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvIQNotification_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvIQNotification_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                DropDownList drpOption = (DropDownList)gvIQNotification.Rows[e.RowIndex].FindControl("drpOption");
                TextBox txtEmail = (TextBox)gvIQNotification.Rows[e.RowIndex].FindControl("txtEmail");
                HiddenField hdnIQMotificationKey = (HiddenField)gvIQNotification.Rows[e.RowIndex].FindControl("hdnIQMotificationKey");

                string _Result = string.Empty;
                IIQNotificationSettingsController _IIQNotificationSettingsController = _ControllerFactory.CreateObject<IIQNotificationSettingsController>();
                IQNotificationSettings _IQNotificationSettings = new IQNotificationSettings();

                _IQNotificationSettings.Notification_Address = txtEmail.Text;
                _IQNotificationSettings.Frequency = Convert.ToString(drpOption.SelectedItem);
                _IQNotificationSettings.IQNotificationKey = Convert.ToInt32(hdnIQMotificationKey.Value);
                _IQNotificationSettings.SearchRequestID = Convert.ToInt32(drpQueryName.SelectedValue);
                _Result = _IIQNotificationSettingsController.UpdateNotificationSettings(_IQNotificationSettings);
                if (!string.IsNullOrEmpty(_Result) && _Result == "-1")
                {
                    //gvCustomer.EditIndex = -1;
                    lblErrorMessage.Text = _ErrorMessage;
                    lblErrorMessage.Visible = true;
                    e.Cancel = true;
                    return;
                }
                else
                {
                    gvIQNotification.EditIndex = -1;
                    BindNotification();
                    //ApplyGridHeaderFilter();
                    lblErrorMessage.Visible = false;
                }
                ShowCurrentTab();
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        private void ShowCurrentTab()
        {
            if (!string.IsNullOrWhiteSpace(hfcurrentTab.Value))
            {
                switch (hfcurrentTab.Value)
                {
                    case "0":
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowTVTab", "displayTVTab();", true);
                        break;
                    case "1":
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowOnlineNewsTab", "displayOnlineNewsTab();", true);
                        break;
                    case "2":
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowSMTab", "displaySocialMediaTab();", true);
                        break;
                    case "3":
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "displayTwitterTab();", true);
                        break;
                }

            }
            else
            {
                string _Script = "$('#divResult').hide();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HidedivResult", _Script, true);
            }
        }

        #endregion

        #region paging

        protected void ucCustomPager_PageIndexChange(object sender, EventArgs e)
        {
            try
            {
                GetTVResult();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowTVTab", "displayTVTab();", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void ucOnlineNewsPager_PageIndexChange(object sender, EventArgs e)
        {
            try
            {
                GetNewsResult();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setOLTab", "ChangeTab('tabOnlineNews','divGridTab',1,1);", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
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
                GetSocialMediaResult();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setSMTab", "ChangeTab('tabSocialMedia','divGridTab',2,1);", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
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
                GetTwitterResult();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "displayTwitterTab();", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }


        #endregion

        #region Sorting

        protected void SortClipDirection(string sortField, bool sortDirectionAsc, Repeater rptView)
        {
            try
            {
                if (rptView.HasControls())
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
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void SortClipDirection(string sortField, bool sortDirectionAsc, GridView gvView)
        {


            try
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
                    upTVGrid.Update();
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
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void GvOnlineNews_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_ViewstateInformation.SortExpressionOnlineNews))
                {
                    if (_ViewstateInformation.SortExpressionOnlineNews.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_ViewstateInformation.IsOnlineNewsSortDirecitonAsc == true)
                        {
                            _ViewstateInformation.IsOnlineNewsSortDirecitonAsc = false;
                        }
                        else
                        {
                            _ViewstateInformation.IsOnlineNewsSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _ViewstateInformation.SortExpressionOnlineNews = e.SortExpression;
                        _ViewstateInformation.IsOnlineNewsSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _ViewstateInformation.SortExpressionOnlineNews = e.SortExpression;
                    _ViewstateInformation.IsOnlineNewsSortDirecitonAsc = true;
                }

                SetViewstateInformation(_ViewstateInformation);
                ucOnlineNewsPager.CurrentPage = 0;
                GetNewsResult();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "shwOLTab", "displayOnlineNewsTab();", true);
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void GvSocialMedia_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_ViewstateInformation.SortExpressionSocialMedia))
                {
                    if (_ViewstateInformation.SortExpressionSocialMedia.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_ViewstateInformation.IsSocialMediaSortDirecitonAsc == true)
                        {
                            _ViewstateInformation.IsSocialMediaSortDirecitonAsc = false;
                        }
                        else
                        {
                            _ViewstateInformation.IsSocialMediaSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _ViewstateInformation.SortExpressionSocialMedia = e.SortExpression;
                        _ViewstateInformation.IsSocialMediaSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _ViewstateInformation.SortExpressionSocialMedia = e.SortExpression;
                    _ViewstateInformation.IsSocialMediaSortDirecitonAsc = true;
                }

                SetViewstateInformation(_ViewstateInformation);
                ucSMPager.CurrentPage = 0;
                GetSocialMediaResult();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "shwSMTab", "displaySocialMediaTab();", true);


            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void rptTV_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "SORT")
                {
                    if (!string.IsNullOrEmpty(_ViewstateInformation.SortExpression))
                    {
                        if (_ViewstateInformation.SortExpression.ToLower() == e.CommandArgument.ToString().ToLower())
                        {
                            if (_ViewstateInformation.IsSortDirecitonAsc == true)
                            {
                                _ViewstateInformation.IsSortDirecitonAsc = false;
                            }
                            else
                            {
                                _ViewstateInformation.IsSortDirecitonAsc = true;
                            }
                        }
                        else
                        {
                            _ViewstateInformation.SortExpression = e.CommandArgument.ToString();
                            _ViewstateInformation.IsSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _ViewstateInformation.SortExpression = e.CommandArgument.ToString();
                        _ViewstateInformation.IsSortDirecitonAsc = true;
                    }

                    SetViewstateInformation(_ViewstateInformation);
                    ucCustomPager.CurrentPage = 0;

                    GetTVResult();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setTVTab", "ChangeTab('tabTV','divGridTab',0,1);", true);
                }

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnTwiiterSort_Click(object sender, EventArgs e)
        {
            try
            {
                _ViewstateInformation.SortExpressionTwitter = ddlTwitterSortExp.SelectedValue;
                _ViewstateInformation.IsTwitterSortDirecitonAsc = Convert.ToBoolean(rbTwitterSortDir.SelectedValue);
                ucTwitterPager.CurrentPage = 0;
                SetViewstateInformation(_ViewstateInformation);
                GetTwitterResult();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "displayTwitterTab();", true);
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Drop Down Events

        protected void drpQueryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (drpQueryName.SelectedIndex == 0)
                {
                    pnlNotification.Style.Add("display", "none");

                    _ViewstateInformation.IsIQAgentTVResultShow = false;
                    _ViewstateInformation.IsIQAgentNMResultShow = false;
                    _ViewstateInformation.IsIQAgentSMResultShow = false;
                    _ViewstateInformation.IsIQAgentTwitterResultShow = false;
                    SetViewstateInformation(_ViewstateInformation);

                    SetTabVisiBility();

                    string _Script = "$('#" + hfcurrentTab.ClientID + "').val('');";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ResetTab", _Script, true);


                }
                else
                {
                    txtEmail.Text = String.Empty;
                    drpOption.SelectedIndex = 0;
                    pnlNotification.Style.Add("display", "block");

                    ISearchRequestController _ISearchRequestController = _ControllerFactory.CreateObject<ISearchRequestController>();
                    List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = _ISearchRequestController.GetSearchRequestsByClientIDQueryName(new Guid(_SessionInformation.ClientGUID), drpQueryName.SelectedItem.Text);
                    if (_ListOfIQAgentSearchRequest != null && _ListOfIQAgentSearchRequest.Count > 0 && !string.IsNullOrWhiteSpace(_ListOfIQAgentSearchRequest[0].SearchTerm))
                    {
                        ParseSearchRequestToShowTabs(_ListOfIQAgentSearchRequest[0].SearchTerm);
                        SetTabVisiBility();

                        GetDataFromDB();

                        pnlNotification.Visible = true;
                        gvIQNotification.EditIndex = -1;
                        BindNotification();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowDiv", "showDivBox();", true);

                        //UpdateUpdatePanel(upGrid);
                        UpdateUpdatePanel(upRawMediaClip);
                    }
                    else
                    {
                        lblRawMediaMsg.Text = "Invalid Request";
                    }
                }
            }
            catch (Exception _Exception)
            {
                base.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void drpOption_PreRender(object sender, EventArgs e)
        {
            try
            {
                DropDownList drpOption = sender as DropDownList;
                BindFrequencyDropDown(drpOption);
                if (_Frequency == NotificationFrequency.Immediate.ToString())
                {
                    drpOption.SelectedIndex = 0;
                }
                else if (_Frequency == NotificationFrequency.Hourly.ToString())
                {
                    drpOption.SelectedIndex = 1;
                }
                else if (_Frequency == NotificationFrequency.OnceDay.ToString())
                {
                    drpOption.SelectedIndex = 2;
                }
                else if (_Frequency == NotificationFrequency.OnceWeek.ToString())
                {
                    drpOption.SelectedIndex = 3;
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Validations

        protected void ClusterValidator_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            string value = e.ValueToConvert as string;
            try
            {
                if (value == "-1")
                {
                    e.ConvertedValue = "";
                }
                else
                {
                    e.ConvertedValue = "0";
                }
            }
            catch (Exception)
            {
                e.ConversionErrorMessage = "Please select Station.";
                e.ConvertedValue = null;
            }
        }
        #endregion

        #region Show Article

        protected void gvOnlineNews_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ShowArticle")
                {
                    string ArticleID = (((e.CommandSource as ImageButton).NamingContainer as GridViewRow).FindControl("hdnArticleID") as HiddenField).Value;
                    DateTime Harvest_Time = Convert.ToDateTime(((e.CommandSource as ImageButton).NamingContainer as GridViewRow).Cells[3].Text);
                    hdnSaveArticleID.Value = ArticleID;
                    hdnArticleType.Value = "NM";
                    upSaveArticle.Update();


                    _ViewstateInformation.ArticleUrl = Convert.ToString(e.CommandArgument);
                    _ViewstateInformation.Harvest_Time = Harvest_Time;
                    SetViewstateInformation(_ViewstateInformation);

                    iFrameOnlineNewsArticle.Attributes.Add("src", _ViewstateInformation.ArticleUrl);


                    iFrameOnlineNewsArticle.Visible = true;


                    upOnlineNewsArticle.Update();

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowOnlineNewsTab", "displayOnlineNewsTab();", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "ShowArticlePopup", "ShowModal('diviFrameOnlineNewsArticle');", true);
                }
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvSocialMedia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ShowArticle")
                {
                    string ArticleID = (((e.CommandSource as ImageButton).NamingContainer as GridViewRow).FindControl("hdnSMArticleID") as HiddenField).Value;
                    DateTime Harvest_Time = Convert.ToDateTime(((e.CommandSource as ImageButton).NamingContainer as GridViewRow).Cells[3].Text);
                    hdnSaveArticleID.Value = ArticleID;
                    hdnArticleType.Value = "SM";
                    upSaveArticle.Update();

                    _ViewstateInformation.ArticleUrl = Convert.ToString(e.CommandArgument);
                    _ViewstateInformation.Harvest_Time = Harvest_Time;
                    SetViewstateInformation(_ViewstateInformation);

                    iFrameOnlineNewsArticle.Attributes.Add("src", _ViewstateInformation.ArticleUrl);


                    iFrameOnlineNewsArticle.Visible = true;

                    upOnlineNewsArticle.Update();

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowSMTab", "displaySocialMediaTab();", true);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "ShowArticlePopup", "ShowModal('diviFrameOnlineNewsArticle');", true);
                }
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Remove Article and Clip

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;

                for (int Count = 0; Count < rptTV.Items.Count; Count++)
                {
                    //CheckBox chkDelete = (CheckBox)grvRawMedia.Rows[Count].FindControl("chkDelete");
                    HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)rptTV.Items[Count].FindControl("ChkDelete");
                    HiddenField hfIQAgentResultsKey = (HiddenField)rptTV.Items[Count].FindControl("hfIQAgentResultsKey");

                    Repeater _RepeaterChild = (Repeater)rptTV.Items[Count].FindControl("rptChildTV");

                    if (chkDelete.Checked == true)
                    {
                        if (strKeys == string.Empty)
                        {
                            strKeys = strKeys + chkDelete.Value;
                        }
                        else
                        {
                            strKeys = strKeys + CommonConstants.Comma + chkDelete.Value;
                        }

                        if (_RepeaterChild != null)
                        {
                            foreach (RepeaterItem _RepeaterItem in _RepeaterChild.Items)
                            {
                                HtmlInputCheckBox _Checkbox = (HtmlInputCheckBox)_RepeaterItem.FindControl("ChkDelete");

                                if (strKeys == string.Empty)
                                {
                                    strKeys = strKeys + _Checkbox.Value;
                                }
                                else
                                {
                                    strKeys = strKeys + CommonConstants.Comma + _Checkbox.Value;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_RepeaterChild != null)
                        {
                            foreach (RepeaterItem _RepeaterItem in _RepeaterChild.Items)
                            {
                                HtmlInputCheckBox _Checkbox = (HtmlInputCheckBox)_RepeaterItem.FindControl("ChkDelete");

                                if (_Checkbox.Checked == true)
                                {
                                    if (strKeys == string.Empty)
                                    {
                                        strKeys = strKeys + _Checkbox.Value;
                                    }
                                    else
                                    {
                                        strKeys = strKeys + CommonConstants.Comma + _Checkbox.Value;
                                    }
                                }
                            }
                        }
                    }
                }

                if (strKeys.Length > 0)
                {
                    string _Result = string.Empty;
                    IIQAgentResultsController _IIQAgentResultsController = _ControllerFactory.CreateObject<IIQAgentResultsController>();
                    _Result = _IIQAgentResultsController.DeleteIQAgentResult(strKeys);

                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessMessage.Text = "Record(s) deleted Successfully.";

                        GetTVResult();
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowTVTab", "displayTVTab();", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnRemoveNM_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;
                for (int Count = 0; Count < gvOnlineNews.Rows.Count; Count++)
                {
                    HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)gvOnlineNews.Rows[Count].FindControl("ChkDelete");

                    if (chkDelete.Checked == true)
                    {
                        if (strKeys == string.Empty)
                        {
                            strKeys = strKeys + chkDelete.Value;
                        }
                        else
                        {
                            strKeys = strKeys + CommonConstants.Comma + chkDelete.Value;
                        }
                    }
                }

                if (strKeys.Length > 0)
                {
                    string _Result = string.Empty;
                    IIQAgent_NMResultsController _IIQAgent_SMResultsController = _ControllerFactory.CreateObject<IIQAgent_NMResultsController>();
                    _Result = _IIQAgent_SMResultsController.DeleteIQAgent_NMResults(strKeys);

                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessMessageNM.Text = "Record(s) deleted Successfully.";

                        GetNewsResult();
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowOnlineNewsTab", "displayOnlineNewsTab();", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnRemoveSM_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;
                for (int Count = 0; Count < gvSocialMedia.Rows.Count; Count++)
                {
                    HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)gvSocialMedia.Rows[Count].FindControl("ChkDelete");

                    if (chkDelete.Checked == true)
                    {
                        if (strKeys == string.Empty)
                        {
                            strKeys = strKeys + chkDelete.Value;
                        }
                        else
                        {
                            strKeys = strKeys + CommonConstants.Comma + chkDelete.Value;
                        }
                    }
                }

                if (strKeys.Length > 0)
                {
                    string _Result = string.Empty;
                    IIQAgent_SMResultsController _IIQAgent_SMResultsController = _ControllerFactory.CreateObject<IIQAgent_SMResultsController>();
                    _Result = _IIQAgent_SMResultsController.DeleteIQAgent_SMResults(strKeys);

                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessMessageSM.Text = "Record(s) deleted Successfully.";

                        GetSocialMediaResult();
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowSMTab", "displaySocialMediaTab();", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ShowHIde", "$(\"#divResult\").show();ShowHideDivResult('divResult',false);", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Save Article

        protected void btnSaveArticle_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnArticleType.Value == "NM")
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
                    _ArchiveNM.ArticleID = hdnSaveArticleID.Value;
                    _ArchiveNM.Url = _ViewstateInformation.ArticleUrl;
                    _ArchiveNM.Harvest_Time = _ViewstateInformation.Harvest_Time;
                    _ArchiveNM.Rating = Convert.ToInt16(txtArticleRate.Text);


                    Uri newsSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrNewsUrl]);
                    SearchEngine _SearchEngine = new SearchEngine(newsSearchURI);
                    SearchNewsRequest _SearchNewsRequest = new SearchNewsRequest();
                    _SearchNewsRequest.IsShowContent = true;
                    _SearchNewsRequest.IDs = new List<String> { _ArchiveNM.ArticleID };
                    SearchNewsResults _searchNewsResults = _SearchEngine.SearchNews(_SearchNewsRequest);
                    if (_searchNewsResults.newsResults != null && _searchNewsResults.newsResults.Count > 0)
                    {
                        _ArchiveNM.Content = _searchNewsResults.newsResults[0].Content;
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
                        lblSaveArticleMsg.Visible = true;
                        //mdlpopupSaveArticle.Show();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticleModal", "ShowModal('pnlSaveArticle');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Article Saved Successfully');", true);
                        SetControlforPopUp(0);
                        var newsGeneratePDFsvc = new NewsGeneratePDFWebServiceClient();
                        newsGeneratePDFsvc.WakeupService();
                        //mdlpopupSaveArticle.Hide();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticleModal", "closeModal('pnlSaveArticle');", true);

                        //lblNewsMsg.Text = "Article Saved Successfully.";
                        //upOnlineNews.Update();
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showOLTab", "displayOnlineNewsTab();", true);
                }
                else if (hdnArticleType.Value == "SM")
                {
                    ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
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
                    _ArchiveSM.ArticleID = hdnSaveArticleID.Value;
                    _ArchiveSM.Url = _ViewstateInformation.ArticleUrl;
                    _ArchiveSM.Harvest_Time = _ViewstateInformation.Harvest_Time;
                    _ArchiveSM.Rating = Convert.ToInt16(txtArticleRate.Text);

                    Uri newsSearchURI = new Uri(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrSMUrl]);
                    SearchEngine _SearchEngine = new SearchEngine(newsSearchURI);
                    SearchSMRequest _SearchSMRequest = new SearchSMRequest();
                    _SearchSMRequest.isShowContent = true;
                    _SearchSMRequest.ids = new List<String> { _ArchiveSM.ArticleID };
                    SearchSMResult _SearchSMResult = _ISocialMediaController.GetSocialMediaGridData(_SearchSMRequest, _SearchEngine);
                    if (_SearchSMResult.smResults != null && _SearchSMResult.smResults.Count > 0)
                    {
                        _ArchiveSM.Content = _SearchSMResult.smResults[0].content;
                    }

                    string _Result = _ISocialMediaController.InsertArchiveSM(_ArchiveSM);

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
                        lblSaveArticleMsg.Visible = true;
                        //mdlpopupSaveArticle.Show();
                        
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticleModal", "ShowModal('pnlSaveArticle');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Article Saved Successfully');", true);
                        SetControlforPopUp(0);
                        var socialGeneratePDFsvc = new SocialGeneratePDFWebServiceClient();
                        socialGeneratePDFsvc.WakeupService();
                        //mdlpopupSaveArticle.Hide();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticleModal", "closeModal('pnlSaveArticle');", true);
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showSMTab", "displaySocialMediaTab();", true);
                }
                else if (hdnArticleType.Value == "Twitter")
                {
                    IArchiveTweetsController _IArchiveTweetsController = _ControllerFactory.CreateObject<IArchiveTweetsController>();
                    ArchiveTweets _ArchiveTweets = new ArchiveTweets();

                    Guid? _NullCategoryGUID = null;

                    TwitterResult _TwitterResult = (TwitterResult)_ViewstateInformation.TweeterResult;

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
                        lblSaveArticleMsg.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticleModal", "ShowModal('pnlSaveArticle');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SavedSearchAlert", "alert('Tweet Saved Successfully');", true);                        
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideSaveArticleModal", "closeModal('pnlSaveArticle');", true);
                        SetControlforPopUp(1);
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "displayTwitterTab();", true);
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Twitter Datalist Event

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
                _ViewstateInformation.TweeterResult = _TweeterResult;
                SetViewstateInformation(_ViewstateInformation);
                txtArticleRate.Text = "1";
                hdnArticleType.Value = "Twitter";
                hdnSaveArticleID.Value = e.CommandArgument.ToString();
                SetControlforPopUp(1);

                upSaveArticle.Update();

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showTwitterTab", "displayTwitterTab();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenSaveArticlePopup", "OpenSaveArticlePopup(1);", true);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowSaveArticleModal", "ShowModal('pnlSaveArticle');", true);



            }
        }

        #endregion

        protected void SetControlforPopUp(int popupType)
        {
            //vlSummerySaveArticle.Visible = false;
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
            }
        }
        #region Save Tweet


        #endregion

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindDropDown();
            ResetAllTab();
        }

        public void ResetAllTab()
        {
            drpQueryName.SelectedIndex = 0;
            pnlNotification.Style.Add("display", "none");
            _ViewstateInformation.IsIQAgentTVResultShow = false;
            _ViewstateInformation.IsIQAgentNMResultShow = false;
            _ViewstateInformation.IsIQAgentSMResultShow = false;
            _ViewstateInformation.IsIQAgentTwitterResultShow = false;
            SetViewstateInformation(_ViewstateInformation);

            SetTabVisiBility();

            string _Script = "$('#" + hfcurrentTab.ClientID + "').val('');" +
                "$('#divResult').hide();";

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ResetTab", _Script, true);
            upRawMediaClip.Update();
        }
    }
}