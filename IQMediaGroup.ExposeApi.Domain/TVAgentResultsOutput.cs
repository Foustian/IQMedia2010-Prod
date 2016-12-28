﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVAgentResultsOutput
    {
        public string Message { get; set; }
        
        public int Status { get; set; }

        public Int64 TotalResults { get; set; }
        
        public bool HasNextPage { get; set; }
        
        public List<TVResult> TVResultList { get; set; }
        
    }
}