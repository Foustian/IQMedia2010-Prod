using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface Model for RL_STATION
    /// </summary>
    public interface IIQ_STATIONModel
    {
        
        /// <summary>
        /// This method gets all Radio Stations
        /// </summary>
        /// <returns>Dataset contains RadioStations Information</returns>
        DataSet SelectRadioStations();
        DataSet GetAllDetailWithRegion(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed);
    }
}
