using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface IIQAgent_SearchRequestController
    {
        void GetAllowedMediaTypesByID(int ID, out bool IsAllowTV, out bool IsAllowNM, out bool IsAllowSM);
    }
}
