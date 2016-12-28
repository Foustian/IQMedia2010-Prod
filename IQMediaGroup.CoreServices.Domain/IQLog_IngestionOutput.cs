using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [Serializable]
    [XmlType("ErrorLogRecord")]
    public class IQLog_IngestionOutput
    {
        #region Primitive Properties

        public string Message
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        #endregion
    }
}
