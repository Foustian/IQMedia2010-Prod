using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Usercontrol.Base;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Core.Enumeration;
using System.Xml;

namespace IQMediaGroup.Admin.Usercontrol.IQAgentConsole
{
    public partial class IQAgentConsole : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        #region Page Events

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                #region Set Bread Crumb

                GenerateBreadCrumb("IQAgent > Agent Queries");

                #endregion
                if (!IsPostBack)
                {
                    BindClientDropdown();
                    BindSearchResults();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Dropdown Binding

        private void BindClientDropdown()
        {
            try
            {
                IClientController _IClientController = _ControllerFactory.CreateObject<IClientController>();
                List<Client> _ListOfClient = _IClientController.GetClientInformation(true);
                ddlClient.DataSource = _ListOfClient;
                ddlClient.DataTextField = "ClientName";
                ddlClient.DataValueField = "ClientGUID";
                ddlClient.DataBind();
                ddlClient.Items.Insert(0, new ListItem("Select One", "-1"));
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion

        #region Grid Events

        private void BindSearchResults()
        {
            try
            {
                if (ddlClient.SelectedIndex > 0)
                {
                    Guid ClientGUID = new Guid(ddlClient.SelectedValue);
                    ISearchRequestController _ISearchRequestController = _ControllerFactory.CreateObject<ISearchRequestController>();
                    List<IQAgentSearchRequest> _ListOfIQAgentSearchRequests = _ISearchRequestController.SelectAllByClientID(ClientGUID);
                    grvSearch.DataSource = _ListOfIQAgentSearchRequests;
                    grvSearch.DataBind();
                    updSearchResults.Update();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grvSearch.PageIndex = e.NewPageIndex;
                BindSearchResults();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvSearch_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                grvSearch.EditIndex = e.NewEditIndex;
                BindSearchResults();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvSearch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                grvSearch.EditIndex = -1;
                BindSearchResults();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                CheckBox cbIsActive = grvSearch.Rows[grvSearch.EditIndex].FindControl("cbIsActive") as CheckBox;
                if (cbIsActive != null)
                {
                    long SearchRequestKey = Convert.ToInt64(grvSearch.DataKeys[grvSearch.EditIndex].Value);
                    ISearchRequestController _ISearchRequestController = _ControllerFactory.CreateObject<ISearchRequestController>();

                    Label lblQueryName = grvSearch.Rows[grvSearch.EditIndex].FindControl("lblQueryName") as Label;
                    if (lblQueryName != null)
                    {
                        _ISearchRequestController.UpdateIsActive(SearchRequestKey, cbIsActive.Checked, Convert.ToInt64(ddlClient.SelectedValue) ,lblQueryName.Text.Trim());
                    }
                    grvSearch.EditIndex = -1;
                    BindSearchResults();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void grvSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            IQAgentSearchRequest _IQAgentSearchRequest = e.Row.DataItem as IQAgentSearchRequest;
            if (_IQAgentSearchRequest != null)
            {
                XmlDocument _XmlDoc = new XmlDocument();
                _XmlDoc.LoadXml(_IQAgentSearchRequest.SearchTerm);

                XmlNodeList nodeList = _XmlDoc.GetElementsByTagName("IQAgentRequest");

                if (nodeList.Count > 0)
                {
                    foreach (System.Xml.XmlNode childNode in _XmlDoc.GetElementsByTagName("IQAgentRequest")[0].ChildNodes)
                    {
                        if (childNode.Name == CommonConstants.IQAgent_XMLTag_SearchTerm)
                        {
                            Label lblSearchTerm = e.Row.FindControl("lblSearchTerm") as Label;
                            if (lblSearchTerm != null)
                            {
                                lblSearchTerm.Text = childNode.InnerText;
                            }
                            continue;
                        }

                        if (childNode.Name == CommonConstants.IQAgent_XMLTag_ProgramTitle)
                        {
                            Label lblProgramName = e.Row.FindControl("lblProgramName") as Label;
                            if (lblProgramName != null)
                            {
                                lblProgramName.Text = childNode.InnerText;
                            }
                            continue;
                        }

                        //if (childNode.Name == CommonConstants.IQAgent_XMLTag_ProgramDescription)
                        //{
                        //    Label lblProgramDescription = e.Row.FindControl("lblProgramDescription") as Label;
                        //    if (lblProgramDescription != null)
                        //    {
                        //        lblProgramDescription.Text = childNode.InnerText;
                        //    }
                        //    continue;
                        //}
                    }
                }
            }
        }

        #endregion

        #region Button Events

        protected void btnGetStoredSearches_Click(object sender, EventArgs e)
        {
            try
            {
                BindSearchResults();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        #endregion Button Events
    }
}