using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class MasterIQ_Station
    {
        public List<SSP_IQ_Dma_Name> _ListofMarket { get; set; }

        public List<SSP_IQ_Class> _ListofType { get; set; }

        public List<IQ_STATION> _ListofAffil { get; set; }
    }
}
