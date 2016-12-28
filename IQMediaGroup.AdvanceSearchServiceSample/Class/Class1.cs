using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMediaGroup.AdvanceSearchServiceSample.Class
{
    public partial class RadioRawMedia
    {
        #region Primitive Properties

        public System.Guid RawMediaID
        {
            get;
            set;
        }

        public string StationID
        {
            get;
            set;
        }

        public string Dma_Name
        {
            get;
            set;
        }

        public string DateTime
        {
            get;
            set;
        }

        public string URL
        {
            get { return _uRL; }
            set { _uRL = value; }
        }
        private string _uRL = "URL";

        #endregion
    }
}