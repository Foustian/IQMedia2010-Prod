using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{

    public class Iq_Service_log
    {

        public Int64? Iq_Service_logKey { get; set; }

        public String ModuleName { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public String ServiceCode { get; set; }

        public String ConfigRequest { get; set; }
    }
}