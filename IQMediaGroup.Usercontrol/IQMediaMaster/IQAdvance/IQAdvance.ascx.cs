using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Core.Enumeration;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Text;
using IQMediaGroup.Controller.Common;
using PMGSearch;
using System.Collections;
using System.Xml.Linq;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IQAdvance
{
    public partial class IQAdvance : BaseControl
    {

        #region Member Variables

        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        int _ResultsPerPage = 10;
        int _RawMediaPageCount = 10;
        ViewstateInformation _ViewstateInformation;

        #endregion Member Variables

        #region Page load Events

        protected override void OnLoad(EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {
                    txtFromDate.Attributes.Add("readonly", "true");
                    txtToDate.Attributes.Add("readonly", "true");
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

                    txtFromDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
                    txtToDate.Text = System.DateTime.Now.ToShortDateString();

                    BindStatSkedProgData();
                    SetDatalistSelection();
                    tblSearch.Style.Add("display", "block");

                    rptRadioStations.Visible = false;
                    rdoRadioStations.Visible = false;
                }
                rdoMarket.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "')");
                rdoMarket.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptMarket.ClientID + "')");

                rdoAffil.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "')");
                rdoAffil.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptAffil.ClientID + "')");

                rdoProgramType.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "')");
                rdoProgramType.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptProgramType.ClientID + "')");

                rdoRadioStations.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptRadioStations.ClientID + "')");
                rdoRadioStations.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptRadioStations.ClientID + "')");

                _ViewstateInformation = GetViewstateInformation();
                SortRadioButtonDirection();
                SortGridDirection();
                SetConfigParams();
                lblErrorMessage.Text = string.Empty;
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
            }
        }

        private void SetConfigParams()
        {
            try
            {

                if (ConfigurationManager.AppSettings["AdvancedSearchPageSize"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["AdvancedSearchPageSize"], out _ResultsPerPage);
                }

                if (ConfigurationManager.AppSettings["RawMediaPageSize"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["RawMediaPageSize"], out _RawMediaPageCount);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region User Defined Methods

        private void SortRadioButtonDirection()
        {
            _ViewstateInformation = GetViewstateInformation();

            GridViewRow gridViewHeaderSearchRow = grvRadioStations.HeaderRow;
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

        private void SortGridDirection()
        {
            _ViewstateInformation = GetViewstateInformation();

            GridViewRow gridViewHeaderSearchRow = null;
            gridViewHeaderSearchRow = grvRawMediaPMGBasic.HeaderRow;

            //GridViewRow gridViewHeaderSearchRow = grvRawMedia.HeaderRow;
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
                                if (headerSearchButton.CommandArgument.Trim().ToLower() == _ViewstateInformation.SortExpression.Trim().ToLower())
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

        public void BindStatSkedProgData()
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                _ViewstateInformation = GetViewstateInformation();
                MasterStatSkedProg _MasterStatSkedProg = null;
                IStatSkedProgController _IStatSkedProgController = _ControllerFactory.CreateObject<IStatSkedProgController>();
                Boolean IsAllDmaAllowed = true;
                Boolean IsAllStationAllowed = true;
                Boolean IsAllClassAllowed = true;
                _MasterStatSkedProg = _IStatSkedProgController.GetAllDetailByClientSettings(new Guid(_SessionInformation.ClientGUID), out IsAllDmaAllowed, out IsAllStationAllowed, out IsAllClassAllowed);

                _ViewstateInformation.IsAllDmaAllowed = IsAllDmaAllowed;
                _ViewstateInformation.IsAllStationAllowed = IsAllStationAllowed;
                _ViewstateInformation.IsAllClassAllowed = IsAllClassAllowed;
                SetViewstateInformation(_ViewstateInformation);

                if (_MasterStatSkedProg._ListofMarket.Count > 0)
                {
                    rptMarket.DataSource = _MasterStatSkedProg._ListofMarket;
                    rptMarket.DataBind();
                }

                if (_MasterStatSkedProg._ListofAffil.Count > 0)
                {
                    rptAffil.DataSource = _MasterStatSkedProg._ListofAffil;
                    rptAffil.DataBind();
                }

                if (_MasterStatSkedProg._ListofType.Count > 0)
                {
                    rptProgramType.DataSource = _MasterStatSkedProg._ListofType;
                    rptProgramType.DataBind();
                }



            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetDatalistSelection()
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                IIQClient_CustomSettingsController _IIQClient_CustomSettingsController = _ControllerFactory.CreateObject<IIQClient_CustomSettingsController>();

                string _xmlstring = _IIQClient_CustomSettingsController.GetSettingByClientGUID(new Guid(_SessionInformation.ClientGUID));
                if (!string.IsNullOrEmpty(_xmlstring))
                {
                    XDocument _xdoc = XDocument.Parse(_xmlstring);

                    if (_xdoc.Descendants("Station_Affiliate_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("Station_Affiliate_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                    {

                        foreach (XElement Xelem in _xdoc.Descendants("Station_Affiliate").Elements("name"))
                        {
                            foreach (DataListItem _item in rptAffil.Items)
                            {
                                HtmlInputCheckBox _chk = (HtmlInputCheckBox)_item.FindControl("chkAffil");
                                if (_chk.Value == Xelem.Value)
                                {
                                    _chk.Checked = true;
                                    break;
                                }
                            }
                        }
                        rdoAffil.Items[1].Selected = true;
                    }
                    else
                    {
                        rdoAffil.Items[0].Selected = true;
                        ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                    }

                    if (_xdoc.Descendants("IQ_Dma_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("IQ_Dma_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                    {

                        foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("name"))
                        {
                            foreach (DataListItem _item in rptMarket.Items)
                            {
                                HtmlInputCheckBox _chk = (HtmlInputCheckBox)_item.FindControl("chkMarket");
                                if (_chk.Value == Xelem.Value)
                                {
                                    _chk.Checked = true;
                                    break;
                                }
                            }
                        }
                        rdoMarket.Items[1].Selected = true;
                    }
                    else
                    {
                        rdoMarket.Items[1].Selected = false;
                        ScriptManager.RegisterStartupScript(rptMarket, rptMarket.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                    }


                    if (_xdoc.Descendants("IQ_Class_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("IQ_Class_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                    {
                        foreach (XElement Xelem in _xdoc.Descendants("IQ_Class").Elements("num"))
                        {
                            foreach (DataListItem _item in rptProgramType.Items)
                            {
                                HtmlInputCheckBox _chk = (HtmlInputCheckBox)_item.FindControl("chkProgramType");
                                if (_chk.Value == Xelem.Value)
                                {
                                    _chk.Checked = true;
                                    break;
                                }
                            }

                        }
                        rdoProgramType.Items[1].Selected = true;
                    }
                    else
                    {
                        rdoProgramType.Items[1].Selected = false;
                        ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                    ScriptManager.RegisterStartupScript(rptMarket, rptMarket.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                    ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        public void BindRawMediaGrid(List<RawMedia> listOfRawMedia)
        {
            try
            {
                int _MaxPMGPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation.RawMediaTotalHitsCount) / Convert.ToDouble(_RawMediaPageCount))));

                grvRawMediaPMGBasic.PageSize = _RawMediaPageCount;

                grvRawMediaPMGBasic.PageIndex = _ViewstateInformation.CurrentRawMediaPage - 1;

                grvRawMediaPMGBasic.DataSource = listOfRawMedia;
                grvRawMediaPMGBasic.DataBind();
                //divtxtplay.Visible = true;
                if (_MaxPMGPage > 1)
                {
                    lblCurrentPageNo.Text = CommonConstants.CurrentPageNoText + _ViewstateInformation.CurrentRawMediaPage.ToString();
                }
                else
                {
                    lblCurrentPageNo.Text = string.Empty;
                    //if (_MaxPMGPage == 0)
                    //{
                    //    divtxtplay.Visible = false;
                    //}
                }
                grvRawMediaPMGBasic.Attributes.Add("style", "word-break:break-all;word-wrap:break-word;overflow:hidden;table-layout:fixed;");

                pnl.Visible = true;

                if (_ViewstateInformation.CurrentRawMediaPage < _MaxPMGPage)
                {
                    imgRawMediaNext.Visible = true;
                }
                else
                {
                    imgRawMediaNext.Visible = false;
                }

                if (_ViewstateInformation.CurrentRawMediaPage > 1)
                {
                    imgRawMediaPrevious.Visible = true;
                }
                else
                {
                    imgRawMediaPrevious.Visible = false;
                }

                SortGridDirection();

                UpdateUpdatePanel(upGrid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Button Event


        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                ValidateFromDateTime();

                if (Page.IsValid)
                {
                    ClearSearchViewState();
                    Int16 _SelectedStations = Convert.ToInt16(rblTvOrRadio.SelectedValue);

                    _ViewstateInformation.SortExpression = "DateTime";
                    _ViewstateInformation.IsSortDirecitonAsc = false;
                    SetViewstateInformation(_ViewstateInformation);

                    if (_SelectedStations == 0)
                    {
                        GetRawMediaResults(1);
                        BindRawMediaGrid(_ViewstateInformation._IQAdvanceResponseListOfRawMedia);
                        pnlPMGSearchBasic.Visible = true;
                    }
                    else
                    {
                        int _PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["RadioStationPageSizeDB"].ToString());
                        GetSelectedRadioStations(0, _PageSize, _ViewstateInformation.SortExpression, _ViewstateInformation.IsSortDirecitonAsc);
                        BindRadioStationGrid();
                    }
                }
                else
                {
                    date.Update();
                    validate.Update();
                }
            }
            catch (MyException _MyException)
            {
                lblErrorMessage.Text = _MyException._StrMsg;
                lblErrorMessage.Visible = true;
            }
            catch (TimeoutException)
            {
                lblErrorMessage.Text = CommonConstants.TimeOutException;
                lblErrorMessage.Visible = true;
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);

                if (_Exception.Message.ToLower().Contains(CommonConstants.DBTimeOutTxt))
                {
                    Response.Redirect(CommonConstants.CustomErrorPageError);
                }
                else
                {
                    Response.Redirect(CommonConstants.CustomErrorPage);
                }

            }
        }

        private void GetSelectedRadioStations(int p_PageNumber, int p_PageSize, string p_SortExpression, bool p_IsSortDirectionAsc)
        {
            try
            {
                _ViewstateInformation = GetViewstateInformation();


                List<string> _ListOfDmaName = new List<string>();

                for (int _RadioStationsCount = 0; _RadioStationsCount < rptRadioStations.Items.Count; _RadioStationsCount++)
                {
                    HtmlInputCheckBox chkRadioStation = (HtmlInputCheckBox)rptRadioStations.Items[_RadioStationsCount].FindControl("chkRadioStation");
                    if (chkRadioStation.Checked)
                    {
                        _ListOfDmaName.Add(chkRadioStation.Value);
                    }

                }

                List<IQ_STATION> _ListOfRadioStations = _ViewstateInformation.ListOfRadioStations;
                List<IQ_STATION> _ListOfSelectedRadioStations = _ListOfRadioStations.FindAll(
                                           delegate(IQ_STATION _Main_RL_STATION)
                                           {
                                               string _dmaname = _ListOfDmaName.Find(delegate(string _TempDmaName) { return _TempDmaName == _Main_RL_STATION.dma_name; });
                                               return (!string.IsNullOrEmpty(_dmaname));
                                           }
                                       );

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

                DateTime _FromDate = Convert.ToDateTime(txtFromDate.Text).AddHours(_FromTime);
                DateTime _ToDate = Convert.ToDateTime(txtToDate.Text).AddHours(_ToTime);

                string _CSIQCCKey = string.Empty;

                List<string> _ListOfIQCCKey = new List<string>();

                for (DateTime _IndexFromDate = _FromDate; _IndexFromDate <= _ToDate; )
                {
                    string IQCCKey;
                    //if ((DateTime.UtcNow - DateTime.Now).Hours == 5)
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
                }

                Int64 _TotalRecordsCount = 0;

                if (!String.IsNullOrEmpty(_CSIQCCKey))
                {
                    IRL_GUIDSController _IRL_GUIDSController = _ControllerFactory.CreateObject<IRL_GUIDSController>();
                    List<RadioStation> _ListOfRadioStation = _IRL_GUIDSController.GetAllRL_GUIDSByRadioStations(_CSIQCCKey, p_PageNumber, p_PageSize, p_SortExpression, p_IsSortDirectionAsc, out _TotalRecordsCount);

                    if (_ViewstateInformation.ListOfRadioStation == null)
                    {
                        _ViewstateInformation.ListOfRadioStation = _ListOfRadioStation;
                    }
                    else
                    {
                        _ViewstateInformation.ListOfRadioStation.AddRange(_ListOfRadioStation);
                    }

                    _ViewstateInformation.TotalRadioStationsCountDB = _TotalRecordsCount;

                    lblNoOfRadioRawMedia.Text = _TotalRecordsCount.ToString();

                    SetViewstateInformation(_ViewstateInformation);
                }
                else
                {
                    grvRadioStations.DataSource = null;
                    grvRadioStations.DataBind();

                    pnlRadioStations.Visible = true;
                    pnl.Visible = true;

                    upGrid.Update();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SearchLog(SearchRequest _SearchRequest, string _SearchType, string _Response)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                string _FileContent = string.Empty;
                _FileContent = "<PMGRequest>";
                _FileContent += "<Terms>" + _SearchRequest.Terms + "</Terms>";
                _FileContent += "<PageNumber>" + _SearchRequest.PageNumber + "</PageNumber>";
                _FileContent += "<PageSize>" + _SearchRequest.PageSize + "</PageSize>";
                _FileContent += "<MaxHighlights>" + _SearchRequest.MaxHighlights + "</MaxHighlights>";
                if (_SearchRequest.StartDate.HasValue)
                {
                    _FileContent += "<StartDate>" + _SearchRequest.StartDate + "</StartDate>";
                }
                else
                {
                    _FileContent += "<StartDate></StartDate>";
                }

                if (!string.IsNullOrEmpty(_SearchRequest.TimeZone))
                {
                    _FileContent += "<IQ_Time_Zone>" + _SearchRequest.TimeZone + "</IQ_Time_Zone>";
                }
                else
                {
                    _FileContent += "<IQ_Time_Zone />";
                }

                if (_SearchRequest.IQDmaName.Count() > 0)
                {
                    _FileContent += "<IQ_Dma_Name>" + string.Join(",", _SearchRequest.IQDmaName.ToArray()) + "</IQ_Dma_Name>";
                }
                else
                {
                    _FileContent += "<IQ_Dma_Num />";
                }

                if (_SearchRequest.IQClassNum.Count() > 0)
                {
                    _FileContent += "<IQ_Class_Num>" + string.Join(",", _SearchRequest.IQClassNum.ToArray()) + "</IQ_Class_Num>";
                }
                else
                {
                    _FileContent += "<IQ_Class_Num />";
                }
                if (_SearchRequest.StationAffil.Count() > 0)
                {
                    _FileContent += "<Station_Affil>" + string.Join(",", _SearchRequest.StationAffil.ToArray()) + "</Station_Affil>";
                }
                else
                {
                    _FileContent += "<Station_Affil />";
                }
                if (!string.IsNullOrEmpty(_SearchRequest.Title120))
                {
                    _FileContent += "<Title120>" + _SearchRequest.Title120 + "</Title120>";
                }
                else
                {
                    _FileContent += "<Title120 />";
                }

                if (!string.IsNullOrEmpty(_SearchRequest.Appearing))
                {
                    _FileContent += "<Appearing>" + _SearchRequest.Appearing + "</Appearing>";
                }
                else
                {
                    _FileContent += "<Appearing />";
                }

                if (_SearchRequest.EndDate.HasValue)
                {
                    _FileContent += "<EndDate>" + _SearchRequest.EndDate + "</EndDate>";
                }
                else
                {
                    _FileContent += "<EndDate></EndDate>";
                }


                _FileContent += "</PMGRequest>";

                _FileContent = _FileContent.Replace("&", "&amp;");

                string _Result = string.Empty;
                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                SearchLog _SearchLog = new SearchLog();
                _SearchLog.CustomerID = _SessionInformation.CustomerKey;
                _SearchLog.SearchType = _SearchType;
                _SearchLog.RequestXML = _FileContent;
                _SearchLog.ErrorResponseXML = _Response;
                _Result = _ISearchLogController.InsertSearchLog(_SearchLog);
                Logger.Info("Search Log Result:" + _Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateFromDateTime()
        {
            if (!string.IsNullOrWhiteSpace(txtFromDate.Text) && !string.IsNullOrWhiteSpace(txtToDate.Text))
            {
                string FromDate = txtFromDate.Text.Trim() + " " + ddlStartTime.SelectedValue + ":00:00" + rdAmPmFromDate.SelectedItem.Text;
                string ToDate = txtToDate.Text.Trim() + " " + ddlEndTime.SelectedValue + ":00:00" + rdAMPMToDate.SelectedItem.Text;

                if (Convert.ToDateTime(FromDate) > Convert.ToDateTime(ToDate))
                {
                    pplEndDate.IsValid = false;
                    pplEndDate.ErrorMessage = CommonConstants.MsgFromToDate;
                }
            }
        }

        protected void RblTVOrRadio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Int16 _SelectedStations = Convert.ToInt16(rblTvOrRadio.SelectedValue);

                if (_SelectedStations == 0)
                {
                    trInitialSearch.Visible = true;
                    trAffiliateNetwork.Visible = true;
                    trProgramSubCategory.Visible = true;
                    trTimeZone.Visible = true;
                    rptMarket.Visible = true;
                    rdoMarket.Visible = true;

                    rptRadioStations.Visible = false;
                    rdoRadioStations.Visible = false;
                    pnlRadioStations.Visible = false;

                    rdoMarket.SelectedIndex = 0;
                    rdoAffil.SelectedIndex = 0;
                    rdoProgramType.SelectedIndex = 0;
                    SetDatalistSelection();
                    txtAppearing.Text = string.Empty;
                    txtProgram.Text = string.Empty;
                    txtSearch.Text = string.Empty;
                }
                else
                {
                    trInitialSearch.Visible = false;
                    trAffiliateNetwork.Visible = false;
                    trProgramSubCategory.Visible = false;
                    trTimeZone.Visible = false;

                    List<IQ_STATION> _ListOfRL_Station = GetRadioStations();

                    List<string> _ListOfDMAName = _ListOfRL_Station.Select(row => row.dma_name).Distinct().ToList();

                    _ViewstateInformation = GetViewstateInformation();
                    _ViewstateInformation.ListOfRadioStations = _ListOfRL_Station;
                    SetViewstateInformation(_ViewstateInformation);

                    rptRadioStations.DataSource = _ListOfDMAName;
                    rptRadioStations.DataBind();
                    rdoRadioStations.SelectedValue = "1";

                    ScriptManager.RegisterStartupScript(rptRadioStations, rptRadioStations.GetType(), "rptRadioStations", "CheckUnCheckAll(true,'" + rptRadioStations.ClientID + "');", true);
                    rptRadioStations.Visible = true;
                    rdoRadioStations.Visible = true;
                    rdoMarket.Visible = false;
                    rptMarket.Visible = false;
                    pnlPMGSearchBasic.Visible = false;
                }

                txtFromDate.Attributes.Add("readonly", "true");
                txtToDate.Attributes.Add("readonly", "true");
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
                _ToTime = _FromTime - 1;
                ddlEndTime.SelectedValue = _ToTime.ToString();
                ddlStartTime.SelectedValue = "0";

                txtFromDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
                txtToDate.Text = System.DateTime.Now.ToShortDateString();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public List<IQ_STATION> GetRadioStations()
        {
            try
            {
                IIQ_STATIONController _IRL_STATIONController = _ControllerFactory.CreateObject<IIQ_STATIONController>();
                List<IQ_STATION> _ListOfRLStation = _IRL_STATIONController.SelectAllRadioStations();

                return _ListOfRLStation;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Grid Events
        protected void LbtnRawMediaPlay_Command(object sender, CommandEventArgs e)
        {
            try
            {
                //Clipframe.Attributes.Add("src", "http://localhost:2281/IFrameRawMedia/Default.aspx?RawMediaID=" + e.CommandArgument.ToString() + "&SearchTerm=" + txtSearch.Text+"&IsUGC=false");

                ClipFrame.Attributes.Add("src", "http://" + Request.Url.Host.ToString() + ":" + Request.Url.Port + "/IFrameRawMedia/Default.aspx?RawMediaID=" + e.CommandArgument.ToString() + "&SearchTerm=" + HttpUtility.UrlEncode(txtSearch.Text) + "&IsUGC=false");

                ClipFrame.Visible = true;

                UpdateUpdatePanel(upVideo);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void ClearSearchViewState()
        {
            try
            {
                _ViewstateInformation.CurrentRawMediaPage = 0;
                _ViewstateInformation._IQAdvanceResponseListOfRawMedia = null;
                _ViewstateInformation.ListOfRadioStation = null;
                _ViewstateInformation.TotalRadioStationsCountDB = 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GrvRawMediaPmgBasic_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                AddGridSortImage(grvRawMediaPMGBasic, grvRawMediaPMGBasic.HeaderRow);

                _ViewstateInformation = GetViewstateInformation();


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

                GetRawMediaResults(1);



                SetViewstateInformation(_ViewstateInformation);

                BindRawMediaGrid(_ViewstateInformation._IQAdvanceResponseListOfRawMedia);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GrvRadioStations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                _ViewstateInformation = GetViewstateInformation();

                int _MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation.ListOfRadioStation.Count) / Convert.ToDouble(_ResultsPerPage))));
                int _MaxDBPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation.TotalRadioStationsCountDB) / Convert.ToDouble(ConfigurationManager.AppSettings["RadioStationPageSizeDB"]))));

                if ((e.NewPageIndex == (_MaxPage - 1)) && (_ViewstateInformation.CurrentRawMediaPage < (_MaxDBPage - 1)))
                {
                    int _PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["RadioStationPageSizeDB"].ToString());

                    GetSelectedRadioStations(_ViewstateInformation.CurrentRawMediaPage + 1, _PageSize, _ViewstateInformation.SortExpression, _ViewstateInformation.IsSortDirecitonAsc);
                }

                grvRadioStations.PageIndex = e.NewPageIndex;
                _ViewstateInformation.CurrentRawMediaPage = e.NewPageIndex;
                BindRadioStationGrid();

                SetViewstateInformation(_ViewstateInformation);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void BindRadioStationGrid()
        {
            try
            {
                _ViewstateInformation = GetViewstateInformation();

                grvRadioStations.DataSource = _ViewstateInformation.ListOfRadioStation;
                grvRadioStations.DataBind();

                if (grvRadioStations.PageCount >= 1)
                {
                    ImageButton btnPrevious = (ImageButton)((grvRadioStations.BottomPagerRow).FindControl("btnPrevious"));
                    ImageButton btnNext = (ImageButton)((grvRadioStations.BottomPagerRow).FindControl("btnNext"));
                    Label lblRadoCurrentPageNo = (Label)((grvRadioStations.BottomPagerRow).FindControl("lblRadoCurrentPageNo"));


                    lblRadoCurrentPageNo.Text = CommonConstants.CurrentPageNoText + (_ViewstateInformation.CurrentRawMediaPage + 1).ToString();

                    if (_ViewstateInformation.CurrentRawMediaPage < grvRadioStations.PageCount - 1)
                        btnNext.Visible = true;
                    else
                        btnNext.Visible = false;

                    if (_ViewstateInformation.CurrentRawMediaPage > 0 && grvRadioStations.PageCount > 1)
                        btnPrevious.Visible = true;
                    else
                        btnPrevious.Visible = false;

                }

                pnlRadioStations.Visible = true;
                pnl.Visible = true;
                SortRadioButtonDirection();
                upGrid.Update();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GrvRadioStations_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                ClearSearchViewState();
                _ViewstateInformation = GetViewstateInformation();



                if (String.IsNullOrEmpty(_ViewstateInformation.SortExpression))
                {
                    _ViewstateInformation.IsSortDirecitonAsc = true;
                }
                else
                {
                    _ViewstateInformation.IsSortDirecitonAsc = !_ViewstateInformation.IsSortDirecitonAsc;
                }

                _ViewstateInformation.SortExpression = e.SortExpression;

                int _PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["RadioStationPageSizeDB"]);

                GetSelectedRadioStations(0, _PageSize, e.SortExpression, _ViewstateInformation.IsSortDirecitonAsc);

                grvRadioStations.PageIndex = 0;

                BindRadioStationGrid();
                SetViewstateInformation(_ViewstateInformation);

                upGrid.Update();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Validations

        protected void CVFromDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (Convert.ToDateTime(txtFromDate.Text) > System.DateTime.Now)
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

        #region PMG Search

        protected void ImgRawMediaNext_Click(object sender, EventArgs e)
        {
            try
            {
                int _MaxPMGPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation.RawMediaTotalHitsCount) / Convert.ToDouble(_RawMediaPageCount))));
                int _MaxPMGResultPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation._IQAdvanceResponseListOfRawMedia.Count) / Convert.ToDouble(_RawMediaPageCount))));
                if ((_ViewstateInformation.CurrentRawMediaPage == (_MaxPMGResultPage)) && (_ViewstateInformation.CurrentRawMediaPage < (_MaxPMGPage)))
                {

                    GetRawMediaResults(_ViewstateInformation.CurrentRawMediaPage + 1, false);
                    BindRawMediaGrid(_ViewstateInformation._IQAdvanceResponseListOfRawMedia);
                }
                else
                {
                    _ViewstateInformation.CurrentRawMediaPage += 1;
                    SetViewstateInformation(_ViewstateInformation);
                    BindRawMediaGrid(_ViewstateInformation._IQAdvanceResponseListOfRawMedia);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ImgRawMediaPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                _ViewstateInformation.CurrentRawMediaPage -= 1;
                SetViewstateInformation(_ViewstateInformation);
                BindRawMediaGrid(_ViewstateInformation._IQAdvanceResponseListOfRawMedia);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetRawMediaResults(int p_PageNumber, bool p_IsInitialization = true)
        {
            try
            {
                System.Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());

                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                int _PMGMaxHighlights = 20;

                if (ConfigurationManager.AppSettings["PMGMaxHighlights"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["PMGMaxHighlights"], out _PMGMaxHighlights);
                }

                SearchRequest _SearchRequest = new SearchRequest();
                _SearchRequest.Terms = txtSearch.Text.Trim();
                //_SearchRequest.PageNumber = p_PageNumber;
                _SearchRequest.PageNumber = p_PageNumber - 1;
                _SearchRequest.PageSize = _RawMediaPageCount;
                _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                if (!string.IsNullOrWhiteSpace(txtProgram.Text))
                {
                    _SearchRequest.Title120 = "\"" + txtProgram.Text + "\"";
                }

                _SearchRequest.Appearing = txtAppearing.Text;
                _SearchRequest.TimeZone = ddlTimeZone.SelectedItem.Text;

                List<string> _Market = new List<string>();
                List<string> _Station_Affil = new List<string>();
                List<string> _IQ_Class_Num = new List<string>();
                if (Convert.ToInt32(rdoMarket.SelectedValue) == 2 || _ViewstateInformation.IsAllDmaAllowed == false)
                {
                    foreach (DataListItem _DataListItem in rptMarket.Items)
                    {
                        HtmlInputCheckBox chkMarket = (HtmlInputCheckBox)_DataListItem.FindControl("chkMarket");
                        if (chkMarket.Checked)
                        {
                            _Market.Add(chkMarket.Value);
                        }
                    }
                }


                if (Convert.ToInt32(rdoAffil.SelectedValue) == 2 || _ViewstateInformation.IsAllStationAllowed == false)
                {
                    foreach (DataListItem _DataListItem in rptAffil.Items)
                    {
                        HtmlInputCheckBox chkAffil = (HtmlInputCheckBox)_DataListItem.FindControl("chkAffil");
                        if (chkAffil.Checked)
                        {
                            _Station_Affil.Add(chkAffil.Value);
                        }
                    }
                }


                if (Convert.ToInt32(rdoProgramType.SelectedValue) == 2 || _ViewstateInformation.IsAllClassAllowed == false)
                {
                    foreach (DataListItem _DataListItem in rptProgramType.Items)
                    {
                        HtmlInputCheckBox chkProgramType = (HtmlInputCheckBox)_DataListItem.FindControl("chkProgramType");
                        if (chkProgramType.Checked)
                        {
                            _IQ_Class_Num.Add(chkProgramType.Value);
                        }
                    }
                }

                _SearchRequest.StationAffil = _Station_Affil;
                _SearchRequest.IQDmaName = _Market;
                _SearchRequest.IQClassNum = _IQ_Class_Num;

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

                DateTime _FromDate = Convert.ToDateTime(txtFromDate.Text);
                DateTime _ToDate = Convert.ToDateTime(txtToDate.Text);

                _SearchRequest.StartDate = new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0);
                _SearchRequest.EndDate = new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0);

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

                SearchLog(_SearchRequest, "IQAdvance", _Responce);

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


                lblNoOfRawMedia.Text = _SearchResult.TotalHitCount.ToString();

                foreach (Hit _Hit in _SearchResult.Hits)
                {
                    RawMedia _RawMedia = new RawMedia();
                    _RawMedia.RawMediaID = new Guid(_Hit.Guid);
                    _RawMedia.Hits = _Hit.TotalNoOfOccurrence;
                    _RawMedia.IQ_Dma_Name = _Hit.Market;
                    _RawMedia.DateTime = new DateTime(_Hit.Timestamp.Year, _Hit.Timestamp.Month, _Hit.Timestamp.Day, (_Hit.Hour / 100), 0, 0);
                    _RawMedia.IQ_CC_Key = _Hit.Iqcckey;
                    _RawMedia.Affiliate = _Hit.Affiliate;
                    _RawMedia.Title120 = _Hit.Title120;

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/StationLogoImages/" + _Hit.StationId + ".gif")))
                    {
                        _RawMedia.StationLogo = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _Hit.StationId + ".gif";
                    }
                    else if (File.Exists(HttpContext.Current.Server.MapPath("~/StationLogoImages/" + _Hit.StationId + ".jpg")))
                    {
                        _RawMedia.StationLogo = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _Hit.StationId + ".jpg";
                    }

                    _ListOfRawMedia.Add(_RawMedia);
                }

                #region Pagination

                _ViewstateInformation.RawMediaTotalHitsCount = _SearchResult.TotalHitCount;
                _ViewstateInformation.CurrentRawMediaPage = p_PageNumber;

                SetViewstateInformation(_ViewstateInformation);

                #endregion Pagination

                if (_ViewstateInformation._IQAdvanceResponseListOfRawMedia != null && p_IsInitialization == false)
                {
                    _ViewstateInformation._IQAdvanceResponseListOfRawMedia.InsertRange(_ViewstateInformation._IQAdvanceResponseListOfRawMedia.Count, _ListOfRawMedia);
                }
                else
                {
                    _ViewstateInformation._IQAdvanceResponseListOfRawMedia = _ListOfRawMedia;
                }

                SetViewstateInformation(_ViewstateInformation);

            }
            catch (System.TimeoutException)
            {
                throw new MyException(CommonConstants.MsgBasicSearchUA);
            }
            catch (Exception)
            {
                throw new MyException(CommonConstants.MsgBasicSearchUA);
            }
        }

        #endregion
    }
}