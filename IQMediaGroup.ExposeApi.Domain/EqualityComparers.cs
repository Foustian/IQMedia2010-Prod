using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class IQ_DmaEqualityComparerByName : IEqualityComparer<Dma>
    {

        public bool Equals(Dma x, Dma y)
        {
            if (string.Compare(x.Name, y.Name, true) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Dma obj)
        {
            //return Convert.ToInt16(obj.Num);
            return 1;
        }
    }

    public class IQ_ClassEqualityComparerByNum : IEqualityComparer<Class>
    {
        public bool Equals(Class x, Class y)
        {
            if (string.Compare(x.Num, y.Num, true) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Class obj)
        {
            //return Convert.ToInt16(obj.Num);
            return 1;
        }
    }

    public class Station_AffilEqualityComparerByName : IEqualityComparer<Affiliate>
    {
        public bool Equals(Affiliate x, Affiliate y)
        {
            if (string.Compare(x.Name, y.Name, true) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Affiliate obj)
        {
            return 1;
        }
    }


}
