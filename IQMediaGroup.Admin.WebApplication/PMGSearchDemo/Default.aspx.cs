using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Admin.Controller.Common;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Admin.WebApplication.PMGSearchDemo
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RedlassoScript.LoadScripts(this.Page, Script.RawMedia);
        }
    }
}
