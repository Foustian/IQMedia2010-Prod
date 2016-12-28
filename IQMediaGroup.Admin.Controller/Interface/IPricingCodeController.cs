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
    public interface IPricingCodeController
    {
        /// <summary>
        /// This method gets Bill Type Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the Bill Type</param>
        /// <returns>List of object of Bill Type Class</returns>
        List<PricingCode> GetPricingCodeInformation();
    }
}
