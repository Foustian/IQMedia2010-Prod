using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class VideoMetaDataOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public dynamic VideoMetaData { get; set; }
    }
}
