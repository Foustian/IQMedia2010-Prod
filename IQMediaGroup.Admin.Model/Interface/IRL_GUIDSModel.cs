using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface Model for RL_GUIDS
    /// </summary>
    public interface IRL_GUIDSModel
    {
        /// <summary>
        /// This Method adds RL_GUIDS Information.
        /// </summary>
        /// <param name="p_RL_GUIDS">RL_GUIDS Object of core</param>
        /// <returns>RL_GUIDSKey of added record</returns>
        string AddRL_GUIDS(RL_GUIDS p_RL_GUIDS, DateTime p_RequestDate, int p_RequestTime);

        /// <summary>
        /// This method gets all RL_GUIDS Records
        /// </summary>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        DataSet GetAllRL_GUIDS();
        
        /// <summary>
        /// This method updates RL_GUIDS detail
        /// </summary>
        /// <param name="p_RL_GUIDS">Object of RL_GUIDS</param>
        /// <returns>1/0-Updated or not</returns>
        string UpdateRL_GUIDS(RL_GUIDS p_RL_GUIDS);

        /// <summary>
        /// This method deletes RL_GUIDS
        /// </summary>
        /// <param name="p_RL_GUIDSKey">RL_GUIDSKey</param>
        /// <returns>1/0-Deleted or not</returns>
        string DeleteRL_GUIDS(string p_RL_GUIDSKey);

        /// <summary>
        /// This method gets RL_GUIDS Records by IQ_CC_Key
        /// </summary>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        DataSet GetRL_GUIDSByIQCCKey(string p_IQ_CC_Key);
    }
}
