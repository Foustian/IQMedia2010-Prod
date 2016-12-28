using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMediaGroup.AdvanceSearchServiceSample.Class
{
    public class CurrentSearch
    {

        public Int32 TotalRecords { get; set; }
        public Int32 PageNumber { get; set; }
        public Int32 PageSize { get; set; }
        public bool HasNextPage { get; set; }
        public string Sortfield { get; set; }
    }
}