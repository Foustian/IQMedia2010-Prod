using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    [Serializable]
    public class MasterStatSkedProg
    {
        public List<SSP_IQ_Dma_Name> _ListofMarket { get; set; }

        public List<SSP_IQ_Class> _ListofType { get; set; }

        public List<SSP_Station_Affil> _ListofAffil { get; set; }
    }
}
