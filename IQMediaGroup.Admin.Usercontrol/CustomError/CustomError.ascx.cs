using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Core.Enumeration;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Usercontrol.Base;

namespace IQMediaGroup.Admin.Usercontrol.CustomError
{
    public partial class CustomError : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                    //if (!string.IsNullOrEmpty(_SessionInformation.ErrorMessage))
                    //{
                    //    lblMsg.Text = _SessionInformation.ErrorMessage;
                    //    lblHeader.Text = "Session Timeout";
                    //    lblErrorMsgHeading.Text = "";
                    //    _SessionInformation.ErrorMessage = null;
                    //    CommonFunctions.SetSessionInformation(_SessionInformation);
                    //}
                    //else
                    //{
                    lblErrorMsgHeading.Text = "Error Message";
                    lblErrorMsgHeading.Visible = false;
                    lblHeader.Text = "Message";
                    lblMsg.Text = CommonConstants.CommonErrorMsg;

                    if (HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers) != null)
                    {
                        List<CurrentUsers> _ListOfCurrentUsers = (List<CurrentUsers>)HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers);

                        CurrentUsers _ExistingUsers = _ListOfCurrentUsers.Find(delegate(CurrentUsers _User) { return _User.SessionID == HttpContext.Current.Session.SessionID; });

                        if (_ExistingUsers != null)
                        {
                            _ExistingUsers.IsActive = false;

                            _ListOfCurrentUsers.Remove(_ExistingUsers);
                        }
                    }

                    CommonConstants.IsLogout = true;
                    Session.Abandon();
                    //}
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
            }
        }
    }
}