using System;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class UGCRawMedia
    {
        /// <summary>
        /// Represents UGCRawMediaID
        /// </summary>
        public Guid UGCGUID { get; set; }

        /// <summary>
        /// Represents Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Represents Keywords
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Represents Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents CreatedDate
        /// </summary>
        public DateTime CreatedDT { get; set; }

        /// <summary>
        /// Represents CategoryName
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Represents FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents CategoryGUID
        /// </summary>
        public Guid CategoryGUID { get; set; }

        /// <summary>
        /// Represents CategoryGUID
        /// </summary>
        public Guid? SubCategory1GUID { get; set; }

        /// <summary>
        /// Represents CategoryGUID
        /// </summary>
        public Guid? SubCategory2GUID { get; set; }

        /// <summary>
        /// Represents CategoryGUID
        /// </summary>
        public Guid? SubCategory3GUID { get; set; }

        /// <summary>
        /// Represents AirDate
        /// </summary>
        public DateTime AirDate { get; set; }

        /// <summary>
        /// Represents CustomerGUID
        /// </summary>
        public Guid CustomerGUID { get; set; }
    }
}