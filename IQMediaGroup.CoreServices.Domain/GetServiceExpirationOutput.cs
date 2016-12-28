using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    public class GetServiceExpirationOutput
    {

        public Int16 status { get; set; }
        public string message { get; set; }

        [XmlElement(ElementName = "GUIDList")]
        public ServiceExpirationData GUIDList { get; set; }
    }

    public class ServiceExpirationData
    {
        [XmlElement(ElementName = "lst")]
        public List<ServiceExpiration> listofMediaLocationOutput { get; set; }
    }

    //public class GUIDData
    //{
    //    public Guid guid { get; set; }
    //    public String IQCCKey { get; set; }
    //}
}
