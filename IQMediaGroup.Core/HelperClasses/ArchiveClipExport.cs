using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contains Archive Clip Information
    /// </summary>
    [XmlType("ArchiveClip")]
    public class ArchiveClipExport
    {
        /// <summary>
        /// Represents Archive Clip Key
        /// </summary>
        [XmlElement(ElementName = "PMGCLIPID")]
        public int PMGCLIPID { get; set; }
        /// <summary>
        /// Represents ClipID
        /// </summary>
        [XmlElement(ElementName = "Clip_GUID")]
        public Guid Clip_GUID { get; set; }

        /// <summary>
        /// Represents DateTime of RawMedia 
        /// </summary>
        [XmlElement(ElementName = "Clip_Air_Date")]
        public DateTime Clip_Air_Date { get; set; }

        /// <summary>
        /// Represents Station Title
        /// </summary>
        [XmlElement(ElementName = "Clip_Title")]
        public string Clip_Title { get; set; }

        /// <summary>
        /// Represents ID of Customer
        /// </summary>
        [XmlElement(ElementName = "PMGCustomer_ID")]
        public int PMGCustomer_ID { get; set; }

        /// <summary>
        /// Represents Clip Category
        /// </summary>
        [XmlElement(ElementName = "Clip_Category")]
        public string Clip_Category { get; set; }

        /// <summary>
        /// Represents Clip Creation Date
        /// </summary>
        [XmlElement(ElementName = "Clip_Creation_Date")]
        public string Clip_Creation_Date { get; set; }

        /// <summary>
        /// Represents Clip Description
        /// </summary>
        [XmlElement(ElementName = "Clip_Description")]
        public string Clip_Description { get; set; }

        /// <summary>
        /// Represents Clip Close Caption
        /// </summary>
        [XmlElement(ElementName = "Clip_CC")]
        public string Clip_CC { get; set; }

        /// <summary>
        /// Represents Clip ThumbNail Image
        /// </summary>
        [XmlElement(ElementName = "Clip_ThumbNail")]
        public byte[] Clip_ThumbNail { get; set; }

        /// <summary>
        /// Represents Clip Export File Date
        /// </summary>
        [XmlElement(ElementName = "Clip_export_file_date")]
        public string Clip_export_file_date { get; set; }

    }
}
