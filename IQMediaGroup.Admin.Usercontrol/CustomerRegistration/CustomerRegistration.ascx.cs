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

namespace IQMediaGroup.Admin.Usercontrol.CustomerRegistration
{
    public partial class CustomerRegistration : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private string _ExistMessage = "Customer Already Exists.";
        private string _InsertMessage = "Customer Inserted Successfully.";
        private string _IsActive = string.Empty;
        private string _ClientName = string.Empty;
        private string _ErrorMessage = "Customer is already exists.";

        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {

                #region Set Bread Crumb

                GenerateBreadCrumb("Registration > Customer Registration");

                #endregion

                if (!string.IsNullOrEmpty(txtClientName.Text.Trim()))
                {
                    btnAdd.Enabled = true;
                }
                string _ClientString = BindAllClient();
                txtClientName.Attributes.Add("autocomplete", "off");
                txtClientName.Attributes.Add("onKeyUp", "ChangeButton('" + _ClientString + "')");
                //txtClientName.Attributes.Add("onkeypress", "clickButton()");
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "test", "ChangeButton('" + _ClientString + "')", true);
                lblNoResults.Visible = false;
                lblNoResults.Text = string.Empty;
                lblErrorMessageRole.Text = string.Empty;
                lblInvalidCustomer.Text = string.Empty;
                //Page.SetFocus(txtFirstName);
                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
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
                    //BindCustomer();
                    //BindClient();
                    txtSetupDate.Text = DateTime.Now.ToString();
                    BindRole();
                    BindDefaultPage();
                }

                lblErrorMessage.Visible = false;
                lblErrorMessageGUID.Text = string.Empty;

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

        private void BindDefaultPage()
        {
            try
            {
                string _Pages = ConfigurationManager.AppSettings[CommonConstants.ConfigPages];
                List<string> _ListOfPages = _Pages.Split(',').ToList();
                List<string> _PageTextValue = null;

                ddlDefaultPage.Items.Clear();

                _ListOfPages.ForEach(delegate(string _Page)
                                        {
                                            _PageTextValue = _Page.Split('|').ToList();
                                            ddlDefaultPage.Items.Add(new ListItem(_PageTextValue[0], _PageTextValue[1]));
                                            ddlDefaultPage.SelectedIndex = 0;
                                        });

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion

        #region "Private Methods"


        private string BindAllClient()
        {
            try
            {
                string _str = string.Empty;
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClients = _IClientController.GetClientInformation(true);

                if (_ListOfClients.Count > 0)
                {
                    //ddlClientName.DataSource = _ListOfClients;
                    //ddlClientName.DataTextField = "ClientName";
                    //ddlClientName.DataValueField = "ClientKey";
                    //ddlClientName.DataBind();
                    //ddlClientName.Items.Insert(0, new ListItem("Select Client Name", "0"));
                    bool _bool = false;
                    foreach (Client _Client in _ListOfClients)
                    {
                        if (_bool == false)
                        {
                            _str = _Client.ClientName;
                            _bool = true;
                        }
                        else
                        {
                            _str = _str + "," + _Client.ClientName;
                        }
                    }


                }
                return _str;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Description:This method will bind All Customer.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindCustomer()
        {
            try
            {
                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                List<Customer> _ListOfCustomer = _ICustomerController.GetAllCustomers();

                if (_ListOfCustomer.Count > 0)
                {
                    gvCustomer.DataSource = _ListOfCustomer;
                    gvCustomer.DataBind();
                    _SessionInformation._ListOfAdminCustomer = _ListOfCustomer;
                    IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindRole()
        {
            try
            {
                IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                List<Role> _ListOfRole = _IRoleController.GetRoleInformation(true);
                var ClientRoles = (from Roles in _ListOfRole
                                   where
                                       Roles.RoleName != RolesName.IframeMicrosite.ToString() &&
                                       Roles.RoleName != RolesName.NielsenData.ToString() &&
                                       Roles.RoleName != RolesName.CompeteData.ToString() &&
                                       Roles.RoleName != RolesName.MicrositeDownload.ToString()
                                   select Roles).ToList();

                if (_ListOfRole.Count > 0)
                {
                    rptRoles.DataSource = ClientRoles;
                    rptRoles.DataBind();
                    for (int _RoleCount = 0; _RoleCount < rptRoles.Items.Count; _RoleCount++)
                    {

                        Label lblRole = (Label)rptRoles.Items[_RoleCount].FindControl("lblRole");
                        CheckBox chkSelectRole = (CheckBox)rptRoles.Items[_RoleCount].FindControl("chkSelectRole");
                        chkSelectRole.Checked = true;
                    }
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        /// <summary>
        /// Description:This method will clear all fields.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void ClearFields()
        {
            try
            {
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";
                txtComments.Text = "";
                txtContactNo.Text = "";
                //ddlClientName.SelectedIndex = 0;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will bind All Clients.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindClient()
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClients = _IClientController.GetClientInformation(true);

                if (_ListOfClients.Count > 0)
                {
                    //ddlClientName.DataSource = _ListOfClients;
                    //ddlClientName.DataTextField = "ClientName";
                    //ddlClientName.DataValueField = "ClientKey";
                    //ddlClientName.DataBind();
                    //ddlClientName.Items.Insert(0, new ListItem("Select Client Name", "0"));
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
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
                Customer _Customer = new Customer();
                _Customer.ClientName = txtClientName.Text;
                DataSet _ListOfCustomer = _ICustomerController.GetAllCustomerWithRoleByClientID(_Customer.ClientName);
                if (_ListOfCustomer.Tables[0].Rows.Count > 0)
                {
                    //Panel1.Style.Add("display", "block");
                    Panel1.Style.Add("display", "block");
                    gvCustomer.DataSource = _ListOfCustomer;
                    gvCustomer.DataBind();
                    //_SessionInformation._ListOfAdminCustomer = _ListOfCustomer;
                    //IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
                }
                else
                {
                    Panel1.Style.Add("display", "none");
                    lblErrorMessageGUID.Text = "No Customer found";
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
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

        /// <summary>
        /// Added By : Sagar Joshi
        /// Purpose: Dynamic Grid Binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void gvCustomer_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int i = 0;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(cell.Text)) && cell.Text != "&nbsp;")
                    {
                        Image _image = new Image();
                        _image.ImageUrl = "~/Images/" + cell.Text + ".jpg";
                        if (System.IO.File.Exists(Server.MapPath(_image.ImageUrl)))
                        {
                            cell.Controls.Add(_image);
                        }
                    }
                }
            }


            if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;
            }
        }

        #endregion

        #region "Button Events"
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = "Save";
                ClearFields();
                BindRole();
                ChkActive.Checked = true;
                ChkActive.Enabled = false;
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClient = new List<Client>();
                _ListOfClient = _IClientController.GetMasterClientInfoByClientName(txtClientName.Text);
                if (_ListOfClient.Count > 0)
                {
                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                    _ViewstateInformation.AdminClientID = _ListOfClient[0].ClientKey;
                    SetViewstateInformation(_ViewstateInformation);

                    txtClientNameAdd.Text = txtClientName.Text;
                    if (string.IsNullOrEmpty(_ListOfClient[0].MasterClient))
                    {
                        txtMasterClient.Text = "NA";
                    }
                    else
                    {
                        txtMasterClient.Text = _ListOfClient[0].MasterClient;
                    }
                    BindAdminCustomer();
                    mdlpopupScreen.Show();
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSearchCustomerByName.Text) && string.IsNullOrEmpty(txtClientName.Text))
                {
                    lblInvalidCustomer.Text = "Please enter valid Client name or Customer Email.";
                }
                else
                {

                    if (!string.IsNullOrEmpty(txtSearchCustomerByName.Text) && !string.IsNullOrEmpty(txtClientName.Text))
                    {
                        BindAdminCustomer();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtSearchCustomerByName.Text))
                        {
                            BindAdminCustomer();
                        }
                        else
                        {
                            ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                            List<Customer> _ListOfCustomer = _ICustomerController.GetCustomerInfoByFirstName(txtSearchCustomerByName.Text);
                            if (_ListOfCustomer.Count > 0)
                            {
                                foreach (Customer _Customer in _ListOfCustomer)
                                {
                                    txtClientName.Text = _Customer.ClientName;
                                }
                                BindAdminCustomer();
                            }
                            else
                            {
                                lblInvalidCustomer.Text = "Invalid Customer Email.";
                                BindAdminCustomer();
                            }
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

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Text = "Update";
                ChkActive.Enabled = true;
                Customer _Customer = new Customer();
                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                string hdnCustomerKey = (sender as LinkButton).CommandArgument;

                _Customer.CustomerKey = Convert.ToInt32(hdnCustomerKey);

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.AdminCustomerID = _Customer.CustomerKey;
                SetViewstateInformation(_ViewstateInformation);

                DataSet _ListOfCustomer = _ICustomerController.GetCustomerInfoWithRoleByCustomerID(_Customer.CustomerKey);

                if (_ListOfCustomer.Tables[0].Rows.Count > 0)
                {
                    ChkActive.Checked = Convert.ToBoolean(_ListOfCustomer.Tables[0].Rows[0]["IsActive"]);
                    txtClientNameAdd.Text = Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["ClientName"]);
                    txtSetupDate.Text = Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["CreatedDate"]);
                    txtMasterClient.Text = Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["MasterClient"]);
                    txtFirstName.Text = Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["FirstName"]);
                    txtLastName.Text = Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["LastName"]);
                    txtEmail.Text = Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["Email"]);
                    txtPassword.Attributes.Add("value", Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["CustomerPassword"]));
                    txtContactNo.Text = Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["ContactNo"]);
                    txtComments.Text = Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["CustomerComment"]);

                    cbIsMultiLogin.Checked = Convert.ToBoolean(_ListOfCustomer.Tables[0].Rows[0]["MultiLogin"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["DefaultPage"])))
                    {
                        ddlDefaultPage.SelectedIndex = -1;
                        ddlDefaultPage.Items.FindByText(Convert.ToString(_ListOfCustomer.Tables[0].Rows[0]["DefaultPage"])).Selected = true;
                    }


                    IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                    List<Role> _ListOfRole = _IRoleController.GetRoleInformation(true);
                    var ClientRoles = (from Roles in _ListOfRole
                                       where
                                           Roles.RoleName != RolesName.IframeMicrosite.ToString() &&
                                           Roles.RoleName != RolesName.NielsenData.ToString() &&
                                           Roles.RoleName != RolesName.CompeteData.ToString() &&
                                           Roles.RoleName != RolesName.MicrositeDownload.ToString()

                                       select Roles).ToList();
                    if (ClientRoles.Count > 0)
                    {
                        rptRoles.DataSource = ClientRoles;
                        rptRoles.DataBind();
                    }
                    for (int _RoleCount = 0; _RoleCount < rptRoles.Items.Count; _RoleCount++)
                    {
                        Label lblRole = (Label)rptRoles.Items[_RoleCount].FindControl("lblRole");
                        CheckBox chkSelectRole = (CheckBox)rptRoles.Items[_RoleCount].FindControl("chkSelectRole");
                        chkSelectRole.Checked = Convert.ToBoolean(_ListOfCustomer.Tables[0].Rows[0][lblRole.Text]);
                    }

                }
                BindAdminCustomer();
                mdlpopupScreen.Show();
            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    foreach (DataListItem _DataListItem in rptRoles.Items)
                    {
                        Label _lblRole = (Label)_DataListItem.FindControl("lblRole");

                        if (_lblRole.Text.ToLower() == ddlDefaultPage.SelectedItem.Value.ToLower())
                        {
                            CheckBox _CheckBox = (CheckBox)_DataListItem.FindControl("chkSelectRole");

                            if (!_CheckBox.Checked)
                            {
                                lblErrorMessageRole.Text = "Selected default page has not given appropriate access right.";
                                lblErrorMessageRole.Visible = true;

                                mdlpopupScreen.Show();
                                return;
                            }

                            break;
                        }
                    }

                    if (btnSave.Text == "Save")
                    {
                        bool _Checked = false;
                        for (int _RoleCount = 0; _RoleCount < rptRoles.Items.Count; _RoleCount++)
                        {
                            CheckBox chkSelectRole = (CheckBox)rptRoles.Items[_RoleCount].FindControl("chkSelectRole");
                            if (chkSelectRole.Checked == true)
                            {
                                _Checked = true;
                                break;
                            }
                        }
                        if (_Checked == true)
                        {
                            string _Result = string.Empty;
                            ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                            Customer _Customer = new Customer();

                            _Customer.FirstName = txtFirstName.Text.ToString();
                            _Customer.LastName = txtLastName.Text.ToString();
                            _Customer.Email = txtEmail.Text;
                            _Customer.Password = txtPassword.Text.ToString();
                            _Customer.Comment = txtComments.Text;
                            _Customer.ContactNo = txtContactNo.Text;
                            _Customer.CustomerGUID = System.Guid.NewGuid().ToString();
                            _Customer.DefaultPage = ddlDefaultPage.SelectedItem.Text;
                            _Customer.MultiLogin = cbIsMultiLogin.Checked;

                            ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                            //_ViewstateInformation.AdminClientID = _ListOfClient[0].ClientKey;

                            _Customer.ClientID = Convert.ToInt32(_ViewstateInformation.AdminClientID);

                            _Result = _ICustomerController.InsertAdminCustomer(_Customer);
                            //BindCustomer();

                            if (_Result == "0")
                            {
                                lblErrorMessage.Visible = false;
                                //lblMessage.Visible = true;
                                //lblMessage.Text = _ExistMessage;
                                lblErrorMessageRole.Text = _ErrorMessage;
                                mdlpopupScreen.Show();
                            }
                            else
                            {
                                for (int _RoleCount = 0; _RoleCount < rptRoles.Items.Count; _RoleCount++)
                                {
                                    CheckBox chkSelectRole = (CheckBox)rptRoles.Items[_RoleCount].FindControl("chkSelectRole");
                                    HiddenField hdnValue = (HiddenField)rptRoles.Items[_RoleCount].FindControl("hdnValue");
                                    if (chkSelectRole.Checked == true)
                                    {
                                        CustomerRoles _CustomerRoles = new CustomerRoles();
                                        _CustomerRoles.CustomerID = Convert.ToInt32(_Result);
                                        _CustomerRoles.RoleID = Convert.ToInt32(hdnValue.Value);
                                        ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                                        string _Result1 = _ICustomerRoleController.InsertCustomerRole(_CustomerRoles);
                                    }
                                }
                                lblErrorMessage.Visible = false;
                                lblMessage.Visible = true;
                                lblMessage.Text = _InsertMessage;
                            }
                            //}
                            //else
                            //{
                            //    lblMessage.Visible = false;
                            //    lblErrorMessageGUID.Visible = true;
                            //    lblErrorMessageGUID.Text = _LoginMessage;
                            //}
                            ClearFields();
                        }
                        else
                        {
                            lblErrorMessageRole.Text = "Please Select Atleast One Role";
                            mdlpopupScreen.Show();
                        }
                    }
                    else
                    {
                        bool _Checked = false;
                        for (int _RoleCount = 0; _RoleCount < rptRoles.Items.Count; _RoleCount++)
                        {
                            CheckBox chkSelectRole = (CheckBox)rptRoles.Items[_RoleCount].FindControl("chkSelectRole");
                            if (chkSelectRole.Checked == true)
                            {
                                _Checked = true;
                                break;
                            }
                        }
                        if (_Checked == true)
                        {
                            string _Result = string.Empty;
                            ICustomerController _IClientController = _ControllerFactory.CreateObject<ICustomerController>();
                            Customer _Customer = new Customer();
                            ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                            _Customer.CustomerKey = Convert.ToInt32(_ViewstateInformation.AdminCustomerID);
                            _Customer.FirstName = txtFirstName.Text.ToString();
                            _Customer.LastName = txtLastName.Text.ToString();
                            _Customer.Email = txtEmail.Text;
                            _Customer.Password = txtPassword.Text.ToString();
                            _Customer.Comment = txtComments.Text;
                            _Customer.ContactNo = txtContactNo.Text;
                            //_Customer.CustomerGUID = System.Guid.NewGuid().ToString();
                            _Customer.ModifiedDate = DateTime.Now;
                            _Customer.IsActive = Convert.ToBoolean(ChkActive.Checked);
                            _Customer.DefaultPage = ddlDefaultPage.SelectedItem.Text;
                            _Customer.MultiLogin = cbIsMultiLogin.Checked;

                            _Result = _IClientController.UpdateAdminCustomer(_Customer);

                            if (_Result == "-1")
                            {
                                //lblErrorMessage.Text = _ErrorMessage;
                                //return;
                                lblErrorMessageRole.Text = _ErrorMessage;
                                mdlpopupScreen.Show();
                            }
                            else
                            {
                                for (int _RoleCount = 0; _RoleCount < rptRoles.Items.Count; _RoleCount++)
                                {
                                    CheckBox chkSelectRole = (CheckBox)rptRoles.Items[_RoleCount].FindControl("chkSelectRole");
                                    HiddenField hdnValue = (HiddenField)rptRoles.Items[_RoleCount].FindControl("hdnValue");
                                    if (chkSelectRole.Checked == true)
                                    {
                                        CustomerRoles _CustomerRoles = new CustomerRoles();
                                        _CustomerRoles.CustomerID = Convert.ToInt32(_ViewstateInformation.AdminCustomerID);
                                        _CustomerRoles.RoleID = Convert.ToInt32(hdnValue.Value);
                                        ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                                        string _Result1 = _ICustomerRoleController.InsertCustomerRole(_CustomerRoles);
                                    }
                                    else
                                    {
                                        CustomerRoles _CustomerRoles = new CustomerRoles();
                                        _CustomerRoles.CustomerID = Convert.ToInt32(_ViewstateInformation.AdminCustomerID);
                                        _CustomerRoles.RoleID = Convert.ToInt32(hdnValue.Value);
                                        ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                                        string _Result1 = _ICustomerRoleController.UpdateCustomerRoleByClientIDRoleID(_CustomerRoles);
                                    }
                                }

                                ClearFields();
                                //BindAdminCustomer();

                                ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                                Customer _Customer1 = new Customer();
                                _Customer1.ClientName = txtClientName.Text;
                                DataSet _ListOfCustomer = _ICustomerController.GetAllCustomerWithRoleByClientID(_Customer1.ClientName);
                                if (_ListOfCustomer.Tables[0].Rows.Count > 0)
                                {
                                    //Panel1.Style.Add("display", "block");
                                    Panel1.Style.Add("display", "block");
                                    gvCustomer.DataSource = _ListOfCustomer;
                                    gvCustomer.DataBind();
                                    //_SessionInformation._ListOfAdminCustomer = _ListOfCustomer;
                                    //IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
                                }
                                else
                                {
                                    Panel1.Style.Add("display", "none");
                                    //lblErrorMessageGUID.Text = "No Customer found";
                                }

                                lblErrorMessage.Visible = false;
                                lblMessage.Visible = true;
                                //lblMessage.Text = _InsertMessage;
                                lblMessage.Text = "Customer Updated Successfully";
                            }
                        }
                        else
                        {
                            lblErrorMessageRole.Text = "Please Select Atleast One Role";
                            mdlpopupScreen.Show();
                        }
                    }
                }
                BindAdminCustomer();
                ddlDefaultPage.SelectedIndex = 0;

                //ClearFields();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        #endregion

        #region "DropDownList Events"
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

        protected void ddlClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ApplyGridHeaderFilter();
                //upCustomer.Update();
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