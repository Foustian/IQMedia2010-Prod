using System.Collections;
using System.Collections.Generic;
using System;

namespace IQMediaGroup.Domain
{
    public class VideoCategoryDataOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public Dictionary<string,Guid> Category { get; set; }
    }
}
