using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Reports.Usercontrol.Base;
using IQMediaGroup.Reports.Controller.Factory;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Reports.Controller.Interface;

namespace IQMediaGroup.Reports.Usercontrol.IQMediaMaster.IQAgentReport
{
    public partial class IQAgentReport : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        SessionInformation _SessionInformation;
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                _SessionInformation = CommonFunctions.GetSessionInformation();
                lblErrorMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    pnlReports.Visible = false;
                    BindReportType();
                    txtReportDate.Attributes.Add("readonly", "true");
                    GetClientCompeteRights();
                }
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ReportDocReady", "ReportDocReady();", true);
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                //Response.Redirect(CommonConstants.CustomErrorPage);
                lblErrorMessage.Text = CommonConstants.CommonErrMsg;
            }
        }

        protected void GetClientCompeteRights()
        {
            try
            {
                IRoleController _IRoleController = _ControllerFactory.CreateObject<IRoleController>();
                ViewstateInformation viewstateInformation = GetViewstateInformation();
                viewstateInformation.IsCompeteData = Convert.ToBoolean(_IRoleController.GetClientRoleByClientGUIDRoleName(new Guid(_SessionInformation.ClientGUID), RolesName.CompeteData.ToString()));
                SetViewstateInformation(viewstateInformation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindReportType()
        {
            try
            {

                IIQ_ReportTypeController _IIQ_ReportTypeController = _ControllerFactory.CreateObject<IIQ_ReportTypeController>();
                List<IQ_ReportType> _ListOfIQ_ReportType = _IIQ_ReportTypeController.GetReportTypeByClientSettings(new Guid(_SessionInformation.ClientGUID), IQ_MasterReportType.IQAgent.ToString());
                if (_ListOfIQ_ReportType.Count > 0)
                {
                    drpReportTypes.DataSource = _ListOfIQ_ReportType;
                    drpReportTypes.DataValueField = "Identity";
                    drpReportTypes.DataTextField = "Name";
                    drpReportTypes.DataBind();
                    drpReportTypes.Items.Insert(0, new ListItem("Select", "0"));
                }
                else
                {
                    lblErrorMessage.Text = "No Report";
                    drpReportTypes.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblErrorMessage.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
                //Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpReportTypes.SelectedIndex != 0)
                {
                    DateTime _ReportDate = Convert.ToDateTime(txtReportDate.Text);
                    IIQ_ReportController _IIQ_ReportController = _ControllerFactory.CreateObject<IIQ_ReportController>();
                    List<IQ_Report> _ListOfIQ_Report = _IIQ_ReportController.GetReportByReportTypeAndClientGuid(Convert.ToInt32(drpReportTypes.SelectedValue.Split(',')[0]), new Guid(_SessionInformation.ClientGUID), _ReportDate);
                    gvReports.DataSource = _ListOfIQ_Report;
                    gvReports.DataBind();
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showClientReportDiv", "$('#divClientReports').show();", true);
                    pnlReports.Visible = true;
                    Report1.Visible = false;
                    upReportGrid.Update();
                }
                else
                {
                    lblErrorMessage.Text = "Please select Report Type.";
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblErrorMessage.Text = CommonConstants.CommonErrMsg;// "An error occured, please try again!";
                //Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void lnkbtnTitle_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid _ReportGuid;
                if (e.CommandName == "LoadReport" && Guid.TryParse(e.CommandArgument.ToString(), out _ReportGuid))
                {
                    IIQ_ReportController _IIQ_ReportController = _ControllerFactory.CreateObject<IIQ_ReportController>();
                    IQ_Report _IQ_Report = _IIQ_ReportController.GetReportXmlByReportGUID(_ReportGuid);
                    if (_IQ_Report != null)
                    {
                        /*switch (_IQ_Report._ReportTypeID)
                        {
                            case 1:*/
                        //byte[] EncReportID = CommonFunctions.AesEncryptStringToBytes(_ReportID.ToString());
                        ViewstateInformation viewState = GetViewstateInformation();
                        Report1.IsCompeteData = viewState.IsCompeteData;
                        Report1.FetchReportXml(_IQ_Report.ReportRule, _IQ_Report.ClientGuid, _IQ_Report.ReportType, _IQ_Report.Identity, Convert.ToString(_IQ_Report.ReportGUID));
                        Report1.Visible = true;
                        upReportGrid.Update();
                        /*      break;
                      }*/
                    }
                    else
                    {
                        lblErrorMessage.Text = "Invalid Report ID";
                    }
                }
            }
            catch (Exception ex)
            {
                this.WriteException(ex);
                lblErrorMessage.Text = CommonConstants.CommonErrMsg; // "An error occured, please try again!";
                //Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void ResetIQAgentReport()
        {
            drpReportTypes.SelectedIndex = 0;
            pnlReports.Visible = false;
            Report1.Visible = false;
            upReportType.Update();
            upReportGrid.Update();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "HideDateFilter", "$('#liReportDateFilter').hide();", true);
        }


    }
}