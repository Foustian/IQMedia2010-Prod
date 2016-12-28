using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using System.Text.RegularExpressions;
using IQMediaGroup.Usercontrol.Base;
using System.Web.UI.HtmlControls;
using System.Configuration;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.RawMediaPlayer
{
    public partial class RawMediaPlayer : BaseControl
    {
        #region Page Events
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        List<IQMediaGroup.Core.HelperClasses.Caption> _ListOfRCaption = null;
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["ClipID"] != null && Request.QueryString["RawMediaDateTime"] != null && Request.QueryString["CacheKey"] != null && Request.QueryString["Searchtext"] != null)
                    {
                        string _ClipID = Request.QueryString["ClipID"];
                        string _RawMediaDateTime = Request.QueryString["RawMediaDateTime"];
                        string _CacheKey = Request.QueryString["CacheKey"];
                        string _SearchText = Request.QueryString["Searchtext"];
                        //PlayVideo(_ClipID,_RawMediaDateTime,_CacheKey,_SearchText);
                    }
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
            }
        }
        #endregion

        #region User Defined Events       

        /// <summary>
        ///  Descritption:This method Add Captions.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="IsClip">Clip</param>
        private void AddCaption(bool IsClip, string _SearchText)
        {
            try
            {
                DivCaption.Controls.Clear();

                IQMediaGroup.Core.HelperClasses.ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                if (_ListOfRCaption != null && _ListOfRCaption.Count > 0)
                {

                    foreach (IQMediaGroup.Core.HelperClasses.Caption _Caption in _ListOfRCaption)
                    {
                        HtmlGenericControl _Div = new HtmlGenericControl();

                        _Div.TagName = CommonConstants.HTMLDiv;
                        _Div.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionMainDivClass);
                        _Div.Attributes.Add(CommonConstants.HTMLOnClick, CommonConstants.CaptionSeekPointFunction + CommonConstants.BracketOpen + _Caption.StartTime + CommonConstants.BracketClose + CommonConstants.SemiColon);


                        if (IsClip == false)
                        {
                            _Div.Attributes.Add(CommonConstants.HTMLOnClick, CommonConstants.CaptionSeekPointFunction + CommonConstants.BracketOpen + (_Caption.StartTime - Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaCaptionDelay].ToString())) + CommonConstants.BracketClose + CommonConstants.SemiColon);
                        }
                        else
                        {
                            _Div.Attributes.Add(CommonConstants.HTMLOnClick, CommonConstants.CaptionSeekPointFunction + CommonConstants.BracketOpen + (_Caption.StartTime) + CommonConstants.BracketClose + CommonConstants.SemiColon);
                        }


                        HtmlGenericControl _DivTime = new HtmlGenericControl();

                        _DivTime.TagName = CommonConstants.HTMLDiv;
                        _DivTime.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionDateTimeClass);
                        _DivTime.InnerText = _Caption.StartDateTime;

                        HtmlGenericControl _DivCaptionString = new HtmlGenericControl();

                        _DivCaptionString.TagName = CommonConstants.HTMLDiv;
                        _DivCaptionString.Attributes.Add(CommonConstants.HTMLClass, CommonConstants.CaptionCaptionClass);

                        if (IsClip == false && _Caption.CaptionString.ToLower().Contains(_SearchText.ToLower()))
                        {
                            if (_SearchText != null)
                            {
                                string _BeforeSubString = _Caption.CaptionString.Substring(0, _Caption.CaptionString.ToLower().IndexOf(_SearchText.ToLower()));
                                string _AfterSubString = _Caption.CaptionString.Substring(_Caption.CaptionString.ToLower().IndexOf(_SearchText.ToLower()) + _SearchText.Length);

                                string _SearchString = "<span style=\"background-color:Yellow;\" class=\"HighlightCaption\">" + _SearchText + "</span>";

                                _DivCaptionString.InnerHtml = _BeforeSubString + _SearchString.ToUpper() + _AfterSubString;
                            }
                            else
                            {
                                _DivCaptionString.InnerText = _Caption.CaptionString;
                            }

                        }
                        else
                        {
                            _DivCaptionString.InnerText = _Caption.CaptionString;
                        }

                        if (IsClip == false)
                        {
                            _Div.Controls.Add(_DivTime);
                        }

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

        /// <summary>
        /// Descritption:This method gets Captions.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ID"></param>
        /// <param name="IsClip"></param>
        /// <param name="p_DateTime"></param>
        /// <param name="p_CacheKey"></param>
        /// <returns></returns>
        public string GetCaption(string p_ID, bool IsClip, string p_DateTime, string p_CacheKey)
        {
            try
            {
                string _CaptionText = string.Empty;

                if (IsClip == false && string.IsNullOrEmpty(p_CacheKey) == false)
                {
                    IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();

                    _ListOfRCaption = _IRawMediaController.GetRawMediaCaption(p_CacheKey);

                    foreach (IQMediaGroup.Core.HelperClasses.Caption _Caption in _ListOfRCaption)
                    {
                        string _DateTime = string.Empty;

                        DateTime? _RawMediaDateTime = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetDateTimeValue(p_DateTime);

                        if (_RawMediaDateTime != null)
                        {
                            string _StrDateTime = (_RawMediaDateTime.Value.AddSeconds(_Caption.StartTime)).ToString();
                            Regex _Regex = new Regex(CommonConstants.RegexExCaptionDateTime);
                            MatchCollection _MatchCollection = _Regex.Matches(_StrDateTime);

                            if (_MatchCollection.Count > 0)
                            {
                                _Caption.StartDateTime = _MatchCollection[0].Value;
                            }
                        }
                    }
                }

                if (IsClip == true && string.IsNullOrEmpty(p_ID) == false)
                {
                    IClipController _IClipController = _ControllerFactory.CreateObject<IClipController>();
                    _ListOfRCaption = _IClipController.GetClipCaption(p_ID);
                }

                return _CaptionText;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void IsVisibleCaption(bool p_Visible)
        {
            DivCaption.Visible = p_Visible;
        }

        #endregion
    }
}