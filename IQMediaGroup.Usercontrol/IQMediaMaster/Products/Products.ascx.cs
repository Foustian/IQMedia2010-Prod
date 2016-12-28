using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.Products
{
    public partial class Products : System.Web.UI.UserControl
    {
        public string baseURL = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            
           // if (ConfigurationManager.AppSettings["MyCliqMediaHost"].Contains(Context.Request.Url.Host))
           // {
           //     baseURL = ConfigurationManager.AppSettings["ServicesBaseURLMyCliqMedia"];
           // }
           // else
            //{
            //    baseURL = ConfigurationManager.AppSettings["ServicesBaseURL"];
            //}
        }
    }
}