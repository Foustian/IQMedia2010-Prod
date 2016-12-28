using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using System.Web;

namespace IQMediaGroup.Controller.Common
{
    public class IQMediaPlayer
    {
        static ControllerFactory _ControllerFactory = new ControllerFactory();

        public static string RenderRawMediaPlayer(string p_UserID, string p_RawMediaID, string p_IsRawMedia, string p_IsUGCRawMedia, string p_ClientGUID, string IsAutoDownload, string p_CustomerGUID, string p_ServicesBaseURL, int? p_Offset, bool? IsActivePlayerLogo, string PlayerLogoImage)
        {
            try
            {
                string _ParamValue = "userId=" + p_UserID + "&RL_User_GUID=" + ConfigurationManager.AppSettings["RL_User_GUID"] + "&clientGUID=" + p_ClientGUID + "&IsAutoDownload=" + IsAutoDownload + "&customerGUID=" + p_CustomerGUID + "&categoryCode=" + ConfigurationManager.AppSettings["CategoryCode"] + "&IsRawMedia=" + p_IsRawMedia + "&IsUGC=" + p_IsUGCRawMedia + "&embedId=" + p_RawMediaID + "&ServicesBaseURL=" + GetServicesBaseURL() + "&autoPlayback=true&Offset=" + Convert.ToString(p_Offset) + "&ClipLength=" + ConfigurationManager.AppSettings["MaxClipLength"] + "&DefaultClipLength=" + ConfigurationManager.AppSettings["DefaultClipLength"];

                if (IsActivePlayerLogo != null && IsActivePlayerLogo == true && !string.IsNullOrEmpty(PlayerLogoImage))
                {
                    _ParamValue += "&PlayerLogo=" + ConfigurationManager.AppSettings["URLWaterMark"] + PlayerLogoImage + "";
                }

                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\"");
                _StringBuilder.AppendFormat(" width=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat(" height=\"{0}\"", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());

                if (HttpContext.Current.Request.Browser.Type.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat(" data=\"{0}\"", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.Append(">");

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.AppendFormat("flashvars=\"{0}\" ", _ParamValue);

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string RenderRawMediaPlayer(string p_UserID, string p_RawMediaID, string p_IsRawMedia, string p_IsUGCRawMedia, string p_ClientGUID, string IsAutoDownload, string p_CustomerGUID, string p_ServicesBaseURL, int? p_Offset, string p_KeyValue, bool? IsActivePlayerLogo, string PlayerLogoImage)
        {
            try
            {
                string _ParamValue = "userId=" + p_UserID + "&RL_User_GUID=" + ConfigurationSettings.AppSettings["RL_User_GUID"] + "&clientGUID=" + p_ClientGUID + "&IsAutoDownload=" + IsAutoDownload + "&customerGUID=" + p_CustomerGUID + "&categoryCode=" + ConfigurationSettings.AppSettings["CategoryCode"] + "&IsRawMedia=" + p_IsRawMedia + "&IsUGC=" + p_IsUGCRawMedia + "&embedId=" + p_RawMediaID + "&ServicesBaseURL=" + GetServicesBaseURL() + "&autoPlayback=true&Offset=" + Convert.ToString(p_Offset) + "&ClipLength=" + ConfigurationManager.AppSettings["MaxClipLength"] + "&DefaultClipLength=" + ConfigurationManager.AppSettings["DefaultClipLength"];

                if (!string.IsNullOrEmpty(p_KeyValue))
                {
                    _ParamValue = _ParamValue + "&KeyValues=" + HttpContext.Current.Server.HtmlEncode(p_KeyValue);
                }

                if (IsActivePlayerLogo != null && IsActivePlayerLogo == true && !string.IsNullOrEmpty(PlayerLogoImage))
                {
                    _ParamValue += "&PlayerLogo=" + ConfigurationManager.AppSettings["URLWaterMark"] + PlayerLogoImage + "";
                }

                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\" ");
                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());

                if (HttpContext.Current.Request.Browser.Type.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.Append(">");

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.Append("<param name=\"flashvars\" value=\"" + _ParamValue + "\"></param>");
                // _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["PlayerLocation"].ToString());
                }

                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.Append("flashvars=\"" + _ParamValue + "\" ");

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string RenderClipPlayer(Guid? ClientGuid, string p_UserID, string p_ClipID, string IsRawMedia, string PageName, string _toEmail, string ServicesBaseURL, bool? IsActivePlayerLogo, string PlayerLogoImage, bool IsMicrosite = false, bool EB = true)
        {
            try
            {
                string _ParamValue = "userId=" + p_UserID + "&IsRawMedia=" + IsRawMedia + "&IsUGC=false&PageName=" + PageName + "&ToEmail=" + _toEmail + "&embedId=" + p_ClipID + "&ServicesBaseURL=" + GetServicesBaseURL() + "&PlayerFromLocal=" + ConfigurationSettings.AppSettings["PlayerFromLocal"] + "&autoPlayback=true";

                if (IsActivePlayerLogo != null && IsActivePlayerLogo == true && !string.IsNullOrEmpty(PlayerLogoImage))
                {
                    _ParamValue += "&PlayerLogo=" + ConfigurationManager.AppSettings["URLWaterMark"] + PlayerLogoImage + "";
                }

                if (ClientGuid != null)
                {
                    _ParamValue += "&clientGUID=" + ClientGuid + "";
                }

                if (EB == false)
                {
                    _ParamValue += "&EB=false";
                }

                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\" ");
                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["ClipObjectWidth"]);
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["ClipObjectHeight"]);

                if (HttpContext.Current.Request.Browser.Type.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }

                if (Convert.ToBoolean(ConfigurationSettings.AppSettings["IsLocal"]) == true)
                {
                    if (!IsMicrosite)
                    {
                        _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"]);
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                    }
                }
                else
                {
                    if (!IsMicrosite)
                    {
                        _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["PlayerLocation"]);
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                    }
                }

                _StringBuilder.Append(">");

                if (Convert.ToBoolean(ConfigurationSettings.AppSettings["IsLocal"]) == true)
                {
                    if (!IsMicrosite)
                    {
                        _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationSettings.AppSettings["LocalPlayerLocation"]);
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationSettings.AppSettings["ResizePlayerLocation"]);
                    }
                }
                else
                {
                    if (!IsMicrosite)
                    {
                        _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationSettings.AppSettings["PlayerLocation"]);
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationSettings.AppSettings["ResizePlayerLocation"]);
                    }
                }

                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    if (!IsMicrosite)
                    {
                        _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["LocalPlayerLocation"]);
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                    }
                }
                else
                {
                    if (!IsMicrosite)
                    {
                        _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["PlayerLocation"]);
                    }
                    else
                    {
                        _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["ResizePlayerLocation"]);
                    }
                }

                _StringBuilder.Append("id=\"HUY\" ");
                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"]);
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"]);
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.Append("autoPlayback=\"true\" ");
                _StringBuilder.Append("wmode=\"transparent\" ");
                _StringBuilder.AppendFormat("flashvars=\"{0}\" ", _ParamValue);

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string RenderIQAgentPlayer(string embedId, string ServicesBaseURL, string autoPlayback,int offset)
        {
            try
            {
                string _ParamValue = "embedId=" + embedId + "&ServicesBaseURL=" + ServicesBaseURL + "&autoPlayback=" + autoPlayback + "&Offset=" + offset;

                /*if (!string.IsNullOrEmpty(p_KeyValue))
                {
                    _ParamValue = _ParamValue + "&KeyValues=" + HttpContext.Current.Server.HtmlEncode(p_KeyValue);
                }

                if (IsActivePlayerLogo != null && IsActivePlayerLogo == true && !string.IsNullOrEmpty(PlayerLogoImage))
                {
                    _ParamValue += "&PlayerLogo=" + ConfigurationManager.AppSettings["URLWaterMark"] + PlayerLogoImage + "";
                }*/

                StringBuilder _StringBuilder = new StringBuilder();

                _StringBuilder.Append("<object id=\"HUY\" name=\"HYETA\" type=\"application/x-shockwave-flash\" ");
                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());

                if (HttpContext.Current.Request.Browser.Type.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }

                if (HttpContext.Current.Request.Browser.Type.Contains("IE"))
                {
                    _StringBuilder.Append(" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\"");
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["IQAgentLocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("data=\"{0}\" ", ConfigurationManager.AppSettings["IQAgentPlayerLocation"].ToString());
                }

                _StringBuilder.Append(">");

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["IQAgentLocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("<param name=\"movie\" value=\"{0}\"></param>", ConfigurationManager.AppSettings["IQAgentPlayerLocation"].ToString());
                }

                _StringBuilder.Append("<param name=\"allowfullscreen\" value=\"true\"></param>");
                _StringBuilder.Append("<param name=\"allowscriptaccess\" value=\"always\"></param>");
                _StringBuilder.Append("<param name=\"quality\" value=\"high\"></param>");
                _StringBuilder.Append("<param name=\"wmode\" value=\"transparent\"></param>");
                _StringBuilder.Append("<param name=\"flashvars\" value=\"" + _ParamValue + "\"></param>");
                // _StringBuilder.AppendFormat("<param name=\"flashvars\" value=\"{0}\"></param>", _ParamValue);

                _StringBuilder.Append("<embed ");
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLocal"]) == true)
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["IQAgentLocalPlayerLocation"].ToString());
                }
                else
                {
                    _StringBuilder.AppendFormat("src=\"{0}\" ", ConfigurationManager.AppSettings["IQAgentPlayerLocation"].ToString());
                }

                _StringBuilder.AppendFormat("width=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectWidth"].ToString());
                _StringBuilder.AppendFormat("height=\"{0}\" ", ConfigurationManager.AppSettings["RawMediaObjectHeight"].ToString());
                _StringBuilder.Append("type=\"application/x-shockwave-flash\" ");
                _StringBuilder.Append("allowscriptaccess=\"true\" ");
                _StringBuilder.Append("allowfullscreen=\"always\" ");
                _StringBuilder.Append("name=\"IQMedia\" ");
                _StringBuilder.Append("flashvars=\"" + _ParamValue + "\" ");

                _StringBuilder.Append(" /></object>");

                return _StringBuilder.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public static string GetServicesBaseURL()
        {
            string baseUrl = ConfigurationManager.AppSettings["ServicesBaseURL"];

            string[] myCliqMediaHost = ConfigurationManager.AppSettings["MyCliqMediaHost"].ToLower().Split(',');

            if (myCliqMediaHost.Contains(HttpContext.Current.Request.Url.Host.ToLower()))
            {
                return ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
            }

            return baseUrl;
        }
    }
}
