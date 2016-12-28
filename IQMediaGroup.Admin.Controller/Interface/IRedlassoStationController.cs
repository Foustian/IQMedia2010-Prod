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
    public interface IRedlassoStationController
    {
        /// <summary>
        /// Description: This Methods Gets StationMarket.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <returns>List of object of Station Market</returns>
        List<RedlassoStation> GetAllStationMarket();

        /// <summary>
        /// Description: This Methods Gets Redlasso Station Info.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <returns>List of object of RedlassoStation</returns>
        List<RedlassoStation> GetRedlassoStationInfo();

        /// <summary>
        /// Description: This Methods insert RedlassoStation.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_RedlassoStation">objet of RedlassoStation</param>
        /// <returns>Output RedlassoStationKey </returns>
        string InsertRedlassoStation(RedlassoStation p_RedlassoStation);

        /// <summary>
        /// Description: This Methods update RedlassoStation.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_RedlassoStation">objet of RedlassoStation</param>
        string UpdateRedlassoStation(RedlassoStation p_RedlassoStation);
    }
}
