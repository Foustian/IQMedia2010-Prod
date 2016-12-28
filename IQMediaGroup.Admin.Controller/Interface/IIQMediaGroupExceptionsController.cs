using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for Exception
    /// </summary>
    public interface IIQMediaGroupExceptionsController
    {
        /// <summary>
        /// This method adds Exception detail
        /// </summary>
        /// <param name="p_CarSenseExceptions">Exception object of core</param>
        /// <returns>ExceptionKey for added Record</returns>
        string AddIQMediaGroupException(IQMediaGroupExceptions p_IQMediaGroupExceptions);
    }
}
