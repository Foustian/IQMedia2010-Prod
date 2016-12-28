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
    internal class SMModel : IQMediaGroupDataLayer, ISMModel
    {
        public DataSet GetIQAgent_SMResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsCompeteData, out string Query_Name)
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
                _ListOfDataType.Add(new DataType("@IsCompeteData", DbType.Int32, p_IsCompeteData, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, Query_Name, ParameterDirection.Output));

                _DataSet = this.GetDataSetWithOutParam("usp_IQAgent_SMResults_SelectReportByDate", _ListOfDataType, out _output);

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

        public string InsertArchiveSM(ArchiveSM p_ArchiveSM)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_ArchiveSM.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, p_ArchiveSM.Keywords, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_ArchiveSM.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_ArchiveSM.CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ArchiveSM.ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGuid", DbType.Guid, p_ArchiveSM.CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1Guid", DbType.Guid, p_ArchiveSM.SubCategory1Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2Guid", DbType.Guid, p_ArchiveSM.SubCategory2Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3Guid", DbType.Guid, p_ArchiveSM.SubCategory3Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArticleID", DbType.String, p_ArchiveSM.ArticleID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Content", DbType.String, p_ArchiveSM.Content, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArticleUri", DbType.String, p_ArchiveSM.Url, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Harvest_Time", DbType.DateTime, p_ArchiveSM.Harvest_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Rating", DbType.Int16, p_ArchiveSM.Rating, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@ArchiveSMKey", DbType.Int32, p_ArchiveSM.ArchiveSMKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_ArchiveSM_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }


}