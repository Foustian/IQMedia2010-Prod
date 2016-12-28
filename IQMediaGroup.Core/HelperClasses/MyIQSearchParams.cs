using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class MyIQSearchParams
    {
        public string SearchTitle { get; set; }
        public string SearchDesc { get; set; }
        public string SearchKey { get; set; }
        public string SearchCC { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? CategoryGUID1 { get; set; }
        public Guid? CategoryGUID2 { get; set; }
        public Guid? CategoryGUID3 { get; set; }
        public Guid? CategoryGUID4 { get; set; }
        public string CategoryOperator1 { get; set; }
        public string CategoryOperator2 { get; set; }
        public string CategoryOperator3 { get; set; }
        public string CustomerGUID { get; set; }
    }

    [Serializable]
    public class MyIQReportParams
    {
        public string ReportType { get; set; }

        //public DateTime? ReportDate { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
