using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    public class IQ_ReportTypeModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Identity { get; set; }

        public string MasterReportType { get; set; }

        public string Description { get; set; }

        public ReportTypeSettings Settings { get; set; }

        public bool IsDefault { get; set; }
    }

    [XmlRoot("Settings")]
    public class ReportTypeSettings
    {
        [XmlElement("ViewPath")]
        public string ViewPath { get; set; }

        [XmlElement("ResultsViewPath")]
        public string ResultsViewPath { get; set; }

        [XmlElement("SPName")]
        public string SPName { get; set; }

        [XmlElement("TemplateType")]
        public string TemplateType { get; set; }
    }
}
