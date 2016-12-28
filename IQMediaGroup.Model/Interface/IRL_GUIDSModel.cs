using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface Model for RL_GUIDS
    /// </summary>
    public interface IRL_GUIDSModel
    {
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

        /// <summary>
        /// This method gets all RL_GUIDS Records
        /// </summary>
        /// <param name="p_StatSkedProg">Object of Statskedprog</param>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        DataSet GetAllRL_GUIDSByStatskedprogParams(StatSkedProg p_StatSkedProg);

        /// <summary>
        /// This method gets RL_GUIDS by IQCCKey of RadioStations
        /// </summary>
        /// <param name="p_IQCCKey">IQCCKey</param>
        /// <param name="p_PageNumber">PageNumber</param>
        /// <param name="p_PageSize">PageSize</param>
        /// <returns>Dataset contains RL_GUIDS information</returns>
        DataSet GetAllRL_GUIDSByRadioStations(string p_IQCCKey, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsSortDirectionAsc);

        DataSet GetAllRL_GUIDSByRadioStationsByXML(SqlXml p_IQStationIDXML, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsSortDirectionAsc, DateTime p_FromDate, DateTime p_ToDate);
    }
}
