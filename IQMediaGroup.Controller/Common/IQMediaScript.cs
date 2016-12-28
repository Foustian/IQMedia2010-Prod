using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.Enumeration;
using System.Web.UI;

namespace IQMediaGroup.Controller.Common
{
    public class IQMediaScript
    {
        public static void LoadScripts(Page p_Page, Script p_Script)
        {
            try
            {
                string _Host = p_Page.ResolveClientUrl("~/Script/");

                switch (p_Script)
                {
                    case Script.RawMedia:                       
                        p_Page.ClientScript.RegisterClientScriptInclude("script3", _Host + "jquery-1.3.2.min.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script1", _Host + "jquery-1.3.2.js");
                        p_Page.ClientScript.RegisterClientScriptInclude("script6", _Host + "IQMediaraw.js?v=4272");
                        
                        break;
                    case Script.Clip:
                        p_Page.ClientScript.RegisterClientScriptInclude("script3", _Host + "jquery-1.3.2.min.js?v=4272");
                        p_Page.ClientScript.RegisterClientScriptInclude("script7", _Host + "IQMediaraw.js?v=4624");                        
                        p_Page.ClientScript.RegisterClientScriptInclude("script1", _Host + "jquery-1.3.2.js");
                        break;
                    case Script.Login:
                        p_Page.ClientScript.RegisterClientScriptInclude("script2", _Host + "jquery-1.3.2.min.js?v=4272");
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
