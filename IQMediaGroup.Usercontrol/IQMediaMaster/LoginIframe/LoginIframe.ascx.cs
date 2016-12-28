using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Usercontrol.Base;

namespace IQMediaGroup.Usercontrol.IQMediaMaster
{
    public partial class LoginIframe : BaseControl
    {

        public event EventHandler LoggedIn;

        ControllerFactory _ControllerFactory = new ControllerFactory();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                ICustomerController _ICustomerController=_ControllerFactory.CreateObject<ICustomerController>();

               List<Customer> _Customer = _ICustomerController.CheckAuthentication(txtUserName.Text, txtPassword.Text);

               if (_Customer.Count>0)
               {
                   var result = IQMedia.Web.Common.Authentication.Login(txtUserName.Text, txtPassword.Text);

                   if (result == IQMedia.Web.Common.Authentication.LoginStatus.Success)
                   {
                       SessionInformationIframe _SessionInformationIframe = CommonFunctions.GetSessionInformationIframe();

                       _SessionInformationIframe.CustomerGUID = new Guid(_Customer[0].CustomerGUID);
                       _SessionInformationIframe.ClientGUID = new Guid(_Customer[0].ClientGUID);
                       //_SessionInformationIframe.IsActivePlayerLogo = _Customer[0].IsClientPlayerLogoActive;
                       //_SessionInformationIframe.PlayerLogoImage = _Customer[0].ClientPlayerLogoImage;

                       CommonFunctions.SetSessionInformationIframe(_SessionInformationIframe);

                       if (LoggedIn != null)
                       {
                           this.LoggedIn(this, new EventArgs());
                       }
                   }   
               }
               else
               {
                   lblError.Text = "User Name And\\Or Password is wrong.";
               }
            }
            catch (Exception _Exception)
            {
                lblError.Text = "Some error occurs, please try again later";
            }
        }
    }
}