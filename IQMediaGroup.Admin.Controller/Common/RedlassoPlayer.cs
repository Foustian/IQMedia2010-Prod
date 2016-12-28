using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Web.UI.HtmlControls;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using System.Web;
using System.Web.UI.HtmlControls;

namespace IQMediaGroup.Admin.Controller.Common
{
    public class RedlassoPlayer
    {
        static ControllerFactory _ControllerFactory = new ControllerFactory();

        public static HtmlGenericControl RenderRawMediaPlayer(string p_UserID, string p_RawMediaID, string IsRawMedia, string _ClientGUID, string _CustomerGUID, string ServicesBaseURL)
        {
            try
            {
                string BrowserType = HttpContext.Current.Request.Browser.Browser;

                if (BrowserType == "AppleMAC-Safari")
                {

                    //IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();
                    //string _ParamValue = _IRawMediaController.GetRawMediaPlayParamValue(p_RawMediaID);

                    string _ParamValue = "userId=" + p_UserID + "&RL_User_GUID=" + ConfigurationSettings.AppSettings["RL_User_GUID"] + "&clientGUID=" + _ClientGUID + "&customerGUID=" + _CustomerGUID + "&categoryCode=" + ConfigurationSettings.AppSettings["CategoryCode"] + "&IsRawMedia=" + IsRawMedia + "&embedId=" + p_RawMediaID + "&ServicesBaseURL=" + ServicesBaseURL + "&autoPlayback=true";

                    HtmlGenericControl div = new HtmlGenericControl();
                    div.TagName = CommonConstants.HTMLDiv;
                    div.Style.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
                    div.Style.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());

                    HtmlGenericControl obj = new HtmlGenericControl();
                    obj.TagName = CommonConstants.HTMLObject;
                    obj.Attributes.Add(CommonConstants.HTMLID, CommonConstants.ObjectID);
                    obj.Attributes.Add(CommonConstants.HTMLname, CommonConstants.ObjectName);
                    obj.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
                    obj.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());
                    obj.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectType].ToString());

                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        obj.Attributes.Add(CommonConstants.HTMLData, "http://local.iqmediacorp.com/" + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectData].ToString());
                    }
                    else
                    {
                        obj.Attributes.Add(CommonConstants.HTMLData, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectData].ToString());
                    }

                    HtmlGenericControl paramMovie = new HtmlGenericControl();
                    paramMovie.TagName = CommonConstants.HTMLParam;
                    paramMovie.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLMovie);

                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        paramMovie.Attributes.Add(CommonConstants.HTMLValue, "http://local.iqmediacorp.com/" + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamMovie].ToString());
                    }
                    else
                    {
                        paramMovie.Attributes.Add(CommonConstants.HTMLValue, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamMovie].ToString());
                    }

                    HtmlGenericControl paramScreen = new HtmlGenericControl();
                    paramScreen.TagName = CommonConstants.HTMLParam;
                    paramScreen.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen.ToLower());
                    paramScreen.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowFullScreen].ToString());

                    HtmlGenericControl paramScript = new HtmlGenericControl();
                    paramScript.TagName = CommonConstants.HTMLParam;
                    paramScript.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess.ToLower());
                    paramScript.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowScriptAccess].ToString());

                    HtmlGenericControl paramQuality = new HtmlGenericControl();
                    paramQuality.TagName = CommonConstants.HTMLParam;
                    paramQuality.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLQuality);
                    paramQuality.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamQuality].ToString());

                    HtmlGenericControl paramWmode = new HtmlGenericControl();
                    paramWmode.TagName = CommonConstants.HTMLParam;
                    paramWmode.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLWmode);
                    paramWmode.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamWmode].ToString());

                    HtmlGenericControl paramScreenCaps = new HtmlGenericControl();
                    paramScreenCaps.TagName = CommonConstants.HTMLParam;
                    paramScreenCaps.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen);
                    paramScreenCaps.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowFullScreen].ToString());

                    HtmlGenericControl paramScriptCaps = new HtmlGenericControl();
                    paramScriptCaps.TagName = CommonConstants.HTMLParam;
                    paramScriptCaps.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess);
                    paramScriptCaps.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowScriptAccess].ToString());


                    HtmlGenericControl paramFlashvars = new HtmlGenericControl();
                    paramFlashvars.TagName = CommonConstants.HTMLParam;
                    paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
                    paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, _ParamValue);

                    obj.Controls.Add(paramMovie);
                    obj.Controls.Add(paramScreen);
                    obj.Controls.Add(paramScript);
                    obj.Controls.Add(paramQuality);
                    obj.Controls.Add(paramWmode);
                    obj.Controls.Add(paramScreenCaps);
                    obj.Controls.Add(paramScreenCaps);
                    obj.Controls.Add(paramFlashvars);

                    return obj;
                }
                else
                {

                    //IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();
                    //string _ParamValue = _IRawMediaController.GetRawMediaPlayParamValue(p_RawMediaID);


                    string _ParamValue = "userId=" + p_UserID + "&RL_User_GUID=" + ConfigurationSettings.AppSettings["RL_User_GUID"] + "&clientGUID=" + _ClientGUID + "&customerGUID=" + _CustomerGUID + "&categoryCode=" + ConfigurationSettings.AppSettings["CategoryCode"] + "&IsRawMedia=" + IsRawMedia + "&embedId=" + p_RawMediaID + "&ServicesBaseURL=" + ServicesBaseURL + "&autoPlayback=true";

                    HtmlGenericControl div = new HtmlGenericControl();
                    div.TagName = CommonConstants.HTMLDiv;
                    div.Style.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
                    div.Style.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());

                    HtmlGenericControl obj = new HtmlGenericControl();
                    obj.TagName = CommonConstants.HTMLObject;
                    obj.Attributes.Add(CommonConstants.HTMLID, CommonConstants.ObjectID);
                    obj.Attributes.Add(CommonConstants.HTMLname, CommonConstants.ObjectName);
                    obj.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
                    obj.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());
                    obj.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectType].ToString());

                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        obj.Attributes.Add(CommonConstants.HTMLData, "http://local.iqmediacorp.com" + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectData].ToString());
                    }
                    else
                    {
                        obj.Attributes.Add(CommonConstants.HTMLData, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectData].ToString());
                    }

                    HtmlGenericControl paramMovie = new HtmlGenericControl();
                    paramMovie.TagName = CommonConstants.HTMLParam;
                    paramMovie.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLMovie);

                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        paramMovie.Attributes.Add(CommonConstants.HTMLValue, "http://local.iqmediacorp.com" + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamMovie].ToString());
                    }
                    else
                    {
                        paramMovie.Attributes.Add(CommonConstants.HTMLValue, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamMovie].ToString());
                    }

                    HtmlGenericControl paramScreen = new HtmlGenericControl();
                    paramScreen.TagName = CommonConstants.HTMLParam;
                    paramScreen.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen);
                    paramScreen.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowFullScreen].ToString());

                    HtmlGenericControl paramScript = new HtmlGenericControl();
                    paramScript.TagName = CommonConstants.HTMLParam;
                    paramScript.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess);
                    paramScript.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowScriptAccess].ToString());

                    HtmlGenericControl paramQuality = new HtmlGenericControl();
                    paramQuality.TagName = CommonConstants.HTMLParam;
                    paramQuality.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLQuality);
                    paramQuality.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamQuality].ToString());

                    HtmlGenericControl paramWmode = new HtmlGenericControl();
                    paramWmode.TagName = CommonConstants.HTMLParam;
                    paramWmode.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLWmode);
                    paramWmode.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamWmode].ToString());

                    HtmlGenericControl paramFlashvars = new HtmlGenericControl();
                    paramFlashvars.TagName = CommonConstants.HTMLParam;
                    paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
                    paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, _ParamValue);

                    HtmlGenericControl embed = new HtmlGenericControl();
                    embed.TagName = CommonConstants.HTMLEmbed;
                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        embed.Attributes.Add(CommonConstants.HTMLSrc, "http://local.iqmediacorp.com" + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedSrc].ToString());
                    }
                    else
                    {
                        embed.Attributes.Add(CommonConstants.HTMLSrc, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedSrc].ToString());
                    }

                    embed.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedWidth].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedHeight].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedtype].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLAllowScriptAccess, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedAllowScriptAccess].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLAllowFullScreen, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedAllowFullScreen].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLName, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedName].ToString());
                    /*embed.Attributes.Add(CommonConstants.HTMLValue, _ParamValue);*/

                    obj.Controls.Add(paramMovie);
                    obj.Controls.Add(paramScreen);
                    obj.Controls.Add(paramScript);
                    obj.Controls.Add(paramQuality);
                    obj.Controls.Add(paramWmode);
                    obj.Controls.Add(paramFlashvars);
                    obj.Controls.Add(embed);

                    return obj;

                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static HtmlGenericControl RenderClipPlayer(string p_UserID, string p_ClipID, string IsRawMedia, string PageName, string _toEmail, string ServicesBaseURL)
        {
            try
            {
                string BrowserType = HttpContext.Current.Request.Browser.Browser;

                if (BrowserType == "AppleMAC-Safari")
                {
                    string _ParamValue = "userId=" + p_UserID + "&IsRawMedia=" + IsRawMedia + "&PageName=" + PageName + "&ToEmail=" + _toEmail + "&embedId=" + p_ClipID + "&ServicesBaseURL=" + ServicesBaseURL + "&PlayerFromLocal=" + ConfigurationSettings.AppSettings["PlayerFromLocal"] + "&autoPlayback=true";

                    HtmlGenericControl div = new HtmlGenericControl();
                    div.TagName = CommonConstants.HTMLDiv;
                    div.Style.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
                    div.Style.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());

                    HtmlGenericControl obj = new HtmlGenericControl();
                    obj.TagName = CommonConstants.HTMLObject;
                    //obj.Attributes.Add(CommonConstants.HTMLID, CommonConstants.ObjectID);
                    //obj.Attributes.Add(CommonConstants.HTMLname, CommonConstants.ObjectName);
                    obj.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectType].ToString());

                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        obj.Attributes.Add(CommonConstants.HTMLData, "http://local.iqmediacorp.com/" + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectData].ToString());
                    }
                    else
                    {
                        obj.Attributes.Add(CommonConstants.HTMLData, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectData].ToString());
                    }

                    obj.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectWidth].ToString());
                    obj.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectHeight].ToString());

                    /*HtmlGenericControl paramMovie = new HtmlGenericControl();
                    paramMovie.TagName = CommonConstants.HTMLParam;
                    paramMovie.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLMovie);
                    paramMovie.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamMovie].ToString());*/

                    HtmlGenericControl paramScreen = new HtmlGenericControl();
                    paramScreen.TagName = CommonConstants.HTMLParam;
                    paramScreen.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen);
                    paramScreen.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamAllowFullScreen].ToString());

                    HtmlGenericControl paramScript = new HtmlGenericControl();
                    paramScript.TagName = CommonConstants.HTMLParam;
                    paramScript.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess);
                    paramScript.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamAllowScriptAccess].ToString());

                    HtmlGenericControl paramQuality = new HtmlGenericControl();
                    paramQuality.TagName = CommonConstants.HTMLParam;
                    paramQuality.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLQuality);
                    paramQuality.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamQuality].ToString());

                    HtmlGenericControl paramWmode = new HtmlGenericControl();
                    paramWmode.TagName = CommonConstants.HTMLParam;
                    paramWmode.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLWmode);
                    paramWmode.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamWmode].ToString());
                    /*HtmlGenericControl paramFlashvars = new HtmlGenericControl();
                    paramFlashvars.TagName = CommonConstants.HTMLParam;
                    if (p_AutoPlay == false)
                    {
                        paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
                        paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID + CommonConstants.Ampersand + CommonConstants.HTMLAutoPlayBack + CommonConstants.Equal + false.ToString().ToLower());
                    }
                    else
                    {
                        paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
                        paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID + CommonConstants.Ampersand + CommonConstants.HTMLAutoPlayBack + CommonConstants.Equal + true.ToString().ToLower());
                    }*/

                    HtmlGenericControl paramScreen1 = new HtmlGenericControl();
                    paramScreen1.TagName = CommonConstants.HTMLParam;
                    paramScreen1.Attributes.Add(CommonConstants.HTMLName, "allowFullScreen");
                    paramScreen1.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamAllowFullScreen].ToString());

                    HtmlGenericControl paramScript1 = new HtmlGenericControl();
                    paramScript1.TagName = CommonConstants.HTMLParam;
                    paramScript1.Attributes.Add(CommonConstants.HTMLName, "allowScriptAccess");
                    paramScript1.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamAllowScriptAccess].ToString());



                    HtmlGenericControl paramFlashvars = new HtmlGenericControl();
                    paramFlashvars.TagName = CommonConstants.HTMLParam;
                    paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
                    paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, _ParamValue);




                    /*HtmlGenericControl embed = new HtmlGenericControl();
                    embed.TagName = CommonConstants.HTMLEmbed;
                    embed.Attributes.Add(CommonConstants.HTMLSrc, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedSrc].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLFlashvars, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID);
                    embed.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedWidth].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedHeight].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedType].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLAllowScriptAccess, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedAllowScriptAccess].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLAllowFullScreen, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedAllowFullScreen].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLWmode, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedWmode].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLAutoPlayBack, true.ToString().ToLower());
                    embed.Attributes.Add(CommonConstants.HTMLName, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedName].ToString());*/

                    //obj.Controls.Add(paramMovie);
                    obj.Controls.Add(paramScreen);
                    obj.Controls.Add(paramScript);
                    obj.Controls.Add(paramQuality);
                    obj.Controls.Add(paramWmode);
                    obj.Controls.Add(paramScreen1);
                    obj.Controls.Add(paramScript1);
                    obj.Controls.Add(paramFlashvars);
                    //obj.Controls.Add(embed);

                    return obj;
                }
                else
                {

                    string _ParamValue = "userId=" + p_UserID + "&IsRawMedia=" + IsRawMedia + "&PageName=" + PageName + "&ToEmail=" + _toEmail + "&embedId=" + p_ClipID + "&ServicesBaseURL=" + ServicesBaseURL + "&PlayerFromLocal=" + ConfigurationSettings.AppSettings["PlayerFromLocal"] + "&autoPlayback=true";
                    //IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();

                    HtmlGenericControl div = new HtmlGenericControl();
                    div.TagName = CommonConstants.HTMLDiv;
                    div.Style.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
                    div.Style.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());

                    HtmlGenericControl obj = new HtmlGenericControl();
                    obj.TagName = CommonConstants.HTMLObject;
                    //obj.Attributes.Add(CommonConstants.HTMLID, CommonConstants.ObjectID);
                    //obj.Attributes.Add(CommonConstants.HTMLname, CommonConstants.ObjectName);
                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        obj.Attributes.Add(CommonConstants.HTMLType, "http://local.iqmediacorp.com/" + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectType].ToString());
                        obj.Attributes.Add(CommonConstants.HTMLData, "http://local.iqmediacorp.com/" + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectData].ToString());
                    }
                    else
                    {
                        obj.Attributes.Add(CommonConstants.HTMLType, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectType].ToString());
                        obj.Attributes.Add(CommonConstants.HTMLData, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectData].ToString());
                    }
                    obj.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectWidth].ToString());
                    obj.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectHeight].ToString());

                    HtmlGenericControl paramMovie = new HtmlGenericControl();
                    paramMovie.TagName = CommonConstants.HTMLParam;
                    paramMovie.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLMovie);
                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        paramMovie.Attributes.Add(CommonConstants.HTMLValue, "http://local.iqmediacorp.com/" + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamMovie].ToString());
                    }
                    else
                    {
                        paramMovie.Attributes.Add(CommonConstants.HTMLValue, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamMovie].ToString());
                    }
                    HtmlGenericControl paramScript = new HtmlGenericControl();
                    paramScript.TagName = CommonConstants.HTMLParam;
                    paramScript.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess);
                    paramScript.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamAllowScriptAccess].ToString());

                    HtmlGenericControl paramScreen = new HtmlGenericControl();
                    paramScreen.TagName = CommonConstants.HTMLParam;
                    paramScreen.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen);
                    paramScreen.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamAllowFullScreen].ToString());


                    HtmlGenericControl paramWmode = new HtmlGenericControl();
                    paramWmode.TagName = CommonConstants.HTMLParam;
                    paramWmode.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLWmode);
                    paramWmode.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamWmode].ToString());
                    /*HtmlGenericControl paramFlashvars = new HtmlGenericControl();
                    paramFlashvars.TagName = CommonConstants.HTMLParam;
                    if (p_AutoPlay == false)
                    {
                        paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
                        paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID + CommonConstants.Ampersand + CommonConstants.HTMLAutoPlayBack + CommonConstants.Equal + false.ToString().ToLower());
                    }
                    else
                    {
                        paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
                        paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID + CommonConstants.Ampersand + CommonConstants.HTMLAutoPlayBack + CommonConstants.Equal + true.ToString().ToLower());
                    }*/

                    HtmlGenericControl paramFlashvars = new HtmlGenericControl();
                    paramFlashvars.TagName = CommonConstants.HTMLParam;
                    paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
                    paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, _ParamValue);




                    HtmlGenericControl embed = new HtmlGenericControl();
                    embed.TagName = CommonConstants.HTMLEmbed;
                    if (ConfigurationSettings.AppSettings["IsLocal"] == true.ToString())
                    {
                        embed.Attributes.Add(CommonConstants.HTMLSrc, "http://local.iqmediacorp.com/" + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedSrc].ToString());
                    }
                    else
                    {
                        embed.Attributes.Add(CommonConstants.HTMLSrc, "http://" + HttpContext.Current.Request.Url.Host.ToString() + ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedSrc].ToString());
                    }
                    //embed.Attributes.Add(CommonConstants.HTMLFlashvars, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID);
                    embed.Attributes.Add("id", ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedName].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLFlashvars, _ParamValue);
                    embed.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedWidth].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedHeight].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedType].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLAllowScriptAccess, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedAllowScriptAccess].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLAllowFullScreen, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedAllowFullScreen].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLWmode, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedWmode].ToString());
                    embed.Attributes.Add(CommonConstants.HTMLAutoPlayBack, true.ToString().ToLower());
                    embed.Attributes.Add(CommonConstants.HTMLName, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedName].ToString());

                    obj.Controls.Add(paramMovie);
                    obj.Controls.Add(paramFlashvars);
                    obj.Controls.Add(paramScreen);
                    obj.Controls.Add(paramScript);
                    obj.Controls.Add(paramWmode);
                    obj.Controls.Add(embed);

                    return obj;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        //public static HtmlGenericControl RenderRawMediaPlayer(string p_RawMediaID)
        //{
        //    try
        //    {
        //        string BrowserType = HttpContext.Current.Request.Browser.Browser;

        //        if (BrowserType == "AppleMAC-Safari")
        //        {

        //            IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();
        //            string _ParamValue = _IRawMediaController.GetRawMediaPlayParamValue(p_RawMediaID);

        //            HtmlGenericControl div = new HtmlGenericControl();
        //            div.TagName = CommonConstants.HTMLDiv;
        //            div.Style.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
        //            div.Style.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());

        //            HtmlGenericControl obj = new HtmlGenericControl();
        //            obj.TagName = CommonConstants.HTMLObject;
        //            obj.Attributes.Add(CommonConstants.HTMLID, CommonConstants.ObjectID);
        //            obj.Attributes.Add(CommonConstants.HTMLname, CommonConstants.ObjectName);
        //            obj.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
        //            obj.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());
        //            obj.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectType].ToString());
        //            obj.Attributes.Add(CommonConstants.HTMLData, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectData].ToString());

        //            HtmlGenericControl paramMovie = new HtmlGenericControl();
        //            paramMovie.TagName = CommonConstants.HTMLParam;
        //            paramMovie.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLMovie);
        //            paramMovie.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamMovie].ToString());

        //            HtmlGenericControl paramScreen = new HtmlGenericControl();
        //            paramScreen.TagName = CommonConstants.HTMLParam;
        //            paramScreen.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen);
        //            paramScreen.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowFullScreen].ToString());

        //            HtmlGenericControl paramScript = new HtmlGenericControl();
        //            paramScript.TagName = CommonConstants.HTMLParam;
        //            paramScript.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess);
        //            paramScript.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowScriptAccess].ToString());

        //            HtmlGenericControl paramQuality = new HtmlGenericControl();
        //            paramQuality.TagName = CommonConstants.HTMLParam;
        //            paramQuality.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLQuality);
        //            paramQuality.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamQuality].ToString());

        //            HtmlGenericControl paramWmode = new HtmlGenericControl();
        //            paramWmode.TagName = CommonConstants.HTMLParam;
        //            paramWmode.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLWmode);
        //            paramWmode.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamWmode].ToString());

        //            HtmlGenericControl paramScreen1 = new HtmlGenericControl();
        //            paramScreen.TagName = CommonConstants.HTMLParam;
        //            paramScreen.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen);
        //            paramScreen.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowFullScreen].ToString());

        //            HtmlGenericControl paramScript1 = new HtmlGenericControl();
        //            paramScript.TagName = CommonConstants.HTMLParam;
        //            paramScript.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess);
        //            paramScript.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowScriptAccess].ToString());


        //            HtmlGenericControl paramFlashvars = new HtmlGenericControl();
        //            paramFlashvars.TagName = CommonConstants.HTMLParam;
        //            paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
        //            paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, _ParamValue);

        //            obj.Controls.Add(paramMovie);
        //            obj.Controls.Add(paramScreen);
        //            obj.Controls.Add(paramScript);
        //            obj.Controls.Add(paramQuality);
        //            obj.Controls.Add(paramWmode);
        //            obj.Controls.Add(paramScreen1);
        //            obj.Controls.Add(paramScript1);
        //            obj.Controls.Add(paramFlashvars);

        //            return obj;
        //        }
        //        else
        //        {

        //            IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();
        //            string _ParamValue = _IRawMediaController.GetRawMediaPlayParamValue(p_RawMediaID);

        //            HtmlGenericControl div = new HtmlGenericControl();
        //            div.TagName = CommonConstants.HTMLDiv;
        //            div.Style.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
        //            div.Style.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());

        //            HtmlGenericControl obj = new HtmlGenericControl();
        //            obj.TagName = CommonConstants.HTMLObject;
        //            obj.Attributes.Add(CommonConstants.HTMLID, CommonConstants.ObjectID);
        //            obj.Attributes.Add(CommonConstants.HTMLname, CommonConstants.ObjectName);
        //            obj.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
        //            obj.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());
        //            obj.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectType].ToString());
        //            obj.Attributes.Add(CommonConstants.HTMLData, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectData].ToString());

        //            HtmlGenericControl paramMovie = new HtmlGenericControl();
        //            paramMovie.TagName = CommonConstants.HTMLParam;
        //            paramMovie.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLMovie);
        //            paramMovie.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamMovie].ToString());

        //            HtmlGenericControl paramScreen = new HtmlGenericControl();
        //            paramScreen.TagName = CommonConstants.HTMLParam;
        //            paramScreen.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen);
        //            paramScreen.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowFullScreen].ToString());

        //            HtmlGenericControl paramScript = new HtmlGenericControl();
        //            paramScript.TagName = CommonConstants.HTMLParam;
        //            paramScript.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess);
        //            paramScript.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamAllowScriptAccess].ToString());

        //            HtmlGenericControl paramQuality = new HtmlGenericControl();
        //            paramQuality.TagName = CommonConstants.HTMLParam;
        //            paramQuality.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLQuality);
        //            paramQuality.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamQuality].ToString());

        //            HtmlGenericControl paramWmode = new HtmlGenericControl();
        //            paramWmode.TagName = CommonConstants.HTMLParam;
        //            paramWmode.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLWmode);
        //            paramWmode.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaParamWmode].ToString());

        //            HtmlGenericControl paramFlashvars = new HtmlGenericControl();
        //            paramFlashvars.TagName = CommonConstants.HTMLParam;
        //            paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
        //            paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, _ParamValue);

        //            HtmlGenericControl embed = new HtmlGenericControl();
        //            embed.TagName = CommonConstants.HTMLEmbed;
        //            embed.Attributes.Add(CommonConstants.HTMLSrc, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedSrc].ToString());
        //            embed.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedWidth].ToString());
        //            embed.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedHeight].ToString());
        //            embed.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedtype].ToString());
        //            embed.Attributes.Add(CommonConstants.HTMLAllowScriptAccess, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedAllowScriptAccess].ToString());
        //            embed.Attributes.Add(CommonConstants.HTMLAllowFullScreen, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedAllowFullScreen].ToString());
        //            embed.Attributes.Add(CommonConstants.HTMLName, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaEmbedName].ToString());

        //            obj.Controls.Add(paramMovie);
        //            obj.Controls.Add(paramScreen);
        //            obj.Controls.Add(paramScript);
        //            obj.Controls.Add(paramQuality);
        //            obj.Controls.Add(paramWmode);
        //            obj.Controls.Add(paramFlashvars);
        //            obj.Controls.Add(embed);

        //            return obj;

        //        }
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        //public static HtmlGenericControl RenderClipPlayer(string p_ClipID, bool p_AutoPlay)
        //{
        //    try
        //    {
        //        IRawMediaController _IRawMediaController = _ControllerFactory.CreateObject<IRawMediaController>();

        //        HtmlGenericControl div = new HtmlGenericControl();
        //        div.TagName = CommonConstants.HTMLDiv;
        //        div.Style.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectWidth].ToString());
        //        div.Style.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigRawMediaObjectHeight].ToString());

        //        HtmlGenericControl obj = new HtmlGenericControl();
        //        obj.TagName = CommonConstants.HTMLObject;
        //        //obj.Attributes.Add(CommonConstants.HTMLID, CommonConstants.ObjectID);
        //        //obj.Attributes.Add(CommonConstants.HTMLname, CommonConstants.ObjectName);
        //        obj.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectType].ToString());
        //        obj.Attributes.Add(CommonConstants.HTMLData, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectData].ToString());
        //        obj.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectWidth].ToString());
        //        obj.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipObjectHeight].ToString());

        //        HtmlGenericControl paramMovie = new HtmlGenericControl();
        //        paramMovie.TagName = CommonConstants.HTMLParam;
        //        paramMovie.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLMovie);
        //        paramMovie.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamMovie].ToString());


        //        HtmlGenericControl paramFlashvars = new HtmlGenericControl();
        //        paramFlashvars.TagName = CommonConstants.HTMLParam;
        //        if (p_AutoPlay == false)
        //        {
        //            paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
        //            paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID + CommonConstants.Ampersand + CommonConstants.HTMLAutoPlayBack + CommonConstants.Equal + false.ToString().ToLower());
        //        }
        //        else
        //        {
        //            paramFlashvars.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLFlashvars);
        //            paramFlashvars.Attributes.Add(CommonConstants.HTMLValue, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID + CommonConstants.Ampersand + CommonConstants.HTMLAutoPlayBack + CommonConstants.Equal + true.ToString().ToLower());
        //        }

        //        HtmlGenericControl paramScript = new HtmlGenericControl();
        //        paramScript.TagName = CommonConstants.HTMLParam;
        //        paramScript.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowScriptAccess);
        //        paramScript.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamAllowScriptAccess].ToString());

        //        HtmlGenericControl paramScreen = new HtmlGenericControl();
        //        paramScreen.TagName = CommonConstants.HTMLParam;
        //        paramScreen.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLAllowFullScreen);
        //        paramScreen.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamAllowFullScreen].ToString());

               
        //        HtmlGenericControl paramWmode = new HtmlGenericControl();
        //        paramWmode.TagName = CommonConstants.HTMLParam;
        //        paramWmode.Attributes.Add(CommonConstants.HTMLName, CommonConstants.HTMLWmode);
        //        paramWmode.Attributes.Add(CommonConstants.HTMLValue, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipParamWmode].ToString());

        //        HtmlGenericControl embed = new HtmlGenericControl();
        //        embed.TagName = CommonConstants.HTMLEmbed;                
        //        embed.Attributes.Add(CommonConstants.HTMLSrc, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedSrc].ToString());
        //        embed.Attributes.Add(CommonConstants.HTMLFlashvars, CommonConstants.ConfigClipParamFlashvarsEmbedID + p_ClipID);
        //        embed.Attributes.Add(CommonConstants.HTMLWidth, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedWidth].ToString());
        //        embed.Attributes.Add(CommonConstants.HTMLHeight, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedHeight].ToString());
        //        embed.Attributes.Add(CommonConstants.HTMLType, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedType].ToString());
        //        embed.Attributes.Add(CommonConstants.HTMLAllowScriptAccess, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedAllowScriptAccess].ToString());
        //        embed.Attributes.Add(CommonConstants.HTMLAllowFullScreen, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedAllowFullScreen].ToString());
        //        embed.Attributes.Add(CommonConstants.HTMLWmode, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedWmode].ToString());
        //        embed.Attributes.Add(CommonConstants.HTMLAutoPlayBack, true.ToString().ToLower());
        //        embed.Attributes.Add(CommonConstants.HTMLName, ConfigurationSettings.AppSettings[CommonConstants.ConfigClipEmbedName].ToString());

        //        obj.Controls.Add(paramMovie);
        //        obj.Controls.Add(paramFlashvars);
        //        obj.Controls.Add(paramScreen);
        //        obj.Controls.Add(paramScript);
        //        obj.Controls.Add(paramWmode);
        //        obj.Controls.Add(embed);

        //        return obj;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}




    }
}
