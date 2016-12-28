<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;
using System.IO;
using System.Linq;


public class Upload : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/octet-stream";
        //context.Response.Expires = -1;
        try
        {

            HttpPostedFile postedFile = context.Request.Files["Filedata"];
            /*string extention = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.') + 1).ToLower();
            string fileNamewithoutext = postedFile.FileName.Substring(0,postedFile.FileName.LastIndexOf("."));*/
            string savepath = "";
            /*string tempPath = "";


            tempPath = System.Configuration.ConfigurationManager.AppSettings["UGCFileUpLoadLocation"];*/

            savepath = System.Configuration.ConfigurationManager.AppSettings["UGCFileUpLoadLocation"];

            //string tempfileName = postedFile.FileName;

            //string tempfileName = postedFile.FileName;       
            
            //string filename = tempfileName.Substring(0, tempfileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("MMddyyyy_hhmmss") + "." + flv
            string filename = postedFile.FileName;
            //if (!Directory.Exists(savepath))
            //Directory.CreateDirectory(savepath);

            postedFile.SaveAs(savepath + filename);
            context.Response.Write(savepath + filename);
            
            

            context.Response.StatusCode = 200;

        }
        catch (Exception ex)
        {
            context.Response.Write("Error: " + ex.Message);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}