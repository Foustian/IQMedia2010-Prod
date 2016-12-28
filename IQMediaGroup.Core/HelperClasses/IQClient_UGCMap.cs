using System;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQClient_UGCMap
    {
        /// <summary>
        /// Represents IQClient_UGCMapKey
        /// </summary>
        public Int64 IQClient_UGCMapKey { get; set; }

        /// <summary>
        /// Represents SoruceGUID
        /// </summary>
        public Guid SourceGUID { get; set; }

        /// <summary>
        /// Repersents SourceID
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// Represents ClientGUID
        /// </summary>
        public Guid ClientGUID { get; set; }

        /// <summary>
        /// Represents AutoClip_Status
        /// </summary>
        public bool AutoClip_Status { get; set; }

        /// <summary>
        /// Represents CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents IsActive
        /// </summary>
        public bool IsActive { get; set; }

    }
}