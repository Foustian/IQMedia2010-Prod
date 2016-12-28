using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Implementation
{
    internal class RedlassoStationModel: IQMediaGroupDataLayer,IRedlassoStationModel
    {
        /// <summary>
        /// This methd gets StationMarket
        /// Added By: Bhavik Barot
        /// </summary>
        public DataSet GetStationMarket()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_RedlassoStation_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets RedlassoStation Information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <returns>Dataset containing RedlassoStationInformation.</returns>
        public DataSet GetRedlassoStationInfo()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_RedlassoStation_Select", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method inserts RedlassoStation Information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_RedlassoStation">Object of RedlassoStation class</param>
        /// <returns>RedlassoStationKey.</returns>
        public string InsertRedlassoStation(RedlassoStation p_RedlassoStation)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RedlassoStationCode", DbType.String, p_RedlassoStation.RedlassoStationCode, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedlassoStationMarketID", DbType.Int64, p_RedlassoStation.RedlassoStationMarketID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedlassoStationTypeID", DbType.Int64, p_RedlassoStation.RedlassoStationTypeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@StationID", DbType.Int64, p_RedlassoStation.StationID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClusterID", DbType.Int64, p_RedlassoStation.ClusterID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@stationCallSgn", DbType.String, p_RedlassoStation.station_call_sign, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedlassoStationKey", DbType.Int64, p_RedlassoStation.RedlassoStationKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_RedlassoStation_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates RedlassoStation.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_RedlassoStation">Object of RedlassoStation class</param>
        /// <returns>RedlassoStationKey.</returns>
        public string UpdateRedlassoStation(RedlassoStation p_RedlassoStation)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RedlassoStationCode", DbType.String, p_RedlassoStation.RedlassoStationCode, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedlassoStationMarketID", DbType.Int64, p_RedlassoStation.RedlassoStationMarketID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedlassoStationTypeID", DbType.Int64, p_RedlassoStation.RedlassoStationTypeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@StationID", DbType.Int64, p_RedlassoStation.StationID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClusterID", DbType.Int64, p_RedlassoStation.ClusterID, ParameterDirection.Input));
                //_ListOfDataType.Add(new DataType("paramstation_call_sign", DbType.String, p_RedlassoStation.station_call_sign, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedlassoStationKey", DbType.Int64, p_RedlassoStation.RedlassoStationKey, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_RedlassoStation_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
