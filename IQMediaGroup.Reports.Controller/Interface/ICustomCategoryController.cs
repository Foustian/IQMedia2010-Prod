using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface ICustomCategoryController
    {
        List<CustomCategory> SelectByClientGUID(Guid p_ClientGUID);
    }
}
