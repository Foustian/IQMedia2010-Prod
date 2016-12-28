using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsvcConsumeService
{
    public class RawMedia
    {        
        
        public System.Guid RawMediaID
        {
            get;
            set;
        }
     
        public string StationLogo
        {
            get;
            set;
        }
       
        public string Title120
        {
            get;
            set;
        }

        public string IQ_Dma_Name
        {
            get;
            set;
        }

        public string DateTime
        {
            get;
            set;
        }

        public Nullable<int> Hits
        {
            get { return _hits; }
            set { _hits = value; }
        }
        private Nullable<int> _hits = 0;

        public string URL
        {
            get;
            set;
        }

        public string IQ_CC_Key
        {
            get;
            set;
        }

        public string Appearing
        {
            get;
            set;
        }        
    }
}
