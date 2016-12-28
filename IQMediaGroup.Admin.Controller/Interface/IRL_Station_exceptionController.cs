using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface Model for RL_Station_exception
    /// </summary>
    public interface IRL_Station_exceptionController
    {
	/// <summary> 
	/// Add RL_Station_exception 
	/// </summary> 
	/// <param name="p_RL_Station_exception">Object of RL_Station_exception</param> 
	/// <returns>RL_Station_exceptionKey of added record</returns> 

        string AddRL_Station_exception(RL_Station_exception p_RL_Station_exception, DateTime p_RequestedDate, int p_RequestedTime);

        string AddRL_Station_exception(RL_Station_exception p_RL_Station_exception, bool p_IsMisMatch);


	/// <summary> 
	/// This method deletes RL_Station_exception 
	/// </summary> 
	/// <param name="p_RL_Station_exceptionKey"> RL_Station_exceptionKey</param> 
	/// <returns>1/0-Deleted or not</returns> 

	 string DeleteRL_Station_exception(string  p_RL_Station_exceptionKey);

     List<RL_Station_exception> GetNoPassCountRL_Station_exception(DateTime p_StartDate, DateTime p_EndDate, IQ_Process p_IQ_Process);
    }
}
