using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.HeaderRightPanel
{
    public partial class HeaderRightPanel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string sPDFPath = "http://localhost:2281/User Manual - HR.pdf";
            string sPDFPath = "http://" + Request.Url.Host.ToString() + "/User_Manual_HR.pdf";
            //string popupScript = "Javascript: return " + "window.open('" + sPDFPath + "', 'CustomPopUp', " + "'width=600, height=600, menubar=no, resizable=yes');";
            //btninformation.Attributes.Add("onclick", popupScript);
            btninformation.HRef = sPDFPath;
        }
    }
}