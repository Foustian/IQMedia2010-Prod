using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IQMediaGroup.Domain
{
    public class CompeteDemographicInput
    {
        public Guid ClientGuid { get; set; }

        [XmlArrayItem("CompeteUrl")]
        public List<string> CompeteUrlSet { get; set; }

        public string SubMediaType { get; set; }
    }
}