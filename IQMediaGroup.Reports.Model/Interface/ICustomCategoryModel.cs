using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface ICustomCategoryModel
    {
        DataSet SelectByClientGUID(Guid p_ClientGUID);
    }
}
