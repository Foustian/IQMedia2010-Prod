using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class NielsenInput
    {
        public Guid? ClientGuid { get; set; }

        public List<RawMedia> RawMediaSet { get; set; }
    }
}
