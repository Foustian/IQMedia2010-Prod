using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Common;

namespace IQMediaGroup.WebApplication
{
    public partial class Default_old : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {               
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                if (_SessionInformation.IsLogIn == false)
                {
                    IQMediaScript.LoadScripts(this.Page, Script.Login);

                }               
            }
            catch (Exception _Exception)
            {
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
    }
}
