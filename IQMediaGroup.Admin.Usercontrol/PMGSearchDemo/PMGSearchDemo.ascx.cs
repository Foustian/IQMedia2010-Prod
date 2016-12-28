using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Usercontrol.Base;
using PMGSearch;
using PMGSearch.debug;
using IQMediaGroup.Core.Enumeration;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Configuration;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Common;

namespace IQMediaGroup.Admin.Usercontrol.PMGSearchDemo
{
    public partial class PMGSearchDemo : BaseControl
    {
        ControllerFactory _ControllerFactory = new ControllerFactory();

        public string _clipID = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            #region Set Bread Crumb
            
            GenerateBreadCrumb("Demo > PMG Search");

            #endregion
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchGrid();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void BindSearchGrid()
        {

            //result.Hits[0].TermOccurrences.Count //will give you thetime a search term was found within the text of 1 hour of video
            //find out number oof pages:  result.hits/Page Size
            //result.TotalHitCount/req.PageSize = number of pages

            //totalhitcount= result.TotalHitCount at top

            List<RawMedia> _ListOfRawMedia = GetRawMediaResults(0);
            gvSearchResult.DataSource = _ListOfRawMedia;
            gvSearchResult.DataBind();
            if (_ListOfRawMedia.Count > 0)
            {
                divPager.Style.Add("display", "block");
            }

            UpdateUpdatePanel(updSearchGrid);
        }

        private List<RawMedia> GetRawMediaResults(int p_PageNumber)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                string PMGSearchRequestUrl = ConfigurationSettings.AppSettings["PMGSearchUrl"].ToString();
                SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                int _RawMediaPageCount = 10;

                if (ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaPageSize] != null && int.TryParse(ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaPageSize], out _RawMediaPageCount))
                {
                    //_RawMediaPageCount = Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaPageSize].ToString());
                }

                int _PMGMaxHighlights = 20;


                if (ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGMaxHighlights] != null && int.TryParse(ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGMaxHighlights], out _PMGMaxHighlights))
                {
                    _PMGMaxHighlights = Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGMaxHighlights].ToString());
                }

                SearchRequest _SearchRequest = new SearchRequest();
                _SearchRequest.Terms = txtSearchTerm.Text.Trim();
                _SearchRequest.PageNumber = p_PageNumber + 1;
                _SearchRequest.PageSize = _RawMediaPageCount;
                _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
                {
                    _SearchRequest.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                }
                if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                {
                    _SearchRequest.EndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                }

                if (_ViewstateInformation != null && !string.IsNullOrEmpty(_ViewstateInformation.SortExpression))
                {
                    _SearchRequest.SortFields = _ViewstateInformation.SortExpression;
                    switch (_ViewstateInformation.SortExpression.ToLower())
                    {
                        case "market":
                            if (_ViewstateInformation.SortingColumn_Market != "DESC")
                            {
                                _SearchRequest.SortFields += "-";
                            }
                            break;
                        case "affiliate":
                            if (_ViewstateInformation.SortingColumn_Affiliate != "DESC")
                            {
                                _SearchRequest.SortFields += "-";
                            }
                            break;
                        case "date,hour":
                            if (_ViewstateInformation.SortingColumn_DateTime != "DESC")
                            {
                                _SearchRequest.SortFields = "date-,hour-";
                            }
                            break;
                    }
                }


                SearchResult _SearchResult = _SearchEngine.search(_SearchRequest);

                List<RawMedia> _ListOfRawMedia = new List<RawMedia>();

                bool _IsPmgSearchTotalHitsFromConfig = true;
                int _MaxPMGHitsCount = 100;

                if (ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGSearchTotalHitsFromConfig] != null && bool.TryParse(ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGSearchTotalHitsFromConfig], out _IsPmgSearchTotalHitsFromConfig))
                {
                    //_IsPmgSearchTotalHitsFromConfig = Convert.ToBoolean(ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGSearchTotalHitsFromConfig].ToString());
                }

                if (_IsPmgSearchTotalHitsFromConfig == true)
                {
                    if (ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGMaxListCount] != null && int.TryParse(ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGMaxListCount], out _MaxPMGHitsCount))
                    {
                        //_MaxPMGHitsCount = Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ConfigPMGMaxListCount].ToString());
                    }

                    _SearchResult.TotalHitCount = _MaxPMGHitsCount;
                }


                lblTotalHourOfVideo.Text = _SearchResult.TotalHitCount.ToString();

                divTotalResultCount.Style.Add("display", "block");

                foreach (Hit _Hit in _SearchResult.Hits)
                {
                    RawMedia _RawMedia = new RawMedia();
                    _RawMedia.RawMediaID = new Guid(_Hit.GUID);
                    _RawMedia.StationID = _Hit.StationID;
                    _RawMedia.Market = _Hit.market;
                    _RawMedia.Affiliate = _Hit.affiliate;
                    _RawMedia.Hits = _Hit.TermOccurrences.Count;
                    _RawMedia.DateTime = new DateTime(_Hit.TimeStamp.Year, _Hit.TimeStamp.Month, _Hit.TimeStamp.Day, (_Hit.Hour / 100), 0, 0);

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

        private string formatOffset(int offs)
        {
            int h = 0;
            int m = 0;
            offs -= ((h = offs / 3600) * 3600);
            offs -= ((m = offs / 60) * 60);
            return ("" + h).PadLeft(2, '0') + ":" + ("" + m).PadLeft(2, '0') + ":" + ("" + offs).PadLeft(2, '0');
        }

        protected void gvSearchResult_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "PlayVideo")
                {
                    try
                    {
                        time.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), CommonConstants.JSFunctionClipKey, CommonConstants.JSFunctionClipActive, true);
                        string VideoGUID = e.CommandArgument.ToString();
                        string CloseCaption = string.Empty;
                        SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                        List<RawMedia> _ListOfRawMedia = _SessionInformation.ListOfRawMedia;
                        if (_ListOfRawMedia != null)
                        {
                            RawMedia _RawMedia = _ListOfRawMedia.Find(delegate(RawMedia p_RawMedia) { return p_RawMedia.RawMediaID == new Guid(e.CommandArgument.ToString()); });
                            if (_RawMedia != null)
                            {
                                InitialClip(VideoGUID, true);
                                AddCaption(_RawMedia.RawMediaCloseCaptions);
                            }
                        }


                    }
                    catch (Exception _Exception)
                    {
                        this.WriteException(_Exception);
                        Response.Redirect(CommonConstants.CustomErrorPage);
                    }
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvSearchResult_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.SortExpression = e.SortExpression;
                SetViewstateInformation(_ViewstateInformation);

                switch (e.SortExpression.ToLower())
                {
                    case "market":
                        _ViewstateInformation.SortingColumn_Market = _ViewstateInformation.SortingColumn_Market == "ASC" ? "DESC" : "ASC";
                        break;
                    case "affiliate":
                        _ViewstateInformation.SortingColumn_Affiliate = _ViewstateInformation.SortingColumn_Affiliate == "ASC" ? "DESC" : "ASC";
                        break;
                    case "date,hour":
                        _ViewstateInformation.SortingColumn_DateTime = _ViewstateInformation.SortingColumn_DateTime == "ASC" ? "DESC" : "ASC";
                        break;
                    default:
                        break;
                }

                List<RawMedia> _ListOfRawMedia = GetRawMediaResults(0);
                gvSearchResult.DataSource = _ListOfRawMedia;
                gvSearchResult.DataBind();
                UpdateUpdatePanel(updSearchGrid);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void InitialClip(string _clipID, bool _playback)
        {
            try
            {
                IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();
                divRawMedia.Controls.Clear();
                //divRawMedia.Controls.Add(RedlassoPlayer.RenderRawMediaPlayer(_clipID));
                IsVisibleCaption(true);
                UpdateUpdatePanel(upVideo);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void AddCaption(List<RawMediaCC> _ListOfRawMediaCC)
        {
            try
            {
                DivCaption.Controls.Clear();

                foreach (RawMediaCC _RawMediaCC in _ListOfRawMediaCC)
                {

                    IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                    HtmlGenericControl _Div = new HtmlGenericControl();

                    _Div.TagName = CommonConstants.HTMLDiv;
                    _Div.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionMainDivClass);

                    _Div.Attributes.Add(CommonConstants.HTMLOnClick, CommonConstants.CaptionSeekPointFunction + CommonConstants.BracketOpen + (_RawMediaCC.TimeOffset - Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaCaptionDelay].ToString())) + CommonConstants.BracketClose + CommonConstants.SemiColon);

                    HtmlGenericControl _DivTime = new HtmlGenericControl();

                    _DivTime.TagName = CommonConstants.HTMLDiv;
                    _DivTime.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionDateTimeClass);

                    int _Diff = Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaCaptionDelay].ToString());

                    _DivTime.InnerHtml = formatOffset(_RawMediaCC.TimeOffset);

                    HtmlGenericControl _DivCaptionString = new HtmlGenericControl();

                    _DivCaptionString.TagName = CommonConstants.HTMLDiv;
                    _DivCaptionString.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionCaptionClass);

                    
                    _DivCaptionString.InnerHtml = _RawMediaCC.SurroundingText;
                    


                    _Div.Controls.Add(_DivTime);

                    _Div.Controls.Add(_DivCaptionString);

                    DivCaption.Controls.Add(_Div);
                    DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionActiveHeight);
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void IsVisibleCaption(bool p_Visible)
        {
            DivCaption.Visible = p_Visible;
        }

        protected void imgRawMediaNext_Click(object sender, EventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                List<RawMedia> _ListOfRawMedia = GetRawMediaResults((_ViewstateInformation.CurrentRawMediaPage + 1));
                gvSearchResult.DataSource = _ListOfRawMedia;
                gvSearchResult.DataBind();
                UpdateUpdatePanel(updSearchGrid);
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
                gvSearchResult.DataSource = _ListOfRawMedia;
                gvSearchResult.DataBind();
                UpdateUpdatePanel(updSearchGrid);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}