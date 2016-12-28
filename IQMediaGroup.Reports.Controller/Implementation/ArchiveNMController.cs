using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using System.Web;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Reports.Model.Interface;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    public class ArchiveNMController : IArchiveNMController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IArchiveNMModel _IArchiveNMModel;

        public ArchiveNMController()
        {
            _IArchiveNMModel = _ModelFactory.CreateObject<IArchiveNMModel>();
        }

        private List<ArchiveNM> FillArchiveNMInformation(DataSet _DataSet)
        {
            try
            {
                List<ArchiveNM> _ListOfArchiveNM = new List<ArchiveNM>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ArchiveNM _ArchiveNM = new ArchiveNM();

                        if (_DataTable.Columns.Contains("ArchiveNMKey") && !_DataRow["ArchiveNMKey"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.ArchiveNMKey = Convert.ToInt32(_DataRow["ArchiveNMKey"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("FirstName") && !_DataRow["FirstName"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        }

                        if (_DataTable.Columns.Contains("LastName") && !_DataRow["LastName"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.LastName = Convert.ToString(_DataRow["LastName"]);
                        }

                        if (_DataTable.Columns.Contains("CustomerGuid") && !_DataRow["CustomerGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.CustomerGuid = new Guid(Convert.ToString(_DataRow["CustomerGuid"]));
                        }

                        if (_DataTable.Columns.Contains("ClientGuid") && !_DataRow["ClientGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.ClientGuid = new Guid(Convert.ToString(_DataRow["ClientGuid"]));
                        }

                        if (_DataTable.Columns.Contains("CategoryGuid") && !_DataRow["CategoryGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.CategoryGuid = new Guid(Convert.ToString(_DataRow["CategoryGuid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory1Guid") && !_DataRow["SubCategory1Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.SubCategory1Guid = new Guid(Convert.ToString(_DataRow["SubCategory1Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory2Guid") && !_DataRow["SubCategory2Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.SubCategory2Guid = new Guid(Convert.ToString(_DataRow["SubCategory2Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory3Guid") && !_DataRow["SubCategory3Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.SubCategory3Guid = new Guid(Convert.ToString(_DataRow["SubCategory3Guid"]));
                        }

                        if (_DataTable.Columns.Contains("ArticleID") && !_DataRow["ArticleID"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.ArticleID = Convert.ToString(_DataRow["ArticleID"]);
                        }

                        if (_DataTable.Columns.Contains("ArticleContent") && !_DataRow["ArticleContent"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Content = Convert.ToString(_DataRow["ArticleContent"]);
                        }

                        if (_DataTable.Columns.Contains("Url") && !_DataRow["Url"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Url = Convert.ToString(_DataRow["Url"]);
                        }


                        if (_DataTable.Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.CategoryNames = Convert.ToString(_DataRow["CategoryName"]);
                        }

                        if (_DataTable.Columns.Contains("Rating") && !_DataRow["Rating"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Rating = Convert.ToInt16(_DataRow["Rating"]);
                        }

                        if (_DataTable.Columns.Contains("Total") && !_DataRow["Total"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Total = Convert.ToInt32(_DataRow["Total"]);
                        }

                        if (_DataTable.Columns.Contains("Publication") && !_DataRow["Publication"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Publication = Convert.ToString(_DataRow["Publication"]);
                        }

                        if (_DataTable.Columns.Contains("IQ_AdShare_Value") && !_DataRow["IQ_AdShare_Value"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.IQ_AdShare_Value = Convert.ToDecimal(_DataRow["IQ_AdShare_Value"]);
                        }

                        if (_DataTable.Columns.Contains("c_uniq_visitor") && !_DataRow["c_uniq_visitor"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.c_uniq_visitor = Convert.ToInt32(_DataRow["c_uniq_visitor"]);
                        }

                        if (_DataTable.Columns.Contains("IsUrlFound") && !_DataRow["IsUrlFound"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.IsUrlFound = Convert.ToBoolean(_DataRow["IsUrlFound"]);
                        }

                        if (_DataTable.Columns.Contains("IsCompeteAll") && !_DataRow["IsCompeteAll"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.IsCompeteAll = Convert.ToBoolean(_DataRow["IsCompeteAll"]);
                        }

                        if (_DataTable.Columns.Contains("harvest_time") && !_DataRow["harvest_time"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Harvest_Time = Convert.ToDateTime(_DataRow["harvest_time"]);
                        }

                        if (_DataTable.Columns.Contains("Publication") && !_DataRow["Publication"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Publication = Convert.ToString(_DataRow["Publication"]);
                        }



                        _ListOfArchiveNM.Add(_ArchiveNM);
                    }
                }

                return _ListOfArchiveNM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveNM> GetArchiveNMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveNM> _ListOfArchiveNM = null;

                _DataSet = _IArchiveNMModel.GetArchiveNMReportGroupByCategory(p_ClientGUID, p_Date);
                _ListOfArchiveNM = FillArchiveNMInformation(_DataSet);
                return _ListOfArchiveNM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveNM> GetArchiveNMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid, Boolean p_IsCompeteData)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveNM> _ListOfArchiveNM = null;

                _DataSet = _IArchiveNMModel.GetArchiveNMByCategoryGuidAndDate(p_ClientGUID, p_SortField, p_IsAscending, p_Date, p_CategoryGuid, p_IsCompeteData);
                _ListOfArchiveNM = FillArchiveNMInformation(_DataSet);
                return _ListOfArchiveNM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertArchiveNM(ArchiveNM p_ArchiveClips)
        {
            try
            {
                string _Result = String.Empty;
                _Result = _IArchiveNMModel.InsertArchiveNM(p_ArchiveClips);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveNM> GetArchiveNMByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid, Boolean p_IsCompeteData)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveNM> _ListOfArchiveNM = null;

                _DataSet = _IArchiveNMModel.GetArchiveNMByDurationNCategoryGuid(p_ClientGUID, p_SortField, p_IsAscending, p_FromDate, p_ToDate, p_CategoryGuid, p_IsCompeteData);
                _ListOfArchiveNM = FillArchiveNMInformation(_DataSet);
                return _ListOfArchiveNM;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
