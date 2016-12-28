using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;

namespace IQMediaGroup.Model.Implementation
{
    internal class ArchiveBLPMModel : IQMediaGroupDataLayer, IArchiveBLPMModel
    {
        public DataSet GetArchiveBLPMBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount)
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
                _ListOfDataType.Add(new DataType("@TotalRecordsCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));


                _DataSet = GetDataSetWithOutParam("usp_ArchiveBLPM_Search", _ListOfDataType, out _output);

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

        public string UpdateArchivePM(ArchiveBLPM p_ArchiveBLPM)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ArchiveBLPMKey", DbType.Int64, p_ArchiveBLPM.ArchiveBLPMKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_ArchiveBLPM.Headline, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_ArchiveBLPM.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, p_ArchiveBLPM.CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1GUID", DbType.Guid, p_ArchiveBLPM.SubCategory1Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2GUID", DbType.Guid, p_ArchiveBLPM.SubCategory2Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3GUID", DbType.Guid, p_ArchiveBLPM.SubCategory3Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, p_ArchiveBLPM.Keywords, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Rating", DbType.Int16, p_ArchiveBLPM.Rating, ParameterDirection.Input));


                _Result = ExecuteNonQuery("usp_ArchiveBLPM_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArchivePMByArchiveBLPMKey(int p_ArchiveBLPMKey)
        {
            try
            {
                DataSet _DataSet = null;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ArchiveBLPMKey", DbType.String, p_ArchiveBLPMKey, ParameterDirection.Input));

                _DataSet = GetDataSet("usp_ArchiveBLPM_SelectByArchiveBLPMKey", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteArchivePM(string p_DeleteArchivePM)
        {
            try
            {

                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ArchiveBLPMKeys", DbType.String, p_DeleteArchivePM, ParameterDirection.Input));
                
                _Result = ExecuteNonQuery("usp_ArchiveBLPM_Delete", _ListOfDataType);

                return _Result;

            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }
    }

}