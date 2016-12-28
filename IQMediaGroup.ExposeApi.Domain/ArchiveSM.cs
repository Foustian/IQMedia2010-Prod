using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public partial class ArchiveSM : ArchiveMedia
    {
        [DataMember(EmitDefaultValue=false)]
        public Int32? Audience
        {
            get;
            set;
        }

        [DataMember(Name = "AudienceValueType", EmitDefaultValue = false)]
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

        [DataMember(Name = "Comments", EmitDefaultValue = false)]
        public Int32? Comments { get; set; }

        [DataMember(Name="Publication")]
        public string Homelink
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue=false)]
        public Nullable<decimal> IQAdShareValue
        {
            get;
            set;
        }

        [DataMember(Name = "Likes", EmitDefaultValue = false)]
        public Int64? Likes { get; set; }

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

        [DataMember(Name = "Shares", EmitDefaultValue = false)]
        public Int64? Shares { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ThumbUrl { get; set; }

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
    }
}