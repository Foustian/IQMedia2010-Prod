using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [XmlRoot(ElementName = "OneHourIngestParamOutput")] 
    public class OneHourIngestParamOutput
    {
        public int status { get; set; }

        public string message { get; set; }

        [XmlElement(ElementName = "OneHourIngestParam")]
        public OneHourIngestParam oneHourIngestParam { get; set; }

    }
}
