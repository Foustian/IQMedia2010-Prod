using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [Serializable]
    [XmlType("ErrorLogRecord")]
    public class IQLog_IngestionInput
    {
        //public Int32 ID
        //{
        //    get;
        //    set;
        //}

        public String StationID
        {
            get;
            set;
        }

        public String IQ_CC_Key
        {
            get;
            set;
        }


        public string MediaType
        {
            get;
            set;


        }


        public String Level
        {
            get;
            set;
        }

        public String Logger
        {
            get;
            set;
        }

        public String LogMessage
        {
            get;
            set;
        }

    }
}
