using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface Model for RL_Station_exception
    /// </summary>
    public interface IRL_Station_exceptionModel
    {
        string AddRL_Station_exception(RL_Station_exception p_RL_Station_exception, DateTime p_RequestedDate, int p_RequestedTime);
        string AddRL_Station_exception(RL_Station_exception p_RL_Station_exception, bool p_IsMisMatch);
        string DeleteRL_Station_exception(string p_RL_Station_exceptionKey);
        DataSet GetNoPassCountRL_Station_exception(DateTime p_StartDate, DateTime p_EndDate, IQ_Process p_IQ_Process);
    }
}
