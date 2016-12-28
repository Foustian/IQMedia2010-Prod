using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVAgentsOutput
    {
        public string Message { get; set; }
        
        public int Status { get; set; }     

        public List<TVAgent> TVAgentList { get; set; }
    }
}
