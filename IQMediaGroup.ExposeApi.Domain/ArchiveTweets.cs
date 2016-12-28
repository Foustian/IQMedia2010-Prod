
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public partial class ArchiveTweets : ArchiveMedia
    {
        [DataMember(Name="Name")]
        public string Actor_DisplayName
        {
            get;
            set;
        }

        [DataMember(Name="Link")]
        public string Actor_Link
        {
            get;
            set;
        }

        [DataMember(Name="Handle")]
        public string Actor_PreferredUserName
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
        public string Title
        {
            get;
            set;
        }             
        /*
        [DataMember]
        public long Actor_FollowersCount
        {
            get;
            set;
        }

        [DataMember]
        public long Actor_FriendsCount
        {
            get;
            set;
        }

        [DataMember]
        public string Actor_Image
        {
            get;
            set;
        }
        */
        
        /*
        [DataMember]
        public long gnip_Klout_Score
        {
            get;
            set;
        }
        */       

        
        /*
        [DataMember]
        public string Tweet_ID
        {
            get;
            set;
        }
         */        
    }
}