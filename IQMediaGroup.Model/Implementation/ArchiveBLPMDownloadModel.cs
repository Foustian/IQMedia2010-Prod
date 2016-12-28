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
    internal class ArchiveBLPMDownloadModel : IQMediaGroupDataLayer, IArchiveBLPMDownloadModel
    {
        public DataSet GetByCustomerGuid(Guid p_CustomerGuid)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_ArchiveBLPMDownload_SelectByCustomerGuid", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertListArchivePMDownload(Guid p_CustomerGuid, SqlXml p_XmlData)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, p_XmlData, ParameterDirection.Input));
                _Result = ExecuteNonQuery("usp_ArchiveBLPMDownload_InsertList", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }       

        public DataSet GetArchivePMDownload(Guid customerGuid)
        {
            try
            {
                DataSet _Result;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, customerGuid, ParameterDirection.Input));
                _Result = GetDataSet("usp_ArchiveBLPMDownload_Select", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateArchivePMDownload(Int64 id,Int16 downloadStatus)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, id, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DownloadStatus", DbType.Int16, downloadStatus, ParameterDirection.Input));
                _Result = ExecuteNonQuery("usp_ArchiveBLPMDownload_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string DeleteArchivePMDownload(Int64 id)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, id, ParameterDirection.Input));
                _Result = ExecuteNonQuery("usp_ArchiveBLPMDownload_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }

}