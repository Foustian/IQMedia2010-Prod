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
    public interface IBillFrequencyModel
    {
        /// <summary>
        /// This method gets Bill Type information.
        /// </summary>
        /// <returns></returns>
        DataSet GetBillFrequencyInfo();
    }
}
