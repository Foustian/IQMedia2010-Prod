using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais Clip Details
    /// </summary>
    [Serializable]
    public class Clip
    {
        
        /// <summary>
        /// Represents ClipID
        /// </summary>
        public Guid ClipID { get; set; }

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

        public DateTime? ClipCreationDate { get; set; }       

        public string Description { get; set; }

        /// <summary>
        /// Represents Thumb Nail Image Of The Clip.
        /// </summary>
        public string ClipThumbNailImage { get; set; }

        public Guid? CategoryGUID { get; set; }

        public Guid? CustomerGUID { get; set; }

        public Guid? ClientGUID { get; set; }

        public string ClosedCaption { get; set; }

    }
}
