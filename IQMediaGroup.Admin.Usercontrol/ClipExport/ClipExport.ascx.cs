using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using System.Threading;
using System.Xml;
using System.IO;
using System.Configuration;

namespace IQMediaGroup.Admin.Usercontrol.ClipExport
{
    public partial class ClipExport : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            lblMessage.Visible = false;
            try
            {
                #region Set Bread Crumb

                GenerateBreadCrumb("Export > Clip Export");

                #endregion

                if (!IsPostBack)
                {
                    txtFromDate.Attributes.Add("readonly", "true");
                    txtToDate.Attributes.Add("readonly", "true");
                    BindClient();
                    ddlCustomer.Items.Insert(0, CommonConstants.SelectCustomer);
                    ddlCustomer.SelectedIndex = 0;
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

        #region "User Defined Events"

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
                
                if (_ListOfClient.Count > 0)
                {

                    ddlClient.DataSource = _ListOfClient;
                    ddlClient.DataTextField = "ClientName";
                    ddlClient.DataValueField = "ClientKey";
                    ddlClient.DataBind();
                    ddlClient.Items.Insert(0, new ListItem("Select Client Name", "0"));
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = CommonConstants.NoDataAvailableClient;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion

        #region "DropDownList Events"
        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Visible = false;

                if (ddlClient.SelectedValue != "0")
                {
                    ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                    List<Customer> _ListOfCustomer = _ICustomerController.GetCustomerByClientID(Convert.ToInt32(ddlClient.SelectedValue));

                    if (_ListOfCustomer.Count > 0)
                    {
                        
                        ddlCustomer.Enabled = true;
                        ddlCustomer.DataSource = _ListOfCustomer;
                        ddlCustomer.DataTextField = "FullName";
                        ddlCustomer.DataValueField = "CustomerKey";
                        ddlCustomer.DataBind();
                        ddlCustomer.Items.Insert(0, new ListItem("Select Customer Name", "0"));
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = CommonConstants.NoCustomer;
                        ddlCustomer.Items.Insert(0, CommonConstants.SelectCustomer);
                        ddlCustomer.SelectedIndex = 0;
                        ddlCustomer.Enabled = false;
                    }
                }
                else
                {
                    ddlCustomer.Items.Insert(0, CommonConstants.SelectCustomer);
                    ddlCustomer.SelectedIndex = 0;
                    ddlCustomer.Enabled = false;
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

        #region Validations


        protected void ClusterValidator_ValueConvertClient(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            string value = e.ValueToConvert as string;
            try
            {
                if (value == "0")
                {
                    e.ConvertedValue = "";
                }
                else
                {
                    e.ConvertedValue = "0";
                }
            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch
            {
                e.ConversionErrorMessage = "Please select Clien Name.";
                e.ConvertedValue = null;
            }
        }

        protected void ClusterValidator_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        {
            string value = e.ValueToConvert as string;
            try
            {
                if (value == "0")
                {
                    e.ConvertedValue = "";
                }
                else
                {
                    e.ConvertedValue = "0";
                }
            }
            catch (System.Threading.ThreadAbortException _ThreadAbortException)
            {
            }
            catch
            {
                e.ConversionErrorMessage = "Please select Clien Name.";
                e.ConvertedValue = null;
            }
        }

        protected void cvFromDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (Convert.ToDateTime(txtFromDate.Text) > System.DateTime.Now)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
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

        protected void cvCompareDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
                {
                    if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
                    {
                        args.IsValid = false;
                    }
                    else
                    {
                        args.IsValid = true;
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

        protected void cvToDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (Convert.ToDateTime(txtToDate.Text) > System.DateTime.Now)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Visible = false;
                if (Page.IsValid)
                {
                    if (ddlCustomer.Enabled != false)
                    {
                        IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                        DateTime _StartDate = Convert.ToDateTime(txtFromDate.Text);
                        DateTime _EndDate = Convert.ToDateTime(txtToDate.Text);
                        _EndDate = _EndDate.AddDays(1);
                        int _CustomerID = Convert.ToInt32(ddlCustomer.SelectedValue);
                        IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                        List<ArchiveClipExport> _ListOfArchiveClip = _IArchiveClipController.GetArchiveClipByDate(_StartDate, _EndDate, _CustomerID);

                        _EndDate = _EndDate.AddDays(-1);
                        string _DateTimeAppend = _StartDate.ToShortDateString() + "-" + _EndDate.ToShortDateString();
                        _DateTimeAppend = _DateTimeAppend.Replace(" ", "_").Replace(":", "_").Replace("/", "_");
                        string _ExistingInfo = string.Empty;
                        string _ClientName = ddlClient.SelectedItem.ToString();
                        string _CustomerName = ddlCustomer.SelectedItem.ToString();
                        string _CustomerKey = ddlCustomer.SelectedValue.ToString();
                        if (_ListOfArchiveClip.Count > 0)
                        {
                            string _XMLString = CommonFunctions.MakeSerialization(_ListOfArchiveClip);

                            XmlDocument _XmlDocument = new XmlDocument();
                            _XmlDocument.LoadXml(_XMLString);

                            string _FolderPath = Server.MapPath(CommonConstants.Tild + CommonConstants.ForwardSlash + ConfigurationSettings.AppSettings[CommonConstants.ClientClipExport]) + CommonConstants.ForwardSlash + CommonConstants.Clip + CommonConstants.UnderScore + _ClientName + CommonConstants.UnderScore + _CustomerKey + CommonConstants.UnderScore + _CustomerName + CommonConstants.UnderScore + _DateTimeAppend + CommonConstants.Dot + CommonConstants.XmlText;

                            _XmlDocument.Save(_FolderPath);
                            lblMessage.Visible = true;
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            lblMessage.Text = CommonConstants.XMLGenerateSuccessMessage;
                        }
                        else
                        {
                            lblMessage.Visible = true;
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = CommonConstants.NoDataAvailable;
                        }
                        ddlClient.SelectedIndex = 0;
                        ddlCustomer.Items.Insert(0, CommonConstants.SelectCustomer);
                        ddlCustomer.SelectedIndex = 0;
                        ddlCustomer.Enabled = false;
                        txtFromDate.Text = string.Empty;
                        txtToDate.Text = string.Empty;
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = CommonConstants.NoCustomer;
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
    }
}