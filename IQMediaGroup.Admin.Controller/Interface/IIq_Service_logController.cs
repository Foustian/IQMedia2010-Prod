using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Admin.Core.Enumeration;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface Model for RL_GUIDS
    /// </summary>
    public interface IIq_Service_logController
    {
        /// <summary>
        /// Description:This Method will insert Iq_Service_log
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Iq_Service_log">object of Iq_Service_log</param>
        /// <returns>Primary key of Iq_Service_log</returns>
        string AddRL_GUIDS(Iq_Service_log p_Iq_Service_log);

        /// <summary>
        /// Description:This method will insert Iq_Service_log.
        /// </summary>
        /// <param name="p_Iq_Service_log">object of Iq_Service_log</param>
        /// <param name="p_ConnectionStringKeys">Name Of ConnectonString</param>
        /// <returns>primary key of Iq_Service_log</returns>
        string AddRL_GUIDS(Iq_Service_log p_Iq_Service_log, ConnectionStringKeys p_ConnectionStringKeys);
    }
}
