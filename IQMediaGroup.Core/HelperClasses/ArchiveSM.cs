using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class ArchiveSM
    {
        public int ArchiveSMKey { get; set; }

        public string Title { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public Guid CustomerGuid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid ClientGuid { get; set; }

        public Guid CategoryGuid { get; set; }

        public Guid? SubCategory1Guid { get; set; }

        public Guid? SubCategory2Guid { get; set; }

        public Guid? SubCategory3Guid { get; set; }

        public string CategoryNames { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public string ArticleID { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public DateTime? Harvest_Time { get; set; }

        public Int16 Rating { get; set; }

        public Int32 Total { get; set; }

        public string Publication { get; set; }

        public bool IsCompleteAll { get; set; }

        public decimal? IQ_AdShare_Value { get; set; }

        public int? c_uniq_visitor { get; set; }

        public bool IsUrlFound { get; set; }

        public string FeedClass { get; set; }

        public string homeLink { get; set; }
    }
}
