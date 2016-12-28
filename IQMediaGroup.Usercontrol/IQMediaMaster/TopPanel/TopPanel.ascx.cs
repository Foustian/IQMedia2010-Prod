using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.TopPanel
{
    public partial class TopPanel : System.Web.UI.UserControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

            if (HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers) != null)
            {
                List<CurrentUsers> _ListOfCurrentUsers = (List<CurrentUsers>)HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers);

                if (_SessionInformation != null && _SessionInformation.IsLogIn == true)
                {
                    CurrentUsers _ExistingUsers = _ListOfCurrentUsers.Find(delegate(CurrentUsers _User) { return _User.SessionID == HttpContext.Current.Session.SessionID && _User.CustomerKey == _SessionInformation.CustomerKey; });

                    if (_ExistingUsers != null && _ExistingUsers.IsActive == false)
                    {
                        Session.Abandon();
                        _SessionInformation = null;
                        lblMsg.Text = CommonConstants.LogOutMsg;
                        _ListOfCurrentUsers.Remove(_ExistingUsers);
                        mpESessionOut.Show();
                    }
                }

            }

            /*if (!IQMedia.Web.Common.Authentication.IsAuthenticated)
            {
                ucLogin.Visible = true;
                ucLogout.Visible = false;

            }
            else
            {
                var currentUser = IQMedia.Web.Common.Authentication.CurrentUser;
                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                List<Customer> listofCustomer = _ICustomerController.GetCustomerByCustomerGUIDForAuthentication(currentUser.Guid);


                List<Role> _ListOfRole = new List<Role>();
                Role _Role = new Role();
                int _CustomerID = listofCustomer[0].CustomerKey;
                IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                _ListOfRole = _IRoleController.GetRoleName(_CustomerID);



                if (listofCustomer != null && listofCustomer.Count > 0)
                {

                    bool hasRole = CommonFunctions.SetSessionInformationByFormAuthentication(listofCustomer[0], _ListOfRole);
                    if (!hasRole)
                    {
                        Response.Redirect("~/NoRole/", false);
                    }
                    ucLogin.Visible = false;
                    ucLogout.Visible = true;
                }
            }*/

            if (_SessionInformation != null && _SessionInformation.IsLogIn == true)
            {
                ucLogin.Visible = false;
                ucLogout.Visible = true;
            }
            else
            {
                ucLogout.Visible = false;
                ucLogin.Visible = true;
            }

            //hlogo.HRef = "~/";
        }
    }
}