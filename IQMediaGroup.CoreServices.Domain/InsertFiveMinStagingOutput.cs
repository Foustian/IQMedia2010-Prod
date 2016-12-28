using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Domain
{
    [Serializable]
    public class InsertFiveMinStagingOutput
    {

        #region Primitive Properties

        public string message
        {
            get;
            set;
        }

        public int status
        {
            get;
            set;
        }

        #endregion
    }
}
