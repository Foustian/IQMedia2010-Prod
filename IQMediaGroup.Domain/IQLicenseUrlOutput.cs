using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Domain
{
    public class IQLicenseUrlOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }
        [XmlArray("ArticleList")]
        [XmlArrayItem("Article")]
        public List<IQLicenseArticleUrl> ArticleList { get; set; }
    }  
}
