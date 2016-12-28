using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface INB_PublicationCategoryController
    {
        /// <summary>
        /// Select All Publication Category from the Database
        /// </summary>
        /// <returns>returns list of Publication Category</returns>
        List<NB_PublicationCategory> GetAllPublicationCategory();
    }
}
