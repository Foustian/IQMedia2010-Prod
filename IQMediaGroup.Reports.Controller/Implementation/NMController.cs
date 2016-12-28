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
using PMGSearch;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    public class NMController : INMController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly INMModel _INMModel;

        public NMController()
        {
            _INMModel = _ModelFactory.CreateObject<INMModel>();
        }


        public List<NewsResult> GetIQAgent_NMResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsCompeteData, out string Query_Name)
        {
            try
            {
                DataSet _DataSet = null;
                List<NewsResult> _ListOfNewsResult = null;

                _DataSet = _INMModel.GetIQAgent_NMResultBySearchDate(p_ClientGuid, p_IQAgentSearchRequestID, p_FromDate, p_ToDate, p_NoOfRecordsToDisplay, p_IsCompeteData, out Query_Name);
                _ListOfNewsResult = FillNewsResultInformation(_DataSet);
                return _ListOfNewsResult;

            }
            catch (Exception)
            {
                throw;
            }
        }


        private List<NewsResult> FillNewsResultInformation(DataSet _DataSet)
        {
            try
            {
                List<NewsResult> _ListOfNewsResult = new List<NewsResult>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        NewsResult _NewsResult = new NewsResult();

                        if (_DataTable.Columns.Contains("URL") && !_DataRow["URL"].Equals(DBNull.Value))
                        {
                            _NewsResult.Article = Convert.ToString(_DataRow["URL"]);
                        }

                        if (_DataTable.Columns.Contains("Publication") && !_DataRow["Publication"].Equals(DBNull.Value))
                        {
                            _NewsResult.publication = Convert.ToString(_DataRow["Publication"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _NewsResult.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("harvest_Time") && !_DataRow["harvest_Time"].Equals(DBNull.Value))
                        {
                            _NewsResult.date = Convert.ToString(_DataRow["harvest_Time"]);
                        }

                        if (_DataTable.Columns.Contains("Genre") && !_DataRow["Genre"].Equals(DBNull.Value))
                        {
                            _NewsResult.Genre = Convert.ToString(_DataRow["Genre"]);
                        }

                        if (_DataTable.Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _NewsResult.ID = Convert.ToString(_DataRow["ID"]);
                        }

                        if (_DataTable.Columns.Contains("ArticleID") && !_DataRow["ArticleID"].Equals(DBNull.Value))
                        {
                            _NewsResult.ID = Convert.ToString(_DataRow["ArticleID"]);
                        }

                        if (_DataTable.Columns.Contains("Category") && !_DataRow["Category"].Equals(DBNull.Value))
                        {
                            _NewsResult.Category = Convert.ToString(_DataRow["Category"]);
                        }

                        if (_DataTable.Columns.Contains("Genre") && !_DataRow["Genre"].Equals(DBNull.Value))
                        {
                            _NewsResult.Genre = Convert.ToString(_DataRow["Genre"]);
                        }

                        if (_DataTable.Columns.Contains("IQ_AdShare_Value") && !_DataRow["IQ_AdShare_Value"].Equals(DBNull.Value))
                        {
                            _NewsResult.IQ_AdShare_Value = Convert.ToDecimal(_DataRow["IQ_AdShare_Value"]);
                        }

                        if (_DataTable.Columns.Contains("c_uniq_visitor") && !_DataRow["c_uniq_visitor"].Equals(DBNull.Value))
                        {
                            _NewsResult.C_uniq_visitor = Convert.ToInt32(_DataRow["c_uniq_visitor"]);
                        }

                        if (_DataTable.Columns.Contains("IsCompeteAll") && !_DataRow["IsCompeteAll"].Equals(DBNull.Value))
                        {
                            _NewsResult.IsCompeteAll = Convert.ToBoolean(_DataRow["IsCompeteAll"]);
                        }

                        if (_DataTable.Columns.Contains("IsUrlFound") && !_DataRow["IsUrlFound"].Equals(DBNull.Value))
                        {
                            _NewsResult.IsUrlFound = Convert.ToBoolean(_DataRow["IsUrlFound"]);
                        }

                        /*if (_DataTable.Columns.Contains("ClientGuid") && !_DataRow["ClientGuid"].Equals(DBNull.Value))
                        {
                            _NewsResult.ClientGuid = new Guid(Convert.ToString(_DataRow["ClientGuid"]));
                        }*/





                        _ListOfNewsResult.Add(_NewsResult);
                    }
                }

                return _ListOfNewsResult;

            }
            catch (Exception)
            {
                throw;
            }
        }




    }

}