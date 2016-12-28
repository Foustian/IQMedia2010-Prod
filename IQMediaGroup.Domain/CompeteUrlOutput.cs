using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace IQMediaGroup.Domain
{
    [XmlRoot(ElementName="CompeteOutput")]
    public class CompeteUrlOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }
        
        [XmlArrayItem(ElementName = "CompeteData")]
        public List<CompeteDataUrl> CompeteDataSet { get; set; }
    }

    public class CompeteDataUrl
    {
        public string CompeteURL
        {
            get;
            set;
        }

        public bool IsCompeteAll
        {
            get;
            set;
        }

        public string IQ_AdShare_Value
        {
            get;
            set;
        }

        public string c_uniq_visitor
        {
            get;
            set;
        }
    }
}
