using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using IQMediaGroup.Core.Enumeration;
using System.Xml;
using System.Threading;
using System.Data;
using System.Xml.Linq;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;
using IQMediaGroup.Reports.Controller.Interface;


namespace IQMediaGroup.Reports.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IClipController
    /// </summary>
    public class SocialMediaController : ISocialMediaController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ISocialMediaModel _ISocialMediaModel;

        public SocialMediaController()
        {
            _ISocialMediaModel = _ModelFactory.CreateObject<ISocialMediaModel>();
        }

        private List<ArchiveSM> FillArchiveSMInformation(DataSet _DataSet)
        {
            try
            {
                List<ArchiveSM> _ListOfArchiveSM = new List<ArchiveSM>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ArchiveSM _ArchiveSM = new ArchiveSM();

                        if (_DataTable.Columns.Contains("ArchiveSMKey") && !_DataRow["ArchiveSMKey"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.ArchiveSMKey = Convert.ToInt32(_DataRow["ArchiveSMKey"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("FirstName") && !_DataRow["FirstName"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        }

                        if (_DataTable.Columns.Contains("LastName") && !_DataRow["LastName"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.LastName = Convert.ToString(_DataRow["LastName"]);
                        }

                        if (_DataTable.Columns.Contains("CustomerGuid") && !_DataRow["CustomerGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.CustomerGuid = new Guid(Convert.ToString(_DataRow["CustomerGuid"]));
                        }

                        if (_DataTable.Columns.Contains("ClientGuid") && !_DataRow["ClientGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.ClientGuid = new Guid(Convert.ToString(_DataRow["ClientGuid"]));
                        }

                        if (_DataTable.Columns.Contains("CategoryGuid") && !_DataRow["CategoryGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.CategoryGuid = new Guid(Convert.ToString(_DataRow["CategoryGuid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory1Guid") && !_DataRow["SubCategory1Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.SubCategory1Guid = new Guid(Convert.ToString(_DataRow["SubCategory1Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory2Guid") && !_DataRow["SubCategory2Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.SubCategory2Guid = new Guid(Convert.ToString(_DataRow["SubCategory2Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory3Guid") && !_DataRow["SubCategory3Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.SubCategory3Guid = new Guid(Convert.ToString(_DataRow["SubCategory3Guid"]));
                        }

                        if (_DataTable.Columns.Contains("ArticleID") && !_DataRow["ArticleID"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.ArticleID = Convert.ToString(_DataRow["ArticleID"]);
                        }

                        if (_DataTable.Columns.Contains("ArticleContent") && !_DataRow["ArticleContent"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Content = Convert.ToString(_DataRow["ArticleContent"]);
                        }

                        if (_DataTable.Columns.Contains("Url") && !_DataRow["Url"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Url = Convert.ToString(_DataRow["Url"]);
                        }

                        if (_DataTable.Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.CategoryNames = Convert.ToString(_DataRow["CategoryName"]);
                        }

                        if (_DataTable.Columns.Contains("Rating") && !_DataRow["Rating"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Rating = Convert.ToInt16(_DataRow["Rating"]);
                        }

                        if (_DataTable.Columns.Contains("Total") && !_DataRow["Total"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Total = Convert.ToInt32(_DataRow["Total"]);
                        }

                        if (_DataTable.Columns.Contains("IQ_AdShare_Value") && !_DataRow["IQ_AdShare_Value"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.IQ_AdShare_Value = Convert.ToDecimal(_DataRow["IQ_AdShare_Value"]);
                        }

                        if (_DataTable.Columns.Contains("c_uniq_visitor") && !_DataRow["c_uniq_visitor"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.c_uniq_visitor = Convert.ToInt32(_DataRow["c_uniq_visitor"]);
                        }

                        if (_DataTable.Columns.Contains("IsUrlFound") && !_DataRow["IsUrlFound"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.IsUrlFound = Convert.ToBoolean(_DataRow["IsUrlFound"]);
                        }

                        if (_DataTable.Columns.Contains("IsCompeteAll") && !_DataRow["IsCompeteAll"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.IsCompleteAll = Convert.ToBoolean(_DataRow["IsCompeteAll"]);
                        }

                        if (_DataTable.Columns.Contains("harvest_time") && !_DataRow["harvest_time"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Harvest_Time = Convert.ToDateTime(_DataRow["harvest_time"]);
                        }

                        if (_DataTable.Columns.Contains("homeLink") && !_DataRow["homeLink"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.homeLink = Convert.ToString(_DataRow["homeLink"]);
                        }

                        _ListOfArchiveSM.Add(_ArchiveSM);
                    }
                }

                return _ListOfArchiveSM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveSM> GetArchiveSMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveSM> _ListOfArchiveSM = null;

                _DataSet = _ISocialMediaModel.GetArchiveSMReportGroupByCategory(p_ClientGUID, p_Date);
                _ListOfArchiveSM = FillArchiveSMInformation(_DataSet);
                return _ListOfArchiveSM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveSM> GetArchiveSMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsCompeteData)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveSM> _ListOfArchiveSM = null;

                _DataSet = _ISocialMediaModel.GetArchiveSMByCategoryGuidAndDate(p_ClientGUID, p_SortField, p_IsAscending, p_Date, p_CategoryGuid, p_IsCompeteData);
                _ListOfArchiveSM = FillArchiveSMInformation(_DataSet);
                return _ListOfArchiveSM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveSM> GetArchiveSMByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveSM> _ListOfArchiveSM = null;

                _DataSet = _ISocialMediaModel.GetArchiveSMByDurationNCategoryGuid(p_ClientGUID, p_SortField, p_IsAscending, p_FromDate, p_ToDate, p_CategoryGuid, p_IsCompeteData);
                _ListOfArchiveSM = FillArchiveSMInformation(_DataSet);
                return _ListOfArchiveSM;

            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}