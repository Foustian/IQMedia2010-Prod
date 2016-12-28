using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Domain
{
    public class CompeteUrlInput
    {
        public Guid? ClientGuid { get; set; }

        [XmlArrayItem("CompeteUrl")]
        public List<string> CompeteUrlSet { get; set; }
    }
}
