using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Implementation
{
    internal class ClipDownloadController : IClipDownloadController
    {
        ModelFactory _ModelFactory = new ModelFactory();
        IClipDownloadModel _IClipDownloadModel;

        public ClipDownloadController()
        {
            _IClipDownloadModel = _ModelFactory.CreateObject<IClipDownloadModel>();
        }

        public List<ClipDownload> SelectByCustomer(Guid p_CustomerGUID)
        {
            try
            {
                DataSet _DataSet = _IClipDownloadModel.SelectByCustomer(p_CustomerGUID);

                List<ClipDownload> _ListOfClipDownload = FillClipDownloadList(_DataSet);

                return _ListOfClipDownload;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<ClipDownload> FillClipDownloadList(DataSet _DataSet)
        {
            try
            {
                List<ClipDownload> _ListOfClipDownload = null;

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    _ListOfClipDownload = new List<ClipDownload>();

                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ClipDownload _ClipDownload = new ClipDownload();

                        if (_DataTable.Columns.Contains("ClipDLFormat") && !_DataRow["ClipDLFormat"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ClipDLFormat = Convert.ToString(_DataRow["ClipDLFormat"]);
                        }

                        if (_DataTable.Columns.Contains("ClipDLRequestDateTime") && !_DataRow["ClipDLRequestDateTime"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ClipDLRequestDateTime = Convert.ToDateTime(_DataRow["ClipDLRequestDateTime"]);
                        }

                        if (_DataTable.Columns.Contains("ClipDownLoadedDateTime") && !_DataRow["ClipDownLoadedDateTime"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ClipDownLoadedDateTime = Convert.ToDateTime(_DataRow["ClipDownLoadedDateTime"]);
                        }

                        if (_DataTable.Columns.Contains("ClipDownloadStatus") && !_DataRow["ClipDownloadStatus"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ClipDownloadStatus = Convert.ToInt16(_DataRow["ClipDownloadStatus"]);
                        }

                        if (_DataTable.Columns.Contains("ClipFileLocation") && !_DataRow["ClipFileLocation"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ClipFileLocation = Convert.ToString(_DataRow["ClipFileLocation"]);
                        }

                        if (_DataTable.Columns.Contains("ClipID") && !_DataRow["ClipID"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ClipID = new Guid(Convert.ToString(_DataRow["ClipID"]));
                        }

                        if (_DataTable.Columns.Contains("ClipTitle") && !_DataRow["ClipTitle"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ClipTitle = Convert.ToString(_DataRow["ClipTitle"]);
                        }

                        if (_DataTable.Columns.Contains("CreatedBy") && !_DataRow["CreatedBy"].Equals(DBNull.Value))
                        {
                            _ClipDownload.CreatedBy = Convert.ToString(_DataRow["CreatedBy"]);
                        }

                        if (_DataTable.Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _ClipDownload.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }

                        if (_DataTable.Columns.Contains("CustomerGUID") && !_DataRow["CustomerGUID"].Equals(DBNull.Value))
                        {
                            _ClipDownload.CustomerGUID = new Guid(Convert.ToString(_DataRow["CustomerGUID"]));
                        }

                        if (_DataTable.Columns.Contains("IQ_ClipDownload_Key") && !_DataRow["IQ_ClipDownload_Key"].Equals(DBNull.Value))
                        {
                            _ClipDownload.IQ_ClipDownload_Key = Convert.ToInt32(_DataRow["IQ_ClipDownload_Key"]);
                        }

                        if (_DataTable.Columns.Contains("IsActive") && !_DataRow["IsActive"].Equals(DBNull.Value))
                        {
                            _ClipDownload.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        }

                        if (_DataTable.Columns.Contains("ModifiedBy") && !_DataRow["ModifiedBy"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ModifiedBy = Convert.ToString(_DataRow["ModifiedBy"]);
                        }

                        if (_DataTable.Columns.Contains("ModifiedDate") && !_DataRow["ModifiedDate"].Equals(DBNull.Value))
                        {
                            _ClipDownload.ModifiedDate = Convert.ToDateTime(_DataRow["ModifiedDate"]);
                        }

                        _ListOfClipDownload.Add(_ClipDownload);
                    }
                }

                return _ListOfClipDownload;
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
                string _Result = _IClipDownloadModel.Insert(p_CustomerGUID, p_XmlData);

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
                string _Result = _IClipDownloadModel.DeactivateClip(p_ClipID);

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
                string _Result = _IClipDownloadModel.Update(p_SqlXml);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateClipDownloadStatus(Int64 p_IQ_ClipDownload_Key, Int16 p_ClipDownloadStatus, string p_Location)
        {
            try
            {
                
                string _Result = _IClipDownloadModel.UpdateClipDownloadStatus(p_IQ_ClipDownload_Key, p_ClipDownloadStatus,p_Location);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<ClipMeta> GetFileLocationFromClipMeta(string p_XML)
        {
            try
            {
                DataSet _Result = _IClipDownloadModel.GetFileLocationFromClipMeta(p_XML);

                if (_Result != null && _Result.Tables[0] != null && _Result.Tables[0].Rows.Count > 0)
                {
                    var temp = (from p in _Result.Tables[0].AsEnumerable()
                                select new ClipMeta
                                             {
                                                 clipGUID = p.Field<Guid>("Clipguid"),
                                                 Location = p.Field<string>("FileLocation"),
                                                 UGCLocation = p.Field<string>("UGCFileLocation")
                                             }).ToList();
                    return temp;


                }
                else
                {
                    return null;
                }
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
                bool _Result = _IClipDownloadModel.CheckForExistingStatusOfService(p_ClipGUID,Ext);
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
