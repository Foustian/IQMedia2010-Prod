using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Interface
{
    public interface IInboundReportingModel
    {
        /// <summary>
        /// Represents Insert Method
        /// </summary>
        /// <param name="p_InboundReporting"></param>
        /// <returns>PK value of newly inserted record</returns>
        string InsertInboundReporting(InboundReporting p_InboundReporting);
    }
}
