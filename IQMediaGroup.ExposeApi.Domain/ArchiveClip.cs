using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public partial class ArchiveClip : ArchiveMedia
    {
        [DataMember]
        public Nullable<int> Audience
        {
            get;
            set;
        }

        [DataMember(Name="AudienceValueType")]
        public string AudienceResult { get; set; }

        [DataMember]
        public string CategoryName
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

        [IgnoreDataMember]
        public System.Guid ClipID
        {
            get;
            set;
        }

        [DataMember]
        public string ClipMeta
        {
            get;
            set;
        }

        [DataMember]
        public string ClipURL { get; set; }        

        [IgnoreDataMember]
        public string Dma_Num
        {
            get;
            set;
        }

        [DataMember]
        public string EmbedLink { get; set; }

        [DataMember]
        public Nullable<decimal> IQAdShareValue
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

        [DataMember]
        public string Station { get; set; }

        [DataMember(Name="Affiliate")]
        public string Station_Affil
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
        public string TimeZone
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

        [DataMember(EmitDefaultValue=false)]
        public int? StartOffset { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? EndOffset { get; set; }
        
        [DataMember]
        public string ThumbnailURL { get; set; }
    }
}
