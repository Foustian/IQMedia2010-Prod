using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using Microsoft.CSharp;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Collections;
using System.Xml.Linq;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    internal class ArchiveClipController : IArchiveClipController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IArchiveClipModel _IArchiveClipModel;
        List<Clip> _ListOfTempClip = new List<Clip>();

        public ArchiveClipController()
        {
            _IArchiveClipModel = _ModelFactory.CreateObject<IArchiveClipModel>();
        }


        /// <summary>
        /// Description: This Methods FillsArchive Clip Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Archive Clip Infromarmation</param>
        /// <returns>List of Object of Archive Clip</returns>
        private List<ArchiveClip> FillArchiveClipInformation(DataSet _DataSet)
        {
            List<ArchiveClip> _ListOfArchiveClip = new List<ArchiveClip>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        ArchiveClip _ArchiveClip = new ArchiveClip();

                        if (_DataTable.Columns.Contains("ClipDate") && !_DataRow["ClipDate"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.ClipDate = Convert.ToDateTime(_DataRow["ClipDate"]);
                        }

                        if (_DataTable.Columns.Contains("ClipID") && !_DataRow["ClipID"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.ClipID = new Guid(_DataRow["ClipID"].ToString());
                        }

                        if (_DataTable.Columns.Contains("ClipLogo") && !_DataRow["ClipLogo"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.ClipLogo = Convert.ToString(_DataRow["ClipLogo"]);
                        }

                        if (_DataTable.Columns.Contains("ClipTitle") && !_DataRow["ClipTitle"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.ClipTitle = Convert.ToString(_DataRow["ClipTitle"]);
                        }

                        if (_DataTable.Columns.Contains("FirstName") && !_DataRow["FirstName"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        }

                        if (_DataTable.Columns.Contains("ClipCreationDate") && !_DataRow["ClipCreationDate"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.ClipCreationDate = Convert.ToDateTime(_DataRow["ClipCreationDate"]);
                        }

                        if (_DataTable.Columns.Contains("ClosedCaption") && _DataRow["ClosedCaption"] != null)
                        {
                            _ArchiveClip.ClosedCaption = Convert.ToString(_DataRow["ClosedCaption"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("CustomerID") && !_DataRow["CustomerID"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.CustomerID = Convert.ToInt32(_DataRow["CustomerID"]);
                        }

                        if (_DataTable.Columns.Contains("Category") && !_DataRow["Category"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.Category = Convert.ToString(_DataRow["Category"]);
                        }

                        if (_DataTable.Columns.Contains("SubCategory1Name") && !_DataRow["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.SubCategory1Name = Convert.ToString(_DataRow["SubCategory1Name"]);
                        }

                        if (_DataTable.Columns.Contains("SubCategory2Name") && !_DataRow["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.SubCategory2Name = Convert.ToString(_DataRow["SubCategory2Name"]);
                        }

                        if (_DataTable.Columns.Contains("SubCategory3Name") && !_DataRow["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.SubCategory3Name = Convert.ToString(_DataRow["SubCategory3Name"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.CategoryName = Convert.ToString(_DataRow["CategoryName"]);
                        }

                        if (_DataTable.Columns.Contains("ArchiveClipKey") && !_DataRow["ArchiveClipKey"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.ArchiveClipKey = Convert.ToInt32(_DataRow["ArchiveClipKey"]);
                        }

                        if (_DataTable.Columns.Contains("ClipThumbnailImagePath") && !_DataRow["ClipThumbnailImagePath"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.ThumbnailImagePath = Convert.ToString(_DataRow["ClipThumbnailImagePath"]);
                        }


                        if (_DataTable.Columns.Contains("ThumbnailImagePath"))
                        {
                            if (!_DataRow["ThumbnailImagePath"].Equals(DBNull.Value))
                            {
                                if (string.IsNullOrEmpty(Convert.ToString(_DataRow["ThumbnailImagePath"])))
                                {
                                    _ArchiveClip.ThumbnailImagePath = "http://" + HttpContext.Current.Request.Url.Host + "/ThumbnailImage/noimage.jpg";
                                }
                                else
                                {
                                    _ArchiveClip.ThumbnailImagePath = Convert.ToString(_DataRow["ThumbnailImagePath"]);
                                }
                            }
                            else
                            {
                                _ArchiveClip.ThumbnailImagePath = "http://" + HttpContext.Current.Request.Url.Host + "/ThumbnailImage/noimage.jpg";
                            }
                        }


                        //if (_DataSet.Tables[0].Columns.Contains("ClipImageContent") && !string.IsNullOrEmpty(Convert.ToString(_DataRow["ClipImageContent"])))
                        //{
                        //    _ArchiveClip.ClipThumbNailImage = (byte[])_DataRow["ClipImageContent"];
                        //}

                        if (_DataTable.Columns.Contains("CategoryGUID") && !_DataRow["CategoryGUID"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.CategoryGUID = new Guid(Convert.ToString(_DataRow["CategoryGUID"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory1GUID") && !_DataRow["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.SubCategory1GUID = new Guid(Convert.ToString(_DataRow["SubCategory1GUID"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory2GUID") && !_DataRow["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.SubCategory2GUID = new Guid(Convert.ToString(_DataRow["SubCategory2GUID"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory3GUID") && !_DataRow["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.SubCategory3GUID = new Guid(Convert.ToString(_DataRow["SubCategory3GUID"]));
                        }

                        if (_DataTable.Columns.Contains("ClientGUID") && !_DataRow["ClientGUID"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.ClientGUID = new Guid(Convert.ToString(_DataRow["ClientGUID"]));
                        }

                        if (_DataTable.Columns.Contains("CustomerGUID") && !_DataRow["CustomerGUID"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.CustomerGUID = new Guid(Convert.ToString(_DataRow["CustomerGUID"]));
                        }

                        if (_DataTable.Columns.Contains("gmt_adj") && !_DataRow["gmt_adj"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.gmt_adj = Convert.ToInt32(_DataRow["gmt_adj"]);
                        }

                        if (_DataTable.Columns.Contains("dst_adj") && !_DataRow["dst_adj"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.dst_adj = Convert.ToInt32(_DataRow["dst_adj"]);
                        }
                        if (_DataTable.Columns.Contains("StartOffset") && !_DataRow["StartOffset"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.StartOffset = Convert.ToInt32(_DataRow["StartOffset"]);
                        }

                        if (_DataTable.Columns.Contains("IQ_CC_Key") && !_DataRow["IQ_CC_Key"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.IQ_CC_Key = Convert.ToString(_DataRow["IQ_CC_Key"]);
                        }

                        if (_DataTable.Columns.Contains("AUDIENCE") && !_DataRow["AUDIENCE"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.Audience = Convert.ToString(_DataRow["AUDIENCE"]);
                        }

                        if (_DataTable.Columns.Contains("IsActualNielsen") && !_DataRow["IsActualNielsen"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.IsActualNielsen = Convert.ToBoolean(_DataRow["IsActualNielsen"]);
                        }

                        if (_DataTable.Columns.Contains("SQAD_SHAREVALUE") && !_DataRow["SQAD_SHAREVALUE"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.Sqad_ShareValue = Convert.ToString(_DataRow["SQAD_SHAREVALUE"]); // string.Format("{0:N}{1}", _DataRow["SQAD_SHAREVALUE"], _ArchiveClip.IsActualNielsen ? "(A)" : "(E)");
                        }

                        if (_DataTable.Columns.Contains("Rating") && !_DataRow["Rating"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.Rating = Convert.ToInt16(_DataRow["Rating"]);
                        }

                        if (_DataTable.Columns.Contains("Total") && !_DataRow["Total"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.Total = Convert.ToInt32(_DataRow["Total"]);
                        }

                        _ListOfArchiveClip.Add(_ArchiveClip);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        public List<ArchiveClip> GetArchiveClipReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _IArchiveClipModel.GetArchiveClipReportGroupByCategory(p_ClientGUID, p_Date);

                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<ArchiveClip> GetArchiveClipByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsNielSenData)
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _IArchiveClipModel.GetArchiveClipByCategoryGuidAndDate(p_ClientGUID, p_SortField, p_IsAscending, p_Date, p_CategoryGuid, p_IsNielSenData);

                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<ArchiveClip> GetArchiveClipByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveClip> _ListOfArchiveClip = null;

                _DataSet = _IArchiveClipModel.GetArchiveClipByDurationNCategoryGuid(p_ClientGUID, p_SortField, p_IsAscending, p_FromDate, p_ToDate, p_CategoryGuid, p_IsCompeteData);
                _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
                return _ListOfArchiveClip;

            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
