using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface INB_NewsCategoryController
    {
        /// <summary>
        /// Select All News Category from the Database
        /// </summary>
        /// <returns>returns list of News Category</returns>
        List<NB_NewsCategory> GetAllNewsCategory();
    }
}
