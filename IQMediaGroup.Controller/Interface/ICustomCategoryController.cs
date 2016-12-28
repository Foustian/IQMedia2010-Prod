using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface ICustomCategoryController
    {
        /// <summary>
        /// Add New Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        string InsertCustomCategory(CustomCategory p_CustomCategory);

        /// <summary>
        /// Update Existing Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        string UpdateCustomCategory(CustomCategory p_CustomCategory);

        /// <summary>
        /// Delete Existing Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        string DeleteCustomCategory(Int64 p_CategoryKey);

        /// <summary>
        /// Returns All Category of Particular Client
        /// </summary>
        /// <param name="p_ClientGUID"></param>
        /// <returns></returns>
        List<CustomCategory> SelectByClientID(Int64 p_ClientID);

        /// <summary>
        /// Returns All Category of Particular Client
        /// </summary>
        /// <param name="p_ClientGUID"></param>
        /// <returns></returns>
        List<CustomCategory> SelectByClientGUID(Guid p_ClientGUID);
    }
}
