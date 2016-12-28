using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public partial class ArchiveNM : ArchiveMedia
    {
        [DataMember]
        public Nullable<int> Audience
        {
            get;
            set;
        }

        [DataMember(Name = "AudienceValueType")]
        public string AudienceResult
        {
            get;
            set;
        }

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

        [DataMember]
        public Nullable<decimal> IQAdShareValue
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

        [DataMember]
        public string Publication
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

        [DataMember]
        public string Url
        {
            get;
            set;
        }                        
        /*
        [DataMember]
        public string ArticleID
        {
            get;
            set;
        }
         */        
    }
}
