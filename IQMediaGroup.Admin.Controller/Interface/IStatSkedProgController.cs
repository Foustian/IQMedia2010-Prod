using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for Station Controller
    /// </summary>
    public interface IStatSkedProgController
    {
        /// <summary>
        /// Description: This Methods gets Program Information from DataSet.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_Program">Dataset for Program</param>
        /// <returns>List of object of Program</returns>
        MasterStatSkedProg GetAllDetail();

        /// <summary>
        /// Description:This Methods gets StatSkedProg details By String.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_StatSkedProg">object of StatSkedProg</param>
        /// <param name="p_TotalRowCount">TotalRowCount</param>
        /// <returns>List of object of StatSkedProg</returns>
        List<StatSkedProg> GetDetailsByString(StatSkedProg _StatSkedProg, out int p_TotalRowCount);

        /// <summary>
        /// Description:This Methods gets StatSkedProg details By String With Time.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_StatSkedProg">object of StatSkedProg</param>
        /// <returns>List of object of StatSkedProg</returns>
        List<StatSkedProg> GetDetailsByStringWithTime(StatSkedProg _StatSkedProg);


        MasterStatSkedProg GetAllDetailByClientSettings(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed);


        
    }
}
