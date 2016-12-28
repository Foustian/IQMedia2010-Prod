using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;


namespace IQMediaGroup.Domain
{
    [MetadataType(typeof(PlayerClipDataMetaData))]    
    [XmlRoot(ElementName="PlayerInfo")]
    public partial class PlayerClipData
    {

    }
    
    public class PlayerClipDataMetaData
    { 
        public string IQ_Dma_Name
        {
            get;
            set;
        }
      
        public string Title120
        {
            get;
            set;
        }
       
        public System.DateTime IQ_Local_Air_Date
        {
            get;
            set;
        }
        
        public string StationID
        {
            get;
            set;
        }

        public object IQ_Dma_Num
        {
            get;
            set;
        }
    }

}
