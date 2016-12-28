using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for Customer Controller
    /// </summary>
    public interface IMediaCategoryController
    {
        /// <summary>
        /// Description: This Methods gets Media Category Information from DataSet.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>List of  Media Category Information</returns>
        List<MediaCategory> GetMediaCategoryDetail();
    }
}
