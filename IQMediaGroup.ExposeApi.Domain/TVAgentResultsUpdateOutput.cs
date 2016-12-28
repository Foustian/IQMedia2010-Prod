using System.Collections.Generic;
using System;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVAgentResultsUpdateOutput
    {
        public string Message { get; set; }

        public int Status { get; set; }

        public Int64 TotalResults { get; set; }

        public bool HasNextPage { get; set; }

        public List<UpdatedTVResult> TVResultList { get; set; }
    }
}