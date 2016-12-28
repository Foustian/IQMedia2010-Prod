using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface for Redlasso Station
    /// </summary>
    public interface IRedlassoStationModel
    {

        DataSet GetStationMarket();

        DataSet GetRedlassoStationInfo();

        string InsertRedlassoStation(RedlassoStation p_RedlassoStation);

        string UpdateRedlassoStation(RedlassoStation p_RedlassoStation);
    }
}
