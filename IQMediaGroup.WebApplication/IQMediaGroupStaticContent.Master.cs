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

namespace IQMediaGroup.WebApplication
{
    public partial class IQMediaGroupStaticContent : System.Web.UI.MasterPage
    {
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {

            //SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

            //if (_SessionInformation != null && _SessionInformation.IsLogIn == true)
            //{
            //    Login1.Visible = false;
            //    Logout2.Visible = true;
            //}
            //else
            //{
            //    Logout2.Visible = false;
            //    Login1.Visible = true;
            //}

            //hlogo.HRef = "~/";


        }



        #endregion


    }
}
