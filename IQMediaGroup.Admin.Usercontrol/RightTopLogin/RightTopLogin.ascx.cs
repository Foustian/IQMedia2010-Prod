using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Core.Enumeration;
using IQMediaGroup.Admin.Controller.Interface;
using System.Configuration;


namespace IQMediaGroup.Admin.Usercontrol.RightTopLogin
{
    public partial class RightTopLogin : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        //static string _Logout = "Logout ";
        //static string _Login = "Login ";

        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (_SessionInformation != null && _SessionInformation.IsAdminLogin == true)
                {
                    lnkLogin.Visible = true;
                    //lnkLogin.Text = _Logout;
                    imgTopOne.Visible = true;
                    imgTopTwo.Visible = true;
                    tdImage.Visible = true;

                    //ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                    //List<CustomerClientRoleAccess> _ListOfCustomerClientRoleAccess = _ICustomerRoleController.GetCustomerClientRoleAccess(_SessionInformation.CustomerKey);

                    //if (_ListOfCustomerClientRoleAccess != null)
                    //{
                    //    foreach (CustomerClientRoleAccess _CustomerRoles in _ListOfCustomerClientRoleAccess)
                    //    {
                    //        if (_CustomerRoles.RoleName == RolesName.GlobalAdminAccess.ToString() && _CustomerRoles.RoleIsActive == true && _CustomerRoles.ClientAccess == true && _CustomerRoles.CustomerAccess == true)
                    //        {
                    //            lnkbtnGlobalAdmin.Visible = true;
                    //            lnkbtnGlobalAdmin.Enabled = true;
                    //        }
                    //    }
                    //}
                }
                //else
                //{
                //    lnkLogin.Visible = false;
                //    lnkLogin.Visible = true;
                //    imgTopOne.Visible = false;
                //    imgTopTwo.Visible = false;
                //    tdImage.Visible = false;
                //    //lnkbtnGlobalAdmin.Visible = false;

                //}
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

        #region "Button Events"
        protected void lnkLogin_Click(object sender, EventArgs e)
        {
            try
            {

                if (HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers) != null)
                {
                    List<CurrentUsers> _ListOfCurrentUsers = (List<CurrentUsers>)HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers);

                    CurrentUsers _ExistingUsers = _ListOfCurrentUsers.Find(delegate(CurrentUsers _User) { return _User.SessionID == HttpContext.Current.Session.SessionID; });

                    if (_ExistingUsers != null)
                    {
                        _ExistingUsers.IsActive = false;

                        _ListOfCurrentUsers.Remove(_ExistingUsers);
                    }
                }

                CommonConstants.IsLogout = true;
                Session.Abandon();
                //_SessionInformation = null;
                Response.Redirect("~/Login/", false);


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