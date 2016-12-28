using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    [Serializable]
    public class TVAgentsInput
    {
        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public List<SRID> SRIDList { get; set; }
    }

    public class SRID
    {
        public Int64 ID { get; set; }
    }
}
