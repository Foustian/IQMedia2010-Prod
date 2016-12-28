using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [XmlRoot(ElementName = "FiveMinIngestParamOutput")] 
    public class FiveMinIngestParamOutput
    {
        public int status { get; set; }

        public string message { get; set; }

        [XmlElement(ElementName = "FiveMinIngestParam")]
        public FiveMinIngestParam fiveMinIngestParam { get; set; }
    }
}
