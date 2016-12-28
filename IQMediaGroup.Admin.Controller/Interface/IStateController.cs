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
    public interface IStateController
    {
        /// <summary>
        /// This method gets State Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the State</param>
        /// <returns>List of object of State Class</returns>
        List<State> GetStateInformation();
    }
}
