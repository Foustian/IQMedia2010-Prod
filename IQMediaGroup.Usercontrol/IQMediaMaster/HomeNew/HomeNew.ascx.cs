using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.HomeNew
{
    public partial class HomeNew : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

            if (HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers) != null)
            {
                List<CurrentUsers> _ListOfCurrentUsers = (List<CurrentUsers>)HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers);                

                if (_SessionInformation != null && _SessionInformation.IsLogIn == true)
                {
                    CurrentUsers _ExistingUsers = _ListOfCurrentUsers.Find(delegate(CurrentUsers _User) { return _User.SessionID == HttpContext.Current.Session.SessionID && _User.CustomerKey == _SessionInformation.CustomerKey; });

                    if (_ExistingUsers != null && _ExistingUsers.IsActive == false)
                    {
                        Session.Abandon();
                        _SessionInformation = null;
                        lblMsg.Text = CommonConstants.LogOutMsg;
                        _ListOfCurrentUsers.Remove(_ExistingUsers);
                        mpESessionOut.Show();
                    }
                }

            }

            if (_SessionInformation!=null && _SessionInformation.IsLogIn==true)
            {
                ucLogin.Visible = false;
                ucLogout.Visible = true;
            }
            else
            {
                ucLogout.Visible = false;
                ucLogin.Visible = true;
            }
        }
    }
}