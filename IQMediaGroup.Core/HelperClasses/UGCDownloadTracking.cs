using System;
namespace IQMediaGroup.Core.HelperClasses
{
    public class UGCDownloadTracking
    {
        /// <summary>
        /// Represents PrimaryKey 
        /// </summary>
        public Int64 UGCDownloadTrackingKey { get; set; }

        /// <summary>
        /// Represents UGCGUID
        /// </summary>
        public Guid UGCGUID { get; set; }

        /// <summary>
        /// Represents CustomerGUID
        /// </summary>
        public Guid CustomerGUID { get; set; }

        /// <summary>
        /// Represents DownloadedDateTime
        /// </summary>
        public DateTime DownloadedDateTime { get; set; }

        /// <summary>
        /// Represents Download success or not
        /// </summary>
        public bool IsDownloadSuccess { get; set; }

        /// <summary>
        /// Represents Description
        /// </summary>
        public string DownloadDescription { get; set; }
    }
}