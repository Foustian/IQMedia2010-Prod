using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Web;
using System.Net;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Controller.Interface
{
    public interface IInboundReportingController
    {
        /// <summary>
        /// Description:This method will insert InboundReporting.
        /// </summary>
        /// <param name="p_InboundReporting">Object of InboundReporting</param>
        /// <returns>Primary key of InboundReporting</returns>
        string InsertInboundReporting(InboundReporting p_InboundReporting);

    }
}
