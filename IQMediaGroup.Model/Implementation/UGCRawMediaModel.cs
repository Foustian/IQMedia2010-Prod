using System;
using System.Data;
using System.Collections.Generic;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Base;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Implementation
{
    internal class UGCRawMediaModel : IQMediaGroupDataLayer, IUGCRawMediaModel
    {


        public DataSet GetUGCRawMediaBySearch(Guid p_ClientGUID, int p_PageNo, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount, string p_CategoryGUID, string p_CustomerGUID, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTermTitle, string p_SearchTermKeyword, string p_SearchTermDesc, out int p_ErrorNumber)
        {
            try
            {
                p_TotalRecordsCount = 0;
                p_ErrorNumber = -1;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.String, p_CategoryGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.String, p_CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermTitle", DbType.String, p_SearchTermTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermDesc", DbType.String, p_SearchTermDesc, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermKeyword", DbType.String, p_SearchTermKeyword, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UGCDateFrom", DbType.Date, p_FromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UGCDateTo", DbType.Date, p_ToDate, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@TotalRecordsCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));

                _ListOfDataType.Add(new DataType("@ErrorNumber", DbType.Int32, p_ErrorNumber, ParameterDirection.Output));

                Dictionary<string, string> _OutputParams = null;

                _DataSet = this.GetDataSetWithOutParam("usp_UGCRawMedia_Search", _ListOfDataType, out _OutputParams);


                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    if (string.IsNullOrEmpty(_OutputParams["@ErrorNumber"]))
                    {
                        p_ErrorNumber = -1;
                    }
                    else
                    {
                        p_ErrorNumber = Convert.ToInt32(_OutputParams["@ErrorNumber"]);
                    }
                    if(!string.IsNullOrEmpty(_OutputParams["@TotalRecordsCount"]))
                        p_TotalRecordsCount = Convert.ToInt32(_OutputParams["@TotalRecordsCount"]);
                }

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        public DataSet GetUGCRawMediabyUGCGUID(UGCRawMedia _InUGCRawMedia)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@UGCGUID", DbType.Guid, _InUGCRawMedia.UGCGUID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_UGCRawMedia_SelectByUGCGUID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateUGCRawMedia(Guid p_RawMediaID, Guid p_CustomerGUID, Guid p_CategoryGUID, Guid? p_SubCategory1GUID, Guid? p_SubCategory2GUID, Guid? p_SubCategory3GUID, string p_Title, string p_Keyword, string p_Description)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RawMediaID", DbType.Guid, p_RawMediaID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, p_CategoryGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1Guid", DbType.Guid, p_SubCategory1GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2Guid", DbType.Guid, p_SubCategory2GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3Guid", DbType.Guid, p_SubCategory3GUID, ParameterDirection.Input));
                
                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, p_Keyword, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, p_Description, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_UGCRawMedia_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        public string DeleteUGCRawMedia(string p_UGCRawMediaIDs)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RawMediaIDs", DbType.String, p_UGCRawMediaIDs, ParameterDirection.Input));                

                _Result = this.ExecuteNonQuery("usp_UGCRawMedia_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        public string FillRecordsFromCore(Guid p_ClientGUID)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_IQUGCArchive_UpdateFromCore", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        public string GetUGCFilePathByUGCGUID(Guid p_UGCGUID)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@UGCGUID", DbType.Guid, p_UGCGUID, ParameterDirection.Input));

                this.ExecuteScalar("usp_IQCore_RecordfileMeta_SelectFilePathByUGCGUID", _ListOfDataType,ref _Result);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateUGCRawMedia(Guid p_RawMediaID, SqlXml p_MetaData)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RawMediaID", DbType.Guid, p_RawMediaID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MetaData", DbType.Xml, p_MetaData, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_UGCRawMedia_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }
    }
}