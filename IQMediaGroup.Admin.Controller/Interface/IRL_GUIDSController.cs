using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface Model for RL_GUIDS
    /// </summary>
    public interface IRL_GUIDSController
    {
        /// <summary> 
        /// Add RL_GUIDS 
        /// </summary> 
        /// <param name="p_RL_GUIDS">Object of RL_GUIDS</param> 
        /// <returns>RL_GUIDSKey of added record</returns> 

        string AddRL_GUIDS(RL_GUIDS p_RL_GUIDS, DateTime p_RequestDateTime, int p_RequestTime);

        /// <summary> 
        /// This method gets all RL_GUIDS 
        /// </summary> 
        /// <returns> List Of Objects RL_GUIDSKey </returns> 

        List<RL_GUIDS> GetAllRL_GUIDS();

        /// <summary> 
        /// This method updates  RL_GUIDS details 
        /// </summary> 
        /// <param name="p_RL_GUIDS">Object of RL_GUIDS</param> 
        /// <returns>1/0-Updated or not</returns> 

        string UpdateRL_GUIDS(RL_GUIDS p_RL_GUIDS);

        /// <summary> 
        /// This method deletes RL_GUIDS 
        /// </summary> 
        /// <param name="p_RL_GUIDSKey"> RL_GUIDSKey</param> 
        /// <returns>1/0-Deleted or not</returns> 

        string DeleteRL_GUIDS(string p_RL_GUIDSKey);

        /// <summary>
        /// This method gets RL_GUIDS Records by IQ_CC_Key
        /// </summary>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        List<RL_GUIDS> GetRL_GUIDSByIQCCKey(string p_IQ_CC_Key);
    }
}
