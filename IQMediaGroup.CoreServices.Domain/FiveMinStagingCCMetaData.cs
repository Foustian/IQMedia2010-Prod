using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{

    [MetadataType(typeof(FiveMinStagingCCMetaData))]
    public partial class FiveMinStagingCC
    {

    }


    public class FiveMinStagingCCMetaData
    {
        [XmlAttribute]
        public Nullable<System.Guid> recordFileGuid
        {
            get;
            set;
        }

        [XmlAttribute]
        public Nullable<int> lastCCTxtSegment
        {
            get;
            set;
        }

        [XmlAttribute]
        public string CCTxtFilename
        {
            get;
            set;
        }

        [XmlAttribute]
        public string ccStatus
        {
            get;
            set;
        }
    }
}
