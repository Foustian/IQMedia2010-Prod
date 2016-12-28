using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class VideoMetaDataInput
    {
        public Guid? MediaID { get; set; }

        public string Type { get; set; }
    }
}
