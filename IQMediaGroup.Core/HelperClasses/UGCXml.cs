using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]    
   [XmlRoot (ElementName="IngestionData")]
    public class UGCXml
    {       
        public class RawInfo
        {
            [XmlElement]
            public string SourceID { get; set; }

            [XmlElement]
            public DateTime AirDate { get; set; }

            [XmlElement]
            public MetaData MetaData { get; set; }

            [XmlElement]
            public bool UGCAutoClip { get; set; }
        }

        public class ClipInfo
        {
            [XmlElement]
            public string Title { get; set; }

            [XmlElement]
            public string Category { get; set; }

            //[XmlElement]
            //public string SubCategory1 { get; set; }

            //[XmlElement]
            //public string SubCategory2 { get; set; }

            //[XmlElement]
            //public string SubCategory3 { get; set; }

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

        [XmlElement (ElementName="RawInfo")]
        public RawInfo _RawInfo { get; set; }

        [XmlElement (ElementName="ClipInfo")]
        public ClipInfo _ClipInfo { get; set; }
    }

   
}