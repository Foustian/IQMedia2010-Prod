using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using System.Data.SqlTypes;
using IQMediaGroup.Reports.Model.Base;
using IQMediaGroup.Reports.Model.Interface;


namespace IQMediaGroup.Reports.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerModel
    /// </summary>
    internal class ArchiveClipModel : IQMediaGroupDataLayer, IArchiveClipModel
    {

        public DataSet GetArchiveClipReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
        {
            try
            {
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDate", DbType.Date, p_Date, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveClip_Report", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArchiveClipByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsNielSenData)
        {
            try
            {
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDate", DbType.Date, p_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID", DbType.Guid, p_CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsNielSenData", DbType.Boolean, p_IsNielSenData, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_ArchiveClip_SelectByCategory", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArchiveClipByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsNielSenData)
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
                _ListOfDataType.Add(new DataType("@IsNielSenData", DbType.Boolean, p_IsNielSenData, ParameterDirection.Input));

                _DataSet = this.GetDataSetWithOutParam("usp_Report_myiq_ArchiveClip_SelectByDurationNCategory", _ListOfDataType, out _output);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }



    }
}
