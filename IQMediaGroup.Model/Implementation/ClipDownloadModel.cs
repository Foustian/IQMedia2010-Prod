using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Implementation
{
    internal class ClipDownloadModel : IQMediaGroupDataLayer, IClipDownloadModel
    {
        public DataSet SelectByCustomer(Guid p_CustomerGUID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_ClipDownload_SelectByCustomer", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Insert(Guid p_CustomerGUID, SqlXml p_XmlData)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, p_XmlData, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_ClipDownload_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string DeactivateClip(Guid p_ClipID)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClipID", DbType.Guid, p_ClipID, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_ClipDownload_DeactivateClip", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string Update(SqlXml p_SqlXml)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, p_SqlXml, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_ClipDownload_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateClipDownloadStatus(Int64 p_IQ_ClipDownload_Key, Int16 p_ClipDownloadStatus,string p_Location)
        {
            try
            {
                string _Result = string.Empty;
                
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQ_ClipDownload_Key", DbType.Int64, p_IQ_ClipDownload_Key, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipDownloadStatus", DbType.Int16, p_ClipDownloadStatus, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Location", DbType.String, p_Location, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_ClipDownload_UpdateClipDownloadStatus", _ListOfDataType);
                
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetFileLocationFromClipMeta(string p_XML)
        {
            try
            {
                DataSet _Result = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@clipXML", DbType.Xml, p_XML, ParameterDirection.Input));
                _Result = this.GetDataSet("usp_IQCore_ClipMeta_SelectFileLocation", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool CheckForExistingStatusOfService(Guid p_ClipGUID,string Ext)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClipGUID", DbType.Guid, p_ClipGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Ext", DbType.String, Ext, ParameterDirection.Input));
                this.ExecuteScalar("usp_IQExportService_CheckClipStatusForDownload", _ListOfDataType, ref _Result);

                return Convert.ToBoolean(_Result);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
