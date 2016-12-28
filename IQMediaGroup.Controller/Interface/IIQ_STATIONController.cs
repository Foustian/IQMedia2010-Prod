using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Controller.Interface
{
    /// <summary>
    /// Interface Model for RL_STATION
    /// </summary>
    public interface IIQ_STATIONController
    {

        /// <summary>
        /// This method gets all Radio Stations
        /// </summary>
        /// <returns></returns>
        List<IQ_STATION> SelectAllRadioStations();
        MasterIQ_Station GetAllDetailWithRegion(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed);
    }
}
