using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Common;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using System.Configuration;
using System.Xml;
using System.Text.RegularExpressions;
using System.Net;

using System.Text;
using System.IO;
using System.Threading;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.MyIQ
{
    public partial class MyIQ : BaseControl
    {
        #region Member Variables

        public string _clipID = string.Empty;

        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        List<Caption> _ListOfRCatpion = null;
        //int _NoOfResultsFromClip = 21;
        ViewstateInformation _ViewstateInformation;
        SessionInformation _SessionInformation;

        #endregion

        #region page events

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                lblSearchErr.Text = "";
                lblNoResults.Visible = false;
                lblNoResults.Text = string.Empty;

                _SessionInformation = CommonFunctions.GetSessionInformation();

                if (_ViewstateInformation == null)
                    _ViewstateInformation = GetViewstateInformation();

                //_NoOfResultsFromClip = grvClip.PageSize + 1;
                //updProgressEditClip.AssociatedUpdatePanelID = upEditClip.UniqueID;
                if (!IsPostBack)
                {
                    txtYourEmail.Text = _SessionInformation.Email;

                    BindTwitterSortDropDown();

                    IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                    _ViewstateInformation.IsNielSenData = Convert.ToBoolean(_IClientRoleController.GetClientRoleByClientGUIDRoleName(new Guid(_SessionInformation.ClientGUID), RolesName.NielsenData.ToString()));
                    SetViewstateInformation(_ViewstateInformation);

                    GetCustomCategoryByClientGUID();
                    BindMediaCategoryCheckList();
                    BindCustomerCheckList();

                    //SetSearchParams();

                    MyIQSearchParams _SearchParams = new MyIQSearchParams();
                    _ViewstateInformation._MyIQSearchParams = _SearchParams;
                    SetViewstateInformation(_ViewstateInformation);

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "InitTab", "ChangeTab('ctl00_Content_Data_UCMyIQControl_tabTV','divGridTab','TV');", true);

                    if (_SessionInformation.IsDownloadClips == true)
                    {
                        btnClipDownload.Visible = true;
                    }
                    else
                    {
                        btnClipDownload.Visible = false;
                    }

                    BindArchiveClips(true);
                    tabTV.Visible = true;
                    divTVResult.Visible = true;

                    if (_SessionInformation.IsmyiQNM)
                    {
                        tabNews.Visible = true;
                        divNewsResult.Visible = true;
                    }
                    else
                    {
                        tabNews.Visible = false;
                        divNewsResult.Visible = false;
                        chkNews.Visible = false;
                        aOnlineNews.Visible = false;
                    }

                    if (_SessionInformation.IsmyiQSM)
                    {
                        tabSocialMedia.Visible = true;
                        divSocialMediaResult.Visible = true;
                    }
                    else
                    {
                        tabSocialMedia.Visible = false;
                        divSocialMediaResult.Visible = false;
                        chkSocialMedia.Visible = false;
                        aSocialMedia.Visible = false;
                    }

                    if (_SessionInformation.IsmyiQPM)
                    {
                        tabPrintMedia.Visible = true;
                        divPrintMediaResult.Visible = true;
                    }
                    else
                    {
                        tabPrintMedia.Visible = false;
                        divPrintMediaResult.Visible = false;
                        chkPrintMedia.Visible = false;
                        aPrintMedia.Visible = false;
                    }

                    if (_SessionInformation.IsMyIQTwitter)
                    {
                        tabTweet.Visible = true;
                        divTweetResult.Visible = true;
                    }
                    else
                    {
                        tabTweet.Visible = false;
                        divTweetResult.Visible = false;
                        chkTwitter.Visible = false;
                        aTweet.Visible = false;
                    }

                    //ShowHideSearchFilters(true);

                    hfCurrentTabIndex.Value = "0";

                    string _Script = "CheckUncheckAll('" + chkOwnerList.ClientID + "','" + chkUserAll.ClientID + "');CheckUncheckAll('divSearchTerm','" + chkSearchTermSelectAll.ClientID + "');"
                        + "ShowHideDivResult('divReportTVResult',true);ShowHideDivResult('divReportNewsResult',true);ShowHideDivResult('divReportSocialMediaResult',true);";


                    //+ "SetSearchParam();" + ;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetSearchParam", _Script, true);

                }

                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "InitTab", "$('#tabs').tabs();", true);


                if (Request["__EventTarget"] != null && Request["__EventArgument"] == "Tab")
                {
                    if (Request["__EventTarget"] == upTVGrid.ClientID)
                    {
                        //if (ValidateSearch())
                        //{
                        hfTVStatus.Value = "1";
                        hfCurrentTabIndex.Value = "0";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                        BindArchiveClips(true);

                        //}
                    }
                    else if (Request["__EventTarget"] == upNewsGrid.ClientID)
                    {
                        //if (ValidateSearch())
                        //{
                        hfNewsStatus.Value = "1";
                        hfCurrentTabIndex.Value = "1";

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                        BindArchiveNM();

                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "$('#tabs').tabs('select', 1);", true);
                        //}
                    }
                    else if (Request["__EventTarget"] == upSocialMediaGrid.ClientID)
                    {
                        //if (ValidateSearch())
                        //{
                        hfSocialMediaStatus.Value = "1";
                        hfCurrentTabIndex.Value = "2";

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                        BindArchiveSM();

                        //}
                    }
                    else if (Request["__EventTarget"] == upPrintMediaGrid.ClientID)
                    {
                        //if (ValidateSearch())
                        //{
                        hfPrintMediaStatus.Value = "1";
                        hfCurrentTabIndex.Value = "3";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                        BindArchivePM();

                        //}
                    }

                    else if (Request["__EventTarget"] == upTweetGrid.ClientID)
                    {
                        //if (ValidateSearch())
                        //{
                        spnTweetNoData.Visible = false;
                        hfTweetStatus.Value = "1";
                        hfCurrentTabIndex.Value = "4";

                        ddlTweetSortColumns.SelectedIndex = 0;
                        rdoTweetSort.SelectedIndex = 0;

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
                        BindArchiveTweets(false);

                        //}
                    }

                }

                //if (rdoSearch.Checked)
                //{
                    chkUserAll.Attributes.Add("onclick", "CheckUncheckAll('" + chkOwnerList.ClientID + "',this.id);");
                    chkSearchTermSelectAll.Attributes.Add("onclick", "CheckUncheckAll('divSearchTerm',this.id);");

                    chkOwnerList.Attributes.Add("onclick", "setCheckbox('" + chkOwnerList.ClientID + "','" + chkUserAll.ClientID + "')");
                    rdoCategoryOperator1.Attributes.Add("align", "center");
                    rdoCategoryOperator2.Attributes.Add("align", "center");
                    rdoCategoryOperator3.Attributes.Add("align", "center");

                    SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);
                    lblSuccessMessage.Text = string.Empty;
                    lblError.Text = string.Empty;
                    lblMsg.Text = string.Empty;
                    lblErrorClips.Text = string.Empty;
                    lblErrorClips.Visible = false;
                    UpdateUpdatePanel(upTVGrid);
                //}

                if (_SessionInformation.IsmyiQNM)
                {
                    //if (rdoSearch.Checked)
                    //{
                        SortClipDirection(_ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, grvArticle);
                        lblSuccessMessageArticle.Text = string.Empty;
                        lblErrorMsgArticle.Text = string.Empty;
                        lblErrorMsgArticle.Visible = false;
                        UpdateUpdatePanel(upNewsGrid);
                    //}
                }

                if (_SessionInformation.IsmyiQSM)
                {
                    //if (rdoSearch.Checked)
                    //{
                        SortClipDirection(_ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, grvSocialArticle);
                        lblSuccessMessageArticleSM.Text = string.Empty;
                        lblErrorMsgArticleSM.Text = string.Empty;
                        lblErrorMsgArticleSM.Visible = false;
                        UpdateUpdatePanel(upSocialMediaGrid);
                    //}
                }

                if (_SessionInformation.IsmyiQPM)
                {
                    //if (rdoSearch.Checked)
                    //{
                        SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, grvPrintMedia);
                        lblSuccessMessagePM.Text = string.Empty;
                        lblErrorMsgPM.Text = string.Empty;
                        lblErrorMsgPM.Visible = false;
                        UpdateUpdatePanel(upPrintMediaGrid);
                    //}
                }
                if (_SessionInformation.IsMyIQTwitter)
                {
                    //if (rdoSearch.Checked)
                    //{
                        lblSuccessMessageTweet.Text = string.Empty;
                        lblErrorMsgTweet.Text = string.Empty;
                        lblErrorMsgTweet.Visible = false;
                        UpdateUpdatePanel(upTweetGrid);
                    //}
                }


                if (Request.UserAgent.ToLower().Contains("android") && CheckVersion())
                {
                    this.Page.ClientScript.RegisterClientScriptInclude("script5", this.Page.ResolveClientUrl("~/Script/") + "AndroidPlayer.js");
                }

                ScriptManager.RegisterStartupScript(upMainGrid, upMainGrid.GetType(), "MyIQDocReady", "MyIQDocReady();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }


        protected void ShowHideSearchFilters(bool IsShowSearch)
        {
            divMainSearch.Visible = IsShowSearch;
            divMinGrid.Visible = IsShowSearch;
            divleftSearch.Visible = IsShowSearch;

            //pnlReport.Visible = !IsShowSearch;

            upMainGrid.Update();
            upMainSearch.Update();
            upBtnSearch.Update();

            //upReport.Update();
        }

        #endregion

        #region Clip Events

        #region Bind Clip


        private void BindArchiveClips(bool p_IsInitialization)
        {
            try
            {
                if (!chkTV.Checked)
                {
                    grvClip.DataSource = null;
                    grvClip.DataBind();
                    ucCustomPager.Visible = false;
                    return;
                }

                if (string.IsNullOrEmpty(_ViewstateInformation.ClipSortExpression))
                {
                    _ViewstateInformation.ClipSortExpression = "ClipCreationDate";

                }

                //if (_ViewstateInformation._CurrentClipPage == null)
                if (ucCustomPager.CurrentPage == null)
                {
                    ucCustomPager.CurrentPage = 0;
                }

                if (_ViewstateInformation.IsmyIQSearchActive == null)
                {
                    _ViewstateInformation.IsmyIQSearchActive = false;
                }

                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                int _TotalRecordsClipCount = 0;

                List<ArchiveClip> _ListOfArchiveClip = _IArchiveClipController.GetArchiveClipBySearchNew(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._MyIQSearchParams.SearchTitle, _ViewstateInformation._MyIQSearchParams.SearchDesc, _ViewstateInformation._MyIQSearchParams.SearchKey, _ViewstateInformation._MyIQSearchParams.SearchCC, _ViewstateInformation._MyIQSearchParams.FromDate, _ViewstateInformation._MyIQSearchParams.ToDate, _ViewstateInformation._MyIQSearchParams.CategoryGUID1, _ViewstateInformation._MyIQSearchParams.CategoryGUID2, _ViewstateInformation._MyIQSearchParams.CategoryGUID3, _ViewstateInformation._MyIQSearchParams.CategoryGUID4, _ViewstateInformation._MyIQSearchParams.CategoryOperator1, _ViewstateInformation._MyIQSearchParams.CategoryOperator2, _ViewstateInformation._MyIQSearchParams.CategoryOperator3, _ViewstateInformation._MyIQSearchParams.CustomerGUID, ucCustomPager.CurrentPage.Value, ucCustomPager.PageSize, _ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, _ViewstateInformation.IsNielSenData, out _TotalRecordsClipCount);

                _ViewstateInformation.TotalRecordsCountClip = _TotalRecordsClipCount;

                SetViewstateInformation(_ViewstateInformation);



                bool _HasRecords = true;

                if (p_IsInitialization == false && (_ListOfArchiveClip == null || _ListOfArchiveClip.Count == 0))
                {
                    _HasRecords = false;
                    _ListOfArchiveClip = new List<ArchiveClip>();
                }

                grvClip.Visible = true;

                grvClip.DataSource = _ListOfArchiveClip;
                grvClip.DataBind();

                if (!_ViewstateInformation.IsNielSenData)
                {
                    grvClip.Columns[5].Visible = false;
                    grvClip.Columns[6].Visible = false;
                    grvClip.Columns[2].HeaderStyle.Width = Unit.Percentage(32);
                    grvClip.Columns[3].HeaderStyle.Width = Unit.Percentage(13);
                    grvClip.Columns[4].HeaderStyle.Width = Unit.Percentage(13);

                }

                btnRefreshLibrary.Visible = true;
                btnManageCategories.Visible = true;
                if (_ListOfArchiveClip.Count == 0)
                {

                    btnClipDownload.Visible = false;
                    
                    btnRemoveClips.Visible = false;
                    lnkEmail.Visible = false;
                    ucCustomPager.Visible = false;
                }
                else
                {
                    if (_SessionInformation.IsDownloadClips == true)
                    {
                        btnClipDownload.Visible = true;
                    }
                    btnRemoveClips.Visible = true;
                    lnkEmail.Visible = true;
                    ucCustomPager.Visible = true;
                    ucCustomPager.TotalRecords = _TotalRecordsClipCount;
                    ucCustomPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                    //ucCustomPager.CurrentPage = _ViewstateInformation._CurrentClipPage;
                    ucCustomPager.BindDataList();
                }

                grvClip.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;");
                if (_HasRecords == false)
                {
                    lblNoResults.Text = CommonConstants.HTMLBreakLine + CommonConstants.NoResultsFound;
                    lblNoResults.Visible = true;
                }

                if (Page.IsPostBack)
                {
                    //string _Script = "SetSearchParam();";
                    //_Script += "setCheckbox(\"" + chkCategories1.ClientID + "\",\"" + txtCat1Selection.ClientID + "\");";
                    //_Script += "setCheckbox(\"" + chkCategories2.ClientID + "\",\"" + txtCat2Selection.ClientID + "\");";
                    //_Script += "setCheckbox(\"" + chkOwnerList.ClientID + "\",\"" + txtOwnerSelection.ClientID + "\");";
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "SetSelection", _Script, true);
                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        private void ClearArchiveClipGridFields()
        {
            try
            {
                grvClip.EditIndex = -1;
                grvClip.PageIndex = 0;
                ucCustomPager.CurrentPage = null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Clip Grid Events

        protected void grvClip_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(_ViewstateInformation.ClipSortExpression))
                {
                    if (_ViewstateInformation.ClipSortExpression.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_ViewstateInformation.IsClipSortDirecitonAsc == true)
                        {
                            _ViewstateInformation.IsClipSortDirecitonAsc = false;
                        }
                        else
                        {
                            _ViewstateInformation.IsClipSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _ViewstateInformation.ClipSortExpression = e.SortExpression;
                        _ViewstateInformation.IsClipSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _ViewstateInformation.ClipSortExpression = e.SortExpression;
                    _ViewstateInformation.IsClipSortDirecitonAsc = true;
                }

                ClearArchiveClipGridFields();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                BindArchiveClips(true);
                SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);

            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }


        #endregion Clip Grid Events

        #endregion Bind Clip

        #region Remove Clip

        protected void btnRemoveClips_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;
                for (int Count = 0; Count < grvClip.Rows.Count; Count++)
                {
                    HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)grvClip.Rows[Count].FindControl("chkDelete");
                    HiddenField hfArchiveClipKey = (HiddenField)grvClip.Rows[Count].FindControl("hfArchiveClipKey");
                    if (chkDelete.Checked == true)
                    {
                        strKeys = strKeys + chkDelete.Value + ",";
                    }
                }
                if (strKeys.Length > 0)
                {
                    strKeys = strKeys.Substring(0, strKeys.Length - 1);
                    string _Result = string.Empty;
                    IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                    _Result = _IArchiveClipController.DeleteArchiveClip(strKeys);
                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessMessage.Text = "Record(s) deleted Successfully.";

                        ClearArchiveClipGridFields();
                        BindArchiveClips(true);
                        SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);
                    }
                    else
                    {

                        ClearArchiveClipGridFields();
                        BindArchiveClips(true);
                        SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);
                    }
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "$('#tabs').tabs('select', 1);", true);

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion Remove Clip

        #region Download Clip

        protected void btnClipDownload_Click(object sender, EventArgs e)
        {
            try
            {
                IClipDownloadController _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();

                List<ClipDownload> _ListOfClipDownload = _IClipDownloadController.SelectByCustomer(new Guid(_SessionInformation.CustomerGUID));

                List<Guid> _ListOfSelectedClipGUID = new List<Guid>();

                foreach (GridViewRow _GridViewRow in grvClip.Rows)
                {
                    HtmlInputCheckBox _HtmlInputCheckBox = (HtmlInputCheckBox)_GridViewRow.FindControl("chkDelete");

                    if (_HtmlInputCheckBox.Checked)
                    {
                        HiddenField _HiddenField = (HiddenField)_GridViewRow.FindControl("hfClipID");

                        _ListOfSelectedClipGUID.Add(new Guid(_HiddenField.Value));

                        if (_ListOfSelectedClipGUID.Count + (_ListOfClipDownload != null ? _ListOfClipDownload.Count : 0) > Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected]))
                        {
                            break;
                        }
                    }
                }


                if (_ListOfSelectedClipGUID.Count + (_ListOfClipDownload != null ? _ListOfClipDownload.Count : 0) > Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected]))
                {
                    lblErrorClips.Text = "You cannot download more than " + ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected] + " clips at a time, please check your download screen.";
                    string _Script = "window.open('../DownloadClip/','DownloadClip','menubar=no,toolbar=no,location=0,width=490,height=600,resizable=yes,scrollbars=no')";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadClip", _Script, true);
                    lblErrorClips.Visible = true;
                }
                else
                {
                    if (_ListOfSelectedClipGUID.Count + (_ListOfClipDownload != null ? _ListOfClipDownload.Count : 0) <= 0)
                    {
                        lblErrorClips.Text = "There doesn't exits any clip to download. Please select atleast one clip.";
                        lblErrorClips.Visible = true;
                    }
                    else
                    {
                        //string _Script = "window.open('../DownloadClip/','DownloadClip','menubar=no,toolbar=no,location=0,width=450,height=600,resizable=yes,scrollbars=yes')";                        
                        string _Script = "window.open('../DownloadClip/','DownloadClip','menubar=no,toolbar=no,location=0,width=490,height=600,resizable=yes,scrollbars=no')";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadClip", _Script, true);
                        _SessionInformation.ListOfSelectedClipsFDownlLoad = _ListOfSelectedClipGUID;
                    }


                }


                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion Download Clip

        #region Email

        //protected void lnkEmail_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

        //        txtYourEmail.Text = _SessionInformation.Email;
        //        if (hfCurrentTabIndex.Value == "0")
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();SetEmailPopupTitle('0');", true);

        //        }
        //        else if (hfCurrentTabIndex.Value == "1")
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();SetEmailPopupTitle('1');", true);
        //        }
        //        else if (hfCurrentTabIndex.Value == "2")
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();SetEmailPopupTitle('2');", true);
        //        }
        //        txtFriendsEmail.Text = string.Empty;
        //        txtSubject.Text = string.Empty;
        //        txtMessage.Text = string.Empty;
        //        UpdateUpdatePanel(upMail);

        //        /*mdlpopupEmail.Show();
        //        UpdateUpdatePanel(upMail);*/
        //    }
        //    catch (Exception _Exception)
        //    {
        //        this.WriteException(_Exception);
        //        Response.Redirect(CommonConstants.CustomErrorPage);
        //    }

        //}

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {


                string ClipGUID = string.Empty;
                bool IsCheck = false;
                string XMLGUID = "";
                System.Data.SqlTypes.SqlXml _ArchiveClipGUIDXML = null;
                string EmailContent = string.Empty;
                List<ArchiveNM> lstArchiveNM = null;
                List<ArchiveSM> lstArchiveSM = null;
                List<ArchiveBLPM> lstArchiveBLPM = null;
                List<ArchiveTweets> lstArchiveTweets = null;


                if (hfCurrentTabIndex.Value == "0")
                {
                    for (int Count = 0; Count < grvClip.Rows.Count; Count++)
                    {
                        HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)grvClip.Rows[Count].FindControl("chkDelete");
                        if (chkDelete != null && chkDelete.Checked == true)
                        {
                            IsCheck = true;
                            HiddenField hfClipID = (HiddenField)grvClip.Rows[Count].FindControl("hfClipID");
                            XMLGUID = XMLGUID + "<Clip>" + hfClipID.Value + "</Clip>";
                        }
                    }


                    if (IsCheck)
                    {
                        XMLGUID = "<list>" + XMLGUID + "</list>";
                    }
                    else
                    {
                        XMLGUID = "<list></list>";
                    }

                    XmlDocument _XmlDocument = new XmlDocument();
                    _XmlDocument.LoadXml(XMLGUID);

                    XmlNodeReader _XmlNodeReader = new XmlNodeReader(_XmlDocument);
                    _ArchiveClipGUIDXML = new System.Data.SqlTypes.SqlXml(_XmlNodeReader);

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "1")
                {
                    lstArchiveNM = new List<ArchiveNM>();
                    foreach (GridViewRow gvRow in grvArticle.Rows)
                    {
                        if (gvRow.FindControl("chkArticleDelete") != null
                                && ((HtmlInputCheckBox)gvRow.FindControl("chkArticleDelete")).Checked)
                        {
                            ArchiveNM archiveNM = new ArchiveNM();

                            archiveNM.Title = gvRow.Cells[1].Text;
                            archiveNM.Url = ((HyperLink)gvRow.FindControl("imgArticleButton")).NavigateUrl;
                            lstArchiveNM.Add(archiveNM);
                        }
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "2")
                {
                    lstArchiveSM = new List<ArchiveSM>();

                    foreach (GridViewRow gvRow in grvSocialArticle.Rows)
                    {
                        if (gvRow.FindControl("chkSocialArticleDelete") != null
                                && ((HtmlInputCheckBox)gvRow.FindControl("chkSocialArticleDelete")).Checked)
                        {
                            ArchiveSM archiveSM = new ArchiveSM();
                            archiveSM.Title = gvRow.Cells[1].Text;
                            archiveSM.Url = ((HyperLink)gvRow.FindControl("imgSocialArticleButton")).NavigateUrl;
                            lstArchiveSM.Add(archiveSM);
                        }

                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "3")
                {
                    lstArchiveBLPM = new List<ArchiveBLPM>();

                    foreach (GridViewRow gvRow in grvPrintMedia.Rows)
                    {
                        if (gvRow.FindControl("chkPMDelete") != null
                            && ((HtmlInputCheckBox)gvRow.FindControl("chkPMDelete")).Checked)
                        {
                            ArchiveBLPM archiveBLPM = new ArchiveBLPM();
                            archiveBLPM.Headline = gvRow.Cells[2].Text;
                            archiveBLPM.Url = ((ImageButton)gvRow.FindControl("imgPrintMediaButton")).CommandArgument;
                            lstArchiveBLPM.Add(archiveBLPM);
                        }

                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "4")
                {
                    lstArchiveTweets = new List<ArchiveTweets>();

                    foreach (DataListItem dlItem in dlTweet.Items)
                    {
                        if (dlItem.FindControl("chkTweetSelect") != null
                            && ((HtmlInputCheckBox)dlItem.FindControl("chkTweetSelect")).Checked)
                        {
                            ArchiveTweets archiveTweets = new ArchiveTweets();
                            archiveTweets.Title = ((HiddenField)dlItem.FindControl("hfTitle")).Value;
                            archiveTweets.Tweet_Body = ((Label)dlItem.FindControl("lblTweetBody")).Text;
                            archiveTweets.Actor_DisplayName = ((Label)dlItem.FindControl("lblDisplayName")).Text;
                            archiveTweets.Tweet_PostedDateTime = Convert.ToDateTime(((Label)dlItem.FindControl("lblPostedDateTime")).Text);

                            lstArchiveTweets.Add(archiveTweets);
                        }
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
                }

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

                                if (hfCurrentTabIndex.Value == "0")
                                {
                                    IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                                    EmailContent = _IArchiveClipController.EmailContent(_ArchiveClipGUIDXML, mailAddress, "myIQ");
                                }
                                else if (hfCurrentTabIndex.Value == "1")
                                {
                                    IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();
                                    EmailContent = _IArchiveNMController.GetEmailContent(lstArchiveNM);
                                }
                                else if (hfCurrentTabIndex.Value == "2")
                                {
                                    ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
                                    EmailContent = _ISocialMediaController.GetEmailContent(lstArchiveSM);
                                }
                                else if (hfCurrentTabIndex.Value == "3")
                                {
                                    IArchiveBLPMController _IArchiveBLPMController = _ControllerFactory.CreateObject<IArchiveBLPMController>();
                                    EmailContent = _IArchiveBLPMController.GetEmailContent(lstArchiveBLPM);
                                }
                                else if (hfCurrentTabIndex.Value == "4")
                                {
                                    IArchiveTweetsController _IArchiveTweetsController = _ControllerFactory.CreateObject<IArchiveTweetsController>();
                                    EmailContent = _IArchiveTweetsController.GetEmailContent(lstArchiveTweets);
                                }

                                string WholeEmailBody = "";
                                WholeEmailBody = CommonFunctions.EmailSend(txtYourEmail.Text, mailAddress, txtSubject.Text, txtMessage.Text, EmailContent);

                                if (hdnEmailType.Value == "Search")
                                {
                                    MailLog(WholeEmailBody, mailAddress);
                                }
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

                    if (hfCurrentTabIndex.Value == "0")
                    {
                        lblSuccessMessage.Text = "Email Sent Successfully.";
                    }
                    else if (hfCurrentTabIndex.Value == "1")
                    {
                        lblSuccessMessageArticle.Text = "Email Sent Successfully.";
                    }
                    else if (hfCurrentTabIndex.Value == "2")
                    {
                        lblSuccessMessageArticleSM.Text = "Email Sent Successfully.";
                    }
                    else if (hfCurrentTabIndex.Value == "3")
                    {
                        lblSuccessMessagePM.Text = "Email Sent Successfully.";
                    }
                    else if (hfCurrentTabIndex.Value == "4")
                    {
                        lblSuccessMessageTweet.Text = "Email Sent Successfully.";
                    }

                    ClearEmailFrom();
                    //mdlpopupEmail.Hide();
                    UpdateUpdatePanel(upMail);
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // Ensure that the control is nested in a server form.
            if (Page != null)
            {
                Page.VerifyRenderingInServerForm(this);
            }
            base.Render(writer);
        }


        private void MailLog(string _EmailBody, string mailAddress)
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
                _OutboundReporting.FromEmailAddress = txtYourEmail.Text;
                _OutboundReporting.ToEmailAddress = mailAddress;
                _OutboundReporting.MailContent = _FileContent;
                _OutboundReporting.ServiceType = "myIQ";
                _Result = _IOutboundReportingController.InsertOutboundReportingLog(_OutboundReporting);
            }
            catch (Exception)
            {

                throw;
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

        #endregion Email

        #region Edit Clip

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {


                if (ddlOwner.Items.Count <= 0)
                {
                    BindCustomerDropDown(ddlOwner, true);
                }

                if (ddlPCategory.Items.Count <= 0)
                {
                    BindMediaCategoryDropDown();
                }

                HiddenField hfClipIDEdit = (HiddenField)grvClip.Rows[((sender as LinkButton).NamingContainer as GridViewRow).RowIndex].FindControl("hfClipID");
                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                ArchiveClip _objArchiveClip = new ArchiveClip();
                _objArchiveClip.ClipID = new Guid(hfClipIDEdit.Value);
                List<ArchiveClip> _ListOfArchiveClip = _IArchiveClipController.GetArchiveClipByClipID(_objArchiveClip);

                if (_ListOfArchiveClip.Count > 0)
                {
                    ddlPCategory.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveClip[0].CategoryGUID)) ? "0" : Convert.ToString(_ListOfArchiveClip[0].CategoryGUID);
                    ddlSubCategory1.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveClip[0].SubCategory1GUID)) ? "0" : Convert.ToString(_ListOfArchiveClip[0].SubCategory1GUID);
                    ddlSubCategory2.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveClip[0].SubCategory2GUID)) ? "0" : Convert.ToString(_ListOfArchiveClip[0].SubCategory2GUID);
                    ddlSubCategory3.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveClip[0].SubCategory3GUID)) ? "0" : Convert.ToString(_ListOfArchiveClip[0].SubCategory3GUID);

                    ddlOwner.SelectedValue = ddlOwner.Items.FindByValue(_ListOfArchiveClip[0].CustomerGUID.ToString()) == null ? "0" : _ListOfArchiveClip[0].CustomerGUID.ToString();
                    txtEditClipTitle.Text = _ListOfArchiveClip[0].ClipTitle;
                    txtDescription.Text = _ListOfArchiveClip[0].Description;
                    txtKeywords.Text = _ListOfArchiveClip[0].Keywords;
                    hdnEditClipID.Value = _ListOfArchiveClip[0].ClipID.ToString();
                    hdnArchiveClipKey.Value = _ListOfArchiveClip[0].ArchiveClipKey.ToString();
                    if (_ListOfArchiveClip[0].Rating == 6)
                    {
                        chkClipPreferred.Checked = true;
                    }
                    else
                    {
                        chkClipPreferred.Checked = false;
                    }
                    txtClipRate.Text = _ListOfArchiveClip[0].Rating.ToString();
                }

                string _Script = "UpdateSubCategory2(\"" + ddlPCategory.ClientID + "\",0);";
                _Script += "UpdateSubCategory2(\"" + ddlSubCategory1.ClientID + "\",0);";
                _Script += "UpdateSubCategory2(\"" + ddlSubCategory2.ClientID + "\",0);";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "LoadCatList", _Script, true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showEditClipModal", "ShowModal('pnlClipPanel');", true);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                //mdlpopupClip.Show();
                UpdateUpdatePanel(upEditClip);

                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "SetPopupHeight", "onshowing();", true);

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void BindCustomerDropDown(DropDownList p_ddlCustomer, bool p_IsEdit = false)
        {
            try
            {

                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();

                List<Customer> _ListOfCustomer = new List<Customer>();
                _ListOfCustomer = _ICustomerController.GetCustomerNameByClientID(_SessionInformation.ClientID);

                p_ddlCustomer.DataTextField = "FirstName";
                p_ddlCustomer.DataValueField = "CustomerGUID";
                p_ddlCustomer.DataSource = _ListOfCustomer;
                p_ddlCustomer.DataBind();

                if (p_IsEdit)
                {
                    p_ddlCustomer.Items.Insert(0, new ListItem("<Blank>", "0"));
                }
                else
                {
                    p_ddlCustomer.Items.Insert(0, new ListItem("All", "0"));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BindMediaCategoryDropDown()
        {

            try
            {

                List<CustomCategory> _ListofCustomCategory = _ViewstateInformation.ListOfCustomCategory;

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
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnClipUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                ArchiveClip _ArchiveClip = new ArchiveClip();

                Guid? _NullCategoryGUID = null;
                _ArchiveClip.ArchiveClipKey = Convert.ToInt32(hdnArchiveClipKey.Value);
                _ArchiveClip.ClipTitle = txtEditClipTitle.Text;
                _ArchiveClip.CustomerGUID = new Guid(ddlOwner.SelectedValue);
                _ArchiveClip.CategoryGUID = ddlPCategory.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlPCategory.SelectedValue);
                _ArchiveClip.SubCategory1GUID = ddlSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory1.SelectedValue);
                _ArchiveClip.SubCategory2GUID = ddlSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory2.SelectedValue);
                _ArchiveClip.SubCategory3GUID = ddlSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlSubCategory3.SelectedValue);
                _ArchiveClip.Keywords = txtKeywords.Text;
                _ArchiveClip.Description = txtDescription.Text;
                _ArchiveClip.FirstName = ddlOwner.SelectedItem.Text;
                _ArchiveClip.ClipID = new Guid(hdnEditClipID.Value);
                _ArchiveClip.Rating = Convert.ToInt16(txtClipRate.Text);



                string _Result = _IArchiveClipController.UpdateArchiveClip(_ArchiveClip);

                if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) < 0)
                {
                    lblClipMsg.Text = "Some error occurs please try again.";
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showEditClipModal", "ShowModal('pnlClipPanel');", true);
                    //mdlpopupClip.Show();

                }
                else
                {
                    lblSuccessMessage.Text = "Record updated Successfully.";
                    ClearArchiveClipGridFields();

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                    BindArchiveClips(false);
                    SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);

                    string _SubCategory = string.Empty;

                    _SubCategory = (ddlSubCategory1.SelectedValue == "0" ? "{\"Key\":\"SubCategory1GUID\",\"Value\":\"" + null + "\"}" : "{\"Key\":\"SubCategory1GUID\",\"Value\":\"" + ddlSubCategory1.SelectedValue + "\"}");
                    _SubCategory = _SubCategory + (ddlSubCategory2.SelectedValue == "0" ? ",{\"Key\":\"SubCategory2GUID\",\"Value\":\"" + null + "\"}" : ",{\"Key\":\"SubCategory2GUID\",\"Value\":\"" + ddlSubCategory2.SelectedValue + "\"}");
                    _SubCategory = _SubCategory + (ddlSubCategory3.SelectedValue == "0" ? ",{\"Key\":\"SubCategory3GUID\",\"Value\":\"" + null + "\"}" : ",{\"Key\":\"SubCategory3GUID\",\"Value\":\"" + ddlSubCategory3.SelectedValue + "\"}");

                    if (!string.IsNullOrWhiteSpace(_SubCategory))
                    {
                        _SubCategory = "," + _SubCategory;
                    }
                    string _JsonInput = "{\"Guid\":\"" + hdnEditClipID.Value + "\",\"Title\":\"" + HttpUtility.HtmlEncode(txtEditClipTitle.Text) + "\",\"Keywords\":\"" + HttpUtility.HtmlEncode(txtKeywords.Text) + "\",\"Description\":\"" + HttpUtility.HtmlEncode(txtDescription.Text) + "\",\"ClipMeta\":[{\"Key\":\"iqClientid\",\"Value\":\"" + _SessionInformation.ClientGUID + "\"},{\"Key\":\"iQUser\",\"Value\":\"" + ddlOwner.SelectedValue + "\"},{\"Key\":\"iQCategory\",\"Value\":\"" + ddlPCategory.SelectedValue + "\"}" + _SubCategory + "]}";
                    string _UpdateURL = ConfigurationManager.AppSettings["UpdateClipInfo"];


                    CookieContainer _CookieContainer = new CookieContainer();

                    if (Request.Cookies != null && Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName] != null)
                    {
                        Cookie _Cookie = new Cookie();

                        _Cookie.Name = System.Web.Security.FormsAuthentication.FormsCookieName;
                        _Cookie.Value = Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName].Value;
                        _Cookie.Domain = System.Web.Security.FormsAuthentication.CookieDomain;

                        _CookieContainer.Add(_Cookie);
                    }

                    string _HttpResponse = CommonFunctions.GetHttpResponse(_UpdateURL, _CookieContainer, _JsonInput);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showEditClipModal", "closeModal('pnlClipPanel');", true);
                    //mdlpopupClip.Hide();
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion Edit Clip

        #region Play Clip

        protected void lbtnPlay_OnCommand(object sender, CommandEventArgs e)
        {
            try
            {
                _clipID = e.CommandArgument.ToString();
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

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
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

                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "CCCallBack", "RegisterCCCallback(0);", true);

                //_ViewstateInformation.VSTimetick = 0;
                this.SetViewstateInformation(_ViewstateInformation);
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

        #endregion Play Clip

        #region Refresh Library

        protected void btnRefreshLibrary_Click(object sender, EventArgs e)
        {
            try
            {
                GetNewClips();
                ClearSearch();
                //if (_ViewstateInformation.IsmyIQSearchActive != null && Convert.ToBoolean(_ViewstateInformation.IsmyIQSearchActive))
                //{

                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                //}
                /*else
                {
                    ClearArchiveClipGridFields();
                    updateCurrentTabStatus(0);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                    BindArchiveClips(true);
                    SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);
                }*/
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void imgbtnRefreshNews_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_ViewstateInformation.IsmyIQSearchActive != null && Convert.ToBoolean(_ViewstateInformation.IsmyIQSearchActive))
                //{
                ClearSearch();
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                //}
                /*else
                {
                    updateCurrentTabStatus(1);
                    ucCustomPagerArticle.CurrentPage = 0;
                    BindArchiveNM();
                    SortClipDirection(_ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, grvArticle);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                }*/



            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void imgbtnRefreshSocialMedia_Click(object sender, EventArgs e)
        {

            try
            {
                //if (_ViewstateInformation.IsmyIQSearchActive != null && Convert.ToBoolean(_ViewstateInformation.IsmyIQSearchActive))
                //{
                ClearSearch();
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                //}
                /*else
                {
                    updateCurrentTabStatus(2);
                    ucCustomPagerSocialArticle.CurrentPage = 0;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                    BindArchiveSM();
                    SortClipDirection(_ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, grvSocialArticle);
                }*/
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void imgbtnRefreshPrintMedia_Click(object sender, EventArgs e)
        {
            try
            {
                ClearSearch();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        private void GetNewClips()
        {
            try
            {

                // commented by meghana on 14-march-2012 removed signle insertion and insert data from GetData into archeiveClip
                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                List<ArchiveClip> _ListOfArchiveClip = _IArchiveClipController.GetData();

                if (_ListOfArchiveClip.Count > 0)
                {
                    int BulkProcessCnt = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfArchiveClipRecordsToInsertAtOnce"]);
                    int ProcessCap = 0;
                    foreach (ArchiveClip _ArchiveClip in _ListOfArchiveClip)
                    {
                        GetLocalDate(_ArchiveClip);
                        GetClosedCaption(_ArchiveClip);
                        ProcessCap++;
                        if (ProcessCap % BulkProcessCnt == 0 || ProcessCap >= _ListOfArchiveClip.Count())
                        {
                            int SkipCnt = ProcessCap >= _ListOfArchiveClip.Count() ? ProcessCap - (_ListOfArchiveClip.Count() % BulkProcessCnt) : ProcessCap - BulkProcessCnt;
                            System.Xml.Linq.XDocument _XDocument = _IArchiveClipController.GenerateListToXML(_ListOfArchiveClip.Skip(SkipCnt).Take(BulkProcessCnt).ToList());
                            System.Data.SqlTypes.SqlXml _SqlXML = new System.Data.SqlTypes.SqlXml(_XDocument.CreateReader());
                            _IArchiveClipController.InsertArchiveClip(_SqlXML);
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetLocalDate(ArchiveClip _ArchiveClip)
        {
            try
            {
                DateTime _LocalDateTimeTemp = _ArchiveClip.ClipDate.Value.AddHours(_ArchiveClip.gmt_adj);

                if (_LocalDateTimeTemp.IsDaylightSavingTime())
                {
                    _ArchiveClip.ClipDate = _LocalDateTimeTemp.AddHours(_ArchiveClip.dst_adj);
                }
                else
                {
                    _ArchiveClip.ClipDate = _LocalDateTimeTemp;
                }

                _ArchiveClip.ClipDate.Value.AddSeconds(_ArchiveClip.StartOffset);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetClosedCaption(ArchiveClip _ArchiveClip)
        {
            try
            {
                string _ClosedCaptionURL = ConfigurationManager.AppSettings[CommonConstants.ConfigGetClosedCaptionFromIQ];
                StringBuilder _Caption = new StringBuilder();


                _Caption.Remove(0, _Caption.Length);

                _Caption = GetCaptionFromIQ(Convert.ToString(_ArchiveClip.ClipID));
                _Caption = _Caption.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                _Caption = _Caption.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                _Caption = _Caption.Replace("<tt xmlns=\"http://www.w3.org/2006/10/ttaf1\" xml:lang=\"EN\">", "<tt>");
                _Caption = _Caption.Replace("<tt xml:lang=\"EN\" xmlns=\"http://www.w3.org/2006/10/ttaf1\">", "<tt>");

                XmlDocument _XmlDocument = new XmlDocument();

                try
                {
                    _XmlDocument.LoadXml(_Caption.ToString());
                    _ArchiveClip.ClosedCaption = _Caption.ToString();
                }
                catch (XmlException)
                {
                    _ArchiveClip.ClosedCaption = "<tt><head><metadata><title></title></metadata></head><body region=\"subtitleArea\"><div><p begin=\"0s\" end=\"0s\">IQM-ClosedCaption is missing</p></div></body></tt>";

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private StringBuilder GetCaptionFromIQ(string p_ClipID)
        {
            try
            {
                string _CaptionText = string.Empty;
                StringBuilder _Response = new StringBuilder();

                if (!string.IsNullOrEmpty(p_ClipID))
                {
                    string _CaptionURL = string.Empty;


                    _CaptionURL = ConfigurationManager.AppSettings[CommonConstants.ConfigGetClosedCaptionFromIQ].ToString() + p_ClipID;

                    WebClient client = new WebClient();
                    _Response.Append(client.DownloadString(_CaptionURL));

                }

                return _Response;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion Refresh Library
        #endregion

        #region News Article Events

        #region Bind Article

        private void BindArchiveNM()
        {
            if (!chkNews.Checked)
            {
                grvArticle.DataSource = null;
                grvArticle.DataBind();
                ucCustomPagerArticle.Visible = false;
                return;
            }

            if (string.IsNullOrEmpty(_ViewstateInformation.ArticleSortExpression))
            {
                _ViewstateInformation.ArticleSortExpression = "CreatedDate";

            }

            if (ucCustomPagerArticle.CurrentPage == null)
            {
                ucCustomPagerArticle.CurrentPage = 0;
            }

            if (_ViewstateInformation.IsmyIQSearchActive == null)
            {
                _ViewstateInformation.IsmyIQSearchActive = false;
            }

            IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();
            int _TotalRecordsArticleCount = 0;

            List<ArchiveNM> _ListOfArchiveNM = _IArchiveNMController.GetArchiveNMBySearch(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._MyIQSearchParams.SearchTitle, _ViewstateInformation._MyIQSearchParams.SearchDesc, _ViewstateInformation._MyIQSearchParams.SearchKey, _ViewstateInformation._MyIQSearchParams.SearchCC, _ViewstateInformation._MyIQSearchParams.FromDate, _ViewstateInformation._MyIQSearchParams.ToDate, _ViewstateInformation._MyIQSearchParams.CategoryGUID1, _ViewstateInformation._MyIQSearchParams.CategoryGUID2, _ViewstateInformation._MyIQSearchParams.CategoryGUID3, _ViewstateInformation._MyIQSearchParams.CategoryGUID4, _ViewstateInformation._MyIQSearchParams.CategoryOperator1, _ViewstateInformation._MyIQSearchParams.CategoryOperator2, _ViewstateInformation._MyIQSearchParams.CategoryOperator3, _ViewstateInformation._MyIQSearchParams.CustomerGUID, ucCustomPagerArticle.CurrentPage.Value, ucCustomPagerArticle.PageSize, _ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, out _TotalRecordsArticleCount);

            _ViewstateInformation.TotalRecordsCountArticle = _TotalRecordsArticleCount;

            SetViewstateInformation(_ViewstateInformation);

            grvArticle.Visible = true;

            grvArticle.DataSource = _ListOfArchiveNM;
            grvArticle.DataBind();

            imgbtnRefreshNews.Visible = true;
            btnManageCategories1.Visible = true;
            if (_ListOfArchiveNM == null || _ListOfArchiveNM.Count == 0)
            {
                ucCustomPagerArticle.Visible = false;
                btnRemoveArticle.Visible = false;
                
                btnArticleNMDownload.Visible = false;
                lnkEmailNews.Visible = false;
            }
            else
            {
                btnRemoveArticle.Visible = true;
                btnArticleNMDownload.Visible = true;
                ucCustomPagerArticle.Visible = true;
                lnkEmailNews.Visible = true;
                ucCustomPagerArticle.TotalRecords = _TotalRecordsArticleCount;
                ucCustomPagerArticle.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                //ucCustomPager.CurrentPage = _ViewstateInformation._CurrentClipPage;
                ucCustomPagerArticle.BindDataList();
            }
            grvArticle.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;");

            if (Page.IsPostBack)
            {
                //string _Script = "SetSearchParam();";
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "SetSelection", _Script, true);
            }
        }

        //private void RegisterPosbBackForNewsArticleDownload()
        //{
        //    foreach (GridViewRow _GridViewRow in grvArticle.Rows)
        //    {
        //        ImageButton _ImageButton = _GridViewRow.FindControl("ibtnDownload") as ImageButton;

        //        if (_ImageButton != null)
        //        {
        //            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(_ImageButton);
        //        }
        //    }
        //}

        #region Article Grid Events

        protected void grvArticle_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(_ViewstateInformation.ArticleSortExpression))
                {
                    if (_ViewstateInformation.ArticleSortExpression.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_ViewstateInformation.IsArticleSortDirecitonAsc == true)
                        {
                            _ViewstateInformation.IsArticleSortDirecitonAsc = false;
                        }
                        else
                        {
                            _ViewstateInformation.IsArticleSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _ViewstateInformation.ArticleSortExpression = e.SortExpression;
                        _ViewstateInformation.IsArticleSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _ViewstateInformation.ArticleSortExpression = e.SortExpression;
                    _ViewstateInformation.IsArticleSortDirecitonAsc = true;
                }

                ucCustomPagerArticle.CurrentPage = 0;

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                BindArchiveNM();
                SortClipDirection(_ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, grvArticle);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "$('#tabs').tabs('select', 1);", true);
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #endregion

        #region Edit Article

        protected void lbtnArticleEdit_Click(object sender, EventArgs e)
        {
            try
            {
                hdnArticleType.Value = "NM";
                if (ddlPArticleCategory.Items.Count <= 0)
                {
                    BindArticleMediaCategoryDropDown();
                }

                HiddenField hfClipIDEdit = (HiddenField)grvArticle.Rows[((sender as LinkButton).NamingContainer as GridViewRow).RowIndex].FindControl("hfArchiveNMKey");
                IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();

                List<ArchiveNM> _ListOfArchiveNM = _IArchiveNMController.GetArchiveNMByArchiveNMKey(Convert.ToInt32(hfClipIDEdit.Value));

                if (_ListOfArchiveNM.Count > 0)
                {
                    ddlPArticleCategory.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveNM[0].CategoryGuid)) ? "0" : Convert.ToString(_ListOfArchiveNM[0].CategoryGuid);
                    ddlArticleSubCategory1.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveNM[0].SubCategory1Guid)) ? "0" : Convert.ToString(_ListOfArchiveNM[0].SubCategory1Guid);
                    ddlArticleSubCategory2.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveNM[0].SubCategory2Guid)) ? "0" : Convert.ToString(_ListOfArchiveNM[0].SubCategory2Guid);
                    ddlArticleSubCategory3.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveNM[0].SubCategory3Guid)) ? "0" : Convert.ToString(_ListOfArchiveNM[0].SubCategory3Guid);

                    txtArticleTitle.Text = _ListOfArchiveNM[0].Title;
                    txtArticleDescription.Text = _ListOfArchiveNM[0].Description;
                    txtArticleKeywords.Text = _ListOfArchiveNM[0].Keywords;
                    hdnEditArchiveKey.Value = _ListOfArchiveNM[0].ArchiveNMKey.ToString();
                    if (_ListOfArchiveNM[0].Rating == 6)
                    { chkArticlePreferred.Checked = true; }
                    else
                    { chkArticlePreferred.Checked = false; }
                    txtArticleRate.Text = _ListOfArchiveNM[0].Rating.ToString();

                }

                spnEditArtile.Text = "Edit Article Details";
                lblrateartile.InnerText = "Rate This Article :";

                string _Script = "UpdateSubCategory2(\"" + ddlPArticleCategory.ClientID + "\",1);" +
                "UpdateSubCategory2(\"" + ddlArticleSubCategory1.ClientID + "\",1);" +
                "UpdateSubCategory2(\"" + ddlArticleSubCategory2.ClientID + "\",1);";

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadCatListArticle", _Script, true);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleEditModal", "ShowModal('pnlEditArticle');", true);
                //mdlpopupArticle.Show();
                UpdateUpdatePanel(upEditArticle);

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnArticleUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnArticleType.Value == "NM")
                {
                    IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();
                    ArchiveNM _ArchiveNM = new ArchiveNM();

                    Guid? _NullCategoryGUID = null;
                    _ArchiveNM.ArchiveNMKey = Convert.ToInt32(hdnEditArchiveKey.Value);
                    _ArchiveNM.Title = txtArticleTitle.Text;
                    _ArchiveNM.CategoryGuid = new Guid(ddlPArticleCategory.SelectedValue);
                    _ArchiveNM.SubCategory1Guid = ddlArticleSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory1.SelectedValue);
                    _ArchiveNM.SubCategory2Guid = ddlArticleSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory2.SelectedValue);
                    _ArchiveNM.SubCategory3Guid = ddlArticleSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory3.SelectedValue);
                    _ArchiveNM.Keywords = txtArticleKeywords.Text;
                    _ArchiveNM.Description = txtArticleDescription.Text;
                    _ArchiveNM.Rating = Convert.ToInt16(txtArticleRate.Text);


                    string _Result = _IArchiveNMController.UpdateArchiveNM(_ArchiveNM);

                    if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) < 0)
                    {
                        lblErrorMsgArticle.Text = "Some error occurs please try again.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleEditModal", "ShowModal('pnlEditArticle');", true);
                        //mdlpopupArticle.Show();

                    }
                    else
                    {
                        lblSuccessMessageArticle.Text = "Record updated Successfully.";
                        ClearArchiveClipGridFields();

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                        BindArchiveNM();
                        SortClipDirection(_ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, grvArticle);
                        //mdlpopupArticle.Hide();
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "$('#tabs').tabs('select', 1);", true);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideEditArticlePnl", "closeModal('pnlEditArticle');", true);

                    }
                }
                else if (hdnArticleType.Value == "SM")
                {
                    ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
                    ArchiveSM _ArchiveSM = new ArchiveSM();

                    Guid? _NullCategoryGUID = null;
                    _ArchiveSM.ArchiveSMKey = Convert.ToInt32(hdnEditArchiveKey.Value);
                    _ArchiveSM.Title = txtArticleTitle.Text;
                    _ArchiveSM.CategoryGuid = new Guid(ddlPArticleCategory.SelectedValue);
                    _ArchiveSM.SubCategory1Guid = ddlArticleSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory1.SelectedValue);
                    _ArchiveSM.SubCategory2Guid = ddlArticleSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory2.SelectedValue);
                    _ArchiveSM.SubCategory3Guid = ddlArticleSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory3.SelectedValue);
                    _ArchiveSM.Keywords = txtArticleKeywords.Text;
                    _ArchiveSM.Description = txtArticleDescription.Text;
                    _ArchiveSM.Rating = Convert.ToInt16(txtArticleRate.Text);


                    string _Result = _ISocialMediaController.UpdateArchiveSM(_ArchiveSM);

                    if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) < 0)
                    {
                        lblErrorMsgArticleSM.Text = "Some error occurs please try again.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleEditModal", "ShowModal('pnlEditArticle');", true);
                        //mdlpopupArticle.Show();

                    }
                    else
                    {
                        lblSuccessMessageArticleSM.Text = "Record updated Successfully.";

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                        BindArchiveSM();
                        SortClipDirection(_ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, grvSocialArticle);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideEditArticlePnl", "closeModal('pnlEditArticle');", true);
                        //mdlpopupArticle.Hide();
                    }
                }

                else if (hdnArticleType.Value == "PM")
                {
                    IArchiveBLPMController _IArchiveBLPMController = _ControllerFactory.CreateObject<IArchiveBLPMController>();
                    ArchiveBLPM archiveBLPM = new ArchiveBLPM();

                    Guid? _NullCategoryGUID = null;
                    archiveBLPM.ArchiveBLPMKey = Convert.ToInt32(hdnEditArchiveKey.Value);
                    archiveBLPM.Headline = txtArticleTitle.Text;
                    archiveBLPM.Description = txtArticleDescription.Text;
                    archiveBLPM.CategoryGuid = new Guid(ddlPArticleCategory.SelectedValue);
                    archiveBLPM.SubCategory1Guid = ddlArticleSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory1.SelectedValue);
                    archiveBLPM.SubCategory2Guid = ddlArticleSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory2.SelectedValue);
                    archiveBLPM.SubCategory3Guid = ddlArticleSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory3.SelectedValue);
                    archiveBLPM.Keywords = txtArticleKeywords.Text;
                    archiveBLPM.Rating = Convert.ToInt16(txtArticleRate.Text);


                    string _Result = _IArchiveBLPMController.UpdateArchivePM(archiveBLPM);

                    if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) < 0)
                    {
                        lblErrorMsgPM.Text = "Some error occurs please try again.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleEditModal", "ShowModal('pnlEditArticle');", true);
                        //mdlpopupArticle.Show();

                    }
                    else
                    {
                        lblSuccessMessagePM.Text = "Record updated Successfully.";

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                        BindArchivePM();
                        SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, grvPrintMedia);
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideEditArticlePnl", "closeModal('pnlEditArticle');", true);
                        UpdateUpdatePanel(upPrintMediaGrid);
                        //mdlpopupArticle.Hide();
                    }
                }
                else if (hdnArticleType.Value == "Twitter")
                {
                    IArchiveTweetsController _IArchiveTweetsController = _ControllerFactory.CreateObject<IArchiveTweetsController>();
                    ArchiveTweets archiveTweets = new ArchiveTweets();

                    Guid? _NullCategoryGUID = null;
                    archiveTweets.ArchiveTweets_Key = Convert.ToInt32(hdnEditArchiveKey.Value);
                    archiveTweets.Title = txtArticleTitle.Text;
                    archiveTweets.Description = txtArticleDescription.Text;
                    archiveTweets.CategoryGuid = new Guid(ddlPArticleCategory.SelectedValue);
                    archiveTweets.SubCategory1Guid = ddlArticleSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory1.SelectedValue);
                    archiveTweets.SubCategory2Guid = ddlArticleSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory2.SelectedValue);
                    archiveTweets.SubCategory3Guid = ddlArticleSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlArticleSubCategory3.SelectedValue);
                    archiveTweets.Keywords = txtArticleKeywords.Text;
                    archiveTweets.Rating = Convert.ToInt16(txtArticleRate.Text);


                    string _Result = _IArchiveTweetsController.UpdateArchiveTweets(archiveTweets);

                    if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) < 0)
                    {
                        lblErrorMsgTweet.Text = "Some error occurs please try again.";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleEditModal", "ShowModal('pnlEditArticle');", true);
                        //mdlpopupArticle.Show();

                    }
                    else
                    {
                        lblSuccessMessageTweet.Text = "Record updated Successfully.";

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
                        BindArchiveTweets(false);                        
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "HideEditArticlePnl", "closeModal('pnlEditArticle');", true);
                        UpdateUpdatePanel(upTweetGrid);
                        
                    }

                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void BindArticleMediaCategoryDropDown()
        {

            try
            {
                List<CustomCategory> _ListofCustomCategory = _ViewstateInformation.ListOfCustomCategory;

                if (_ListofCustomCategory != null && _ListofCustomCategory.Count > 0)
                {
                    ddlPArticleCategory.DataTextField = "CategoryName";
                    ddlPArticleCategory.DataValueField = "CategoryGUID";
                    ddlPArticleCategory.DataSource = _ListofCustomCategory;
                    ddlPArticleCategory.DataBind();

                    ddlPArticleCategory.Items.Insert(0, new ListItem("<Blank>", "0"));

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
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Remove Article

        protected void btnRemoveArticle_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;
                for (int Count = 0; Count < grvArticle.Rows.Count; Count++)
                {
                    HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)grvArticle.Rows[Count].FindControl("chkArticleDelete");
                    if (chkDelete.Checked == true)
                    {
                        strKeys = strKeys + chkDelete.Value + ",";
                    }
                }
                if (strKeys.Length > 0)
                {
                    strKeys = strKeys.Substring(0, strKeys.Length - 1);
                    string _Result = string.Empty;
                    IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();
                    _Result = _IArchiveNMController.DeleteArchiveNM(strKeys);
                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessMessageArticle.Text = "Record(s) deleted Successfully.";
                    }
                    ucCustomPagerArticle.CurrentPage = 0;
                    BindArchiveNM();
                    SortClipDirection(_ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, grvArticle);
                }
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "$('#tabs').tabs('select', 1);", true);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }


        #endregion

        #region Download News Article

        protected void btnArticleNMDownload_Click(object sender, EventArgs e)
        {
            try
            {
                IArchiveNMDownloadController _IArchiveNMDownloadController = _ControllerFactory.CreateObject<IArchiveNMDownloadController>();

                List<ArchiveNMDownload> _ListOfArchiveNMDownload = _IArchiveNMDownloadController.GetByCustomerGuid(new Guid(_SessionInformation.CustomerGUID));


                List<string> _ListOfSelectedArticleID = new List<string>();

                foreach (GridViewRow _GridViewRow in grvArticle.Rows)
                {
                    HtmlInputCheckBox _HtmlInputCheckBox = (HtmlInputCheckBox)_GridViewRow.FindControl("chkArticleDelete");

                    if (_HtmlInputCheckBox.Checked)
                    {
                        HiddenField _HiddenField = (HiddenField)_GridViewRow.FindControl("hfArticleNMID");

                        _ListOfSelectedArticleID.Add(_HiddenField.Value);

                        if (_ListOfSelectedArticleID.Count + (_ListOfArchiveNMDownload != null ? _ListOfArchiveNMDownload.Count : 0) > Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected]))
                        {
                            break;
                        }
                    }
                }


                if (_ListOfSelectedArticleID.Count + (_ListOfArchiveNMDownload != null ? _ListOfArchiveNMDownload.Count : 0) > Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected]))
                {
                    lblErrorMsgArticle.Text = "You cannot download more than " + ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected] + " articles at a time, please check your download screen.";
                    string _Script = "window.open('../DownloadArchiveNM/','DownloadArchiveNM','menubar=no,toolbar=no,location=0,width=490,height=600,resizable=yes,scrollbars=no')";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadArchiveNM", _Script, true);
                    lblErrorMsgArticle.Visible = true;
                }
                else
                {
                    if (_ListOfSelectedArticleID.Count + (_ListOfArchiveNMDownload != null ? _ListOfArchiveNMDownload.Count : 0) <= 0)
                    {
                        lblErrorMsgArticle.Text = "There doesn't exits any article to download. Please select atleast one article.";
                        lblErrorMsgArticle.Visible = true;
                    }
                    else
                    {
                        //string _Script = "window.open('../DownloadClip/','DownloadClip','menubar=no,toolbar=no,location=0,width=450,height=600,resizable=yes,scrollbars=yes')";                        
                        string _Script = "window.open('../DownloadArchiveNM/','DownloadArchiveNM','menubar=no,toolbar=no,location=0,width=490,height=600,resizable=yes,scrollbars=no')";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadArchiveNM", _Script, true);
                        _SessionInformation.ListOfSelectedArchiveNMDownload = _ListOfSelectedArticleID;
                    }


                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion


        #endregion

        #region Social Article Events

        #region Bind Social Article

        private void BindArchiveSM()
        {
            try
            {
                if (!chkSocialMedia.Checked)
                {
                    grvSocialArticle.DataSource = null;
                    grvSocialArticle.DataBind();
                    ucCustomPagerSocialArticle.Visible = false;
                    return;
                }

                if (string.IsNullOrEmpty(_ViewstateInformation.SocialArticleSortExpression))
                {
                    _ViewstateInformation.SocialArticleSortExpression = "CreatedDate";

                }

                if (ucCustomPagerSocialArticle.CurrentPage == null)
                {
                    ucCustomPagerSocialArticle.CurrentPage = 0;
                }

                if (_ViewstateInformation.IsmyIQSearchActive == null)
                {
                    _ViewstateInformation.IsmyIQSearchActive = false;
                }

                ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
                int _TotalRecordsArticleCount = 0;

                List<ArchiveSM> _ListOfArchiveSM = _ISocialMediaController.GetArchiveSMBySearch(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._MyIQSearchParams.SearchTitle, _ViewstateInformation._MyIQSearchParams.SearchDesc, _ViewstateInformation._MyIQSearchParams.SearchKey, _ViewstateInformation._MyIQSearchParams.SearchCC, _ViewstateInformation._MyIQSearchParams.FromDate, _ViewstateInformation._MyIQSearchParams.ToDate, _ViewstateInformation._MyIQSearchParams.CategoryGUID1, _ViewstateInformation._MyIQSearchParams.CategoryGUID2, _ViewstateInformation._MyIQSearchParams.CategoryGUID3, _ViewstateInformation._MyIQSearchParams.CategoryGUID4, _ViewstateInformation._MyIQSearchParams.CategoryOperator1, _ViewstateInformation._MyIQSearchParams.CategoryOperator2, _ViewstateInformation._MyIQSearchParams.CategoryOperator3, _ViewstateInformation._MyIQSearchParams.CustomerGUID, ucCustomPagerSocialArticle.CurrentPage.Value, ucCustomPagerArticle.PageSize, _ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, out _TotalRecordsArticleCount);

                _ViewstateInformation.TotalRecordsCountSocialArticle = _TotalRecordsArticleCount;

                SetViewstateInformation(_ViewstateInformation);

                grvSocialArticle.Visible = true;

                grvSocialArticle.DataSource = _ListOfArchiveSM;
                grvSocialArticle.DataBind();

                imgbtnRefreshSocialMedia.Visible = true;
                btnManageCategories2.Visible = true;
                if (_ListOfArchiveSM == null || _ListOfArchiveSM.Count == 0)
                {
                    ucCustomPagerSocialArticle.Visible = false;
                    btnRemoveSocialArticle.Visible = false;
                    btnArticleSMDownload.Visible = false;
                    lnkEmailSocialMedia.Visible = false;
                }
                else
                {
                    btnRemoveSocialArticle.Visible = true;
                    btnArticleSMDownload.Visible = true;
                    lnkEmailSocialMedia.Visible = true;
                    ucCustomPagerSocialArticle.Visible = true;
                    ucCustomPagerSocialArticle.TotalRecords = _TotalRecordsArticleCount;
                    ucCustomPagerSocialArticle.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                    ucCustomPagerSocialArticle.BindDataList();
                }
                grvSocialArticle.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;");
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        //private void RegisterPosbBackForSocialArticleDownload()
        //{
        //    foreach (GridViewRow _GridViewRow in grvSocialArticle.Rows)
        //    {
        //        ImageButton _ImageButton = _GridViewRow.FindControl("ibtnSocialDownload") as ImageButton;

        //        if (_ImageButton != null)
        //        {
        //            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(_ImageButton);
        //        }
        //    }
        //}

        #region Social Article Grid Events

        protected void grvSocialArticle_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(_ViewstateInformation.SocialArticleSortExpression))
                {
                    if (_ViewstateInformation.SocialArticleSortExpression.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_ViewstateInformation.IsSocialArticleSortDirecitonAsc == true)
                        {
                            _ViewstateInformation.IsSocialArticleSortDirecitonAsc = false;
                        }
                        else
                        {
                            _ViewstateInformation.IsSocialArticleSortDirecitonAsc = true;
                        }
                    }
                    else
                    {
                        _ViewstateInformation.SocialArticleSortExpression = e.SortExpression;
                        _ViewstateInformation.IsSocialArticleSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _ViewstateInformation.SocialArticleSortExpression = e.SortExpression;
                    _ViewstateInformation.IsSocialArticleSortDirecitonAsc = true;
                }

                ucCustomPagerSocialArticle.CurrentPage = 0;
                SetViewstateInformation(_ViewstateInformation);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                BindArchiveSM();
                SortClipDirection(_ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, grvSocialArticle);
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #endregion

        #region Remove Social Article

        protected void btnRemoveSocialArticle_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;
                for (int Count = 0; Count < grvSocialArticle.Rows.Count; Count++)
                {
                    HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)grvSocialArticle.Rows[Count].FindControl("chkSocialArticleDelete");
                    if (chkDelete.Checked == true)
                    {
                        strKeys = strKeys + chkDelete.Value + ",";
                    }
                }
                if (strKeys.Length > 0)
                {
                    strKeys = strKeys.Substring(0, strKeys.Length - 1);
                    string _Result = string.Empty;
                    ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();
                    _Result = _ISocialMediaController.DeleteArchiveSM(strKeys);
                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessMessageArticleSM.Text = "Record(s) deleted Successfully.";
                    }
                    ucCustomPagerSocialArticle.CurrentPage = 0;
                    BindArchiveSM();
                    SortClipDirection(_ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, grvSocialArticle);
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Edit Social Article

        protected void lbtnSocialArticleEdit_Click(object sender, EventArgs e)
        {
            try
            {
                hdnArticleType.Value = "SM";
                if (ddlPArticleCategory.Items.Count <= 0)
                {
                    BindArticleMediaCategoryDropDown();
                }

                HiddenField hfArticleIDEdit = (HiddenField)grvSocialArticle.Rows[((sender as LinkButton).NamingContainer as GridViewRow).RowIndex].FindControl("hfArchiveSMKey");
                ISocialMediaController _ISocialMediaController = _ControllerFactory.CreateObject<ISocialMediaController>();

                List<ArchiveSM> _ListOfArchiveSM = _ISocialMediaController.GetArchiveSMByArchiveSMKey(Convert.ToInt32(hfArticleIDEdit.Value));

                if (_ListOfArchiveSM.Count > 0)
                {
                    ddlPArticleCategory.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveSM[0].CategoryGuid)) ? "0" : Convert.ToString(_ListOfArchiveSM[0].CategoryGuid);
                    ddlArticleSubCategory1.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveSM[0].SubCategory1Guid)) ? "0" : Convert.ToString(_ListOfArchiveSM[0].SubCategory1Guid);
                    ddlArticleSubCategory2.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveSM[0].SubCategory2Guid)) ? "0" : Convert.ToString(_ListOfArchiveSM[0].SubCategory2Guid);
                    ddlArticleSubCategory3.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveSM[0].SubCategory3Guid)) ? "0" : Convert.ToString(_ListOfArchiveSM[0].SubCategory3Guid);

                    txtArticleTitle.Text = _ListOfArchiveSM[0].Title;
                    txtArticleDescription.Text = _ListOfArchiveSM[0].Description;
                    txtArticleKeywords.Text = _ListOfArchiveSM[0].Keywords;
                    hdnEditArchiveKey.Value = _ListOfArchiveSM[0].ArchiveSMKey.ToString();
                    if (_ListOfArchiveSM[0].Rating == 6)
                    { chkArticlePreferred.Checked = true; }
                    else
                    { chkArticlePreferred.Checked = false; }
                    txtArticleRate.Text = _ListOfArchiveSM[0].Rating.ToString();

                }

                string _Script = "UpdateSubCategory2(\"" + ddlPArticleCategory.ClientID + "\",1);" +
                "UpdateSubCategory2(\"" + ddlArticleSubCategory1.ClientID + "\",1);" +
                "UpdateSubCategory2(\"" + ddlArticleSubCategory2.ClientID + "\",1);";

                spnEditArtile.Text = "Edit Article Details";
                lblrateartile.InnerText = "Rate This Article :";

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadCatListArticle", _Script, true);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleEditModal", "ShowModal('pnlEditArticle');", true);
                //mdlpopupArticle.Show();
                UpdateUpdatePanel(upEditArticle);

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Download Social Article

        protected void btnArticleSMDownload_Click(object sender, EventArgs e)
        {
            try
            {
                IArchiveSMDownloadController _IArchiveSMDownloadController = _ControllerFactory.CreateObject<IArchiveSMDownloadController>();

                List<ArchiveSMDownload> _ListOfArchiveSMDownload = _IArchiveSMDownloadController.GetByCustomerGuid(new Guid(_SessionInformation.CustomerGUID));


                List<string> _ListOfSelectedArticleID = new List<string>();

                foreach (GridViewRow _GridViewRow in grvSocialArticle.Rows)
                {
                    HtmlInputCheckBox _HtmlInputCheckBox = (HtmlInputCheckBox)_GridViewRow.FindControl("chkSocialArticleDelete");

                    if (_HtmlInputCheckBox.Checked)
                    {
                        HiddenField _HiddenField = (HiddenField)_GridViewRow.FindControl("hfArticleSMID");

                        _ListOfSelectedArticleID.Add(_HiddenField.Value);

                        if (_ListOfSelectedArticleID.Count + (_ListOfArchiveSMDownload != null ? _ListOfArchiveSMDownload.Count : 0) > Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected]))
                        {
                            break;
                        }
                    }
                }


                if (_ListOfSelectedArticleID.Count + (_ListOfArchiveSMDownload != null ? _ListOfArchiveSMDownload.Count : 0) > Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected]))
                {
                    lblErrorMsgArticleSM.Text = "You cannot download more than " + ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected] + " articles at a time, please check your download screen.";
                    string _Script = "window.open('../DownloadArchiveSM/','DownloadArchiveSM','menubar=no,toolbar=no,location=0,width=490,height=600,resizable=yes,scrollbars=no')";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadArchiveSM", _Script, true);
                    lblErrorMsgArticleSM.Visible = true;
                }
                else
                {
                    if (_ListOfSelectedArticleID.Count + (_ListOfArchiveSMDownload != null ? _ListOfArchiveSMDownload.Count : 0) <= 0)
                    {
                        lblErrorMsgArticleSM.Text = "There doesn't exits any article to download. Please select atleast one article.";
                        lblErrorMsgArticleSM.Visible = true;
                    }
                    else
                    {
                        string _Script = "window.open('../DownloadArchiveSM/','DownloadArchiveSM','menubar=no,toolbar=no,location=0,width=490,height=600,resizable=yes,scrollbars=no')";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadArchiveSM", _Script, true);
                        _SessionInformation.ListOfSelectedArchiveSMDownload = _ListOfSelectedArticleID;
                    }


                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #endregion

        #region Print Media Events

        private void BindArchivePM()
        {
            if (!chkPrintMedia.Checked)
            {
                grvPrintMedia.DataSource = null;
                grvPrintMedia.DataBind();
                ucCustomPagerPM.Visible = false;
                return;
            }

            if (string.IsNullOrEmpty(_ViewstateInformation.PrintMediaSortExpression))
            {
                _ViewstateInformation.PrintMediaSortExpression = "PubDate";

            }

            if (ucCustomPagerPM.CurrentPage == null)
            {
                ucCustomPagerPM.CurrentPage = 0;
            }

            if (_ViewstateInformation.IsmyIQSearchActive == null)
            {
                _ViewstateInformation.IsmyIQSearchActive = false;
            }

            IArchiveBLPMController _IArchiveBLPMController = _ControllerFactory.CreateObject<IArchiveBLPMController>();
            int _TotalRecordsPrintMediaCount = 0;

            List<ArchiveBLPM> _ListOfArchiveBLPM = _IArchiveBLPMController.GetArchiveBLPMBySearch(new Guid(_SessionInformation.ClientGUID),
                                                                _ViewstateInformation._MyIQSearchParams.SearchTitle,
                                                                _ViewstateInformation._MyIQSearchParams.SearchDesc,
                                                                _ViewstateInformation._MyIQSearchParams.SearchKey,
                                                                _ViewstateInformation._MyIQSearchParams.SearchCC,
                                                                _ViewstateInformation._MyIQSearchParams.FromDate,
                                                                _ViewstateInformation._MyIQSearchParams.ToDate,
                                                                _ViewstateInformation._MyIQSearchParams.CategoryGUID1,
                                                                _ViewstateInformation._MyIQSearchParams.CategoryGUID2,
                                                                _ViewstateInformation._MyIQSearchParams.CategoryGUID3,
                                                                _ViewstateInformation._MyIQSearchParams.CategoryGUID4,
                                                                _ViewstateInformation._MyIQSearchParams.CategoryOperator1,
                                                                _ViewstateInformation._MyIQSearchParams.CategoryOperator2,
                                                                _ViewstateInformation._MyIQSearchParams.CategoryOperator3,
                                                                ucCustomPagerPM.CurrentPage.Value,
                                                                ucCustomPagerPM.PageSize,
                                                                _ViewstateInformation.PrintMediaSortExpression,
                                                                _ViewstateInformation.IsPrintMediaSortDirectionAsc,
                                                                out _TotalRecordsPrintMediaCount);

            _ViewstateInformation.TotalRecordsCountPrintMedia = _TotalRecordsPrintMediaCount;

            SetViewstateInformation(_ViewstateInformation);

            grvPrintMedia.Visible = true;

            grvPrintMedia.DataSource = _ListOfArchiveBLPM;
            grvPrintMedia.DataBind();

            imgbtnRefreshPrintMedia.Visible = true;
            btnManageCategories3.Visible = true;
            if (_ListOfArchiveBLPM == null || _ListOfArchiveBLPM.Count == 0)
            {
                ucCustomPagerPM.Visible = false;
                btnRemovePrintMedia.Visible = false;
                btnPrintMediaDownload.Visible = false;
                lnkEmailPrintMedia.Visible = false;
            }
            else
            {

                btnRemovePrintMedia.Visible = true;
                btnPrintMediaDownload.Visible = true;
                ucCustomPagerPM.Visible = true;
                lnkEmailPrintMedia.Visible = true;
                ucCustomPagerPM.TotalRecords = _TotalRecordsPrintMediaCount;
                ucCustomPagerPM.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                //ucCustomPager.CurrentPage = _ViewstateInformation._CurrentClipPage;
                ucCustomPagerPM.BindDataList();
            }
            grvPrintMedia.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;");

            if (Page.IsPostBack)
            {
                //string _Script = "SetSearchParam();";
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "SetSelection", _Script, true);
            }
        }

        #region Remove Print Media

        protected void btnRemovePrintMedia_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;
                for (int Count = 0; Count < grvPrintMedia.Rows.Count; Count++)
                {
                    HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)grvPrintMedia.Rows[Count].FindControl("chkPMDelete");
                    if (chkDelete.Checked == true)
                    {
                        strKeys = strKeys + chkDelete.Value + ",";
                    }
                }
                if (strKeys.Length > 0)
                {
                    strKeys = strKeys.Substring(0, strKeys.Length - 1);
                    string _Result = string.Empty;
                    IArchiveBLPMController _IArchiveBLPMController = _ControllerFactory.CreateObject<IArchiveBLPMController>();
                    _Result = _IArchiveBLPMController.DeleteArchivePM(strKeys);
                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessMessagePM.Text = "Record(s) deleted Successfully.";
                    }
                    ucCustomPagerPM.CurrentPage = 0;
                    BindArchivePM();
                    SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, grvPrintMedia);
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }


        #endregion

        #region Print Media Grid Events

        protected void imgPrintMediaButton_Command(object sender, CommandEventArgs e)
        {
            try
            {
                iFrameOnlineNewsArticle.Attributes.Add("src", e.CommandArgument.ToString());
                iFrameOnlineNewsArticle.Visible = true;

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleModal", "ShowModal('diviFrameOnlineNewsArticle');", true);
                //mpeArticlePopup.Show();
                upOnlineNewsArticle.Update();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        protected void grvPrintMedia_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(_ViewstateInformation.PrintMediaSortExpression))
                {
                    if (_ViewstateInformation.PrintMediaSortExpression.ToLower() == e.SortExpression.ToLower())
                    {
                        if (_ViewstateInformation.IsPrintMediaSortDirectionAsc == true)
                        {
                            _ViewstateInformation.IsPrintMediaSortDirectionAsc = false;
                        }
                        else
                        {
                            _ViewstateInformation.IsPrintMediaSortDirectionAsc = true;
                        }
                    }
                    else
                    {
                        _ViewstateInformation.PrintMediaSortExpression = e.SortExpression;
                        _ViewstateInformation.IsPrintMediaSortDirectionAsc = true;
                    }
                }
                else
                {
                    _ViewstateInformation.PrintMediaSortExpression = e.SortExpression;
                    _ViewstateInformation.IsPrintMediaSortDirectionAsc = true;
                }

                ucCustomPagerPM.CurrentPage = 0;
                SetViewstateInformation(_ViewstateInformation);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                BindArchivePM();
                SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, grvPrintMedia);
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }


        #endregion

        #region Print Media Download

        protected void btnPrintMediaDownload_Click(object sender, EventArgs e)
        {
            try
            {
                IArchiveBLPMDownloadController _IArchiveBLPMDownloadController = _ControllerFactory.CreateObject<IArchiveBLPMDownloadController>();

                List<ArchiveBLPMDownload> _ListOfArchiveBLPMDownload = _IArchiveBLPMDownloadController.GetByCustomerGuid(new Guid(_SessionInformation.CustomerGUID));


                List<string> _ListOfSelectedPMID = new List<string>();

                foreach (GridViewRow _GridViewRow in grvPrintMedia.Rows)
                {
                    HtmlInputCheckBox _HtmlInputCheckBox = (HtmlInputCheckBox)_GridViewRow.FindControl("chkPMDelete");

                    if (_HtmlInputCheckBox.Checked)
                    {
                        HiddenField _HiddenField = (HiddenField)_GridViewRow.FindControl("hfArchivePMKey");

                        _ListOfSelectedPMID.Add(_HiddenField.Value);

                        if (_ListOfSelectedPMID.Count + (_ListOfArchiveBLPMDownload != null ? _ListOfArchiveBLPMDownload.Count : 0) > Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected]))
                        {
                            break;
                        }
                    }
                }


                if (_ListOfSelectedPMID.Count + (_ListOfArchiveBLPMDownload != null ? _ListOfArchiveBLPMDownload.Count : 0) > Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected]))
                {
                    lblErrorMsgPM.Text = "You cannot download more than " + ConfigurationManager.AppSettings[CommonConstants.ConfigMaxNoOfClipsSelected] + " print media at a time, please check your download screen.";
                    string _Script = "window.open('../DownloadArchiveBLPM/','DownloadArchiveBLPM','menubar=no,toolbar=no,location=0,width=490,height=600,resizable=yes,scrollbars=no')";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadArchiveBLPM", _Script, true);
                    lblErrorMsgPM.Visible = true;
                }
                else
                {
                    if (_ListOfSelectedPMID.Count + (_ListOfArchiveBLPMDownload != null ? _ListOfArchiveBLPMDownload.Count : 0) <= 0)
                    {
                        lblErrorMsgPM.Text = "There doesn't exits any print media to download. Please select atleast one print media.";
                        lblErrorMsgPM.Visible = true;
                    }
                    else
                    {
                        //string _Script = "window.open('../DownloadClip/','DownloadClip','menubar=no,toolbar=no,location=0,width=450,height=600,resizable=yes,scrollbars=yes')";                        
                        string _Script = "window.open('../DownloadArchiveBLPM/','DownloadArchiveBLPM','menubar=no,toolbar=no,location=0,width=490,height=600,resizable=yes,scrollbars=no')";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadArchiveBLPM", _Script, true);
                        _SessionInformation.ListOfSelectedArchiveBLPMDownload = _ListOfSelectedPMID;
                    }


                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Print Media Edit Event

        protected void lbtnPrintMediaEdit_Click(object sender, EventArgs e)
        {
            try
            {
                hdnArticleType.Value = "PM";
                if (ddlPArticleCategory.Items.Count <= 0)
                {
                    BindArticleMediaCategoryDropDown();
                }

                HiddenField hfArticleIDEdit = (HiddenField)grvPrintMedia.Rows[((sender as LinkButton).NamingContainer as GridViewRow).RowIndex].FindControl("hfArchivePMKey");
                IArchiveBLPMController _IArchiveBLPMController = _ControllerFactory.CreateObject<IArchiveBLPMController>();

                List<ArchiveBLPM> _ListOfArchiveBLPM = _IArchiveBLPMController.GetArchivePMByArchiveBLPMKey(Convert.ToInt32(hfArticleIDEdit.Value));

                if (_ListOfArchiveBLPM.Count > 0)
                {
                    ddlPArticleCategory.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveBLPM[0].CategoryGuid)) ? "0" : Convert.ToString(_ListOfArchiveBLPM[0].CategoryGuid);
                    ddlArticleSubCategory1.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveBLPM[0].SubCategory1Guid)) ? "0" : Convert.ToString(_ListOfArchiveBLPM[0].SubCategory1Guid);
                    ddlArticleSubCategory2.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveBLPM[0].SubCategory2Guid)) ? "0" : Convert.ToString(_ListOfArchiveBLPM[0].SubCategory2Guid);
                    ddlArticleSubCategory3.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveBLPM[0].SubCategory3Guid)) ? "0" : Convert.ToString(_ListOfArchiveBLPM[0].SubCategory3Guid);

                    txtArticleTitle.Text = _ListOfArchiveBLPM[0].Headline;

                    txtArticleKeywords.Text = _ListOfArchiveBLPM[0].Keywords;
                    txtArticleDescription.Text = _ListOfArchiveBLPM[0].Description;
                    hdnEditArchiveKey.Value = _ListOfArchiveBLPM[0].ArchiveBLPMKey.ToString();
                    if (_ListOfArchiveBLPM[0].Rating == 6)
                    { chkArticlePreferred.Checked = true; }
                    else
                    { chkArticlePreferred.Checked = false; }
                    txtArticleRate.Text = _ListOfArchiveBLPM[0].Rating.ToString();

                }

                string _Script = "UpdateSubCategory2(\"" + ddlPArticleCategory.ClientID + "\",1);" +
                "UpdateSubCategory2(\"" + ddlArticleSubCategory1.ClientID + "\",1);" +
                "UpdateSubCategory2(\"" + ddlArticleSubCategory2.ClientID + "\",1);";

                spnEditArtile.Text = "Edit Article Details";
                lblrateartile.InnerText = "Rate This Article :";

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadCatListArticle", _Script, true);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleEditModal", "$('#liTitle').hide();ShowModal('pnlEditArticle');", true);
                //mdlpopupArticle.Show();
                UpdateUpdatePanel(upEditArticle);

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #endregion

        #region Twitter

        private void BindArchiveTweets(bool isBeingSorted)
        {
            if (!chkTwitter.Checked)
            {
                dlTweet.DataSource = null;
                dlTweet.DataBind();
                ucCustomePagerTweet.Visible = false;
                return;
            }

            if (string.IsNullOrEmpty(_ViewstateInformation.SortExpressionTweet))
            {
                _ViewstateInformation.SortExpressionTweet = "Tweet_PostedDateTime";
            }
            else
            {
                _ViewstateInformation.SortExpressionTweet = ddlTweetSortColumns.SelectedValue;
            }

            if (ucCustomePagerTweet.CurrentPage == null || isBeingSorted)
            {
                ucCustomePagerTweet.CurrentPage = 0;
            }

            if (_ViewstateInformation.IsmyIQSearchActive == null)
            {
                _ViewstateInformation.IsmyIQSearchActive = false;
            }

            IArchiveTweetsController _IArchiveTweetsController = _ControllerFactory.CreateObject<IArchiveTweetsController>();
            int _TotalRecordsTweetsCount = 0;



            List<ArchiveTweets> _ListOfArchiveTweets = _IArchiveTweetsController.GetArchiveTweetsBySearch(new Guid(_SessionInformation.ClientGUID),
                                                                    _ViewstateInformation._MyIQSearchParams.SearchTitle,
                                                                    _ViewstateInformation._MyIQSearchParams.SearchDesc,
                                                                    _ViewstateInformation._MyIQSearchParams.SearchKey,
                                                                    _ViewstateInformation._MyIQSearchParams.SearchCC,
                                                                    _ViewstateInformation._MyIQSearchParams.FromDate,
                                                                    _ViewstateInformation._MyIQSearchParams.ToDate,
                                                                    _ViewstateInformation._MyIQSearchParams.CategoryGUID1,
                                                                    _ViewstateInformation._MyIQSearchParams.CategoryGUID2,
                                                                    _ViewstateInformation._MyIQSearchParams.CategoryGUID3,
                                                                    _ViewstateInformation._MyIQSearchParams.CategoryGUID4,
                                                                    _ViewstateInformation._MyIQSearchParams.CategoryOperator1,
                                                                    _ViewstateInformation._MyIQSearchParams.CategoryOperator2,
                                                                    _ViewstateInformation._MyIQSearchParams.CategoryOperator3,
                                                                    _ViewstateInformation._MyIQSearchParams.CustomerGUID,
                                                                    ucCustomePagerTweet.CurrentPage.Value,
                                                                    ucCustomePagerTweet.PageSize,
                                                                    _ViewstateInformation.SortExpressionTweet,
                                                                    rdoTweetSort.SelectedValue == "0" ? true : false,
                                                                    out _TotalRecordsTweetsCount);

            _ViewstateInformation.TotalRecordsCountTweets = _TotalRecordsTweetsCount;

            SetViewstateInformation(_ViewstateInformation);

            dlTweet.Visible = true;



            dlTweet.DataSource = _ListOfArchiveTweets;
            dlTweet.DataBind();

            imgbtnRefreshTweet.Visible = true;
            btnManageCategoriesTweet.Visible = true;
            if (_ListOfArchiveTweets == null || _ListOfArchiveTweets.Count == 0)
            {
                ucCustomePagerTweet.Visible = false;
                btnRemoveTweet.Visible = false;
                lnkEmailTweet.Visible = false;
                divTweetSort.Visible = false;

                spnTweetNoData.Visible = true;
            }
            else
            {
                spnTweetNoData.Visible = false;
                divTweetSort.Visible = true;
                btnRemoveTweet.Visible = true;
                ucCustomePagerTweet.Visible = true;
                lnkEmailTweet.Visible = true;
                ucCustomePagerTweet.TotalRecords = _TotalRecordsTweetsCount;
                ucCustomePagerTweet.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                //ucCustomPager.CurrentPage = _ViewstateInformation._CurrentClipPage;
                ucCustomePagerTweet.BindDataList();
            }
            dlTweet.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;border:1px solid #999999;");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ValidateDatalist", "ValidateDataList('" + dlTweet.ClientID + "');", true);
        }

        protected void imgbtnRefreshTweet_Click(object sender, EventArgs e)
        {
            try
            {
                ClearSearch();
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
            string ConfigTwitterSortSettings = ConfigurationManager.AppSettings["TwitterSortSettingsMyiQ"];
            _TwitterSortIterms = ConfigTwitterSortSettings.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(part => part.Split('='))
               .ToDictionary(split => split[0], split => split[1]);

            ddlTweetSortColumns.DataSource = _TwitterSortIterms;
            ddlTweetSortColumns.DataTextField = "Value";
            ddlTweetSortColumns.DataValueField = "Key";
            ddlTweetSortColumns.DataBind();
        }

        #region Remove Twitter

        protected void btnRemoveTweet_Click(object sender, EventArgs e)
        {
            try
            {
                string strKeys = string.Empty;

                foreach (DataListItem dlItem in dlTweet.Items)
                {
                    HtmlInputCheckBox chkTweetSelect = (HtmlInputCheckBox)dlItem.FindControl("chkTweetSelect");
                    if (chkTweetSelect.Checked == true)
                    {
                        strKeys = strKeys + chkTweetSelect.Value + ",";
                    }
                }
                if (strKeys.Length > 0)
                {
                    strKeys = strKeys.Substring(0, strKeys.Length - 1);
                    string _Result = string.Empty;
                    IArchiveTweetsController _IArchiveTweetsController = _ControllerFactory.CreateObject<IArchiveTweetsController>();
                    _Result = _IArchiveTweetsController.DeleteArchiveTweets(strKeys);
                    if (!string.IsNullOrEmpty(_Result))
                    {
                        lblSuccessMessageTweet.Text = "Record(s) deleted Successfully.";
                    }
                    ucCustomePagerTweet.CurrentPage = 0;
                    BindArchiveTweets(false);
                    //SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, grvPrintMedia);
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Sort Twitter

        protected void btnSortTweet_Click(object sender, EventArgs e)
        {
            try
            {
                BindArchiveTweets(true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Twitter Paging

        protected void ucCustomePagerTweet_PageIndexChange(object sender, EventArgs e)
        {
            try
            {

                if (_ViewstateInformation != null)
                {

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
                    BindArchiveTweets(false);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Twitter Edit Event

        protected void lbtnTweetEdit_Click(object sender, EventArgs e)
        {
            try
            {
                hdnArticleType.Value = "Twitter";
                if (ddlPArticleCategory.Items.Count <= 0)
                {
                    BindArticleMediaCategoryDropDown();
                }

                HiddenField hfArticleIDEdit = (HiddenField)dlTweet.Items[((sender as LinkButton).NamingContainer as DataListItem).ItemIndex].FindControl("hfArchiveTweetsKey");
                IArchiveTweetsController _IArchiveTweetsController = _ControllerFactory.CreateObject<IArchiveTweetsController>();

                List<ArchiveTweets> _ListOfArchiveTweets = _IArchiveTweetsController.GetArchiVeTweetsByArchiveTweets_Key(Convert.ToInt32(hfArticleIDEdit.Value));

                if (_ListOfArchiveTweets.Count > 0)
                {
                    ddlPArticleCategory.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveTweets[0].CategoryGuid)) ? "0" : Convert.ToString(_ListOfArchiveTweets[0].CategoryGuid);
                    ddlArticleSubCategory1.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveTweets[0].SubCategory1Guid)) ? "0" : Convert.ToString(_ListOfArchiveTweets[0].SubCategory1Guid);
                    ddlArticleSubCategory2.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveTweets[0].SubCategory2Guid)) ? "0" : Convert.ToString(_ListOfArchiveTweets[0].SubCategory2Guid);
                    ddlArticleSubCategory3.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_ListOfArchiveTweets[0].SubCategory3Guid)) ? "0" : Convert.ToString(_ListOfArchiveTweets[0].SubCategory3Guid);

                    txtArticleTitle.Text = _ListOfArchiveTweets[0].Title;

                    txtArticleKeywords.Text = _ListOfArchiveTweets[0].Keywords;
                    txtArticleDescription.Text = _ListOfArchiveTweets[0].Description;
                    hdnEditArchiveKey.Value = _ListOfArchiveTweets[0].ArchiveTweets_Key.ToString();
                    if (_ListOfArchiveTweets[0].Rating == 6)
                    { chkArticlePreferred.Checked = true; }
                    else
                    { chkArticlePreferred.Checked = false; }
                    txtArticleRate.Text = _ListOfArchiveTweets[0].Rating.ToString();

                }

                string _Script = "UpdateSubCategory2(\"" + ddlPArticleCategory.ClientID + "\",1);" +
                "UpdateSubCategory2(\"" + ddlArticleSubCategory1.ClientID + "\",1);" +
                "UpdateSubCategory2(\"" + ddlArticleSubCategory2.ClientID + "\",1);";

                spnEditArtile.Text = "Edit Tweet Details";
                lblrateartile.InnerText = "Rate This Tweet :";

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadCatListArticle", _Script, true);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowArticleEditModal", "ShowModal('pnlEditArticle');", true);
                //mdlpopupArticle.Show();
                UpdateUpdatePanel(upEditArticle);

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #endregion

        #region Manage Category

        protected void btnManageCategories_Click(object sender, EventArgs e)
        {
            try
            {

                if (((ImageButton)sender).ID == btnManageCategories.ID)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                }
                else if (((ImageButton)sender).ID == btnManageCategories1.ID)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                }
                else if (((ImageButton)sender).ID == btnManageCategories2.ID)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                }
                else if (((ImageButton)sender).ID == btnManageCategories3.ID)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                }
                else if (((ImageButton)sender).ID == btnManageCategoriesTweet.ID)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
                }

                BindCustomCategory();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);
                //mpCustomCategory.Show();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void BindCustomCategory()
        {
            try
            {

                gvCustomCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                gvCustomCategory.DataBind();

                upCustomCategory.Update();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnSaveCustomCategory_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                if (txtCategoryName.Text.Trim() != string.Empty)
                {
                    GridViewRow _GridViewRow = grvClip.HeaderRow;
                    DropDownList _ddlCategory = null;

                    string _SelectedCategory = string.Empty;

                    if (_GridViewRow != null)
                    {
                        _ddlCategory = (DropDownList)_GridViewRow.FindControl("ddlCategory");
                        if (_ddlCategory != null)
                        {
                            _SelectedCategory = _ddlCategory.SelectedValue;
                        }
                    }

                    if (!string.IsNullOrEmpty(_SessionInformation.ClientGUID))
                    {
                        ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                        CustomCategory _CustomCategory = new CustomCategory();
                        _CustomCategory.CategoryName = txtCategoryName.Text.Trim();
                        _CustomCategory.CategoryDescription = txtCategoryDescription.Text.Trim();
                        _CustomCategory.IsActive = true;
                        _CustomCategory.ClientGUID = new Guid(_SessionInformation.ClientGUID);

                        string _CustomCategoryKey = _ICustomCategoryController.InsertCustomCategory(_CustomCategory);
                        if (!string.IsNullOrEmpty(_CustomCategoryKey) && Convert.ToInt64(_CustomCategoryKey) < 0)
                        {
                            lblMsg.Text = CommonConstants.CategoryAlreadyExists;
                            txtCategoryName.Focus();
                        }
                        else
                        {

                            GetCustomCategoryByClientGUID();
                            BindMediaCategoryCheckList();
                            BindCustomCategory();

                            if (ddlPArticleCategory.Items.Count > 0)
                            {
                                BindArticleMediaCategoryDropDown();
                            }

                            txtCategoryName.Text = string.Empty;
                            txtCategoryDescription.Text = string.Empty;
                            txtCategoryName.Focus();
                            lblMsg.Text = CommonConstants.CategorySavedSuccessfully;

                            if (_ddlCategory != null && !string.IsNullOrEmpty(_SelectedCategory))
                            {
                                _ddlCategory.SelectedValue = _SelectedCategory;
                            }

                        }
                    }


                }
                else
                {
                    lblMsg.Text = CommonConstants.CategoryAlreadyExists;
                    txtCategoryName.Text = string.Empty;
                    txtCategoryDescription.Text = string.Empty;
                    txtCategoryName.Focus();
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);

                ShowCurrentTab();
                /*if (hfCurrentTabIndex.Value == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "1")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "2")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "3")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                }*/


                UpdateUpdatePanel(upBtnSearch);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #region Custom Category Grid Events

        protected void gvCustomCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lblMsg.Text = string.Empty;

            if (e.CommandName == "DeleteRecord")
            {
                if (e.CommandArgument != null && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    LinkButton _LinkButton = e.CommandSource as LinkButton;

                    if (_LinkButton != null)
                    {
                        GridViewRow _GridViewRow = _LinkButton.NamingContainer as GridViewRow;

                        if (_GridViewRow != null)
                        {
                            Guid _CategoryGUIDDataKey = new Guid(Convert.ToString(gvCustomCategory.DataKeys[_GridViewRow.RowIndex].Value));

                            ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                            string _Deleted = _ICustomCategoryController.DeleteCustomCategory(Convert.ToInt64(e.CommandArgument.ToString()));
                            int Count = Convert.ToInt32(_Deleted);
                            if (!string.IsNullOrEmpty(_Deleted) && Count > 0)
                            {

                                lblMsg.Text = CommonConstants.CategoryDeletedSuccessfully;
                                GetCustomCategoryByClientGUID();
                                BindMediaCategoryCheckList();

                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);
                                //mpCustomCategory.Show();
                                BindCustomCategory();
                                UpdateUpdatePanel(upBtnSearch);

                            }
                            else
                            {
                                lblMsg.Text = CommonConstants.CategoryAssociatedWithClip;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);
                            }
                        }
                    }
                }


                ShowCurrentTab();

                /*if (hfCurrentTabIndex.Value == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "1")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "2")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "3")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                }*/
            }
        }

        protected void gvCustomCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnDelete = e.Row.FindControl("lbtnDelete") as LinkButton;
                LinkButton lbtnEdit = e.Row.FindControl("lbtnEdit") as LinkButton;
                if (lbtnDelete != null && lbtnEdit != null)
                {
                    CustomCategory _CustomCategory = e.Row.DataItem as CustomCategory;
                    if (_CustomCategory != null)
                    {
                        if (_CustomCategory.CategoryName.ToLower() == ConfigurationManager.AppSettings["DefaultCustomCategory"].ToLower())
                        {
                            Label _lblCategoryName = e.Row.FindControl("lblCategoryName") as Label;
                            if (_lblCategoryName != null)
                            {
                                _lblCategoryName.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                            }
                            lbtnDelete.Visible = false;
                            lbtnEdit.Visible = false;
                            e.Row.Cells[0].Text = "&nbsp;";

                        }
                    }
                }
            }
        }

        protected void gvCustomCategory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                gvCustomCategory.EditIndex = e.NewEditIndex;

                gvCustomCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                gvCustomCategory.DataBind();


                ShowCurrentTab();

                /*if (hfCurrentTabIndex.Value == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "1")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "2")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "3")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                }*/

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);
                //mpCustomCategory.Show();
                UpdateUpdatePanel(upCustomCategory);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvCustomCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCustomCategory.EditIndex = -1;
                if (_ViewstateInformation.ListOfCustomCategory != null)
                {
                    gvCustomCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    gvCustomCategory.DataBind();
                }

                ShowCurrentTab();

                /*if (hfCurrentTabIndex.Value == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "1")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "2")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "3")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                }*/

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);
                //mpCustomCategory.Show();
                UpdateUpdatePanel(upCustomCategory);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvCustomCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (_ViewstateInformation.ListOfCustomCategory != null)
                {
                    gvCustomCategory.PageIndex = e.NewPageIndex;
                    gvCustomCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    gvCustomCategory.DataBind();
                }


                ShowCurrentTab();
                /*if (hfCurrentTabIndex.Value == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "1")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "2")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                }
                else if (hfCurrentTabIndex.Value == "3")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                }*/

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);
                //mpCustomCategory.Show();
                UpdateUpdatePanel(upCustomCategory);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvCustomCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;

                TextBox _txtCategory = gvCustomCategory.Rows[gvCustomCategory.EditIndex].FindControl("txtCategoryName") as TextBox;
                TextBox _txtCategoryDescription = gvCustomCategory.Rows[gvCustomCategory.EditIndex].FindControl("txtCategoryDescription") as TextBox;
                HiddenField _hfCategoryKey = gvCustomCategory.Rows[gvCustomCategory.EditIndex].FindControl("hfCustomCategoryKey") as HiddenField;

                if (!string.IsNullOrEmpty(_hfCategoryKey.Value))
                {
                    ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                    CustomCategory _CustomCategory = new CustomCategory();

                    _CustomCategory.CategoryKey = Convert.ToInt64(_hfCategoryKey.Value);
                    _CustomCategory.CategoryName = _txtCategory.Text.Trim();
                    _CustomCategory.CategoryDescription = _txtCategoryDescription.Text.Trim();
                    _CustomCategory.ClientGUID = new Guid(_SessionInformation.ClientGUID);
                    string _Result = _ICustomCategoryController.UpdateCustomCategory(_CustomCategory);

                    if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) < 0)
                    {
                        lblMsg.Text = CommonConstants.CategoryAlreadyExists;
                        e.Cancel = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);
                        ShowCurrentTab();
                    }
                    else
                    {
                        gvCustomCategory.EditIndex = -1;
                        lblMsg.Text = "Category Updated Successfully.";

                        GetCustomCategoryByClientGUID();
                        BindMediaCategoryCheckList();
                        BindCustomCategory();

                        if (ddlPArticleCategory.Items.Count > 0)
                        {
                            BindArticleMediaCategoryDropDown();
                        }


                        ClearSearch();
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCustomCatModal", "ShowModal('pnlCustomCategory');", true);
                        //mpCustomCategory.Show();
                        UpdateUpdatePanel(upBtnSearch);
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

        #endregion Manage Category

        #region Search

        private void BindMediaCategoryCheckList()
        {

            try
            {

                if (_ViewstateInformation.ListOfCustomCategory != null && _ViewstateInformation.ListOfCustomCategory.Count > 0)
                {
                    ddlCategory1.DataTextField = "CategoryName";
                    ddlCategory1.DataValueField = "CategoryGUID";
                    ddlCategory1.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlCategory1.DataBind();
                    ddlCategory1.Items.Insert(0, new ListItem("All", "0"));

                    ddlCategory2.DataTextField = "CategoryName";
                    ddlCategory2.DataValueField = "CategoryGUID";
                    ddlCategory2.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlCategory2.DataBind();
                    ddlCategory2.Items.Insert(0, new ListItem("All", "0"));

                    ddlCategory3.DataTextField = "CategoryName";
                    ddlCategory3.DataValueField = "CategoryGUID";
                    ddlCategory3.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlCategory3.DataBind();
                    ddlCategory3.Items.Insert(0, new ListItem("All", "0"));

                    ddlCategory4.DataTextField = "CategoryName";
                    ddlCategory4.DataValueField = "CategoryGUID";
                    ddlCategory4.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlCategory4.DataBind();
                    ddlCategory4.Items.Insert(0, new ListItem("All", "0"));

                }

                upBtnSearch.Update();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BindCustomerCheckList()
        {
            try
            {

                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();

                List<Customer> _ListOfCustomer = new List<Customer>();
                _ListOfCustomer = _ICustomerController.GetCustomerNameByClientID(_SessionInformation.ClientID);

                var customList = _ListOfCustomer.AsQueryable().Select(cust => new { CustomerGUID = cust.CustomerGUID, Name = (cust.FirstName + " " + cust.LastName) });

                chkOwnerList.DataTextField = "Name";
                chkOwnerList.DataValueField = "CustomerGUID";
                chkOwnerList.DataSource = customList;
                chkOwnerList.DataBind();

                //chkOwnerList.Items.Insert(0, new ListItem("All", "0"));
                //chkOwnerList.Attributes.Add("onclick", "setCheckbox(this.id,'" + txtOwnerSelection.ClientID + "')");

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                if (ValidateSearch())
                {
                    _ViewstateInformation.IsmyIQSearchActive = false;
                    SetSearchParams();
                    SetViewstateInformation(_ViewstateInformation);
                    switch (GetGridToBind())
                    {
                        case 0:
                            hfTVStatus.Value = "1";
                            hfNewsStatus.Value = "0";
                            hfSocialMediaStatus.Value = "0";
                            hfPrintMediaStatus.Value = "0";

                            if (_SessionInformation.IsmyiQNM)
                            {
                                hfNewsStatus.Value = "0";
                                grvArticle.Visible = false;
                                imgbtnRefreshNews.Visible = false;
                                lnkEmailNews.Visible = false;
                                btnRemoveArticle.Visible = false;
                                btnArticleNMDownload.Visible = false;
                                ucCustomPagerArticle.Visible = false;
                            }

                            if (_SessionInformation.IsmyiQSM)
                            {
                                hfSocialMediaStatus.Value = "0";
                                grvSocialArticle.Visible = false;
                                imgbtnRefreshSocialMedia.Visible = false;
                                lnkEmailSocialMedia.Visible = false;
                                btnRemoveSocialArticle.Visible = false;
                                btnArticleSMDownload.Visible = false;
                                ucCustomPagerSocialArticle.Visible = false;
                            }

                            //Invisible Print Media
                            if (_SessionInformation.IsmyiQPM)
                            {
                                hfPrintMediaStatus.Value = "0";
                                grvPrintMedia.Visible = false;
                                imgbtnRefreshPrintMedia.Visible = false;
                                btnPrintMediaDownload.Visible = false;
                                btnRemovePrintMedia.Visible = false;
                                lnkEmailPrintMedia.Visible = false;
                                ucCustomPagerPM.Visible = false;
                            }

                            //invisible Tweet
                            if (_SessionInformation.IsMyIQTwitter)
                            {
                                hfTweetStatus.Value = "0";
                                dlTweet.Visible = false;

                                divTweetSort.Visible = false;
                                btnRemoveTweet.Visible = false;
                                lnkEmailTweet.Visible = false;
                                ucCustomePagerTweet.Visible = false;
                            }

                            ucCustomPager.CurrentPage = 0;
                            ClearArchiveClipGridFields();

                            hfCurrentTabIndex.Value = "0";
                            BindArchiveClips(true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                            SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);

                            break;
                        case 1:
                            hfNewsStatus.Value = "1";
                            hfTVStatus.Value = "0";
                            hfSocialMediaStatus.Value = "0";
                            hfPrintMediaStatus.Value = "0";

                            grvClip.Visible = false;
                            btnRefreshLibrary.Visible = false;
                            btnClipDownload.Visible = false;
                            btnRemoveClips.Visible = false;
                            lnkEmail.Visible = false;
                            ucCustomPager.Visible = false;

                            if (_SessionInformation.IsmyiQSM)
                            {
                                hfSocialMediaStatus.Value = "0";
                                grvSocialArticle.Visible = false;
                                imgbtnRefreshSocialMedia.Visible = false;
                                lnkEmailSocialMedia.Visible = false;
                                btnRemoveSocialArticle.Visible = false;
                                btnArticleSMDownload.Visible = false;
                                ucCustomPagerSocialArticle.Visible = false;
                            }


                            //Invisible Print Media
                            if (_SessionInformation.IsmyiQPM)
                            {
                                hfPrintMediaStatus.Value = "0";
                                grvPrintMedia.Visible = false;
                                imgbtnRefreshPrintMedia.Visible = false;
                                btnPrintMediaDownload.Visible = false;
                                btnRemovePrintMedia.Visible = false;
                                lnkEmailPrintMedia.Visible = false;
                                ucCustomPagerPM.Visible = false;
                            }

                            //invisible Tweet
                            if (_SessionInformation.IsMyIQTwitter)
                            {
                                hfTweetStatus.Value = "0";
                                dlTweet.Visible = false;

                                divTweetSort.Visible = false;
                                btnRemoveTweet.Visible = false;
                                lnkEmailTweet.Visible = false;
                                ucCustomePagerTweet.Visible = false;

                            }
                            ucCustomPagerArticle.CurrentPage = 0;

                            hfCurrentTabIndex.Value = "1";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                            BindArchiveNM();
                            SortClipDirection(_ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, grvArticle);
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "$('#tabs').tabs('select', 1);", true);

                            break;
                        case 2:
                            hfSocialMediaStatus.Value = "1";
                            hfTVStatus.Value = "0";
                            hfNewsStatus.Value = "0";
                            hfPrintMediaStatus.Value = "0";


                            grvClip.Visible = false;
                            btnRefreshLibrary.Visible = false;
                            btnClipDownload.Visible = false;
                            btnRemoveClips.Visible = false;
                            lnkEmail.Visible = false;
                            ucCustomPager.Visible = false;

                            if (_SessionInformation.IsmyiQNM)
                            {
                                hfNewsStatus.Value = "0";
                                grvArticle.Visible = false;
                                imgbtnRefreshNews.Visible = false;
                                lnkEmailNews.Visible = false;
                                btnRemoveArticle.Visible = false;
                                btnArticleNMDownload.Visible = false;
                                ucCustomPagerArticle.Visible = false;
                            }

                            //Invisible Print Media
                            if (_SessionInformation.IsmyiQPM)
                            {
                                hfPrintMediaStatus.Value = "0";
                                grvPrintMedia.Visible = false;
                                imgbtnRefreshPrintMedia.Visible = false;
                                btnPrintMediaDownload.Visible = false;
                                btnRemovePrintMedia.Visible = false;
                                lnkEmailPrintMedia.Visible = false;
                                ucCustomPagerPM.Visible = false;
                            }


                            //invisible Tweet
                            if (_SessionInformation.IsMyIQTwitter)
                            {
                                hfTweetStatus.Value = "0";
                                dlTweet.Visible = false;

                                divTweetSort.Visible = false;
                                btnRemoveTweet.Visible = false;
                                lnkEmailTweet.Visible = false;
                                ucCustomePagerTweet.Visible = false;

                            }

                            ucCustomPagerSocialArticle.CurrentPage = 0;

                            hfCurrentTabIndex.Value = "2";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                            BindArchiveSM();
                            SortClipDirection(_ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, grvSocialArticle);
                            break;

                        case 3:
                            hfPrintMediaStatus.Value = "1";
                            hfTVStatus.Value = "0";
                            hfNewsStatus.Value = "0";
                            hfSocialMediaStatus.Value = "0";

                            grvClip.Visible = false;
                            btnRefreshLibrary.Visible = false;
                            btnClipDownload.Visible = false;
                            btnRemoveClips.Visible = false;
                            lnkEmail.Visible = false;
                            ucCustomPager.Visible = false;

                            if (_SessionInformation.IsmyiQNM)
                            {
                                hfNewsStatus.Value = "0";
                                grvArticle.Visible = false;
                                imgbtnRefreshNews.Visible = false;
                                lnkEmailNews.Visible = false;
                                btnRemoveArticle.Visible = false;
                                btnArticleNMDownload.Visible = false;
                                ucCustomPagerArticle.Visible = false;
                            }

                            if (_SessionInformation.IsmyiQSM)
                            {
                                hfSocialMediaStatus.Value = "0";
                                grvSocialArticle.Visible = false;
                                imgbtnRefreshSocialMedia.Visible = false;
                                lnkEmailSocialMedia.Visible = false;
                                btnRemoveSocialArticle.Visible = false;
                                btnArticleSMDownload.Visible = false;
                                ucCustomPagerSocialArticle.Visible = false;
                            }

                            //invisible Tweet
                            if (_SessionInformation.IsMyIQTwitter)
                            {
                                hfTweetStatus.Value = "0";
                                dlTweet.Visible = false;

                                divTweetSort.Visible = false;
                                btnRemoveTweet.Visible = false;
                                lnkEmailTweet.Visible = false;
                                ucCustomePagerTweet.Visible = false;
                            }

                            ucCustomPagerPM.CurrentPage = 0;

                            hfCurrentTabIndex.Value = "3";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                            BindArchivePM();
                            SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, grvPrintMedia);
                            break;

                        case 4:

                            hfTVStatus.Value = "0";
                            hfTweetStatus.Value = "1";

                            grvClip.Visible = false;
                            btnRefreshLibrary.Visible = false;
                            btnClipDownload.Visible = false;
                            btnRemoveClips.Visible = false;
                            lnkEmail.Visible = false;
                            ucCustomPager.Visible = false;

                            if (_SessionInformation.IsmyiQNM)
                            {
                                hfNewsStatus.Value = "0";
                                grvArticle.Visible = false;
                                imgbtnRefreshNews.Visible = false;
                                lnkEmailNews.Visible = false;
                                btnRemoveArticle.Visible = false;
                                btnArticleNMDownload.Visible = false;
                                ucCustomPagerArticle.Visible = false;
                            }

                            if (_SessionInformation.IsmyiQSM)
                            {
                                hfSocialMediaStatus.Value = "0";
                                grvSocialArticle.Visible = false;
                                imgbtnRefreshSocialMedia.Visible = false;
                                lnkEmailSocialMedia.Visible = false;
                                btnRemoveSocialArticle.Visible = false;
                                btnArticleSMDownload.Visible = false;
                                ucCustomPagerSocialArticle.Visible = false;
                            }


                            //Invisible Print Media
                            if (_SessionInformation.IsmyiQPM)
                            {
                                hfPrintMediaStatus.Value = "0";
                                grvPrintMedia.Visible = false;
                                imgbtnRefreshPrintMedia.Visible = false;
                                btnPrintMediaDownload.Visible = false;
                                btnRemovePrintMedia.Visible = false;
                                lnkEmailPrintMedia.Visible = false;
                                ucCustomPagerPM.Visible = false;
                            }


                            ucCustomePagerTweet.CurrentPage = 0;

                            hfCurrentTabIndex.Value = "4";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab", "showTweetTab();", true);
                            BindArchiveTweets(false);
                            //SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, dlTweet);
                            break;

                    }
                }
                else
                {

                    ShowCurrentTab();

                    /*if (hfCurrentTabIndex.Value == "0")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                    }
                    else if (hfCurrentTabIndex.Value == "1")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                    }
                    else if (hfCurrentTabIndex.Value == "2")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                    }
                    else if (hfCurrentTabIndex.Value == "3")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                    }*/

                    //string _Script = "SetSearchParam();";
                    //_Script += "setCheckbox(\"" + chkCategories1.ClientID + "\",\"" + txtCat1Selection.ClientID + "\");";
                    //_Script += "setCheckbox(\"" + chkCategories2.ClientID + "\",\"" + txtCat2Selection.ClientID + "\");";
                    //_Script = "setCheckbox(\"" + chkOwnerList.ClientID + "\",\"" + txtOwnerSelection.ClientID + "\");";
                    //_Script += "$(\"#" + DivSearch.ClientID + "\").show();";
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetSelection", _Script, true);
                }
                ShowCurrentTab();
                upBtnSearch.Update();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void SetSearchParams()
        {
            MyIQSearchParams _SearchParams = new MyIQSearchParams();
            //if (_ViewstateInformation.IsmyIQSearchActive == true)
            //{

            #region Search Term Filter
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string _SearchTerm = txtSearch.Text.Trim();
                divSearchTermFilterStatus.InnerText = "ON";
                divSearchTermFilterStatus.Style.Add("color", "red");
                imgSearchTermFilter.Src = "~/images/filter-Selected.png";
                _ViewstateInformation.IsmyIQSearchActive = true;

                //if (cbAll.Checked)
                //{
                //    _SearchParams.SearchTitle = _SearchTerm;
                //    _SearchParams.SearchCC = _SearchTerm;
                //    _SearchParams.SearchKey = _SearchTerm;
                //    _SearchParams.SearchDesc = _SearchTerm;
                //}
                //else
                //{
                if (cbCC.Checked)
                {
                    _SearchParams.SearchCC = _SearchTerm;
                }

                if (cbDescription.Checked)
                {
                    _SearchParams.SearchDesc = _SearchTerm;
                }

                if (cbKeywords.Checked)
                {
                    _SearchParams.SearchKey = _SearchTerm;
                }

                if (cbTitle.Checked)
                {
                    _SearchParams.SearchTitle = _SearchTerm;
                }
                //}
            }
            else
            {
                divSearchTermFilterStatus.InnerText = "OFF";
                divSearchTermFilterStatus.Style.Add("color", "black");
                imgSearchTermFilter.Src = "~/images/filter.png";
            }
            #endregion

            #region Time Filter
            if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && !string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                divTimeFilterStatus.InnerText = "ON";
                divTimeFilterStatus.Style.Add("color", "red");
                imgTimeFilter.Src = "~/images/filter-Selected.png";
                _SearchParams.FromDate = Convert.ToDateTime(txtFromDate.Text);
                _SearchParams.ToDate = Convert.ToDateTime(txtToDate.Text);
                _ViewstateInformation.IsmyIQSearchActive = true;
            }
            else
            {
                divTimeFilterStatus.InnerText = "OFF";
                divTimeFilterStatus.Style.Add("color", "black");
                imgTimeFilter.Src = "~/images/filter.png";
            }
            #endregion

            #region Category 1 Filter

            if (ddlCategory1.SelectedValue != "0")
            {
                _SearchParams.CategoryGUID1 = new Guid(ddlCategory1.SelectedValue);
                if (ddlCategory2.SelectedValue != "0")
                {
                    _SearchParams.CategoryOperator1 = rdoCategoryOperator1.SelectedValue;
                    _SearchParams.CategoryGUID2 = new Guid(ddlCategory2.SelectedValue);

                    if (ddlCategory3.SelectedValue != "0")
                    {
                        _SearchParams.CategoryOperator2 = rdoCategoryOperator2.SelectedValue;
                        _SearchParams.CategoryGUID3 = new Guid(ddlCategory3.SelectedValue);

                        if (ddlCategory4.SelectedValue != "0")
                        {
                            _SearchParams.CategoryOperator3 = rdoCategoryOperator3.SelectedValue;
                            _SearchParams.CategoryGUID4 = new Guid(ddlCategory4.SelectedValue);
                        }
                    }
                }
            }


            if (_SearchParams.CategoryGUID1 != null)
            {
                divCategoryFilterStatus1.InnerText = "ON";
                divCategoryFilterStatus1.Style.Add("color", "red");
                imgCategoryFilter1.Src = "~/images/filter-Selected.png";
                _ViewstateInformation.IsmyIQSearchActive = true;
            }
            else
            {
                divCategoryFilterStatus1.InnerText = "OFF";
                divCategoryFilterStatus1.Style.Add("color", "black");
                imgCategoryFilter1.Src = "~/images/filter.png";
            }

            #endregion

            #region Owner Filter
            if (!chkUserAll.Checked)
            {
                foreach (ListItem li in chkOwnerList.Items)
                {
                    if (li.Selected)
                    {
                        _SearchParams.CustomerGUID += "'" + li.Value + "'" + ",";
                    }
                }
            }
            if (_SearchParams.CustomerGUID != null && _SearchParams.CustomerGUID.IndexOf(",") > 0)
            {
                divUserFilterStatus.InnerText = "ON";
                divUserFilterStatus.Style.Add("color", "red");
                imgUserFilter.Src = "~/images/filter-Selected.png";
                _SearchParams.CustomerGUID = _SearchParams.CustomerGUID.Substring(0, _SearchParams.CustomerGUID.Length - 1);
                _ViewstateInformation.IsmyIQSearchActive = true;
            }
            else
            {
                divUserFilterStatus.InnerText = "OFF";
                divUserFilterStatus.Style.Add("color", "black");
                imgUserFilter.Src = "~/images/filter.png";
            }
            #endregion
            //}

            _ViewstateInformation.IsArticleSortDirecitonAsc = false;
            _ViewstateInformation.IsClipSortDirecitonAsc = false;
            _ViewstateInformation.ClipSortExpression = string.Empty;
            _ViewstateInformation.ArticleSortExpression = string.Empty;
            _ViewstateInformation._MyIQSearchParams = _SearchParams;
            SetViewstateInformation(_ViewstateInformation);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ClearSearch();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void ClearSearch()
        {
            try
            {

                _ViewstateInformation._MyIQSearchParams = new MyIQSearchParams();
                _ViewstateInformation.ClipSortExpression = "ClipCreationDate";
                _ViewstateInformation.IsClipSortDirecitonAsc = false;
                _ViewstateInformation.ArticleSortExpression = "CreatedDate";
                _ViewstateInformation.IsArticleSortDirecitonAsc = false;
                _ViewstateInformation.SocialArticleSortExpression = "CreatedDate";
                _ViewstateInformation.IsSocialArticleSortDirecitonAsc = false;


                //_ViewstateInformation._CurrentClipPage = 0;
                ucCustomPager.CurrentPage = 0;
                ucCustomPagerArticle.CurrentPage = 0;
                ucCustomPagerSocialArticle.CurrentPage = 0;

                txtSearch.Text = string.Empty;

                divSearchTermFilterStatus.InnerText = "OFF";
                divSearchTermFilterStatus.Style.Add("color", "black");
                imgSearchTermFilter.Src = "~/images/filter.png";


                //cbAll.Checked = true;
                cbCC.Checked = true;
                cbDescription.Checked = true;
                cbKeywords.Checked = true;
                cbTitle.Checked = true;

                //divSearchTermFilterStatus.InnerText = "OFF";
                //divSearchTermFilterStatus.Style.Add("color", "black");
                //imgSearchTermFilter.Src = "~/images/filter.png";

                txtFromDate.Text = "";
                txtToDate.Text = "";

                divTimeFilterStatus.InnerText = "OFF";
                divTimeFilterStatus.Style.Add("color", "black");
                imgTimeFilter.Src = "~/images/filter.png";


                ddlCategory1.SelectedValue = "0";
                ddlCategory2.SelectedValue = "0";
                ddlCategory3.SelectedValue = "0";
                ddlCategory4.SelectedValue = "0";
                rdoCategoryOperator1.SelectedIndex = 1;
                rdoCategoryOperator2.SelectedIndex = 1;
                rdoCategoryOperator3.SelectedIndex = 1;
                divCategoryFilterStatus1.InnerText = "OFF";
                divCategoryFilterStatus1.Style.Add("color", "black");
                imgCategoryFilter1.Src = "~/images/filter.png";

                chkUserAll.Checked = true;
                divUserFilterStatus.InnerText = "OFF";
                divUserFilterStatus.Style.Add("color", "black");
                imgUserFilter.Src = "~/images/filter.png";
                //txtCat1Selection.Text = "";
                //txtCat2Selection.Text = "";
                //txtOwnerSelection.Text = "";
                //if (_ViewstateInformation.IsmyIQSearchActive == true)
                //{
                if (hfCurrentTabIndex.Value == "1")
                {
                    hfTVStatus.Value = "0";
                    hfNewsStatus.Value = "1";

                    //Invisible TV
                    grvClip.Visible = false;
                    btnRefreshLibrary.Visible = false;
                    btnClipDownload.Visible = false;
                    btnRemoveClips.Visible = false;
                    lnkEmail.Visible = false;
                    ucCustomPager.Visible = false;

                    //Invisible Social media
                    if (_SessionInformation.IsmyiQSM)
                    {
                        hfSocialMediaStatus.Value = "0";
                        grvSocialArticle.Visible = false;
                        imgbtnRefreshSocialMedia.Visible = false;
                        lnkEmailSocialMedia.Visible = false;
                        btnRemoveSocialArticle.Visible = false;
                        btnArticleSMDownload.Visible = false;
                        ucCustomPagerSocialArticle.Visible = false;
                    }

                    //Invisible Print Media
                    if (_SessionInformation.IsmyiQPM)
                    {
                        hfPrintMediaStatus.Value = "0";
                        grvPrintMedia.Visible = false;
                        imgbtnRefreshPrintMedia.Visible = false;
                        btnPrintMediaDownload.Visible = false;
                        btnRemovePrintMedia.Visible = false;
                        lnkEmailPrintMedia.Visible = false;
                        ucCustomPagerPM.Visible = false;
                    }

                    //invisible Tweet
                    if (_SessionInformation.IsMyIQTwitter)
                    {
                        hfTweetStatus.Value = "0";
                        dlTweet.Visible = false;

                        divTweetSort.Visible = false;
                        btnRemoveTweet.Visible = false;
                        lnkEmailTweet.Visible = false;
                        ucCustomePagerTweet.Visible = false;
                    }
                    BindArchiveNM();
                    SortClipDirection(_ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, grvArticle);

                    string _Script = "CheckUncheckAll('" + chkOwnerList.ClientID + "','" + chkUserAll.ClientID + "');showOnlineNewsTab();";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ResetUserList", _Script, true);
                }
                else if (hfCurrentTabIndex.Value == "2")
                {
                    hfTVStatus.Value = "0";
                    hfSocialMediaStatus.Value = "1";

                    grvClip.Visible = false;
                    btnRefreshLibrary.Visible = false;
                    btnClipDownload.Visible = false;
                    btnRemoveClips.Visible = false;
                    lnkEmail.Visible = false;
                    ucCustomPager.Visible = false;

                    if (_SessionInformation.IsmyiQNM)
                    {
                        hfNewsStatus.Value = "0";
                        grvArticle.Visible = false;
                        imgbtnRefreshNews.Visible = false;
                        lnkEmailNews.Visible = false;
                        btnRemoveArticle.Visible = false;
                        btnArticleNMDownload.Visible = false;
                        ucCustomPagerArticle.Visible = false;
                    }

                    //Invisible Print Media
                    if (_SessionInformation.IsmyiQPM)
                    {
                        hfPrintMediaStatus.Value = "0";
                        grvPrintMedia.Visible = false;
                        imgbtnRefreshPrintMedia.Visible = false;
                        btnPrintMediaDownload.Visible = false;
                        btnRemovePrintMedia.Visible = false;
                        lnkEmailPrintMedia.Visible = false;
                        ucCustomPagerPM.Visible = false;
                    }

                    //invisible Tweet
                    if (_SessionInformation.IsMyIQTwitter)
                    {
                        hfTweetStatus.Value = "0";
                        dlTweet.Visible = false;

                        divTweetSort.Visible = false;
                        btnRemoveTweet.Visible = false;
                        lnkEmailTweet.Visible = false;
                        ucCustomePagerTweet.Visible = false;
                    }

                    BindArchiveSM();
                    SortClipDirection(_ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, grvSocialArticle);
                    string _Script = "CheckUncheckAll('" + chkOwnerList.ClientID + "','" + chkUserAll.ClientID + "');showSocialMediaTab();";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ResetUserList", _Script, true);
                }
                else if (hfCurrentTabIndex.Value == "3")
                {
                    hfPrintMediaStatus.Value = "1";
                    hfTVStatus.Value = "0";

                    //invisible Clip
                    grvClip.Visible = false;
                    btnRefreshLibrary.Visible = false;
                    btnClipDownload.Visible = false;
                    btnRemoveClips.Visible = false;
                    lnkEmail.Visible = false;
                    ucCustomPager.Visible = false;

                    //invisible Online News
                    if (_SessionInformation.IsmyiQNM)
                    {
                        hfNewsStatus.Value = "0";
                        grvArticle.Visible = false;
                        imgbtnRefreshNews.Visible = false;
                        lnkEmailNews.Visible = false;
                        btnRemoveArticle.Visible = false;
                        btnArticleNMDownload.Visible = false;
                        ucCustomPagerArticle.Visible = false;
                    }

                    //invisible Social Media
                    if (_SessionInformation.IsmyiQSM)
                    {
                        hfSocialMediaStatus.Value = "0";
                        grvSocialArticle.Visible = false;
                        imgbtnRefreshSocialMedia.Visible = false;
                        lnkEmailSocialMedia.Visible = false;
                        btnRemoveSocialArticle.Visible = false;
                        btnArticleSMDownload.Visible = false;
                        ucCustomPagerSocialArticle.Visible = false;
                    }


                    //invisible Tweet
                    if (_SessionInformation.IsMyIQTwitter)
                    {
                        hfTweetStatus.Value = "0";
                        dlTweet.Visible = false;

                        divTweetSort.Visible = false;
                        btnRemoveTweet.Visible = false;
                        lnkEmailTweet.Visible = false;
                        ucCustomePagerTweet.Visible = false;
                    }

                    ucCustomPagerPM.CurrentPage = 0;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                    BindArchivePM();
                    SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, grvPrintMedia);
                }
                else if (hfCurrentTabIndex.Value == "0")
                {
                    hfTVStatus.Value = "1";
                    if (_SessionInformation.IsmyiQNM)
                    {
                        hfNewsStatus.Value = "0";
                        grvArticle.Visible = false;
                        imgbtnRefreshNews.Visible = false;
                        lnkEmailNews.Visible = false;
                        btnRemoveArticle.Visible = false;
                        btnArticleNMDownload.Visible = false;
                        ucCustomPagerArticle.Visible = false;
                    }

                    if (_SessionInformation.IsmyiQSM)
                    {
                        hfSocialMediaStatus.Value = "0";
                        grvSocialArticle.Visible = false;
                        imgbtnRefreshSocialMedia.Visible = false;
                        lnkEmailSocialMedia.Visible = false;
                        btnRemoveSocialArticle.Visible = false;
                        btnArticleSMDownload.Visible = false;
                        ucCustomPagerSocialArticle.Visible = false;
                    }

                    //Invisible Print Media
                    if (_SessionInformation.IsmyiQPM)
                    {
                        hfPrintMediaStatus.Value = "0";
                        grvPrintMedia.Visible = false;
                        imgbtnRefreshPrintMedia.Visible = false;
                        btnPrintMediaDownload.Visible = false;
                        btnRemovePrintMedia.Visible = false;
                        lnkEmailPrintMedia.Visible = false;
                        ucCustomPagerPM.Visible = false;
                    }

                    //invisible Tweet
                    if (_SessionInformation.IsMyIQTwitter)
                    {
                        hfTweetStatus.Value = "0";
                        dlTweet.Visible = false;

                        divTweetSort.Visible = false;
                        btnRemoveTweet.Visible = false;
                        lnkEmailTweet.Visible = false;
                        ucCustomePagerTweet.Visible = false;
                    }

                    ClearArchiveClipGridFields();
                    BindArchiveClips(true);
                    SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);
                    string _Script = "CheckUncheckAll('" + chkOwnerList.ClientID + "','" + chkUserAll.ClientID + "');showTVTab();";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ResetUserList", _Script, true);
                }
                else if (hfCurrentTabIndex.Value == "4")
                {
                    hfTVStatus.Value = "0";
                    hfTweetStatus.Value = "1";
                    hfCurrentTabIndex.Value = "4";
                    grvClip.Visible = false;
                    btnRefreshLibrary.Visible = false;
                    btnClipDownload.Visible = false;
                    btnRemoveClips.Visible = false;
                    lnkEmail.Visible = false;
                    ucCustomPager.Visible = false;

                    ddlTweetSortColumns.SelectedIndex = 0;
                    rdoTweetSort.SelectedIndex = 0;

                    if (_SessionInformation.IsmyiQNM)
                    {
                        hfNewsStatus.Value = "0";
                        grvArticle.Visible = false;
                        imgbtnRefreshNews.Visible = false;
                        lnkEmailNews.Visible = false;
                        btnRemoveArticle.Visible = false;
                        btnArticleNMDownload.Visible = false;
                        ucCustomPagerArticle.Visible = false;
                    }

                    if (_SessionInformation.IsmyiQSM)
                    {
                        hfSocialMediaStatus.Value = "0";
                        grvSocialArticle.Visible = false;
                        imgbtnRefreshSocialMedia.Visible = false;
                        lnkEmailSocialMedia.Visible = false;
                        btnRemoveSocialArticle.Visible = false;
                        btnArticleSMDownload.Visible = false;
                        ucCustomPagerSocialArticle.Visible = false;
                    }


                    //Invisible Print Media
                    if (_SessionInformation.IsmyiQPM)
                    {
                        hfPrintMediaStatus.Value = "0";
                        grvPrintMedia.Visible = false;
                        imgbtnRefreshPrintMedia.Visible = false;
                        btnPrintMediaDownload.Visible = false;
                        btnRemovePrintMedia.Visible = false;
                        lnkEmailPrintMedia.Visible = false;
                        ucCustomPagerPM.Visible = false;
                    }


                    ucCustomePagerTweet.CurrentPage = 0;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab", "showTweetTab();", true);
                    BindArchiveTweets(false);
                    

                }

                //}

                _ViewstateInformation.IsmyIQSearchActive = false;
                SetViewstateInformation(_ViewstateInformation);

                UpdateUpdatePanel(upMainSearch);
                UpdateUpdatePanel(upBtnSearch);
                UpdateUpdatePanel(upMainGrid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected bool ValidateSearch()
        {
            lblSearchErr.Text = "";
            bool validate = true;

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                if (!cbCC.Checked && !cbKeywords.Checked && !cbDescription.Checked && !cbTitle.Checked)
                {
                    validate = false;
                    lblSearchErr.Text += "Please Select Atleast One Search Criteria<br/>";
                }
            }

            if ((!string.IsNullOrWhiteSpace(txtFromDate.Text) && string.IsNullOrWhiteSpace(txtToDate.Text)) || (!string.IsNullOrWhiteSpace(txtToDate.Text) && string.IsNullOrWhiteSpace(txtFromDate.Text)))
            {
                validate = false;
                lblSearchErr.Text += "Please Select From Date and To Date<br/>";
            }

            if (ddlCategory2.SelectedValue != "0" && (ddlCategory1.SelectedValue == "0"))
            {
                validate = false;
                lblSearchErr.Text += "Please first select all preceding categories.<br/>";
            }
            else if (ddlCategory3.SelectedValue != "0" && (ddlCategory2.SelectedValue == "0" || ddlCategory1.SelectedValue == "0"))
            {
                validate = false;
                lblSearchErr.Text += "Please first select all preceding categories.<br/>";
            }
            else if (ddlCategory4.SelectedValue != "0" && (ddlCategory3.SelectedValue == "0" || ddlCategory2.SelectedValue == "0" || ddlCategory1.SelectedValue == "0"))
            {
                validate = false;
                lblSearchErr.Text += "Please first select all preceding categories.<br/>";
            }

            if (!chkTV.Checked && !chkNews.Checked && !chkSocialMedia.Checked && !chkPrintMedia.Checked)
            {
                validate = false;
                lblSearchErr.Text += "Atleast one filter must be selected.<br/>";
            }

            return validate;
        }

        protected int GetGridToBind()
        {
            try
            {
                if (chkTV.Checked)
                {
                    tabTV.Visible = true;
                    divTVResult.Visible = true;
                }
                else
                {
                    tabTV.Visible = false;
                    divTVResult.Visible = false;

                }
                if (chkNews.Checked && _SessionInformation.IsmyiQNM)
                {
                    tabNews.Visible = true;
                    divNewsResult.Visible = true;
                }
                else
                {
                    tabNews.Visible = false;
                    divNewsResult.Visible = false;
                }


                if (chkSocialMedia.Checked && _SessionInformation.IsmyiQSM)
                {
                    tabSocialMedia.Visible = true;
                    divSocialMediaResult.Visible = true;
                }
                else
                {
                    tabSocialMedia.Visible = false;
                    divSocialMediaResult.Visible = false;
                }

                if (chkPrintMedia.Checked && _SessionInformation.IsmyiQPM)
                {
                    tabPrintMedia.Visible = true;
                    divPrintMediaResult.Visible = true;
                }
                else
                {
                    tabPrintMedia.Visible = false;
                    divPrintMediaResult.Visible = false;
                }

                if (chkTwitter.Checked)
                {
                    tabTweet.Visible = true;
                    divTweetResult.Visible = true;
                }
                else
                {
                    tabTweet.Visible = false;
                    divTweetResult.Visible = false;
                }

                upMainGrid.Update();

                switch (hfCurrentTabIndex.Value)
                {
                    case "0":
                        if (chkTV.Checked)
                            return 0;
                        break;
                    case "1":
                        if (chkNews.Checked && _SessionInformation.IsmyiQNM)
                            return 1;
                        break;
                    case "2":
                        if (chkSocialMedia.Checked && _SessionInformation.IsmyiQSM)
                            return 2;
                        break;

                    case "3":
                        if (chkPrintMedia.Checked && _SessionInformation.IsmyiQPM)
                            return 3;
                        break;

                    case "4":
                        if (chkTwitter.Checked && _SessionInformation.IsMyIQTwitter)
                            return 4;
                        break;
                }

                if (chkTV.Checked)
                    return 0;
                else if (chkNews.Checked && _SessionInformation.IsmyiQNM)
                    return 1;
                else if (chkSocialMedia.Checked && _SessionInformation.IsmyiQSM)
                    return 2;
                else if (chkPrintMedia.Checked && _SessionInformation.IsmyiQPM)
                    return 3;
                else if (chkTwitter.Checked && _SessionInformation.IsMyIQTwitter)
                    return 4;
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
            return 0;
        }

        #endregion Search

        #region CustomCategory

        private void GetCustomCategoryByClientGUID()
        {
            try
            {

                string _ClientGUID = _SessionInformation.ClientGUID;
                List<CustomCategory> _ListofCustomCategory = new List<CustomCategory>();

                ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                _ListofCustomCategory = _ICustomCategoryController.SelectByClientGUID(new Guid(_ClientGUID));

                _ViewstateInformation.ListOfCustomCategory = _ListofCustomCategory;
                SetViewstateInformation(_ViewstateInformation);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CustomCategory

        #region Paging and Sorting

        protected void ucCustomPager_PageIndexChange(object sender, EventArgs e)// int currentpageNumber)
        {
            try
            {

                if (_ViewstateInformation != null)
                {
                    // _ViewstateInformation._CurrentClipPage = currentpageNumber;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
                    BindArchiveClips(false);
                    SortClipDirection(_ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, grvClip);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ucCustomPagerArticle_PageIndexChange(object sender, EventArgs e)
        {
            try
            {

                if (_ViewstateInformation != null)
                {

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
                    BindArchiveNM();
                    SortClipDirection(_ViewstateInformation.ArticleSortExpression, _ViewstateInformation.IsArticleSortDirecitonAsc, grvArticle);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ucCustomPagerSocialArticle_PageIndexChange(object sender, EventArgs e)
        {
            try
            {

                if (_ViewstateInformation != null)
                {

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
                    BindArchiveSM();
                    SortClipDirection(_ViewstateInformation.SocialArticleSortExpression, _ViewstateInformation.IsSocialArticleSortDirecitonAsc, grvSocialArticle);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ucCustomPagerPM_PageIndexChange(object sender, EventArgs e)
        {
            try
            {

                if (_ViewstateInformation != null)
                {

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
                    BindArchivePM();
                    SortClipDirection(_ViewstateInformation.PrintMediaSortExpression, _ViewstateInformation.IsPrintMediaSortDirectionAsc, grvPrintMedia);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

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

                                if (sortDirectionAsc == true)
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

                            headerSearchButton.Controls.Add(divSearch);
                        }
                    }
                }
            }
        }


        #endregion

        public void ShowCurrentTab()
        {
            if (hfCurrentTabIndex.Value == "0")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab1", "showTVTab();", true);
            }
            else if (hfCurrentTabIndex.Value == "1")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab2", "showOnlineNewsTab();", true);
            }
            else if (hfCurrentTabIndex.Value == "2")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab3", "showSocialMediaTab();", true);
            }
            else if (hfCurrentTabIndex.Value == "3")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab4", "showPrintMediaTab();", true);
            }
            else if (hfCurrentTabIndex.Value == "4")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowTab5", "showTweetTab();", true);
            }



        }

    }
}
