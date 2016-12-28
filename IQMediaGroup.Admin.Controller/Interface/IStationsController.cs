using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for Station Controller
    /// </summary>
    public interface IStationsController
    {
        /// <summary>
        /// Description: This Methods Insert Stations.
        /// Added By: Maulik Gandhi   
        /// </summary>
        string InsertStations(Stations p_Stations);

        /// <summary>
        /// Description: This get all Station Information.
        /// Added By: Maulik Gandhi   
        /// </summary>
        List<Stations> GetStationDetail(Stations p_Stations);

        /// <summary>
        /// Description: This get all Station Information by Station Name.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_StationName">list of object of Stations</param>
        /// <returns>list of object of Stations</returns>
        List<Stations> GetStations(string p_StationName);
    }
}
