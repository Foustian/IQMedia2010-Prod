using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace IQMediaGroup.WebApplication
{
    public partial class TestForDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            /*FileInfo[] rgFiles = di.GetFiles();
            foreach (FileInfo fi in rgFiles)
            {
                Response.Write( fi.Name+"<br />");
            }*/

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
          /*  FileInfo _FileInfo = new FileInfo("\\\\10.10.1.31\\RL_CLIPS\\Clipfiles\\" + txtR.Text);
            
            Response.Write(_FileInfo.FullName);

            if (_FileInfo.Exists)
            {
                Response.Write("File created");
            }
            else
            {
                
                Response.Write(File.ReadAllBytes("\\\\10.10.1.31\\RL_CLIPS\\Clipfiles\\" + txtR.Text));
                Response.Write("not");
            }*/

            Response.Write(File.ReadAllBytes("\\\\appserver\\programmer\\vishal\\D03D0FD8-DFE4-4835-95F8-0A1F00D21D36.wmv"));
        }
    }
}