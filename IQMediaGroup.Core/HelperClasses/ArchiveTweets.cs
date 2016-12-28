using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class ArchiveTweets
    {
        public int ArchiveTweets_Key { get; set; }

        public string Actor_DisplayName { get; set; }

        public string Actor_PreferredUserName { get; set; }

        public string Title { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public Int64 Actor_FollowersCount { get; set; }

        public Int64 Actor_FriendsCount { get; set; }

        public string Tweet_Body { get; set; }

        public String Actor_Image { get; set; }

        public DateTime Tweet_PostedDateTime { get; set; }

        public Int64 gnip_Klout_Score { get; set; }

        public Int64 Total { get; set; }

        public String Actor_link { get; set; }

        public Int64 Tweet_ID { get; set; }

        public Guid ClientGuid { get; set; }

        public Guid CustomerGuid { get; set; }

        public Guid CategoryGuid { get; set; }

        public Guid? SubCategory1Guid { get; set; }

        public Guid? SubCategory2Guid { get; set; }

        public Guid? SubCategory3Guid { get; set; }

        public bool IsActive { get; set; }

        public Int16 Rating { get; set; }

        public string CategoryNames { get; set; }
    }
}
