using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.NavigationPanel;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Usercontrol.IQMediaMaster.StaticMasterRightContent;

namespace IQMediaGroup.WebApplication.PoliticalParties
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NavigationPanel navigationPanel = (NavigationPanel)(this.Page.Master.FindControl(CommonConstants.TopPanel).FindControl(CommonConstants.NavigationPanel));

            navigationPanel.ActiveTab = "spnIndustries";

            StaticMasterRightContent staticMasterRightContent = (StaticMasterRightContent)(this.Page.Master.FindControl(CommonConstants.StaticMasterRightContent));
            staticMasterRightContent.ActiveDiv = "divIndustriesPoliticalParties";
        }
    }
}