using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Core.Enumeration;
using System.Configuration;
using System.IO;
using IQMediaGroup.Core.HelperClasses;
using System.Xml;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using System.Data.SqlTypes;
using System.Xml.Linq;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Threading;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IQCustom
{
    public partial class IQCustom : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        int _NoOfResultsFromDBUGCRawMedia = 11;
        public ViewstateInformation _ViewstateInformationFtp;

        List<FtpDirectoryInfo> ListOfDirectory;
        List<FtpFileInfo> ListOfFiles;

        [Serializable]
        class FtpDirectoryInfo
        {
            public string Name;
            public string Path;


        }

        [Serializable]
        class FtpFileInfo
        {
            public string Name { get; set; }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                _NoOfResultsFromDBUGCRawMedia = grvUGCRawMedia.PageSize + 1;

                if (!IsPostBack)
                {
                    _ViewstateInformationFtp = GetViewstateInformation();
                    GetCustomCategoryByClientGUID();
                    GetCustomerByClientID();
                    BindCustomerCheckList();
                    BindMediaCategoryCheckList();
                    BindAllMediaCategoryDropDown();

                    BindCustomerDropDown(ddlOwner, true);
                    SetClientFtpUpload();

                    BindUGCRawMedia(true);

                    Clipframe.Attributes.Add("src", "http://" + Request.Url.Host.ToString() + "/IFrameRawMedia/Default.aspx?CC=false" + "&IsUGC=true");

                    DateTime _CurrentDate = DateTime.Now;

                    ddlYear.SelectedValue = Convert.ToString(_CurrentDate.Year);
                    ddlMonth.SelectedValue = Convert.ToString(_CurrentDate.Month);
                    ddlDay.SelectedValue = Convert.ToString(_CurrentDate.Day);
                    ddlHour.SelectedValue = Convert.ToString(_CurrentDate.Hour);
                    ddlMinute.SelectedValue = Convert.ToString(_CurrentDate.Minute);


                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetSearchParam", "SetSearchParam();", true);
                }



                tblUploading.Style.Add("display", "none");
                tblNewUpload.Style.Add("display", "block");
                tblFile.Style.Add("display", "block");



                lbtnCancelPopUpUpload.Style.Add("display", "none");
                lbtnCancelPopUp.Style.Add("display", "block");

                chkCategories1.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkCategories1.ClientID + "')");
                chkOwnerList.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkOwnerList.ClientID + "')");

                SortUGCRawMediaDirection();
                lblNoResults.Visible = false;
                lblNoResults.Text = string.Empty;

                string[] Extentions = ConfigurationManager.AppSettings["UGCFileUploadExtention"].Split(new char[] { ',' });

                ScriptManager.RegisterArrayDeclaration(this.Page, "Extention", String.Join(",", Extentions.Select(extnetion => "'" + extnetion + "'")));
                string _sctipt = string.Empty;
                _sctipt += "if(document.getElementById('ctl00_Content_Data_IQCustom1_FileUpload1Uploader') == undefined || document.getElementById('ctl00_Content_Data_IQCustom1_FileUpload1Uploader') == null){";
                _sctipt += "LoadUploadify();";
                _sctipt += "}";

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadUploadify", _sctipt, true);

                string ErrorMessage = ConfigurationManager.AppSettings["UGCErrorMessage"];
                ScriptManager.RegisterHiddenField(this.Page, "ErrorMessage", ErrorMessage);

                lblMsg.Text = string.Empty;
                lblTxtMsg.Visible = false;
                lblTxtMsg.Text = string.Empty;

                RegisterPosbBackForDownloadAndUploadEdit();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void SortUGCRawMediaDirection()
        {
            ViewstateInformation _ViewstateInformation = GetViewstateInformation();

            GridViewRow gridViewHeaderSearchRow = grvUGCRawMedia.HeaderRow;
            if (gridViewHeaderSearchRow != null)
            {
                foreach (TableCell headerSearchCell in gridViewHeaderSearchRow.Cells)
                {
                    if (headerSearchCell.HasControls())
                    {
                        LinkButton headerSearchButton = headerSearchCell.Controls[0] as LinkButton;

                        if (headerSearchButton != null)
                        {
                            if (headerSearchButton.FindControl("SortImage") as Image == null)
                            {
                                HtmlGenericControl divSearch = new HtmlGenericControl("div");

                                Label headerSearchText = new Label();
                                headerSearchText.Text = headerSearchButton.Text;

                                divSearch.Controls.Add(headerSearchText);

                                //if (e.SortExpression == headerButton.CommandArgument)
                                //{
                                if (headerSearchButton.CommandArgument == _ViewstateInformation.SortExpression)
                                {
                                    Image headerSearchImage = new Image();
                                    headerSearchImage.ID = "SortImage";

                                    if (_ViewstateInformation.IsSortDirecitonAsc == true)
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

                                    headerSearchButton.Controls.Add(divSearch);

                                    break;
                                }
                            }
                            //}

                        }
                    }
                }
            }
        }


        private void GetCustomerByClientID()
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();

                List<Customer> _ListOfCustomer = new List<Customer>();
                _ListOfCustomer = _ICustomerController.GetCustomerNameByClientID(_SessionInformation.ClientID);

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                _ViewstateInformation.ListOfUGCCustomer = _ListOfCustomer;
                SetViewstateInformation(_ViewstateInformation);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<CustomCategory> GetCustomCategoryByClientGUID()
        {
            try
            {


                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (_SessionInformation.IsUgcAutoClip)
                {
                    ugcAutoClip.Visible = true;
                    chkAutoClip.Checked = false;
                }
                else
                {
                    ugcAutoClip.Visible = false;
                    chkAutoClip.Checked = false;
                }

                string _ClientGUID = _SessionInformation.ClientGUID;
                List<CustomCategory> _ListofCustomCategory = new List<CustomCategory>();

                ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                _ListofCustomCategory = _ICustomCategoryController.SelectByClientGUID(new Guid(_ClientGUID));
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.ListOfCustomCategory = _ListofCustomCategory;
                SetViewstateInformation(_ViewstateInformation);

                return _ListofCustomCategory;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindAllMediaCategoryDropDown()
        {

            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (_ViewstateInformation.ListOfCustomCategory != null && _ViewstateInformation.ListOfCustomCategory.Count > 0)
                {

                    // bind all category dropdowns for Upload Media Popup
                    ddlCategory.DataTextField = "CategoryName";
                    ddlCategory.DataValueField = "CategoryGUID";
                    ddlCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new ListItem("<Blank>", "0"));

                    ddlSubCategory1.DataTextField = "CategoryName";
                    ddlSubCategory1.DataValueField = "CategoryGUID";
                    ddlSubCategory1.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlSubCategory1.DataBind();
                    ddlSubCategory1.Items.Insert(0, new ListItem("<Blank>", "0"));

                    ddlSubCategory2.DataTextField = "CategoryName";
                    ddlSubCategory2.DataValueField = "CategoryGUID";
                    ddlSubCategory2.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlSubCategory2.DataBind();
                    ddlSubCategory2.Items.Insert(0, new ListItem("<Blank>", "0"));

                    ddlSubCategory3.DataTextField = "CategoryName";
                    ddlSubCategory3.DataValueField = "CategoryGUID";
                    ddlSubCategory3.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlSubCategory3.DataBind();
                    ddlSubCategory3.Items.Insert(0, new ListItem("<Blank>", "0"));

                    // bind all category dropdowns for Edit Media Popup
                    ddlPCategory.DataTextField = "CategoryName";
                    ddlPCategory.DataValueField = "CategoryGUID";
                    ddlPCategory.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlPCategory.DataBind();
                    ddlPCategory.Items.Insert(0, new ListItem("<Blank>", "0"));

                    ddlEditSubCategory1.DataTextField = "CategoryName";
                    ddlEditSubCategory1.DataValueField = "CategoryGUID";
                    ddlEditSubCategory1.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlEditSubCategory1.DataBind();
                    ddlEditSubCategory1.Items.Insert(0, new ListItem("<Blank>", "0"));

                    ddlEditSubCategory2.DataTextField = "CategoryName";
                    ddlEditSubCategory2.DataValueField = "CategoryGUID";
                    ddlEditSubCategory2.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlEditSubCategory2.DataBind();
                    ddlEditSubCategory2.Items.Insert(0, new ListItem("<Blank>", "0"));

                    ddlEditSubCategory3.DataTextField = "CategoryName";
                    ddlEditSubCategory3.DataValueField = "CategoryGUID";
                    ddlEditSubCategory3.DataSource = _ViewstateInformation.ListOfCustomCategory;
                    ddlEditSubCategory3.DataBind();
                    ddlEditSubCategory3.Items.Insert(0, new ListItem("<Blank>", "0"));
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

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

                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindCustomerCheckList()
        {
            try
            {

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                List<Customer> _ListOfCustomer = _ViewstateInformation.ListOfUGCCustomer;

                chkOwnerList.DataTextField = "FirstName";
                chkOwnerList.DataValueField = "CustomerGUID";
                chkOwnerList.DataSource = _ListOfCustomer;
                chkOwnerList.DataBind();
                chkOwnerList.Items.Insert(0, new ListItem("All", "0"));

                chkOwnerList.Attributes.Add("onclick", "setCheckbox(this.id,'" + txtOwnerSelection.ClientID + "')");

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindUGCRawMedia(bool p_IsInitialization, bool p_IsRefresh = false)
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (string.IsNullOrEmpty(_ViewstateInformation.SortExpression))
                {
                    _ViewstateInformation.SortExpression = "AirDate";
                    /*_ViewstateInformation.SortExpression = "UGCCreateDT";*/
                }

                //if (_ViewstateInformation._CurrentUGCRawMediaPage == null)
                //{
                //    _ViewstateInformation._CurrentUGCRawMediaPage = 0;
                //}
                if (ucCustomPager.CurrentPage == null)
                {
                    ucCustomPager.CurrentPage = 0;
                }

                IUGCRawMediaController _IUGCRawMediaController = _ControllerFactory.CreateObject<IUGCRawMediaController>();

                int _TotalRecordsCount = 0;
                int ErrorNumber = -1;

                string SearchTitle = string.Empty;
                string SearchDesc = string.Empty;
                string SearchKey = string.Empty;
                DateTime? FromDate = null;
                DateTime? ToDate = null;
                string CategoryGUID1 = string.Empty;
                string CustomerGUID = string.Empty;
                lblActiveSearch.Text = string.Empty;
                if (_ViewstateInformation.IsIQCustomSearchActive == true)
                {
                    lblActiveSearch.Text = "Search Active!";
                    if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    {
                        string _SearchTerm = GenerateSearchTerm(txtSearch.Text.Trim());

                        if (cbAll.Checked)
                        {
                            SearchTitle = _SearchTerm;
                            SearchKey = _SearchTerm;
                            SearchDesc = _SearchTerm;
                        }
                        else
                        {
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
                            if (li.Value == "0")
                            {
                                CategoryGUID1 = "";
                                break;
                            }
                            CategoryGUID1 += "'" + li.Value + "'" + ",";
                        }
                    }
                    if (CategoryGUID1.IndexOf(",") > 0)
                    {
                        CategoryGUID1 = CategoryGUID1.Substring(0, CategoryGUID1.Length - 1);
                    }



                    foreach (ListItem li in chkOwnerList.Items)
                    {
                        if (li.Selected)
                        {
                            if (li.Value == "0")
                            {
                                CustomerGUID = "";
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



                if (p_IsRefresh)
                {
                    _IUGCRawMediaController.FillRecordsFromCore(new Guid(_SessionInformation.ClientGUID));
                }



                List<UGCRawMedia> _ListOfUGCRawMedia = _IUGCRawMediaController.GetUGCRawMediaBySearch(new Guid(_SessionInformation.ClientGUID), ucCustomPager.CurrentPage.Value, grvUGCRawMedia.PageSize, _ViewstateInformation.SortExpression, _ViewstateInformation.IsSortDirecitonAsc, out _TotalRecordsCount, CategoryGUID1, CustomerGUID, FromDate, ToDate, SearchTitle, SearchKey, SearchDesc, out ErrorNumber);

                if (ErrorNumber == -1)
                {


                    _ViewstateInformation.TotalRecordsCountUGCRawMedia = _TotalRecordsCount;

                   

                    SetViewstateInformation(_ViewstateInformation);

                    /* grvUGCRawMedia.PageIndex = _ViewstateInformation._CurrentUGCRAWMediaPage.Value;*/

                    bool _HasRecords = true;

                    if (p_IsInitialization == false && (_ListOfUGCRawMedia == null || _ListOfUGCRawMedia.Count == 0))
                    {
                        _HasRecords = false;
                        _ListOfUGCRawMedia = new List<UGCRawMedia>();
                        _ListOfUGCRawMedia.Add(new UGCRawMedia());
                    }

                    grvUGCRawMedia.DataSource = _ListOfUGCRawMedia;
                    grvUGCRawMedia.DataBind();

                    ucCustomPager.TotalRecords = _TotalRecordsCount;
                    ucCustomPager.NoOfPagesToDisplay = Convert.ToInt16(ConfigurationManager.AppSettings["NoOfPagesToDisplay"]);
                    //ucCustomPager.CurrentPage = _ViewstateInformation._CurrentUGCRawMediaPage;
                    ucCustomPager.BindDataList();

                    if (_HasRecords == false)
                    {
                        grvUGCRawMedia.Rows[0].Visible = false;

                        lblNoResults.Text = CommonConstants.HTMLBreakLine + CommonConstants.NoResultsFound;
                        lblNoResults.Visible = true;
                    }

                    SortUGCRawMediaDirection();

                    if (Page.IsPostBack)
                    {
                        string _Script = "setCheckbox(\"" + chkCategories1.ClientID + "\",\"" + txtCat1Selection.ClientID + "\");";
                        _Script += "setCheckbox(\"" + chkOwnerList.ClientID + "\",\"" + txtOwnerSelection.ClientID + "\");";
                        _Script += "SetSearchParam();";
                        //_Script += "$(\"#" + DivSearch.ClientID + "\").hide();";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "SetSelection", _Script, true);
                    }

                }
                else
                {
                    if (ErrorNumber == 7630)
                    {
                        lblSearchErr.Text = "Incorrect syntax for Search Term";
                    }
                    else
                    {
                        lblSearchErr.Text = "Error occurred, please try again!!";
                    }

                    string _Script = "setCheckbox(\"" + chkCategories1.ClientID + "\",\"" + txtCat1Selection.ClientID + "\");";
                    _Script += "setCheckbox(\"" + chkOwnerList.ClientID + "\",\"" + txtOwnerSelection.ClientID + "\");";
                    _Script += "SetSearchParam();";
                    _Script += "$(\"#" + DivSearch.ClientID + "\").show();";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "SetSelection", _Script, true);

                }


            }

            catch (Exception )
            {
                throw;
            }
        }

        /// <summary>
        /// Get Stemming applied on Search String if Applied '#' on Last of Search String
        /// </summary>
        /// <param name="SearchVal"></param>
        /// <returns></returns>
        protected string GenerateSearchTerm(string SearchVal)
        {

            if (SearchVal.Substring(SearchVal.Length - 1, 1) == "#")
            {
                SearchVal = SearchVal.Remove(SearchVal.Length - 1, 1);

                var StrStemmed = Regex.Replace(SearchVal,
                            @"([\""][\w ]+[\""])|(\w+)",
                            m => (Enum.GetValues(typeof(FullTextLogicalOperator)).Cast<FullTextLogicalOperator>().Select(v => v.ToString()).ToList()).Contains(m.Value.ToUpper()) ? m.Value : "FORMSOF (INFLECTIONAL," + m.Value + ")"
                          );
                return Convert.ToString(StrStemmed);
            }
            else
            {
                return SearchVal;
            }
        }

        private void BindCustomerDropDown(DropDownList p_ddlCustomer, bool p_IsEdit = false)
        {

            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                List<Customer> _ListOfCustomer = _ViewstateInformation.ListOfUGCCustomer;

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

        protected void btnUploadMedia_Click(object sender, EventArgs e)
        {
            try
            {

                _ViewstateInformationFtp = GetViewstateInformation();
                string _FileName = hdnName.Value;

                /*string extention = _FileName.Substring(_FileName.LastIndexOf('.') + 1).ToLower();
                string fileNamewithoutext = _FileName.Substring(0, _FileName.LastIndexOf("."));

                string _MergeName = fileNamewithoutext + "." + extention;*/

                string _FinalFileName = _FileName.Substring(0, _FileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("MMddyyyy_hhmmss") + _FileName.Substring(_FileName.LastIndexOf("."));

                _FinalFileName = Regex.Replace(_FinalFileName, @"[\s\\/]", "_");

                string ClipUploadLocation = Server.MapPath("~/UGC-Content/");

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileUpLoadLocation]) && Directory.Exists(ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileUpLoadLocation]))
                {
                    ClipUploadLocation = ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileUpLoadLocation];
                }

                if (hndIsFtpUpload.Value != "true")
                {
                    FileInfo _FileInfo = new FileInfo(ClipUploadLocation + _FileName);
                    _FileInfo.MoveTo(ClipUploadLocation + _FinalFileName);
                }
                else
                {
                    string path = _ViewstateInformationFtp.FtpFilePath + "\\" + _FileName;

                    FileInfo _FileInfo = new FileInfo(path);
                    _FileInfo.MoveTo(ClipUploadLocation + _FinalFileName);
                }


                MakeUGCUploadServiceCall(_FinalFileName);

                CreateXml(_FinalFileName);

                tblUploading.Style.Add("display", "block");
                tblNewUpload.Style.Add("display", "none");
                tblFile.Style.Add("display", "none");

                lbtnCancelPopUpUpload.Style.Add("display", "block");
                lbtnCancelPopUp.Style.Add("display", "none");

                lblUploadedStatus.Text = "Completed";
                txtFile.Text = _FileName;

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "LoadUploadify1", "$('#ctl00_Content_Data_IQCustom1_divFU').attr('style','display:block');", true);
                hndIsFtpUpload.Value = "false";
                mpeUploadMedia.Show();

                upUploadMedia.Update();

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void MakeUGCUploadServiceCall(string _FinalFileName)
        {
            try
            {
                string _HttpResponse = CommonFunctions.GetHttpResponse(ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileUploadService] + _FinalFileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreateXml(string p_FileName)
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                string ClipUploadLocation = string.Empty;
                ClipUploadLocation = ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileUpLoadLocation];

                string ClipDownloadLocation = string.Empty;
                ClipDownloadLocation = Convert.ToString(ConfigurationManager.AppSettings[CommonConstants.ConfigUGCFileDownloadLocation]);

                ControllerFactory _ControllerFactory = new ControllerFactory();
                IIQClient_UGCMapController _IIQClient_UGCMapController = _ControllerFactory.CreateObject<IIQClient_UGCMapController>();

                IQClient_UGCMap _IQClient_UGCMap = _IIQClient_UGCMapController.GetIQClient_UGCMapByClientGUID(new Guid(_SessionInformation.ClientGUID));

                UGCXml _UGCXml = new UGCXml();

                _UGCXml._RawInfo = new UGCXml.RawInfo();

                _UGCXml._RawInfo.AirDate = DateTime.Now;
                _UGCXml._RawInfo.SourceID = _IQClient_UGCMap.SourceID;

                if (chkAutoClip.Checked)
                    _UGCXml._RawInfo.UGCAutoClip = true;
                else
                    _UGCXml._RawInfo.UGCAutoClip = false;

                _UGCXml._RawInfo.MetaData = new UGCXml.MetaData();

                _UGCXml._RawInfo.MetaData.ListOfMeta = new List<UGCXml.Meta>();

                UGCXml.Meta _MetaTitle = new UGCXml.Meta();

                _MetaTitle.Key = "UGC-Title";
                _MetaTitle.Value = txtMediaTitle.Text.Trim();

                UGCXml.Meta _MetaKeyWords = new UGCXml.Meta();

                _MetaKeyWords.Key = "UGC-Kwords";
                _MetaKeyWords.Value = txtKeywords.Text.Trim();

                UGCXml.Meta _MetaCategory = new UGCXml.Meta();

                _MetaCategory.Key = "UGC-Category";
                _MetaCategory.Value = ddlCategory.SelectedValue;


                UGCXml.Meta _MetaSubCategory1 = null;

                if (ddlSubCategory1.SelectedValue != "0")
                {
                    _MetaSubCategory1 = new UGCXml.Meta();

                    _MetaSubCategory1.Key = "UGC-SubCategory1";
                    _MetaSubCategory1.Value = ddlSubCategory1.SelectedValue;
                }


                UGCXml.Meta _MetaSubCategory2 = null;

                if (ddlSubCategory2.SelectedValue != "0")
                {
                    _MetaSubCategory2 = new UGCXml.Meta();

                    _MetaSubCategory2.Key = "UGC-SubCategory2";
                    _MetaSubCategory2.Value = ddlSubCategory2.SelectedValue;
                }


                UGCXml.Meta _MetaSubCategory3 = null;

                if (ddlSubCategory3.SelectedValue != "0")
                {
                    _MetaSubCategory3 = new UGCXml.Meta();

                    _MetaSubCategory3.Key = "UGC-SubCategory3";
                    _MetaSubCategory3.Value = ddlSubCategory3.SelectedValue;
                }


                UGCXml.Meta _MetaCreatedDate = new UGCXml.Meta();

                _MetaCreatedDate.Key = "UGC-CreateDT";
                _MetaCreatedDate.Value = new DateTime(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToInt32(ddlDay.SelectedValue), Convert.ToInt32(ddlHour.SelectedValue), Convert.ToInt32(ddlMinute.SelectedValue), 0).ToString() + " " + ddlTimeZone.SelectedValue;

                UGCXml.Meta _MetaDesc = new UGCXml.Meta();

                _MetaDesc.Key = "UGC-Desc";
                _MetaDesc.Value = txtDescription.Text;

                UGCXml.Meta _MetaUser = new UGCXml.Meta();

                _MetaUser.Key = "iQUser";
                _MetaUser.Value = _SessionInformation.CustomerGUID;

                UGCXml.Meta _MetaFileName = new UGCXml.Meta();

                _MetaFileName.Key = "UGC-FileName";
                _MetaFileName.Value = p_FileName;

                UGCXml.Meta _MetaFileLocation = new UGCXml.Meta();

                _MetaFileLocation.Key = "UGC-FileLocation";
                _MetaFileLocation.Value = ClipDownloadLocation + DateTime.Now.Year + @"\" + DateTime.Now.Month + @"\" + DateTime.Now.Day + @"\";

                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaTitle);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaKeyWords);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaCategory);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaSubCategory1);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaSubCategory2);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaSubCategory3);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaCreatedDate);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaDesc);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaUser);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaFileName);
                _UGCXml._RawInfo.MetaData.ListOfMeta.Add(_MetaFileLocation);

                _UGCXml._ClipInfo = new UGCXml.ClipInfo();

                if (_IQClient_UGCMap.AutoClip_Status == true)
                {
                    _UGCXml._ClipInfo.Title = txtMediaTitle.Text;
                    _UGCXml._ClipInfo.Category = "PR";
                    _UGCXml._ClipInfo.Keywords = txtKeywords.Text;
                    _UGCXml._ClipInfo.Description = txtDescription.Text;
                    _UGCXml._ClipInfo.User = ConfigurationManager.AppSettings[CommonConstants.ConfigIQMediaUserGUID];

                    _UGCXml._ClipInfo.MetaData = new UGCXml.MetaData();

                    UGCXml.Meta _MetaClipInfoTitle = new UGCXml.Meta();

                    _MetaClipInfoTitle.Key = "iQClientid";
                    _MetaClipInfoTitle.Value = _SessionInformation.ClientGUID;

                    UGCXml.Meta _MetaClipInfoUser = new UGCXml.Meta();

                    _MetaClipInfoUser.Key = "iQUser";
                    _MetaClipInfoUser.Value = _SessionInformation.CustomerGUID;

                    UGCXml.Meta _MetaClipInfoCategory = new UGCXml.Meta();

                    _MetaClipInfoCategory.Key = "iQCategory";
                    _MetaClipInfoCategory.Value = ddlCategory.SelectedValue;

                    UGCXml.Meta _ClipMetaSubCategory1 = null;

                    if (ddlSubCategory1.SelectedValue != "0")
                    {
                        _ClipMetaSubCategory1 = new UGCXml.Meta();

                        _ClipMetaSubCategory1.Key = "SubCategory1GUID";
                        _ClipMetaSubCategory1.Value = ddlSubCategory1.SelectedValue;
                    }


                    UGCXml.Meta _ClipMetaSubCategory2 = null;

                    if (ddlSubCategory2.SelectedValue != "0")
                    {
                        _ClipMetaSubCategory2 = new UGCXml.Meta();

                        _ClipMetaSubCategory2.Key = "SubCategory2GUID";
                        _ClipMetaSubCategory2.Value = ddlSubCategory2.SelectedValue;
                    }


                    UGCXml.Meta _ClipMetaSubCategory3 = null;

                    if (ddlSubCategory3.SelectedValue != "0")
                    {
                        _ClipMetaSubCategory3 = new UGCXml.Meta();

                        _ClipMetaSubCategory3.Key = "SubCategory3GUID";
                        _ClipMetaSubCategory3.Value = ddlSubCategory3.SelectedValue;
                    }

                    _UGCXml._ClipInfo.MetaData.ListOfMeta = new List<UGCXml.Meta>();

                    _UGCXml._ClipInfo.MetaData.ListOfMeta.Add(_MetaClipInfoTitle);
                    _UGCXml._ClipInfo.MetaData.ListOfMeta.Add(_MetaClipInfoUser);
                    _UGCXml._ClipInfo.MetaData.ListOfMeta.Add(_MetaClipInfoCategory);
                    _UGCXml._ClipInfo.MetaData.ListOfMeta.Add(_ClipMetaSubCategory1);
                    _UGCXml._ClipInfo.MetaData.ListOfMeta.Add(_ClipMetaSubCategory2);
                    _UGCXml._ClipInfo.MetaData.ListOfMeta.Add(_ClipMetaSubCategory3);

                }

                /* XDocument _XDocument = XDocument.Parse(CommonFunctions.MakeSerializationWithoutNameSpace(_UGCXml));*/

                XmlDocument _XmlDocument = new XmlDocument();
                _XmlDocument.LoadXml(CommonFunctions.MakeSerializationWithoutNameSpace(_UGCXml));

                _XmlDocument.Save(ClipUploadLocation + p_FileName + ".xml");

                /* _XDocument.Save(_XmlWriter);*/

                IUGC_Upload_LogController _IUGC_Upload_LogController = _ControllerFactory.CreateObject<IUGC_Upload_LogController>();

                UGC_Upload_Log _UGC_Upload_Log = new UGC_Upload_Log();

                _UGC_Upload_Log.CustomerGUID = new Guid(_SessionInformation.CustomerGUID);

                using (XmlNodeReader _XmlNodeReader = new XmlNodeReader(_XmlDocument))
                {
                    _UGC_Upload_Log.UGCContentXml = new SqlXml(_XmlNodeReader);
                }

                _UGC_Upload_Log.FileName = p_FileName;
                _UGC_Upload_Log.UploadedDateTime = DateTime.Now;

                string _Result = _IUGC_Upload_LogController.Insert(_UGC_Upload_Log);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void ClearFields()
        {
            try
            {
                txtDescription.Text = string.Empty;
                txtKeywords.Text = string.Empty;
                txtMediaTitle.Text = string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ClearUGCGridFields()
        {
            try
            {
                grvUGCRawMedia.EditIndex = -1;
                grvUGCRawMedia.PageIndex = 0;

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                //_ViewstateInformation._CurrentUGCRawMediaPage = null;
                ucCustomPager.CurrentPage = null;

                SetViewstateInformation(_ViewstateInformation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case "audio/mpeg3":
                    return ".mp3";
                case "video/mp4":
                    return ".mp4";
                default:
                    return ".mp3";
            }
        }

        protected void lbtnPlay_OnCommand(object sender, CommandEventArgs e)
        {
            Clipframe.Attributes.Add("src", "http://" + Request.Url.Host.ToString() + "/IFrameRawMedia/Default.aspx?CC=false&RawMediaID=" + e.CommandArgument.ToString() + "&IsUGC=true");
            /*Clipframe.Attributes.Add("src", "http://localhost:2281/IFrameRawMedia/Default.aspx?CC=false&RawMediaID=" + e.CommandArgument.ToString() + "&IsUGC=true");*/
            upClipFrame.Update();
            //upUploadMedia.Update();
        }

        

        protected void grvUGCRawMedia_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (!string.IsNullOrEmpty(_ViewstateInformation.SortExpression))
                {
                    if (_ViewstateInformation.SortExpression.ToLower() == e.SortExpression.ToLower())
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
                        _ViewstateInformation.SortExpression = e.SortExpression;
                        _ViewstateInformation.IsSortDirecitonAsc = true;
                    }
                }
                else
                {
                    _ViewstateInformation.SortExpression = e.SortExpression;
                    _ViewstateInformation.IsSortDirecitonAsc = true;
                }

                ClearUGCGridFields();

                SetViewstateInformation(_ViewstateInformation);

                BindUGCRawMedia(true);

                //SortUGCRawMediaDirection();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                _ViewstateInformation.SortExpression = null;
                _ViewstateInformation.IsSortDirecitonAsc = false;

                SetViewstateInformation(_ViewstateInformation);

                ClearUGCGridFields();

                BindUGCRawMedia(true, true);
                /* BindUGCRawMedia(true);*/

                //SortUGCRawMediaDirection();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected bool ValidateSearch()
        {
            lblSearchErr.Text = "";
            bool validate = true;

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                if (!cbAll.Checked && !cbKeywords.Checked && !cbDescription.Checked && !cbTitle.Checked)
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string _UGCRawMediaIDs = string.Empty;

                foreach (GridViewRow _GridViewRow in grvUGCRawMedia.Rows)
                {
                    HtmlInputCheckBox _chkDelete = (HtmlInputCheckBox)_GridViewRow.FindControl("chkDelete");

                    if (_chkDelete.Checked == true)
                    {
                        _UGCRawMediaIDs = _UGCRawMediaIDs + "'" + _chkDelete.Value + "'" + ",";
                    }
                }

                if (_UGCRawMediaIDs.Length > 0)
                {
                    _UGCRawMediaIDs = _UGCRawMediaIDs.Substring(0, _UGCRawMediaIDs.Length - 1);

                    string _Result = string.Empty;

                    IUGCRawMediaController _IUGCRawMediaController = _ControllerFactory.CreateObject<IUGCRawMediaController>();
                    _Result = _IUGCRawMediaController.DeleteUGCRawMedia(_UGCRawMediaIDs);

                    if (!string.IsNullOrEmpty(_Result) && Convert.ToInt32(_Result) > 0)
                    {
                        lblTxtMsg.Text = "Record(s) deleted Successfully.";
                        lblTxtMsg.ForeColor = System.Drawing.Color.Green;
                        lblTxtMsg.Visible = true;

                        ClearUGCGridFields();
                        BindUGCRawMedia(true);
                    }
                    else
                    {
                        lblTxtMsg.Text = "Some error occurs please try again.";
                        lblTxtMsg.ForeColor = System.Drawing.Color.Red;
                        lblTxtMsg.Visible = true;
                    }
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void ibtnDownload_Command(object sender, CommandEventArgs e)
        {
            UGCDownloadTracking _UGCDownloadTracking = new UGCDownloadTracking();
            IUGCDownloadTrackingController _IUGCDownloadTrackingController = _ControllerFactory.CreateObject<IUGCDownloadTrackingController>();

            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                Guid _UGCGUID = new Guid(Convert.ToString(e.CommandArgument));

                IUGCRawMediaController _IUGCRawMediaController = _ControllerFactory.CreateObject<IUGCRawMediaController>();

                string _FilePath = _IUGCRawMediaController.GetUGCFilePathByUGCGUID(_UGCGUID);

                _UGCDownloadTracking.UGCGUID = _UGCGUID;
                _UGCDownloadTracking.CustomerGUID = new Guid(_SessionInformation.CustomerGUID);
                _UGCDownloadTracking.DownloadedDateTime = DateTime.Now;
                _UGCDownloadTracking.IsDownloadSuccess = false;

                // Create New instance of FileInfo class to get the properties of the file being downloaded
                if (!string.IsNullOrEmpty(_FilePath))
                {
                    FileInfo _FileInfo = new FileInfo(_FilePath);

                    // Checking if file exists
                    if (_FileInfo.Exists && _FileInfo.Length > 0)
                    {
                        _UGCDownloadTracking.IsDownloadSuccess = true;
                        _UGCDownloadTracking.DownloadDescription = "File download complete." + _FilePath;

                        _IUGCDownloadTrackingController.Insert(_UGCDownloadTracking);

                        // Clear the content of the response                  
                        Response.ClearContent();

                        // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + _FilePath.Substring(_FilePath.LastIndexOf('\\') + 1) + "\"");

                        // Add the file size into the response header
                        Response.AddHeader("Content-Length", _FileInfo.Length.ToString());

                        // Set the ContentType
                        Response.ContentType = ReturnExtension(_FileInfo.Extension.ToLower());

                        // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                        Response.WriteFile(_FileInfo.FullName);

                        // End the response
                        Response.End();

                        //Server.Transfer(Server.MapPath("~/"), true);
                        /*Response.Flush();*/
                        //HttpContext.Current.ApplicationInstance.CompleteRequest();

                    }
                    else
                    {
                        lblTxtMsg.Text = "An error occurred, please try again later.";
                        lblTxtMsg.Visible = true;
                        lblTxtMsg.ForeColor = System.Drawing.Color.Red;

                        _UGCDownloadTracking.IsDownloadSuccess = false;
                        _UGCDownloadTracking.DownloadDescription = "File doesn't exist. " + _FilePath;
                        _IUGCDownloadTrackingController.Insert(_UGCDownloadTracking);
                    }
                }
                else
                {
                    _UGCDownloadTracking.IsDownloadSuccess = false;
                    _UGCDownloadTracking.DownloadDescription = "FilePath is empty.";
                    _IUGCDownloadTrackingController.Insert(_UGCDownloadTracking);

                    lblTxtMsg.Text = "An error occurred, please try again later.";
                    lblTxtMsg.Visible = true;
                    lblTxtMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (ThreadAbortException _ThreadAbortException)
            {

            }
            catch (Exception _Exception)
            {
                try
                {
                    _UGCDownloadTracking.IsDownloadSuccess = false;
                    _UGCDownloadTracking.DownloadDescription = _Exception.Message;
                    _IUGCDownloadTrackingController.Insert(_UGCDownloadTracking);
                }
                catch
                {

                }

                this.WriteException(_Exception);
                lblTxtMsg.Text = "An error occurred, please try again later.";
                lblTxtMsg.Visible = true;
                lblTxtMsg.ForeColor = System.Drawing.Color.Red;
                //Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvUGCRawMedia_DataBound(object sender, EventArgs e)
        {
            try
            {



                RegisterPosbBackForDownloadAndUploadEdit();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvUGCRawMedia_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lbtnEdit = (LinkButton)e.Row.FindControl("lbtnEdit");
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(lbtnEdit);
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void RegisterPosbBackForDownloadAndUploadEdit()
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                if (grvUGCRawMedia != null && grvUGCRawMedia.Rows.Count > 0)
                {
                    if (_SessionInformation.IsUGCDownload)
                    {
                        grvUGCRawMedia.Columns[7].Visible = true;

                        foreach (GridViewRow _GridViewRow in grvUGCRawMedia.Rows)
                        {
                            ImageButton _ImageButton = _GridViewRow.FindControl("ibtnDownload") as ImageButton;

                            if (_ImageButton != null)
                            {
                                ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(_ImageButton);
                            }
                        }
                    }
                    else
                    {
                        grvUGCRawMedia.Columns[7].Visible = false;
                    }


                    if (_SessionInformation.IsUGCUploadEdit)
                    {
                        grvUGCRawMedia.Columns[0].Visible = true;
                        grvUGCRawMedia.Columns[8].Visible = true;
                        btnRefresh.Visible = true;
                        btnDelete.Visible = true;
                        btnUploadMedia.Visible = true;
                        tgtUpload.Visible = true;
                        mpeUploadMedia.Enabled = true;
                        pnlUploadMedia.Visible = true;

                        //foreach (GridViewRow _GridViewRow in grvUGCRawMedia.Rows)
                        //{
                        //    LinkButton _LinkButtonEdit = _GridViewRow.FindControl("lbtnEdit") as LinkButton;

                        //    if (_LinkButtonEdit != null)
                        //    {
                        //        ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(_LinkButtonEdit);
                        //    }
                        //}
                    }
                    else
                    {
                        grvUGCRawMedia.Columns[0].Visible = false;
                        grvUGCRawMedia.Columns[8].Visible = false;
                        btnRefresh.Visible = false;
                        btnDelete.Visible = false;
                        btnUploadMedia.Visible = false;
                        tgtUpload.Visible = false;
                        mpeUploadMedia.Enabled = false;
                        pnlUploadMedia.Visible = false;

                    }
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {

                HiddenField hfUGCIDEdit = (HiddenField)grvUGCRawMedia.Rows[((sender as LinkButton).NamingContainer as GridViewRow).RowIndex].FindControl("hfUGCGUID");
                IUGCRawMediaController _IUGCRawMediaController = _ControllerFactory.CreateObject<IUGCRawMediaController>();
                UGCRawMedia _objUGCRawMedia = new UGCRawMedia();
                _objUGCRawMedia.UGCGUID = new Guid(hfUGCIDEdit.Value);
                UGCRawMedia _objUGCRawMedia1 = _IUGCRawMediaController.GetUGCRawMediabyUGCGUID(_objUGCRawMedia);
                if (_objUGCRawMedia1 != null)
                {
                    txtEditUGCTitle.Text = _objUGCRawMedia1.Title;
                    txtEditUGCDesc.Text = _objUGCRawMedia1.Description;
                    txtEditUGCKeyword.Text = _objUGCRawMedia1.Keywords;
                    ddlPCategory.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_objUGCRawMedia1.CategoryGUID)) ? "0" : Convert.ToString(_objUGCRawMedia1.CategoryGUID);
                    ddlEditSubCategory1.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_objUGCRawMedia1.SubCategory1GUID)) ? "0" : Convert.ToString(_objUGCRawMedia1.SubCategory1GUID);
                    ddlEditSubCategory2.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_objUGCRawMedia1.SubCategory2GUID)) ? "0" : Convert.ToString(_objUGCRawMedia1.SubCategory2GUID);
                    ddlEditSubCategory3.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_objUGCRawMedia1.SubCategory3GUID)) ? "0" : Convert.ToString(_objUGCRawMedia1.SubCategory3GUID);
                    ddlOwner.SelectedValue = string.IsNullOrEmpty(Convert.ToString(_objUGCRawMedia1.CustomerGUID)) ? "0" : Convert.ToString(_objUGCRawMedia1.CustomerGUID);
                    hdnEditUDCID.Value = Convert.ToString(_objUGCRawMedia1.UGCGUID);

                    string _Script = "UpdateSubCategory2(\"" + ddlPCategory.ClientID + "\");";
                    _Script += "UpdateSubCategory2(\"" + ddlEditSubCategory1.ClientID + "\");";
                    _Script += "UpdateSubCategory2(\"" + ddlEditSubCategory2.ClientID + "\");";

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadCatList", _Script, true);


                    mdlpopupUGC.Show();
                    UpdateUpdatePanel(upEditUGC);
                }


            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValidateSearch())
                {
                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                    _ViewstateInformation.IsIQCustomSearchActive = true;
                    _ViewstateInformation._CurrentClipPage = 0;
                    SetViewstateInformation(_ViewstateInformation);
                    ClearUGCGridFields();
                    BindUGCRawMedia(true);
                    UpdateUpdatePanel(upUploadMedia);
                }
                else
                {
                    string _Script = "setCheckbox(\"" + chkCategories1.ClientID + "\",\"" + txtCat1Selection.ClientID + "\");";
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
            ViewstateInformation _ViewstateInformation = GetViewstateInformation();

            if (_ViewstateInformation.IsIQCustomSearchActive == true)
            {
                _ViewstateInformation.IsIQCustomSearchActive = false;
                _ViewstateInformation.ClipSortExpression = "ClipCreationDate";
                SetViewstateInformation(_ViewstateInformation);

                txtSearch.Text = string.Empty;

                cbAll.Checked = true;

                cbDescription.Checked = true;
                cbKeywords.Checked = true;
                cbTitle.Checked = true;


                cbDescription.Disabled = true;
                cbKeywords.Disabled = true;
                cbTitle.Disabled = true;

                txtFromDate.Text = "";
                txtToDate.Text = "";

                chkCategories1.SelectedValue = null;

                chkOwnerList.SelectedValue = null;
                txtCat1Selection.Text = "";

                txtOwnerSelection.Text = "";
                ClearUGCGridFields();
                BindUGCRawMedia(true);
                UpdateUpdatePanel(upUploadMedia);

            }
        }

        protected void btnUpdateUGC_Click(object sender, EventArgs e)
        {
            try
            {
                IUGCRawMediaController _IUGCRawMediaController = _ControllerFactory.CreateObject<IUGCRawMediaController>();

                Guid? _NullCategoryGUID = null;
                Guid? SubCategory1GUID = ddlEditSubCategory1.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlEditSubCategory1.SelectedValue);
                Guid? SubCategory2GUID = ddlEditSubCategory2.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlEditSubCategory2.SelectedValue);
                Guid? SubCategory3GUID = ddlEditSubCategory3.SelectedValue == "0" ? _NullCategoryGUID : new Guid(ddlEditSubCategory3.SelectedValue);
                // string _Result = _IUGCRawMediaController.UpdateUGCRawMedia(new Guid(hdnEditUDCID.Value), new Guid(ddlOwner.SelectedValue), new Guid(ddlPCategory.SelectedValue), SubCategory1GUID, SubCategory2GUID, SubCategory3GUID, txtEditUGCTitle.Text, txtEditUGCKeyword.Text, txtEditUGCDesc.Text);


                var doc = new XDocument(
                new XElement("Root", new XElement("Meta", new XAttribute("Key", "UGC-Title"), new XAttribute("Value", txtEditUGCTitle.Text)),
                                    new XElement("Meta", new XAttribute("Key", "UGC-Kwords"), new XAttribute("Value", txtEditUGCKeyword.Text)),
                                    new XElement("Meta", new XAttribute("Key", "UGC-Desc"), new XAttribute("Value", txtEditUGCDesc.Text)),
                                    new XElement("Meta", new XAttribute("Key", "UGC-Category"), new XAttribute("Value", ddlPCategory.SelectedValue)),
                                    SubCategory1GUID == null ? null : new XElement("Meta", new XAttribute("Key", "UGC-SubCategory1"), new XAttribute("Value", SubCategory1GUID)),
                                    SubCategory2GUID == null ? null : new XElement("Meta", new XAttribute("Key", "UGC-SubCategory2"), new XAttribute("Value", SubCategory2GUID)),
                                    SubCategory3GUID == null ? null : new XElement("Meta", new XAttribute("Key", "UGC-SubCategory3"), new XAttribute("Value", SubCategory3GUID)),
                                    new XElement("Meta", new XAttribute("Key", "iQUser"), new XAttribute("Value", ddlOwner.SelectedValue))));


                string _Result = _IUGCRawMediaController.UpdateUGCRawMedia(new Guid(hdnEditUDCID.Value), new SqlXml(doc.CreateReader()));

                if (string.IsNullOrEmpty(_Result) || Convert.ToInt32(_Result) <= 0)
                {
                    lblUGCMsg.Text = "An error occurred please try again.";
                    mdlpopupUGC.Show();

                }
                else
                {
                    lblTxtMsg.Text = "Record updated Successfully.";
                    lblTxtMsg.ForeColor = System.Drawing.Color.Green;
                    lblTxtMsg.Visible = true;

                    ClearUGCGridFields();
                    BindUGCRawMedia(true);

                    UpdateUpdatePanel(upUploadMedia);
                    mdlpopupUGC.Hide();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #region Ftp Browse Events and Methods


        private void SetClientFtpUpload()
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                string _ClientUGCFtpDetail = _IClientController.GetClientFtpDetilByClientID(_SessionInformation.ClientID);
                if (!string.IsNullOrWhiteSpace(_ClientUGCFtpDetail))
                {
                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                    _ViewstateInformation.FtpUrl = _ClientUGCFtpDetail;
                    SetViewstateInformation(_ViewstateInformation);
                    IsAllowFtp.Value = "1";
                    trVDir.Nodes.Add(new TreeNode("..", _ViewstateInformation.FtpUrl));
                    BindDir(_ViewstateInformation.FtpUrl, true);
                    BindFiles(_ViewstateInformation.FtpUrl);
                    modalpopupFtpBrowse.Enabled = true;
                    pnlFtpBrowse.Visible = true;

                }
                else
                {
                    modalpopupFtpBrowse.Enabled = false;
                    btnFtpBrowse.Visible = false;
                    pnlFtpBrowse.Visible = false;
                    //pnlFtpBrowse.Controls.Remove(ucFtpBrowse);
                    IsAllowFtp.Value = "0";
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnFtpBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                if (hndIsFtpUpload.Value != "true")
                {
                    _ViewstateInformationFtp = GetViewstateInformation();
                    trVDir.CollapseAll();
                    trVDir.Nodes[0].Expand();
                    trVDir.Nodes[0].Selected = true;
                    BindFiles(_ViewstateInformationFtp.FtpUrl);
                    txtFileName.Text = string.Empty;
                    upFtpBrowse.Update();
                    modalpopupFtpBrowse.Show();
                }
                else
                {
                    modalpopupFtpBrowse.Show();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void BindFiles(string FtpURL)
        {
            try
            {
                string[] _files = Directory.GetFiles(FtpURL);
                ListOfFiles = (from file in _files select new FtpFileInfo { Name = file.Substring(file.LastIndexOf("\\") + 1) }).ToList();

                grdFiles.DataSource = ListOfFiles;
                grdFiles.DataBind();
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
        }

        public void BindDir(string FtpURL, bool IsParent)
        {
            try
            {
                string[] _directories = Directory.GetDirectories(FtpURL);
                ListOfDirectory = (from dir in _directories select new FtpDirectoryInfo { Name = dir.Substring(dir.LastIndexOf("\\") + 1), Path = dir.Substring(0, dir.LastIndexOf("\\") + 1) }).ToList();

                foreach (var ftpdin in ListOfDirectory)
                {
                    TreeNode trParent = new TreeNode();
                    trParent.Text = ftpdin.Name;
                    trParent.Value = ftpdin.Path + ftpdin.Name;
                    if (IsParent)
                        trVDir.Nodes[0].ChildNodes.Add(trParent);
                    else
                        trVDir.SelectedNode.ChildNodes.Add(trParent);
                }
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                _ViewstateInformationFtp = GetViewstateInformation();
                string FilePath = trVDir.SelectedNode == null ? _ViewstateInformationFtp.FtpUrl : trVDir.SelectedNode.Value;
                _ViewstateInformationFtp.FtpFilePath = FilePath;
                SetViewstateInformation(_ViewstateInformationFtp);
                fileuploadname.Value = txtFileName.Text;
                hdnName.Value = txtFileName.Text;
                hndIsFtpUpload.Value = "true";
                //pnlFtpBrowse.Visible = false;
                modalpopupFtpBrowse.Hide();
                mpeUploadMedia.Show();
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "disableuplodify", "$('#ctl00_Content_Data_IQCustom1_divFU').attr('style','display:none');", true);
                upUploadMedia.Update();
                upFtpBrowse.Update();
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                hndIsFtpUpload.Value = "false";
                fileuploadname.Value = string.Empty;
                hdnName.Value = string.Empty;
                modalpopupFtpBrowse.Hide();
                //pnlFtpBrowse.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "LoadUploadify1", "LoadUploadify();", true);

                mpeUploadMedia.Show();
                upUploadMedia.Update();
                upFtpBrowse.Update();
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
        }

        // capturing SelectedNodeChanged even to get the directory and files in selected directory
        protected void trVDir_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                BindFiles(trVDir.SelectedValue.Trim());
                if (trVDir.SelectedNode.ChildNodes.Count <= 0)
                    BindDir(trVDir.SelectedValue.Trim(), false);
                trVDir.SelectedNode.Expand(); //expand the selected node
                txtFileName.Text = string.Empty;
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }

        }

        protected void lnkFilename_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkbutton = (LinkButton)sender;
                txtFileName.Text = lnkbutton.Text;
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
        }
        #endregion

        #region Paging

        protected void ucCustomPager_PageIndexChange(object sender,EventArgs e)// int currentpageNumber)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (_ViewstateInformation != null)
                {
                    //_ViewstateInformation._CurrentUGCRawMediaPage = currentpageNumber;
                    BindUGCRawMedia(false);
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