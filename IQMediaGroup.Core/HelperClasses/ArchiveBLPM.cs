using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class ArchiveBLPM
    {
        public int ArchiveBLPMKey { get; set; }

        public string Pub_Name { get; set; }

        public string BLID { get; set; }

        public string Headline { get; set; }

        public string Description { get; set; }

        public DateTime? PubDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Keywords { get; set; }

        public Int16 Rating { get; set; }

        public String FileLocation { get; set; }

        public String Url { get; set; }

        public Guid CategoryGuid { get; set; }

        public Guid? SubCategory1Guid { get; set; }

        public Guid? SubCategory2Guid { get; set; }

        public Guid? SubCategory3Guid { get; set; }

        public Int32? Circulation { get; set; }

    }


}
