using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Web;
using System.Net;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for Industry
    /// </summary>
    public interface IIndustryController
    {
        /// <summary>
        /// This method gets Industry Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the Industry</param>
        /// <returns>List of object of Industry Class</returns>
        List<Industry> GetIndustryInformation();
    }
}
