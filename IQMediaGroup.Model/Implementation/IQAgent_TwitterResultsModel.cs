using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQAgent_TwitterResultsModel : IQMediaGroupDataLayer, IIQAgent_TwitterResultsModel
    {
        public DataSet GetIQAgentTwitterResultsBySearchRequestID(int p_SearchRequestID, int p_PageSize, int p_PageNumber, string p_SortField, bool p_IsAcending, out int p_TotalRecordCount)
        {
            try
            {
                p_TotalRecordCount = 0;
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int64, p_SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNo", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAcending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TotalRecordsCount", DbType.Int32, p_TotalRecordCount, ParameterDirection.Output));

                Dictionary<string, string> _OutputParams;

                _DataSet = this.GetDataSetWithOutParam("usp_IQAgent_TwitterResults_SelectByIQAgentSearchRequestID", _ListOfDataType, out _OutputParams);
                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    p_TotalRecordCount = Convert.ToInt32(_OutputParams["@TotalRecordsCount"]);
                }

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
