using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;
using System.ServiceProcess;
using System.Configuration;
using System.IO;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using System.Web.UI.HtmlControls;
using System.Threading;

namespace IQMediaGroup.WebApplication
{
    public partial class IQMediaGroupInner : System.Web.UI.MasterPage
    {
        public string PageTitle
        {
            set
            {
                HtmlGenericControl divPageTitle = (HtmlGenericControl)this.Page.Master.FindControl("divPageTitle");

                if (divPageTitle != null)
                {
                    divPageTitle.InnerText = value;
                }
            }
        }

        #region Page Events

        public int SetSMTimeOut
        {
            get
            {
                return ScriptManager1.AsyncPostBackTimeout;
            }
            set
            {
                ScriptManager1.AsyncPostBackTimeout = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
            LinkButton _lbtnRMyiq = (LinkButton)Logout1.FindControl("lbtnRMyiq");

            if (_lbtnRMyiq != null)
            {
                _lbtnRMyiq.Visible = false;
            }

            try
            {

                if (!IsPostBack)
                {
                    InsertInboundParameters();
                }

                #region IMAGE CHANGE
                if (_SessionInformation != null && !string.IsNullOrEmpty(_SessionInformation.CustomeHeaderImage) && _SessionInformation.ISCustomeHeader == true)
                {
                    //if (File.Exists("~/Images/" + _SessionInformation.CustomeHeaderImage))
                    //{
                    imgMainLogo.Src = ConfigurationManager.AppSettings["URLCustomHeader"] + _SessionInformation.CustomeHeaderImage;
                    //}
                }
                #endregion


                if (_SessionInformation == null || _SessionInformation.IsLogIn != true)
                {
                    //_SessionInformation = CommonFunctions.GetSessionInformation()
                    _SessionInformation.ErrorMessage = CommonConstants.SessionTimeOutMsg;
                    CommonFunctions.SetSessionInformation(_SessionInformation);

                    Response.Redirect(CommonConstants.CustomErrorPage, true);

                }

                if (HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers) != null)
                {
                    List<CurrentUsers> _ListOfCurrentUsers = (List<CurrentUsers>)HttpContext.Current.Cache.Get(CommonConstants.CurrentUsers);

                    //CurrentUsers _ExistingUsers = _ListOfCurrentUsers.Find(delegate(CurrentUsers _User) { return _User.SessionID == HttpContext.Current.Session.SessionID && _User.IsActive == false; });

                    CurrentUsers _ExistingUsers = _ListOfCurrentUsers.Find(delegate(CurrentUsers _User) { return _User.SessionID == HttpContext.Current.Session.SessionID && _User.CustomerKey == _SessionInformation.CustomerKey; });

                    if (_ExistingUsers != null && _ExistingUsers.IsActive == false)
                    {
                        //Session.Abandon();
                        //CommonConstants.IsLogout = true;                     

                        Response.Redirect(CommonConstants.HomePage, true);

                    }
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetAllowResponseInBrowserHistory(false);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.MinValue);

                hlogo.HRef = "~/";
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception _Exception)
            {

                BaseControl _BaseControl = new BaseControl();
                _BaseControl.WriteException(_Exception);
            }

        }
        #endregion

        #region User Defined Functions

        private void InsertInboundParameters()
        {
            try
            {
                ControllerFactory _ControllerFactory = new ControllerFactory();
                IInboundReportingController _IInboundReportingController = _ControllerFactory.CreateObject<IInboundReportingController>();

                System.Collections.IEnumerator en = Request.ServerVariables.Keys.GetEnumerator();
                en.MoveNext();

                string strServerVariables = string.Empty;
                strServerVariables += "<ServerVariables>";
                foreach (string strKey in Request.ServerVariables.Keys)
                {
                    strServerVariables += "<" + strKey + ">";
                    strServerVariables += Request.ServerVariables[strKey];
                    strServerVariables += "</" + strKey + ">";
                }
                strServerVariables += "</ServerVariables>";

                string strHTMLFormPost = string.Empty;
                strHTMLFormPost += "<HTMLFormPostValue>";
                foreach (string strKey in Request.Form.Keys)
                {
                    strServerVariables += "<" + strKey + ">";
                    strServerVariables += Request.Form[strKey];
                    strServerVariables += "</" + strKey + ">";
                }
                strHTMLFormPost += "</HTMLFormPostValue>";

                string strQuerystring = string.Empty;
                strQuerystring += "<QueryStringValue>";
                foreach (string strKey in Request.QueryString.Keys)
                {
                    strQuerystring += "<" + strKey + ">";
                    strQuerystring += Request.QueryString[strKey];
                    strQuerystring += "</" + strKey + ">";
                }
                strQuerystring += "</QueryStringValue>";

                InboundReporting _InboundReporting = new InboundReporting();
                _InboundReporting.RequestCollection = "<InboundParams>" + strServerVariables + strHTMLFormPost + strQuerystring + "</InboundParams>";
                _IInboundReportingController.InsertInboundReporting(_InboundReporting);

            }
            catch (Exception _Exception)
            {

            }
        }

        #endregion

        #region Button Events
        protected void ImgbtnContectUs_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/" + "Contact/");
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        protected void lnkProducts_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "Product/");
        }

        protected void lnkAboutUs_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "AboutUs/");
        }

        protected void lnkCareer_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "Careers/");
        }

        protected void lnkContactUs_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + "ContactUs/");
        }

        #endregion

    }
}
