using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace IQMediaGroup.Domain
{
    public class NielsenDemographicInput
    {
        public Guid ClientGuid { get; set; }

        public bool IsGUID { get; set; }

        public List<NielsenDemographicGUID> GUIDList { get; set; }

        public List<NielsenDemographicIQCCKey> IQCCKeyList { get; set; }
    }

    public class NielsenDemographicGUID
    {
        public Guid GUID { get; set; }

        public string IQ_Dma_Num { get; set; }

        public int IQStartPoint { get; set; }
    }

    public class NielsenDemographicIQCCKey
    {
        [Required(AllowEmptyStrings=false)]
        public string IQCCKey { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string IQ_Dma_Num { get; set; }

        public int IQStartPoint { get; set; }
    }

    public class NielsenDemographicIQCCKeyComparer : IEqualityComparer<NielsenDemographicIQCCKey>
    {

        public bool Equals(NielsenDemographicIQCCKey x, NielsenDemographicIQCCKey y)
        {
            if (string.Compare(x.IQCCKey, y.IQCCKey, true) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(NielsenDemographicIQCCKey obj)
        {
            return 1;
        }
    }

    public class NielsenDemographicGUIDComparer : IEqualityComparer<NielsenDemographicGUID>
    {

        public bool Equals(NielsenDemographicGUID x, NielsenDemographicGUID y)
        {
            if (x.GUID == y.GUID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(NielsenDemographicGUID obj)
        {
            return 1;
        }
    }
}
