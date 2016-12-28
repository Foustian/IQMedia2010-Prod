using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using darrenjohnstone.net.FileUpload;
namespace IQMediaGroup.Usercontrol.IQMediaMaster.Upload
{
    public partial class Upload : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack)
            {
                
                string fileName = System.IO.Path.GetFileName(this.upload.Status.UploadedFiles[0].FileName);

                //  let the parent page know we have processed the uplaod
                const string js = "window.parent.onComplete('success', '{0} has been uploaded successfully.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "progress", string.Format(js, fileName), true);
            }
        }
    }
}