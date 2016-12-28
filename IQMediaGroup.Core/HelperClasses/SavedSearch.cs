using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class SavedSearch
    {
        public Int32 ID { get; set; }
        public Guid CustomerGUID { get; set; }
        public String IQPremiumSearchRequestXml { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Guid CategoryGuid { get; set; }
        public Boolean IsDefualtSearch { get; set; }
        public Boolean IsIQAgent { get; set; }


    }
}
