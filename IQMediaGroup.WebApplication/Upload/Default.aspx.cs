using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IQMediaGroup.WebApplication.Upload
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack)
            {
                if (this.upload.Status.UploadedFiles.Count > 0)
                {
                    string fileName = System.IO.Path.GetFileName(this.upload.Status.UploadedFiles[0].FileName);
                    //  let the parent page know we have processed the uplaod
                    const string js = "window.parent.onComplete('success', '{0} has been uploaded successfully.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "progress", string.Format(js, fileName), true);
                }
            }
        }
    }
}