using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface for Redlasso Station
    /// </summary>
    public interface IRedlassoStationMarketModel
    {
        /// <summary>
        /// This method gets StationMarket
        /// Added By: Bhavik Barot
        /// </summary>
        DataSet GetRedlassoStationMarket();

        /// <summary>
        /// This method gets StationMarket
        /// Added By: Bhavik Barot
        /// </summary>
        DataSet GetRedlassoActiveStationMarket();

        /// <summary>
        /// This method updates RedlassoStationMarket.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="_RedlassoStationMarket">Object of RedlassoStationMarket class</param>
        /// <returns>RedlassoStationMarketKey</returns>
        string UpdateRedlassoStationMarket(RedlassoStationMarket _RedlassoStationMarket);

        /// <summary>
        /// This method inserts RedlassoStationMarket information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_RedlassoStationMarket">Object of RedlassoStationMarket class.</param>
        /// <returns>RedlassoStationMarketKey.</returns>
        string InsertRedlassoStationMarket(RedlassoStationMarket p_RedlassoStationMarket);

       
    }
}
