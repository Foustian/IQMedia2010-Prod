using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Services.Config.Sections.Mappings
{
    public class Parameter
    {
        /// <summary>
        /// The URL 'key' of the parameter being passed in the URL string.
        /// </summary>
        [XmlAttribute]
        public string Key { get; set; }

        /// <summary>
        /// The object type the incomming parameter should be casted to.
        /// </summary>
        [XmlAttribute]
        public string Type { get; set; }
    }
}