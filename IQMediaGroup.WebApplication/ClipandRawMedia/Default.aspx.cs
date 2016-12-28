using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Common;

namespace IQMediaGroup.WebApplication.ClipandRawMedia
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                IQMediaGroupResponsive iQMediaGroupResponsive = (IQMediaGroupResponsive)this.Page.Master;
                iQMediaGroupResponsive.PageTitle = CommonConstants.IQBasicPageTitle;

                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (_SessionInformation.IsiQBasic == true)
                {
                    HeaderTabPanel _HeaderTabPanel = (HeaderTabPanel)this.Page.Master.FindControl(CommonConstants.HeaderTabPanel);

                    _HeaderTabPanel.ActiveTab = CommonConstants.aIQBasic; //CommonConstants.LBtnIQMediaClipper;                    
                }
                else
                {                    
                    Response.Redirect("~/NoRole/", false);
                }
            }
            catch (Exception _Exception)
            {
                BaseControl _BaseControl = new BaseControl();

                _BaseControl.WriteException(_Exception);
            }
        }
    }
}
