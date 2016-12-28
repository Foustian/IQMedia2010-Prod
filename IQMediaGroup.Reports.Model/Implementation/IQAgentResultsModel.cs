using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Reports.Model.Interface;
using System.Data;
using System.Data.SqlTypes;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Reports.Model.Base;


namespace IQMediaGroup.Reports.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerModel
    /// </summary>
    internal class IQAgentResultsModel : IQMediaGroupDataLayer, IIQAgentResultsModel
    {
        public DataSet GetIQAgentResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsNielSenData, out string Query_Name, out string SearchTerm)
        {
            try
            {
                Query_Name = string.Empty;
                SearchTerm = string.Empty;

                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQAgentSearchRequestID", DbType.Int32, p_IQAgentSearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FromDate", DbType.DateTime, p_FromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ToDate", DbType.DateTime, p_ToDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfRecordsToDisplay", DbType.Int32, p_NoOfRecordsToDisplay, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsNielSenData", DbType.Boolean, p_IsNielSenData, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, Query_Name, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Output));


                Dictionary<string, string> _OutputParams = null;

                _DataSet = this.GetDataSetWithOutParam("usp_IQAgentResults_SelectReportByDate", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    Query_Name = Convert.ToString(_OutputParams["@Query_Name"]);
                    SearchTerm = Convert.ToString(_OutputParams["@SearchTerm"]);
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
