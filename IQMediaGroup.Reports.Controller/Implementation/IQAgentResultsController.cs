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
using IQMediaGroup.Reports.Controller.Factory;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;
using System.Data.SqlTypes;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    internal class IQAgentResultsController : IIQAgentResultsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQAgentResultsModel _IIQAgentResultsModel;

        public IQAgentResultsController()
        {
            _IIQAgentResultsModel = _ModelFactory.CreateObject<IIQAgentResultsModel>();
        }

        public List<IQAgentResults> GetIQAgentResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, Boolean p_IsNielSenData, out string Query_Name, out string SearchTerm)
        {
            try
            {
                DataSet _DataSet = null;
                List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();

                _DataSet = _IIQAgentResultsModel.GetIQAgentResultBySearchDate(p_ClientGuid, p_IQAgentSearchRequestID, p_FromDate, p_ToDate, p_NoOfRecordsToDisplay, p_IsNielSenData, out Query_Name, out SearchTerm);

                _ListOfIQAgentResults = FillIQAgentResult(_DataSet);

                return _ListOfIQAgentResults;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Description: This Methods Fills SearchRequest Information from DataSet.
        /// </summary>
        /// <param name="_DataSet">Dataset for Search Request Infromarmation</param>
        /// <returns>List of Object of Search Request</returns>
        private List<IQAgentResults> FillIQAgentResult(DataSet _DataSet)
        {
            List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQAgentResults _IQAgentResults = new IQAgentResults();
                        if (_DataSet.Tables[0].Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.ID = Convert.ToInt64(_DataRow["ID"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Title120") && !_DataRow["Title120"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.Title120 = Convert.ToString(_DataRow["Title120"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("RL_VideoGUID") && !_DataRow["RL_VideoGUID"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.RL_VideoGUID = new Guid(_DataRow["RL_VideoGUID"].ToString());
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Number_Hits") && !_DataRow["Number_Hits"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.Number_Hits = Convert.ToInt32(_DataRow["Number_Hits"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQAgentResultUrl") && !_DataRow["IQAgentResultUrl"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.IQAgentResultUrl = Convert.ToString(_DataRow["IQAgentResultUrl"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("RL_Date") && !_DataRow["RL_Date"].Equals(DBNull.Value))
                        {
                            string RL_Station_Date = Convert.ToString(Convert.ToDateTime(_DataRow["RL_Date"]).ToShortDateString());
                            int Time = (Convert.ToInt32(_DataRow["RL_Time"]) / 100);
                            string RL_Station_Time = Time > 12 ? Time.ToString() + ":00:00 PM" : Time.ToString() + ":00:00 AM";
                            _IQAgentResults.RL_Date = Convert.ToDateTime(RL_Station_Date + " " + RL_Station_Time);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Rl_Market"))
                        {
                            _IQAgentResults.RL_Market = Convert.ToString(_DataRow["Rl_Market"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Rl_Station"))
                        {
                            _IQAgentResults.Rl_Station = Convert.ToString(_DataRow["Rl_Station"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IsActualNielsen"))
                        {
                            _IQAgentResults.IsActualNielsen = Convert.ToBoolean(_DataRow["IsActualNielsen"]);

                            if (_DataSet.Tables[0].Columns.Contains("SQAD_SHAREVALUE") && !_DataRow["SQAD_SHAREVALUE"].Equals(DBNull.Value))
                            {
                                _IQAgentResults.SQAD_SHAREVALUE = string.Format("{0:N}{1}",_DataRow["SQAD_SHAREVALUE"],_IQAgentResults.IsActualNielsen?"(A)":"(E)");
                            } 
                        }

                        if (_DataSet.Tables[0].Columns.Contains("AUDIENCE") && !_DataRow["AUDIENCE"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.AUDIENCE = Convert.ToInt32(_DataRow["AUDIENCE"]);
                        }

                        _ListOfIQAgentResults.Add(_IQAgentResults);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfIQAgentResults;
        }
    }
}
