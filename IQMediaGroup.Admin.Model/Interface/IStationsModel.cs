using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface for Station
    /// </summary>
    public interface IStationsModel
    {
        /// <summary>
        /// This method inserts Station information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Stations">Object of Station class</param>
        /// <returns>StationKey.</returns>
        string InsertStations(Stations p_Stations);

        /// <summary>
        /// Description: This method Gets the All Station Information.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns></returns>
        DataSet GetStationDetail(Stations p_Stations);

        /// <summary>
        /// This method gets station information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_StationName">StationName</param>
        /// <returns>Dataset containing station information.</returns>
        DataSet GetStations(string p_StationName);
    }
}
