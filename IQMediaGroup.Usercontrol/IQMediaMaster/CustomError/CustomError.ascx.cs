using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.CustomError
{
    public partial class CustomError : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                    if (!string.IsNullOrEmpty(_SessionInformation.ErrorMessage))
                    {
                        lblMsg.Text = _SessionInformation.ErrorMessage;
                        lblHeader.Text = "Session Timeout";
                        lblErrorMsgHeading.Text = "";
                        _SessionInformation.ErrorMessage = null;
                        CommonFunctions.SetSessionInformation(_SessionInformation);
                    }
                    else
                    {
                        lblErrorMsgHeading.Text = "Error Message";
                        lblErrorMsgHeading.Visible = false;
                        lblHeader.Text = "Message";

                        if (Request.QueryString["e"]!=null && Convert.ToString(Request.QueryString["e"])=="to")
                        {
                            lblMsg.Text = CommonConstants.DBTimeOutMsg;
                        }
                        else
                        {
                            lblMsg.Text = CommonConstants.CommonErrorMsg; 
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
            }
        }
    }
}