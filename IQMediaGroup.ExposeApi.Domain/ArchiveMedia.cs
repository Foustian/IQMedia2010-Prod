using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Domain
{
     [KnownType(typeof(ArchiveClip))]
     [KnownType(typeof(ArchiveNM))]
     [KnownType(typeof(ArchiveSM))]
     [KnownType(typeof(ArchiveTweets))]
     [KnownType(typeof(ArchiveTVEyes))]
     [KnownType(typeof(ArchiveBLPM))]
    [DataContract]
    public class ArchiveMedia
    {
        [DataMember(Name="SeqID")]
        public long ID
        {
            get;
            set;
        }



        [DataMember]
        public string Content { get; set; }

        [DataMember(Name="CreatedDateTime")]
        public Nullable<System.DateTime> CreatedDate
        {
            get;
            set;
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }        

        [IgnoreDataMember]
        public string HighlightingText { get; set; }


        [DataMember]
        public string Keywords
        {
            get;
            set;
        }

        [DataMember(Name="MediaDateTime")]
        public System.DateTime MediaDate { get; set; }

        [IgnoreDataMember]
        public long MediaID
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public string MediaType { get; set; }        

        [DataMember]
        public Nullable<long> ParentID
        {
            get;
            set;
        }        

        [DataMember(Name="MediaType")]
        public string SubMediaType { get{return _SubMediaType;} set { _SubMediaType = CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonConstants.SubMediaType>(value)); } }
        private string _SubMediaType;                
    }
}
