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
using System.IO;
using PMGSearch;
using IQMediaGroup.Core.Enumeration;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IRawMedia
{
    public partial class IRawMedia : BaseControl
    {
        ControllerFactory _ControllerFactory = new ControllerFactory();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SessionInformationIframe _SessionInformationIframe = CommonFunctions.GetSessionInformationIframe();                

                if (IQMedia.Web.Common.Authentication.IsAuthenticated && ((_SessionInformationIframe != null && _SessionInformationIframe.ClientGUID != null && _SessionInformationIframe.CustomerGUID != null) || IQMedia.Web.Common.Authentication.CurrentUser != null))
                {
                    _SessionInformationIframe.IsRedirect = null;    

                    if (_SessionInformationIframe.ClientGUID == null || _SessionInformationIframe.CustomerGUID == null)
                    {
                        var _User = IQMedia.Web.Common.Authentication.CurrentUser;

                        if (_User != null)
                        {
                            IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();

                            List<Client> _ListOfClient = _IClientController.GetClientGUIDByCustomerGUID(_User.Guid);
                            if (_ListOfClient.Count > 0)
                            {
                                _SessionInformationIframe.ClientGUID = _ListOfClient[0].ClientGUID;
                                //_SessionInformationIframe.PlayerLogoImage = _ListOfClient[0].PlayerLogoPath;
                                //_SessionInformationIframe.IsActivePlayerLogo = _ListOfClient[0].IsActivePlayerLogo;
                                _SessionInformationIframe.CustomerGUID = _User.Guid;
                            }
                            CommonFunctions.SetSessionInformationIframe(_SessionInformationIframe);
                        }
                    }

                    InitializePlayer();

                    trPlayer.Visible = true;
                    trLogin.Visible = false;
                }
                else
                {
                    if (Request.QueryString["AU"] != null && (_SessionInformationIframe.IsRedirect==null || _SessionInformationIframe.IsRedirect==false))
                    {
                        _SessionInformationIframe.IsRedirect = true;

                        CommonFunctions.SetSessionInformationIframe(_SessionInformationIframe);

                        HttpCookie _HttpCookie = new HttpCookie(".IQAUTH");
                        _HttpCookie.Value = Convert.ToString(Request.QueryString["AU"]).Substring(5);
                        _HttpCookie.Domain = ".iqmediacorp.com";

                        Response.Cookies.Add(_HttpCookie);
                        Response.Redirect(Request.Url.AbsoluteUri, true);
                    }
                    else
                    {
                        trPlayer.Visible = false;
                        trLogin.Visible = true;
                    }
                }
            }
            catch (Exception _Exception)
            {

                lblMsg.Text = "Some error occurs, please try again";
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;

                trPlayer.Visible = false;
            }
        }

        private void InitializePlayer()
        {
            try
            {
                lblMsg.Visible = false;
                lblMsg.Text = string.Empty;

                if (Request.QueryString["RawMediaID"] != null && CommonFunctions.GetGUIDValue(Convert.ToString(Request.QueryString["RawMediaID"])) != null)
                {
                    int? _Offset = null;

                    if (Request.QueryString["Offset"] != null)
                    {
                        _Offset = CommonFunctions.GetIntValue(Convert.ToString(Request.QueryString["Offset"]));
                    }

                    divRawMedia.Visible = true;
                    // IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                    SessionInformationIframe _SessionInformationIframe = CommonFunctions.GetSessionInformationIframe();

                    divRawMedia.Controls.Clear();

                    string baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];

                    if (HttpContext.Current.Request.ServerVariables["Http_Host"]=="mycliqmedia")
                    {
                        baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
                    }

                    if (string.IsNullOrEmpty(Request.QueryString["KeyValues"]))
                    {
                        string RawMediaObject = IQMediaPlayer.RenderRawMediaPlayer(string.Empty, Convert.ToString(Request.QueryString["RawMediaID"]), "true", "false", Convert.ToString(_SessionInformationIframe.ClientGUID), "true", Convert.ToString(_SessionInformationIframe.CustomerGUID), baseURL, _Offset, _SessionInformationIframe.IsActivePlayerLogo, _SessionInformationIframe.PlayerLogoImage);
                        divRawMedia.InnerHtml = RawMediaObject;
                        hfDivRawMedia.Value = RawMediaObject;
                    }
                    else
                    {
                        string RawMediaObject = IQMediaPlayer.RenderRawMediaPlayer(string.Empty, Convert.ToString(Request.QueryString["RawMediaID"]), "true", "false", Convert.ToString(_SessionInformationIframe.ClientGUID), "true", Convert.ToString(_SessionInformationIframe.CustomerGUID), baseURL, _Offset, Request.QueryString["KeyValues"], _SessionInformationIframe.IsActivePlayerLogo, _SessionInformationIframe.PlayerLogoImage);
                        divRawMedia.InnerHtml = RawMediaObject;
                        hfDivRawMedia.Value = RawMediaObject;
                    } 

                    time.Style.Add("display", "block");
                    time.Visible = true;

                    DivCaption.InnerText = string.Empty;
                    DivCaption.Controls.Clear();

                    if (Request.QueryString["SearchTerm"] != null)
                    {
                        Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
                        SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

                        SearchRequest _SearchRequest = new SearchRequest();
                        _SearchRequest.GuidList = Convert.ToString(Request.QueryString["RawMediaID"]);
                        _SearchRequest.IsShowCC = true;
                        _SearchRequest.Terms = Convert.ToString(Request.QueryString["SearchTerm"]).Trim();
                        //_SearchRequest.Appearing = !string.IsNullOrEmpty(Request.QueryString["Appearing"]) ? Request.QueryString["Appearing"] : null;

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
                                ClosedCaption.Style.Add("display", "block");

                                foreach (TermOccurrence _TermOccurrence in _SearchResult.Hits[0].TermOccurrences)
                                {
                                    IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                                    HtmlGenericControl _Div = new HtmlGenericControl();

                                    _Div.TagName = CommonConstants.HTMLDiv;
                                    _Div.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionMainDivClass);

                                    _Div.Attributes.Add(CommonConstants.HTMLOnClick, CommonConstants.CaptionSeekPointFunction + CommonConstants.BracketOpen + (_TermOccurrence.TimeOffset - Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaCaptionDelay].ToString())) + CommonConstants.BracketClose + CommonConstants.SemiColon);

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
                                    DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionActiveHeight);
                                }
                            }
                            else
                            {
                                DivCaption.InnerText = CommonConstants.NoResultsFound;
                                DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);
                            }
                        }
                        else
                        {
                            DivCaption.InnerText = CommonConstants.NoResultsFound;
                            DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);
                        }
                    }
                    else
                    {

                        DivCaption.InnerText = CommonConstants.NoResultsFound;
                        DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);

                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadScript", "LoadPlayer();", true);
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

        protected void ucLoginIframe_LoggedIn(object sender, EventArgs e)
        {
            try
            {
                SessionInformationIframe _SessionInformationIframe = CommonFunctions.GetSessionInformationIframe();

                if ((_SessionInformationIframe != null && _SessionInformationIframe.CustomerGUID != null && _SessionInformationIframe.ClientGUID != null))
                {
                    trPlayer.Visible = true;
                    trLogin.Visible = false;

                    InitializePlayer();
                }
            }
            catch (Exception _Exception)
            {
                throw;
            }
        }
    }
}