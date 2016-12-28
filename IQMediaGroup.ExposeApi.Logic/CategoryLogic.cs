using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class CategoryLogic : BaseLogic, ILogic
    {
        public List<Category> GetCategoryList(Guid p_ClientGUID)
        {
            return Context.GetCategoryByClientGuid(p_ClientGUID).ToList();
        }
    }
}
