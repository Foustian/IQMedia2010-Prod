using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface of Category
    /// </summary>
    public interface IMediaCategoryModel
    {
        /// <summary>
        /// Description: This method Gets the All Product Information.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>List of Media Category Detail</returns>
        DataSet GetMediaCategoryDetail();
        
    }
}
