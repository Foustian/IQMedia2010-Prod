using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IQMediaGroup.WebApplication.Report
{
    public partial class MyIQ : System.Web.UI.Page
    {
         protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                if (_SessionInformation.IsMyIQnew == true)
                {
                }
                else
                {
                    (UCMyIQReport1.FindControl("lblReportErr") as Label).Text = "You are not authorized to view this page";
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}