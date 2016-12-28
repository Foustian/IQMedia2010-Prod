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
    internal class ArchiveNMModel : IQMediaGroupDataLayer, IArchiveNMModel
    {
        public DataSet GetArchiveNMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
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

        public DataSet GetArchiveNMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsCompeteData)
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
                _ListOfDataType.Add(new DataType("@IsCompeteData", DbType.Boolean, p_IsCompeteData, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveNM_SelectByCategory", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

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

        public DataSet GetArchiveNMByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData)
        {
            try
            {
                string _output;

                DataSet _DataSet = null;
                Guid? catGUID = null;

                if (p_CategoryGuid != Guid.Empty)
                {
                    catGUID = p_CategoryGuid;
                }

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, catGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsCompeteData", DbType.Boolean, p_IsCompeteData, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_Report_myiq_ArchiveNM_SelectByDurationNCategory", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
