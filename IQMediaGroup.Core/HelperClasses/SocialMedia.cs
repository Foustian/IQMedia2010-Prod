using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class SocialMedia
    {

        public List<SM_SourceType> ListOFSourceType {get; set;}

        public List<SM_SourceCategory> ListOFSourceCategory { get; set; }

        public class SM_SourceCategory
        {
            public int ID { get; set; }

            public string Lable { get; set; }

            public string Value { get; set; }

            public int Order { get; set; }
        }

        public class SM_SourceType
        {
            public int ID { get; set; }

            public string Lable { get; set; }

            public string Value { get; set; }

            public int Order { get; set; }
        }
    }
}
