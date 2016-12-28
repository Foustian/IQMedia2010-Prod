using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [XmlRoot(ElementName="FiveMinStagingOutput")]
    public class FiveMinStagingOutput
    {
        public int status { get; set; }

        public string message { get; set; }

        [XmlElement(ElementName = "FiveMinStaging")]
        public FiveMinStaging fiveMinStaging { get; set; }
    }
}
