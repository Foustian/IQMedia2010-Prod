using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Services.IOS.ExportClip
{
    public class ExportTask
    {

        public Guid id { get; set; }
        public Guid clipGuid { get; set; }
        public DateTime startDate{ get; set; }

        #region IEquatable<ExportTask> Members

        public bool Equals(ExportTask other)
        {
            return id.Equals(other.id);
        }

        #endregion
    }
}
