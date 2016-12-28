using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Core.Enumeration;
using PMGSearch;
using IQMediaGroup.Controller.Common;
using System.Threading;
using System.Runtime.InteropServices;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.ClipandRawMediaControl
{
    public partial class ClipandRawMedia : BaseControl
    {
        #region Member Variables

        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        int _NoOfResultsFromDBArchiveClip = 11;

        #endregion Member Variables

        #region Page Events
        protected override void OnLoad(EventArgs e)
        {
            try
            {

                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                _SessionInformation.TabActiceKey = "False";
                IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);

                hfIsTimeOut.Value = true.ToString().ToLower();
                lblTimeOutMsg.Text = string.Empty;
                lblTimeOutMsg.Style.Add("display", "none");

                if (!IsPostBack)
                {

                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                    int _CurrentTime = DateTime.Now.Hour;
                    int? _FromTime = null;
                    int? _ToTime = null;
                    if (_CurrentTime > 12)
                    {
                        _FromTime = _CurrentTime - 12;
                    }
                    else
                    {
                        _FromTime = _CurrentTime;
                    }
                    _ToTime = _FromTime - 1;


                    if (!string.IsNullOrEmpty(_ViewstateInformation._ClipSearchTerm))
                    {
                        txtSearchText.Text = _ViewstateInformation._ClipSearchTerm;
                    }

                    ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                    List<CustomerClientRoleAccess> _ListOfCustomerClientRoleAccess = _ICustomerRoleController.GetCustomerClientRoleAccess(_SessionInformation.CustomerKey);

                    GetClientStatSkedProgData();


                    //================================================================================
                    //This loop etrates for all cutomer roles in list of customer roles and makes 
                    //advanced search tab visible
                    //================================================================================

                    foreach (CustomerClientRoleAccess _CustomerRoles in _ListOfCustomerClientRoleAccess)
                    {
                        if (_CustomerRoles.RoleName == RolesName.AdvancedSearchAccess.ToString() && _CustomerRoles.RoleIsActive == true && _CustomerRoles.CustomerAccess == true && _CustomerRoles.ClientAccess == true)
                        {
                            break;
                        }
                    }
                }
                if (ConfigurationManager.AppSettings[CommonConstants.ConfigNoOfResultsFromDBArchiveClip] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings[CommonConstants.ConfigNoOfResultsFromDBArchiveClip], out _NoOfResultsFromDBArchiveClip);
                }
                SortArchiveClipDirection();
                SortSearchRawMediaDirection();
                lblRawMediaMsg.Visible = false;

            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion Page Events

        #region RawMedia Search

        private void GetClientStatSkedProgData()
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                IStatSkedProgController _IStatSkedProgController = _ControllerFactory.CreateObject<IStatSkedProgController>();
                Boolean IsAllDmaAllowed = true;
                Boolean IsAllStationAllowed = true;
                Boolean IsAllClassAllowed = true;
                _ViewstateInformation.MasterStatSkedProgClientSettings = _IStatSkedProgController.GetAllDetailByClientSettings(new Guid(_SessionInformation.ClientGUID), out IsAllDmaAllowed, out IsAllStationAllowed, out IsAllClassAllowed);

                _ViewstateInformation.IsAllDmaAllowed = IsAllDmaAllowed;
                _ViewstateInformation.IsAllStationAllowed = IsAllStationAllowed;
                _ViewstateInformation.IsAllClassAllowed = IsAllClassAllowed;
                SetViewstateInformation(_ViewstateInformation);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        protected void btnSearchRawMedia_Click(object sender, EventArgs e)
        {
            try
            {
                //string temp = HttpContext.Current.Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName].Value;

                if (Page.IsValid)
                {
                    List<RawMedia> _ListOfRawMedia = new List<RawMedia>();

                    try
                    {
                        ViewstateInformation _ViewstateInformation = GetViewstateInformation();                       
                        _ViewstateInformation.SortExpression = "datetime";
                        _ViewstateInformation.IsSortDirecitonAsc = false;
                        SetViewstateInformation(_ViewstateInformation);

                        _ListOfRawMedia = GetRawMediaResults(0);

                        BindRawMediaGrid(_ListOfRawMedia);

                    }
                    catch (Exception _InnerException)
                    {
                        lblRawMediaMsg.Text = CommonConstants.MsgBasicSearchUA;
                        lblRawMediaMsg.Visible = true;
                    }
                }
            }

            catch (MyException _MyException)
            {
                lblRawMediaMsg.Text = _MyException._StrMsg;
                lblRawMediaMsg.Visible = true;
            }
            catch (TimeoutException _TimeoutException)
            {
                lblRawMediaMsg.Text = CommonConstants.TimeOutException;
                lblRawMediaMsg.Visible = true;
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        private void SortSearchRawMediaDirection()
        {
            ViewstateInformation _ViewstateInformation = GetViewstateInformation();

            GridViewRow gridViewHeaderSearchRow = grvRawMedia.HeaderRow;
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

        private List<RawMedia> GetRawMediaResults(int p_PageNumber)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                int _RawMediaPageCount = 10;

                if (ConfigurationManager.AppSettings[CommonConstants.ConfigRawMediaPageSize] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings[CommonConstants.ConfigRawMediaPageSize], out _RawMediaPageCount);
                }

                int _PMGMaxHighlights = 20;

                if (ConfigurationManager.AppSettings[CommonConstants.ConfigPMGMaxHighlights] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings[CommonConstants.ConfigPMGMaxHighlights], out _PMGMaxHighlights);
                }

                SearchRequest _SearchRequest = new SearchRequest();
                _SearchRequest.Terms = txtSearchMediaText.Text.Trim();
                //_SearchRequest.PageNumber = p_PageNumber + 1;
                _SearchRequest.PageNumber = p_PageNumber;
                _SearchRequest.PageSize = _RawMediaPageCount;
                _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                if (_ViewstateInformation.IsAllDmaAllowed == false && _ViewstateInformation.MasterStatSkedProgClientSettings._ListofMarket.Count() > 0)
                {
                    _SearchRequest.IQDmaName = _ViewstateInformation.MasterStatSkedProgClientSettings._ListofMarket.Select(m=>m.IQ_Dma_Name).ToList();
                }

                if (_ViewstateInformation.IsAllStationAllowed == false && _ViewstateInformation.MasterStatSkedProgClientSettings._ListofAffil.Count() > 0)
                {
                    _SearchRequest.StationAffil = _ViewstateInformation.MasterStatSkedProgClientSettings._ListofAffil.Select(s => s.Station_Affil).ToList();
                }

                if (_ViewstateInformation.IsAllClassAllowed == false && _ViewstateInformation.MasterStatSkedProgClientSettings._ListofType.Count() > 0)
                {
                    _SearchRequest.IQClassNum = _ViewstateInformation.MasterStatSkedProgClientSettings._ListofType.Select(s => s.IQ_Class_Num).ToList();
                }

                if (_ViewstateInformation != null && !string.IsNullOrEmpty(_ViewstateInformation.SortExpression))
                {
                    if (!_ViewstateInformation.IsSortDirecitonAsc)
                    {
                        _SearchRequest.SortFields = _ViewstateInformation.SortExpression + "-";
                    }
                    else
                    {
                        _SearchRequest.SortFields = _ViewstateInformation.SortExpression;
                    }
                }                

                SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest);
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

                SearchLog(_SearchRequest.Terms, _SearchRequest.PageNumber, _SearchRequest.PageSize, _SearchRequest.MaxHighlights, _SearchRequest.StartDate, _SearchRequest.EndDate, _Responce);

                List<RawMedia> _ListOfRawMedia = new List<RawMedia>();

                bool _IsPmgSearchTotalHitsFromConfig = true;
                int _MaxPMGHitsCount = 100;

                if (ConfigurationManager.AppSettings[CommonConstants.ConfigPMGSearchTotalHitsFromConfig] != null)
                {
                    bool.TryParse(ConfigurationManager.AppSettings[CommonConstants.ConfigPMGSearchTotalHitsFromConfig], out _IsPmgSearchTotalHitsFromConfig);
                }

                if (_IsPmgSearchTotalHitsFromConfig == true)
                {
                    if (ConfigurationManager.AppSettings[CommonConstants.ConfigPMGMaxListCount] != null)
                    {
                        int.TryParse(ConfigurationManager.AppSettings[CommonConstants.ConfigPMGMaxListCount], out _MaxPMGHitsCount);
                    }

                    _SearchResult.TotalHitCount = _MaxPMGHitsCount;
                }

                lblNoOfRawMedia.Text = _SearchResult.TotalHitCount.ToString();

                foreach (Hit _Hit in _SearchResult.Hits)
                {
                    RawMedia _RawMedia = new RawMedia();
                    _RawMedia.RawMediaID = new Guid(_Hit.Guid);
                    _RawMedia.StationID = _Hit.StationId;
                    _RawMedia.Market = _Hit.Market;
                    //_RawMedia.Affiliate = _Hit.affiliate;
                    _RawMedia.Hits = _Hit.TotalNoOfOccurrence;
                    _RawMedia.Title120 = _Hit.Title120;
                    _RawMedia.DateTime = new DateTime(_Hit.Timestamp.Year, _Hit.Timestamp.Month, _Hit.Timestamp.Day, (_Hit.Hour / 100), 0, 0);

                    List<RawMediaCC> _ListOfRawMediaCC = new List<RawMediaCC>();
                    foreach (TermOccurrence _TermOccurrence in _Hit.TermOccurrences)
                    {
                        RawMediaCC _RawMediaCC = new RawMediaCC();
                        _RawMediaCC.SearchTerm = _TermOccurrence.SearchTerm;
                        _RawMediaCC.SurroundingText = _TermOccurrence.SurroundingText;
                        _RawMediaCC.TimeOffset = _TermOccurrence.TimeOffset;
                        _ListOfRawMediaCC.Add(_RawMediaCC);
                    }

                    _RawMedia.RawMediaCloseCaptions = _ListOfRawMediaCC;
                    _ListOfRawMedia.Add(_RawMedia);
                }

                #region Pagination



                _ViewstateInformation.RawMediaTotalHitsCount = _SearchResult.TotalHitCount;
                _ViewstateInformation.CurrentRawMediaPage = p_PageNumber;

                SetViewstateInformation(_ViewstateInformation);

                int _MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_SearchResult.TotalHitCount) / Convert.ToDouble(_RawMediaPageCount))));

                if (_SearchResult.TotalHitCount > _RawMediaPageCount && p_PageNumber < (_MaxPage - 1))
                {
                    imgRawMediaNext.Visible = true;
                }
                else
                {
                    imgRawMediaNext.Visible = false;
                }

                if (p_PageNumber > 0)
                {
                    imgRawMediaPrevious.Visible = true;
                }
                else
                {
                    imgRawMediaPrevious.Visible = false;
                }

                #endregion Pagination

                #region Current Page No. Label


                if (_SearchResult.TotalHitCount > 0)
                {
                    lblCurrentPageNo.Visible = true;
                    lblCurrentPageNo.Text = CommonConstants.CurrentPageNoText + Convert.ToString(p_PageNumber + 1);
                }
                else
                {
                    lblCurrentPageNo.Visible = false;
                }

                #endregion

                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                _SessionInformation.ListOfRawMedia = _ListOfRawMedia;

                CommonFunctions.SetSessionInformation(_SessionInformation);

                return _ListOfRawMedia;
            }
            catch (System.TimeoutException _TimeoutException)
            {
                throw _TimeoutException;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void SearchLog(string _Terms, int _PageNumber, int _PageSize, int _MaxHighlights, DateTime? _StartDate, DateTime? _EndDate, string _Response)
        {
            IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

            string _FileContent = string.Empty;
            _FileContent = "<PMGRequest>";
            _FileContent += "<Terms>" + _Terms + "</Terms>";
            _FileContent += "<PageNumber>" + _PageNumber + "</PageNumber>";
            _FileContent += "<PageSize>" + _PageSize + "</PageSize>";
            _FileContent += "<MaxHighlights>" + _MaxHighlights + "</MaxHighlights>";
            if (_StartDate.HasValue)
            {
                _FileContent += "<StartDate>" + _StartDate + "</StartDate>";
            }
            else
            {
                _FileContent += "<StartDate></StartDate>";
            }
            if (_EndDate.HasValue)
            {
                _FileContent += "<EndDate>" + _EndDate + "</EndDate>";
            }
            else
            {
                _FileContent += "<EndDate></EndDate>";
            }
            _FileContent += "</PMGRequest>";

            string _Result = string.Empty;
            ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
            SearchLog _SearchLog = new SearchLog();
            _SearchLog.CustomerID = _SessionInformation.CustomerKey;
            _SearchLog.SearchType = "IQBasic";
            _SearchLog.RequestXML = _FileContent;
            _SearchLog.ErrorResponseXML = _Response;
            _Result = _ISearchLogController.InsertSearchLog(_SearchLog);
        }

        protected void grvRawMedia_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable _DataTable = (IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation()).RawMediaSearchResult;

                grvRawMedia.PageIndex = e.NewPageIndex;

                grvRawMedia.DataSource = _DataTable;
                grvRawMedia.DataBind();

                SortSearchRawMediaDirection();

            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvRawMedia_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                AddGridSortImage(grvRawMedia, grvRawMedia.HeaderRow);
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

                SetViewstateInformation(_ViewstateInformation);

                List<RawMedia> _ListOfRawMedia = GetRawMediaResults(0);
                BindRawMediaGrid(_ListOfRawMedia);

                //SortSearchRawMediaDirection();
            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        /// <summary>
        /// Descritption:This method bind list of raw media to grid.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ListOfRawMedia">List of RawMedia</param>
        public void BindRawMediaGrid(List<RawMedia> p_ListOfRawMedia)
        {
            try
            {
                grvRawMedia.PageIndex = 0;

                foreach (RawMedia _RawMedia in p_ListOfRawMedia)
                {
                    if (File.Exists(Server.MapPath("~/StationLogoImages/" + _RawMedia.StationID + ".gif")))
                    {
                        _RawMedia.StationLogo = "http://" + Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _RawMedia.StationID + ".gif";
                    }
                    else if (File.Exists(Server.MapPath("~/StationLogoImages/" + _RawMedia.StationID + ".jpg")))
                    {
                        _RawMedia.StationLogo = "http://" + Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _RawMedia.StationID + ".jpg";
                    }
                }

                grvRawMedia.DataSource = p_ListOfRawMedia;
                grvRawMedia.DataBind();

                grvRawMedia.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;");

                DataTable _DataTable = GenericToDataTable.ConvertTo<RawMedia>(p_ListOfRawMedia);

                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                _SessionInformation.RawMediaSearchResult = _DataTable;

                IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
                pnlClip.Visible = false;
                grvClip.Visible = false;
                pnl.Visible = true;
                grvRawMedia.Visible = true;
                //divtxtplay.Visible = true;
                lblSearchResult.Text = CommonConstants.RMSearchResultLabel;
                ViewstateInformation _ViewstateInformation =GetViewstateInformation();
                int _RawMediaPageCount = 10;

                if (ConfigurationManager.AppSettings[CommonConstants.ConfigRawMediaPageSize] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings[CommonConstants.ConfigRawMediaPageSize], out _RawMediaPageCount);
                }
                int _MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation.RawMediaTotalHitsCount) / Convert.ToDouble(_RawMediaPageCount))));
                //if (_MaxPage == 0)
                //{
                //    divtxtplay.Visible = false;
                //}

                hfIsTimeOut.Value = true.ToString().ToLower();
                SortSearchRawMediaDirection();
                UpdateUpdatePanel(upRawMediaClip);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion RawMedia Search

        #region Clip Search

        protected void btnSearchClip_Click(object sender, EventArgs e)
        {
            try
            {
                RegistrScript(true);

                if (Page.IsValid)
                {
                    ClearClipViewState();

                    SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                    _ViewstateInformation._ClipSearchTerm = txtSearchText.Text;
                    _ViewstateInformation.SortExpression = "ClipCreationDate";
                    _ViewstateInformation.IsSortDirecitonAsc = false;

                    List<ArchiveClip> _ListOfArchiveClip = GetClipDataFromDB(new Guid(_SessionInformation.ClientGUID), txtSearchText.Text, 0, _NoOfResultsFromDBArchiveClip, _ViewstateInformation.SortExpression, _ViewstateInformation.IsSortDirecitonAsc);

                    _ViewstateInformation._ListOfArchiveClipSearch = _ListOfArchiveClip;
                    _ViewstateInformation._CurrentArchiveClipSearchPage = 0;

                    SetViewstateInformation(_ViewstateInformation);

                    grvClip.PageIndex = 0;

                    BindClipGrid(_ListOfArchiveClip);

                    SortArchiveClipDirection();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void RegistrScript(bool p_IsClip)
        {
            try
            {
                if (p_IsClip == true)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), CommonConstants.JSFunctionClipKey, CommonConstants.JSFunctionClipActive, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), CommonConstants.JSFunctionClipKey, CommonConstants.JSFunctionRMActive, true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ClearClipViewState()
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                _ViewstateInformation._ClipSearchTerm = string.Empty;
                _ViewstateInformation._ListOfArchiveClipSearch = null;
                _ViewstateInformation._CurrentArchiveClipSearchPage = null;
                _ViewstateInformation.SortDirection = string.Empty;
                _ViewstateInformation.SortExpression = string.Empty;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void SortArchiveClipDirection()
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
        
        public List<ArchiveClip> GetClipDataFromDB(Guid p_ClientGUID, string p_SearchTerm, int p_PageNo, int p_PageSize, string p_SortField, bool p_IsAscending)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                int _TotalRecordsCount = 0;

                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();

                List<ArchiveClip> _ListOfArchiveClip = _IArchiveClipController.GetArchiveClipBySearchTerm(p_ClientGUID, p_SearchTerm, p_PageNo, p_PageSize, p_SortField, p_IsAscending, out _TotalRecordsCount);

                _ViewstateInformation.TotalRecordsCountArchiveClip = _TotalRecordsCount;

                SetViewstateInformation(_ViewstateInformation);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method bind clip grid
        /// </summary>
        /// <param name="p_ListOfClip">List of clips</param>
        private void BindClipGrid(List<ArchiveClip> p_ListOfArchiveClip)
        {
            try
            {

                grvClip.DataSource = p_ListOfArchiveClip;
                grvClip.DataBind();

                grvClip.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;");

                if (grvClip.Rows.Count == Convert.ToInt32(CommonConstants.Zero))
                {
                    pnlClip.Height = CommonConstants.Hundred;
                }
                else if (grvClip.Rows.Count > 0 && grvClip.Rows.Count < 8)
                {
                    pnlClip.Height = grvClip.Rows.Count * 100;
                }
                else
                {
                    pnlClip.Height = 550;
                }

                pnlClip.Visible = true;
                grvClip.Visible = true;
                pnl.Visible = false;
                grvRawMedia.Visible = false;
                UpdateUpdatePanel(upRawMediaClip);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        protected void grvClip_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                RegistrScript(true);

                grvClip.PageIndex = 0;

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                if (_ViewstateInformation.SortExpression == null || _ViewstateInformation.SortExpression.ToLower() != e.SortExpression.ToLower())
                {
                    _ViewstateInformation.SortExpression = e.SortExpression;
                    _ViewstateInformation.SortDirection = SortDirection.Ascending.ToString();
                }

                if (string.IsNullOrEmpty(_ViewstateInformation.SortDirection))
                {
                    _ViewstateInformation.SortDirection = SortDirection.Ascending.ToString();
                }

                if (_ViewstateInformation.SortDirection.ToLower() == SortDirection.Ascending.ToString().ToLower())
                {
                    _ViewstateInformation.SortDirection = SortDirection.Descending.ToString();
                    _ViewstateInformation.IsSortDirecitonAsc = true;
                }
                else
                {
                    _ViewstateInformation.SortDirection = SortDirection.Ascending.ToString();
                    _ViewstateInformation.IsSortDirecitonAsc = false;
                }

                List<ArchiveClip> _ListOfArchiveClip = GetClipDataFromDB(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._ClipSearchTerm, 0, _NoOfResultsFromDBArchiveClip, e.SortExpression, _ViewstateInformation.IsSortDirecitonAsc);

                _ViewstateInformation._ListOfArchiveClipSearch = _ListOfArchiveClip;

                SetViewstateInformation(_ViewstateInformation);

                BindClipGrid(_ListOfArchiveClip);

                SortArchiveClipDirection();

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void lbtnPlay_Command(object sender, CommandEventArgs e)
        {
            try
            {
                RegistrScript(true);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowDiv", "showDivBox();", true);

                divClip.Style.Add("display", "block");
                Clipframe.Style.Add("display", "none");

                ClipPlayer2.RenderClip(e.CommandArgument.ToString());

                UpdateUpdatePanel(upVideo);

            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvClip_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                RegistrScript(true);

                grvClip.PageIndex = e.NewPageIndex;

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                List<ArchiveClip> _ListOfArchiveClip = null;

                if (_ViewstateInformation != null && _ViewstateInformation._ListOfArchiveClipSearch != null)
                {
                    int _MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation._ListOfArchiveClipSearch.Count) / Convert.ToDouble(grvClip.PageSize))));
                    int _MaxDBPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation.TotalRecordsCountArchiveClip) / Convert.ToDouble(_NoOfResultsFromDBArchiveClip))));

                    if ((e.NewPageIndex == (_MaxPage - 1)) && (_ViewstateInformation._CurrentArchiveClipSearchPage < _MaxDBPage))
                    {
                        _ViewstateInformation._CurrentArchiveClipSearchPage = _ViewstateInformation._CurrentArchiveClipSearchPage + 1;
                        _ListOfArchiveClip = GetClipDataFromDB(new Guid(_SessionInformation.ClientGUID), _ViewstateInformation._ClipSearchTerm, _ViewstateInformation._CurrentArchiveClipSearchPage.Value, _NoOfResultsFromDBArchiveClip, _ViewstateInformation.SortExpression, _ViewstateInformation.IsSortDirecitonAsc);
                        _ViewstateInformation._ListOfArchiveClipSearch.InsertRange(_ViewstateInformation._ListOfArchiveClipSearch.Count, _ListOfArchiveClip);
                    }
                }

                BindClipGrid(_ViewstateInformation._ListOfArchiveClipSearch);

                SortArchiveClipDirection();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion Clip Search

        #region PMG Search region

        protected void grvRawMedia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PlayVideo")
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                Button lbtnPlay = e.CommandSource as Button;
                if (_SessionInformation != null)
                {
                    List<RawMedia> _ListOfRawMedia = _SessionInformation.ListOfRawMedia;
                    if (_ListOfRawMedia != null)
                    {
                        RawMedia _RawMedia = _ListOfRawMedia.Find(delegate(RawMedia p_RawMedia) { return p_RawMedia.RawMediaID == new Guid(e.CommandArgument.ToString()); });
                        if (_RawMedia != null)
                        {

                            RawMediaID.Value = e.CommandArgument.ToString();

                            Clipframe.Attributes.Add("src", "http://" + Request.Url.Host.ToString() + ":" + Request.Url.Port + "/IFrameRawMedia/Default.aspx?RawMediaID=" + e.CommandArgument.ToString() + "&SearchTerm=" + HttpUtility.UrlEncode(txtSearchMediaText.Text) + "&IsUGC=false");                            

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowDiv", "showDivBox();", true);

                            divClip.Style.Add("display", "none");
                            Clipframe.Style.Add("display", "block");

                            UpdateUpdatePanel(upVideo);

                        }
                    }
                }
            }
        }

        protected void imgRawMediaNext_Click(object sender, EventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                List<RawMedia> _ListOfRawMedia = GetRawMediaResults((_ViewstateInformation.CurrentRawMediaPage + 1));

                BindRawMediaGrid(_ListOfRawMedia);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        protected void imgRawMediaPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                List<RawMedia> _ListOfRawMedia = GetRawMediaResults((_ViewstateInformation.CurrentRawMediaPage - 1));

                BindRawMediaGrid(_ListOfRawMedia);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion

    }
}