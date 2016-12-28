using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;
using PMGSearch;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using System.Web;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Implementation
{
    public class ArchiveBLPMDownloadController : IArchiveBLPMDownloadController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IArchiveBLPMDownloadModel _IIArchiveBLPMDownloadModel;

        public ArchiveBLPMDownloadController()
        {
            _IIArchiveBLPMDownloadModel = _ModelFactory.CreateObject<IArchiveBLPMDownloadModel>();
        }

        public List<ArchiveBLPMDownload> GetByCustomerGuid(Guid p_CustomerGuid)
        {
            try
            {
                DataSet _DataSet = _IIArchiveBLPMDownloadModel.GetByCustomerGuid(p_CustomerGuid);

                List<ArchiveBLPMDownload> _ListOfArchiveNMDownload = FillArchiveBLPMDownloadInformation(_DataSet);

                return _ListOfArchiveNMDownload;
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
                _Result = _IIArchiveBLPMDownloadModel.InsertListArchivePMDownload(p_CustomerGuid, p_XmlData);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<ArchiveBLPMDownload> GetArchivePMDownload(Guid customerGuid)
        {
            try
            {
                DataSet _Result;
                _Result = _IIArchiveBLPMDownloadModel.GetArchivePMDownload(customerGuid);
                List<ArchiveBLPMDownload> lstArchiveBLPMDownload = FillArchiveBLPMDownloadInformation(_Result);
                return lstArchiveBLPMDownload;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateArchivePMDownload(Int64 id, Int16 downloadStatus)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IIArchiveBLPMDownloadModel.UpdateArchivePMDownload(id, downloadStatus);

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
                _Result = _IIArchiveBLPMDownloadModel.DeleteArchivePMDownload(id);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<ArchiveBLPMDownload> FillArchiveBLPMDownloadInformation(DataSet _DataSet)
        {
            try
            {
                List<ArchiveBLPMDownload> _ListOfArchiveBLPMDownload = new List<ArchiveBLPMDownload>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ArchiveBLPMDownload archiveBLPMDownload = new ArchiveBLPMDownload();

                        if (_DataTable.Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            archiveBLPMDownload.ID = Convert.ToInt32(_DataRow["ID"]);
                        }

                        if (_DataTable.Columns.Contains("DownloadLocation") && !_DataRow["DownloadLocation"].Equals(DBNull.Value))
                        {
                            archiveBLPMDownload.DownloadLocation = Convert.ToString(_DataRow["DownloadLocation"]);
                        }

                        if (_DataTable.Columns.Contains("DLRequestDateTime") && !_DataRow["DLRequestDateTime"].Equals(DBNull.Value))
                        {
                            archiveBLPMDownload.DLRequestDateTime = Convert.ToDateTime(_DataRow["DLRequestDateTime"]);
                        }

                        if (_DataTable.Columns.Contains("Headline") && !_DataRow["Headline"].Equals(DBNull.Value))
                        {
                            archiveBLPMDownload.Headline = Convert.ToString(_DataRow["Headline"]);
                        }

                        _ListOfArchiveBLPMDownload.Add(archiveBLPMDownload);

                    }
                }

                return _ListOfArchiveBLPMDownload;

            }
            catch (Exception)
            {
                throw;
            }
        }


    }

}