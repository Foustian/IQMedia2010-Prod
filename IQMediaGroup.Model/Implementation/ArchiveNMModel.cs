using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Implementation
{
    internal class ArchiveNMModel : IQMediaGroupDataLayer, IArchiveNMModel
    {

        public string InsertArchiveNM(ArchiveNM p_ArchiveClips)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_ArchiveClips.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, p_ArchiveClips.Keywords, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_ArchiveClips.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_ArchiveClips.CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ArchiveClips.ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGuid", DbType.Guid, p_ArchiveClips.CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1Guid", DbType.Guid, p_ArchiveClips.SubCategory1Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2Guid", DbType.Guid, p_ArchiveClips.SubCategory2Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3Guid", DbType.Guid, p_ArchiveClips.SubCategory3Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArticleID", DbType.String, p_ArchiveClips.ArticleID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Content", DbType.String, p_ArchiveClips.Content, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArticleUri", DbType.String, p_ArchiveClips.Url, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Harvest_Time", DbType.DateTime, p_ArchiveClips.Harvest_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Rating", DbType.Int16, p_ArchiveClips.Rating, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@ArchiveNMKey", DbType.Int32, p_ArchiveClips.ArchiveNMKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_ArchiveNM_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertArchiveNMByList(SqlXml p_SqlXml,out int p_Status,out int p_RecordsInserted)
        {
            try
            {
                p_Status = 0;
                p_RecordsInserted = 0;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, p_SqlXml, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int16, p_Status, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@RowCount", DbType.Int32, p_RecordsInserted, ParameterDirection.Output));

                Dictionary<string, string> _OutputParam = new Dictionary<string, string>();

                string _ReturnValue = this.ExecuteNonQuery("usp_ArchiveNM_InsertList", _ListOfDataType, out _OutputParam);

                if (_OutputParam != null && _OutputParam.Keys.Count > 0)
                {
                    p_Status = Convert.ToInt32(_OutputParam["@Status"]);
                    p_RecordsInserted = Convert.ToInt32(_OutputParam["@RowCount"]);
                }

                return _ReturnValue;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArchiveNMBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount)
        {
            try
            {
                p_TotalRecordsCount = 0;
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermTitle", DbType.String, p_SearchTermTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermDesc", DbType.String, p_SearchTermDesc, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermKeyword", DbType.String, p_SearchTermKeyword, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermCC", DbType.String, p_SearchTermCC, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DateFrom", DbType.Date, p_FromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DateTo", DbType.Date, p_ToDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID1", DbType.Guid, p_Category1GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID2", DbType.Guid, p_Category2GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID3", DbType.Guid, p_Category3GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID4", DbType.Guid, p_Category4GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator1", DbType.String, p_CategoryOperator1, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator2", DbType.String, p_CategoryOperator2, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator3", DbType.String, p_CategoryOperator3, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.String, p_ListCustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TotalRecordsCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));


                _DataSet = GetDataSetWithOutParam("usp_ArchiveNM_Search", _ListOfDataType, out _output);

                if (!string.IsNullOrEmpty(_output))
                {
                    p_TotalRecordsCount = Convert.ToInt32(_output);
                }

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteArchiveNM(string p_DeleteArchiveNM)
        {
            try
            {

                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ArchiveNMKeys", DbType.String, p_DeleteArchiveNM, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_ArchiveNM_Delete", _ListOfDataType);

                return _Result;

            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

        public string UpdateArchiveNM(ArchiveNM p_ArchiveNM)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ArchiveNMKey", DbType.Int64, p_ArchiveNM.ArchiveNMKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_ArchiveNM.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, p_ArchiveNM.CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1GUID", DbType.Guid, p_ArchiveNM.SubCategory1Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2GUID", DbType.Guid, p_ArchiveNM.SubCategory2Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3GUID", DbType.Guid, p_ArchiveNM.SubCategory3Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, p_ArchiveNM.Keywords, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_ArchiveNM.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Rating", DbType.Int16, p_ArchiveNM.Rating, ParameterDirection.Input));


                _Result = ExecuteNonQuery("usp_ArchiveMN_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string GetArticlePathByArticleID(string ArticleID)
        {
            try
            {

                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ArticleID", DbType.String, ArticleID, ParameterDirection.Input));

                IDataReader _darareader = GetDataReader("usp_IQCore_NM_SelectArticleFilePathByArticleID", _ListOfDataType);

               if (_darareader.Read())
                   _Result = _darareader.GetString(0);

               return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetArchiveNMByArchiveNMKey(int p_ArchiveNMKey)
        {
            try
            {
                DataSet _DataSet = null;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ArchiveNMKey", DbType.String, p_ArchiveNMKey, ParameterDirection.Input));

                _DataSet = GetDataSet("usp_ArchiveNM_SelectByArchiveNMKey", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateIQCoreNMStatus(string p_ArticleID)
        {
            try
            {

                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ArticleID", DbType.String, p_ArticleID, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_IQCore_NM_UpdateStatusByArticleID", _ListOfDataType);

                return _Result;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataSet GetArchiveNMReportGroupByCategory(Guid p_ClientGUID,DateTime p_Date)
        {
            try
            {
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArticleDate", DbType.Date, p_Date, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveNM_Report", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArchiveNMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid)
        {
            try
            {
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArticleDate", DbType.Date, p_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, p_CategoryGuid, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveNM_SelectByCategory", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
