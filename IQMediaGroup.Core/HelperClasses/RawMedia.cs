using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais RawMedia Details
    /// </summary>
    [Serializable]
    [XmlType("RawMedia")]
    public class RawMedia
    {
        /// <summary>
        /// Represents RawMediaID
        /// </summary>
        [XmlElement(ElementName = "RawMediaID")]
        public Guid RawMediaID { get; set; }

        /// <summary>
        /// Represents DateTime of RawMedia 
        /// </summary>
        [XmlElement(ElementName = "DateTime")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Represents Station Title
        /// </summary>
        [XmlElement(ElementName = "StationTitle")]
        public string StationTitle { get; set; }

        /// <summary>
        /// Represents Station Logo
        /// </summary>
        [XmlElement(ElementName = "StationLogo")]
        public string StationLogo { get; set; }


        /// <summary>
        /// Represents Number Of Hits On The Search Text
        /// </summary>
        [XmlElement(ElementName = "Hits")]
        public int Hits { get; set; }

        /// <summary>
        /// Represents CacheKey of RawMedia
        /// </summary>
        [XmlElement(ElementName = "CacheKey")]
        public string CacheKey { get; set; }

        /// <summary>
        /// Represents Complete of RawMedia
        /// </summary>
        [XmlElement(ElementName = "Complete")]
        public bool Complete { get; set; }

        /// <summary>
        /// Represents Time Zone Of RawMedia
        /// </summary>
        [XmlElement(ElementName = "TimeZone")]
        public string TimeZone { get; set; }

        /// <summary>
        /// Represents StationID
        /// </summary>
        [XmlElement(ElementName = "StationID")]
        public string StationID { get; set; }

        /// <summary>
        /// Represents Station URL
        /// </summary>
        [XmlElement(ElementName = "StationUrl")]
        public string StationUrl { get; set; }

        /// <summary>
        /// Represents Closed Caption Text
        /// </summary>
        [XmlElement(ElementName = "HitList")]
        public string HitList { get; set; }

        /// <summary>
        /// Represents IQ_Dma_Name
        /// </summary>
        [XmlElement(ElementName = "IQ_Dma_Name")]
        public string IQ_Dma_Name { get; set; }

        /// <summary>
        /// Represents Title120
        /// </summary>
        [XmlElement(ElementName = "Title120")]
        public string Title120 { get; set; }

        /// <summary>
        /// This List of RawMediaCC represents collection of close captions for this RawMedia.
        /// </summary>
        [XmlElement(ElementName = "RawMediaCloseCaptions ")]
        public List<RawMediaCC> RawMediaCloseCaptions { get; set; }

        public string IQ_CC_Key { get; set; }

        /// <summary>
        /// Represents Market
        /// </summary>
        public string Market { get; set; }

        /// <summary>
        /// Represents Affiliate
        /// </summary>
        public string Affiliate { get; set; }

        public string IQNielenseAudience { get; set; }

        public string IQAddShareValue { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public int FullSentiment { get; set; }

    }
}
