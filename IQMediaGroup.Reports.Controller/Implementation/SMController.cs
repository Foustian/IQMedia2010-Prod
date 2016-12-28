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
    public class SMController : ISMController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ISMModel _ISMModel;

        public SMController()
        {
            _ISMModel = _ModelFactory.CreateObject<ISMModel>();
        }

        public List<SMResult> GetIQAgent_SMResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsCompeteData, out string Query_Name)
        {
            try
            {
                DataSet _DataSet = null;
                List<SMResult> _ListOfSMResult = null;

                _DataSet = _ISMModel.GetIQAgent_SMResultBySearchDate(p_ClientGuid, p_IQAgentSearchRequestID, p_FromDate, p_ToDate, p_NoOfRecordsToDisplay, p_IsCompeteData, out Query_Name);
                _ListOfSMResult = FillNewsResultInformation(_DataSet);
                return _ListOfSMResult;

            }
            catch (Exception)
            {
                throw;
            }
        }


        private List<SMResult> FillNewsResultInformation(DataSet _DataSet)
        {
            try
            {
                List<SMResult> _ListOfSMResult = new List<SMResult>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        SMResult _SMResult = new SMResult();

                        if (_DataTable.Columns.Contains("SeqID") && !_DataRow["SeqID"].Equals(DBNull.Value))
                        {
                            _SMResult.id = Convert.ToString(_DataRow["SeqID"]);
                        }

                        if (_DataTable.Columns.Contains("link") && !_DataRow["link"].Equals(DBNull.Value))
                        {
                            _SMResult.link = Convert.ToString(_DataRow["link"]);
                        }

                        if (_DataTable.Columns.Contains("homelink") && !_DataRow["homelink"].Equals(DBNull.Value))
                        {
                            _SMResult.homeLink = Convert.ToString(_DataRow["homelink"]);
                        }

                        if (_DataTable.Columns.Contains("description") && !_DataRow["description"].Equals(DBNull.Value))
                        {
                            _SMResult.description = Convert.ToString(_DataRow["description"]);
                        }

                        if (_DataTable.Columns.Contains("itemHarvestDate_DT") && !_DataRow["itemHarvestDate_DT"].Equals(DBNull.Value))
                        {
                            _SMResult.itemHarvestDate_DT = Convert.ToString(_DataRow["itemHarvestDate_DT"]);
                        }

                        if (_DataTable.Columns.Contains("feedCategories") && !_DataRow["feedCategories"].Equals(DBNull.Value))
                        {
                            _SMResult.feedCategories = Convert.ToString(_DataRow["feedCategories"]);
                        }

                        if (_DataTable.Columns.Contains("feedClass") && !_DataRow["feedClass"].Equals(DBNull.Value))
                        {
                            _SMResult.feedClass = Convert.ToString(_DataRow["feedClass"]);
                        }

                        if (_DataTable.Columns.Contains("feedRank") && !_DataRow["feedRank"].Equals(DBNull.Value))
                        {
                            _SMResult.feedRank = Convert.ToInt16(_DataRow["feedRank"]);
                        }

                        if (_DataTable.Columns.Contains("IQ_AdShare_Value") && !_DataRow["IQ_AdShare_Value"].Equals(DBNull.Value))
                        {
                            _SMResult.IQ_AdShare_Value = Convert.ToDecimal(_DataRow["IQ_AdShare_Value"]);
                        }

                        if (_DataTable.Columns.Contains("c_uniq_visitor") && !_DataRow["c_uniq_visitor"].Equals(DBNull.Value))
                        {
                            _SMResult.C_uniq_visitor = Convert.ToInt32(_DataRow["c_uniq_visitor"]);
                        }

                        if (_DataTable.Columns.Contains("IsCompeteAll") && !_DataRow["IsCompeteAll"].Equals(DBNull.Value))
                        {
                            _SMResult.IsCompeteAll = Convert.ToBoolean(_DataRow["IsCompeteAll"]);
                        }

                        if (_DataTable.Columns.Contains("IsUrlFound") && !_DataRow["IsUrlFound"].Equals(DBNull.Value))
                        {
                            _SMResult.IsUrlFound = Convert.ToBoolean(_DataRow["IsUrlFound"]);
                        }

                        _ListOfSMResult.Add(_SMResult);
                    }
                }

                return _ListOfSMResult;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertArchiveSM(ArchiveSM p_ArchiveSM)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ISMModel.InsertArchiveSM(p_ArchiveSM);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}