using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class InsertClipTimeSyncInput
    {
        public Guid? ClipGuid { get; set; }

        public int? StartOffset { get; set; }

        public int? EndOffset { get; set; }
    }
}
