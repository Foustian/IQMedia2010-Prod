using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface for Program
    /// </summary>
    public interface IStatSkedProgModel
    {
        /// <summary>
        /// Description: This method Gets the All Program Information.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>Dataset containing Program details.</returns>
        DataSet GetAllDetail();

        /// <summary>
        /// Description: This method Gets the All Program Information by string.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>Dataset containing Program details.</returns>
        DataSet GetDetailsByString(StatSkedProg _StatSkedProg);

        /// <summary>
        /// Description: This method Gets the All Program Information by string.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>Dataset containing Program details.</returns>
        DataSet GetDetailsByStringWithTime(StatSkedProg _StatSkedProg);

        /// <summary>
        /// Description:This Method will Get Details of StatSkedProg
        /// </summary>
        /// <param name="p_GUIDs">GUID</param>
        /// <returns>Dataset of StatSkedProg</returns>
        DataSet GetDetailByGUIDs(string p_GUIDs);

        /// <summary>
        /// Description: This method Gets the All Program Information by string.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>Dataset containing Program details.</returns>
        DataSet GetDetailsForDefaultSearch(StatSkedProg _StatSkedProg);

        DataSet GetDetailByIQCCKeys(string p_IQCCKeys);

        DataSet GetStatskedprogByParams(StatSkedProg _StatSkedProg);

        DataSet GetAllIQCCKeyByStatskedprogParams(StatSkedProg p_StatSkedProg);

        DataSet GetAllDetailByClientSettings(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed);

        DataSet GetAllDetailWithRegion(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed);

    }
}
