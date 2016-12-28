using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.WebApplication.CustomError
{
    public partial class CustomError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                if (_SessionInformation.ErrorMessage != string.Empty)
                {
                    //imgError.Visible = false;
                    lblMsg.Text = _SessionInformation.ErrorMessage;
                    lblHeader.Text = "Session Timeout";
                    _SessionInformation = null;
                    lblErrorMsgHeading.Text = "";

                }
                else
                {
                    lblErrorMsgHeading.Text = "Error Message";
                    lblHeader.Text = "Error";
                    lblMsg.Text = CommonConstants.CommonErrorMsg;
                    _SessionInformation = null;
                }
            }
        }
    }
}
