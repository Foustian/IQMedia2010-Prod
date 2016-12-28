using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.Xml.XPath;
using System.Configuration;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IQMediaMessenger
{
    public partial class IQMediaMessenger : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             try
             {
                 HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ConfigurationSettings.AppSettings["BlogRSSURL"]);
                 request.Method = WebRequestMethods.Http.Get;
                 HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                 StreamReader reader = new StreamReader(response.GetResponseStream());
                 string _Content = reader.ReadToEnd();
                 response.Close();

                 XDocument _XDocument = XDocument.Parse(_Content);

                 XNamespace atom = "http://www.w3.org/2005/Atom";

                 var feeds = from entry in _XDocument.Descendants(atom + "entry")
                             select new
                             {
                                 Title = (string)entry.Element(atom + "title").Value,
                                 PublishDate = Convert.ToDateTime(entry.Element(atom + "published").Value).ToShortDateString(),
                                 Link = entry.Elements(atom + "link")
                                         .Where(link => (string)link.Attribute("rel") == "alternate")
                                             .Select(link => (string)link.Attribute("href"))
                                         .First(),

                             };

                 rptRSS.DataSource = feeds;
                 rptRSS.DataBind();
             }
             catch (Exception _Exception)
             {
                 lblErrorMessage.Visible = true;
                 lblErrorMessage.Text = "Temporary unavailable!!";
             }
        }
    }
}