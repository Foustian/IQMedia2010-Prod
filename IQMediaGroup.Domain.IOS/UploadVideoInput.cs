using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain.IOS
{
    public class UploadVideoInput
    {
        public Guid? UID { get; set; }
        public Guid? CatID { get; set; }
        public string Tags { get; set; }
        public DateTime? VideoDT { get; set; }
        public DateTime? DT { get; set; }
        public string TimeZone { get; set; }
        public string FileName { get; set; }
    }
}
