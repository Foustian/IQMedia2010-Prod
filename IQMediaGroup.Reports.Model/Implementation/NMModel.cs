using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Reports.Model.Base;
using IQMediaGroup.Reports.Model.Interface;

namespace IQMediaGroup.Reports.Model.Implementation
{
    internal class NMModel : IQMediaGroupDataLayer, INMModel
    {
        public DataSet GetIQAgent_NMResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay,Boolean p_IsCompeteData, out string Query_Name)
        {
            try
            {
                Query_Name = string.Empty;

                string _output;

                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQAgentSearchRequestID", DbType.Int32, p_IQAgentSearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FromDate", DbType.DateTime, p_FromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ToDate", DbType.DateTime, p_ToDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfRecordsToDisplay", DbType.Int32, p_NoOfRecordsToDisplay, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsCompeteData", DbType.Boolean, p_IsCompeteData, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, Query_Name, ParameterDirection.Output));

                _DataSet = this.GetDataSetWithOutParam("usp_IQAgent_NMResults_SelectReportByDate", _ListOfDataType, out _output);

                if (!string.IsNullOrEmpty(_output))
                {
                    Query_Name = Convert.ToString(_output);
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