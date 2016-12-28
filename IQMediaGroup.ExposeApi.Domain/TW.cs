using System;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public class TW : AgentMedia
    {
        [DataMember(Name = "Name",EmitDefaultValue=false)]
        public string DisplayName { get; set; }

        [DataMember(EmitDefaultValue=false)]
        public Int64 FollowersCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Int64 FriendsCount { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQProminence { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public decimal IQProminenceMultiplier { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Int64 KloutScore { get; set; }

        [DataMember(Name = "Handle",EmitDefaultValue=false)]
        public string PreferredName { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ThumbUrl { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }
    }
}