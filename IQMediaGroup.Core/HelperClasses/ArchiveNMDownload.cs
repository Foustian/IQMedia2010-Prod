using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class ArchiveNMDownload
    {
        public Int64 ID { get; set; }

        public string ArticleID { get; set; }

        public Guid CustomerGuid { get; set; }

        public Int16 DownloadStatus { get; set; }

        public DateTime? DLRequestDateTime { get; set; }

        public string Title { get; set; }

        public DateTime? DownLoadedDateTime { get; set; }

        public bool IsActive { get; set; }


        public string FileLocation { get; set; }

        public string PdfSvcStatus { get; set; }
    }
}
