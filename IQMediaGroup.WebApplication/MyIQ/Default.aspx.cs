using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel;
using System.Web.UI.HtmlControls;

namespace IQMediaGroup.WebApplication.MyIQ
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                IQMediaGroupResponsive iQMediaGroupResponsive = (IQMediaGroupResponsive)this.Page.Master;
                //iQMediaGroupResponsive.PageTitle = CommonConstants.MYIQPageTitle;
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (!_SessionInformation.IsMyiQReport)
                {
                    rdoReport.Visible = false;
                    rdoSearch.Visible =false;
                    pnlReport.Visible = false;
                }

                if (_SessionInformation.IsMyIQnew == true)
                {
                    //sionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                    if (!Page.IsPostBack)
                    {
                        rdoSearch.Checked = true;
                        pnlReport.Visible = false;
                    }

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetAllowResponseInBrowserHistory(false);
                    Response.Cache.SetNoStore();
                    Response.Cache.SetExpires(DateTime.MinValue);

                    HeaderTabPanel _HeaderTabPanel = (HeaderTabPanel)this.Page.Master.FindControl(CommonConstants.HeaderTabPanel);

                    _HeaderTabPanel.ActiveTab = CommonConstants.aMYIQnew; //CommonConstants.LBtnIQMediaArchieve;

                }
                else
                {
                    Response.Redirect("~/NoRole/?FromUrl=~/MyIQ/", false);
                    Response.Redirect("~/NoRole/", false);
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        protected void FilterType_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSearch.Checked)
            {
                pnlSearch.Visible = true;
                pnlReport.Visible = false;
                (UCMyIQControl.FindControl("hfCurrentTabIndex") as HiddenField).Value = "0";
                UCMyIQControl.ClearSearch();
                //UCMyIQControl.UpdateUserControl();
            }
            else
            {
                pnlSearch.Visible = false;
                pnlReport.Visible = true;
                UCMyIQReportControl.ResetReport();
                //UCMyIQReportControl.UpdateUserControl();
                //ShowHideSearchFilters(false);
            }
            // we should set update panel postion to static , as jquery block ui , sets its position to relative 
            // bcoz of that, popups postion does not comes properly. , so to make it work properly, we again set it to static.
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "SetUpTypePos", "$('#" + upType.ClientID + "').css({ \"position\": \"static\"});", true);
        }

        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
        //       server control at run time. */
        //}
    }
}