using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVAgent
    {
        [DataMember]
        public long SRID
        {
            get;
            set;
        }

        [DataMember]
        [XmlIgnore]
        [System.Runtime.Serialization.IgnoreDataMember]
        public string AgentName
        {
            get;
            set;
        }

        [DataMember]
        [XmlIgnore]
        [System.Runtime.Serialization.IgnoreDataMember]
        public string SearchTerm
        {
            get;
            set;
        }

        [DataMember]
        public Nullable<System.DateTime> CreatedDate
        {
            get;
            set;
        }

        [DataMember]
        public Nullable<System.DateTime> ModifiedDate
        {
            get;
            set;
        }

        [DataMember]
        public SearchRequestAgent SearchRequest { get; set; }
    }
}
