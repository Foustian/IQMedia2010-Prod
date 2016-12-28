using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Model.Interface
{
    public interface IIQClient_UGCMapModel
    {
        DataSet GetIQClient_UGCMapByClientGUID(Guid p_ClientGUID);
    }
}
