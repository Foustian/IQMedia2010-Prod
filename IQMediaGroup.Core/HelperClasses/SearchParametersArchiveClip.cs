using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    [XmlType("SearchParameters")]
    public class SearchParametersArchiveClip
    {
        [XmlElement(ElementName = "LoopCount")]
        public string LoopCount;

        [XmlElement(ElementName = "ResponseTimeOut")]
        public string ResponseTimeOut;

        [XmlElement(ElementName = "connectionStrings")]
        public string ConnectionString;

        [XmlElement(ElementName = "DebugFileFlag")]
        public string DebugFileFlag;

        [XmlElement(ElementName = "DebugFilePath")]
        public string DebugFilePath;

        [XmlElement(ElementName = "MaxRecords")]
        public string MaxRecords;        
    }
}
