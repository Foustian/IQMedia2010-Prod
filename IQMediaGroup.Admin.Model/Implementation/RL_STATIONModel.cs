using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IRL_STATIONModel
    /// </summary>
    internal class RL_STATIONModel : IQMediaGroupDataLayer, IRL_STATIONModel
    {

        /// <summary>
        /// This method gets all RL_STATION Records
        /// </summary>
        /// <returns>DataSet contains RL_STATION detail</returns>
        public DataSet GetAllRL_STATION()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_RL_STATION_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method deletes RL_STATION
        /// </summary>
        /// <param name="p_RL_Stationkey">RL_Stationkey</param>
        /// <returns>1/0-Deleted or not</returns>
        public string DeleteRL_STATION(string p_RL_Stationkey)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RL_Stationkey", DbType.String, p_RL_Stationkey, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_RL_STATION_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method returns all RL_STATION which is Active and Format = 'TV'
        /// </summary>
        /// <returns>DataSet returns all RL_STATION which is Active and Format = 'TV'</returns>
        public DataSet SelectAllTVStation()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_RL_STATION_SelectAllTVStation", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}