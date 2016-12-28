using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// Contais Search Details
    /// </summary>
    public class Search
    {
        /// <summary>
        /// Represents Search Parameter
        /// </summary>
        public string SearchText { get; set; }

        public string SearchStartDate { get; set; }

        public string SearchEndDate { get; set; }

        public string TodayDate { get; set; }

        public string Test { get; set; }

        public string StartIndex { get; set; }

        public string SelectedIndex { get; set; }


    }
}
