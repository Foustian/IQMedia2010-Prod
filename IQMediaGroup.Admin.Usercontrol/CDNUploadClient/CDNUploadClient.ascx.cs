using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Xml;
using System.Data;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Admin.Core.Enumeration;

namespace IQMediaGroup.Admin.Usercontrol.CDNUploadClient
{
    public partial class CDNUploadClient : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        protected void Page_Load(object sender, EventArgs e)
        {
            GenerateBreadCrumb("CDN > CDN Upload Client");
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                GetAndBindAllClient();
                
            }
        }

        protected void gvEnableClient_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (_ViewstateInformation.SortDirection == "asc")
                {
                    _ViewstateInformation.SortDirection = "desc";
                }
                else
                {
                    _ViewstateInformation.SortDirection = "asc";
                }

                _ViewstateInformation.CurrentPage = 1;

                SetViewstateInformation(_ViewstateInformation);
                BindEnabledClient();
                
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
       
        protected void imgPrevious_Click(object sender, EventArgs e)
        {

            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.CurrentPage = _ViewstateInformation.CurrentPage - 1;
                SetViewstateInformation(_ViewstateInformation);
                BindEnabledClient();
            }
            catch (Exception ex)
            {

                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void imgNext_Click(object sender, EventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.CurrentPage = _ViewstateInformation.CurrentPage + 1;
                SetViewstateInformation(_ViewstateInformation);
                BindEnabledClient();
            }
            catch (Exception ex)
            {

                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void SetPaging(ViewstateInformation _ViewstateInformation)
        {
            if (_ViewstateInformation.TotalPages > 1)
            {
                divrawmediapaging.Visible = true;
                if (_ViewstateInformation.CurrentPage >= _ViewstateInformation.TotalPages)
                {
                    imgNext.Visible = false;
                }
                else
                {
                    imgNext.Visible = true;
                }
                if (_ViewstateInformation.CurrentPage == 1)
                {
                    imgPrevious.Visible = false;
                }
                else
                {
                    imgPrevious.Visible = true;

                }
            }
            else
            {
                divrawmediapaging.Visible = false;
            }
        }

        public void BindEnabledClient()
        {

            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                if (string.IsNullOrEmpty(_ViewstateInformation.SortDirection))
                {
                    _ViewstateInformation.SortDirection = "asc";
                }

                List<Client> _ListOfClient = _ViewstateInformation._ListOfClient;

                List<Client> _lstclientenable = new List<Client>();

                if (_ViewstateInformation.SortDirection == "asc")
                {

                    _lstclientenable = (List<Client>)(from a in _ListOfClient
                                                      where a.CDNUpload == true
                                                      select a).OrderBy(p => p.ClientName).ToList();
                }
                else
                {
                    _lstclientenable = (List<Client>)(from a in _ListOfClient
                                                      where a.CDNUpload == true
                                                      select a).OrderByDescending(a => a.ClientName).ToList();
                }

                if (!string.IsNullOrEmpty(txtClientName.Text))
                {
                    _lstclientenable = _lstclientenable.Where(c => c.ClientName.ToLower().Contains(txtClientName.Text.ToLower())).ToList();
                }

                if (!(_ViewstateInformation.CurrentPage.HasValue))
                {
                    _ViewstateInformation.CurrentPage = 1;
                }

                _ViewstateInformation.EnabledClientCount = _lstclientenable.Count;
                _ViewstateInformation.TotalPages = (int)Math.Ceiling((double)_lstclientenable.Count / (double)gvEnableClient.PageSize);


                SetViewstateInformation(_ViewstateInformation);


                if (_lstclientenable.Count > 0)
                {
                    gvEnableClient.DataSource = _lstclientenable.Skip((_ViewstateInformation.CurrentPage.Value - 1) * gvEnableClient.PageSize).Take(gvEnableClient.PageSize).ToList();
                    gvEnableClient.DataBind();
                    btnDisable.Visible = true;
                }
                else
                {
                    gvEnableClient.DataSource = null;
                    gvEnableClient.DataBind();
                    btnDisable.Visible = false;
                }

                SetPaging(_ViewstateInformation);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void BindDisabledClient()
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                List<Client> _ListOfClient = _ViewstateInformation._ListOfClient;

                List<Client> _lstclientdisable = (List<Client>)(from a in _ListOfClient
                                                                where a.CDNUpload == false
                                                                select a).ToList();

                chkClientDisable.DataTextField = "ClientName";
                chkClientDisable.DataValueField = "Clientkey";
                chkClientDisable.DataSource = _lstclientdisable;
                chkClientDisable.DataBind();

                if (_lstclientdisable.Count > 0)
                {
                    chkClientDisable.Items.Insert(0, new ListItem("All", "0"));
                    chkClientDisable.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkClientDisable.ClientID + "')");
                    btnEnable.Visible = true;
                }
                else
                {
                    btnEnable.Visible = false;
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void GetAndBindAllClient()
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();

                List<Client> _ListOfClient = new List<Client>();
                _ListOfClient = _IClientController.GetClientInfoForCDN();

                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation._ListOfClient = _ListOfClient;
                _ViewstateInformation.CurrentPage = 1;
                SetViewstateInformation(_ViewstateInformation);

                BindDisabledClient();
                BindEnabledClient();
                

            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        public void btnSearch_click(object sender, EventArgs e)
        {

            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.CurrentPage = 1;
                SetViewstateInformation(_ViewstateInformation);
                BindEnabledClient();
                if (chkClientDisable.Items.Count > 0)
                {
                    chkClientDisable.Items[0].Attributes.Add("onclick", "CheckUncheckAll('" + chkClientDisable.ClientID + "')");
                }
                
            }
            catch (Exception ex)
            {
                
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void btnResetSearch_click(object sender, EventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation.CurrentPage = 1;
                txtClientName.Text = string.Empty;
                BindEnabledClient();
            }
            catch (Exception ex)
            {

                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void btnEnable_click(object sender, EventArgs e)
        {

            XmlDocument doc = new XmlDocument();
            

            XmlNode AllDataNode = doc.CreateElement("Clients");
            doc.AppendChild(AllDataNode);
            try
            {
                foreach (ListItem lstitem in chkClientDisable.Items)
                {
                    if (lstitem.Selected)
                    {
                        XmlNode nameNode = doc.CreateElement("ClientID");
                        nameNode.AppendChild(doc.CreateTextNode(lstitem.Value));
                        AllDataNode.AppendChild(nameNode);

                    }
                }


                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                Int64 Status = _IClientController.UpdateClientInfoForCDN(doc, true);
                if (Status == 0)
                {
                    lblMessage.Text = "Record(s) updated successfully.";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Text = "An error occurred, please try again.";
                    lblMessage.Visible = true;
                }
                GetAndBindAllClient();
                //doc.ToString().Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>",string.Empty);
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
            
        }

        public void btnDisable_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            
            XmlNode AllDataNode = doc.CreateElement("Clients");
            doc.AppendChild(AllDataNode);
            try
            {
                foreach (GridViewRow _datarow in gvEnableClient.Rows)
                {
                    HtmlInputCheckBox chk = (HtmlInputCheckBox)_datarow.FindControl("chkSelect");
                    if (chk.Checked)
                    {
                        XmlNode nameNode = doc.CreateElement("ClientID");
                        HiddenField _hiddenfield = (HiddenField)_datarow.FindControl("hdnClientKey");
                        nameNode.AppendChild(doc.CreateTextNode(_hiddenfield.Value));
                        AllDataNode.AppendChild(nameNode);

                    }
                }


                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                Int64 Status = _IClientController.UpdateClientInfoForCDN(doc, false);
                if (Status == 0)
                {
                    lblMessage.Text = "Record(s) updated successfully.";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Text = "An error occurred, please try again.";
                    lblMessage.Visible = true;
                }
                GetAndBindAllClient();
                //doc.ToString().Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>",string.Empty);
            }
            catch (Exception ex)
            {

                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);

            }

            
        }

    }
}