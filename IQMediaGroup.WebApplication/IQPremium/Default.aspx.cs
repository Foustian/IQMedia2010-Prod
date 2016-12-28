using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.WebApplication.IQPremium
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IQMediaGroupResponsive iQMediaGroupResponsive = (IQMediaGroupResponsive)this.Page.Master;
            //iQMediaGroupResponsive.PageTitle = CommonConstants.MYIQPageTitle;
            IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

            if (_SessionInformation.IsiQPremium == true)
            {
                //sionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetAllowResponseInBrowserHistory(false);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.MinValue);

                HeaderTabPanel _HeaderTabPanel = (HeaderTabPanel)this.Page.Master.FindControl(CommonConstants.HeaderTabPanel);

                _HeaderTabPanel.ActiveTab = CommonConstants.aIQPremium; //CommonConstants.LBtnIQMediaArchieve;
            }
            else
            {
                Response.Redirect("~/NoRole/?FromUrl=~/IQPremium/", false);
                Response.Redirect("~/NoRole/", false);
            }
        }
    }
}