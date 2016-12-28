using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais Clip Details
    /// </summary>
    [Serializable]
    public class ArchiveClip
    {
        public int ArchiveClipKey { get; set; }
        /// <summary>
        /// Represents ClipID
        /// </summary>
        public Guid ClipID { get; set; }

        /// <summary>
        /// Represents ClientID
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Represents DateTime of RawMedia 
        /// </summary>
        public DateTime? ClipDate { get; set; }

        /// <summary>
        /// Represents Station Title
        /// </summary>
        public string ClipTitle { get; set; }

        /// <summary>
        /// Represents Station Logo
        /// </summary>
        public string ClipLogo { get; set; }

        /// <summary>
        /// Represents FirstName of Customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents ID of Customer
        /// </summary>
        public int CustomerID { get; set; }

        public string Category { get; set; }

        public string CategoryName { get; set; }

        public string SubCategory1Name { get; set; }

        public string SubCategory2Name { get; set; }

        public string SubCategory3Name { get; set; }

        public DateTime? ClipCreationDate { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string ClosedCaption { get; set; }

        public string SearchText { get; set; }

        /// <summary>
        /// Represents ThumbnailImagePath
        /// </summary>
        public string ThumbnailImagePath { get; set; }


        public int gmt_adj { get; set; }

        public int dst_adj { get; set; }

        public int StartOffset { get; set; }

        /// <summary>
        /// Represents Created Date
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents IsActive
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Represents XmlData
        /// </summary>
        public SqlXml XmlData { get; set; }

        public Guid? CategoryGUID { get; set; }

        public Guid? SubCategory1GUID { get; set; }

        public Guid? SubCategory2GUID { get; set; }

        public Guid? SubCategory3GUID { get; set; }

        public Guid? ClientGUID { get; set; }

        public Guid? CustomerGUID { get; set; }

        public string IQ_CC_Key { get; set; }

        public string Audience { get; set; }

        public string Sqad_ShareValue { get; set; }

        public Int16 Rating { get; set; }

        public Int32 Total { get; set; }

        public bool IsActualNielsen { get; set; }

    }
}
