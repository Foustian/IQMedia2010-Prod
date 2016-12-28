using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [XmlRoot(ElementName = "FiveMinStagingCCOutput")]
    public class FiveMinStagingCCOutput
    {
        public int status { get; set; }

        public string message { get; set; }

        [XmlElement(ElementName = "FiveMinStagingCC")]
        public FiveMinStagingCC fiveMinStagingCC { get; set; }
    }
}
