using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using IQMediaGroup.Common.Util;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public class AgentResultsInput
    {
        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public Int64? SRID { get; set; }

        [DataMember]
        public Int64? SeqID { get; set; }

        [DataMember]
        public int? Rows { get; set; }

        [DataMember]
        public string MediaType { get; set; }
        
        public string SubMediaTypeEnum { get { return !string.IsNullOrWhiteSpace(MediaType) ? CommonFunctions.GetValueFromDescription<IQMediaGroup.Common.Util.CommonConstants.SubMediaCategory>(MediaType) : null; } }
    }
}
