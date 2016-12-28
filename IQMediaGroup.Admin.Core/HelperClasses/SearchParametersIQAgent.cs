using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    [Serializable]
    [XmlType("SearchParameters")]
    public class SearchParametersIQAgent
    {
        [XmlElement(ElementName = "StartDate")]
        public string StartDate;

        [XmlElement(ElementName = "EndDate")]
        public string EndDate;

        [XmlElement(ElementName = "DaysDuration")]
        public string DaysDuration;

        [XmlElement(ElementName = "HoursDuration")]
        public string HoursDuration;

        [XmlElement(ElementName = "NumResultsPerPage")]
        public string NumResultsPerPage;

        [XmlElement(ElementName = "NumIQ_CC_KeyPerSearch")]
        public string NumIQ_CC_KeyPerSearch;

        /*[XmlElement(ElementName = "NumGUIDSPerSearch")]
        public string NumGUIDSPerSearch;*/

        [XmlElement(ElementName = "connectionStrings")]
        public string ConnectionString;                

        [XmlElement(ElementName = "DebugFileFlag")]
        public string DebugFileFlag;

        [XmlElement(ElementName = "DebugFilePath")]
        public string DebugFilePath;

        [XmlElement(ElementName = "QueryName")]
        public string QueryName;

        [XmlElement(ElementName = "ClientID")]
        public string ClientID;

        [XmlElement(ElementName = "QueryVersion")]
        public string QueryVersion;

        [XmlElement(ElementName = "PMGSearchUrl")]
        public string PMGSearchUrl;

        [XmlElement(ElementName = "IsPMGSearchLog")]
        public bool IsPMGSearchLog;

    }
}
