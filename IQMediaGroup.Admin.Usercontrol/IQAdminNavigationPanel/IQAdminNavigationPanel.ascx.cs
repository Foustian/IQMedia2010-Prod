using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Usercontrol.Base;
//using IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel;
using IQMediaGroup.Admin.Core.Enumeration;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Usercontrol
{
    public partial class IQAdminNavigationPanel : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                btnCareer.HRef = "~/CareerContent/";
                btnContactUs.HRef = "~/ContactUsContent/";
                btnAboutUs.HRef = "~/AboutUsContent/";
                btnClientRegistration.HRef = "~/ClientRegistration/";
                btnIQRole.HRef = "~/IQRole/";
                btnCustomerRegistration.HRef = "~/CustomerRegistration";
                btnCustomerRole.HRef = "~/CustomerRole";
                btnClientRole.HRef = "~/ClientRole";
                btnProduct.HRef = "~/ProductContent";
                btnClipExport.HRef = "~/ClipExport";
                btnPMGSearchDemo.HRef = "~/PMGSearchDemo";

                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                long CustomerID = _SessionInformation.AdminUserKey;
                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                List<Customer> _ListOfCustomer = _ICustomerController.GetCustomerByCustomerIDRoleName(CustomerID, "IQAgentAccess");

                if (_ListOfCustomer.Count > 0)
                {
                    btnIQAgentConsole.Visible = true;
                    //btnIQAgentSetupPage.Visible = true;
                    btnIQAgentConsole.HRef = "~/IQAgentConsole";
                    //btnIQAgentSetupPage.HRef = "~/IQAgentSetup";
                }
                else
                {
                    btnIQAgentConsole.Visible = false;
                    //btnIQAgentSetupPage.Visible = false;
                }
                //HeaderTabPanel _HeaderTabPanel = (HeaderTabPanel)this.Page.Master.FindControl(CommonConstants.HeaderTabPanel);

                //_HeaderTabPanel.ActiveTab = CommonConstants.LBtnNothing;
            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        #endregion
    }
}