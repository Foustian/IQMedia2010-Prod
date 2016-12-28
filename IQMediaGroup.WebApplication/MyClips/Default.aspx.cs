using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Common;
using IQMediaGroup.Core.HelperClasses;
using System.IO;

namespace IQMediaGroup.WebApplication.MyClips
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                IQMediaGroupResponsive iQMediaGroupResponsive = (IQMediaGroupResponsive)this.Page.Master;
                iQMediaGroupResponsive.PageTitle = CommonConstants.MYIQPageTitle;
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (_SessionInformation.IsmyIQ == true)
                {
                    //sionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetAllowResponseInBrowserHistory(false);
                    Response.Cache.SetNoStore();
                    Response.Cache.SetExpires(DateTime.MinValue);

                    HeaderTabPanel _HeaderTabPanel = (HeaderTabPanel)this.Page.Master.FindControl(CommonConstants.HeaderTabPanel);

                    _HeaderTabPanel.ActiveTab = CommonConstants.aMYIQ; //CommonConstants.LBtnIQMediaArchieve;
                }
                else
                {
                    Response.Redirect("~/NoRole/?FromUrl=~/MyClips/" , false);
                    Response.Redirect("~/NoRole/", false);
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        //// Serialize view state
        //protected override void SavePageStateToPersistenceMedium(object pageViewState)
        //{
        //    LosFormatter losformatter = new LosFormatter();

        //    StringWriter sw = new StringWriter();
        //    losformatter.Serialize(sw, pageViewState);
        //    string viewStateString = sw.ToString();
        //    byte[] b = Convert.FromBase64String(viewStateString);
        //    b = Compressor.CompressViewState(b);
        //    ScriptManager.RegisterHiddenField(this.Page, "__CUSTOMVIEWSTATE", Convert.ToBase64String(b));
        //}

        //// Deserialize view state
        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    string custState = Request.Form["__CUSTOMVIEWSTATE"];
        //    byte[] b = Convert.FromBase64String(custState);
        //    b = Compressor.DecompressViewState(b);
        //    LosFormatter losformatter = new LosFormatter();
        //    return losformatter.Deserialize(Convert.ToBase64String(b));
        //}   
    }
}
