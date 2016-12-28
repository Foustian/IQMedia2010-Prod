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

namespace IQMediaGroup.Usercontrol.IQMediaMaster.Logout
{
    public partial class Logout : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            try
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

                int _Service = 0;

                if (ConfigurationManager.AppSettings["ServicesBaseURL"] != null && int.TryParse(ConfigurationManager.AppSettings["ServicesBaseURL"], out _Service))
                {                   
                    if (_Service == 1 || _Service == 2)
                    {
                        IQMedia.Web.Common.Authentication.Logout();
                    }
                }

                _SessionInformation = null;
                Response.Redirect("~/", false);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void lbtnRMyiq_Click(object sender, EventArgs e)
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                if (_SessionInformation == null || _SessionInformation.IsLogIn != true)
                {
                    //_SessionInformation = CommonFunctions.GetSessionInformation()
                    _SessionInformation.ErrorMessage = CommonConstants.SessionTimeOutMsg;
                    CommonFunctions.SetSessionInformation(_SessionInformation);
                    Response.Redirect(CommonConstants.CustomErrorPage,false);
                }
                else
                {
                    Response.Redirect("~/MyClips/",false);
                }
            }
            catch (Exception _Exception)
            {                
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
    }
}