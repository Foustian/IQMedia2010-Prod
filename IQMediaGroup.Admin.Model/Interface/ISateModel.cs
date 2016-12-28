using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface for Role
    /// </summary>
    public interface IStateModel
    {
        /// <summary>
        /// This method gets State information.
        /// </summary>
        /// <returns></returns>
        DataSet GetStateInfo();
    }
}
