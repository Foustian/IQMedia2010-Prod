using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Xml.Linq;
using IQMediaGroup.Admin.Core.Enumeration;

namespace IQMediaGroup.Admin.Usercontrol.ClientSearchSettings
{
    public partial class ClientSearchSettings : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        ViewstateInformation _ViewstateInformation;

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                lblErrorMessage.Text = string.Empty;
                lblSuccessMessage.Text = string.Empty;
                _ViewstateInformation = GetViewstateInformation(); 
                if (!IsPostBack)
                {
                    _ViewstateInformation.SortDirDma = "asc";
                    _ViewstateInformation.SortDirAffil = "asc";
                    GetStatskedprogData();
                    BindClientDropDown();
                }

                rdoMarket.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "')");
                rdoMarket.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptMarket.ClientID + "')");

                rdoAffil.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "')");
                rdoAffil.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptAffil.ClientID + "')");

                rdoProgramType.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "')");
                rdoProgramType.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptProgramType.ClientID + "')");
                
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetChkClickEvent", "SetChkClickEvent('" + rptAffil.ClientID + "');SetChkClickEvent('" + rptMarket.ClientID + "');SetChkClickEvent('" + rptProgramType.ClientID + "');", true);
                
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        private void BindClientDropDown()
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClient = _IClientController.GetClientInformation(true);
                ddlClient.Items.Clear();
                ddlClient.DataSource = _ListOfClient;
                ddlClient.DataTextField = "ClientName";
                ddlClient.DataValueField = "ClientKey";
                ddlClient.DataBind();
                ddlClient.Items.Insert(0, new ListItem("Select Client", "-1"));
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        private void GetStatskedprogData()
        {
            try
            {
                MasterStatSkedProg _MasterStatSkedProg = null;
                IStatSkedProgController _IStatSkedProgController = _ControllerFactory.CreateObject<IStatSkedProgController>();
                _MasterStatSkedProg = _IStatSkedProgController.GetAllDetail();

                _ViewstateInformation._MasterStatSkedProg = _MasterStatSkedProg;
                SetViewstateInformation(_ViewstateInformation);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        protected void BindStatskedprogData()
        {
            BindDmaSet();
            BindAffilSet();
            BindProgramSet();
        }

        protected void BindAffilSet()
        {
            if (_ViewstateInformation._MasterStatSkedProg._ListofAffil.Count > 0)
            {
                if (rdoSortAffil.SelectedValue == "0")
                {
                    if (_ViewstateInformation.SortDirAffil == "asc")
                    {
                        rptAffil.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofAffil.OrderBy(a => a.Station_Affil);
                        imgsortaffil.ImageUrl = "~/Images/arrow-up.gif";
                    }
                    else
                    {
                        rptAffil.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofAffil.OrderByDescending(a => a.Station_Affil);
                        imgsortaffil.ImageUrl = "~/Images/arrow-down.gif";
                    }
                }
                else
                {
                    if (_ViewstateInformation.SortDirAffil == "asc")
                    {
                        rptAffil.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofAffil.OrderBy(a => a.Station_Affil_Num);

                        imgsortaffil.ImageUrl = "~/Images/arrow-up.gif";
                    }
                    else
                    {
                        rptAffil.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofAffil.OrderByDescending(a => a.Station_Affil_Num);
                        imgsortaffil.ImageUrl = "~/Images/arrow-down.gif";
                    }
                }
                rptAffil.DataBind();
            }
        }

        protected void BindDmaSet()
        {
            if (_ViewstateInformation._MasterStatSkedProg._ListofMarket.Count > 0)
            {
                if (rdoSortMarket.SelectedValue == "0")
                {

                    if (_ViewstateInformation.SortDirDma == "asc")
                    {
                        rptMarket.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofMarket.OrderBy(d => d.IQ_Dma_Name);

                        imgsortdma.ImageUrl = "~/Images/arrow-up.gif";
                    }
                    else
                    {
                        rptMarket.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofMarket.OrderByDescending(a => a.IQ_Dma_Name);
                        imgsortdma.ImageUrl = "~/Images/arrow-down.gif";
                    }
                }
                else
                {
                    if (_ViewstateInformation.SortDirDma == "asc")
                    {
                        rptMarket.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofMarket.OrderBy(d => d.IQ_Dma_Num);
                        imgsortdma.ImageUrl = "~/Images/arrow-up.gif";
                    }
                    else
                    {
                        rptMarket.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofMarket.OrderByDescending(d => d.IQ_Dma_Num);
                        imgsortdma.ImageUrl = "~/Images/arrow-down.gif";
                    }
                }

                rptMarket.DataBind();
            }
        }

        protected void BindProgramSet()
        {
            if (_ViewstateInformation._MasterStatSkedProg._ListofType.Count > 0)
            {
                rptProgramType.DataSource = _ViewstateInformation._MasterStatSkedProg._ListofType;
                rptProgramType.DataBind();
            }
        }

        private void SetDatalistSelection()
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();

                string XmlSearchSettings = _IClientController.GetClientSearchSettingsByClientID(Convert.ToInt64(ddlClient.SelectedValue));
                if (!string.IsNullOrEmpty(XmlSearchSettings))
                {
                    XDocument _xdoc = XDocument.Parse(XmlSearchSettings);

                    if (_xdoc.Descendants("Station_Affiliate_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("Station_Affiliate_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                    {

                        foreach (XElement Xelem in _xdoc.Descendants("Station_Affiliate").Elements("name"))
                        {
                            foreach (DataListItem _item in rptAffil.Items)
                            {
                                HtmlInputCheckBox _chk = (HtmlInputCheckBox)_item.FindControl("chkSelectAffil");
                                Label _Label = (Label)_item.FindControl("lblAffil");

                                if (_Label.Text == Xelem.Value)
                                {
                                    _chk.Checked = true;
                                    break;
                                }
                            }
                        }
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "SetCheck1", "SetCheckedList('" + rptAffil.ClientID + "');", true);
                    }
                    else
                    {
                        rdoAffil.Items[0].Selected = true;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                    }

                    if (_xdoc.Descendants("IQ_Dma_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("IQ_Dma_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                    {

                        foreach (XElement Xelem in _xdoc.Descendants("IQ_Dma").Elements("num"))
                        {
                            foreach (DataListItem _item in rptMarket.Items)
                            {
                                HtmlInputCheckBox _chk = (HtmlInputCheckBox)_item.FindControl("chkSelectMarket");
                                if (_chk.Value == Xelem.Value)
                                {
                                    _chk.Checked = true;
                                    break;
                                }
                            }
                        }
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "SetCheck2", "SetCheckedList('" + rptMarket.ClientID + "');", true);
                    }
                    else
                    {
                        rdoMarket.Items[0].Selected = true;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                    }

                    if (_xdoc.Descendants("IQ_Class_Set").Elements().Count() > 0 && Convert.ToBoolean(_xdoc.Descendants("IQ_Class_Set").ElementAt(0).Attribute("IsAllowAll").Value) == false)
                    {

                        foreach (XElement Xelem in _xdoc.Descendants("IQ_Class").Elements("num"))
                        {
                            foreach (DataListItem _item in rptProgramType.Items)
                            {
                                HtmlInputCheckBox _chk = (HtmlInputCheckBox)_item.FindControl("chkSelectProgramType");
                                if (_chk.Value == Xelem.Value)
                                {
                                    _chk.Checked = true;
                                    break;
                                }
                            }
                        }
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "SetCheck3", "SetCheckedList('" + rptProgramType.ClientID + "');", true);
                    }
                    else
                    {
                        rdoProgramType.Items[0].Selected = true;
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (Page.IsValid && ddlClient.SelectedIndex > 0)
                {
                    XDocument xmlDocument = new XDocument(new XElement("SearchSettings",
                     Convert.ToString(rdoMarket.SelectedValue) == "1" ? new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "true")) :
                     new XElement("IQ_Dma_Set", new XAttribute("IsAllowAll", "false"),
                        from DataListItem market in rptMarket.Items
                        let marketcheck = (HtmlInputCheckBox)market.FindControl("chkSelectMarket")
                        let marketlbl = (Label)market.FindControl("lblMarket")
                        where marketcheck.Checked == true
                        select new XElement("IQ_Dma",
                            new XElement("num", marketcheck.Value),
                            new XElement("name", marketlbl.Text))),

                      rdoAffil.SelectedValue == "1" ? new XElement("Station_Affiliate_Set", new XAttribute("IsAllowAll", "true")) :
                      new XElement("Station_Affiliate_Set", new XAttribute("IsAllowAll", "false"),
                        from DataListItem affil in rptAffil.Items
                        let affilcheck = (HtmlInputCheckBox)affil.FindControl("chkSelectAffil")
                        let affillbl = (Label)affil.FindControl("lblAffil")
                        where affilcheck.Checked == true
                        select new XElement("Station_Affiliate",
                            new XElement("num", affilcheck.Value),
                            new XElement("name", affillbl.Text))),

                      rdoProgramType.SelectedValue == "1" ? new XElement("IQ_Class_Set", new XAttribute("IsAllowAll", "true")) :
                      new XElement("IQ_Class_Set", new XAttribute("IsAllowAll", "false"),
                      from DataListItem program in rptProgramType.Items
                      let programcheck = (HtmlInputCheckBox)program.FindControl("chkSelectProgramType")
                      let programlbl = (Label)program.FindControl("lblProgramType")
                      where programcheck.Checked == true
                      select new XElement("IQ_Class",
                      new XElement("num", programcheck.Value),
                      new XElement("name", programlbl.Text)))
                      ));

                    IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                    string RowsAffected = _IClientController.SaveClientSearchSettingsByXml(Convert.ToInt64(ddlClient.SelectedValue), xmlDocument);
                    if (Convert.ToInt32(RowsAffected) > 0)
                    {
                        lblSuccessMessage.Text = "Client Search Settigs Saved Successfully";
                        
                    }
                    else
                    {
                        lblErrorMessage.Text = "Error Saving Client Search Settigs, Please Try Again!!";
                    }
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "SetCheck4", "SetCheckedList('" + rptAffil.ClientID + "');SetCheckedList('" + rptMarket.ClientID + "');SetCheckedList('" + rptProgramType.ClientID + "');", true);
                }
                else
                {
                    lblErrorMessage.Text = "Please Select Client";
                }         
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }

        }

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlClient.SelectedIndex > 0)
                {
                    BindStatskedprogData();
                    SetDatalistSelection();
                    SearchSettings.Visible = true;
                }
                else
                {
                    lblErrorMessage.Text = "Please Select Client";
                    SearchSettings.Visible= false;
                }
            }
            catch(Exception ex)
            {
                this.WriteException(ex);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void SortSelection(object sender, EventArgs e)
        {
            ImageButton _ImageButton= sender as ImageButton;
            if (_ImageButton != null && _ImageButton.ID == imgsortaffil.ID)
            {
                _ViewstateInformation.SortDirAffil = _ViewstateInformation.SortDirAffil == "asc" ? "desc" : "asc";
            }

            if (_ImageButton != null && _ImageButton.ID == imgsortdma.ID)
            {
                _ViewstateInformation.SortDirDma = _ViewstateInformation.SortDirDma == "asc" ? "desc" : "asc";
            }
            SetViewstateInformation(_ViewstateInformation);
            BindStatskedprogData();
            SetDatalistSelection();
        }
    }

}