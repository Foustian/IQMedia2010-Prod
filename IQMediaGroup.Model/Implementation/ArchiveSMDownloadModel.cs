using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Base;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Implementation
{
    internal class ArchiveSMDownloadModel : IQMediaGroupDataLayer, IArchiveSMDownloadModel
    {
        public DataSet GetByCustomerGuid(Guid p_CustomerGuid)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_ArticleSMDownload_SelectByCustomerGuid", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertList(Guid p_CustomerGuid, SqlXml p_XmlData)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@XmlData", DbType.Xml, p_XmlData, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_ArticleSMDownload_InsertList", _ListOfDataType);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeactivateArticle(Guid p_ID)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_ArticleSMDownload_DeactivateArticle", _ListOfDataType);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateDownloadStatus(Int64 p_ID, Int16 p_DownloadStatus, string p_FileLocation)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DownloadStatus", DbType.Int16, p_DownloadStatus, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FileLocation", DbType.String, p_FileLocation, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_ArticleSMDownload_UpdateDownloadStatus", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetArticleFileLocationAndStatus(string p_XML)
        {
            try
            {
                DataSet _Result = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ArticleXml", DbType.Xml, p_XML, ParameterDirection.Input));
                _Result = this.GetDataSet("usp_IQCore_SM_SelectLocationAndStatusByList", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
