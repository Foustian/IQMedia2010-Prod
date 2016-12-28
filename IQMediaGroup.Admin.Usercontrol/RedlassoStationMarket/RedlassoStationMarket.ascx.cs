using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.Enumeration;


namespace IQMediaGroup.Admin.Usercontrol.RedlassoStationMarket
{
    public partial class RedlassoStationMarket : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private string _ExistMessage = "Redlasso Station Market Already Exists.";
        private string _InsertMessage = "Redlasso Station Market Inserted Successfully.";
        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Page.SetFocus(txtStationMarketName);
                if (!IsPostBack)
                {
                    BindRedlassoStationMarket();
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
            try
            {
                if (Page.IsValid)
                {
                    IQMediaGroup.Core.HelperClasses.RedlassoStationMarket _RedlassoStationMarket = new IQMediaGroup.Core.HelperClasses.RedlassoStationMarket();
                    _RedlassoStationMarket.StationMarketName = txtStationMarketName.Text;
                    IRedlassoStationMarketController _IRedlassoStationMarketController = _ControllerFactory.CreateObject<IRedlassoStationMarketController>();
                    string _Result = _IRedlassoStationMarketController.InsertRedlassoStationMarket(_RedlassoStationMarket);
                    BindRedlassoStationMarket();
                    txtStationMarketName.Text = "";
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
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        #endregion

        #region "Private Methods"

        /// <summary>
        /// Description:This method will bind All Redlasso Station Market.
        /// Added By:Bhavik Barot.
        /// </summary>
        public void BindRedlassoStationMarket()
        {
            try
            {
                List<IQMediaGroup.Core.HelperClasses.RedlassoStationMarket> _ListOfRedlassoStationMarket = null;
                IRedlassoStationMarketController _IRedlassoStationMarketController = _ControllerFactory.CreateObject<IRedlassoStationMarketController>();
                _ListOfRedlassoStationMarket = _IRedlassoStationMarketController.GetAllRedlassoStationMarket();
                gvStationMarket.DataSource = _ListOfRedlassoStationMarket;
                gvStationMarket.DataBind();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion

        #region "Grid Events"
        protected void gvStationMarket_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvStationMarket.EditIndex = e.NewEditIndex;
                BindRedlassoStationMarket();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvStationMarket_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                TextBox txtMarketName = (TextBox)gvStationMarket.Rows[e.RowIndex].FindControl("txtMarketName");
                DropDownList ddlStatus = (DropDownList)gvStationMarket.Rows[e.RowIndex].FindControl("ddlStatus");
                HiddenField hdnRedlassoStationMarketKey = (HiddenField)gvStationMarket.Rows[e.RowIndex].FindControl("hdnRedlassoStationMarketKey");
                IRedlassoStationMarketController _IRedlassoStationMarketController = _ControllerFactory.CreateObject<IRedlassoStationMarketController>();
                IQMediaGroup.Core.HelperClasses.RedlassoStationMarket _RedlassoStationMarket = new IQMediaGroup.Core.HelperClasses.RedlassoStationMarket();
                _RedlassoStationMarket.RedlassoStationMarketKey = Convert.ToInt64(hdnRedlassoStationMarketKey.Value);
                _RedlassoStationMarket.StationMarketName = txtMarketName.Text;
                if (ddlStatus.SelectedValue == "1")
                    _RedlassoStationMarket.IsActive = false;
                else
                    _RedlassoStationMarket.IsActive = true;
                _IRedlassoStationMarketController.UpdateRedlassoStationMarket(_RedlassoStationMarket);
                gvStationMarket.EditIndex = -1;
                BindRedlassoStationMarket();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvStationMarket_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvStationMarket.EditIndex = -1;
                BindRedlassoStationMarket();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void gvStationMarket_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvStationMarket.PageIndex = e.NewPageIndex;
                BindRedlassoStationMarket();
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