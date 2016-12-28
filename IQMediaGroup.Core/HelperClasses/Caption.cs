using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class Caption
    {
        public Caption()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Represents Caption Text
        /// </summary>
        public string CaptionString { get; set; }

        /// <summary>
        /// Represnets StartDateTime of Caption
        /// </summary>
        public string StartDateTime { get; set; }

        /// <summary>
        /// Represents StartTime of Caption in Seconds.
        /// </summary>
        public int StartTime { get; set; }

        /// <summary>
        /// Represents EndTime of Caption in Seconds.
        /// </summary>
        public int EndTime { get; set; }
    }
}
