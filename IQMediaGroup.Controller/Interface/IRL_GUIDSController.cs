using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Interface
{
    /// <summary>
    /// Interface Model for RL_GUIDS
    /// </summary>
    public interface IRL_GUIDSController
    {
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

        /// <summary>
        /// This method gets all RL_GUIDs by StatSkedProgParams
        /// </summary>
        /// <param name="p_StatSkedProg">Object of StatSkedProg</param>
        /// <returns>List of objects of RL_GUIDs</returns>
        List<RL_GUIDS> GetAllRL_GUIDSByStatskedprogParams(StatSkedProg p_StatSkedProg);

        List<RadioStation> GetAllRL_GUIDSByRadioStations(string p_IQCCKey, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsSortDirectionAsc, out Int64 p_TotalRecordsCount);

        List<RadioStation> GetAllRL_GUIDSByRadioStationsByXML(SqlXml p_IQStationIDXML, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsSortDirectionAsc, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_TotalRecordsCount);
    }
}
