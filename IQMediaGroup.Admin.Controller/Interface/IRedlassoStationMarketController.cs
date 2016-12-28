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
    public interface IRedlassoStationMarketController
    {
        /// <summary>
        /// Description:This method gets all RedlassoStationMarket
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>List Of Object of helperclass Station</returns>
        List<RedlassoStationMarket> GetAllRedlassoStationMarket();

        /// <summary>
        /// Description: This Methods Update RedlassoStationMarket Information.
        /// Added By: Maulik Gandhi   
        /// </summary>
        string UpdateRedlassoStationMarket(RedlassoStationMarket _RedlassoStationMarket);

        /// <summary>
        /// Description: This Methods Insert RedlassoStationMarket Information.
        /// Added By: Maulik Gandhi   
        /// </summary>
        string InsertRedlassoStationMarket(RedlassoStationMarket p_RedlassoStationMarket);

        /// <summary>
        /// Description:This method gets all Active RedlassoStationMarket
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>List Of Object of helperclass Station</returns>i
        List<RedlassoStationMarket> GetAllRedlassoAcitveStationMarket();
    }
}
