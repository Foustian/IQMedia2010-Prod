using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class SummaryReportModel
    {
        public Int64 Number_Docs { get; set; }
        public DateTime GMT_DateTime { get; set; }
        public string MediaType { get; set; }
        public Int64 Number_Of_Hits { get; set; }
        public Int64 Audience { get; set; }
        public decimal IQMediaValue { get; set; }
        public string SubMediaType { get; set; }
        public string Query_Name { get; set; }
        public Int64 SearchRequestID { get; set; }
        public int? ThirdPartyDataTypeID { get; set; }
    }
    public class SummaryReportMulti
    {
        public string MediaRecords { get; set; }
        public string SubMediaRecords { get; set; }
        public string TVRecords { get; set; }
        public Int64 TVRecordsSum { get; set; }
        public Int64 TVPrevRecordsSum { get; set; }
        public string NMRecords { get; set; }
        public Int64 NMRecordsSum { get; set; }
        public Int64 NMPrevRecordsSum { get; set; }
        public string TWRecords { get; set; }
        public Int64 TWRecordsSum { get; set; }
        public Int64 TWPrevRecordsSum { get; set; }
        public string ForumRecords { get; set; }
        public Int64 ForumRecordsSum { get; set; }
        public Int64 ForumPrevRecordsSum { get; set; }
        public string AudienceRecords { get; set; }
        public Int64 AudienceRecordsSum { get; set; }
        public Int64 AudiencePrevRecordsSum { get; set; }
        public string SocialMediaRecords { get; set; }
        public Int64 SocialMediaRecordsSum { get; set; }
        public Int64 SocialMediaPrevRecordsSum { get; set; }
        public string BlogRecords { get; set; }
        public Int64 BlogRecordsSum { get; set; }
        public Int64 BlogPrevRecordsSum { get; set; }
        public string IQMediaValueRecords { get; set; }
        public decimal IQMediaValueRecordsSum { get; set; }
        public decimal IQMediaValuePrevRecordsSum { get; set; }
        public string TotalNumOfHits { get; set; }
        public string PMRecords { get; set; }
        public Int64 PMRecordsSum { get; set; }
        public Int64 PMPrevRecordsSum { get; set; }
        public string TMRecords { get; set; }
        public Int64 TMRecordsSum { get; set; }
        public Int64 TMPrevRecordsSum { get; set; }
        public string MSRecords { get; set; }
        public Int64 MSRecordsSum { get; set; }

    }
}
