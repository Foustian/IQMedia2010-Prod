using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Model.Interface
{
    public interface INB_PublicationCategoryModel
    {
        /// <summary>
        /// Select All Publication Category from the Database
        /// </summary>
        /// <returns>DataSet</returns>
        DataSet GetAllPublicationCategory();
    }
}
