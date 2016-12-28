using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class ArchiveBLPMDownload
    {
        public Int64 ID { get; set; }

        public Int64 MediaID { get; set; }

        public String Headline { get; set; }

        public Guid CustomerGUID { get; set; }

        public Int16 DownloadStatus { get; set; }

        public String DownloadLocation { get; set; }

        public DateTime DLRequestDateTime { get; set; }

        public Boolean IsActive { get; set; }

    }


}
