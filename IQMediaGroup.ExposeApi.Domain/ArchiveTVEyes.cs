using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public partial class ArchiveTVEyes : ArchiveMedia
    {
        [DataMember]
        public Nullable<System.Guid> CategoryGuid
        {
            get;
            set;
        }

        [DataMember]
        public string CategoryName
        {
            get;
            set;
        }        

        [IgnoreDataMember]
        public string DMARank
        {
            get;
            set;
        }        

        [DataMember]
        public string Market
        {
            get;
            set;
        }

        [DataMember]
        public Nullable<short> NegativeSentiment
        {
            get;
            set;
        }

        [DataMember]
        public Nullable<short> PositiveSentiment
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public string StationID
        {
            get;
            set;
        }                

        [DataMember]
        public string Title
        {
            get;
            set;
        }        
    }
}