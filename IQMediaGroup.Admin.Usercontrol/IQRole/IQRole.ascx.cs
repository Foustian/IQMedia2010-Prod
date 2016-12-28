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

namespace IQMediaGroup.Admin.Usercontrol.IQRole
{
    public partial class IQRole : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private string _ExistMessage = "Role Already Exists.";
        private string _InsertMessage = "Role Inserted Successfully.";
        private string _ErrorMessage = "Role is already exists.";
        private string _IsActive = string.Empty;

        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                #region Set Bread Crumb

                GenerateBreadCrumb("Role Mapping > IQ Role");

                #endregion
                Page.SetFocus(txtRoleName);
                lblMessage.Visible = false;
                if (!IsPostBack)
                {
                    BindRole();
                    
                }
                lblErrorMessage1.Visible = false;
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

        /// <summary>
        /// Description:This method will bind All Role.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindRole()
        {
            try
            {
                IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                List<Role> _ListOfRole = _IRoleController.GetRoleInformation(null);

                if (_ListOfRole.Count > 0)
                {
                    gvRole.DataSource = _ListOfRole;
                    gvRole.DataBind();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion

        #region "Button Events"
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    #region "Test Customer Search Request----------------------Delete"
                    string[] makets = new string[] { "All Station" };
                    string[] programType = new string[] { "News" };
                    string[] programCategory = new string[] { "Obama" };

                    #endregion
                    IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                    Role _Role = new Role();
                    _Role.RoleName = txtRoleName.Text.Trim();
                    Cluster _Cluster = new Cluster();
                    string _Result = _IRoleController.InsertRole(_Role);
                    BindRole();
                    txtRoleName.Text = "";
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

        #region "GridEvents"

        protected void gvRole_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                TextBox txtRoleName = (TextBox)gvRole.Rows[e.RowIndex].FindControl("txtRoleName");
                DropDownList ddlStatus = (DropDownList)gvRole.Rows[e.RowIndex].FindControl("ddlStatus");
                HiddenField hdnRoleKey = (HiddenField)gvRole.Rows[e.RowIndex].FindControl("hdnRoleKey");
                IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();

                Role _Role = new Role();
                _Role.RoleID = Convert.ToInt32(hdnRoleKey.Value);
                _Role.RoleName = txtRoleName.Text.Trim();
                if (ddlStatus.SelectedValue == "1")
                {
                    _IsActive = "False";
                    _Role.IsActive = false;
                }
                else
                {
                    _IsActive = "True";
                    _Role.IsActive = true;
                }
                string retValue = _IRoleController.UpdateRole(_Role);
                if (!string.IsNullOrEmpty(retValue) && retValue == "-1")
                {
                    lblErrorMessage1.Text = _ErrorMessage;
                    lblErrorMessage1.Visible = true;
                    e.Cancel = true;
                   
                    
                    return;
                }
                else
                {
                    gvRole.EditIndex = -1;
                    BindRole();
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

        protected void gvRole_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvRole.EditIndex = -1;
                BindRole();
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

        protected void gvRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvRole.PageIndex = e.NewPageIndex;
                BindRole();
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

        protected void gvRole_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Label lblStatus = (Label)gvRole.Rows[e.NewEditIndex].FindControl("lblStatus");
                _IsActive = lblStatus.Text;
                gvRole.EditIndex = e.NewEditIndex;
                BindRole();
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
        #endregion
    }
}