using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface Model for RL_STATION
    /// </summary>
    public interface IRL_STATIONModel
    {
        DataSet GetAllRL_STATION();
        string DeleteRL_STATION(string p_RL_Stationkey);
        DataSet SelectAllTVStation();
    }
}
