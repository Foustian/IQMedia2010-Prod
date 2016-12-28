using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQAgent_TwitterResult
    {
        public int ID { get; set; }

        public int IQAgentSearchRequestID { get; set; }

        public string actor_link { get; set; }

        public string actor_displayName { get; set; }

        public string actor_preferredName { get; set; }

        public string Summary { get; set; }

        public Int16 gnip_Klout_score { get; set; }

        public Int32 actor_followerscount { get; set; }

        public Int32 actor_friendscount { get; set; }

        public DateTime tweet_postedDateTime { get; set; }

        public string actor_image { get; set; }

        public string tweetid { get; set; }


    }
}
