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
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using System.Data.SqlTypes;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class IQAgentResultsController : IIQAgentResultsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQAgentResultsModel _IIQAgentResultsModel;

        public IQAgentResultsController()
        {
            _IIQAgentResultsModel = _ModelFactory.CreateObject<IIQAgentResultsModel>();
        }

        /// <summary>
        /// Description: This Methods gets Search request Information from DataSet.
        /// Added By:Maulik gandhi
        /// </summary>
        /// <returns>List of Object of Search Request</returns>
        public MasterIQResults GetSearchRequestByQueryName(IQAgentResults p_IQAgentResults)
        {
            DataSet _DataSet = null;
            //List<IQAgentResults> _ListOfIQAgentResults = null;
            MasterIQResults _MasterIQResults = new MasterIQResults();
            try
            {
                _DataSet = _IIQAgentResultsModel.GetRequestByQueryName(p_IQAgentResults);

                _MasterIQResults._ListofQuery = FillQueryName(_DataSet);
                _MasterIQResults._ListofResults = FillSearchRequestInformation(_DataSet);
                //_ListOfIQAgentResults = 
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            // return _ListOfIQAgentResults;
            return _MasterIQResults;
        }

        /// <summary>
        /// Description: This Methods Fills SearchRequest Information from DataSet.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for Search Request Infromarmation</param>
        /// <returns>List of Object of Search Request</returns>
        private List<IQAgentResults> FillQueryName(DataSet _DataSet)
        {
            List<IQAgentResults> _ListOfIQAgentQuery = new List<IQAgentResults>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[1].Rows)
                    {
                        IQAgentResults _IQAgentResults = new IQAgentResults();
                        _IQAgentResults.SearchTerm = Convert.ToString(_DataRow["SearchTerm"]);
                        _ListOfIQAgentQuery.Add(_IQAgentResults);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfIQAgentQuery;
        }


        /// <summary>
        /// Description: This Methods Fills SearchRequest Information from DataSet.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for Search Request Infromarmation</param>
        /// <returns>List of Object of Search Request</returns>
        private List<IQAgentResults> FillSearchRequestInformation(DataSet _DataSet)
        {
            List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQAgentResults _IQAgentResults = new IQAgentResults();
                        _IQAgentResults.IQAgentResultKey = Convert.ToInt64(_DataRow["IQAgentResultKey"]);
                        _IQAgentResults.RL_VideoGUID = new Guid(_DataRow["RL_VideoGUID"].ToString());
                        //_IQAgentResults.CC_Text = Convert.ToString(_DataRow["CC_Text"]);
                        _IQAgentResults.Number_Hits = Convert.ToInt32(_DataRow["Number_Hits"]);

                        string RL_Station_Date = Convert.ToString(Convert.ToDateTime(_DataRow["RL_Date"]).ToShortDateString());
                        int Time = (Convert.ToInt32(_DataRow["RL_Time"]) / 100);
                        string RL_Station_Time = Time > 12 ? Time.ToString() + ":00:00 PM" : Time.ToString() + ":00:00 AM";
                        _IQAgentResults.RL_Date = Convert.ToDateTime(RL_Station_Date + " " + RL_Station_Time);

                        if (_DataSet.Tables[0].Columns.Contains("Rl_Market"))
                        {
                            _IQAgentResults.RL_Market = Convert.ToString(_DataRow["Rl_Market"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Rl_Station"))
                        {
                            _IQAgentResults.Rl_Station = Convert.ToString(_DataRow["Rl_Station"]);
                        }

                        //_IQAgentResults.RL_Date = new DateTime(;

                        //_IQAgentResults.SearchTerm = Convert.ToString(_DataRow["SearchTerm"]);
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

        /// <summary>
        /// Description:This Method will Delete IQAgentResult
        /// </summary>
        /// <param name="p_IQAgentResultKeys">IQAgentResultKeys</param>
        /// <returns>Count</returns>
        public string DeleteIQAgentResult(string p_IQAgentResultKeys)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IIQAgentResultsModel.DeleteIQAgentResult(p_IQAgentResultKeys);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        ///// <summary>
        ///// This method inserts IQAgentResults details
        ///// </summary>
        ///// <param name="p_IQAgentResults">Object of Class IQAgentResults</param>
        ///// <returns>return Primary Key if details inserted successfully</returns>
        //public string InsertIQAgentResult(IQAgentResults p_IQAgentResults)
        //{
        //    try
        //    {
        //        string _Result = string.Empty;

        //        _Result = _IIQAgentResultsModel.InsertIQAgentResult(p_IQAgentResults);

        //        return _Result;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        /// <summary>
        /// Description:This Method will Insert IQAgentResult
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <returns>Primary key of IQAgentResult</returns>
        public string InsertIQAgentResultsList(SqlXml p_SqlXml)
        {
            try
            {
                string _ReturnValue = _IIQAgentResultsModel.InsertIQAgentResultsList(p_SqlXml);
                return _ReturnValue;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets IQAgentResults and SearchTerm by SearchRequestID
        /// </summary>
        /// <param name="p_SearchRequestID">SearchRequestID</param>
        /// <param name="p_PageNumber">Page Number</param>
        /// <param name="p_PageSize">Page Size</param>
        /// <param name="p_SortField">Field on which sorting applied</param>
        /// <param name="p_IsAscSortDirection">Sort Direction is Ascending or not</param>
        /// <param name="p_SearchTerm">Search Term</param>
        /// <returns>DataSet contains IQAgentResults details</returns>
        public List<IQAgentResults> GetIQAgentResultBySearchRequest(int p_SearchRequestID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_SortDirection, out string p_SearchTerm)
        {
            try
            {
                DataSet _DataSet = null;
                List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();

                _DataSet = _IIQAgentResultsModel.GetIQAgentResultBySearchRequest(p_SearchRequestID, p_PageNumber, p_PageSize, p_SortField, p_SortDirection, out p_SearchTerm);

                _ListOfIQAgentResults = FillSearchRequestInformation(_DataSet);

                return _ListOfIQAgentResults;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will Insert IQAgentResult
        /// </summary>
        /// <param name="p_SqlXml">XML</param>
        /// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        /// <returns>Primary key of IQAgentResult</returns>
        public string InsertIQAgentResultsList(SqlXml p_SqlXml, ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                string _ReturnValue = _IIQAgentResultsModel.InsertIQAgentResultsList(p_SqlXml, p_ConnectionStringKeys);
                return _ReturnValue;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public List<IQAgentResults> SelectForParentChildRelationship(IQAgentResults _IQAgentResults, out string p_SearchTerm, out int p_TotalRecords)
        {
            try
            {
                DataSet _DataSet = null;

                p_SearchTerm = string.Empty;
                p_TotalRecords = 0;

                List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();

                _DataSet = _IIQAgentResultsModel.SelectForParentChildRelationship(_IQAgentResults);

                _ListOfIQAgentResults = FillParentChildResultSet(_DataSet);

                if (_DataSet.Tables.Count >= 3 && _DataSet.Tables[2] != null && _DataSet.Tables[2].Rows.Count > 0)
                {
                    p_SearchTerm = _DataSet.Tables[2].Rows[0]["SearchTerm"].ToString();
                }

                if (_DataSet.Tables.Count >= 4 && _DataSet.Tables[3] != null && _DataSet.Tables[3].Rows.Count > 0)
                {
                    p_TotalRecords = Convert.ToInt32(_DataSet.Tables[3].Rows[0]["TotalRecords"].ToString());
                }

                return _ListOfIQAgentResults;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods Fills SearchRequest Information from DataSet.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for Search Request Infromarmation</param>
        /// <returns>List of Object of Search Request</returns>
        private List<IQAgentResults> FillParentChildResultSet(DataSet _DataSet)
        {
            List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQAgentResults _IQAgentResults = new IQAgentResults();

                        // Fill Parent objects
                        if (_DataSet.Tables[0].Columns.Contains("IQAgentResultKey") && !_DataRow["IQAgentResultKey"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.IQAgentResultKey = Convert.ToInt64(_DataRow["IQAgentResultKey"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("RL_VideoGUID") && !_DataRow["RL_VideoGUID"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.RL_VideoGUID = new Guid(_DataRow["RL_VideoGUID"].ToString());
                        }

                        //if (_DataSet.Tables[0].Columns.Contains("CC_Text") && !_DataRow["CC_Text"].Equals(DBNull.Value))
                        //{
                        //    _IQAgentResults.CC_Text = Convert.ToString(_DataRow["CC_Text"]);
                        //}

                        if (_DataSet.Tables[0].Columns.Contains("Number_Hits") && !_DataRow["Number_Hits"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.Number_Hits = Convert.ToInt32(_DataRow["Number_Hits"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Title120") && !_DataRow["Title120"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.Title120 = Convert.ToString(_DataRow["Title120"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("StationMarket") && !_DataRow["StationMarket"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.StationMarket = Convert.ToString(_DataRow["StationMarket"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("StationID") && !_DataRow["StationID"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.Rl_Station = Convert.ToString(_DataRow["StationID"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("Hits") && !_DataRow["Hits"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.Number_Hits = Convert.ToInt32(_DataRow["Hits"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("DatabaseKey") && !_DataRow["DatabaseKey"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.DatabaseKey = Convert.ToString(_DataRow["DatabaseKey"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("IQ_Local_Air_DateTime") && !_DataRow["IQ_Local_Air_DateTime"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.IQ_Local_Air_DateTime = Convert.ToDateTime(_DataRow["IQ_Local_Air_DateTime"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("IQ_Local_Air_Date") && !_DataRow["IQ_Local_Air_Date"].Equals(DBNull.Value))
                        {
                            _IQAgentResults.IQ_Local_AirDate = Convert.ToDateTime(_DataRow["IQ_Local_Air_Date"]);
                        }


                        DataRow[] _ChildRows = _DataSet.Tables[1].Select("DatabaseKey=" + _DataRow["DatabaseKey"].ToString() + " AND IQ_Local_Air_Date = #" + _DataRow["IQ_Local_Air_Date"].ToString() + "#");
                        _IQAgentResults.ChildResults = new List<IQAgentResults>();

                        foreach (DataRow _ChildRow in _ChildRows)
                        {
                            IQAgentResults _IQAgentResultChildRow = new IQAgentResults();

                            if (_DataSet.Tables[1].Columns.Contains("IQAgentResultKey") && !_ChildRow["IQAgentResultKey"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.IQAgentResultKey = Convert.ToInt64(_ChildRow["IQAgentResultKey"]);
                            }

                            if (_DataSet.Tables[1].Columns.Contains("RL_VideoGUID") && !_ChildRow["RL_VideoGUID"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.RL_VideoGUID = new Guid(_ChildRow["RL_VideoGUID"].ToString());
                            }

                            //if (_DataSet.Tables[1].Columns.Contains("CC_Text") && !_ChildRow["CC_Text"].Equals(DBNull.Value))
                            //{
                            //    _IQAgentResultChildRow.CC_Text = Convert.ToString(_ChildRow["CC_Text"]);
                            //}

                            if (_DataSet.Tables[1].Columns.Contains("Number_Hits") && !_ChildRow["Number_Hits"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.Number_Hits = Convert.ToInt32(_ChildRow["Number_Hits"]);
                            }

                            if (_DataSet.Tables[1].Columns.Contains("Title120") && !_ChildRow["Title120"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.Title120 = Convert.ToString(_ChildRow["Title120"]);
                            }
                            if (_DataSet.Tables[1].Columns.Contains("StationMarket") && !_ChildRow["StationMarket"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.StationMarket = Convert.ToString(_ChildRow["StationMarket"]);
                            }
                            if (_DataSet.Tables[1].Columns.Contains("StationID") && !_ChildRow["StationID"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.Rl_Station = Convert.ToString(_ChildRow["StationID"]);
                            }
                            if (_DataSet.Tables[1].Columns.Contains("Hits") && !_ChildRow["Hits"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.Number_Hits = Convert.ToInt32(_ChildRow["Hits"]);
                            }
                            if (_DataSet.Tables[1].Columns.Contains("DatabaseKey") && !_ChildRow["DatabaseKey"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.DatabaseKey = Convert.ToString(_ChildRow["DatabaseKey"]);
                            }
                            if (_DataSet.Tables[1].Columns.Contains("IQ_Local_Air_DateTime") && !_ChildRow["IQ_Local_Air_DateTime"].Equals(DBNull.Value))
                            {
                                _IQAgentResultChildRow.IQ_Local_Air_DateTime = Convert.ToDateTime(_ChildRow["IQ_Local_Air_DateTime"]);
                            }
                            _IQAgentResults.ChildResults.Add(_IQAgentResultChildRow);
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
