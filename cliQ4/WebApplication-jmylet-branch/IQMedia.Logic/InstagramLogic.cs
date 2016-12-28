using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Web.Logic.Base;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace IQMedia.Web.Logic
{
    public class InstagramLogic : ILogic
    {
        public void InsertSources(List<string> tags, List<string> users)
        {
            string sourceXml = null;
            XDocument xdoc = new XDocument(new XElement("Instagram"));

            if (tags != null && tags.Count > 0)
            {
                xdoc.Root.Add(new XElement("Tags",
                    from ele in tags
                    select new XElement("Tag", ele)
                            ));
            }

            if (users != null && users.Count > 0)
            {
                xdoc.Root.Add(new XElement("Users",
                    from ele in users
                    select new XElement("User", ele)
                            ));
            }
            sourceXml = xdoc.ToString();

            InstagramDA instagramDA = (InstagramDA)DataAccessFactory.GetDataAccess(DataAccessType.Instagram);
            instagramDA.InsertSources(sourceXml);
        }
    }
}
