using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IQMediaGroup.Common.Util
{
    public static class CommonConstants
    {
        public enum formatType
        {
            xml,
            json
        }

        public enum MediaType
        {
            NM,
            SM
        }

        public enum SubMediaType
        { 
            [Description("Blog")]            
            Blog,
            [Description("Facebook")]
            FB,
            [Description("Forum")]
            Forum,
            [Description("Instagram")]
            IG,
            [Description("Online News")]
            NM,
            [Description("Print Media")]
            PM,
            [Description("Print Media Text")]
            PQ,
            [Description("Print Media LexisNexis")]
            LN,
            [Description("Radio")]
            Radio,
            [Description("Social Media")]
            SocialMedia,
            [Description("TV")]
            TV,
            [Description("Twitter")]
            TW
        }

        public enum SubMediaCategory
        {
            [Description("Blog")]
            Blog,
            [Description("Facebook")]
            FB,
            [Description("Forum")]
            Forum,
            [Description("Instagram")]
            IG,
            [Description("Online News")]
            NM,
            [Description("Print Media LexisNexis")]
            LN,
            [Description("Print Media")]
            PM,
            [Description("Print Media Text")]
            PQ,
            [Description("Radio")] //TM
            Radio,
            [Description("Social Media")]
            SocialMedia,
            [Description("TV")]
            TV,
            [Description("Twitter")]
            TW            
        }

        public enum Roles
        {
            v4TV,
            v4NM,
            v4SM,
            v4TW,
            v4TM,
            v4BLPM,
            v4PQ,
            v4Radio,
            v4Dashboard,
            v4Library,
            v4Feeds,
            v4API,
            v4IQAgentSetup,
            NielsenData,
            CompeteData
        }

        public static Dictionary<SubMediaCategory, Roles> SubMediaCategoryRoles = new Dictionary<SubMediaCategory, Roles>(){
            {SubMediaCategory.TV,Roles.v4TV},
            {SubMediaCategory.NM, Roles.v4NM},
            {SubMediaCategory.Blog, Roles.v4SM},
            {SubMediaCategory.Forum, Roles.v4SM},
            {SubMediaCategory.SocialMedia, Roles.v4SM},
            {SubMediaCategory.FB, Roles.v4SM},
            {SubMediaCategory.IG, Roles.v4SM},
            {SubMediaCategory.TW, Roles.v4TW},
            {SubMediaCategory.Radio, Roles.v4TM},
            {SubMediaCategory.PM, Roles.v4BLPM},
            {SubMediaCategory.PQ, Roles.v4PQ}
        };
    }
}
