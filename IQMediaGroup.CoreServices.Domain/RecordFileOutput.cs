using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [Serializable]
    [XmlType("RecordFile")]
    public class RecordFileOutput
    {
        #region Primitive Properties

        public string Message
        {
            get;
            set;
        }

        public System.Guid RecordFileGUID
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public short Status
        {
            get;
            set;
        }

        #endregion
    }
}
