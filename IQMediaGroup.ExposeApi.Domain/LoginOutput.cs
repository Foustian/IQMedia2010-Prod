using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class LoginOutput
    {
        public string Message { get; set; }

        public int Status { get; set; }

        public string SessionID { get; set; }
    }
}
