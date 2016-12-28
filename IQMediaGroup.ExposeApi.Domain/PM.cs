using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public class PM : AgentMedia
    {
        [DataMember(EmitDefaultValue=false)]
        public List<string> Authors { get; set; }
        
        [DataMember(Name = "Audience",EmitDefaultValue=false)]
        public Int64 Circulation { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }

        [DataMember(Name = "Publisher",EmitDefaultValue=false)] // To keep name sync in Archive and Agent
        public string Publication { get; set; }
    }
}