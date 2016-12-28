using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    

    [Serializable]
    [XmlRoot]
    public class UGCMetaData
    {
        [XmlElement(ElementName="IngestionData")]
        public IngestionData IngestionData1 { get; set; }

        public class IngestionData
        {
            public class RawInfo
            {
                [XmlElement]
                public string SourceID { get; set; }

                [XmlElement]
                public DateTime AirDate { get; set; }

                [XmlElement]
                public MetaData MetaData { get; set; }
            }

            public class ClipInfo
            {
                [XmlElement]
                public string Title { get; set; }

                [XmlElement]
                public string Category { get; set; }

                [XmlElement]
                public string Keywords { get; set; }

                [XmlElement]
                public string Description { get; set; }

                [XmlElement]
                public string User { get; set; }

                [XmlElement]
                public MetaData MetaData { get; set; }
            }

            public class MetaData
            {
                [XmlElement(ElementName = "Meta")]
                public List<Meta> ListOfMeta { get; set; }
            }

            public class Meta
            {
                [XmlAttribute]
                public string Key { get; set; }

                [XmlAttribute]
                public string Value { get; set; }
            }

            [XmlElement(ElementName = "RawInfo")]
            public RawInfo _RawInfo { get; set; }

            [XmlElement(ElementName = "ClipInfo")]
            public ClipInfo _ClipInfo { get; set; }
        }
       
    }

    
}
