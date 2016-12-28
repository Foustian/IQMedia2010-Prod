using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface IIQAgent_SearchRequestModel
    {
        void GetAllowedMediaTypesByID(Int32 ID, out bool IsAllowTV, out bool IsAllowNM, out bool IsAllowSM);
    }
}
