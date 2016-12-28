using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Controller.Common;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.WebApplication.IframeRawMedia
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IQMediaScript.LoadScripts(this.Page, Script.RawMedia);
        }
    }
}
