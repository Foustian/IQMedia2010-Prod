using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Usercontrol.Base;

namespace IQMediaGroup.Admin.WebApplication.CustomerDetails
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            try
            {
                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                if (_SessionInformation == null || _SessionInformation.IsAdminLogin != true)
                {
                    Response.Redirect("~/Login/");
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetAllowResponseInBrowserHistory(false);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.MinValue);

            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {

            }
            catch (Exception _Exception)
            {

                BaseControl _BaseControl = new BaseControl();
                _BaseControl.WriteException(_Exception);
            }

        }
    }
}
