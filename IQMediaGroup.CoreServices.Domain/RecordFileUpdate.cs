using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [Serializable]
    [XmlType("UpdateRecordFile")]
    
    public class RecordFileUpdate
    {
        #region Primitive Properties

        public Nullable<System.Guid> RecordfileID
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public Nullable<int> EndOffset
        {
            get;
            set;
        }

        public Nullable<int> RootPathID
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        #endregion
    }
}
