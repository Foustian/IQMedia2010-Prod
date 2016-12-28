using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace IQMediaGroup.Domain
{   
    [XmlRoot("PlayerInfo")]
    public class PlayerRawMediaData
    {
        public string IQ_Dma_Name
        {
            get;
            set;
        }

        public DateTime? IQ_Local_Air_Date
        {
            get;
            set;
        }

        [XmlArray(ElementName = "Title120s")]
        [XmlArrayItem(ElementName = "Title120")]
        public List<string> Title120s
        {
            get;
            set;
        }

        [XmlArray(ElementName = "IQ_Start_Points")]
        [XmlArrayItem(ElementName = "IQ_Start_Point")]
        public List<int?> IQ_Start_Points
        {
            get;
            set;
        }

        [XmlArray(ElementName = "IQ_Start_Minutes")]
        [XmlArrayItem(ElementName = "IQ_Start_Minute")]
        public List<int?> IQ_Start_Minutes
        {
            get;
            set;
        }

        public string IQ_Dma_Num
        {
            get;
            set;
        }

        public string StationID { get; set; }

        public string StationAffiliate { get; set; }

        public string StationCallSign { get; set; }

        public string TimeZone { get; set; }
    }
}