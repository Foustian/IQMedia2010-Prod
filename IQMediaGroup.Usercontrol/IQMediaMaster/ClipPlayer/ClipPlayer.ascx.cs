using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using System.Text.RegularExpressions;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using System.Configuration;
using IQMediaGroup.Core.HelperClasses;
using System.Xml;
using System.Data;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Controller.Common;
using System.Net;
using System.IO;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.ClipPlayer
{
    public partial class ClipPlayer : BaseControl
    {
        List<IQMediaGroup.Core.HelperClasses.Caption> _ListOfRCatpion = null;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        string _Expression = "^?[{|\\(]?[0-9a-fA-F]{8}[-]?([0-9a-fA-F]{4}[-]?){3}[0-9a-fA-F]{12}[\\)|}]?";
        string _UserName = string.Empty;
        string _Password = string.Empty;


        private bool _IsDefaultLoad = true;
        public bool IsDefaultLoad
        {
            set { _IsDefaultLoad = value; }
            get { return _IsDefaultLoad; }
        }

        private Guid? _DefaultClipID = null;
        public Guid? DefaultClipID
        {
            set { _DefaultClipID = value; }
            get { return _DefaultClipID; }
        }

        private bool _IsMicriSite = false;
        public bool IsMicriSite
        {
            set { _IsMicriSite = value; }
            get { return _IsMicriSite; }
        }

        #region Page Events




        protected override void OnLoad(EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {

                    if (Request.QueryString["ClipID"] != null && Request.QueryString["PN"] != null)
                    {
                        string _ClipID = Request.QueryString["ClipID"];

                        //string _toEmail = Server.UrlEncode(Request.QueryString["TE"]);
                        //_toEmail = _toEmail.Replace("+", "%2b");
                        //_toEmail = IQMediaGroup.Core.HelperClasses.CommonFunctions.Decrypt(Server.UrlDecode(_toEmail), ConfigurationManager.AppSettings["EncryptionKey"]);

                        //if (_toEmail == "Invalid length for a Base-64 char array.")
                        //{
                        //    _toEmail = "iqmedia@iqmediacorp.com";
                        //}

                        string _PageName = Server.UrlEncode(Request.QueryString["PN"]);

                        _PageName = _PageName.Replace("+", "%2b");
                        _PageName = IQMediaGroup.Core.HelperClasses.CommonFunctions.Decrypt(Server.UrlDecode(_PageName), ConfigurationManager.AppSettings["EncryptionKey"]);
                        InitialClip(_ClipID, "false", "");

                    }
                    else
                    {

                        if (IsDefaultLoad == true)
                        {
                            string _DefaultClip = ConfigurationSettings.AppSettings["DefaultClipID"];
                            InitialClip(_DefaultClip, "false", "");
                        }
                        else if (DefaultClipID != null)
                        {
                            InitialClip(Convert.ToString(DefaultClipID.Value), "false", "");
                        }

                    }

                }

            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);

            }
        }
        #endregion

        #region user defined Events
        private void InitialClip(string _clipID, string _playback, string _toEmail, Guid? ClientGuid = null)
        {
            try
            {


                if (!((Request.UserAgent.ToLower().Contains("ipad") || Request.UserAgent.ToLower().Contains("iphone") || Request.UserAgent.ToLower().Contains("ipod")) || (Request.UserAgent.ToLower().Contains("android") && CheckVersion())))
                {
                    divRawMedia.Visible = true;
                    IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();
                    SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                    IClipController _IClipController = _ControllerFactory.CreateObject<IClipController>();

                    try
                    {
                        _ListOfRCatpion = _IClipController.GetClipCaption(_clipID);
                    }
                    catch (Exception ex)
                    {
                        this.WriteException(ex);
                    }

                    string baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];

                    divRawMedia.Controls.Clear();

                    divRawMedia.InnerHtml = IQMediaPlayer.RenderClipPlayer(ClientGuid, "07175c0e-2b70-4325-be6d-611910730968", _clipID, "false", "ClipPlayer", _toEmail, baseURL, _SessionInformation.IsClientPlayerLogoActive, _SessionInformation.ClientPlayerLogoImage, IsMicriSite, false);

                    AddCaption(true);
                    IsVisibleCaption(true);

                    //_ViewstateInformation.VSTimetick = 0;
                    this.SetViewstateInformation(_ViewstateInformation);
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        //public static string DoHttpGetRequest(string p_URL)
        //{
        //    try
        //    {
        //        Uri _Uri = new Uri(p_URL);
        //        HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);

        //        _objWebRequest.Timeout = 100000000;
        //        _objWebRequest.Method = "GET";

        //        StreamReader _StreamReader = null;

        //        string _ResponseRawMedia = string.Empty;

        //        if ((_objWebRequest.GetResponse().ContentLength > 0))
        //        {
        //            _StreamReader = new StreamReader(_objWebRequest.GetResponse().GetResponseStream());
        //            _ResponseRawMedia = _StreamReader.ReadToEnd();
        //            _StreamReader.Dispose();
        //        }

        //        return _ResponseRawMedia;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

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
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }





        /// <summary>
        /// Description:This function Add Caption.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="IsClip">Isclip true or false</param>
        private void AddCaption(bool IsClip)
        {
            if (_ListOfRCatpion != null)
            {


                DivCaption.Controls.Clear();

                DataTable _DataTable = new DataTable();

                _DataTable.Columns.Add("CaptionText");
                _DataTable.Columns.Add("CaptionStartTime");



                foreach (IQMediaGroup.Core.HelperClasses.Caption _Caption in _ListOfRCatpion)
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
            }
        }

        public void InitPlayerFromMicroSite(string ClipID, Guid ClientID)
        {
            InitialClip(ClipID, string.Empty, string.Empty, ClientID);
        }

        //private bool CheckVersion()
        //{
        //    Version defaultAndroidVersion = new Version(ConfigurationManager.AppSettings[CommonConstants.ConfigAndroidDefaultVersion]);
        //    string useragent = Request.UserAgent.ToLower(); //"Mozilla/5.0 (Linux; U; Android 2.1-update1; en-gb; GT-I5801 Build/ECLAIR) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17";
        //    //Regex regex = new Regex(@"(?<=\bandroid\s\b)(\d+(?:\.\d+)+)");
        //    Regex regex = new Regex(ConfigurationManager.AppSettings[CommonConstants.ConfigAndroidVersionRegex]);
        //    string version = Convert.ToString(regex.Match(useragent));

        //    if (string.IsNullOrWhiteSpace(version))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            Version currentVersion = new Version(version);
        //            if (currentVersion >= defaultAndroidVersion)
        //            {
        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            return false;
        //        }

        //    }
        //    return false;

        //}
        #endregion



    }
}