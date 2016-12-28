using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Implementation
{
    public class UGCRawMediaController:IUGCRawMediaController
    {
        ModelFactory _ModelFactory = new ModelFactory();
        IUGCRawMediaModel _IUGCRawMediaModel = null;

        public UGCRawMediaController()
        {
            _IUGCRawMediaModel = _ModelFactory.CreateObject<IUGCRawMediaModel>();
        }


        public UGCRawMedia GetUGCRawMediabyUGCGUID(UGCRawMedia _InUGCRawMedia)
        {
            try
            {
                DataSet _DataSet = _IUGCRawMediaModel.GetUGCRawMediabyUGCGUID(_InUGCRawMedia);

                UGCRawMedia _UGCRawMedia = FillUGCRawMedia(_DataSet);


                return _UGCRawMedia;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<UGCRawMedia> GetUGCRawMediaBySearch(Guid p_ClientGUID, int p_PageNo, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount, string p_CategoryGUID, string p_CustomerGUID, DateTime? p_FromDate, DateTime? p_ToDate, string p_SearchTermTitle, string p_SearchTermKeyword, string p_SearchTermDesc,out int p_ErrorNumber)
        {
            try
            {
                DataSet _DataSet = _IUGCRawMediaModel.GetUGCRawMediaBySearch(p_ClientGUID, p_PageNo, p_PageSize, p_SortField, p_IsAscending, out p_TotalRecordsCount, p_CategoryGUID, p_CustomerGUID, p_FromDate, p_ToDate, p_SearchTermTitle, p_SearchTermKeyword, p_SearchTermDesc, out p_ErrorNumber);

                List<UGCRawMedia> _ListOfUGCRawMedia = FillUGCRawMediaList(_DataSet);

                return _ListOfUGCRawMedia;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<UGCRawMedia> FillUGCRawMediaList(DataSet _DataSet)
        {
            try
            {
                List<UGCRawMedia> _ListOfUGCRawMedia = null;

                if (_DataSet!=null && _DataSet.Tables.Count>0 && _DataSet.Tables[0].Rows.Count>0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];
                    _ListOfUGCRawMedia = new List<UGCRawMedia>();

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        UGCRawMedia _UGCRawMedia = new UGCRawMedia();

                        if (_DataTable.Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.CategoryName = Convert.ToString(_DataRow["CategoryName"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("FirstName") && !_DataRow["FirstName"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        }

                        if (_DataTable.Columns.Contains("UGCGUID") && !_DataRow["UGCGUID"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.UGCGUID = new Guid(Convert.ToString(_DataRow["UGCGUID"]));
                        }

                        if (_DataTable.Columns.Contains("CreateDT") && !_DataRow["CreateDT"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.CreatedDT = Convert.ToDateTime(_DataRow["CreateDT"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryGUID") && !_DataRow["CategoryGUID"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.CategoryGUID = new Guid(Convert.ToString(_DataRow["CategoryGUID"]));
                        }

                        if (_DataTable.Columns.Contains("AirDate") && !_DataRow["AirDate"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.AirDate = Convert.ToDateTime(Convert.ToString(_DataRow["AirDate"]));
                        }

                        if (_DataTable.Columns.Contains("CustomerGUID") && !_DataRow["CustomerGUID"].Equals(DBNull.Value))
                        {
                            _UGCRawMedia.CustomerGUID = new Guid(Convert.ToString(_DataRow["CustomerGUID"]));
                        }

                        _ListOfUGCRawMedia.Add(_UGCRawMedia);

                    }
                }

                return _ListOfUGCRawMedia;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        private UGCRawMedia FillUGCRawMedia(DataSet _DataSet)
        {
            try
            {
                UGCRawMedia _objUGCRawMedia = null;

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    _objUGCRawMedia = new UGCRawMedia();
                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        

                        if (_DataTable.Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.CategoryName = Convert.ToString(_DataRow["CategoryName"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("FirstName") && !_DataRow["FirstName"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        }

                        if (_DataTable.Columns.Contains("UGCGUID") && !_DataRow["UGCGUID"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.UGCGUID = new Guid(Convert.ToString(_DataRow["UGCGUID"]));
                        }

                        if (_DataTable.Columns.Contains("CreateDT") && !_DataRow["CreateDT"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.CreatedDT = Convert.ToDateTime(_DataRow["CreateDT"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryGUID") && !_DataRow["CategoryGUID"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.CategoryGUID = new Guid(Convert.ToString(_DataRow["CategoryGUID"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory1GUID") && !_DataRow["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.SubCategory1GUID = new Guid(Convert.ToString(_DataRow["SubCategory1GUID"]));
                        }
                        

                        if (_DataTable.Columns.Contains("SubCategory2GUID") && !_DataRow["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.SubCategory2GUID = new Guid(Convert.ToString(_DataRow["SubCategory2GUID"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory3GUID") && !_DataRow["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.SubCategory3GUID = new Guid(Convert.ToString(_DataRow["SubCategory3GUID"]));
                        }

                        if (_DataTable.Columns.Contains("AirDate") && !_DataRow["AirDate"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.AirDate = Convert.ToDateTime(Convert.ToString(_DataRow["AirDate"]));
                        }

                        if (_DataTable.Columns.Contains("CustomerGUID") && !_DataRow["CustomerGUID"].Equals(DBNull.Value))
                        {
                            _objUGCRawMedia.CustomerGUID = new Guid(Convert.ToString(_DataRow["CustomerGUID"]));
                        }

                        

                    }
                }

                return _objUGCRawMedia;
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
                string _Result = _IUGCRawMediaModel.UpdateUGCRawMedia(p_RawMediaID, p_CustomerGUID, p_CategoryGUID, p_SubCategory1GUID,p_SubCategory2GUID,p_SubCategory3GUID, p_Title, p_Keyword, p_Description);

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
                string _Result = _IUGCRawMediaModel.UpdateUGCRawMedia(p_RawMediaID, p_MetaData);

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
                string _Result = _IUGCRawMediaModel.DeleteUGCRawMedia(p_UGCRawMediaIDs);

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
                string _Result = _IUGCRawMediaModel.FillRecordsFromCore(p_ClientGUID);

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
                string _Result = _IUGCRawMediaModel.GetUGCFilePathByUGCGUID(p_UGCGUID);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}