using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQAgent_SMResultsController : IIQAgent_SMResultsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQAgent_SMResultsModel _IIQAgent_SMResultsModel;

        public IQAgent_SMResultsController()
        {
            _IIQAgent_SMResultsModel = _ModelFactory.CreateObject<IIQAgent_SMResultsModel>();
        }

        public List<IQAgent_SMResult> GetIQAgentSMResultsBySearchRequestID(int p_SearchRequestID, int p_PageSize, int p_PageNumber, string p_SortField, bool p_IsAcending,out int p_TotalRecordsCount)
        {
            try
            {
                List<IQAgent_SMResult> _ListOfIQAgent_NMResults;
                DataSet _DataSet = _IIQAgent_SMResultsModel.GetIQAgentSMResultsBySearchRequestID(p_SearchRequestID, p_PageSize, p_PageNumber, p_SortField, p_IsAcending, out p_TotalRecordsCount);
                _ListOfIQAgent_NMResults = FillIQAgent_SMResultsInformation(_DataSet);
                return _ListOfIQAgent_NMResults;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteIQAgent_SMResults(string IQAgent_SMResultKey)
        {
            try
            {
                string Result = string.Empty;
                Result = _IIQAgent_SMResultsModel.DeleteIQAgent_SMResults(IQAgent_SMResultKey);
                return Result;
            }
            catch
            {
                throw;
            }
        }

        private List<IQAgent_SMResult> FillIQAgent_SMResultsInformation(DataSet _DataSet)
        {
            try
            {
                List<IQAgent_SMResult> _ListOfIQAgent_NMResult = new List<IQAgent_SMResult>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        IQAgent_SMResult _IQAgent_NMResult = new IQAgent_SMResult();

                        if (_DataTable.Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.ID = Convert.ToInt32(_DataRow["ID"]);
                        }

                        if (_DataTable.Columns.Contains("IQAgentSearchRequestID") && !_DataRow["IQAgentSearchRequestID"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.IQAgentSearchRequestID = Convert.ToInt32(_DataRow["IQAgentSearchRequestID"]);
                        }

                        if (_DataTable.Columns.Contains("SeqID") && !_DataRow["SeqID"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.SeqID = Convert.ToString(_DataRow["SeqID"]);
                        }

                        if (_DataTable.Columns.Contains("link") && !_DataRow["link"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.link = Convert.ToString(_DataRow["link"]);
                        }

                        if (_DataTable.Columns.Contains("itemHarvestDate_DT") && !_DataRow["itemHarvestDate_DT"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.itemHarvestDate_DT = Convert.ToDateTime(_DataRow["itemHarvestDate_DT"]);
                        }

                        if (_DataTable.Columns.Contains("homelink") && !_DataRow["homelink"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.homelink = Convert.ToString(_DataRow["homelink"]);
                        }

                        if (_DataTable.Columns.Contains("description") && !_DataRow["description"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.description = Convert.ToString(_DataRow["description"]);
                        }

                        if (_DataTable.Columns.Contains("feedCategories") && !_DataRow["feedCategories"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.feedCategories = Convert.ToString(_DataRow["feedCategories"]);
                        }

                        if (_DataTable.Columns.Contains("feedClass") && !_DataRow["feedClass"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.feedClass = Convert.ToString(_DataRow["feedClass"]);
                        }

                        if (_DataTable.Columns.Contains("Genre") && !_DataRow["feedRank"].Equals(DBNull.Value))
                        {
                            _IQAgent_NMResult.feedRank = Convert.ToInt32(_DataRow["feedRank"]);
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
