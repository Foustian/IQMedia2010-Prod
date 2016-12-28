using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{
    [XmlType("RL_GUIDS")]
    public class RL_GUIDS
    {

        ///
        /// Represents RL_GUIDSKey Field 
        ///
        [XmlElement(ElementName = "RL_GUIDSKey")]
        public Int64? RL_GUIDSKey { get; set; }

        ///
        /// Represents RL_Station_ID Field 
        ///
        [XmlElement(ElementName = "RL_Station_ID")]
        public String RL_Station_ID { get; set; }

        ///
        /// Represents RL_Station_Date Field 
        ///
        [XmlElement(ElementName = "RL_Station_Date")]
        public DateTime RL_Station_Date { get; set; }

        ///
        /// Represents RL_Station_Time Field 
        ///
        [XmlElement(ElementName = "RL_Station_Time")]
        public Int32 RL_Station_Time { get; set; }

        ///
        /// Represents RL_Time_zone Field 
        ///
        [XmlElement(ElementName = "RL_Time_zone")]
        public String RL_Time_zone { get; set; }

        ///
        /// Represents RL_GUID Field 
        ///
        [XmlElement(ElementName = "RL_GUID")]
        public String RL_GUID { get; set; }        

        ///
        /// Represents GMT_Date Field 
        ///
        [XmlElement(ElementName = "GMT_Date")]
        public DateTime GMT_Date { get; set; }

        ///
        /// Represents GMT_Time Field 
        ///
        [XmlElement(ElementName = "GMT_Time")]
        public Int32 GMT_Time { get; set; }

        ///
        /// Represents IQ_CC_Key Field 
        ///
        [XmlElement(ElementName = "IQ_CC_Key")]
        public String IQ_CC_Key { get; set; }

        ///
        /// Represents GUID_Status Field 
        ///
        [XmlElement(ElementName = "GUID_Status")]
        public Boolean GUID_Status { get; set; }

        ///
        /// Represents CreatedBy Field 
        ///
        public String CreatedBy { get; set; }

        ///
        /// Represents ModifiedBy Field 
        ///
        public String ModifiedBy { get; set; }

        ///
        /// Represents CreatedDate Field 
        ///
        public DateTime CreatedDate { get; set; }

        ///
        /// Represents ModifiedDate Field 
        ///
        public DateTime ModifiedDate { get; set; }

        ///
        /// Represents IsActive Field 
        ///
        public Boolean IsActive { get; set; }



    }
}