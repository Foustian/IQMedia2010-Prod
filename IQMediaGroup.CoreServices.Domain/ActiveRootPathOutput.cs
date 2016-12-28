using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.CoreServices.Domain
{
    public class ActiveRootPathOutput
    {
        public int  status { get; set; }
        public string message { get; set; }
        public List<RootPath> rootpaths { get; set; }
    }
}
