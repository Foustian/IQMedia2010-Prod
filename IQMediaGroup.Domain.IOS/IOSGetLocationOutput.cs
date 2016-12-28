using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Domain.IOS
{
    [XmlRoot("Root")]
    public class IOSGetLocationOutput
    {
        public string Media { get; set; }

        public bool IsValidMedia { get; set; }

        public bool IsOldVersion { get; set; }

        public string IOSAppUpdateUrl { get; set; }

        public bool HasException { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
