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
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using Microsoft.CSharp;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Collections;
using System.Xml.Linq;

namespace IQMediaGroup.Controller.Implementation
{
    internal class ArchiveClipController : IArchiveClipController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly IArchiveClipModel _IArchiveClipModel;
        List<Clip> _ListOfTempClip = new List<Clip>();

        public ArchiveClipController()
        {
            _IArchiveClipModel = _ModelFactory.CreateObject<IArchiveClipModel>();
        }

        /// <summary>
        /// This method inserts Client Role Information
        /// </summary>
        /// <param name="p_ArchiveClips">Object Of ArchiveClips Class</param>
        /// <returns></returns>
        public string InsertArchiveClip(ArchiveClip p_ArchiveClips)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveClipModel.InsertClip(p_ArchiveClips);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods gets Archive Clip Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">List of Object of Archive Clip</param>
        /// <returns>List of Object of Archive Clip</returns>
        public List<ArchiveClip> GetArchiveClip(ArchiveClip p_ArchiveClip)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClip(p_ArchiveClip);
                _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
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

                        if (_DataTable.Columns.Contains("SQAD_SHAREVALUE") && !_DataRow["SQAD_SHAREVALUE"].Equals(DBNull.Value))
                        {
                            _ArchiveClip.Sqad_ShareValue = Convert.ToString(_DataRow["SQAD_SHAREVALUE"]);
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

        public List<ArchiveClip> GetAllArchiveClip(ArchiveClip p_ArchiveClip)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetAllArchiveClip(p_ArchiveClip);
                _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        /// <summary>
        /// Description: This Methods gets Archive Clip by ClipID Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Archive Clip </returns>
        public List<ArchiveClip> GetArchiveClipByClipID(ArchiveClip p_ArchiveClip)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClipByClipID(p_ArchiveClip);
                _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }


        public List<ArchiveClip> GetArchiveClipBySearchTerm(Guid p_ClientGUID, string p_SearchTerm, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount)
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _IArchiveClipModel.GetArchiveClipBySearchTerm(p_ClientGUID, p_SearchTerm, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, out p_TotalRecordsCount);

                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods gets Archive Clip by ClipID Information from DataSet.
        /// Added By: Meghana Ravani   
        /// </summary>
        /// <param name="p_ClientKey">ClientID of Client</param>
        /// <returns>List of Object of Archive Clip by ClipID</returns>
        public List<ArchiveClip> GetArchiveClipBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_ClipFromDate, DateTime? p_ClipToDate, string p_ListCategory1GUID, string p_ListCategory2GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, bool p_IsNielSenRights, out int p_TotalRecordsCount)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClipBySearch(p_ClientGUID, p_SearchTermTitle, p_SearchTermDesc, p_SearchTermKeyword, p_SearchTermCC, p_ClipFromDate, p_ClipToDate, p_ListCategory1GUID, p_ListCategory2GUID, p_ListCustomerGUID, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, p_IsNielSenRights, out p_TotalRecordsCount);
                _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }

        public List<ArchiveClip> GetArchiveClipBySearchNew(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_ClipFromDate, DateTime? p_ClipToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, bool p_IsNielSenRights, out int p_TotalRecordsCount)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClipBySearchNew(p_ClientGUID, p_SearchTermTitle, p_SearchTermDesc, p_SearchTermKeyword, p_SearchTermCC, p_ClipFromDate, p_ClipToDate, p_Category1GUID, p_Category2GUID, p_Category3GUID, p_Category4GUID, p_CategoryOperator1, p_CategoryOperator2, p_CategoryOperator3, p_ListCustomerGUID, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, p_IsNielSenRights, out p_TotalRecordsCount);
                _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
                return _ListOfArchiveClip;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public List<ArchiveClip> GetArchiveClipByParams(Guid p_ClientGUID, string p_ListCategoryGUID, string p_ListSubCategory1GUID, string p_ListSubCategory2GUID, string p_ListSubCategory3GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, string p_SearchText, string p_ClipTitle, out Guid? p_ClipID, out int p_TotalRecordsCount)
        {
            DataSet _DataSet = null;
            List<ArchiveClip> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClipByParams(p_ClientGUID, p_ListCategoryGUID, p_ListSubCategory1GUID, p_ListSubCategory2GUID, p_ListSubCategory3GUID, p_ListCustomerGUID, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, p_SearchText, p_ClipTitle, out p_ClipID, out p_TotalRecordsCount);
                _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfArchiveClip;
        }


        /// <summary>
        /// This method gets Archive clip data between Start Date And End Date
        /// </summary>
        /// <param name="p_StartDate">Start Date</param>
        /// <param name="p_EndDate">End Date</param>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>List of object of Archive Clip Details</returns>
        public List<ArchiveClipExport> GetArchiveClipByDate(DateTime p_StartDate, DateTime p_EndDate, int p_CustomerID)
        {
            DataSet _DataSet = null;
            List<ArchiveClipExport> _ListOfArchiveClip = null;
            try
            {
                _DataSet = _IArchiveClipModel.GetArchiveClipByDate(p_StartDate, p_EndDate, p_CustomerID);
                _ListOfArchiveClip = FillArchiveClipInformationByDate(_DataSet);
                return _ListOfArchiveClip;
            }
            catch (Exception _Exceprion)
            {
                throw _Exceprion;
            }
        }

        /// <summary>
        /// Description: This Methods FillsArchive Clip Information from DataSet.
        /// Added By: vishal parekh   
        /// </summary>
        /// <param name="_DataSet">Dataset for Archive Clip Infromarmation</param>
        /// <returns>List of Object of Archive Clip</returns>
        private List<ArchiveClipExport> FillArchiveClipInformationByDate(DataSet _DataSet)
        {
            List<ArchiveClipExport> _ListOfArchiveClip = new List<ArchiveClipExport>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {

                        ArchiveClipExport _ArchiveClip = new ArchiveClipExport();

                        _ArchiveClip.PMGCLIPID = Convert.ToInt32(_DataRow["ArchiveClipKey"]);
                        _ArchiveClip.Clip_Air_Date = Convert.ToDateTime(_DataRow["ClipDate"]);
                        _ArchiveClip.Clip_GUID = new Guid(_DataRow["ClipID"].ToString());
                        _ArchiveClip.Clip_Title = Convert.ToString(_DataRow["ClipTitle"]);
                        _ArchiveClip.Clip_Creation_Date = Convert.ToString(_DataRow["ClipCreationDate"]);
                        _ArchiveClip.Clip_CC = Convert.ToString(_DataRow["ClosedCaption"]);
                        _ArchiveClip.Clip_Description = Convert.ToString(_DataRow["Description"]);
                        _ArchiveClip.PMGCustomer_ID = Convert.ToInt32(_DataRow["CustomerID"]);
                        _ArchiveClip.Clip_export_file_date = DateTime.Now.ToString();

                        //if ((_DataRow["ClipImageContent"]).Equals(System.DBNull.Value))
                        //{
                        //    _ArchiveClip.Clip_ThumbNail = null;
                        //}
                        //else
                        //{
                        //    _ArchiveClip.Clip_ThumbNail = (byte[])(_DataRow["ClipImageContent"]);
                        //}

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

        /// <summary>
        /// Description:This method will Generate List To XML.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_ListOfArchiveClip">List of Object of Archive Clip</param>
        /// <returns>XDocument</returns>
        public XDocument GenerateListToXML(List<ArchiveClip> _ListOfArchiveClip)
        {
            XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                      new XElement("list",
                       from _ArchiveClip in _ListOfArchiveClip
                       select new XElement("Element",
                           string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipID)) ? null :
                       new XAttribute("ClipID", _ArchiveClip.ClipID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipLogo)) ? null :
                       new XAttribute("ClipLogo", _ArchiveClip.ClipLogo),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipTitle)) ? null :
                       new XAttribute("ClipTitle", _ArchiveClip.ClipTitle),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipDate)) ? null :
                       new XAttribute("ClipDate", _ArchiveClip.ClipDate),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.FirstName)) ? null :
                       new XAttribute("FirstName", _ArchiveClip.FirstName),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CustomerID)) ? null :
                       new XAttribute("CustomerID", _ArchiveClip.CustomerID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Category)) ? null :
                       new XAttribute("Category", _ArchiveClip.Category),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.SubCategory1GUID)) ? null :
                       new XAttribute("SubCategory1GUID", _ArchiveClip.SubCategory1GUID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.SubCategory2GUID)) ? null :
                       new XAttribute("SubCategory2GUID", _ArchiveClip.SubCategory2GUID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.SubCategory3GUID)) ? null :
                       new XAttribute("SubCategory3GUID", _ArchiveClip.SubCategory3GUID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Description)) ? null :
                       new XAttribute("Description", _ArchiveClip.Description),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Keywords)) ? null :
                       new XAttribute("Keywords", _ArchiveClip.Keywords),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClosedCaption)) ? null :
                       new XAttribute("ClosedCaption", _ArchiveClip.ClosedCaption),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipCreationDate)) ? null :
                       new XAttribute("ClipCreationDate", _ArchiveClip.ClipCreationDate),
                           //string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClipThumbNailImage)) ? null :
                           //new XAttribute("ClipImageContent", Convert.ToBase64String(_ArchiveClip.ClipThumbNailImage)),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ThumbnailImagePath)) ? null :
                       new XAttribute("ThumbnailImagePath", _ArchiveClip.ThumbnailImagePath),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CreatedDate)) ? null :
                       new XAttribute("CreatedDate", _ArchiveClip.CreatedDate),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ModifiedDate)) ? null :
                       new XAttribute("ModifiedDate", _ArchiveClip.ModifiedDate),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.IsActive)) ? null :
                       new XAttribute("IsActive", _ArchiveClip.IsActive),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CategoryGUID)) ? null :
                       new XAttribute("CategoryGUID", _ArchiveClip.CategoryGUID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClientGUID)) ? null :
                       new XAttribute("ClientGUID", _ArchiveClip.ClientGUID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CustomerGUID)) ? null :
                       new XAttribute("CustomerGUID", _ArchiveClip.CustomerGUID),
                       string.IsNullOrEmpty(_ArchiveClip.IQ_CC_Key) ? null :
                       new XAttribute("IQ_CC_Key", _ArchiveClip.IQ_CC_Key)
                           )));
            return xmlDocument;
        }


        /// <summary>
        /// Description:This method will Find Clip Key.
        /// </summary>
        /// <param name="p_FindClipKeyValue">FindClipKeyValue</param>
        /// <param name="p_String">String</param>
        /// <returns>Is Clip key or not.</returns>
        private bool FindClipKey(KeyValue p_FindClipKeyValue, string p_String)
        {
            try
            {
                if (p_String.Contains(p_FindClipKeyValue._FindKey))
                {
                    string _SubString = p_String;
                    _SubString = p_String.Substring(p_String.IndexOf("\"v\":"));
                    _SubString = _SubString.Substring(5, (_SubString.Length - 6));

                    p_FindClipKeyValue._KeyValue = _SubString;
                    p_FindClipKeyValue._SetKey = true;

                    return true;
                }

                return false;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will Find Clip Key.
        /// </summary>
        /// <param name="p_FindClipKeyValue">FindClipKeyValue</param>
        /// <param name="p_String">String</param>
        /// <returns>Is Clip key or not.</returns>
        private bool FindClipKeyValue(KeyValue p_FindClipKeyValue, string p_String)
        {
            try
            {
                if (p_String.ToLower().Contains(p_FindClipKeyValue._FindKey.ToLower()))
                {
                    string _SubString = p_String.Substring(p_String.ToLower().IndexOf(p_FindClipKeyValue._FindKey.ToLower()));
                    int _StartIndex = _SubString.IndexOf("\"v\":");
                    int _EndIndex = _SubString.IndexOf("\"}");
                    _SubString = _SubString.Substring(_StartIndex, (_EndIndex - _StartIndex));
                    _SubString = _SubString.Substring(5, (_SubString.Length - 5));

                    p_FindClipKeyValue._KeyValue = _SubString;
                    p_FindClipKeyValue._SetKey = true;

                    return true;
                }

                return false;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        ///  Description:This method Get Archive Clip By Customer.
        /// </summary>
        /// <param name="p_CustomerID">CustomerID</param>
        /// <returns>List of object of ArchiveClip</returns>
        public List<ArchiveClip> GetArchiveClipByCustomer(Int64 p_CustomerID)
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _IArchiveClipModel.GetArchiveClipByCustomer(p_CustomerID);

                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        ///  Description:This method Get Archive Clip By Customer.
        /// </summary>
        /// <param name="p_CustomerGUID">CustomerGUID</param>
        /// <returns>List of object of ArchiveClip</returns>
        public List<ArchiveClip> GetArchiveClipByCustomerGUID(Guid p_CustomerGUID)
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _IArchiveClipModel.GetArchiveClipByCustomerGUID(p_CustomerGUID);

                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_ListOfArchiveClip"></param>
        /// <returns></returns>
        private string GetXmlArchiveClip(IEnumerable p_ListOfArchiveClip)
        {
            try
            {

                MemoryStream stream = new MemoryStream();

                /*using (XmlWriter writer = XmlWriter.Create(stream))
                {*/
                XmlWriter writer = XmlWriter.Create(stream);
                writer.WriteStartElement("list");

                foreach (ArchiveClip _ArchiveClip in p_ListOfArchiveClip)
                {
                    writer.WriteStartElement("Element");

                    if (_ArchiveClip.ClipID != null)
                    {
                        writer.WriteAttributeString("ClipID", _ArchiveClip.ClipID.ToString());
                    }

                    if (_ArchiveClip.ClipLogo != null)
                    {
                        writer.WriteAttributeString("ClipLogo", _ArchiveClip.ClipLogo.ToString());
                    }

                    if (_ArchiveClip.ClipTitle != null)
                    {
                        writer.WriteAttributeString("ClipTitle", _ArchiveClip.ClipTitle.ToString());
                    }

                    if (_ArchiveClip.ClipDate != null)
                    {
                        writer.WriteAttributeString("ClipDate", _ArchiveClip.ClipDate.ToString());
                    }

                    if (_ArchiveClip.FirstName != null)
                    {
                        writer.WriteAttributeString("FirstName", _ArchiveClip.FirstName.ToString());
                    }

                    /* if (_ArchiveClip.CustomerID != null)
                     {
                         writer.WriteAttributeString("CustomerID", _ArchiveClip.CustomerID.ToString());
                     }*/

                    if (_ArchiveClip.Category != null)
                    {
                        writer.WriteAttributeString("Category", _ArchiveClip.Category.ToString());
                    }

                    if (_ArchiveClip.Description != null)
                    {
                        writer.WriteAttributeString("Description", _ArchiveClip.Description.ToString());
                    }

                    if (_ArchiveClip.ClosedCaption != null)
                    {
                        // writer.WriteAttributeString("ClosedCaption", _ArchiveClip.ClosedCaption.ToString()); 
                    }

                    if (_ArchiveClip.ClipCreationDate != null)
                    {
                        writer.WriteAttributeString("ClipCreationDate", _ArchiveClip.ClipCreationDate.ToString());
                    }

                    //if (_ArchiveClip.ClipThumbNailImage != null)
                    //{
                    //    writer.WriteAttributeString("ClipImageContent", _ArchiveClip.ClipThumbNailImage.ToString());
                    //}

                    if (_ArchiveClip.ThumbnailImagePath != null)
                    {
                        writer.WriteAttributeString("ThumbnailImagePath", _ArchiveClip.ThumbnailImagePath.ToString());
                    }

                    if (_ArchiveClip.CreatedDate != null)
                    {
                        writer.WriteAttributeString("CreatedDate", _ArchiveClip.CreatedDate.ToString());
                    }

                    if (_ArchiveClip.ModifiedDate != null)
                    {
                        writer.WriteAttributeString("ModifiedDate", _ArchiveClip.ModifiedDate.ToString());
                    }

                    if (_ArchiveClip.IsActive != null)
                    {
                        writer.WriteAttributeString("IsActive", _ArchiveClip.IsActive.ToString());
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                stream.Position = 0;

                StreamReader _StreamReader = new StreamReader(stream);


                //return new SqlXml(stream);
                return _StreamReader.ReadToEnd();
                //}
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will Insert Clip.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <returns>Primary Key of Archive Clip</returns>
        public string InsertArchiveClip(SqlXml p_SqlXml)
        {
            try
            {
                string _ReturnValue = _IArchiveClipModel.InsertArchiveClip(p_SqlXml);
                return _ReturnValue;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<ArchiveClip> GetArchiveClipFromXML(SqlXml ClipXML)
        {
            try
            {
                DataSet _DataSet = null;
                _DataSet = _IArchiveClipModel.GetArchiveClipFromXML(ClipXML);
                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);
                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string EmailContent(string ClipGUID, string mailAddress, string PageName)
        {
            try
            {
                List<ArchiveClip> _ListOfArchiveClip = new List<ArchiveClip>();
                ArchiveClip _ArchiveClip = new ArchiveClip();
                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                _ListOfArchiveClip = _IArchiveClipController.GetAllArchiveClip(_ArchiveClip);

                StringBuilder EmailBuilder = new StringBuilder();

                EmailBuilder.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\" style=\"font-family:Verdana;font-size:11px;\">");
                EmailBuilder.Append("<tr>");
                EmailBuilder.Append("<th>Image</th>");
                EmailBuilder.Append("<th style=\"width:150px;\" align=\"center\">Title</th>");
                EmailBuilder.Append("<th>Clip URL</th>");
                EmailBuilder.Append("</tr>");

                string[] ClipGUIDarr = ClipGUID.Split(',');

                for (int count = 0; count < ClipGUIDarr.Length; count++)
                {
                    ArchiveClip _ArchiveClipTemp = _ListOfArchiveClip.Find(delegate(ArchiveClip _ArchiveClipObjTmp) { return _ArchiveClipObjTmp.ClipID == new Guid(ClipGUIDarr[count]); });


                    //string _FileName = HttpContext.Current.Server.MapPath("~/" + "ThumbnailImage/" + _ArchiveClipTemp.ClipID + ".jpg");



                    string Name = HttpContext.Current.Request.Url.ToString();

                    string[] _DomainName = Name.Split("/".ToCharArray());

                    string _FinalString = _DomainName[0] + "//" + _DomainName[2];

                    if (ClipGUIDarr.Length > 0)
                    {
                        //if (_ArchiveClipTemp.ThumbnailImagePath != null && _ArchiveClipTemp.ThumbnailImagePath.Length > 0)
                        //{
                        //    strKeys1 += "<tr>";
                        //    strKeys1 += "<td><a href=\"" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ArchiveClipTemp.ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "\">" + "<img src=\"" + _ArchiveClipTemp.ThumbnailImagePath + "\" id=\"imgClip\" height=\"100\" width=\"100\" /></a></td>";
                        //    strKeys1 += "<td style=\"width:150px;\" align=\"center\">" + _ArchiveClipTemp.ClipTitle + "</td>";
                        //    strKeys1 += "<td><a href=\"" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ArchiveClipTemp.ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "\">" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ArchiveClipTemp.ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "</a></td>";
                        //    strKeys1 += "</tr>";
                        //}
                        //else
                        //{
                        //    strKeys1 += "<tr>";
                        //    strKeys1 += "<td><a href=\"" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ArchiveClipTemp.ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "\">" + "<img src=\"" + "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/ThumbnailImage/noimage.jpg" + "\" id=\"imgClip\" height=\"100\" width=\"100\" /></a></td>";
                        //    strKeys1 += "<td style=\"width:150px;\" align=\"center\">" + _ArchiveClipTemp.ClipTitle + "</td>";
                        //    strKeys1 += "<td><a href=\"" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ArchiveClipTemp.ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "\">" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ArchiveClipTemp.ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "</a></td>";
                        //    strKeys1 += "</tr>";
                        //}

                        EmailBuilder.Append("<tr>");
                        EmailBuilder.AppendFormat("<td align=\"center\"><a href=\"{0}{1}{2}&amp;TE={3}&amp;PN={4}\"><img src=\"{5}&amp;eid={2}\" id=\"imgClip\" height=\"100\" width=\"100\" /></a></td>", _FinalString, ConfigurationManager.AppSettings["ClipURL"], _ListOfArchiveClip[count].ClipID, HttpContext.Current.Server.UrlDecode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationManager.AppSettings["EncryptionKey"])), HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationManager.AppSettings["EncryptionKey"])), ConfigurationManager.AppSettings["ClipGetPreview"]);
                        EmailBuilder.AppendFormat("<td style=\"width:150px;\" align=\"center\">{0}</td>", System.Web.HttpUtility.HtmlEncode(_ListOfArchiveClip[count].ClipTitle));
                        EmailBuilder.AppendFormat("<td><a href=\"{0}{1}{2}&amp;TE={3}&amp;PN={4}\">{0}{1}{2}&amp;TE={3}&amp;PN={4}</a></td>", _FinalString, ConfigurationManager.AppSettings["ClipURL"], _ListOfArchiveClip[count].ClipID, HttpContext.Current.Server.UrlDecode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationManager.AppSettings["EncryptionKey"])), HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationManager.AppSettings["EncryptionKey"])));
                        EmailBuilder.Append("</tr>");
                        EmailBuilder.Append("<tr><td></td></tr>");

                    }
                }

                EmailBuilder.Append("</table>");

                return EmailBuilder.ToString();
                //SendEmail()

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string EmailContent(SqlXml ClipXML, string mailAddress, string PageName)
        {
            try
            {
                List<ArchiveClip> _ListOfArchiveClip = new List<ArchiveClip>();
                ArchiveClip _ArchiveClip = new ArchiveClip();
                IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                _ListOfArchiveClip = _IArchiveClipController.GetArchiveClipFromXML(ClipXML);

                StringBuilder EmailBuilder = new StringBuilder();

                EmailBuilder.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\" style=\"font-family:Verdana;font-size:11px;\">");
                EmailBuilder.Append("<tr>");
                EmailBuilder.Append("<th>Image</th>");
                EmailBuilder.Append("<th style=\"width:150px;\" align=\"center\">Title</th>");
                EmailBuilder.Append("<th>Clip URL</th>");
                EmailBuilder.Append("</tr>");

                for (int count = 0; count < _ListOfArchiveClip.Count; count++)
                {

                    string Name = HttpContext.Current.Request.Url.ToString();

                    string[] _DomainName = Name.Split("/".ToCharArray());

                    string _FinalString = _DomainName[0] + "//" + _DomainName[2];


                    //if (_ListOfArchiveClip[count].ThumbnailImagePath != null && _ListOfArchiveClip[count].ThumbnailImagePath.Length > 0)
                    //{
                    //    strKeys1 += "<tr>";
                    //    strKeys1 += "<td align=\"center\"><a href=\"" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ListOfArchiveClip[count].ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "\">" + "<img src=\"" + _ListOfArchiveClip[count].ThumbnailImagePath + "\" id=\"imgClip\" height=\"100\" width=\"100\" /></a></td>";
                    //    strKeys1 += "<td style=\"width:150px;\" align=\"center\">" + System.Web.HttpUtility.HtmlEncode(_ListOfArchiveClip[count].ClipTitle) + "</td>";
                    //    strKeys1 += "<td><a href=\"" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ListOfArchiveClip[count].ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "\">" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ListOfArchiveClip[count].ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "</a></td>";
                    //    strKeys1 += "</tr>";
                    //}
                    //else
                    //{
                    //    strKeys1 += "<tr>";
                    //    strKeys1 += "<td><a href=\"" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ListOfArchiveClip[count].ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "\">" + "<img src=\"" + "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/ThumbnailImage/noimage.jpg" + "\" id=\"imgClip\" height=\"100\" width=\"100\" /></a></td>";
                    //    strKeys1 += "<td style=\"width:150px;\" align=\"center\">" + System.Web.HttpUtility.HtmlEncode(_ListOfArchiveClip[count].ClipTitle) + "</td>";
                    //    strKeys1 += "<td><a href=\"" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ListOfArchiveClip[count].ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "\">" + _FinalString + ConfigurationSettings.AppSettings["ClipURL"] + _ListOfArchiveClip[count].ClipID + "&amp;TE=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationSettings.AppSettings["EncryptionKey"])) + "&amp;PN=" + HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationSettings.AppSettings["EncryptionKey"])) + "</a></td>";
                    //    strKeys1 += "</tr>";
                    //}

                    EmailBuilder.Append("<tr>");
                    EmailBuilder.AppendFormat("<td align=\"center\"><a href=\"{0}{1}{2}&amp;TE={3}&amp;PN={4}\"><img src=\"{5}&amp;eid={2}\" id=\"imgClip\" height=\"100\" width=\"100\" /></a></td>", _FinalString, ConfigurationManager.AppSettings["ClipURL"], _ListOfArchiveClip[count].ClipID, HttpContext.Current.Server.UrlDecode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationManager.AppSettings["EncryptionKey"])), HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationManager.AppSettings["EncryptionKey"])), ConfigurationManager.AppSettings["ClipGetPreview"]);
                    EmailBuilder.AppendFormat("<td style=\"width:150px;\" align=\"center\">{0}</td>", System.Web.HttpUtility.HtmlEncode(_ListOfArchiveClip[count].ClipTitle));
                    EmailBuilder.AppendFormat("<td><a href=\"{0}{1}{2}&amp;TE={3}&amp;PN={4}\">{0}{1}{2}&amp;TE={3}&amp;PN={4}</a></td>", _FinalString, ConfigurationManager.AppSettings["ClipURL"], _ListOfArchiveClip[count].ClipID, HttpContext.Current.Server.UrlDecode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationManager.AppSettings["EncryptionKey"])), HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationManager.AppSettings["EncryptionKey"])));
                    EmailBuilder.Append("</tr>");
                    EmailBuilder.Append("<tr><td></td></tr>");

                    //strKeys1 += "<tr><td colspan=\"1\"><a href=\"" + ConfigurationManager.AppSettings["iOSURL"] + "ClipID=" + _ListOfArchiveClip[count].ClipID + "&amp;BaseUrl=" + ConfigurationManager.AppSettings["ServicesBaseURL"] + "\">Click here for iPad/iPhone</a></td></tr>";
                    //strKeys1 += "<tr><td></td></tr>";

                }
                EmailBuilder.Append("</table>");

                return EmailBuilder.ToString();
                //SendEmail()

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string EmailContent(string URL, string FileName, string _imagePath, string FileID, string mailAddress, string PageName)
        {
            try
            {
                StringBuilder EmailBuilder = new StringBuilder();

                EmailBuilder.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\" style=\"font-family:Verdana;font-size:11px;\">");
                EmailBuilder.Append("<tr>");
                EmailBuilder.Append("<th>Image</th>");
                EmailBuilder.Append("<th style=\"width:150px;\" align=\"center\">Title</th>");
                EmailBuilder.Append("<th>Clip URL</th>");
                EmailBuilder.Append("</tr>");

                EmailBuilder.Append("<tr>");
                EmailBuilder.AppendFormat("<td align=\"center\"><a href=\"{0}&amp;TE={1}&amp;PN={2}\"><img src=\"{3}\" id=\"imgClip\" height=\"100\" width=\"100\" /></a></td>", URL, HttpContext.Current.Server.UrlDecode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationManager.AppSettings["EncryptionKey"])), HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationManager.AppSettings["EncryptionKey"])), _imagePath);
                EmailBuilder.AppendFormat("<td style=\"width:150px;\" align=\"center\">{0}</td>", FileName);
                EmailBuilder.AppendFormat("<td><a href=\"{0}&amp;TE={1}&amp;PN={2}\">{0}&amp;TE={1}&amp;PN={2}</a></td>", URL, HttpContext.Current.Server.UrlDecode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(mailAddress, ConfigurationManager.AppSettings["EncryptionKey"])), HttpContext.Current.Server.UrlEncode(IQMediaGroup.Core.HelperClasses.CommonFunctions.Encrypt(PageName, ConfigurationManager.AppSettings["EncryptionKey"])), _imagePath);
                EmailBuilder.Append("</tr>");

                EmailBuilder.Append("</table>");


                return EmailBuilder.ToString(); ;
                //SendEmail()

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void WriteBytesToFile(string fileName, byte[] content)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
            }
            finally
            {
                fs.Close();
                w.Close();
            }

        }

        /// <summary>
        /// Description:This method DeleteArchiveClip 
        /// </summary>
        /// <param name="p_DeleteArchiveClip">DeleteArchiveClip</param>
        /// <returns>Count</returns>
        public string DeleteArchiveClip(string p_DeleteArchiveClip)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveClipModel.DeleteArchiveClip(p_DeleteArchiveClip);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Update Existing Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        public string UpdateArchiveClip(ArchiveClip p_ArchiveClip)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IArchiveClipModel.UpdateArchiveClip(p_ArchiveClip);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string InsertArchiveClipFromConsole(ArchiveClip p_ArchiveClip)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IArchiveClipModel.InsertArchiveClipFromConsole(p_ArchiveClip);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<ArchiveClip> GetData()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _IArchiveClipModel.GetData();

                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
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

        public List<ArchiveClip> GetArchiveClipByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid)
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _IArchiveClipModel.GetArchiveClipByCategoryGuidAndDate(p_ClientGUID,p_SortField,p_IsAscending,p_Date,p_CategoryGuid);

                List<ArchiveClip> _ListOfArchiveClip = FillArchiveClipInformation(_DataSet);

                return _ListOfArchiveClip;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


    }
}
