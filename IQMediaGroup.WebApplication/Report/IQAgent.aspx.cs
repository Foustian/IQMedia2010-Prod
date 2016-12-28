using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Reports.Controller.Factory;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.WebApplication.Report
{
    public partial class IQAgent : System.Web.UI.Page
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (_SessionInformation.IsiQAgent == true)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetIframeHeight", "var doch=$(document).height();parent.alertsize(doch);", true);
                }
                else
                {
                    (UCIQAgentReport1.FindControl("lblErrorMessage") as Label).Text = "You are not authorized to view this page";
                    //Response.Redirect("~/NoRole/", false);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}