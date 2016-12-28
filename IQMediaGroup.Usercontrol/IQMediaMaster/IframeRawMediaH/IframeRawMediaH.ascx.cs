using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Common;
using IQMediaGroup.Usercontrol.Base;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using System.Web.UI.HtmlControls;
using PMGSearch;
using System.IO;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IframeRawMediaH
{
    public partial class IframeRawMediaH : System.Web.UI.UserControl
    {
        public Guid? RawMediaID { get; set; }

        public bool IsUGC { get; set; }

        public int? Offset { get; set; }

        public string SearchTerm { get; set; }

        protected override void OnLoad(EventArgs e)
        {

        }

        public void InitializePlayer()
        {
            try
            {
                // lblMsg.Visible = false;
                //lblMsg.Text = string.Empty;
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                /* if (Request.QueryString["CC"] == null || Convert.ToBoolean(Request.QueryString["CC"]) != false)
                 {
                     tblClosedCaption.Visible = true;
                 }
                 else
                 {
                     tblClosedCaption.Visible = false;
                 }*/




                if (RawMediaID != null)
                {
                    divRawMedia.Visible = true;
                    // IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                    divRawMedia.Controls.Clear();

                    string baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];

                    if (HttpContext.Current.Request.ServerVariables["Http_Host"] == "mycliqmedia")
                    {
                        baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
                    }


                    /*string RawMediaObject = IQMediaPlayer.RenderRawMediaPlayer(string.Empty, Convert.ToString(RawMediaID), "true", Convert.ToString(IsUGC), _SessionInformation.ClientGUID, "false", _SessionInformation.CustomerGUID, baseURL, Offset, _SessionInformation.IsClientPlayerLogoActive, _SessionInformation.ClientPlayerLogoImage);

                    divRawMedia.InnerHtml = RawMediaObject;*/
                    // hfDivRawMedia.Value = RawMediaObject;

                    // time.Style.Add("display", "block");
                    // time.Visible = true;

                    DivCaption.InnerText = string.Empty;
                    DivCaption.Controls.Clear();

                    if (!string.IsNullOrEmpty(SearchTerm.Trim()))
                    {
                        Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
                        SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                        SearchRequest _SearchRequest = new SearchRequest();
                        _SearchRequest.GuidList = Convert.ToString(RawMediaID);
                        _SearchRequest.Terms = SearchTerm.Trim();
                        _SearchRequest.IsShowCC = true;

                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[CommonConstants.ConfigSolrQTIframe]))
                        {
                            _SearchRequest.SolrQT = ConfigurationManager.AppSettings[CommonConstants.ConfigSolrQTIframe];
                        }

                        int _PMGMaxHighlights = 20;

                        if (ConfigurationManager.AppSettings[CommonConstants.ConfigPMGMaxHighlights] != null)
                        {
                            int.TryParse(ConfigurationManager.AppSettings[CommonConstants.ConfigPMGMaxHighlights], out _PMGMaxHighlights);
                        }

                        _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                        SearchResult _SearchResult = _SearchEngine.Search(_SearchRequest);

                        if (!string.IsNullOrEmpty(_SearchResult.ResponseXml) && _SearchResult.Hits.Count > 0)
                        {
                            if (_SearchResult.Hits[0].TermOccurrences.Count > 0)
                            {
                                List<RawMediaCC> _ListOfRawMediaCC = new List<RawMediaCC>();
                                // ClosedCaption.Style.Add("display", "block");

                                foreach (TermOccurrence _TermOccurrence in _SearchResult.Hits[0].TermOccurrences)
                                {
                                    //IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                                    if (Offset == null)
                                    {
                                        if (_TermOccurrence.TimeOffset >= Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigRawMediaCaptionDelay].ToString()))
                                        {
                                            Offset = _TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigRawMediaCaptionDelay].ToString());
                                        }
                                        else
                                        {
                                            Offset = _TermOccurrence.TimeOffset;
                                        }
                                    }
                                    HtmlGenericControl _Div = new HtmlGenericControl();

                                    _Div.TagName = CommonConstants.HTMLDiv;
                                    _Div.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionMainDivClass);

                                    _Div.Attributes.Add(CommonConstants.HTMLOnClick, CommonConstants.CaptionSeekPointFunction + CommonConstants.BracketOpen + (_TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigRawMediaCaptionDelay].ToString())) + CommonConstants.BracketClose + CommonConstants.SemiColon);

                                    HtmlGenericControl _DivTime = new HtmlGenericControl();

                                    _DivTime.TagName = CommonConstants.HTMLDiv;
                                    _DivTime.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionDateTimeClass);

                                    int _Diff = Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigRawMediaCaptionDelay].ToString());

                                    _DivTime.InnerText = formatOffset(_TermOccurrence.TimeOffset);

                                    HtmlGenericControl _DivCaptionString = new HtmlGenericControl();

                                    _DivCaptionString.TagName = CommonConstants.HTMLDiv;
                                    _DivCaptionString.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionCaptionClass);

                                    _DivCaptionString.InnerHtml = _TermOccurrence.SurroundingText;

                                    _Div.Controls.Add(_DivTime);

                                    _Div.Controls.Add(_DivCaptionString);

                                    DivCaption.Controls.Add(_Div);
                                    //DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionActiveHeight);
                                }
                            }
                            else
                            {
                                DivCaption.InnerText = CommonConstants.NoResultsFound;
                                //DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);
                            }
                        }
                        else
                        {
                            DivCaption.InnerText = CommonConstants.NoResultsFound;
                            //DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);
                        }
                    }
                    else
                    {

                        DivCaption.InnerText = CommonConstants.NoResultsFound;
                        //DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);

                    }

                    string RawMediaObject = IQMediaPlayer.RenderRawMediaPlayer(string.Empty, Convert.ToString(RawMediaID), "true", Convert.ToString(IsUGC), _SessionInformation.ClientGUID, "false", _SessionInformation.CustomerGUID, baseURL, Offset, _SessionInformation.IsClientPlayerLogoActive, _SessionInformation.ClientPlayerLogoImage);

                    divRawMedia.InnerHtml = RawMediaObject;
                    // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadScript", "LoadPlayer();", true);
                }
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
            string str = ("" + h).PadLeft(2, '0') + ":" + ("" + m).PadLeft(2, '0') + ":" + ("" + offs).PadLeft(2, '0');
            return str;
        }
    }
}