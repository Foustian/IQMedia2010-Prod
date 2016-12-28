using System.Xml.Serialization;

namespace IQMedia.Services.SMS.Config.Sections
{
    [XmlRoot(ElementName = "add")]
    public class Meta
    {
        /// <summary>
        /// Key
        /// </summary>
        [XmlAttribute]
        public string Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>        
        [XmlAttribute]
        public string Value { get; set; }
    }
}
