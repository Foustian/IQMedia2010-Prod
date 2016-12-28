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
    internal class RedlassoStationMarketModel : IQMediaGroupDataLayer, IRedlassoStationMarketModel
    {

        /// <summary>
        /// This method gets StationMarket
        /// Added By: Bhavik Barot
        /// </summary>
        public DataSet GetRedlassoStationMarket()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_RedlassoStationMarket_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets StationMarket
        /// Added By: Bhavik Barot
        /// </summary>
        public DataSet GetRedlassoActiveStationMarket()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_RedlassoStationMarket_SelectActiveMarket", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates RedlassoStationMarket.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="_RedlassoStationMarket">Object of RedlassoStationMarket class</param>
        /// <returns>RedlassoStationMarketKey</returns>
        public string UpdateRedlassoStationMarket(RedlassoStationMarket _RedlassoStationMarket)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RedlassoStationMarketName", DbType.String, _RedlassoStationMarket.StationMarketName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedlassoStationMarketKey", DbType.Int32, _RedlassoStationMarket.RedlassoStationMarketKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, _RedlassoStationMarket.IsActive, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_RedlassoStationMarket_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method inserts RedlassoStationMarket information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_RedlassoStationMarket">Object of RedlassoStationMarket class.</param>
        /// <returns>RedlassoStationMarketKey.</returns>
        public string InsertRedlassoStationMarket(RedlassoStationMarket p_RedlassoStationMarket)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RedlassoStationMarketName", DbType.String, p_RedlassoStationMarket.StationMarketName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedlassoStationMarketKey", DbType.Int32, p_RedlassoStationMarket.RedlassoStationMarketKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_RedlassoStationMarket_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
