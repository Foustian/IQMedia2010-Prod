using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using System.Web.UI.HtmlControls;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.NavigationPanel
{
    public partial class NavigationPanel : System.Web.UI.UserControl
    {
        public string ActiveTab
        {
            set
            {

                spnHome.Attributes["class"]  =  CommonConstants.CssStaticInActiveTab;
                spnAboutUs.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                spnContactUs.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                spnIndustries.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                spnProducts.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                spnResources.Attributes["class"] = CommonConstants.CssStaticInActiveTab;

                HtmlAnchor activeSpan = (HtmlAnchor)this.FindControl(value);

                if (activeSpan != null)
                {
                    activeSpan.Attributes["class"] = CommonConstants.CssStaticActiveTab;
                }                 
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}