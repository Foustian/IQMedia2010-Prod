using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class RL_Clip_Fetch
    {
        /// <summary>
        /// Represents Start Time Of The Request
        /// </summary>
        public string Startdatetimestamp { get; set; }

        /// <summary>
        /// Represents WebRequestURL
        /// </summary>
        public string ClipSearchRequest { get; set; }

        /// <summary>
        /// Represents Response
        /// </summary>
        public string ClipSearchresult { get; set; }

        /// <summary>
        /// Represents TimeoutLoop
        /// </summary>
        public string TimeoutLoop { get; set; }

        public List<ArchiveClipInfoRequestResponse> ListOfArchiveClipRequestResponse { get; set; }

        /// <summary>
        /// Represents Databasereadorwritefailure
        /// </summary>
        public string Databasereadorwritefailure { get; set; }

        /// <summary>
        /// Represents End Time Of The Request
        /// </summary>
        public string Enddatetimestamp { get; set; }

    }
}
