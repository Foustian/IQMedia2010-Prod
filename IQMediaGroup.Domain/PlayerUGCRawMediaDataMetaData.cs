using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
namespace IQMediaGroup.Domain
{
    [MetadataType(typeof(PlayerUGCRawMediaDataMetaData))]
    [XmlRoot(ElementName = "PlayerInfo")]
    public partial class PlayerUGCRawMediaData
    { 
    
    }

    public class PlayerUGCRawMediaDataMetaData
    {
        public System.DateTime AirDate
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Keywords
        {
            get;
            set;
        }
    }
}