using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.Data;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Core.Enumeration;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Drawing.Imaging;
using System.Collections;
using IQMediaGroup.Controller.Common;
using System.Reflection;
using System.Diagnostics;
using IQMediaGroup.Core.App_Code;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.MyClips
{
    public partial class MyClips : BaseControl
    {
        #region Member Variables

        public string _clipID = string.Empty;

        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        List<IQMediaGroup.Core.HelperClasses.Caption> _ListOfRCatpion = null;
        //int _NoOfResultsFromClip = 21;

        #endregion

        #region page events



        protected override void OnLoad(EventArgs e)
        {
            try
            {

                lblNoResults.Visible = false;
                lblNoResults.Text = string.Empty;

                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                //_NoOfResultsFromClip = grvClip.PageSize + 1;
                //updProgressEditClip.AssociatedUpdatePanelID = upEditClip.UniqueID;
                if (!IsPostBack)
                {
                    GetCustomCategoryByClientGUID();
                    BindMediaCategoryCheckList();
                    BindCustomerCheckList();
                    BindArchiveClips(true);
                    if (_SessionInformation.IsDownloadClips == true)
                    {
                        btnClipDownload.Visible = true;
                    }
                    else
                    {
                        btnClipDownload.Visible = false;
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetSearchParam", "SetSearchParam();", true);

                }
                UpdateUpdatePanel(upMainGrid);
                chkCategories1.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkCategories1.ClientID + "')");
                chkCategories2.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkCategories2.ClientID + "')");
                chkOwnerList.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkOwnerList.ClientID + "')");

                SortClipDirection();
                lblSuccessMessage.Text = string.Empty;
                lblError.Text = string.Empty;
                lblMsg.Text = string.Empty;
                lblErrorClips.Text = string.Empty;
                lblErrorClips.Visible = false;
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }


        #endregion

        #region Bind Clip

        private void SortClipDirection()
        {
            ViewstateInformation _ViewstateInformation = GetViewstateInformation();

            GridViewRow gridViewHeaderSearchRow = grvClip.HeaderRow;

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


                            if (headerSearchButton.CommandArgument == _ViewstateInformation.ClipSortExpression)
                            {
                                Image headerSearchImage = new Image();

                                if (_ViewstateInformation.IsClipSortDirecitonAsc == true)
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

        private void BindArchiveClips(bool p_IsInitialization)
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

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

                string SearchTitle = string.Empty;
                string SearchDesc = string.Empty;
                string SearchKey = string.Empty;
                string SearchCC = string.Empty;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                string CategoryGUID1 = string.Empty;
                string CategoryGUID2 = string.Empty;
                string CustomerGUID = string.Empty;
                lblActiveSearch.Text = string.Empty;
                if (_ViewstateInformation.IsmyIQSearchActive == true)
                {

                    lblActiveSearch.Text = "Search Active!";
                    if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        string _SearchTerm = txtSearch.Text.Trim();

                        if (cbAll.Checked)
                        {
                            SearchTitle = _SearchTerm;
                            SearchCC = _SearchTerm;
                            SearchKey = _SearchTerm;
                            SearchDesc = _SearchTerm;
                        }
                        else
                        {
                            if (cbCC.Checked)
                            {
                                SearchCC = _SearchTerm;
                            }

                            if (cbDescription.Checked)
                            {
                                SearchDesc = _SearchTerm;
                            }

                            if (cbKeywords.Checked)
                            {
                                SearchKey = _SearchTerm;
                            }

                            if (cbTitle.Checked)
                            {
                                SearchTitle = _SearchTerm;
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && !string.IsNullOrWhiteSpace(txtToDate.Text))
                    {
                        FromDate = Convert.ToDateTime(txtFromDate.Text);
                        ToDate = Convert.ToDateTime(txtToDate.Text);
                    }
                    foreach (ListItem li in chkCategories1.Items)
                    {
                        if (li.Selected)
                        {
                            if (String.Compare(li.Value, "0", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                CategoryGUID1 = string.Empty;
                                break;
                            }
                            CategoryGUID1 += "'" + li.Value + "'" + ",";
                        }
                    }
                    if (CategoryGUID1.IndexOf(",") > 0)
                    {
                        CategoryGUID1 = CategoryGUID1.Substring(0, CategoryGUID1.Length - 1);
                    }

                    foreach (ListItem li in chkCategories2.Items)
                    {
                        if (li.Selected)
                        {
                            if (String.Compare(li.Value, "0", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                CategoryGUID2 = string.Empty;
                                break;
                            }
                            CategoryGUID2 += "'" + li.Value + "'" + ",";
                        }
                    }
                    if (CategoryGUID2.IndexOf(",") > 0)
                    {
                        CategoryGUID2 = CategoryGUID2.Substring(0, CategoryGUID2.Length - 1);
                    }

                    foreach (ListItem li in chkOwnerList.Items)
                    {
                        if (li.Selected)
                        {
                            if (String.Compare(li.Value, "0", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                CustomerGUID = string.Empty;
                                break;
                            }
                            CustomerGUID += "'" + li.Value + "'" + ",";
                        }
                    }
                    if (CustomerGUID.IndexOf(",") > 0)
                    {
                        CustomerGUID = CustomerGUID.Substring(0, CustomerGUID.Length - 1);
                    }

                }

                List<ArchiveClip> _ListOfArchiveClip = _IArchiveClipController.GetArchiveClipBySearch(new Guid(_SessionInformation.ClientGUID), SearchTitle, SearchDesc, SearchKey, SearchCC, FromDate, ToDate, CategoryGUID1, CategoryGUID2, CustomerGUID, ucCustomPager.CurrentPage.Value, ucCustomPager.PageSize, _ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsClipSortDirecitonAsc, false, out _TotalRecordsClipCount);

                _ViewstateInformation.TotalRecordsCountClip = _TotalRecordsClipCount;

                SetViewstateInformation(_ViewstateInformation);



                bool _HasRecords = true;

                if (p_IsInitialization == false && (_ListOfArchiveClip == null || _ListOfArchiveClip.Count == 0))
                {
                    _HasRecords = false;
                    _ListOfArchiveClip = new List<ArchiveClip>();
                    _ListOfArchiveClip.Add(new ArchiveClip());
                }

                grvClip.DataSource = _ListOfArchiveClip;
                grvClip.DataBind();


                ucCustomPager.TotalRecords = _TotalRecordsClipCount;
                ucCustomPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                //ucCustomPager.CurrentPage = _ViewstateInformation._CurrentClipPage;
                ucCustomPager.BindDataList();

                grvClip.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;");
                if (_HasRecords == false)
                {
                    grvClip.Rows[0].Visible = false;

                    lblNoResults.Text = CommonConstants.HTMLBreakLine + CommonConstants.NoResultsFound;
                    lblNoResults.Visible = true;
                }

                if (Page.IsPostBack)
                {
                    string _Script = "setCheckbox(\"" + chkCategories1.ClientID + "\",\"" + txtCat1Selection.ClientID + "\");";
                    _Script += "setCheckbox(\"" + chkCategories2.ClientID + "\",\"" + txtCat2Selection.ClientID + "\");";
                    _Script += "setCheckbox(\"" + chkOwnerList.ClientID + "\",\"" + txtOwnerSelection.ClientID + "\");";
                    _Script += "SetSearchParam();";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "SetSelection", _Script, true);
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

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                //_ViewstateInformation._CurrentClipPage = null;
                ucCustomPager.CurrentPage = null;

                SetViewstateInformation(_ViewstateInformation);
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

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

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

                BindArchiveClips(true);
                SortClipDirection();

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
                        SortClipDirection();
                    }
                    else
                    {

                        ClearArchiveClipGridFields();
                        BindArchiveClips(true);
                        SortClipDirection();
                    }
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
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

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
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion Download Clip

        #region Email

        protected void lnkEmail_Click(object sender, EventArgs e)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                txtYourEmail.Text = _SessionInformation.Email;
                UpdateUpdatePanel(upMainGrid);

                mdlpopupEmail.Show();
                UpdateUpdatePanel(upMail);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {



                string ClipGUID = string.Empty;
                bool IsCheck = false;
                string XMLGUID = "";
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
                System.Data.SqlTypes.SqlXml _ArchiveClipGUIDXML = new System.Data.SqlTypes.SqlXml(_XmlNodeReader);
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
                                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                                string EmailContent = _IArchiveClipController.EmailContent(_ArchiveClipGUIDXML, mailAddress, "myIQ");
                                string WholeEmailBody = "";
                                WholeEmailBody = IQMediaGroup.Core.HelperClasses.CommonFunctions.EmailSend(txtYourEmail.Text, mailAddress, txtSubject.Text, txtMessage.Text, EmailContent);

                                MailLog(WholeEmailBody, mailAddress);

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
                }

                if (!string.IsNullOrEmpty(IncorrectMessages))
                {

                    mdlpopupEmail.Show();
                    lblError.Text = "Following email address invalid " + IncorrectMessages;
                    lblError.Visible = true;
                }
                else
                {
                    lblSuccessMessage.Text = "Email Sent Successfully.";
                    ClearEmailFrom();
                    mdlpopupEmail.Hide();
                    UpdateUpdatePanel(upMail);
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
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
            txtYourEmail.Text = "";
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

        #region Manage Category

        protected void btnManageCategories_Click(object sender, EventArgs e)
        {
            try
            {

                BindCustomCategory();
                mpCustomCategory.Show();
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
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

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

                    SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
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

                mpCustomCategory.Show();
                UpdateUpdatePanel(upCustomCategory);
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

                                mpCustomCategory.Show();
                                BindCustomCategory();
                                UpdateUpdatePanel(upMainGrid);

                            }
                            else
                            {
                                lblMsg.Text = CommonConstants.CategoryAssociatedWithClip;
                            }
                        }
                    }
                }
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
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                gvCustomCategory.EditIndex = e.NewEditIndex;

                gvCustomCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                gvCustomCategory.DataBind();

                mpCustomCategory.Show();
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
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                if (_ViewstateInformation.ListOfCustomCategory != null)
                {
                    gvCustomCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    gvCustomCategory.DataBind();
                }

                mpCustomCategory.Show();
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
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                if (_ViewstateInformation.ListOfCustomCategory != null)
                {
                    gvCustomCategory.PageIndex = e.NewPageIndex;
                    gvCustomCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    gvCustomCategory.DataBind();
                }

                mpCustomCategory.Show();
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
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
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
                    }
                    else
                    {
                        gvCustomCategory.EditIndex = -1;
                        lblMsg.Text = "Category Updated Successfully.";

                        GetCustomCategoryByClientGUID();
                        BindMediaCategoryCheckList();
                        BindCustomCategory();

                        ClearSearch();
                        mpCustomCategory.Show();
                        UpdateUpdatePanel(upMainGrid);
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
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (_ViewstateInformation.ListOfCustomCategory != null && _ViewstateInformation.ListOfCustomCategory.Count > 0)
                {
                    chkCategories1.DataTextField = "CategoryName";
                    chkCategories1.DataValueField = "CategoryGUID";
                    chkCategories1.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    chkCategories1.DataBind();
                    chkCategories1.Items.Insert(0, new ListItem("All", "0"));
                    chkCategories1.Attributes.Add("onclick", "setCheckbox(this.id,'" + txtCat1Selection.ClientID + "')");

                    chkCategories2.DataTextField = "CategoryName";
                    chkCategories2.DataValueField = "CategoryGUID";
                    chkCategories2.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    chkCategories2.DataBind();
                    chkCategories2.Items.Insert(0, new ListItem("All", "0"));
                    chkCategories2.Attributes.Add("onclick", "setCheckbox(this.id,'" + txtCat2Selection.ClientID + "')");

                    chkCategories1.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkCategories1.ClientID + "')");
                    chkCategories2.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkCategories2.ClientID + "')");

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
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();

                List<Customer> _ListOfCustomer = new List<Customer>();
                _ListOfCustomer = _ICustomerController.GetCustomerNameByClientID(_SessionInformation.ClientID);

                chkOwnerList.DataTextField = "FirstName";
                chkOwnerList.DataValueField = "CustomerGUID";
                chkOwnerList.DataSource = _ListOfCustomer;
                chkOwnerList.DataBind();
                chkOwnerList.Items.Insert(0, new ListItem("All", "0"));

                chkOwnerList.Attributes.Add("onclick", "setCheckbox(this.id,'" + txtOwnerSelection.ClientID + "')");

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
                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                    _ViewstateInformation.IsmyIQSearchActive = true;
                    //_ViewstateInformation._CurrentClipPage = 0;
                    ucCustomPager.CurrentPage = 0;
                    SetViewstateInformation(_ViewstateInformation);
                    ClearArchiveClipGridFields();
                    BindArchiveClips(true);
                    SortClipDirection();

                }
                else
                {
                    string _Script = "setCheckbox(\"" + chkCategories1.ClientID + "\",\"" + txtCat1Selection.ClientID + "\");";
                    _Script += "setCheckbox(\"" + chkCategories2.ClientID + "\",\"" + txtCat2Selection.ClientID + "\");";
                    _Script += "setCheckbox(\"" + chkOwnerList.ClientID + "\",\"" + txtOwnerSelection.ClientID + "\");";
                    _Script += "SetSearchParam();";
                    _Script += "$(\"#" + DivSearch.ClientID + "\").show();";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "SetSelection", _Script, true);
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (_ViewstateInformation.IsmyIQSearchActive == true)
                {
                    ClearSearch();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void ClearSearch()
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                _ViewstateInformation.IsmyIQSearchActive = false;
                _ViewstateInformation.ClipSortExpression = "ClipCreationDate";
                //_ViewstateInformation._CurrentClipPage = 0;
                ucCustomPager.CurrentPage = 0;
                SetViewstateInformation(_ViewstateInformation);

                txtSearch.Text = string.Empty;

                cbAll.Checked = true;
                cbCC.Checked = true;
                cbDescription.Checked = true;
                cbKeywords.Checked = true;
                cbTitle.Checked = true;

                cbCC.Disabled = true;
                cbDescription.Disabled = true;
                cbKeywords.Disabled = true;
                cbTitle.Disabled = true;

                txtFromDate.Text = "";
                txtToDate.Text = "";

                chkCategories1.SelectedValue = null;
                chkCategories2.SelectedValue = null;
                chkOwnerList.SelectedValue = null;
                txtCat1Selection.Text = "";
                txtCat2Selection.Text = "";
                txtOwnerSelection.Text = "";
                ClearArchiveClipGridFields();
                BindArchiveClips(true);
                SortClipDirection();

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
                if (!cbAll.Checked && !cbCC.Checked && !cbKeywords.Checked && !cbDescription.Checked && !cbTitle.Checked)
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

            return validate;
        }

        #endregion Search

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
                }

                string _Script = "UpdateSubCategory2(\"" + ddlPCategory.ClientID + "\");";
                _Script += "UpdateSubCategory2(\"" + ddlSubCategory1.ClientID + "\");";
                _Script += "UpdateSubCategory2(\"" + ddlSubCategory2.ClientID + "\");";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "LoadCatList", _Script, true);
                mdlpopupClip.Show();
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
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

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
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

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

                string _Result = _IArchiveClipController.UpdateArchiveClip(_ArchiveClip);

                if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) < 0)
                {
                    lblClipMsg.Text = "Some error occurs please try again.";
                    mdlpopupClip.Show();

                }
                else
                {
                    lblSuccessMessage.Text = "Record updated Successfully.";
                    ClearArchiveClipGridFields();
                    BindArchiveClips(false);
                    SortClipDirection();


                    SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

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

                    mdlpopupClip.Hide();
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
                InitialClip(_clipID, "false");
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void InitialClip(string _clipID, string _playback)
        {
            try
            {
                divRawMedia.Visible = true;
                IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

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

                //_ViewstateInformation.VSTimetick = 0;
                this.SetViewstateInformation(_ViewstateInformation);

                UpdateUpdatePanel(upClipPlayer);
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
                foreach (IQMediaGroup.Core.HelperClasses.Caption _Caption in _ListOfRCatpion)
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

                ClearArchiveClipGridFields();

                BindArchiveClips(true);
                SortClipDirection();
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

        #region CustomCategory

        private void GetCustomCategoryByClientGUID()
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                string _ClientGUID = _SessionInformation.ClientGUID;
                List<CustomCategory> _ListofCustomCategory = new List<CustomCategory>();

                ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                _ListofCustomCategory = _ICustomCategoryController.SelectByClientGUID(new Guid(_ClientGUID));

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.ListOfCustomCategory = _ListofCustomCategory;
                SetViewstateInformation(_ViewstateInformation);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CustomCategory

        #region Paging

        protected void ucCustomPager_PageIndexChange(object sender, EventArgs e)// int currentpageNumber)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (_ViewstateInformation != null)
                {
                    // _ViewstateInformation._CurrentClipPage = currentpageNumber;
                    BindArchiveClips(false);
                    SortClipDirection();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

    }


}