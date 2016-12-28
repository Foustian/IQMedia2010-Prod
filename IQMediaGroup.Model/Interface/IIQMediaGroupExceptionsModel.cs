using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface Model for Exception
    /// </summary>
    public interface IIQMediaGroupExceptionsModel
    {
        /// <summary>
        /// This Method adds Exception detail into table.
        /// </summary>
        /// <param name="p_CarSenseExceptions">Exception class of core</param>
        /// <returns>ExceptionKey for added record.</returns>
        string AddIQMediaGroupException(IQMediaGroupExceptions p_IQMediaGroupExceptions);
    }
}
