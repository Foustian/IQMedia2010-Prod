using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class UnSuspendTVAgentInput
    {
        [DataMember]
        public Int64 SRID { get; set; }

        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public Guid ClientGUID { get; set; }
    }
}
