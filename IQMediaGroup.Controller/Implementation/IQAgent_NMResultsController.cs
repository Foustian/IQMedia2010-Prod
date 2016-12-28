using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQAgent_NMResultsController : IIQAgent_NMResultsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQAgent_NMResultsModel _IIQAgent_NMResultsModel;

        public IQAgent_NMResultsController()
        {
            _IIQAgent_NMResultsModel = _ModelFactory.CreateObject<IIQAgent_NMResultsModel>();
        }

        public List<IQAgent_NMResult> GetIQAgentNMResultsBySearchRequestID(int p_SearchRequestID, int p_PageSize, int p_PageNumber, string p_SortField, bool p_IsAcending, out int p_TotalRecordCount)
        {
            try
            {
                List<IQAgent_NMResult> _ListOfIQAgent_NMResults;
                DataSet _DataSet = _IIQAgent_NMResultsModel.GetIQAgentNMResultsBySearchRequestID(p_SearchRequestID, p_PageSize, p_PageNumber, p_SortField, p_IsAcending, out p_TotalRecordCount);
                _ListOfIQAgent_NMResults = FillIQAgent_NMResultsInformation(_DataSet);
                return _ListOfIQAgent_NMResults;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteIQAgent_NMResults(string IQAgent_NMResultKey)
        {
            try
            {
                string Result = string.Empty;
                Result = _IIQAgent_NMResultsModel.DeleteIQAgent_NMResults(IQAgent_NMResultKey);
                return Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IQAgent_NMResult> FillIQAgent_NMResultsInformation(DataSet _DataSet)
        {
            try
            {
                List<IQAgent_NMResult> _ListOfIQAgent_NMResult = new List<IQAgent_NMResult>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        IQAgent_NMResult _IQAgent_NMResult = new IQAgent_NMResult();

                        if (_DataTable.Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.ID = Convert.ToInt32(_DataRow["ID"]);
                        }

                        if (_DataTable.Columns.Contains("IQAgentSearchRequestID") && !_DataRow["IQAgentSearchRequestID"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.IQAgentSearchRequestID = Convert.ToInt32(_DataRow["IQAgentSearchRequestID"]);
                        }

                        if (_DataTable.Columns.Contains("ArticleID") && !_DataRow["ArticleID"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.ArticleID = Convert.ToString(_DataRow["ArticleID"]);
                        }

                        if (_DataTable.Columns.Contains("Url") && !_DataRow["Url"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.Url = Convert.ToString(_DataRow["Url"]);
                        }

                        if (_DataTable.Columns.Contains("harvest_time") && !_DataRow["harvest_time"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.harvest_time = Convert.ToDateTime(_DataRow["harvest_time"]);
                        }

                        if (_DataTable.Columns.Contains("Publication") && !_DataRow["Publication"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.Publication = Convert.ToString(_DataRow["Publication"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("Category") && !_DataRow["Category"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.Category = Convert.ToString(_DataRow["Category"]);
                        }

                        if (_DataTable.Columns.Contains("Genre") && !_DataRow["Genre"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.Genre = Convert.ToString(_DataRow["Genre"]);
                        }

                        if (_DataTable.Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }

                        if (_DataTable.Columns.Contains("IsActive") && !_DataRow["IsActive"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        }

                        _ListOfIQAgent_NMResult.Add(_IQAgent_NMResult);
                    }
                }

                return _ListOfIQAgent_NMResult;

            }
            catch (Exception)
            {
                throw;
            }
        } 
    }
}
