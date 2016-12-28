using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for PromotionalVideo Controller
    /// </summary>
    public interface IPromotionalVideoController
    {
        /// <summary>
        /// Description: This method Gets the Promotional Video Information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_VideoID">VideoID</param>
        /// <returns>Video Information for the VideoID</returns>
        PromotionalVideo GetPromotionalVideoByVideoID();

        /// <summary>
        /// Description: This method Gets the Promotional Video Information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_VideoID">VideoID</param>
        /// <returns>Video Information for the VideoID</returns>
        List<PromotionalVideo> GetPromotionalVideoByPageName(string p_PageName);
    }
}
