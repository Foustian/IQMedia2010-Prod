﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.NavigationPanel;
using IQMediaGroup.Usercontrol.IQMediaMaster.StaticMasterRightContent;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.WebApplication.CopyrightPolicy
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NavigationPanel navigationPanel = (NavigationPanel)(this.Page.Master.FindControl(CommonConstants.TopPanel).FindControl(CommonConstants.NavigationPanel));
            navigationPanel.ActiveTab = "spnAboutUs";


            StaticMasterRightContent staticMasterRightContent = (StaticMasterRightContent)(this.Page.Master.FindControl(CommonConstants.StaticMasterRightContent));
            staticMasterRightContent.ActiveDiv = "divAboutUs";
        }
    }
}