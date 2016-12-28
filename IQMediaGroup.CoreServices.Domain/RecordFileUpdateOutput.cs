using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [Serializable]
    [XmlType("UpdateRecordFile")]
    public class RecordFileUpdateOutput
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
