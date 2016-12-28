using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Admin.Core.Enumeration;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace IQMediaGroup.Admin.Usercontrol.IQAgentQueryEditor
{
    public partial class IQAgentQueryEditor : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        string strIQAgentUserID = "IQAgentUserID";

        public long IQAgentUserID
        {
            get
            {
                if (ViewState[strIQAgentUserID] != null)
                {
                    return Convert.ToInt64(ViewState[strIQAgentUserID]);
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                ViewState[strIQAgentUserID] = value;
            }
        }

        #region Page Evetns

        protected override void OnLoad(EventArgs e)
        {
            try
            {

                #region Set Bread Crumb

                GenerateBreadCrumb("IQAgent > Agent Setup");

                #endregion

                if (!IsPostBack)
                {
                    BindClientDropDown();
                    pnlStatSkedData.Visible = false;
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

        private void BindStatskedprogData(Guid ClientGuid)
        {
            try
            {

                 
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                MasterStatSkedProg _MasterStatSkedProg = null;
                IStatSkedProgController _IStatSkedProgController = _ControllerFactory.CreateObject<IStatSkedProgController>();
                Boolean IsAllDmaAllowed = true;
                Boolean IsAllStationAllowed = true;
                Boolean IsAllClassAllowed = true;
                _MasterStatSkedProg = _IStatSkedProgController.GetAllDetailByClientSettings(ClientGuid, out IsAllDmaAllowed,out IsAllStationAllowed,out IsAllClassAllowed);

                _ViewstateInformation.IsAllDmaAllowed = IsAllDmaAllowed;
                _ViewstateInformation.IsAllStationAllowed = IsAllStationAllowed;
                _ViewstateInformation.IsAllClassAllowed = IsAllClassAllowed;
                SetViewstateInformation(_ViewstateInformation);
                
                
                if (_MasterStatSkedProg._ListofMarket.Count > 0)
                {
                    rptMarket.DataSource = _MasterStatSkedProg._ListofMarket;
                    rptMarket.DataBind();
                }

                if (_MasterStatSkedProg._ListofAffil.Count > 0)
                {
                    rptAffil.DataSource = _MasterStatSkedProg._ListofAffil;
                    rptAffil.DataBind();
                }

                if (_MasterStatSkedProg._ListofType.Count > 0)
                {
                    rptProgramType.DataSource = _MasterStatSkedProg._ListofType;
                    rptProgramType.DataBind();
                }
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        #endregion Page Evetns

        #region User Define Methods

        public string GetMarketString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataListItem li in rptMarket.Items)
            {
                HtmlInputCheckBox chkMarket = li.FindControl("chkSelectMarket") as HtmlInputCheckBox;
                if (chkMarket != null && chkMarket.Checked)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("," + HttpUtility.HtmlEncode(chkMarket.Value) + "");
                    }
                    else
                    {
                        sb.Append("" + HttpUtility.HtmlEncode(chkMarket.Value) + "");
                    }
                }

            }

            if (sb.Length == 0)
            {
                foreach (DataListItem li in rptMarket.Items)
                {
                    HtmlInputCheckBox chkMarket = li.FindControl("chkSelectMarket") as HtmlInputCheckBox;
                    if (chkMarket != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append("," + HttpUtility.HtmlEncode(chkMarket.Value) + "");
                        }
                        else
                        {
                            sb.Append("" + HttpUtility.HtmlEncode(chkMarket.Value) + "");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public string GetMarketNameString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataListItem li in rptMarket.Items)
            {
                HtmlInputCheckBox chkMarket = li.FindControl("chkSelectMarket") as HtmlInputCheckBox;
                Label lblMarketName = li.FindControl("lblMarket") as Label;
                if (chkMarket != null && chkMarket.Checked && lblMarketName != null)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",'" + HttpUtility.HtmlEncode(lblMarketName.Text.Trim()) + "'");
                    }
                    else
                    {
                        sb.Append("'" + HttpUtility.HtmlEncode(lblMarketName.Text.Trim()) + "'");
                    }
                }

            }

            if (sb.Length == 0)
            {
                foreach (DataListItem li in rptMarket.Items)
                {
                    Label lblMarketName = li.FindControl("lblMarket") as Label;
                    if (lblMarketName != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(",'" + HttpUtility.HtmlEncode(lblMarketName.Text.Trim()) + "'");
                        }
                        else
                        {
                            sb.Append("'" + HttpUtility.HtmlEncode(lblMarketName.Text.Trim()) + "'");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public string GetAffiliateNetworkNameString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataListItem li in rptAffil.Items)
            {
                HtmlInputCheckBox chkAffil = li.FindControl("chkSelectAffil") as HtmlInputCheckBox;
                Label lblAffil = li.FindControl("lblAffil") as Label;
                if (chkAffil != null && chkAffil.Checked && lblAffil != null)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",'" + HttpUtility.HtmlEncode(lblAffil.Text) + "'");
                    }
                    else
                    {
                        sb.Append("'" + HttpUtility.HtmlEncode(lblAffil.Text) + "'");
                    }
                }

            }

            if (sb.Length == 0)
            {
                foreach (DataListItem li in rptAffil.Items)
                {
                    Label lblAffil = li.FindControl("lblAffil") as Label;
                    if (lblAffil != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(",'" + HttpUtility.HtmlEncode(lblAffil.Text) + "'");
                        }
                        else
                        {
                            sb.Append("'" + HttpUtility.HtmlEncode(lblAffil.Text) + "'");
                        }
                    }

                }

            }

            return sb.ToString();
        }

        public string GetAffiliateNetworkString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataListItem li in rptAffil.Items)
            {
                HtmlInputCheckBox chkAffil = li.FindControl("chkSelectAffil") as HtmlInputCheckBox;
                if (chkAffil != null && chkAffil.Checked)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("," + chkAffil.Value + "");
                    }
                    else
                    {
                        sb.Append("" + chkAffil.Value + "");
                    }
                }

            }

            if (sb.Length == 0)
            {
                foreach (DataListItem li in rptAffil.Items)
                {
                    HtmlInputCheckBox chkAffil = li.FindControl("chkSelectAffil") as HtmlInputCheckBox;
                    if (chkAffil != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append("," + chkAffil.Value + "");
                        }
                        else
                        {
                            sb.Append("" + chkAffil.Value + "");
                        }
                    }

                }

            }

            return sb.ToString();
        }

        public string GetProgramSubCategoryNameString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataListItem li in rptProgramType.Items)
            {
                HtmlInputCheckBox chkSelectProgramType = li.FindControl("chkSelectProgramType") as HtmlInputCheckBox;
                Label lblProgramType = li.FindControl("lblProgramType") as Label;
                if (chkSelectProgramType != null && chkSelectProgramType.Checked && lblProgramType != null)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",'" + HttpUtility.HtmlEncode(lblProgramType.Text) + "'");
                    }
                    else
                    {
                        sb.Append("'" + HttpUtility.HtmlEncode(lblProgramType.Text) + "'");
                    }
                }

            }

            if (sb.Length == 0)
            {
                foreach (DataListItem li in rptProgramType.Items)
                {
                    Label lblProgramType = li.FindControl("lblProgramType") as Label;
                    if (lblProgramType != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(",'" + HttpUtility.HtmlEncode(lblProgramType.Text) + "'");
                        }
                        else
                        {
                            sb.Append("'" + HttpUtility.HtmlEncode(lblProgramType.Text) + "'");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public string GetProgramSubCategoryString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataListItem li in rptProgramType.Items)
            {
                HtmlInputCheckBox chkSelectProgramType = li.FindControl("chkSelectProgramType") as HtmlInputCheckBox;
                if (chkSelectProgramType != null && chkSelectProgramType.Checked)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("," + chkSelectProgramType.Value + "");
                    }
                    else
                    {
                        sb.Append("" + chkSelectProgramType.Value + "");
                    }
                }

            }

            if (sb.Length == 0)
            {
                foreach (DataListItem li in rptProgramType.Items)
                {
                    HtmlInputCheckBox chkSelectProgramType = li.FindControl("chkSelectProgramType") as HtmlInputCheckBox;
                    if (chkSelectProgramType != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append("," + chkSelectProgramType.Value + "");
                        }
                        else
                        {
                            sb.Append("" + chkSelectProgramType.Value + "");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private void FillInfoFromSearchTermXML(string p_SearchTerm)
        {
            string strSearchTerm = string.Empty;
            string strProgramTitle = string.Empty;
            string strProgramDescription = string.Empty;
            string strAppearing = string.Empty;
            string strIQ_Dma_Num = string.Empty;
            string strIQ_Category = string.Empty;
            string strIQ_SubCategory = string.Empty;
            string strStationAffil = string.Empty;

            XmlDocument _XmlDoc = new XmlDocument();
            _XmlDoc.LoadXml(p_SearchTerm);

            XmlNodeList nodeList = _XmlDoc.GetElementsByTagName(CommonConstants.IQAgent_XMLTag_RootTag);

            if (nodeList.Count > 0)
            {
                foreach (System.Xml.XmlNode childNode in _XmlDoc.GetElementsByTagName(CommonConstants.IQAgent_XMLTag_RootTag)[0].ChildNodes)
                {

                    if (childNode.Name == CommonConstants.IQAgent_XMLTag_SearchTerm)
                    {
                        strSearchTerm = childNode.InnerText;
                        continue;
                    }

                    if (childNode.Name == CommonConstants.IQAgent_XMLTag_ProgramTitle)
                    {
                        strProgramTitle = childNode.InnerText;
                        continue;
                    }

                    //if (childNode.Name == CommonConstants.IQAgent_XMLTag_ProgramDescription)
                    //{
                    //    strProgramDescription = childNode.InnerText;
                    //    continue;
                    //}

                    if (childNode.Name == CommonConstants.IQAgent_XMLTag_Appearing)
                    {
                        strAppearing = childNode.InnerText;
                        continue;
                    }

                    

                    if (childNode.Name == CommonConstants.IQAgent_XMLTag_IQ_Dma_Num)
                    {
                        strIQ_Dma_Num = childNode.InnerText;
                        if (childNode.Attributes.Count > 0)
                        {
                            string value = childNode.Attributes[CommonConstants.IQAgent_XMLAttribute_IsManualSelect].Value.ToLower();
                            if (value == "true")
                            {
                                rdoMarket.SelectedValue = "2";
                                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptmarket1", "SetCheckedFromList('" + strIQ_Dma_Num.Replace("'","") + "','" + rptMarket.ClientID + "');", true);
                                //rptMarket.Enabled = true;
                            }
                            else
                            {
                                rdoMarket.SelectedValue = "1";
                                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptmarket2", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                            }
                        }
                        continue;
                    }

                    if (childNode.Name == CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat_Num)
                    {
                        strIQ_SubCategory = childNode.InnerText;
                        if (childNode.Attributes.Count > 0)
                        {
                            string value = childNode.Attributes[CommonConstants.IQAgent_XMLAttribute_IsManualSelect].Value.ToLower();
                            if (value == "true")
                            {
                                rdoProgramType.SelectedValue = "2";
                                ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType1", "SetCheckedFromList('" + strIQ_SubCategory.Replace("'", "") + "','" + rptProgramType.ClientID + "');", true);
                                //rptProgramType.Enabled = true;
                            }
                            else
                            {
                                rdoProgramType.SelectedValue = "1";
                                ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType2", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                            }
                        }
                        continue;
                    }

                    if (childNode.Name == CommonConstants.IQAgent_XMLTag_Station_Affil_Num)
                    {
                        strStationAffil = childNode.InnerText;
                        if (childNode.Attributes.Count > 0)
                        {
                            string value = childNode.Attributes[CommonConstants.IQAgent_XMLAttribute_IsManualSelect].Value.ToLower();
                            if (value == "true")
                            {
                                rdoAffil.SelectedValue = "2";
                                ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil1", "SetCheckedFromList('" + strStationAffil.Replace("'", "") + "','" + rptAffil.ClientID + "');", true);
                                //rptAffil.Enabled = true;
                            }
                            else
                            {
                                rdoAffil.SelectedValue = "1";
                                ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil2", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                            }
                        }
                        continue;
                    }
                }
            }

            txtSearchTerm.Text = strSearchTerm;
            txtProgramTitle.Text = strProgramTitle;
            //txtProgramDescription.Text = strProgramDescription;
            txtAppearing.Text = strAppearing;
            upStatSkedProg.Update();
        }

        private string GetSearchTermXML()
        {

            ViewstateInformation _ViewstateInformation = GetViewstateInformation();


            bool IsIQ_Dma_ManualSelect = false;
            bool IsIQ_Sub_CatManualSelect = false;
            bool IsStation_AffilManualSelect = false;

            if (rdoMarket.SelectedValue == "2" || _ViewstateInformation.IsAllDmaAllowed == false)
                IsIQ_Dma_ManualSelect = true;


            if (rdoProgramType.SelectedValue == "2" || _ViewstateInformation.IsAllClassAllowed == false)
                IsIQ_Sub_CatManualSelect = true;

            if (rdoAffil.SelectedValue == "2" || _ViewstateInformation.IsAllStationAllowed == false)
                IsStation_AffilManualSelect = true;

            bool IsDefaultSettings = txtProgramTitle.Text == string.Empty
                                    //&& txtProgramDescription.Text == string.Empty
                                    && txtAppearing.Text == string.Empty
                                    && !IsIQ_Dma_ManualSelect
                                    && !IsIQ_Sub_CatManualSelect
                                    && !IsStation_AffilManualSelect;

            StringBuilder strXML = new StringBuilder();
            string strIsManualSelect = CommonConstants.IQAgent_XMLAttribute_IsManualSelect;

            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_RootTag + ">");

            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_SearchTerm + ">" + txtSearchTerm.Text.Trim() + "</" + CommonConstants.IQAgent_XMLTag_SearchTerm + ">");
            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_ProgramTitle + ">" + txtProgramTitle.Text.Trim() + "</" + CommonConstants.IQAgent_XMLTag_ProgramTitle + ">");
            //strXML.Append("<" + CommonConstants.IQAgent_XMLTag_ProgramDescription + ">" + txtProgramDescription.Text.Trim() + "</" + CommonConstants.IQAgent_XMLTag_ProgramDescription + ">");
            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_Appearing + ">" + txtAppearing.Text.Trim() + "</" + CommonConstants.IQAgent_XMLTag_Appearing + ">");
            

            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_IQ_Dma_Num + " " + strIsManualSelect + "=\"" + IsIQ_Dma_ManualSelect.ToString().ToLower() + "\" >" + (IsIQ_Dma_ManualSelect ? GetMarketString() : "all") + "</" + CommonConstants.IQAgent_XMLTag_IQ_Dma_Num + ">");
            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_IQ_Dma_Name + " " + strIsManualSelect + "=\"" + IsIQ_Dma_ManualSelect.ToString().ToLower() + "\" >" + (IsIQ_Dma_ManualSelect ? GetMarketNameString() : "all") + "</" + CommonConstants.IQAgent_XMLTag_IQ_Dma_Name + ">");
            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat + " " + strIsManualSelect + "=\"" + IsIQ_Sub_CatManualSelect.ToString().ToLower() + "\" >" + (IsIQ_Sub_CatManualSelect ? GetProgramSubCategoryNameString() :"all") + "</" + CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat + ">");
            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat_Num + " " + strIsManualSelect + "=\"" + IsIQ_Sub_CatManualSelect.ToString().ToLower() + "\" >" + (IsIQ_Sub_CatManualSelect ? GetProgramSubCategoryString() : "all") + "</" + CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat_Num + ">");
            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_Station_Affil + " " + strIsManualSelect + "=\"" + IsStation_AffilManualSelect.ToString().ToLower() + "\" >" + (IsStation_AffilManualSelect ? GetAffiliateNetworkNameString() : "all") + "</" + CommonConstants.IQAgent_XMLTag_Station_Affil + ">");
            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_Station_Affil_Num + " " + strIsManualSelect + "=\"" + IsStation_AffilManualSelect.ToString().ToLower() + "\" >" + (IsStation_AffilManualSelect ? GetAffiliateNetworkString() : "all") + "</" + CommonConstants.IQAgent_XMLTag_Station_Affil_Num + ">");
            strXML.Append("<" + CommonConstants.IQAgent_XMLTag_IsDefaultSettings + ">" + IsDefaultSettings.ToString().ToLower() + "</" + CommonConstants.IQAgent_XMLTag_IsDefaultSettings + ">");

            strXML.Append("</" + CommonConstants.IQAgent_XMLTag_RootTag + ">");

            return strXML.ToString();
        }

        private void Clear()
        {
            try
            {
                txtIQAgentUserID.Text = string.Empty;
                List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = new List<IQAgentSearchRequest>();
                IQAgentSearchRequest _objIQAgentSearchRequest = new IQAgentSearchRequest();
                _objIQAgentSearchRequest.Query_Name = "";
                _ListOfIQAgentSearchRequest.Add(_objIQAgentSearchRequest);
                ddlQueryName.DataSource = _ListOfIQAgentSearchRequest;
                ddlQueryName.DataTextField = "Query_Name";
                ddlQueryName.DataValueField = "Query_Name";
                ddlQueryName.DataBind();
                txtCurrentQueryVersion.Text = string.Empty;
                txtSearchTerm.Text = string.Empty;
                txtProgramTitle.Text = string.Empty;
                //txtProgramDescription.Text = string.Empty;
                txtAppearing.Text = string.Empty;
                IQAgentUserID = -1;

                rdoAffil.SelectedValue = "1";
                rdoMarket.SelectedValue = "1";
                rdoProgramType.SelectedValue = "1";

                ScriptManager.RegisterStartupScript(rptMarket, rptMarket.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                upStatSkedProg.Update();
                // Clear All DataLists
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region DropDown Events

        private void BindClientDropDown()
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClient = _IClientController.GetClientInformation(true);
                ddlClient.Items.Clear();
                ddlClient.DataSource = from Client C in _ListOfClient
                                       select new
                                       {
                                           ClientKey = Convert.ToString(C.ClientKey) + ',' + Convert.ToString(C.ClientGUID),
                                           ClientName = C.ClientName
                                       };
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

        protected void ddlClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlClient.SelectedIndex > 0)
                {
                    Clear();
                    
                    string ClientKey = ddlClient.SelectedValue;
                    long ClientID = Convert.ToInt32(ClientKey.Split(',')[0]);
                    Guid ClientGuid = new Guid(ClientKey.Split(',')[1]);

                    #region Fill query Dropdown List from ClientID

                    ISearchRequestController _ISearchRequestController = _ControllerFactory.CreateObject<ISearchRequestController>();
                    List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = _ISearchRequestController.SelectByClientID(ClientID);
                    ddlQueryName.Items.Clear();
                    ddlQueryName.DataSource = _ListOfIQAgentSearchRequest;
                    ddlQueryName.DataTextField = "Query_Name";
                    ddlQueryName.DataValueField = "Query_Name";
                    ddlQueryName.DataBind();
                    ddlQueryName.Items.Insert(0, new ListItem("Choose Query", "-1"));

                    #endregion Fill query Dropdown List from ClientID

                    #region Fill Customer From ClientID Whose Role is "IQAgentUser"

                    ICustomerController _ICustomerController = _ControllerFactory.CreateObject<ICustomerController>();
                    List<Customer> _ListOfCustomer = _ICustomerController.GetCustomerByClientIDRoleName(ClientID, CommonConstants.IQAgentUserRoleName);

                    if (_ListOfCustomer.Count > 0)
                    {
                        lblMsg.Text = string.Empty;
                        foreach (Customer _Customer in _ListOfCustomer)
                        {
                            IQAgentUserID = _Customer.CustomerKey;
                            txtIQAgentUserID.Text = _Customer.Email;
                            break;
                        }
                    }
                    else
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = CommonConstants.NoUserWithIQAgentRole;
                    }

                    #endregion Fill Customer From ClientID Whose Rolw is "IQ Agent User"

                    #region Bind SSP Data from ClientGuid 
                    
                    BindStatskedprogData(ClientGuid);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                    pnlStatSkedData.Visible = true;

                    upStatSkedProg.Update();
                    
                    #endregion


                    updIQAgentQueryEditor.Update();
                }
                else
                {
                    Clear();
                    updIQAgentQueryEditor.Update();
                    pnlStatSkedData.Visible = false;
                    upStatSkedProg.Update();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void ddlQueryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlQueryName.SelectedIndex > 0 && ddlClient.SelectedIndex > 0)
                {
                    lblMsg.Text = string.Empty;

                    string ClientKey = ddlClient.SelectedValue;
                    long ClientID = Convert.ToInt32(ClientKey.Split(',')[0]);

                    ISearchRequestController _ISearchRequestController = _ControllerFactory.CreateObject<ISearchRequestController>();
                    List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = _ISearchRequestController.GetSearchRequestsByClientIDQueryName(ClientID, ddlQueryName.SelectedValue);
                    foreach (IQAgentSearchRequest _IQAgentSearchRequest in _ListOfIQAgentSearchRequest)
                    {
                        txtCurrentQueryVersion.Text = _IQAgentSearchRequest.Query_Version.ToString();
                        FillInfoFromSearchTermXML(_IQAgentSearchRequest.SearchTerm);
                        break;
                    }
                    updIQAgentQueryEditor.Update();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void ddlQueryName_ItemInserting(object sender, AjaxControlToolkit.ComboBoxItemInsertEventArgs e)
        {
            try
            {
                txtCurrentQueryVersion.Text = "0";
                txtSearchTerm.Text = string.Empty;
                txtSearchTerm.Focus();
                updIQAgentQueryEditor.Update();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion DropDown Events

        #region Button Events

        protected void btnSaveQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlQueryName.Text == string.Empty || ddlQueryName.SelectedValue == "-1")
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "Please Select Query..";
                    return;
                }

                if (IQAgentUserID == -1)
                {
                    lblMsg.Text = CommonConstants.NoUserWithIQAgentRole;
                    updIQAgentQueryEditor.Update();
                    return;
                }

                string ClientKey = ddlClient.SelectedValue;
                long ClientID = Convert.ToInt32(ClientKey.Split(',')[0]);
                Guid ClientGuid = new Guid(ClientKey.Split(',')[1]);

                ISearchRequestController _ISearchRequestController = _ControllerFactory.CreateObject<ISearchRequestController>();
                IQAgentSearchRequest _IQAgentSearchRequest = new IQAgentSearchRequest();
                _IQAgentSearchRequest.ClientID = ClientID;
                _IQAgentSearchRequest.IQ_Agent_UserID = IQAgentUserID;
                _IQAgentSearchRequest.Query_Name = ddlQueryName.Text;
                _IQAgentSearchRequest.Query_Version = 0;
                _IQAgentSearchRequest.SearchTerm = GetSearchTermXML();

                string PKValue = _ISearchRequestController.InsertSearchRequest(_IQAgentSearchRequest);

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Query saved successfully...";
                Clear();
                ddlClient.SelectedIndex = 0;
                pnlStatSkedData.Visible = false;
                updIQAgentQueryEditor.Update();
            }

            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Validations

        //protected void ClientValidator_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        //{
        //    try
        //    {
        //        string value = e.ValueToConvert as string;
        //        try
        //        {
        //            if (value == "-1")
        //            {
        //                e.ConvertedValue = "";
        //                e.ConversionErrorMessage = "Please select Client.";
        //            }
        //            else
        //            {
        //                e.ConvertedValue = "0";
        //            }
        //        }
        //        catch
        //        {
        //            e.ConversionErrorMessage = "Please select Client.";
        //            e.ConvertedValue = null;
        //        }
        //    }
        //    catch (Exception _Exception)
        //    {
        //        this.WriteException(_Exception);
        //    }
        //}

        //protected void QueryNameValidator_ValueConvert(object sender, Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs e)
        //{
        //    try
        //    {
        //        string value = e.ValueToConvert as string;
        //        try
        //        {
        //            if (value == "")
        //            {
        //                e.ConvertedValue = "";
        //                e.ConversionErrorMessage = "Please enter query.";
        //            }
        //            else
        //            {
        //                e.ConvertedValue = "0";
        //            }
        //        }
        //        catch
        //        {
        //            e.ConversionErrorMessage = "Please enter query.";
        //            e.ConvertedValue = null;
        //        }
        //    }
        //    catch (Exception _Exception)
        //    {
        //        this.WriteException(_Exception);
        //    }
        //}

        #endregion

    }
}

