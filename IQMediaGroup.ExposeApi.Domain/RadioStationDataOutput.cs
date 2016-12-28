using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class RadioStationDataOutput
    {
        public string Message { get; set; }
        
        public int Status { get; set; }

        public List<RadioStation> RadioStationList { get; set; }

        
    }
}
