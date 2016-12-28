using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;


namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerModel
    /// </summary>
    internal class IQAgentSearchResponceModel : IQMediaGroupDataLayer, ISearchResponceModel
    {
        /// <summary>
        /// Description:This method inserts Search Request  Information.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_SearchResponce">object of SearchResponce</param>
        /// <returns>Search responce key</returns>
        public string InsertSearchResponce(IQAgentSearchResponce _IQAgentSearchResponce)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int64, _IQAgentSearchResponce.SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, _IQAgentSearchResponce.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RawMediaGUID", DbType.Guid, _IQAgentSearchResponce.RawMediaGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@HitsCount", DbType.Int32, _IQAgentSearchResponce.HitsCount, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsComplete", DbType.Boolean, _IQAgentSearchResponce.IsComplete, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CCText", DbType.String, _IQAgentSearchResponce.CCText, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RedllasoStationCode", DbType.String, _IQAgentSearchResponce.RedllasoStationCode, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ResponceDate", DbType.DateTime, _IQAgentSearchResponce.ResponceDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchResponceKey", DbType.Int32, _IQAgentSearchResponce.SearchResponceKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_SearchResponce_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception; 
            }
        }

    }
}
