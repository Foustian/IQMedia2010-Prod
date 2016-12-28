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

namespace IQMediaGroup.Admin.Usercontrol.ClientRole
{
    public partial class ClientRole : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private string _ExistMessage = "Client Role Already Exists.";
        private string _InsertMessage = "Client Role Inserted Successfully.";
        private string _IsActive = string.Empty;

        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                #region Set Bread Crumb

                GenerateBreadCrumb("Role Mapping > Client Role Mapping");

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
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    BindClient();
                    BindRole();
                    BindClientRole();
                }
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
            lblMessage.Text = string.Empty;
            try
            {
                ClientRoles _ClientRoles = new ClientRoles();
                _ClientRoles.ClientID = Convert.ToInt32(ddlClient.SelectedValue);
                _ClientRoles.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                string _Result = _IClientRoleController.InsertClientRole(_ClientRoles);
                BindClientRole();
                ddlClient.SelectedIndex = 0;
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

        protected void gvClientRole_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Label lblStatus = (Label)gvClientRole.Rows[e.NewEditIndex].FindControl("lblStatus");
                _IsActive = lblStatus.Text;
                gvClientRole.EditIndex = e.NewEditIndex;
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

        protected void gvClientRole_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvClientRole.EditIndex = -1;
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

        protected void gvClientRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                DropDownList ddlClientName = gvClientRole.HeaderRow.FindControl("ddlClientName") as DropDownList;
                DropDownList ddlRoleName = gvClientRole.HeaderRow.FindControl("ddlRoleName") as DropDownList;

                List<ClientRoles> _ListOfClientRoles = new List<ClientRoles>();

                if (ddlRoleName.SelectedIndex == 0 && ddlClientName.SelectedIndex == 0)
                {

                    if (_SessionInformation._ClientRoles != null)
                    {
                        _ListOfClientRoles = (List<ClientRoles>)_SessionInformation._ClientRoles;
                    }
                }
                else
                {
                    if (_SessionInformation._ListOfSelectedClientRoles != null)
                    {

                        _ListOfClientRoles = (List<ClientRoles>)_SessionInformation._ListOfSelectedClientRoles;
                    }
                }

                gvClientRole.PageIndex = e.NewPageIndex;
                gvClientRole.DataSource = _ListOfClientRoles;
                gvClientRole.DataBind();

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

        protected void gvClientRole_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                ClientRoles _ClientRoles = new ClientRoles();
                HiddenField hdnClientRoleKey = (HiddenField)gvClientRole.Rows[e.RowIndex].FindControl("hdnClientRoleKey");
                DropDownList ddlStatus = (DropDownList)gvClientRole.Rows[e.RowIndex].FindControl("ddlStatus");
                _ClientRoles.ClientRoleKey = Convert.ToInt32(hdnClientRoleKey.Value);
                if (ddlStatus.SelectedValue == "1")
                {
                    _IsActive = "Flase";
                    _ClientRoles.IsAccess = false;
                }
                else
                {
                    _IsActive = "True";
                    _ClientRoles.IsAccess = true;
                }

                 IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                 string retValue= _IClientRoleController.UpdateClientRole(_ClientRoles);
                 gvClientRole.EditIndex = -1;
                 BindClientRole();
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

        protected void gvClientRole_DataBound(object sender, EventArgs e)
        {
            try
            {
                BindRoleToGrid();
                BindClientToGrid();
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }



        #endregion

        #region "DropDown Events"
        protected void ddlClientGrid_PreRender(object sender, EventArgs e)
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

        protected void ddlStatus_PreRender(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlStatus = sender as DropDownList;

                ddlStatus.Items.Clear();

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

        #endregion

        #region "Private Methods"

        private void ApplyGridHeaderFilter()
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                DropDownList ddlClientName = gvClientRole.HeaderRow.FindControl("ddlClientName") as DropDownList;
                DropDownList ddlRoleName = gvClientRole.HeaderRow.FindControl("ddlRoleName") as DropDownList;


                List<ClientRoles> _ListOfClientRoles = new List<ClientRoles>();
                List<ClientRoles> _ListOfSelectedClientRoles = new List<ClientRoles>();

                // store selecte

                ViewstateInformation _ViewstateInformation = this.GetViewstateInformation();

                _ViewstateInformation.VSRoleName = ddlRoleName.SelectedValue;
                _ViewstateInformation.VSClientNameGrid = ddlClientName.SelectedValue;

                this.SetViewstateInformation(_ViewstateInformation);


                _ListOfClientRoles = _SessionInformation._ClientRoles;
                
                if (ddlRoleName.SelectedIndex == 0 && ddlClientName.SelectedIndex == 0)
                {
                    _ListOfSelectedClientRoles = _ListOfClientRoles;
                }

                else if (ddlClientName.SelectedIndex > 0 && ddlRoleName.SelectedIndex == 0)
                {
                    var listOFName =
                                        from _ClientRoles in _ListOfClientRoles
                                        where _ClientRoles.ClientID == Convert.ToInt32(ddlClientName.SelectedValue)
                                        select _ClientRoles;
                    _ListOfSelectedClientRoles = new List<ClientRoles>(listOFName);
                }

                else if (ddlClientName.SelectedIndex == 0 && ddlRoleName.SelectedIndex > 0)
                {
                    var listOFName =
                                        from _ClientRoles in _ListOfClientRoles
                                        where _ClientRoles.RoleID == Convert.ToInt32(ddlRoleName.SelectedValue)
                                        select _ClientRoles;
                    _ListOfSelectedClientRoles = new List<ClientRoles>(listOFName);
                }

                else
                {
                    var listOFName =
                                        from _ClientRoles in _ListOfClientRoles
                                        where _ClientRoles.ClientID == Convert.ToInt32(ddlClientName.SelectedValue) &&  _ClientRoles.RoleID == Convert.ToInt32(ddlRoleName.SelectedValue)
                                        select _ClientRoles;
                    _ListOfSelectedClientRoles = new List<ClientRoles>(listOFName);
                }



                bool IsNoRecords = false;

                if (_ListOfSelectedClientRoles.Count == 0)
                {
                    ClientRoles _ClientRoles = new ClientRoles();
                    _ListOfSelectedClientRoles.Add(_ClientRoles);
                    IsNoRecords = true;
                }
                gvClientRole.DataSource = _ListOfSelectedClientRoles;
                gvClientRole.DataBind();

                if (gvClientRole.Rows.Count > 0)
                {
                    if (IsNoRecords == true)
                    {
                        gvClientRole.Rows[0].Visible = false;

                        lblNoResults.Text = CommonConstants.HTMLBreakLine + CommonConstants.NoResultsFound;
                        lblNoResults.Visible = true;
                    }
                }

                _SessionInformation._ListOfSelectedClientRoles = _ListOfSelectedClientRoles;
                IQMediaGroup.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);


            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        
        
        /// <summary>
        /// Description:This method will bind Client.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindClient()
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClient = _IClientController.GetClientInformation(true);

                ddlClient.DataSource = _ListOfClient;
                ddlClient.DataTextField = "ClientName";
                ddlClient.DataValueField = "ClientKey";
                ddlClient.DataBind();
                ddlClient.Items.Insert(0, new ListItem("Select Client Name", "0"));
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        private void BindClientToGrid()
        {
            try
            {
                if (gvClientRole.Rows.Count > 0)
                {

                    IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                    DropDownList ddlClientName = (DropDownList)gvClientRole.HeaderRow.FindControl("ddlClientName");

                    IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                    List<Client> _ListOfClient = _IClientController.GetClientInformation(true);
                    if (_ListOfClient.Count > 0)
                    {
                        ddlClientName.DataSource = _ListOfClient;
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


        private void BindRoleToGrid()
        {
            try
            {
                if (gvClientRole.Rows.Count > 0)
                {

                    IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                    DropDownList ddlRoleName = (DropDownList)gvClientRole.HeaderRow.FindControl("ddlRoleName");

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

        /// <summary>
        /// Description:This method will bind ClientRole Grid.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindClientRole()
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                List<ClientRoles> _ListOfClientRole = _IClientRoleController.GetClientRoleAdmin();
                if (_ListOfClientRole.Count > 0)
                {
                    gvClientRole.DataSource = _ListOfClientRole;
                    gvClientRole.DataBind();
                }

                _SessionInformation._ClientRoles = _ListOfClientRole;

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