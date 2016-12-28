using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.WebApplication
{
    public partial class MyCliqMedia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

            if (_SessionInformation != null && _SessionInformation.IsLogIn == true)
            {
                UCMyCliqMediaLogin.Visible = false;
                ucLogout.Visible = true;
            }
            else
            {
                ucLogout.Visible = false;
                UCMyCliqMediaLogin.Visible = true;
            }
        }
    }
}