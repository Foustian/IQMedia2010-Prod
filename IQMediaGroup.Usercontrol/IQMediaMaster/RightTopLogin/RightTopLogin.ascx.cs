using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Interface;
using System.Configuration;


namespace IQMediaGroup.Usercontrol.IQMediaMaster.RightTopLogin
{
    public partial class RightTopLogin : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        static string _Logout = "Logout ";
        static string _Login = "Login ";

        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (_SessionInformation != null && _SessionInformation.IsLogIn == true)
                {
                    lnkLogin.Visible = true;
                    lnkLogin.Text = _Logout;                   
                }
                else
                {
                    lnkLogin.Visible = false;
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);

            }
        }
        #endregion

        #region "Button Events"
        protected void lnkLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (lnkLogin.Text == _Logout)
                {
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

                    SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                    CommonConstants.IsLogout = true;
                    Session.Abandon();
                    _SessionInformation = null;
                    Response.Redirect("~/", false);
                }
                else if (lnkLogin.Text == _Login)
                {
                    Response.Redirect("~/", false);
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
    
        #endregion
    }
}