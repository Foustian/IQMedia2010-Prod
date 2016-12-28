using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Usercontrol.Base;
using System.Configuration;
using IQMediaGroup.Controller.Common;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using PMGSearch;
using IQMediaGroup.Core.Enumeration;
using System.Web.UI.HtmlControls;
using System.Text;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IQAgentIframe
{
    public partial class IQAgentIframe : BaseControl
    {

        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        private int InitialOffset
        {
            get { return _initialOffset; }
            set { _initialOffset = value; }
        }int _initialOffset = 0;

        protected override void OnLoad(EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {

                    if (Request.QueryString["media"] != null)
                    {
                        UTF8Encoding encoding = new UTF8Encoding();
                        byte[] Key = encoding.GetBytes("43DD9199E882F08814E1864B45E4F117");
                        byte[] IV = encoding.GetBytes("40275DC0B57CD8D6");
                        string encryptedText = Request.QueryString["media"];
                        string decryptedText = CommonFunctions.DecryptStringFromBytes_Aes(encryptedText,Key,IV);

                        if (!string.IsNullOrWhiteSpace(decryptedText))
                        {
                            /*string[] data = decryptedText.Split('&');
                            if (data.Length > 0)
                            {
                                string rawMediaID = data[0];
                                DateTime clipDate = Convert.ToDateTime(data[1]);

                                if (clipDate > DateTime.Now)
                                {
                                    InitialClip(rawMediaID, "false", "");
                                }
                                else
                                {
                                    divRawMedia.Controls.Clear();
                                    divRawMedia.InnerHtml = GetMessageHtml("Invalid Stream");
                                    //divRawMedia.Style.Add("height", "20px");
                                    divRawMedia.Visible = true;
                                }
                            }*/

                            IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();
                            List<IQAgentIFrame> lstIQAgentIFrame = _IRawMediaController.GetIqagentIframeData_ByiQAgentiFrameID(new Guid(decryptedText));

                            if (lstIQAgentIFrame != null && lstIQAgentIFrame.Count > 0)
                            {
                                DateTime clipDate = lstIQAgentIFrame[0].Expiry_Date;
                                if (clipDate > DateTime.Now)
                                {
                                    if (!string.IsNullOrWhiteSpace(lstIQAgentIFrame[0].SearchTerm))
                                    {
                                        GetClosedCaption(lstIQAgentIFrame[0].RawMediaGuid, lstIQAgentIFrame[0].SearchTerm);
                                    }
                                    InitialClip(HttpUtility.UrlEncode(encryptedText), ConfigurationManager.AppSettings["IQAgentPlayerAutoPlayBack"], "");
                                }
                                else
                                {
                                    divRawMedia.Controls.Clear();
                                    divRawMedia.InnerHtml = GetMessageHtml("Invalid Stream");
                                    //divRawMedia.Style.Add("height", "20px");
                                    divRawMedia.Visible = true;
        
                                    DivCaption.InnerText = string.Empty;
                                    DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);
                                }
                            }
                        }
                        else
                        {
                            divRawMedia.InnerHtml = GetMessageHtml("You are not authorized to view this page");
                            //divRawMedia.Style.Add("height", "20px");
                            divRawMedia.Visible = true;

                            DivCaption.InnerText = string.Empty;
                            DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);
                        }
                    }
                    else
                    {
                        divRawMedia.InnerHtml = GetMessageHtml("You are not authorized to view this page");
                        //divRawMedia.Style.Add("height", "20px");
                        divRawMedia.Visible = true;

                        DivCaption.InnerText = string.Empty;
                        DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);
                    }

                }

            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch (Exception _Exception)
            {
                divRawMedia.InnerHtml = GetMessageHtml("An error occured, please try again.");
                //divRawMedia.Style.Add("height", "20px");
                divRawMedia.Visible = true;

                DivCaption.InnerText = string.Empty;
                DivCaption.Style.Add(CommonConstants.HTMLHeight, CommonConstants.CaptionDeActiveHeight);

                this.WriteException(_Exception);

            }
        }

        private void InitialClip(string _clipID, string _playback, string _toEmail)
        {
            try
            {
                divRawMedia.Visible = true;
                IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();



                string baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];

                divRawMedia.Controls.Clear();

                /*if (Request.UserAgent.ToLower().Contains("android") && CheckVersion())
                {
                    //divRawMedia.InnerHtml = "<video width=\"545px\" height=\"340px\" tabindex=\"0\" id=\"vidClip\" controls=\"controls\">"
                    //                            + "<source src=\"\" type=\"application/x-mpegURL\"></source> "
                    //                             + "</video>";

                    //string handlerScript = " function CallHandler() { alert('handler called'); $.ajax({"
                    //                        + "type: 'GET',"
                    //                        + "dataType: 'jsonp',"
                    //                         + "url: \"" + ConfigurationManager.AppSettings[CommonConstants.ConfigAndroidGetvars] + "" + _clipID + "&IsAndroid=true\","
                    //                        + "success: OnComplete,"
                    //                        + "error: OnFail});} "

                    //                        + " function OnComplete(result, textStatus, jqXHR) {"
                    //                        + "alert('Complete');"
                    //                         + "$.each(result, function () {"
                    //                        + "if (this.IsValidMedia.toString() == \"true\") {"
                    //    //+ " $('#ClipPlayerControl_divRawMedia').html = $('#vidClip').find('source').attr('src', this.Media+'.m3u8');} });}"
                    //                            + " document.getElementById('ClipPlayerControl_divRawMedia').innerHTML = '<video width=\"545px\" height=\"340px\" tabindex=\"0\" controls=\"controls\" autoplay=\"autoplay\" id=\"vidClip\"><source src=\"' + this.Media + '.m3u8\" type=\"application/x-mpegURL\"></source></video> <div style=\"position: absolute; top: 43%; left: 43%;\" onclick=\"playVideo();\" id=\"divplaybtn\"><img id=\"Img2\" runat=\"server\" images=\"~/images/play1.png\"  /></div>';} });}"
                    //    //document.getElementById('divtest').innerHTML = '<video width="545px" height="340px" tabindex="0" id="vidClip" controls="controls"><source src="' + this.Media + '" type="application/x-mpegURL"></source></video>';
                    //                            + " function OnFail(jqXHR, textStatus, errorThrown) {alert('" + ConfigurationManager.AppSettings[CommonConstants.ConfigHTML5PlayerError] + "');}   function playVideo() { document.getElementById('divplaybtn').style.display = 'none'; var myVideo = document.getElementById(\"vidClip\"); myVideo.play();} CallHandler();";


                    this.Page.ClientScript.RegisterClientScriptInclude("script5", this.Page.ResolveClientUrl("~/Script/") + "AndroidPlayer.js");

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AndroidVideo", "LoadHTML5Player('" + _clipID + "');", true);


                }
                else
                {*/


                /*if (Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod"))
                {
                    this.Page.ClientScript.RegisterClientScriptInclude("iosscript", this.Page.ResolveClientUrl("~/Script/") + "iOSPlayer.js");
                    if (ConfigurationManager.AppSettings["MyCliqMediaHost"].Contains(Context.Request.Url.Host))
                    {
                        baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
                    }
                    else
                    {
                        baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];
                    }

                    string scriptForIPad = "CheckForIOS('" + _clipID + "','" + baseURL + "','" + ConfigurationManager.AppSettings["IOSAppURL"] + "');";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "IOSCheck", scriptForIPad.ToString(), true);
                }*/
                if (Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod"))
                {
                    /*HtmlAnchor IOSAnchor = new HtmlAnchor();
                    IOSAnchor.Attributes.Add("href", "itms-services://?action=download-manifest&amp;url=" + ConfigurationManager.AppSettings["IOSAppURL"] + "");
                    IOSAnchor.InnerHtml = "<span style=\"font-size:50px;\">Tap Here to install latest IQMedia Application</span>";
                    iosDiv.Controls.Add(IOSAnchor);

                    iosDiv.Visible = true;*/

                    if (ConfigurationManager.AppSettings["MyCliqMediaHost"].Contains(Context.Request.Url.Host))
                    {
                        baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
                    }
                    else
                    {
                        baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];
                    }

                    string scriptForIPad = "function CheckForIOS(){ " +
                                          "document.location = 'iqmedia://clipid=" + _clipID + "&baseurl=" + baseURL + "';" +
                                          "var time = (new Date()).getTime();" +
                                          "setTimeout(function(){var now = (new Date()).getTime();" +
                                          "if((now-time)<1500) {" +
                                          "document.location='" + ConfigurationManager.AppSettings["IOSAppURL"] + "'}},1000);" +
                                          "} CheckForIOS();";

                    /*string scriptForIPad = "function CheckForIOS(){ " +
                                          "document.location = 'iqmedia://clipid=" + Request.QueryString["ClipID"] + "&baseurl=" + baseURL + "';" +                                             
                                          "setTimeout(function(){ " +
                                          "if (isRedirected == false) {" +
                                          "document.location='" + ConfigurationManager.AppSettings["IOSAppURL"] + "'}},500);" +
                                          "} CheckForIOS();";*/

                    /*string scriptForIPad = "function CheckForIOS() {" +
                                           "document.location = 'iqmedia://clipid=" + Request.QueryString["ClipID"] + "&baseurl=" + baseURL + "';" +
                                            "setTimeout(function () { if (isRedirected == false) { document.location = 'http://qa.iqmediacorp.com/iosapp'  } }, 500);}";*/

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "IOSCheck", scriptForIPad.ToString(), true);

                }
                else
                {

                    divRawMedia.InnerHtml = IQMediaPlayer.RenderIQAgentPlayer(_clipID, baseURL, _playback,_initialOffset);
                }
                /*}*/
                //PlayVideo(_clipID, "false");




                //_ViewstateInformation.VSTimetick = 0;
                this.SetViewstateInformation(_ViewstateInformation);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        private string GetMessageHtml(String messageString)
        {
            try
            {
                return "<div class=\"expiredLink\"><div class=\"paddingTop30p\">" + messageString + "</div></div>";
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                return "<div class=\"expiredLink\"><div class=\"paddingTop30p\">An error occured, please try again.</div></div>";
            }
        }

        public void GetClosedCaption(Guid rawMediaGuid, string searchTerm)
        {
            Uri PMGSearchRequestUrl = new Uri(ConfigurationManager.AppSettings["PMGSearchUrl"].ToString());
            SearchEngine _SearchEngine = new SearchEngine(PMGSearchRequestUrl);

            SearchRequest _SearchRequest = new SearchRequest();
            _SearchRequest.GuidList = Convert.ToString(rawMediaGuid);
            _SearchRequest.Terms = searchTerm;
            _SearchRequest.IsShowCC = true;



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
                    _initialOffset = _SearchResult.Hits[0].TermOccurrences.OrderBy(a => a.TimeOffset).FirstOrDefault().TimeOffset;

                    if (_initialOffset != null)
                    {
                        if (_initialOffset - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]) >= 0)
                        {
                            _initialOffset = _initialOffset - Convert.ToInt32(ConfigurationManager.AppSettings["PlayerDefaultOffset"]);
                        }
                        else
                        {
                            _initialOffset = 0;
                        }
                    }

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