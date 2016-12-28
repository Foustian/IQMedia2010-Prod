using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.Enumeration;
using System.Web.UI;
//using System.Web.UI;

namespace IQMediaGroup.Admin.Controller.Common
{
    public class RedlassoScript
    {
        public static void LoadScripts(Page p_Page, Script p_Script)
        {
            try
            {
                string _Host = p_Page.ResolveClientUrl("~/Script/");

                switch (p_Script)
                {
                    case Script.RawMedia:                       
                        p_Page.ClientScript.RegisterClientScriptInclude("script2", _Host + "rl-jsapi-1.0.0.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script3", _Host + "jquery-1.3.2.min.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script4", _Host + "jquery-ui-1.7.2.custom.min.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script9", _Host + "general.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script5", _Host + "auth.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script6", _Host + "raw.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script7", _Host + "blogger.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script8", _Host + "RedlassoLogin.js");
                        p_Page.ClientScript.RegisterClientScriptInclude("script1", _Host + "jquery-1.3.2.js");
                        break;
                    case Script.Clip:
                        p_Page.ClientScript.RegisterClientScriptInclude("script2", _Host + "rl-jsapi-1.0.0.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script3", _Host + "jquery-1.3.2.min.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script5", _Host + "jquery-ui-1.7.2.custom.min.js");
                        p_Page.ClientScript.RegisterClientScriptInclude("script9", _Host + "general.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script6", _Host + "auth.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script7", _Host + "raw.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script8", _Host + "RedlassoLogin.js");
                        p_Page.ClientScript.RegisterClientScriptInclude("script1", _Host + "jquery-1.3.2.js");
                        break;
                    case Script.Login:
                        p_Page.ClientScript.RegisterClientScriptInclude("script1", _Host + "rl-jsapi-1.0.0.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script2", _Host + "jquery-1.3.2.min.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script3", _Host + "jquery-ui-1.7.2.custom.min.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script5", _Host + "general.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script4", _Host + "RedlassoLogin.js");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
