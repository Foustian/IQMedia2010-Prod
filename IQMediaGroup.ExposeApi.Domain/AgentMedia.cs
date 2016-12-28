using System.Runtime.Serialization;
using System;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Domain
{
    [KnownType(typeof(TVAgentResult))]
    [KnownType(typeof(NM))]
    [KnownType(typeof(SM))]
    [KnownType(typeof(TW))]
    [KnownType(typeof(TM))]
    [KnownType(typeof(PM))]
    [KnownType(typeof(PQ))]
    [DataContract]
    public class AgentMedia
    {
        [DataMember(EmitDefaultValue = false)]
        public string Content { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime GMTDateTime { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Highlights { get; set; }

        [DataMember]
        public int? HitCount { get; set; }

        [DataMember(Name = "SeqID")]
        public Int64 ID { get; set; }

        [DataMember]
        public int NegativeSentiment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Int64 ParentID { get; set; }

        [DataMember]
        public int PositiveSentiment { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Int64 SRID { get; set; }

        [DataMember(Name = "MediaType")]
        public string SubMediaType { get { return _SubMediaType; } set { _SubMediaType = CommonFunctions.GetEnumDescription(CommonFunctions.StringToEnum<CommonConstants.SubMediaCategory>(value)); } }
        private string _SubMediaType;

        [DataMember(EmitDefaultValue = false)]
        public string Title { get; set; }        
    }
}