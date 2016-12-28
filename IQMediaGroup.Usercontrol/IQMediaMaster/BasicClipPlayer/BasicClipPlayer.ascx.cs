using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using System.Text.RegularExpressions;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Common;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Factory;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Usercontrol.Base;
using System.Configuration;
using System.IO;
using System.Xml;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.Clip
{
    public partial class BasicClipPlayer : BaseControl
    {

        protected override void OnLoad(EventArgs e)
        {
        }

        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public void RenderClip(string p_ClipID)
        {
            //GetCaption(p_ClipID.ToString(), true, string.Empty, string.Empty);

            string _RegexExpression = CommonConstants.RegexExClipID;

            Regex _Regex = new Regex(_RegexExpression);

            System.Text.RegularExpressions.MatchCollection _Match = _Regex.Matches(p_ClipID.ToString());

            string _clipID = string.Empty;

            if (_Match.Count > 0)
            {
                _clipID = _Match[0].Value;
            }

            _clipID.Replace(CommonConstants.DblQuote, string.Empty);

            InitialClip(_clipID, "false");

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadScript", "showClipPlayer();", true);
        }

        private void InitialClip(string _clipID, string IsRawMedia)
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                string baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];

                if (HttpContext.Current.Request.ServerVariables["Http_Host"].ToLower()=="mycliqmedia")
                {
                    baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
                }

                string RawMediaObject = IQMediaPlayer.RenderClipPlayer(new Guid(_SessionInformation.ClientGUID), string.Empty, _clipID, IsRawMedia, "iqBasic", _SessionInformation.Email, baseURL, _SessionInformation.IsClientPlayerLogoActive, _SessionInformation.ClientPlayerLogoImage);
                divRawMedia.InnerHtml = RawMediaObject;
                hfDivRawMedia.Value = RawMediaObject;

                List<Caption> _ListOfCaption = GetCaption(_clipID);
                AddCaption(_ListOfCaption);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        private void AddCaption(List<Caption> p_ListOfCaption)
        {
            try
            {
                DivCaption.Controls.Clear();

                IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                if (p_ListOfCaption != null && p_ListOfCaption.Count > 0)
                {
                    ClosedCaption.Style.Add("display", "block");

                    foreach (IQMediaGroup.Core.HelperClasses.Caption _Caption in p_ListOfCaption)
                    {
                        HtmlGenericControl _Div = new HtmlGenericControl();

                        _Div.TagName = "span";
                        _Div.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionMainDivClass);

                        _Div.Attributes.Add(CommonConstants.HTMLOnClick, CommonConstants.CaptionSeekPointFunction + CommonConstants.BracketOpen + (_Caption.StartTime) + CommonConstants.BracketClose + CommonConstants.SemiColon);

                        HtmlGenericControl _DivTime = new HtmlGenericControl();

                        _DivTime.TagName = "span";
                        _DivTime.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionDateTimeClass);
                        _DivTime.InnerText = _Caption.StartDateTime;

                        HtmlGenericControl _DivCaptionString = new HtmlGenericControl();

                        _DivCaptionString.TagName = "span";
                        _DivCaptionString.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionCaptionClass);

                        _DivCaptionString.InnerText = _Caption.CaptionString;

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
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<Caption> GetCaption(string p_ClipID)
        {
            try
            {
                List<Caption> _ListOfCaption = null;

                IClipController _IClipController = _ControllerFactory.CreateObject<IClipController>();
                _ListOfCaption = _IClipController.GetClipCaption(p_ClipID);

                return _ListOfCaption;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}