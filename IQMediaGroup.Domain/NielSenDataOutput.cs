using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class NielSenDataOutput
    {
        public int Status { get; set; }

        public List<NielSenDataDB> NielSenData { get; set; }
    }
}
