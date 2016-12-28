using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.Enumeration;
using System.Configuration;

namespace IQMediaGroup.Admin.Usercontrol.Login
{
    public partial class Login : System.Web.UI.UserControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        string _ErrorMessage = "User Name And\\Or Password is wrong.";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.SetFocus(txtUserName);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                List<Customer> _ListOfCustomerInformation = new List<Customer>();
                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();

                _ListOfCustomerInformation = _ICustomerController.CheckAuthentication(txtUserName.Text, txtPassword.Text);

                if (_ListOfCustomerInformation.Count > 0)
                {
                    _SessionInformation.IsAdminLogin = true;
                    _SessionInformation.AdminUserKey = _ListOfCustomerInformation[0].CustomerKey;
                    _SessionInformation.ClientGUID = _ListOfCustomerInformation[0].ClientGUID;
                    _SessionInformation.ClientName = _ListOfCustomerInformation[0].ClientName;
                    _SessionInformation.ClientID = _ListOfCustomerInformation[0].ClientID;
                    if (_ListOfCustomerInformation[0].ClientGUID.ToLower() == ConfigurationManager.AppSettings[CommonConstants.ConfigIQMediaClientGUID].ToLower())
                    {
                        _SessionInformation.IsIQMediaAdmin = true;
                    }
                    else
                    {
                        _SessionInformation.IsIQMediaAdmin = false;
                    }

                    IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
                    ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                    List<CustomerClientRoleAccess> _ListOfCustomerClientRoleAccess = _ICustomerRoleController.GetCustomerClientRoleAccess(_SessionInformation.AdminUserKey);

                    if (_ListOfCustomerClientRoleAccess != null)
                    {
                        bool IsAccess = false;
                        foreach (CustomerClientRoleAccess _CustomerRoles in _ListOfCustomerClientRoleAccess)
                        {
                            if (_CustomerRoles.RoleName == RolesName.GlobalAdminAccess.ToString() && _CustomerRoles.RoleIsActive == true && _CustomerRoles.ClientAccess == true && _CustomerRoles.CustomerAccess == true)
                            {
                                IsAccess = true;
                                break;
                            }
                           
                        }
                        if (IsAccess == true)
                        {
                            if (_SessionInformation.IsIQMediaAdmin)
                            {
                                Response.Redirect("~/CustomerRegistration/", false);
                            }
                            else
                            {
                                Response.Redirect("~/CustomerDetails/",false);
                            }
                            
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = _ErrorMessage;
                            txtUserName.Text = string.Empty;
                        }
                    }
                   
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = _ErrorMessage;
                    txtUserName.Text = string.Empty;
                }
            }
            catch (Exception _Exception)
            {
                
                throw _Exception;
            }
        }
    }
}