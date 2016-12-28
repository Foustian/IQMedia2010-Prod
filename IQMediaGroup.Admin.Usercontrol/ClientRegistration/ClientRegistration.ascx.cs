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
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace IQMediaGroup.Admin.Usercontrol.ClientRegistration
{
    public partial class ClientRegistration : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private static readonly ControllerFactory _ControllerFactory1 = new ControllerFactory();
        private string _ExistMessage = "Client Already Exists.";
        private string _InsertMessage = "Client Inserted Successfully.";
        private string _ImageExistMessage = "Image already exists with another user, Please upload image with another name.";
        private string _ImageTypeMessage = "Only " + ConfigurationManager.AppSettings["ClientImageExtensions"].Replace(",", ", ") + " image is allowed";
        private string _CustomHeaderSizeMessage = "Size of image should be within width=" + ConfigurationManager.AppSettings["CustomHeaderWidth"] + "px, height=" + ConfigurationManager.AppSettings["CustomHeaderHeight"] + "px";
        private string _PlayerLogoSizeMessage = "Size of image should be within width=" + ConfigurationManager.AppSettings["PlayerLogoWidth"] + "px, height=" + ConfigurationManager.AppSettings["PlayerLogoHeight"] + "px";
        private string _InvalidImageMessage = "Custom Header/Player Logo image is not in correct format";

        private string _IsActive = string.Empty;
        public string strDropDownMasterClient = string.Empty;
        public bool IsInsert;
        #region "Page Events"

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                #region Set Bread Crumb

                GenerateBreadCrumb("Registration > Client Registration");

                #endregion
                txtClientName.Attributes.Add("autocomplete", "off");
                lblError.Text = "";
                lblErrorMessageRole.Text = "";
                lblCustomHeaderSize.Text = _CustomHeaderSizeMessage;
                lblPlayerLogoSize.Text = _PlayerLogoSizeMessage;
                lblMsgFileType.Text = _ImageTypeMessage;

                //Page.SetFocus(txtClientName);
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
                    txtSetupDate.Text = DateTime.Now.ToString();
                    BindClient();
                    BindRole();
                    BindIndustry();
                    BindBillType();
                    BindBillFrequency();
                    BindPricingCode();
                    BindState();
                    BindMasterClient();
                    GetAllDefaultClientSetting();

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

        private void GetAllDefaultClientSetting()
        {
            IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
            DataSet _DataSetClientSettigs = _IClientController.GetAllDefaultSettings();
            if (_DataSetClientSettigs.Tables[0].Rows.Count > 0)
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.DefaultCompeteMultipier = Convert.ToDecimal(_DataSetClientSettigs.Tables[0].Rows[0]["CompeteMultiplier"]);
                _ViewstateInformation.DefaultOnlineNewsAdRate = Convert.ToDecimal(_DataSetClientSettigs.Tables[0].Rows[0]["OnlineNewsAdRate"]);
                _ViewstateInformation.DefaultOtherOnlineAdRate = Convert.ToDecimal(_DataSetClientSettigs.Tables[0].Rows[0]["OtherOnlineAdRate"]);
                _ViewstateInformation.DefaultURLPercentRead = Convert.ToDecimal(_DataSetClientSettigs.Tables[0].Rows[0]["URLPercentRead"]);
                _ViewstateInformation.DefaultIQAgentCount = Convert.ToInt32(_DataSetClientSettigs.Tables[0].Rows[0]["TotalNoOfIQAgent"]);
                _ViewstateInformation.DefaultIQAgentNotificationCount = Convert.ToInt32(_DataSetClientSettigs.Tables[0].Rows[0]["TotalNoOfIQNotification"]);
                SetViewstateInformation(_ViewstateInformation);
            }

        }

        /// <summary>
        /// Description:This method will bind All client.
        /// Added By:Bhavik Barot.
        /// </summary>
        private void BindClient()
        {
            try
            {
                //IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                //List<Client> _ListOfClient = _IClientController.GetClientInformation(null);

                //if (_ListOfClient.Count > 0)
                //{
                //    /*var ClientNames =
                //                        from p in _ListOfClient
                //                        select p.ClientName;

                //    drpQueryName.DataSource = ClientNames;
                //    drpQueryName.DataBind();*/
                //    //foreach (Client _Client in _ListOfClient)
                //    //{
                //    //    IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                //    //    List<ClientRoles> _listofClientRoles = _IClientRoleController.GetClientRoleByClientID(_Client.ClientKey);
                //    //    if (_listofClientRoles.Count > 0)
                //    //    {
                //    //        foreach (ClientRoles _ClientRoles in _listofClientRoles)
                //    //        {
                //    //            if (_ClientRoles.RoleName == "GlobalAdminAccess")
                //    //            {

                //    //                HtmlInputCheckBox chkDelete = (HtmlInputCheckBox)gvClient.Rows[].FindControl("chkDelete");
                //    //                chkDelete.Checked = true;
                //    //            }
                //    //        }
                //    //    }


                //    //}
                //    gvClient.DataSource = _ListOfClient;
                //    gvClient.DataBind();



                //}

                if (!string.IsNullOrEmpty(txtClientName.Text))
                {
                    IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                    Client _Client = new Client();
                    _Client.ClientName = txtClientName.Text;

                    DataSet _ListOfClient = _IClientController.GetClientInfoWithRole(_Client.ClientName);
                    if (_ListOfClient.Tables[0].Rows.Count > 0)
                    {
                        gvClient.DataSource = _ListOfClient;
                        gvClient.DataBind();
                    }
                    else
                    {
                        lblError.Text = "No Client Found";
                    }
                }
                else
                {
                    IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                    DataSet _ListOfClient = _IClientController.GetClientInfoWithRole();

                    if (_ListOfClient.Tables[0].Rows.Count > 0)
                    {
                        gvClient.DataSource = _ListOfClient;
                        gvClient.DataBind();
                    }
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindIndustry()
        {
            try
            {
                IIndustryController _IIndustryController = _ControllerFactory.CreateObject<IIndustryController>();
                List<Industry> _ListOfIndustry = _IIndustryController.GetIndustryInformation();
                if (_ListOfIndustry.Count > 0)
                {
                    drpIndustry.DataTextField = "IndustryCode";
                    drpIndustry.DataValueField = "IndustryKey";
                    drpIndustry.DataSource = _ListOfIndustry;
                    drpIndustry.DataBind();
                    drpIndustry.Items.Insert(0, new ListItem("Select", "0"));
                    drpIndustry.SelectedValue = "0";
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindBillType()
        {
            try
            {
                IBillTypeController _IBillTypeController = _ControllerFactory.CreateObject<IBillTypeController>();
                List<BillType> _ListOfBillType = _IBillTypeController.GetBillTypeInformation();
                if (_ListOfBillType.Count > 0)
                {
                    drpBillType.DataTextField = "Bill_Type";
                    drpBillType.DataValueField = "BillTypeKey";
                    drpBillType.DataSource = _ListOfBillType;
                    drpBillType.DataBind();
                    drpBillType.Items.Insert(0, new ListItem("Select", "0"));
                    drpBillType.SelectedValue = "0";
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindBillFrequency()
        {
            try
            {
                IBillFrequencyController _IBillFrequencyController = _ControllerFactory.CreateObject<IBillFrequencyController>();
                List<BillFrequency> _ListOfBillFrequency = _IBillFrequencyController.GetBillFrequencyInformation();
                if (_ListOfBillFrequency.Count > 0)
                {
                    drpBillFrequency.DataTextField = "Bill_Frequency";
                    drpBillFrequency.DataValueField = "BillFrequencyKey";
                    drpBillFrequency.DataSource = _ListOfBillFrequency;
                    drpBillFrequency.DataBind();
                    drpBillFrequency.Items.Insert(0, new ListItem("Select", "0"));
                    drpBillFrequency.SelectedValue = "0";
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindPricingCode()
        {
            try
            {
                IPricingCodeController _IPricingCodeController = _ControllerFactory.CreateObject<IPricingCodeController>();
                List<PricingCode> _ListOfPricingCode = _IPricingCodeController.GetPricingCodeInformation();
                if (_ListOfPricingCode.Count > 0)
                {
                    drpPricingCode.DataTextField = "Pricing_Code";
                    drpPricingCode.DataValueField = "PricingCodeKey";
                    drpPricingCode.DataSource = _ListOfPricingCode;
                    drpPricingCode.DataBind();
                    drpPricingCode.Items.Insert(0, new ListItem("Select", "0"));
                    drpPricingCode.SelectedValue = "0";
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void BindState()
        {
            try
            {
                IStateController _IStateController = _ControllerFactory.CreateObject<IStateController>();
                List<State> _ListOfState = _IStateController.GetStateInformation();
                if (_ListOfState.Count > 0)
                {
                    drpState.DataTextField = "StateName";
                    drpState.DataValueField = "StateKey";
                    drpState.DataSource = _ListOfState;
                    drpState.DataBind();
                    drpState.Items.Insert(0, new ListItem("Select", "0"));
                    drpState.SelectedValue = "0";
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
                if (_ListOfRole.Count > 0)
                {
                    rptRoles.DataSource = _ListOfRole;
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

        private void BindMasterClient()
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClient = _IClientController.GetMasterClientInformation();
                if (_ListOfClient.Count > 0)
                {
                    ddlMasterClient.DataSource = _ListOfClient;
                    ddlMasterClient.DataTextField = "MasterClient";
                    ddlMasterClient.DataValueField = "MasterClient";
                    ddlMasterClient.DataBind();
                    ddlMasterClient.Items.Insert(0, new ListItem("Select", "Select"));
                    //foreach (Client _Client in _ListOfClient)
                    //{
                    //    strDropDownMasterClient += "<option value='" + _Client.MasterClient + "' >" + _Client.MasterClient + "</option>";
                    //}
                    //upClient.Update();
                    //gvClient.DataSource = _ListOfClient;
                    //gvClient.DataBind();
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region "Grid Events"

        protected void gvClient_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvClient.PageIndex = e.NewPageIndex;
                BindClient();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);

            }
        }

        protected void gvClient_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

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
                if (e.Row.Cells.Count >= 3)
                {
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                }
            }
        }

        #endregion

        #region "Button Events"
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindClient();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                trCompete.Attributes.Add("style", "display:block");
                txtCompeteNewsAdRate.Text = _ViewstateInformation.DefaultOnlineNewsAdRate.ToString();
                txtCompeteDefaultAdRate.Text = _ViewstateInformation.DefaultOtherOnlineAdRate.ToString();
                txtCompeteMultiplier.Text = _ViewstateInformation.DefaultCompeteMultipier.ToString();
                txtCompeteURLPercentRead.Text = _ViewstateInformation.DefaultURLPercentRead.ToString();
                hdnIsCompeteSelected.Value = "1";

                hfHeaderImage.Value = string.Empty;
                hfPlayerLogo.Value = string.Empty;

                aHeaderImage.Visible = false;
                aPlayerLogo.Visible = false;
                btnSave.Text = "Save";
                BindRole();
                txtClientNameAdd.Text = "";
                txtSetupDate.Text = DateTime.Now.ToString();
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtCity.Text = "";
                drpState.SelectedValue = "0";
                txtZip.Text = "";
                txtAttention.Text = "";
                txtPhone.Text = "";
                drpIndustry.SelectedValue = "0";
                drpBillType.SelectedValue = "0";
                drpBillFrequency.SelectedValue = "0";
                drpPricingCode.SelectedValue = "0";
                ddlMasterClient.SelectedValue = "Select";
                txtNoofUsers.Text = "1";
                txtTotalNoOfIQNotification.Text = _ViewstateInformation.DefaultIQAgentNotificationCount.ToString();
                txtTotalNoOfIQAgent.Text = _ViewstateInformation.DefaultIQAgentCount.ToString();
                ChkActive.Checked = true;
                ChkActive.Enabled = false;
                chkIsCustomHeader.Checked = false;
                chkIsPlayerLogo.Checked = false;
                BindClient();
                txtClientName.Text = string.Empty;
                mdlpopupScreen.Show();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void RemoveImage()
        {
            if (!String.IsNullOrEmpty(Convert.ToString(hfCreatedHeaderImage.Value)))
            {
                if (File.Exists(ConfigurationManager.AppSettings["DirCustomHeader"] + @"\" + Convert.ToString(hfCreatedHeaderImage.Value)))
                {
                    File.Delete(ConfigurationManager.AppSettings["DirCustomHeader"] + @"\" + Convert.ToString(hfCreatedHeaderImage.Value));
                }
            }

            if (!String.IsNullOrEmpty(Convert.ToString(hfCreatePlayerLogo.Value)))
            {
                if (File.Exists(ConfigurationManager.AppSettings["DirPlayerLogo"] + @"\" + Convert.ToString(hfCreatePlayerLogo.Value)))
                {
                    File.Delete(ConfigurationManager.AppSettings["DirPlayerLogo"] + @"\" + Convert.ToString(hfCreatePlayerLogo.Value));
                }
            }
        }

        private void CreateImage()
        {
            string image = string.Empty;
            try
            {

                if (fuCustomHeaderImage.HasFile)
                {
                    image = Regex.Replace(txtClientNameAdd.Text.Trim().Replace("\"", "_").Replace(@"\", "_"), @"[\/?:*|<>]", "_") + "_" + DateTime.Now.ToString().Replace(':', '_').Replace('/', '_') + "_CustomHeader" + fuCustomHeaderImage.PostedFile.FileName.ToString().Substring(fuCustomHeaderImage.PostedFile.FileName.ToString().LastIndexOf('.'));
                    image = image.Replace(" ", "_");
                    hfCreatedHeaderImage.Value = image;
                    fuCustomHeaderImage.PostedFile.SaveAs(ConfigurationManager.AppSettings["DirCustomHeader"] + @"\" + image);

                }
            }
            catch (DirectoryNotFoundException ex)
            {

            }

            try
            {
                if (fuPlayerLogo.HasFile)
                {
                    image = Regex.Replace(txtClientNameAdd.Text.Trim().Replace("\"", "_").Replace(@"\", "_"), @"[\/?:*|<>]", "_") + "_" + DateTime.Now.ToString().Replace(':', '_').Replace('/', '_') + "_PlayerLogo" + fuPlayerLogo.PostedFile.FileName.ToString().Substring(fuPlayerLogo.PostedFile.FileName.ToString().LastIndexOf('.'));
                    image = image.Replace(" ", "_");
                    hfCreatePlayerLogo.Value = image;
                    fuPlayerLogo.PostedFile.SaveAs(ConfigurationManager.AppSettings["DirPlayerLogo"] + @"\" + image);
                }
            }
            catch (DirectoryNotFoundException ex)
            {


            }
        }

        private bool CheckForImage()
        {
            string[] Extentions = ConfigurationManager.AppSettings["ClientImageExtensions"].Split(new char[] { ',' });

            if (fuCustomHeaderImage.HasFile)
            {
                if (Extentions.Contains(System.IO.Path.GetExtension(fuCustomHeaderImage.FileName).ToLower().Substring(1)))
                {
                    System.Drawing.Bitmap imageHeader = new System.Drawing.Bitmap(fuCustomHeaderImage.PostedFile.InputStream);
                    int height = imageHeader.Height;
                    int width = imageHeader.Width;

                    if (height > Convert.ToInt16(ConfigurationManager.AppSettings["CustomHeaderHeight"])
                        || width > Convert.ToInt16(ConfigurationManager.AppSettings["CustomHeaderWidth"]))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            if (fuPlayerLogo.HasFile)
            {
                if (Extentions.Contains(System.IO.Path.GetExtension(fuPlayerLogo.FileName).ToLower().Substring(1)))
                {
                    System.Drawing.Bitmap imagePlayerLogo = new System.Drawing.Bitmap(fuPlayerLogo.PostedFile.InputStream);
                    int height = imagePlayerLogo.Height;
                    int width = imagePlayerLogo.Width;

                    if (height > Convert.ToInt16(ConfigurationManager.AppSettings["PlayerLogoHeight"])
                        || width > Convert.ToInt16(ConfigurationManager.AppSettings["PlayerLogoWidth"]))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (CheckForImage())
                    {
                        if (hdnIsCompeteSelected.Value == "1" && ((string.IsNullOrEmpty(txtCompeteMultiplier.Text)) || string.IsNullOrEmpty(txtCompeteNewsAdRate.Text) || string.IsNullOrEmpty(txtCompeteDefaultAdRate.Text) || string.IsNullOrEmpty(txtCompeteURLPercentRead.Text)))
                        {
                            lblErrorMessageRole.Text = "Please Enter Compete Values";
                            trCompete.Attributes.Add("style", "display:block");
                            mdlpopupScreen.Show();
                        }
                        else
                        {
                            if (btnSave.Text == "Save")
                            {
                                if (Convert.ToInt32(txtNoofUsers.Text) > 0)
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
                                        CreateImage();
                                        string _Result = string.Empty;
                                        IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                                        Client _Client = new Client();
                                        _Client.ClientName = txtClientNameAdd.Text.Trim();
                                        _Client.ClientGUID = System.Guid.NewGuid();
                                        string _DefaultCategory = ConfigurationManager.AppSettings["DefaultCustomCategory"];
                                        _Client.DefaultCategory = _DefaultCategory;
                                        //Cluster _Cluster = new Cluster();
                                        _Client.PricingCodeID = Convert.ToInt64(drpPricingCode.SelectedValue);
                                        _Client.BillFrequencyID = Convert.ToInt64(drpBillFrequency.SelectedValue);
                                        _Client.BillTypeID = Convert.ToInt64(drpBillType.SelectedValue);
                                        _Client.IndustryID = Convert.ToInt64(drpIndustry.SelectedValue);
                                        _Client.StateID = Convert.ToInt64(drpState.SelectedValue);
                                        _Client.Address1 = txtAddress1.Text.Trim();
                                        _Client.Address2 = txtAddress2.Text.Trim();
                                        _Client.City = txtCity.Text.Trim();
                                        _Client.Zip = txtZip.Text.Trim();
                                        _Client.Attention = txtAttention.Text.Trim();
                                        _Client.Phone = txtPhone.Text.Trim();
                                        if (ddlMasterClient.SelectedValue == "Select" || ddlMasterClient.SelectedValue == "")
                                        {
                                            _Client.MasterClient = "";
                                        }
                                        else
                                        {
                                            _Client.MasterClient = ddlMasterClient.SelectedValue;
                                        }
                                        //_Client.MasterClient = "test1";
                                        _Client.NoOfUser = Convert.ToInt32(txtNoofUsers.Text.Trim());
                                        if (!string.IsNullOrWhiteSpace(txtTotalNoOfIQNotification.Text))
                                        {
                                            _Client.NoOfIQNotification = Convert.ToInt16(txtTotalNoOfIQNotification.Text.Trim());
                                        }

                                        if (!string.IsNullOrWhiteSpace(txtTotalNoOfIQAgent.Text))
                                        {
                                            _Client.NoOfIQAgnet = Convert.ToInt16(txtTotalNoOfIQAgent.Text.Trim());
                                        }

                                        if (hdnIsCompeteSelected.Value == "1" && !string.IsNullOrWhiteSpace(txtCompeteMultiplier.Text))
                                        {
                                            _Client.CompeteMultiplier = Convert.ToDecimal(txtCompeteMultiplier.Text);
                                        }

                                        if (hdnIsCompeteSelected.Value == "1" && !string.IsNullOrWhiteSpace(txtCompeteNewsAdRate.Text))
                                        {
                                            _Client.OnlineNewsAdRate = Convert.ToDecimal(txtCompeteNewsAdRate.Text);
                                        }

                                        if (hdnIsCompeteSelected.Value == "1" && !string.IsNullOrWhiteSpace(txtCompeteDefaultAdRate.Text))
                                        {
                                            _Client.OtherOnlineAdRate = Convert.ToDecimal(txtCompeteDefaultAdRate.Text);
                                        }

                                        if (hdnIsCompeteSelected.Value == "1" && !string.IsNullOrWhiteSpace(txtCompeteURLPercentRead.Text))
                                        {
                                            _Client.UrlPercentRead = Convert.ToDecimal(txtCompeteURLPercentRead.Text);
                                        }

                                        if (fuCustomHeaderImage.HasFile)
                                        {
                                            _Client.CustomHeaderPath = hfCreatedHeaderImage.Value;
                                            _Client.IsCustomHeader = chkIsCustomHeader.Checked == true ? true : false;
                                        }

                                        if (fuPlayerLogo.HasFile)
                                        {
                                            _Client.PlayerLogoPath = hfCreatePlayerLogo.Value;
                                            _Client.IsActivePlayerLogo = chkIsPlayerLogo.Checked == true ? true : false;
                                        }
                                        int Status = 0;
                                        _Result = _IClientController.InsertClient(_Client, out Status);

                                        txtClientName.Text = "";
                                        if (_Result == "0")
                                        {
                                            RemoveImage();
                                            lblErrorMessageRole.Visible = true;
                                            lblErrorMessageRole.Text = _ExistMessage;
                                            mdlpopupScreen.Show();
                                        }
                                        else if (Status == 1)
                                        {
                                            RemoveImage();
                                            lblErrorMessageRole.Visible = true;
                                            lblErrorMessageRole.Text = _ImageExistMessage;
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
                                                    ClientRoles _ClientRoles = new ClientRoles();
                                                    _ClientRoles.ClientID = Convert.ToInt32(_Result);
                                                    _ClientRoles.RoleID = Convert.ToInt32(hdnValue.Value);
                                                    IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                                                    string _Result1 = _IClientRoleController.InsertClientRole(_ClientRoles);
                                                }
                                            }
                                            lblMessage.Visible = true;
                                            lblMessage.Text = _InsertMessage;
                                            ClearFields();
                                        }
                                    }
                                    else
                                    {
                                        lblErrorMessageRole.Text = "Please Select Atleast One Role";
                                        mdlpopupScreen.Show();
                                    }
                                }
                                else
                                {
                                    lblErrorMessageRole.Text = "No of Users can not be less then 1";
                                    mdlpopupScreen.Show();
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(txtNoofUsers.Text) > 0)
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
                                        CreateImage();
                                        string _Result = string.Empty;
                                        IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                                        Client _Client = new Client();
                                        _Client.ClientName = txtClientNameAdd.Text.Trim();
                                        IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                                        _Client.ClientKey = _SessionInformation.ClientID;
                                        //_Client.ClientGUID = System.Guid.NewGuid();
                                        //string _DefaultCategory = ConfigurationManager.AppSettings["DefaultCustomCategory"];
                                        //_Client.DefaultCategory = _DefaultCategory;
                                        _Client.PricingCodeID = Convert.ToInt64(drpPricingCode.SelectedValue);
                                        _Client.BillFrequencyID = Convert.ToInt64(drpBillFrequency.SelectedValue);
                                        _Client.BillTypeID = Convert.ToInt64(drpBillType.SelectedValue);
                                        _Client.IndustryID = Convert.ToInt64(drpIndustry.SelectedValue);
                                        _Client.StateID = Convert.ToInt64(drpState.SelectedValue);
                                        _Client.Address1 = txtAddress1.Text.Trim();
                                        _Client.Address2 = txtAddress2.Text.Trim();
                                        _Client.City = txtCity.Text.Trim();
                                        _Client.Zip = txtZip.Text.Trim();
                                        _Client.Attention = txtAttention.Text.Trim();
                                        _Client.Phone = txtPhone.Text.Trim();
                                        if (ddlMasterClient.SelectedValue == "Select")
                                        {
                                            _Client.MasterClient = "";
                                        }
                                        else
                                        {
                                            _Client.MasterClient = ddlMasterClient.SelectedValue;
                                        }
                                        //_Client.MasterClient = "test1";
                                        _Client.NoOfUser = Convert.ToInt32(txtNoofUsers.Text.Trim());
                                        _Client.ModifiedDate = DateTime.Now;
                                        if (!string.IsNullOrWhiteSpace(txtTotalNoOfIQNotification.Text))
                                        {
                                            _Client.NoOfIQNotification = Convert.ToInt16(txtTotalNoOfIQNotification.Text.Trim());
                                        }

                                        if (!string.IsNullOrWhiteSpace(txtTotalNoOfIQAgent.Text))
                                        {
                                            _Client.NoOfIQAgnet = Convert.ToInt16(txtTotalNoOfIQAgent.Text.Trim());
                                        }

                                        if (hdnIsCompeteSelected.Value == "1" && !string.IsNullOrWhiteSpace(txtCompeteMultiplier.Text))
                                        {
                                            _Client.CompeteMultiplier = Convert.ToDecimal(txtCompeteMultiplier.Text);
                                        }

                                        if (hdnIsCompeteSelected.Value == "1" && !string.IsNullOrWhiteSpace(txtCompeteNewsAdRate.Text))
                                        {
                                            _Client.OnlineNewsAdRate = Convert.ToDecimal(txtCompeteNewsAdRate.Text);
                                        }

                                        if (hdnIsCompeteSelected.Value == "1" && !string.IsNullOrWhiteSpace(txtCompeteDefaultAdRate.Text))
                                        {
                                            _Client.OtherOnlineAdRate = Convert.ToDecimal(txtCompeteDefaultAdRate.Text);
                                        }

                                        if (hdnIsCompeteSelected.Value == "1" && !string.IsNullOrWhiteSpace(txtCompeteURLPercentRead.Text))
                                        {
                                            _Client.UrlPercentRead = Convert.ToDecimal(txtCompeteURLPercentRead.Text);
                                        }

                                        if (fuCustomHeaderImage.HasFile)
                                        {
                                            _Client.CustomHeaderPath = hfCreatedHeaderImage.Value; //Regex.Replace(txtClientNameAdd.Text.Trim().Replace("\"", "_").Replace(@"\", "_"), @"[\/?:*|<>]", "_") + "_CustomHeader" + fuCustomHeaderImage.PostedFile.FileName.ToString().Substring(fuCustomHeaderImage.PostedFile.FileName.ToString().LastIndexOf('.'));
                                            //Path.GetFileName(fuCustomHeaderImage.PostedFile.FileName);
                                        }
                                        else
                                        {
                                            _Client.CustomHeaderPath = string.IsNullOrEmpty(Convert.ToString(hfHeaderImage.Value)) == true ? null : hfHeaderImage.Value;
                                        }
                                        _Client.IsCustomHeader = chkIsCustomHeader.Checked == true ? true : false;

                                        if (fuPlayerLogo.HasFile)
                                        {

                                            _Client.PlayerLogoPath = hfCreatePlayerLogo.Value; //Regex.Replace(txtClientNameAdd.Text.Trim().Replace("\"", "_").Replace(@"\", "_"), @"[\/?:*|<>]", "_") + "_PlayerLogo" + fuPlayerLogo.PostedFile.FileName.ToString().Substring(fuPlayerLogo.PostedFile.FileName.ToString().LastIndexOf('.'));

                                        }
                                        else
                                        {
                                            _Client.PlayerLogoPath = string.IsNullOrEmpty(Convert.ToString(hfPlayerLogo.Value)) == true ? null : hfPlayerLogo.Value;
                                        }
                                        _Client.IsActivePlayerLogo = chkIsPlayerLogo.Checked == true ? true : false;

                                        _Client.IsActive = Convert.ToBoolean(ChkActive.Checked);

                                        int Status = 0;
                                        int NotificationStatus = 0;
                                        int IQAgentStatus = 0;
                                        _Result = _IClientController.UpdateClient(_Client, out Status, out NotificationStatus, out IQAgentStatus);

                                        if (NotificationStatus == -2)
                                        {
                                            RemoveImage();
                                            lblErrorMessageRole.Visible = true;
                                            lblErrorMessageRole.Text = "Client already has more notification, then mentioned.";
                                            mdlpopupScreen.Show();

                                        }
                                        else if (IQAgentStatus == -2)
                                        {
                                            RemoveImage();
                                            lblErrorMessageRole.Visible = true;
                                            lblErrorMessageRole.Text = "Client already has more IQAgent Queries, then mentioned.";
                                            mdlpopupScreen.Show();

                                        }
                                        else if (_Result == "-1")
                                        {
                                            RemoveImage();
                                            lblErrorMessageRole.Visible = true;
                                            lblErrorMessageRole.Text = CommonConstants.ClientNameAlreadyExists;
                                            mdlpopupScreen.Show();
                                            //return;
                                        }

                                        else if (Status == 1)
                                        {
                                            RemoveImage();
                                            lblErrorMessageRole.Visible = true;
                                            lblErrorMessageRole.Text = _ImageExistMessage;
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
                                                    ClientRoles _ClientRoles = new ClientRoles();
                                                    _ClientRoles.ClientID = Convert.ToInt32(_SessionInformation.ClientID);
                                                    _ClientRoles.RoleID = Convert.ToInt32(hdnValue.Value);
                                                    IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                                                    string _Result1 = _IClientRoleController.InsertClientRole(_ClientRoles);
                                                }
                                                else
                                                {
                                                    ClientRoles _ClientRoles = new ClientRoles();
                                                    _ClientRoles.ClientID = Convert.ToInt32(_SessionInformation.ClientID);
                                                    _ClientRoles.RoleID = Convert.ToInt32(hdnValue.Value);
                                                    IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                                                    string _Result1 = _IClientRoleController.UpdateClientRoleBiClientIDRoleID(_ClientRoles);
                                                }
                                            }
                                            lblMessage.Visible = true;
                                            lblMessage.Text = "Client Updated Successfully";
                                            ClearFields();
                                            /*txtClientNameAdd.Text = "";
                                            txtSetupDate.Text = "";
                                            txtAddress1.Text = "";
                                            txtAddress2.Text = "";
                                            txtCity.Text = "";
                                            drpState.SelectedValue = "0";
                                            txtZip.Text = "";
                                            txtAttention.Text = "";
                                            txtPhone.Text = "";
                                            drpIndustry.SelectedValue = "0";
                                            drpBillType.SelectedValue = "0";
                                            drpBillFrequency.SelectedValue = "0";
                                            drpPricingCode.SelectedValue = "0";
                                            ddlMasterClient.SelectedValue = "Select";
                                            txtNoofUsers.Text = "";*/
                                        }
                                    }
                                    else
                                    {
                                        lblErrorMessageRole.Text = "Please Select Atleast One Role";
                                        mdlpopupScreen.Show();
                                    }

                                }
                                else
                                {
                                    lblErrorMessageRole.Text = "No of Users can not be less then 1";
                                    mdlpopupScreen.Show();
                                }
                            }
                        }
                    }
                    else
                    {
                        lblErrorMessageRole.Text = _InvalidImageMessage;
                        mdlpopupScreen.Show();
                    }
                    BindClient();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

            //mdlpopupScreen.Show();
            //upClient.Update();

        }

        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                trCompete.Attributes.Add("style", "display:none");
                txtCompeteNewsAdRate.Text = string.Empty;
                txtCompeteDefaultAdRate.Text = string.Empty;
                txtCompeteMultiplier.Text = string.Empty;
                txtCompeteURLPercentRead.Text = string.Empty;
                hdnIsCompeteSelected.Value = "0";

                ClearFields();
                hfHeaderImage.Value = string.Empty;
                hfPlayerLogo.Value = string.Empty;
                hfCreatedHeaderImage.Value = string.Empty;
                hfCreatePlayerLogo.Value = string.Empty;
                btnSave.Text = "Update";

                string hdnClientKey = (sender as LinkButton).CommandArgument;
                ChkActive.Enabled = true;

                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                Client _Client = new Client();
                _Client.ClientKey = Convert.ToInt32(hdnClientKey);
                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                _SessionInformation.ClientID = _Client.ClientKey;
                IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.SetSessionInformation(_SessionInformation);
                DataSet _ListOfClient = _IClientController.GetClientInfoWithRoleByClientID(_Client.ClientKey);
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                if (_ListOfClient.Tables[0].Rows.Count > 0)
                {
                    txtClientNameAdd.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["ClientName"]);
                    txtSetupDate.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["CreatedDate"]);
                    if (string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["MasterClient"])))
                    {
                        ddlMasterClient.SelectedValue = "Select";
                    }
                    else
                    {
                        ddlMasterClient.SelectedValue = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["MasterClient"]);
                    }
                    ChkActive.Checked = Convert.ToBoolean(_ListOfClient.Tables[0].Rows[0]["IsActive"]);
                    //Request.Form[ddlMasterClient.NamingContainer.UniqueID+"$"+ddlMasterClient.ID]
                    txtAddress1.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["Address1"]);
                    txtAddress2.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["Address2"]);
                    txtCity.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["City"]);
                    txtZip.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["Zip"]);
                    txtAttention.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["Attention"]);
                    txtPhone.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["Phone"]);

                    if (string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["StateID"])))
                    {
                        drpState.SelectedValue = "0";
                    }
                    else
                    {
                        drpState.SelectedValue = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["StateID"]);
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["IndustryID"])))
                    {
                        drpIndustry.SelectedValue = "0";
                    }
                    else
                    {
                        drpIndustry.SelectedValue = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["IndustryID"]);
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["BillTypeID"])))
                    {
                        drpBillType.SelectedValue = "0";
                    }
                    else
                    {
                        drpBillType.SelectedValue = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["BillTypeID"]);
                    }

                    if (string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["BillFrequencyID"])))
                    {
                        drpBillFrequency.SelectedValue = "0";
                    }
                    else
                    {
                        drpBillFrequency.SelectedValue = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["BillFrequencyID"]);
                    }


                    if (string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["PricingCodeID"])))
                    {
                        drpPricingCode.SelectedValue = "0";
                    }
                    else
                    {
                        drpPricingCode.SelectedValue = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["PricingCodeID"]);
                    }

                    chkIsCustomHeader.Checked = Convert.ToBoolean(_ListOfClient.Tables[0].Rows[0]["IsCustomHeader"]) ? true : false;
                    chkIsPlayerLogo.Checked = Convert.ToBoolean(_ListOfClient.Tables[0].Rows[0]["IsActivePlayerLogo"]) ? true : false;
                    hfHeaderImage.Value = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["CustomHeaderImage"]);
                    hfPlayerLogo.Value = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["PlayerLogo"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["CustomHeaderImage"])))
                    {
                        aHeaderImage.Visible = true;
                        aHeaderImage.HRef = ConfigurationManager.AppSettings["URLCustomHeader"] + Convert.ToString(_ListOfClient.Tables[0].Rows[0]["CustomHeaderImage"]);
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["PlayerLogo"])))
                    {
                        aPlayerLogo.Visible = true;
                        aPlayerLogo.HRef = ConfigurationManager.AppSettings["URLWaterMark"] + Convert.ToString(_ListOfClient.Tables[0].Rows[0]["PlayerLogo"]);
                    }

                    txtNoofUsers.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["NoOfUser"]);

                    //CheckBox chkSelectRole = (CheckBox)rptRoles.Rows[e.NewEditIndex].FindControl("hdnClientKey");
                    IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                    List<Role> _ListOfRole = _IRoleController.GetRoleInformation(true);
                    if (_ListOfRole.Count > 0)
                    {
                        //foreach (Role _Role in _ListOfRole)
                        //{
                        //    _Role.RoleName
                        //}

                        rptRoles.DataSource = _ListOfRole;
                        rptRoles.DataBind();
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["NoOfIQNotification"])))
                        txtTotalNoOfIQNotification.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["NoOfIQNotification"]);
                    else
                        txtTotalNoOfIQNotification.Text = _ViewstateInformation.DefaultIQAgentNotificationCount.ToString();

                    if (!string.IsNullOrEmpty(Convert.ToString(_ListOfClient.Tables[0].Rows[0]["NoOfIQAgent"])))
                        txtTotalNoOfIQAgent.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["NoOfIQAgent"]);
                    else
                        txtTotalNoOfIQAgent.Text = _ViewstateInformation.DefaultIQAgentCount.ToString();


                    txtCompeteDefaultAdRate.Text = _ViewstateInformation.DefaultOtherOnlineAdRate.ToString();
                    txtCompeteMultiplier.Text = _ViewstateInformation.DefaultCompeteMultipier.ToString();
                    txtCompeteNewsAdRate.Text = _ViewstateInformation.DefaultOnlineNewsAdRate.ToString();
                    txtCompeteURLPercentRead.Text = _ViewstateInformation.DefaultURLPercentRead.ToString();

                    for (int _RoleCount = 0; _RoleCount < rptRoles.Items.Count; _RoleCount++)
                    {
                        Label lblRole = (Label)rptRoles.Items[_RoleCount].FindControl("lblRole");
                        CheckBox chkSelectRole = (CheckBox)rptRoles.Items[_RoleCount].FindControl("chkSelectRole");
                        //rptRoles.Items
                        chkSelectRole.Checked = Convert.ToBoolean(_ListOfClient.Tables[0].Rows[0][lblRole.Text]);
                        if (lblRole.Text == "CompeteData" && Convert.ToBoolean(_ListOfClient.Tables[0].Rows[0][lblRole.Text]) == true)
                        {
                            hdnIsCompeteSelected.Value = "1";
                            trCompete.Attributes.Add("style", "display:block");

                            txtCompeteDefaultAdRate.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["OtherOnlineAdRate"]);
                            txtCompeteNewsAdRate.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["OnlineNewsAdRate"]);
                            txtCompeteMultiplier.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["CompeteMultiplier"]);
                            txtCompeteURLPercentRead.Text = Convert.ToString(_ListOfClient.Tables[0].Rows[0]["URLPercentRead"]);
                        }
                    }
                }
                BindClient();
                mdlpopupScreen.Show();
                //btnSave.Text = "test";
                //mdlpopupScreen.Show();


            }
            catch (Exception _Exception)
            {

                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void ClearFields()
        {
            try
            {

                txtClientNameAdd.Text = "";
                txtSetupDate.Text = "";
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtCity.Text = "";
                drpState.SelectedValue = "0";
                txtZip.Text = "";
                txtAttention.Text = "";
                txtPhone.Text = "";
                drpIndustry.SelectedValue = "0";
                drpBillType.SelectedValue = "0";
                drpBillFrequency.SelectedValue = "0";
                drpPricingCode.SelectedValue = "0";
                ddlMasterClient.SelectedValue = "Select";
                txtNoofUsers.Text = "";
                txtTotalNoOfIQNotification.Text = "";
                txtTotalNoOfIQAgent.Text = string.Empty;

                for (int _RoleCount = 0; _RoleCount < rptRoles.Items.Count; _RoleCount++)
                {
                    Label lblRole = (Label)rptRoles.Items[_RoleCount].FindControl("lblRole");
                    CheckBox chkSelectRole = (CheckBox)rptRoles.Items[_RoleCount].FindControl("chkSelectRole");
                    chkSelectRole.Checked = true;
                }
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
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void ddlMasterClient_PreRender(object sender, EventArgs e)
        {
            try
            {
                /*string _Script = " $('#ctl00_Content_Data_ClientRegistration1_ddlMasterClient').combobox({data: [],autoShow: false,listHTML: function (val, index) {return $.ui.combobox.defaults.listHTML(val, index);}});";                

                ScriptManager.RegisterStartupScript(this, this.GetType(), "DDL", _Script, true);*/
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        //protected void drpMasterClient_ItemInserting(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //txtSearchMediaText.ReadOnly = false;
        //        //txtSearchMediaText.Text = CommonConstants.EmptyString;
        //        //ddlCluster.Enabled = true;
        //        //ddlCluster.SelectedIndex = 0;
        //        //txtFromDate.Enabled = true;
        //        //txtFromDate.Text = CommonConstants.EmptyString;
        //        //txtToDate.Enabled = true;
        //        //txtToDate.Text = CommonConstants.EmptyString;
        //    }
        //    catch (Exception _Exception)
        //    {

        //        throw _Exception;
        //    }
        //}

        //protected void drpMasterClient_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //if (drpQueryName.SelectedIndex == 0)
        //        //{
        //        //    divIQAgent.Style.Add("display", "none");
        //        //}
        //        //else
        //        //{
        //        //    divIQAgent.Style.Add("display", "block");
        //        //}

        //        //BindRawMediaGrid();
        //    }
        //    catch (Exception _Exception)
        //    {

        //        throw _Exception;
        //    }
        //}
        #endregion



    }
}

