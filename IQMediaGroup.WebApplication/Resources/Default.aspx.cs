using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.NavigationPanel;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.WebApplication.Resources
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NavigationPanel navigationPanel = (NavigationPanel)(this.Page.FindControl(CommonConstants.TopPanel).FindControl(CommonConstants.NavigationPanel));
            navigationPanel.ActiveTab = "spnResources";
        }
    }
}