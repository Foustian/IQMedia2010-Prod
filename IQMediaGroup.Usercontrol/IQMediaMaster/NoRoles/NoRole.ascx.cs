using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.NoRoles
{
    public partial class NoRole : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              //  lblHeader.Text = "Message";
            }
        }

        protected void btnredirect_Click1(object sender, EventArgs e)
        {
            try
            {               
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();                

                if (_SessionInformation.IsiQBasic == true)
                {
                    Response.Redirect("~/ClipandRawMedia", false);
                }
                else if (_SessionInformation.IsmyIQ == true)
                {
                    Response.Redirect("~/MyClips", false);
                }
                else if (_SessionInformation.IsiQAdvance == true)
                {
                    Response.Redirect("~/IQAdvance", false);
                }
                else if (_SessionInformation.IsiQAgent == true)
                {
                    Response.Redirect("~/IQAgent", false);
                }
                else
                {
                    Response.Redirect("~/", false);
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}