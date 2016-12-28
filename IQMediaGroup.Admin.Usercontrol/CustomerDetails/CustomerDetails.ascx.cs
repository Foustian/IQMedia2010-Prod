using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Core.Enumeration;
using System.Data;
using System.Configuration;

namespace IQMediaGroup.Admin.Usercontrol.CustomerDetails
{
    public partial class CustomerDetails : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        
        
        private string _IsActive = string.Empty;
        private string _ClientName = string.Empty;
        IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation;
        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {

                _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
             
                lblNoResults.Visible = false;
                lblNoResults.Text = string.Empty;
                
                List<CustomerRoles> _ListOfCustomerRoles = _SessionInformation.CustomerRoles;
                if (_ListOfCustomerRoles != null)
                {
                    foreach (CustomerRoles _CustomerRoles in _ListOfCustomerRoles)
                    {
                        if (_CustomerRoles.RoleName == RolesName.GlobalAdminAccess.ToString() && _CustomerRoles.IsAccess == false)
                        {
                            Response.Redirect(CommonConstants.CustomErrorPage);
                        }
                    }
                }
                
                if (!IsPostBack)
                {
                    lblClient.Text = _SessionInformation.ClientName;
                    GetClientRoles();
                    BindAdminCustomer();
                }                

                
                

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

        #region "Private Methods"

        private void GetClientRoles()
        {
            
            IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
            List<ClientRoles> _ListOfClientRoles = _IClientRoleController.GetClientRoleByClientID(_SessionInformation.ClientID);
            
            ViewstateInformation _ViewstateInformation = GetViewstateInformation();

            _ViewstateInformation._ListOfClientRoles = _ListOfClientRoles;
            SetViewstateInformation(_ViewstateInformation);
            
        }

        private void ApplyGridHeaderFilter()
        {
            try
            {
                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                DropDownList ddlClientName = gvCustomer.HeaderRow.FindControl("ddlClientName") as DropDownList;

                

                List<Customer> _ListOfCustomer = new List<Customer>();
                List<Customer> _ListOfSelectedCustomer = new List<Customer>();

                // store selecte

                ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                _ViewstateInformation.VSClientNameGrid = ddlClientName.SelectedValue;

                this.SetViewstateInformation(_ViewstateInformation);


                _ListOfCustomer = _SessionInformation._ListOfAdminCustomer;

                if (ddlClientName.SelectedIndex == 0)
                {
                    _ListOfSelectedCustomer = _ListOfCustomer;
                }
                else
                {
                    var listOFName =
                                        from _Customer in _ListOfCustomer
                                        where _Customer.ClientID == Convert.ToInt32(ddlClientName.SelectedValue)
                                        select _Customer;
                    _ListOfSelectedCustomer = new List<Customer>(listOFName);
                }



                bool IsNoRecords = false;

                if (_ListOfSelectedCustomer.Count == 0)
                {
                    Customer _Customer = new Customer();
                    _ListOfSelectedCustomer.Add(_Customer);
                    IsNoRecords = true;
                }

                gvCustomer.DataSource = _ListOfSelectedCustomer;
                gvCustomer.DataBind();

                _SessionInformation._ListOfSelectedAdminCustomer = _ListOfSelectedCustomer;

                IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);

                if (gvCustomer.Rows.Count > 0)
                {
                    if (IsNoRecords == true)
                    {
                        gvCustomer.Rows[0].Visible = false;

                        lblNoResults.Text = CommonConstants.HTMLBreakLine + CommonConstants.NoResultsFound;
                        lblNoResults.Visible = true;
                    }
                }

                //_SessionInformation.SelectedClips = _ListOFSelectedClips;
                //IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);

                //upCustomer.Update();

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void BindAdminCustomer()
        {
            try
            {
                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                
                DataSet _ListOfCustomer = _ICustomerController.GetAllCustomerWithRoleByClientGUID(new Guid(_SessionInformation.ClientGUID));
                if (_ListOfCustomer.Tables[0].Rows.Count > 0)
                {
                    Panel1.Style.Add("display", "block");
                    gvCustomer.DataSource = _ListOfCustomer;
                    gvCustomer.DataBind();
                    
                }
                else
                {
                    Panel1.Style.Add("display", "none");
                    lblErrorMessage.Text = "No Customer found";
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private bool ValidateEdit(int EditIndex)
        {
            ViewstateInformation _ViewstateInformation = GetViewstateInformation();
            List<ClientRoles> _ListOfClientRoles = _ViewstateInformation._ListOfClientRoles;
            
            bool IsRoleSelected = false;
            bool IsValidDefaultPage = true;
            bool IsRoleCheck = false;
            DropDownList ddlDefaultPage =  (DropDownList)gvCustomer.Rows[EditIndex].FindControl("ddlDefaultPage");
            foreach (ClientRoles Role in _ListOfClientRoles)
            {
                CheckBox chkSelectRole = new CheckBox();


                if (Role.RoleName == RolesName.IQBasic.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkEIQBasic");
                }
                else if (Role.RoleName == RolesName.AdvancedSearchAccess.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkEAdvancedSearchAccess");
                }
                else if (Role.RoleName == RolesName.GlobalAdminAccess.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkEGlobalAdminAccess");
                }
                else if (Role.RoleName == RolesName.myIQAccess.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkEmyIQAccess");
                }
                else if (Role.RoleName == RolesName.IQAgentWebsiteAccess.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkEIQAgentWebsiteAccess");
                }
                else if (Role.RoleName == RolesName.DownloadClips.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkEDownloadClips");
                }
                else if (Role.RoleName == RolesName.IQCustomAccess.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkEIQCustomAccess");
                }
                else if (Role.RoleName == RolesName.UGCDownload.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkEUGCDownload");
                }
                else if (Role.RoleName == RolesName.UGCUploadEdit.ToString())
                {
                    chkSelectRole = (CheckBox)gvCustomer.Rows[EditIndex].FindControl("chkUGCUploadEdit");
                }
                


                if (chkSelectRole.Checked)
                {
                    IsRoleSelected = true;
                    
                }

                if (Role.RoleName == ddlDefaultPage.SelectedValue)
                {
                    if (!chkSelectRole.Checked)
                    {
                        IsRoleCheck = true;
                        IsValidDefaultPage = false;
                        
                    }
                }
            }

            if (!IsRoleSelected)
            {
                lblErrorMessage.Text = "Please Select Atleast One Role";
                return false;
            }

            if (!IsValidDefaultPage && IsRoleCheck)
            {
                lblErrorMessage.Text = "Selected default page has not given appropriate access right.";
                return false;
            }

            return true;
            
        }

        private List<string> GetDefaultPageList()
        {
            try
            {
                string _Pages = ConfigurationManager.AppSettings[CommonConstants.ConfigPages];
                List<string> _ListOfPages = _Pages.Split(',').ToList();
                return _ListOfPages;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region "Grid Events"
        protected void gvCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCustomer.PageIndex = e.NewPageIndex;
                BindAdminCustomer();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvCustomer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvCustomer.EditIndex = e.NewEditIndex;
                BindAdminCustomer();

                DropDownList ddlDefaultPage = gvCustomer.Rows[e.NewEditIndex].FindControl("ddlDefaultPage") as DropDownList;
                HiddenField hdnDefaultPage = gvCustomer.Rows[e.NewEditIndex].FindControl("hdnDefaultPage") as HiddenField;

                List<string> _ListOfPages = GetDefaultPageList();
                List<string> _PageTextValue = null;


                _ListOfPages.ForEach(delegate(string _Page)
                {
                    _PageTextValue = _Page.Split('|').ToList();
                    ddlDefaultPage.Items.Add(new ListItem(_PageTextValue[0], _PageTextValue[1]));
                    ddlDefaultPage.SelectedIndex = 0;
                });


                if (!string.IsNullOrEmpty(hdnDefaultPage.Value))
                {
                    ddlDefaultPage.SelectedIndex = -1;
                    ddlDefaultPage.Items.FindByText(hdnDefaultPage.Value).Selected = true; 
                }

                

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvCustomer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                lblErrorMessage.Text = "";
                gvCustomer.EditIndex = -1;
                BindAdminCustomer();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvCustomer_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                lblErrorMessage.Text = "";
                if (ValidateEdit(e.RowIndex))
                {
                    TextBox txtFirstName = (TextBox)gvCustomer.Rows[e.RowIndex].FindControl("txtFirstName");
                    TextBox txtLastName = (TextBox)gvCustomer.Rows[e.RowIndex].FindControl("txtLastName");
                    TextBox txtEmail = (TextBox)gvCustomer.Rows[e.RowIndex].FindControl("txtEmail");
                    TextBox txtPassword = (TextBox)gvCustomer.Rows[e.RowIndex].FindControl("txtPassword");
                    TextBox txtContactNo = (TextBox)gvCustomer.Rows[e.RowIndex].FindControl("txtContactNo");
                    TextBox txtComments = (TextBox)gvCustomer.Rows[e.RowIndex].FindControl("txtCustomerComment");
                    CheckBox ChkActive = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkStatus");
                    DropDownList ddlDefaultPage = (DropDownList)gvCustomer.Rows[e.RowIndex].FindControl("ddlDefaultPage");
                    int CustomerKey = Convert.ToInt32(gvCustomer.DataKeys[e.RowIndex].Value);

                    string _Result = string.Empty;
                    ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                    Customer _Customer = new Customer();
                    _Customer.CustomerKey = CustomerKey;
                    _Customer.FirstName = txtFirstName.Text.ToString();
                    _Customer.LastName = txtLastName.Text.ToString();
                    _Customer.Email = txtEmail.Text;
                    _Customer.Password = txtPassword.Text.ToString();
                    _Customer.Comment = txtComments.Text;
                    _Customer.ContactNo = txtContactNo.Text;
                    _Customer.ModifiedDate = DateTime.Now;
                    _Customer.IsActive = Convert.ToBoolean(ChkActive.Checked);
                    _Customer.DefaultPage = ddlDefaultPage.SelectedItem.Text;

                    int EmailCount = 0;
                    _Result = _ICustomerController.UpdateClientAdminCustomer(_Customer,out EmailCount);

                    if (EmailCount > 0)
                    {
                        e.Cancel = true;
                        lblErrorMessage.Text = "Email Address Already Exits. Please Try Another Email.";
                        return;
                    }
                    else if (_Result == "-1")
                    {
                        e.Cancel = true;
                        lblErrorMessage.Text = "Error Updating in Client, Please try Again";
                        return;
                    }
                    else
                    {
                        ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                        List<ClientRoles> _ListOfClientRoles = _ViewstateInformation._ListOfClientRoles;
                        for (int _RoleCount = 0; _RoleCount < _ListOfClientRoles.Count; _RoleCount++)
                        {
                            CheckBox chkSelectRole = new CheckBox();
                            IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                            List<Role> RolesId = _IRoleController.GetRoleName(_ListOfClientRoles[_RoleCount].RoleName);


                            if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.IQBasic.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkEIQBasic");
                            }
                            else if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.AdvancedSearchAccess.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkEAdvancedSearchAccess");
                            }
                            else if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.GlobalAdminAccess.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkEGlobalAdminAccess");
                            }
                            else if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.myIQAccess.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkEmyIQAccess");
                            }
                            else if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.IQAgentWebsiteAccess.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkEIQAgentWebsiteAccess");
                            }
                            else if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.DownloadClips.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkEDownloadClips");
                            }
                            else if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.IQCustomAccess.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkEIQCustomAccess");
                            }
                            else if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.UGCDownload.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkEUGCDownload");
                            }
                            else if (_ListOfClientRoles[_RoleCount].RoleName == RolesName.UGCUploadEdit.ToString())
                            {
                                chkSelectRole = (CheckBox)gvCustomer.Rows[e.RowIndex].FindControl("chkUGCUploadEdit");
                            }

                            
                            
                            
                            CustomerRoles _CustomerRoles = new CustomerRoles();
                            _CustomerRoles.CustomerID = Convert.ToInt32(CustomerKey);
                            _CustomerRoles.RoleID = Convert.ToInt32(RolesId[0].RoleID);
                            ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();

                            if (chkSelectRole.Checked == true)
                            {                               
                                string _Result1 = _ICustomerRoleController.InsertCustomerRole(_CustomerRoles);
                            }
                            else
                            {                               
                                string _Result1 = _ICustomerRoleController.UpdateCustomerRoleByClientIDRoleID(_CustomerRoles);
                            }
                        }
                        gvCustomer.EditIndex = -1;
                        BindAdminCustomer();


                    }
                }
                else
                {
                    e.Cancel = true;
                    return;

                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
           
        }

        protected void gvCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                try
                {
                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                    List<ClientRoles> _ListOfClientRoles = _ViewstateInformation._ListOfClientRoles;
                  
                    foreach (ClientRoles Role in _ListOfClientRoles)
                    {
                        if (e.Row.RowIndex != gvCustomer.EditIndex)
                        {

                            if (Role.RoleName == RolesName.IQBasic.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkIQBasic")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                            }
                            else if (Role.RoleName == RolesName.AdvancedSearchAccess.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkAdvancedSearchAccess")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                            }
                            else if (Role.RoleName == RolesName.GlobalAdminAccess.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkGlobalAdminAccess")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                            }
                            else if (Role.RoleName == RolesName.myIQAccess.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkmyIQAccess")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                            }
                            else if (Role.RoleName == RolesName.IQAgentWebsiteAccess.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkIQAgentWebsiteAccess")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                            }
                            else if (Role.RoleName == RolesName.DownloadClips.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkDownloadClips")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                            }
                            else if (Role.RoleName == RolesName.IQCustomAccess.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkIQCustomAccess")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                            }
                            else if (Role.RoleName == RolesName.UGCDownload.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkUGCDownload")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                            }
                            else if (Role.RoleName == RolesName.UGCUploadEdit.ToString())
                            {
                                ((((CheckBox)e.Row.FindControl("chkUGCUploadEdit")).Parent as DataControlFieldCell).ContainingField).Visible = true;
                                
                            }
                        }
                    }
                }
                catch (Exception _Exception)
                {
                    this.WriteException(_Exception);
                    Response.Redirect(CommonConstants.CustomErrorPage);
                }
                
            }
        }

        #endregion

        

    }
}