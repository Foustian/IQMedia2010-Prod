using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    [XmlType("SearchParameters")]
    public class SearchParameters
    {
        [XmlElement(ElementName = "StartDate")]
        public string StartDate;

        [XmlElement(ElementName = "EndDate")]
        public string EndDate;

        [XmlElement(ElementName = "Time")]
        public string Time;

        [XmlElement(ElementName = "DBTable")]
        public string  DBTable;

        [XmlElement(ElementName = "LoopCount")]
        public string LoopCount;

        [XmlElement(ElementName = "CCSearchResponseTimeOut")]
        public string CCSearchResponseTimeOut;

        [XmlElement(ElementName = "connectionStrings")]
        public string ConnectionString;

        [XmlElement(ElementName = "DebugFileFlag")]
        public bool DebugFileFlag;

        [XmlElement(ElementName = "DebugFilePath")]
        public string DebugFilePath;

        [XmlElement(ElementName = "CCTextFileInputPath")]
        public string CCTextFileInputPath;

        [XmlElement(ElementName = "CCTextFileOutputPath")]
        public string CCTextFileOutputPath;

        [XmlElement(ElementName = "HourlyEmailTimeFromMinute")]
        public string HourlyEmailTimeFromMinute;

        [XmlElement(ElementName = "HourlyEmailTimeToMinute")]
        public string HourlyEmailTimeToMinute;

        [XmlElement(ElementName = "DailyEmailTimeHours")]
        public string DailyEmailTimeHours;

        [XmlElement(ElementName = "WeeklyEmailDay")]
        public string WeeklyEmailDay;

        [XmlElement(ElementName = "SMTPFromEMail")]
        public string SMTPFromEMail;

        [XmlElement(ElementName = "EmailTemplateFilePath")]
        public string EmailTemplateFilePath;

        [XmlElement(ElementName = "Subject")]
        public string Subject;

    }
}
