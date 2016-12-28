using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.CoreServices.Domain
{
    public class MediaLocationOutput
    {
        public Guid GUID { get; set; }
        public string FullLocation { get; set; }
        public string MediaStatus { get; set; }
    }
}
