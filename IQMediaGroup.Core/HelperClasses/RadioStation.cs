using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class RadioStation
    {
        public Guid RawMediaID { get; set; }
        public DateTime RawMediaDateTime { get; set; }
        public string RL_Station_ID { get; set; }
        public string dma_name { get; set; }
        public string StationLogo { get; set; }
    }
}
