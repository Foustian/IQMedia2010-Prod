using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class ClipDownload
    {
        /// <summary>
        /// Represents IQ_ClipDownload_Key Primary Key
        /// </summary>
        public Int64 IQ_ClipDownload_Key { get; set; }

        /// <summary>
        /// Represents ClipID
        /// </summary>
        public Guid ClipID { get; set; }

        /// <summary>
        /// Represents Customer GUID
        /// </summary>
        public Guid CustomerGUID { get; set; }

        /// <summary>
        /// Represents ClipDownloadStatus
        /// </summary>
        public Int16 ClipDownloadStatus { get; set; }

        /// <summary>
        /// Represents ClipDLRequestDateTime
        /// </summary>
        public DateTime? ClipDLRequestDateTime { get; set; }

        /// <summary>
        /// Represents ClipDLFormat
        /// </summary>
        public string ClipDLFormat { get; set; }

        /// <summary>
        /// Represents ClipTitle
        /// </summary>
        public string ClipTitle { get; set; }

        /// <summary>
        /// Represents ClipFileLocation
        /// </summary>
        public string ClipFileLocation { get; set; }

        /// <summary>
        /// Represents ClipDownLoadedDateTime
        /// </summary>
        public DateTime? ClipDownLoadedDateTime { get; set; }

        /// <summary>
        /// Represents CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime ModifiedDate { get; set; }


        /// <summary>
        /// Represents flag for active or not
        /// </summary>
        public bool IsActive { get; set; }

    }
}
