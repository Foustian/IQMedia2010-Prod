using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    public class GetMediaLocationOutput
    {
        public int status { get; set; }
        public string message { get; set; }

        [XmlElement(ElementName = "MediaLocations")]
        public MediaLocations mediaLocations { get; set; }
    }

    public class MediaLocations
    {
        [XmlElement(ElementName = "lst")]
        public List<MediaLocation> listofMediaLocationOutput { get; set; }
    }


}
