using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface of Customer
    /// </summary>
    public interface IPromotionalVideoModel
    {
        /// <summary>
        /// Description: This method Gets the Promotional Video Information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_VideoID">VideoID</param>
        /// <returns>Video Information for the VideoID</returns>
        DataSet GetPromotionalVideoByVideoID();

        /// <summary>
        /// Description: This method Gets the Promotional Video Information By Page Name.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_PageID"></param>
        /// <returns></returns>
        DataSet GetPromotionalVideoByPageName(string p_PageName);
    }
}
