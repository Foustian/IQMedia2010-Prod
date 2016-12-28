using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Domain
{
    [Serializable]
    public class IQLicenseUrlInput
    {
        public Guid ClientGUID { get; set; }
        public string EventName { get; set; }
        [XmlArray("ArticleList")]
        [XmlArrayItem("Article")]
        public List<IQLicenseArticleUrl> ArticleDetails { get; set; }
    }
    
    public class IQLicenseArticleUrl
    {
        public Int64 ArticleID { get; set; }
        public short IQLicense { get; set; }
        public string Url { get; set; }
    }
}
