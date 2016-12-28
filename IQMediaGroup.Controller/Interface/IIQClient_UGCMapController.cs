using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQClient_UGCMapController
    {
        IQClient_UGCMap GetIQClient_UGCMapByClientGUID(Guid p_ClientGUID); 
    }
}
