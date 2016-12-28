using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.StaticMasterRightContent
{
    public partial class StaticMasterRightContent : System.Web.UI.UserControl
    {

        public string ActiveDiv
        {
            set
            {
                divProducts.Visible = false;
                divOptimizedCloud.Visible = false;
                divInlineWorkShop.Visible = false;
                divMyiQ.Visible = false;
                divIndustries.Visible = false;
                divAboutUs.Visible = false;
                divProPrep.Visible = false;

                HtmlGenericControl activeDiv = (HtmlGenericControl)this.FindControl(value);

                if (activeDiv != null)
                {
                    activeDiv.Visible = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}