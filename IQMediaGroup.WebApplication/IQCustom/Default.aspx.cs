using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel;
using IQMediaGroup.Core.Enumeration;


namespace IQMediaGroup.WebApplication.IQCustom
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                IQMediaGroupResponsive iQMediaGroupResponsive = (IQMediaGroupResponsive)this.Page.Master;
                iQMediaGroupResponsive.PageTitle = CommonConstants.IQCustomPageTitle;

                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (_SessionInformation.IsiQCustom == true)
                {
                    HeaderTabPanel _HeaderTabPanel = (HeaderTabPanel)this.Page.Master.FindControl(CommonConstants.HeaderTabPanel);

                    _HeaderTabPanel.ActiveTab = CommonConstants.aIQCustom;// CommonConstants.LBtnIQMediaCustom;

                }
                else
                {
                    //Response.Redirect("~/NoRole/?FromUrl=~/IQAdvance/", false);
                    Response.Redirect("~/NoRole/", false);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}