using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface Model for RL_GUIDS
    /// </summary>
    public interface IIq_Service_logModel
    {
        /// <summary>
        /// Description:This method will insert Iq_Service_log.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Iq_Service_log">object of Iq_Service_log</param>
        /// <returns>primary key of Iq_Service_log</returns>
        string AddIq_Service_log(Iq_Service_log p_Iq_Service_log);

        /// <summary>
        /// Description:This method will insert Iq_Service_log.
        /// </summary>
        /// <param name="p_Iq_Service_log">object of Iq_Service_log</param>
        /// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        /// <returns>primary key of Iq_Service_log</returns>
        string AddIq_Service_log(Iq_Service_log p_Iq_Service_log, ConnectionStringKeys p_ConnectionStringKeys);
    }
}
