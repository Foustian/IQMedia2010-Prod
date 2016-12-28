using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IRL_STATIONModel
    /// </summary>
    internal class IQ_STATIONModel : IQMediaGroupDataLayer, IIQ_STATIONModel
    {
        /// <summary>
        /// This method gets all Radio Stations
        /// </summary>
        /// <returns>Dataset contains information of RadioStations</returns>
        public DataSet SelectRadioStations()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_RL_Station_SelectRadioStations", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetAllDetailWithRegion(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                IsAllDmaAllowed = true; IsAllStationAllowed = true; IsAllClassAllowed = true;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAllDmaAllowed", DbType.Boolean, IsAllDmaAllowed, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IsAllStationAllowed", DbType.Boolean, IsAllStationAllowed, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IsAllClassAllowed", DbType.Boolean, IsAllClassAllowed, ParameterDirection.Output));

                Dictionary<string, string> _OutputParams = null;

                _DataSet = this.GetDataSetWithOutParam("usp_STATSKEDPROG_SelectAllWithRegion", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    IsAllDmaAllowed = string.IsNullOrEmpty(_OutputParams["@IsAllDmaAllowed"]) ? true : Convert.ToBoolean(_OutputParams["@IsAllDmaAllowed"]);
                    IsAllStationAllowed = string.IsNullOrEmpty(_OutputParams["@IsAllStationAllowed"]) ? true : Convert.ToBoolean(_OutputParams["@IsAllStationAllowed"]);
                    IsAllClassAllowed = string.IsNullOrEmpty(_OutputParams["@IsAllClassAllowed"]) ? true : Convert.ToBoolean(_OutputParams["@IsAllClassAllowed"]);
                }
                else
                {
                    IsAllDmaAllowed = true; IsAllStationAllowed = true; IsAllClassAllowed = true;
                }

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}