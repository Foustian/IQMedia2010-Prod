using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{

    
    [MetadataType(typeof(FiveMinStagingMetaData))]    
    public partial class FiveMinStaging
    {

    }

    
    public class FiveMinStagingMetaData
    {
        [XmlAttribute]
        public Nullable<System.Guid> recordFileGuid
        {
            get;
            set;
        }

        [XmlAttribute]
        public Nullable<int> lastMediaSeg
        {
            get;
            set;
        }

        [XmlAttribute]
        public string mediaStatus
        {
            get;
            set;
        }

        [XmlAttribute]
        public string mediaFilename
        {
            get;
            set;
        }
    }
}
