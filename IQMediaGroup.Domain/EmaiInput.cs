using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class EmaiInput
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }
        public string _imagePath { get; set; }
        public string FileID { get; set; }
        public string PageName { get; set; }
    }
}
