using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// This class contains Promotional Video details
    /// </summary>
    public class PromotionalVideo
    {
        /// <summary>
        /// Represents Primary Key.
        /// </summary>
        public Guid? VideoKey { get; set; }

        /// <summary>
        /// Represents Path Of The Video File.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Represents Src attribute of the Javascript file.
        /// </summary>
        public string SrcPath { get; set; }

        /// <summary>
        /// Represents Movie attribute of the Javascript file.
        /// </summary>
        public string MoviePath { get; set; }

        /// <summary>
        /// Represents whether to display or not.
        /// </summary>
        public bool IsDisplay { get; set; }

        /// <summary>
        /// Representd DisplayPage Name
        /// </summary>
        public string DisplayPageName { get; set; }

        /// <summary>
        /// Represents status of the video.
        /// </summary>
        public bool IsActive { get; set; }

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
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents Position Of The Video
        /// </summary>
        public string Position { get; set; }
    }
}
