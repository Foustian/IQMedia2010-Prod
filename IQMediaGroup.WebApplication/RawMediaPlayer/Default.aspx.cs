using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Common;

namespace IQMediaGroup.WebApplication.RawMediaPlayer
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IQMediaScript.LoadScripts(this.Page,Script.RawMedia);
        }
    }
}
