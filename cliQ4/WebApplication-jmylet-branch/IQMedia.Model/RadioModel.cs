using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class RadioModel
    {
        public long RL_GUIDSKey { get; set; }
      
        public string IQ_Station_ID { get; set; }

        public string Market { get; set; }

        public DateTime? RL_StationDateTime { get; set; }

        public string RL_GUID { get; set; }
    }
}
