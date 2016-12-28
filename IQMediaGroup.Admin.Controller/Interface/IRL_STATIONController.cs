using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface Model for RL_STATION
    /// </summary>
    public interface IRL_STATIONController
    {

        /// <summary> 
        /// This method gets all RL_STATION 
        /// </summary> 
        /// <returns> List Of Objects RL_Stationkey </returns> 

        List<RL_STATION> GetAllRL_STATION();

        /// <summary> 
        /// This method updates  RL_STATION details 
        /// </summary> 
        /// <param name="p_RL_STATION">Object of RL_STATION</param> 
        /// <returns>1/0-Updated or not</returns> 


        /// <summary> 
        /// This method deletes RL_STATION 
        /// </summary> 
        /// <param name="p_RL_Stationkey"> RL_Stationkey</param> 
        /// <returns>1/0-Deleted or not</returns> 

        string DeleteRL_STATION(string p_RL_Stationkey);

        /// <summary> 
        /// This method gets all RL_STATION hich is active and format is 'TV'
        /// </summary> 
        /// <returns> List Of Objects RL_Stationkey </returns> 

        List<RL_STATION> SelectAllTVStation();
    }
}
