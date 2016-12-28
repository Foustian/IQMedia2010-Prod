using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Common;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace IQMediaGroup.WebApplication.ClipPlayer
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClipPlayerControl.Visible = false;
                if (Request.QueryString["ClipID"] != null && Request.QueryString["PN"] != null)
                {
                    IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>(); ;
                    string ClientHeaderImage = _IClientController.GetCustomHeaderByClipGuid(new Guid(Request.QueryString["ClipID"]));
                    if (!string.IsNullOrWhiteSpace(ClientHeaderImage))
                    {
                        imgLogo.Src = ConfigurationManager.AppSettings["URLCustomHeader"] + ClientHeaderImage;
                    }
                    #region OldLogic
                    //if (Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod"))
                    //{
                    //    #region Old Logic
                    //    /*clipDiv.Visible = false;
                    //    Page.Controls.Remove(ClipPlayerControl);                        
                    //    string baseURL = string.Empty;
                    //    if (ConfigurationManager.AppSettings["MyCliqMediaHost"].Contains(Context.Request.Url.Host))
                    //    {
                    //        baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
                    //    }
                    //    else
                    //    {
                    //        baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];
                    //    }

                    //    string scriptForIPad = "function CheckForIOS(){ " +
                    //                          "document.location = 'iqmedia://clipid=" + Request.QueryString["ClipID"] + "&baseurl=" + baseURL + "';" +
                    //                          "var time = (new Date()).getTime();" +
                    //                          "setTimeout(function(){var now = (new Date()).getTime();" +
                    //                          "if((now-time)<1500) {" +
                    //                          "document.location='" + ConfigurationManager.AppSettings["IOSAppURL"] + "'}},1000);" +
                    //                          "} CheckForIOS();";

                    //    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "IOSCheck", scriptForIPad.ToString(), true);*/

                    //    #endregion

                    //    clipDiv.Visible = false;
                    //    Page.Controls.Remove(ClipPlayerControl);

                    //    this.Page.ClientScript.RegisterClientScriptInclude("script5", this.Page.ResolveClientUrl("~/Script/") + "AndroidPlayer.js");
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AndroidVideo", "LoadHTML5Player('" + Request.QueryString["ClipID"] + "');", true);

                    //}
                    //else if (Request.UserAgent.ToLower().Contains("android") && CheckVersion())
                    //{
                    //    clipDiv.Visible = false;
                    //    Page.Controls.Remove(ClipPlayerControl);
                    //    //divRawMedia.InnerHtml = "<video width=\"545px\" height=\"340px\" tabindex=\"0\" id=\"vidClip\" controls=\"controls\">"
                    //    //                            + "<source src=\"\" type=\"application/x-mpegURL\"></source> "
                    //    //                             + "</video>";

                    //    //string handlerScript = " function CallHandler() { alert('handler called'); $.ajax({"
                    //    //                        + "type: 'GET',"
                    //    //                        + "dataType: 'jsonp',"
                    //    //                         + "url: \"" + ConfigurationManager.AppSettings[CommonConstants.ConfigAndroidGetvars] + "" + _clipID + "&IsAndroid=true\","
                    //    //                        + "success: OnComplete,"
                    //    //                        + "error: OnFail});} "

                    //    //                        + " function OnComplete(result, textStatus, jqXHR) {"
                    //    //                        + "alert('Complete');"
                    //    //                         + "$.each(result, function () {"
                    //    //                        + "if (this.IsValidMedia.toString() == \"true\") {"
                    //    //    //+ " $('#ClipPlayerControl_divRawMedia').html = $('#vidClip').find('source').attr('src', this.Media+'.m3u8');} });}"
                    //    //                            + " document.getElementById('ClipPlayerControl_divRawMedia').innerHTML = '<video width=\"545px\" height=\"340px\" tabindex=\"0\" controls=\"controls\" autoplay=\"autoplay\" id=\"vidClip\"><source src=\"' + this.Media + '.m3u8\" type=\"application/x-mpegURL\"></source></video> <div style=\"position: absolute; top: 43%; left: 43%;\" onclick=\"playVideo();\" id=\"divplaybtn\"><img id=\"Img2\" runat=\"server\" images=\"~/images/play1.png\"  /></div>';} });}"
                    //    //    //document.getElementById('divtest').innerHTML = '<video width="545px" height="340px" tabindex="0" id="vidClip" controls="controls"><source src="' + this.Media + '" type="application/x-mpegURL"></source></video>';
                    //    //                            + " function OnFail(jqXHR, textStatus, errorThrown) {alert('" + ConfigurationManager.AppSettings[CommonConstants.ConfigHTML5PlayerError] + "');}   function playVideo() { document.getElementById('divplaybtn').style.display = 'none'; var myVideo = document.getElementById(\"vidClip\"); myVideo.play();} CallHandler();";


                    //    this.Page.ClientScript.RegisterClientScriptInclude("script5", this.Page.ResolveClientUrl("~/Script/") + "AndroidPlayer.js");

                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "AndroidVideo", "LoadHTML5Player('" + Request.QueryString["ClipID"] + "');", true);

                    //}
                    #endregion

                    Logger.Info("User Agent : " + Request.UserAgent);

                    if ((Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod")) || (Request.UserAgent.ToLower().Contains("android") && CheckVersion()))
                    {
                        try
                        {
                            ClipPlayerControl.Visible = false;
                            string url = string.Format(ConfigurationManager.AppSettings["AndroidGetvars"], Request.QueryString["ClipID"]);

                            string respone = DoHttpGetRequest(url);

                            Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                            jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(respone.Replace(@"\", string.Empty).Replace("([", string.Empty).Replace("])", string.Empty).Replace("\"", "'"));
                            if (Convert.ToBoolean(Convert.ToString(jsonData["IsValidMedia"])))
                            {
                                string media = Convert.ToString(jsonData["Media"]);
                                Response.Redirect(media + ".m3u8",false);
                            }
                            else
                            {
                                lblErrorMsg.Text = "Invalid Media";
                            }
                        }
                        catch (Exception _Exception)
                        {
                            IQMediaGroup.Core.HelperClasses.IQMediaGroupExceptions _IQMediaGroupExceptions = new IQMediaGroup.Core.HelperClasses.IQMediaGroupExceptions();
                            _IQMediaGroupExceptions.ExceptionStackTrace = " Stack Trace : " + _Exception.StackTrace;
                            _IQMediaGroupExceptions.ExceptionMessage = _Exception.ToString();
                            _IQMediaGroupExceptions.CreatedBy = "Base - Write Exception";
                            _IQMediaGroupExceptions.ModifiedBy = "Base - Write Exception";

                            string _ReturnValue = string.Empty;
                            IIQMediaGroupExceptionsController _IIQMediaGroupExceptionsController = _ControllerFactory.CreateObject<IIQMediaGroupExceptionsController>();
                            _ReturnValue = _IIQMediaGroupExceptionsController.AddIQMediaGroupException(_IQMediaGroupExceptions);

                            lblErrorMsg.Text = "An Error Occured, please try again.";
                        }
                    }
                    else
                    {
                        ClipPlayerControl.Visible = true;
                        clipDiv.Visible = true;
                        iosDiv.Visible = false;

                        string _ImagePath = string.Empty;
                        string _VidepPath = string.Empty;

                        List<ArchiveClip> _ListOfArchiveClip = new List<ArchiveClip>();
                        ArchiveClip _ArchiveClip = new ArchiveClip();
                        _ArchiveClip.ClipID = new Guid(Request.QueryString["ClipID"]);
                        IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                        _ListOfArchiveClip = _IArchiveClipController.GetArchiveClipByClipID(_ArchiveClip);

                        if (_ListOfArchiveClip.Count > 0)
                        {

                            ArchiveClip _ArchiveClipTemp = _ListOfArchiveClip[0];




                            string Name = HttpContext.Current.Request.Url.ToString();

                            string[] _DomainName = Name.Split("/".ToCharArray());

                            string _FinalString = _DomainName[0] + "//" + _DomainName[2];


                            _ImagePath = Convert.ToString(ConfigurationManager.AppSettings["ClipGetPreview"]) + "&eid=" + Request.QueryString["ClipID"];

                            //if (!string.IsNullOrEmpty(_ArchiveClipTemp.ThumbnailImagePath))
                            //{
                            //    _ImagePath = _ArchiveClipTemp.ThumbnailImagePath;
                            //}
                            //else
                            //{
                            //    _ImagePath = _FinalString + "/ThumbnailImage/noimage.jpg";


                            //}

                            //_VidepPath = _FinalString + ConfigurationSettings.AppSettings["ClipParamMovie"] + "?IsRawMedia=false&PageName=" + Request.QueryString["PN"] + "&ToEmail=" + Request.QueryString["TE"] + "&embedId=" + Request.QueryString["ClipID"] + "&ServicesBaseURL=" + ConfigurationManager.AppSettings["ServicesBaseURL"] + "&PlayerFromLocal=" + ConfigurationSettings.AppSettings["PlayerFromLocal"] + "&EB=false&autoPlayback=true";
                            _VidepPath = ConfigurationSettings.AppSettings["PlayerLocation"] + "?IsRawMedia=false&PageName=" + Request.QueryString["PN"] + "&embedId=" + Request.QueryString["ClipID"] + "&ServicesBaseURL=" + ConfigurationManager.AppSettings["ServicesBaseURL"] + "&PlayerFromLocal=" + ConfigurationSettings.AppSettings["PlayerFromLocal"] + "&EB=false&autoPlayback=true";


                            /*relImage.Attributes.Add("href", "http://qa.iqmediacorp.com/ThumbnailImage/0a75acaa-87eb-4cb1-84c0-c5066fc4bab2.jpg");*/
                            HtmlLink link = new HtmlLink();
                            link.Attributes.Add("href", _ImagePath);
                            link.Attributes.Add("type", "image/jpeg");
                            link.Attributes.Add("rel", "image_src");

                            HtmlLink Videoimage = new HtmlLink();
                            Videoimage.Attributes.Add("href", _VidepPath);
                            Videoimage.Attributes.Add("type", "image/jpeg");
                            Videoimage.Attributes.Add("rel", "video_src");


                            HtmlMeta Title = new HtmlMeta();
                            Title.Attributes.Add("content", _ArchiveClipTemp.ClipTitle);
                            Title.Attributes.Add("property", "og:title");


                            HtmlMeta VideoType = new HtmlMeta();
                            VideoType.Attributes.Add("content", "application/x-shockwave-flash");
                            VideoType.Attributes.Add("property", "og:video:type");

                            HtmlMeta VideoUrl = new HtmlMeta();
                            VideoUrl.Attributes.Add("content", _VidepPath);
                            VideoUrl.Attributes.Add("property", "og:video");

                            HtmlMeta VideoThumb = new HtmlMeta();
                            VideoThumb.Attributes.Add("content", _ImagePath);
                            VideoThumb.Attributes.Add("property", "og:image");

                           

                            Head1.Controls.Add(link);
                            Head1.Controls.Add(Videoimage);
                            Head1.Controls.Add(Title);
                            Head1.Controls.Add(VideoUrl);
                            Head1.Controls.Add(VideoType);
                            Head1.Controls.Add(VideoThumb);

                        }

                        if (!IsPostBack)
                        {
                            InsertInboundParameters();
                        }

                        hlogo.HRef = "~/";

                        IQMediaScript.LoadScripts(this.Page, Script.Clip);
                    }
                }
            }
            catch (Exception _Exception)
            {
                lblErrorMsg.Text = "An Error Occured, please try again.";
                IQMediaGroup.Core.HelperClasses.IQMediaGroupExceptions _IQMediaGroupExceptions = new IQMediaGroup.Core.HelperClasses.IQMediaGroupExceptions();
                _IQMediaGroupExceptions.ExceptionStackTrace = " Stack Trace : " + _Exception.StackTrace;
                _IQMediaGroupExceptions.ExceptionMessage = _Exception.ToString();
                _IQMediaGroupExceptions.CreatedBy = "Base - Write Exception";
                _IQMediaGroupExceptions.ModifiedBy = "Base - Write Exception";

                string _ReturnValue = string.Empty;
                IIQMediaGroupExceptionsController _IIQMediaGroupExceptionsController = _ControllerFactory.CreateObject<IIQMediaGroupExceptionsController>();
                _ReturnValue = _IIQMediaGroupExceptionsController.AddIQMediaGroupException(_IQMediaGroupExceptions);
            }
        }

        #region Button Events

        protected void ImgbtnContectUs_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Contact/", false);
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/", false);
        }

        protected void lnkProducts_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Product/", false);
        }

        protected void lnkAboutUs_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AboutUs/", false);
        }

        protected void lnkCareer_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Careers/", false);
        }

        protected void lnkContactUs_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContactUs/", false);
        }

        //protected void Unnamed1_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("~/" + "Home/");
        //}

        #endregion

        private void InsertInboundParameters()
        {
            try
            {
                ControllerFactory _ControllerFactory = new ControllerFactory();
                IInboundReportingController _IInboundReportingController = _ControllerFactory.CreateObject<IInboundReportingController>();

                System.Collections.IEnumerator en = Request.ServerVariables.Keys.GetEnumerator();
                en.MoveNext();

                string strServerVariables = string.Empty;
                strServerVariables += "<ServerVariables>";
                foreach (string strKey in Request.ServerVariables.Keys)
                {
                    strServerVariables += "<" + strKey + ">";
                    strServerVariables += Request.ServerVariables[strKey];
                    strServerVariables += "</" + strKey + ">";
                }
                strServerVariables += "</ServerVariables>";

                string strHTMLFormPost = string.Empty;
                strHTMLFormPost += "<HTMLFormPostValue>";
                foreach (string strKey in Request.Form.Keys)
                {
                    strServerVariables += "<" + strKey + ">";
                    strServerVariables += Request.Form[strKey];
                    strServerVariables += "</" + strKey + ">";
                }
                strHTMLFormPost += "</HTMLFormPostValue>";

                string strQuerystring = string.Empty;
                strQuerystring += "<QueryStringValue>";
                foreach (string strKey in Request.QueryString.Keys)
                {
                    strQuerystring += "<" + strKey + ">";
                    strQuerystring += Request.QueryString[strKey];
                    strQuerystring += "</" + strKey + ">";
                }
                strQuerystring += "</QueryStringValue>";

                InboundReporting _InboundReporting = new InboundReporting();
                _InboundReporting.RequestCollection = "<InboundParams>" + strServerVariables + strHTMLFormPost + strQuerystring + "</InboundParams>";
                _IInboundReportingController.InsertInboundReporting(_InboundReporting);

            }
            catch (Exception _Exception)
            {

            }
        }

        public void WriteBytesToFile(string fileName, byte[] content)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
            }
            finally
            {
                fs.Close();
                w.Close();
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
                if (useragent.Contains("android") && useragent.Contains("tablet"))
                {
                    return true;
                }
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

        public static string DoHttpGetRequest(string p_URL)
        {
            try
            {
                Uri _Uri = new Uri(p_URL);
                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);

                _objWebRequest.Timeout = 100000000;
                _objWebRequest.Method = "GET";

                StreamReader _StreamReader = null;

                string _ResponseRawMedia = string.Empty;

                if ((_objWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_objWebRequest.GetResponse().GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                return _ResponseRawMedia;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
