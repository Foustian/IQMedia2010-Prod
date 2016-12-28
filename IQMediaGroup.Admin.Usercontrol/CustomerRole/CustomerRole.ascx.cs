using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Admin.Usercontrol.CustomerRole
{
    public partial class CustomerRole : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private string _ExistMessage = "Customer Role Already Exists.";
        private string _InsertMessage = "Customer Role Inserted Successfully.";
        private string _IsActive = string.Empty;
        private string _ClientName = string.Empty;
        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                #region Set Bread Crumb

                GenerateBreadCrumb("Role Mapping > Customer Role Mapping");


                #endregion
                lblNoResults.Visible = false;
                lblNoResults.Text = string.Empty;
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
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
                lblMessage.Visible = false;
                if (!IsPostBack)
                {
                    BindCustomer();
                    BindRole();
                    BindCustomerRole();
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

        #region "Button Events"
        protected void btnAdd_Click(object sender, EventArgs e)
        {


            try
            {
                CustomerRoles _CustomerRoles = new CustomerRoles();
                _CustomerRoles.CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
                _CustomerRoles.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                string _Result = _ICustomerRoleController.InsertCustomerRole(_CustomerRoles);
                BindCustomerRole();
                ddlCustomer.SelectedIndex = 0;
                ddlRole.SelectedIndex = 0;
                if (_Result == "0")
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = _ExistMessage;
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = _InsertMessage;
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

        #region "Grid Events"
        protected void gvCustomerRole_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Label lblStatus = (Label)gvCustomerRole.Rows[e.NewEditIndex].FindControl("lblStatus");
                _IsActive = lblStatus.Text;

                gvCustomerRole.EditIndex = e.NewEditIndex;
                ApplyGridHeaderFilter();
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

        protected void gvCustomerRole_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvCustomerRole.EditIndex = -1;
                //BindCustomerRole();
                ApplyGridHeaderFilter();
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

        protected void gvCustomerRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                DropDownList ddlCustomerName = (DropDownList)gvCustomerRole.HeaderRow.FindControl("ddlCustomerName");
                DropDownList ddlRoleName = (DropDownList)gvCustomerRole.HeaderRow.FindControl("ddlRoleName");
                DropDownList ddlClientName = (DropDownList)gvCustomerRole.HeaderRow.FindControl("ddlClientName");
                List<CustomerRoles> _ListOfCustomerRoles = new List<CustomerRoles>();

                if (ddlCustomerName.SelectedIndex == 0 && ddlRoleName.SelectedIndex == 0 && ddlClientName.SelectedIndex == 0)
                {

                    if (_SessionInformation._CustomerRoles != null)
                    {
                        _ListOfCustomerRoles = (List<CustomerRoles>)_SessionInformation._CustomerRoles;
                    }
                }
                else
                {
                    if (_SessionInformation._ListOfSelectedCustomerRoles != null)
                    {

                        _ListOfCustomerRoles = (List<CustomerRoles>)_SessionInformation._ListOfSelectedCustomerRoles;
                    }
                }

                gvCustomerRole.PageIndex = e.NewPageIndex;
                gvCustomerRole.DataSource = _ListOfCustomerRoles;
                gvCustomerRole.DataBind();

                //gvCustomerRole.PageIndex = e.NewPageIndex;
                //BindCustomerRole();
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

        protected void gvCustomerRole_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                CustomerRoles _CustomerRoles = new CustomerRoles();
                HiddenField hdnCustomerRoleKey = (HiddenField)gvCustomerRole.Rows[e.RowIndex].FindControl("hdnCustomerRoleKey");
                DropDownList ddlStatus = (DropDownList)gvCustomerRole.Rows[e.RowIndex].FindControl("ddlStatus");
                _CustomerRoles.CustomerRoleKey = Convert.ToInt32(hdnCustomerRoleKey.Value);
                if (ddlStatus.SelectedValue == "1")
                {
                    _IsActive = "False";
                    _CustomerRoles.IsAccess = false;
                }
                else
                {
                    _IsActive = "True";
                    _CustomerRoles.IsAccess = true;
                }
                ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                _ICustomerRoleController.UpdateCustomerRole(_CustomerRoles);
                gvCustomerRole.EditIndex = -1;
                BindCustomerRole();
                ApplyGridHeaderFilter();
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

        protected void gvCustomerRole_DataBound(object sender, EventArgs e)
        {
            try
            {
                BindCustomerToGrid();
                BindRoleToGrid();
                BindClientToGrid();
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }


        private void BindCustomerToGrid()
        {
            try
            {
                if (gvCustomerRole.Rows.Count > 0)
                {

                    IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                    DropDownList ddlCustomerName = (DropDownList)gvCustomerRole.HeaderRow.FindControl("ddlCustomerName");

                    ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                    List<Customer> _ListOfCustomer = _ICustomerController.GetAllCustomers();

                    if (_ListOfCustomer.Count > 0)
                    {
                        //_SessionInformation.FilteredUser = _ListOfCustomer;
                        ddlCustomerName.DataSource = _ListOfCustomer;
                        ddlCustomerName.DataTextField = "FullName";
                        ddlCustomerName.DataValueField = "CustomerKey";
                        ddlCustomerName.DataBind();
                        ddlCustomerName.Items.Insert(0, new ListItem("All Customers", "0"));
                        //IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
                    }

                    ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                    if (!string.IsNullOrEmpty(_ViewstateInformation.VSCustomerName))
                    {
                        ddlCustomerName.SelectedValue = _ViewstateInformation.VSCustomerName;
                    }
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

        private void BindRoleToGrid()
        {
            try
            {
                if (gvCustomerRole.Rows.Count > 0)
                {

                    IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                    DropDownList ddlRoleName = (DropDownList)gvCustomerRole.HeaderRow.FindControl("ddlRoleName");

                    IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                    List<Role> _ListOfRole = _IRoleController.GetRoleInformation(true);

                    if (_ListOfRole.Count > 0)
                    {
                        ddlRoleName.DataSource = _ListOfRole;
                        ddlRoleName.DataTextField = "RoleName";
                        ddlRoleName.DataValueField = "RoleID";
                        ddlRoleName.DataBind();
                        ddlRoleName.Items.Insert(0, new ListItem("All Roles", "0"));
                    }

                    ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                    if (!string.IsNullOrEmpty(_ViewstateInformation.VSRoleName))
                    {
                        ddlRoleName.SelectedValue = _ViewstateInformation.VSRoleName;
                    }
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

        private void BindClientToGrid()
        {
            try
            {
                if (gvCustomerRole.Rows.Count > 0)
                {

                    IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                    DropDownList ddlClientName = (DropDownList)gvCustomerRole.HeaderRow.FindControl("ddlClientName");

                    IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                    List<Client> _ListOfClients = _IClientController.GetClientInformation(true);

                    if (_ListOfClients.Count > 0)
                    {
                        ddlClientName.DataSource = _ListOfClients;
                        ddlClientName.DataTextField = "ClientName";
                        ddlClientName.DataValueField = "ClientKey";
                        ddlClientName.DataBind();
                        ddlClientName.Items.Insert(0, new ListItem("All Clients", "0"));
                    }
                    ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                    if (!string.IsNullOrEmpty(_ViewstateInformation.VSClientNameGrid))
                    {
                        ddlClientName.SelectedValue = _ViewstateInformation.VSClientNameGrid;
                    }
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

        #region "DropDown Events"
        protected void ddlCustomerGrid_PreRender(object sender, EventArgs e)
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClients = _IClientController.GetClientInformation(true);
                DropDownList ddlClientNameGrid = sender as DropDownList;
                ddlClientNameGrid.DataSource = _ListOfClients;
                ddlClientNameGrid.DataTextField = "ClientName";
                ddlClientNameGrid.DataValueField = "ClientKey";
                ddlClientNameGrid.DataBind();
                ddlClientNameGrid.Items.Insert(0, new ListItem("Select", "0"));
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

        protected void ddlRoleGrid_PreRender(object sender, EventArgs e)
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClients = _IClientController.GetClientInformation(null);
                DropDownList ddlClientNameGrid = sender as DropDownList;
                ddlClientNameGrid.DataSource = _ListOfClients;
                ddlClientNameGrid.DataTextField = "ClientName";
                ddlClientNameGrid.DataValueField = "ClientKey";
                ddlClientNameGrid.DataBind();
                ddlClientNameGrid.Items.Insert(0, new ListItem("Select", "0"));
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

        protected void ddlStatus_PreRender(object sender, EventArgs e)
        {
            try
            {

                DropDownList ddlStatus = sender as DropDownList;
                ddlStatus.Items.Insert(0, new ListItem("Select Status", "-1"));
                ddlStatus.Items.Insert(1, new ListItem("True", "0"));
                ddlStatus.Items.Insert(2, new ListItem("False", "1"));

                if (_IsActive == "True")
                    ddlStatus.SelectedIndex = 1;
                else
                    ddlStatus.SelectedIndex = 2;
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

        protected void ddlCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ApplyGridHeaderFilter();
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

        protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ApplyGridHeaderFilter();
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


        protected void ddlClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ApplyGridHeaderFilter();
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

        protected void ddlClientNameGrid_PreRender(object sender, EventArgs e)
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClients = _IClientController.GetClientInformation(true);
                DropDownList ddlClientNameGrid = sender as DropDownList;
                ddlClientNameGrid.DataSource = _ListOfClients;
                ddlClientNameGrid.DataTextField = "ClientName";
                ddlClientNameGrid.DataValueField = "ClientKey";
                ddlClientNameGrid.DataBind();
                ddlClientNameGrid.Items.Insert(0, new ListItem("Select", "0"));
                int _Index = 0;
                bool _SelectStatus = false;

                ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                if (_ViewstateInformation != null && !string.IsNullOrEmpty(_ViewstateInformation.VSClientName))
                {
                    _ClientName = _ViewstateInformation.VSClientName;
                }

                foreach (Client _Clients in _ListOfClients)
                {
                    if (_Clients.ClientName == _ClientName)
                    {
                        _SelectStatus = true;
                        break;
                    }
                    _Index++;
                }

                if (_SelectStatus == true)
                {
                    ddlClientNameGrid.SelectedIndex = _Index + 1;
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

        private void ApplyGridHeaderFilter()
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                DropDownList ddlCustomerName = gvCustomerRole.HeaderRow.FindControl("ddlCustomerName") as DropDownList;
                DropDownList ddlRoleName = gvCustomerRole.HeaderRow.FindControl("ddlRoleName") as DropDownList;
                DropDownList ddlClientName = gvCustomerRole.HeaderRow.FindControl("ddlClientName") as DropDownList;

                List<CustomerRoles> _ListOfCustomerRoles = new List<CustomerRoles>();
                List<CustomerRoles> _ListOfSelectedCustomerRoles = new List<CustomerRoles>();

                // store selecte

                ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                _ViewstateInformation.VSCustomerName = ddlCustomerName.SelectedValue;
                _ViewstateInformation.VSRoleName = ddlRoleName.SelectedValue;
                _ViewstateInformation.VSClientNameGrid = ddlClientName.SelectedValue;

                this.SetViewstateInformation(_ViewstateInformation);


                _ListOfCustomerRoles = _SessionInformation._CustomerRoles;
                if (ddlCustomerName.SelectedIndex == 0 && ddlRoleName.SelectedIndex == 0 && ddlClientName.SelectedIndex == 0)
                {
                    _ListOfSelectedCustomerRoles = _ListOfCustomerRoles;
                }

                else if (ddlCustomerName.SelectedIndex > 0 && ddlRoleName.SelectedIndex == 0 && ddlClientName.SelectedIndex == 0)
                {
                    var listOFName =
                                        from _CustomerRoles in _ListOfCustomerRoles
                                        where _CustomerRoles.CustomerID == Convert.ToInt32(ddlCustomerName.SelectedValue)
                                        select _CustomerRoles;
                    _ListOfSelectedCustomerRoles = new List<CustomerRoles>(listOFName);
                }

                else if (ddlCustomerName.SelectedIndex == 0 && ddlRoleName.SelectedIndex > 0 && ddlClientName.SelectedIndex == 0)
                {
                    var listOFName =
                                        from _CustomerRoles in _ListOfCustomerRoles
                                        where _CustomerRoles.RoleID == Convert.ToInt32(ddlRoleName.SelectedValue)
                                        select _CustomerRoles;
                    _ListOfSelectedCustomerRoles = new List<CustomerRoles>(listOFName);
                }

                else if (ddlCustomerName.SelectedIndex == 0 && ddlRoleName.SelectedIndex == 0 && ddlClientName.SelectedIndex > 0)
                {
                    var listOFName =
                                        from _CustomerRoles in _ListOfCustomerRoles
                                        where _CustomerRoles.ClientID == Convert.ToInt32(ddlClientName.SelectedValue)
                                        select _CustomerRoles;
                    _ListOfSelectedCustomerRoles = new List<CustomerRoles>(listOFName);
                }

                else if (ddlCustomerName.SelectedIndex > 0 && ddlRoleName.SelectedIndex > 0 && ddlClientName.SelectedIndex == 0)
                {
                    var listOFName =
                                        from _CustomerRoles in _ListOfCustomerRoles
                                        where _CustomerRoles.CustomerID == Convert.ToInt32(ddlCustomerName.SelectedValue) && _CustomerRoles.RoleID == Convert.ToInt32(ddlRoleName.SelectedValue)
                                        select _CustomerRoles;
                    _ListOfSelectedCustomerRoles = new List<CustomerRoles>(listOFName);
                }

                else if (ddlCustomerName.SelectedIndex > 0 && ddlRoleName.SelectedIndex == 0 && ddlClientName.SelectedIndex > 0)
                {
                    var listOFName =
                                        from _CustomerRoles in _ListOfCustomerRoles
                                        where _CustomerRoles.CustomerID == Convert.ToInt32(ddlCustomerName.SelectedValue) && _CustomerRoles.ClientID == Convert.ToInt32(ddlClientName.SelectedValue)
                                        select _CustomerRoles;
                    _ListOfSelectedCustomerRoles = new List<CustomerRoles>(listOFName);
                }

                else if (ddlCustomerName.SelectedIndex == 0 && ddlRoleName.SelectedIndex > 0 && ddlClientName.SelectedIndex > 0)
                {
                    var listOFName =
                                        from _CustomerRoles in _ListOfCustomerRoles
                                        where _CustomerRoles.RoleID == Convert.ToInt32(ddlRoleName.SelectedValue) && _CustomerRoles.ClientID == Convert.ToInt32(ddlClientName.SelectedValue)
                                        select _CustomerRoles;
                    _ListOfSelectedCustomerRoles = new List<CustomerRoles>(listOFName);
                }

                else
                {
                    var listOFName =
                                        from _CustomerRoles in _ListOfCustomerRoles
                                        where _CustomerRoles.CustomerID == Convert.ToInt32(ddlCustomerName.SelectedValue) && _CustomerRoles.RoleID == Convert.ToInt32(ddlRoleName.SelectedValue) && _CustomerRoles.ClientID == Convert.ToInt32(ddlClientName.SelectedValue)
                                        select _CustomerRoles;
                    _ListOfSelectedCustomerRoles = new List<CustomerRoles>(listOFName);
                }



                bool IsNoRecords = false;

                if (_ListOfSelectedCustomerRoles.Count == 0)
                {
                    CustomerRoles _CustomerRoles = new CustomerRoles();
                    _ListOfSelectedCustomerRoles.Add(_CustomerRoles);
                    IsNoRecords = true;
                }

                gvCustomerRole.DataSource = _ListOfSelectedCustomerRoles;
                gvCustomerRole.DataBind();

                _SessionInformation._ListOfSelectedCustomerRoles = _ListOfSelectedCustomerRoles;

                IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);

                if (gvCustomerRole.Rows.Count > 0)
                {
                    if (IsNoRecords == true)
                    {
                        gvCustomerRole.Rows[0].Visible = false;

                        lblNoResults.Text = CommonConstants.HTMLBreakLine + CommonConstants.NoResultsFound;
                        lblNoResults.Visible = true;
                    }
                }

                //_SessionInformation.SelectedClips = _ListOFSelectedClips;
                //IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
                upClient.Update();

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        /// <summary>
        /// Description:This method will bind Customer.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindCustomer()
        {
            try
            {
                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                List<Customer> _ListOfCustomer = _ICustomerController.GetAllCustomers();

                if (_ListOfCustomer.Count > 0)
                {
                    ddlCustomer.DataSource = _ListOfCustomer;
                    ddlCustomer.DataTextField = "FullName";
                    ddlCustomer.DataValueField = "CustomerKey";
                    ddlCustomer.DataBind();
                    ddlCustomer.Items.Insert(0, new ListItem("Select Customer Name", "0"));
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will bind Roles.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindRole()
        {
            try
            {
                IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                List<Role> _ListOfRole = _IRoleController.GetRoleInformation(true);

                if (_ListOfRole.Count > 0)
                {
                    ddlRole.DataSource = _ListOfRole;
                    ddlRole.DataTextField = "RoleName";
                    ddlRole.DataValueField = "RoleID";
                    ddlRole.DataBind();
                    ddlRole.Items.Insert(0, new ListItem("Select Role Name", "0"));
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will bind CustomerRole Grid.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindCustomerRole()
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                List<CustomerRoles> _ListOfCustomerRole = _ICustomerRoleController.GetCustomerRoleAdmin();

                if (_ListOfCustomerRole.Count > 0)
                {
                    gvCustomerRole.DataSource = _ListOfCustomerRole;
                    gvCustomerRole.DataBind();
                }
                _SessionInformation._CustomerRoles = _ListOfCustomerRole;

                IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion


    }
}