using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class IQTimeSyncModel
    {
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public int S { get; set; }
        public double A { get; set; }
        public double V { get; set; }
    }
}
