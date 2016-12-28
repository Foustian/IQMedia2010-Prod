using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel;
using IQMediaGroup.Core.Enumeration;
using System.Configuration;
using IQMediaGroup.Controller.Common;
using System.Web.UI.HtmlControls;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.WebApplication.IQAgent
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                IQMediaGroupResponsive iQMediaGroupResponsive = (IQMediaGroupResponsive)this.Page.Master;
                //iQMediaGroupResponsive.PageTitle = CommonConstants.IQAgentPageTitle;

                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();


                if (_SessionInformation.IsiQAgent == true)
                {

                    if (!Page.IsPostBack)
                    {
                        pnlReport.Visible = false;
                        pnlSearch.Visible = true;
                        rdoIQAgentType.SelectedValue = "0";
                    }

                    if (!_SessionInformation.IsiQAgentReport)
                    {
                        rdoIQAgentType.Visible = false;
                        pnlReport.Visible = false;
                        //(UCIQAgentControl.FindControl("aReport") as HtmlAnchor).Visible = false;
                    }

                    HeaderTabPanel _HeaderTabPanel = (HeaderTabPanel)this.Page.Master.FindControl(CommonConstants.HeaderTabPanel);

                    _HeaderTabPanel.ActiveTab = CommonConstants.aIQAgent;// CommonConstants.LBtnIQMediaAgent;

                    // IQMediaGroupInner _IQMediaContent = (IQMediaGroupInner)this.Master;



                    if (iQMediaGroupResponsive != null)
                    {
                        int SMTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["SetSMTimeOut"]);

                        if (SMTimeOut >= CommonConstants.SMTimeOut)
                        {
                            iQMediaGroupResponsive.SetSMTimeOut = CommonConstants.SMTimeOut;
                        }
                        else if (SMTimeOut > 0)
                        {
                            iQMediaGroupResponsive.SetSMTimeOut = SMTimeOut;
                        }
                        else
                        {
                            iQMediaGroupResponsive.SetSMTimeOut = CommonConstants.SMTimeOut;
                        }

                        int SessionTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["SessionTimeOut"]);

                        if (SessionTimeOut >= CommonConstants.SessionTimeOutCst)
                        {
                            Session.Timeout = CommonConstants.SessionTimeOutCst;
                        }
                        else if (SessionTimeOut > 0)
                        {
                            Session.Timeout = SessionTimeOut;
                        }
                        else
                        {
                            Session.Timeout = CommonConstants.SessionTimeOutCst;
                        }
                        //Session.Timeout = 
                    }
                    //<script src="../Script/IQAgent.js?ver=1.1" type="text/javascript"></script>
                    //<script type="text/javascript" src="../Script/Report.js"></script>
                }
                else
                {
                    //Response.Redirect("~/NoRole/?FromUrl=~/IQAgent/", false);
                    Response.Redirect("~/NoRole/", false);
                }
            }
            catch (Exception _Exception)
            {
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void IQAgentResultType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (rdoReport.Checked)
                //{
                //    pnlReport.Visible = true;
                //    pnlSearch.Visible = false;
                //}
                //else
                //{
                //    pnlReport.Visible = false;
                //    pnlSearch.Visible = true;
                //    UCIQAgentControl.SetTabVisiBility();
                //    UCIQAgentControl.ShowCurrentTab();
                //}
            }
            catch (Exception ex)
            {
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void rdoIQAgentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoIQAgentType.SelectedValue == "0")
                {
                    pnlReport.Visible = false;

                    pnlSearch.Visible = true;
                    UCIQAgentControl.ResetAllTab();
                    //UCIQAgentControl.ShowCurrentTab();
                }
                else
                {
                    pnlSearch.Visible = false;

                    pnlReport.Visible = true;
                    UCIQAgentControl1.ResetIQAgentReport();
                }
               /* UpdateUpdatePanel(upIQAgentReport);
                UpdateUpdatePanel(upIQAgentSearch);*/

                /*upIQAgentReport.Update();
                upIQAgentSearch.Update();*/

                // we should set update panel postion to static , as jquery block ui , sets its position to relative 
                // bcoz of that, popups postion does not comes properly. , so to make it work properly, we again set it to static.
               // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetUpTypePos", "$('#" + upType.ClientID + "').css({ \"position\": \"static\"});", true);
            }
            catch (Exception ex)
            {
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void UpdateUpdatePanel(UpdatePanel p_UpdatePanel)
        {
            if (p_UpdatePanel.UpdateMode == UpdatePanelUpdateMode.Conditional)
            {
                p_UpdatePanel.Update();
            }
        }
    }
}
