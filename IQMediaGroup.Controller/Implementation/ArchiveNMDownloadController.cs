using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Implementation
{
    public class ArchiveNMDownloadController : IArchiveNMDownloadController
    {
        ModelFactory _ModelFactory = new ModelFactory();
        IArchiveNMDownloadModel _IArchiveNMDownloadModel;

        public ArchiveNMDownloadController()
        {
            _IArchiveNMDownloadModel = _ModelFactory.CreateObject<IArchiveNMDownloadModel>();
        }

        public List<ArchiveNMDownload> GetByCustomerGuid(Guid p_CustomerGuid)
        {
            try
            {
                DataSet _DataSet = _IArchiveNMDownloadModel.GetByCustomerGuid(p_CustomerGuid);

                List<ArchiveNMDownload> _ListOfArchiveNMDownload = FillArchiveNMDownloadList(_DataSet);

                return _ListOfArchiveNMDownload;
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
                string _Result = _IArchiveNMDownloadModel.InsertList(p_CustomerGuid,p_XmlData);

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
                string _Result = _IArchiveNMDownloadModel.DeactivateArticle(p_ID);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateDownloadStatus(Int64 p_ID, Int16 p_DownloadStatus,string p_FileLocation)
        {
            try
            {
                string _Result = _IArchiveNMDownloadModel.UpdateDownloadStatus(p_ID, p_DownloadStatus, p_FileLocation);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveNMDownload> GetArticleFileLocationAndStatus(string p_XML)
        {
            try
            {
                DataSet _DataSet = _IArchiveNMDownloadModel.GetArticleFileLocationAndStatus(p_XML);

                List<ArchiveNMDownload> _ListOfArchiveNMDownload = FillArchiveNMDownloadList(_DataSet);

                return _ListOfArchiveNMDownload;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<ArchiveNMDownload> FillArchiveNMDownloadList(DataSet _DataSet)
        {
            try
            {
                List<ArchiveNMDownload> _ListOfArchiveNMDownload = null;

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    _ListOfArchiveNMDownload = new List<ArchiveNMDownload>();

                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ArchiveNMDownload _ArchiveNMDownload = new ArchiveNMDownload();

                        if (_DataTable.Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.ID = Convert.ToInt64(_DataRow["ID"]);
                        }

                        if (_DataTable.Columns.Contains("ArticleID") && !_DataRow["ArticleID"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.ArticleID = Convert.ToString(_DataRow["ArticleID"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("CustomerGuid") && !_DataRow["CustomerGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.CustomerGuid = new Guid(Convert.ToString(_DataRow["CustomerGuid"]));
                        }

                        if (_DataTable.Columns.Contains("DownloadStatus") && !_DataRow["DownloadStatus"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.DownloadStatus = Convert.ToInt16(_DataRow["DownloadStatus"]);
                        }

                        if (_DataTable.Columns.Contains("DLRequestDateTime") && !_DataRow["DLRequestDateTime"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.DLRequestDateTime = Convert.ToDateTime(_DataRow["DLRequestDateTime"]);
                        }

                        if (_DataTable.Columns.Contains("DownLoadedDateTime") && !_DataRow["DownLoadedDateTime"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.DownLoadedDateTime = Convert.ToDateTime(_DataRow["DownLoadedDateTime"]);
                        }

                        if (_DataTable.Columns.Contains("IsActive") && !_DataRow["IsActive"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        }

                        if (_DataTable.Columns.Contains("Status") && !_DataRow["Status"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.PdfSvcStatus = Convert.ToString(_DataRow["Status"]);
                        }

                        if (_DataTable.Columns.Contains("DownloadLocation") && !_DataRow["DownloadLocation"].Equals(DBNull.Value))
                        {
                            _ArchiveNMDownload.FileLocation = Convert.ToString(_DataRow["DownloadLocation"]);
                        }


                        _ListOfArchiveNMDownload.Add(_ArchiveNMDownload);
                    }
                }
                return _ListOfArchiveNMDownload;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
