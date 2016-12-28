using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public class SM : AgentMedia
    {
        [DataMember(EmitDefaultValue = false)]
        public int Audience { get; set; }

        [DataMember(Name = "AudienceValueType",EmitDefaultValue=false)]
        public string AudienceResult { get; set; }

        [DataMember(Name = "Comments", EmitDefaultValue = false)]
        public Int32? Comments { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQAdShareValue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQProminence { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQProminenceMultiplier { get; set; }

        [DataMember(Name = "Likes", EmitDefaultValue = false)]
        public Int64? Likes { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Publication { get; set; }

        [DataMember(Name = "Shares", EmitDefaultValue = false)]
        public Int64? Shares { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ThumbUrl { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }
    }

    [XmlRoot(ElementName="ArticleStatsModel")]
    public class ArticleStatus
    {
        [DataMember(Name = "Likes", EmitDefaultValue = false)]
        public Int64? Likes { get; set; }

        [DataMember(Name = "Shares", EmitDefaultValue = false)]
        public Int64? Shares { get; set; }

        [DataMember(Name = "Comments", EmitDefaultValue = false)]
        public Int32? Comments { get; set; }
    }
}