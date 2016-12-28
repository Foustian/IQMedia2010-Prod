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
using System.Threading;
using PMGSearch;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQAgentSearchRequestController : ISearchRequestController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ISearchRequestModel _ISearchRequestModel;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public IQAgentSearchRequestController()
        {
            _ISearchRequestModel = _ModelFactory.CreateObject<ISearchRequestModel>();
        }

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        ///// <summary>
        ///// Description: This method inserts Search Request Information
        ///// Added By:Maulik Gandhi
        ///// </summary>
        ///// <param name="p_SearchRequests">Object Of SearchRequests Class</param>
        ///// <returns>SearchRequestKey</returns>
        //public string InsertSearchRequest(IQAgentSearchRequest p_IQAgentSearchRequest)
        //{
        //    try
        //    {
        //        string _Result = string.Empty;
        //        _Result = _ISearchRequestModel.InsertSearchRequest(p_IQAgentSearchRequest);

        //        return _Result;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        ///// <summary>
        ///// Description: This Methods Fills SearchRequest Information from DataSet.
        ///// Added By: Maulik Gandhi
        ///// </summary>
        ///// <param name="_DataSet">Dataset for Search Request Infromarmation</param>
        ///// <returns>List of Object of Search Request</returns>
        //private List<IQAgentSearchRequest> FillSearchRequestInformation(DataSet _DataSet)
        //{
        //    List<IQAgentSearchRequest> _ListOfSearchRequest = new List<IQAgentSearchRequest>();

        //    try
        //    {
        //        if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
        //            {
        //                IQAgentSearchRequest _IQAgentSearchRequest = new IQAgentSearchRequest();
        //                _IQAgentSearchRequest.Query_Name = Convert.ToString(_DataRow["Query_Name"]);
        //                _IQAgentSearchRequest.SearchRequestKey = Convert.ToInt32(_DataRow["SearchRequestKey"]);
        //                _ListOfSearchRequest.Add(_IQAgentSearchRequest);
        //            }
        //        }
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //    return _ListOfSearchRequest;
        //}


        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        ///// <summary>
        ///// Description: This Methods gets Search request Information from DataSet.
        ///// Added By:Maulik gandhi
        ///// </summary>
        ///// <returns>List of Object of Search Request</returns>
        //public List<IQAgentSearchRequest> GetSearchRequestByQueryName(IQAgentSearchRequest p_IQAgentSearchRequest)
        //{
        //    DataSet _DataSet = null;
        //    List<IQAgentSearchRequest> _ListOfSearchRequest = null;
        //    try
        //    {
        //        _DataSet = _ISearchRequestModel.GetSearchRequestByQueryName(p_IQAgentSearchRequest);
        //        _ListOfSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //    return _ListOfSearchRequest;
        //}

        /// <summary>
        /// Description: This Methods Fills SearchRequest Information from DataSet.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for Search Request Infromarmation</param>
        /// <returns>List of Object of Search Request</returns>
        private List<IQAgentSearchRequest> FillSearchRequestInformationByQueryName(DataSet _DataSet)
        {
            List<IQAgentSearchRequest> _ListOfSearchRequest = new List<IQAgentSearchRequest>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQAgentSearchRequest _IQAgentSearchRequest = new IQAgentSearchRequest();

                        if (_DataTable.Columns.Contains("ID"))
                        {
                            _IQAgentSearchRequest.ID = CommonFunctions.GetIntValue(_DataRow["ID"].ToString());
                        }

                        if (_DataTable.Columns.Contains("ClientGUID") && !_DataRow["ClientGUID"].Equals(DBNull.Value))
                        {
                            _IQAgentSearchRequest.ClientGuid = new Guid(_DataRow["ClientGUID"].ToString());
                        }


                        if (_DataTable.Columns.Contains("Query_Name"))
                        {
                            _IQAgentSearchRequest.Query_Name = Convert.ToString(_DataRow["Query_Name"]);
                        }

                        if (_DataTable.Columns.Contains("Query_Version"))
                        {
                            _IQAgentSearchRequest.Query_Version = CommonFunctions.GetIntValue(_DataRow["Query_Version"].ToString());
                        }

                        if (_DataTable.Columns.Contains("SearchTerm"))
                        {
                            _IQAgentSearchRequest.SearchTerm = Convert.ToString(_DataRow["SearchTerm"]);
                        }                        

                        if (_DataTable.Columns.Contains("IsActive"))
                        {
                            _IQAgentSearchRequest.IsActive = Convert.ToBoolean(_DataRow["IsActive"].ToString());
                        }

                        _ListOfSearchRequest.Add(_IQAgentSearchRequest);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSearchRequest;
        }

        /// <summary>
        /// Description: This Methods gets Search request Information from DataSet By ClientID.
        /// </summary>
        /// <returns>List of Object of Search Request</returns>
        public List<IQAgentSearchRequest> SelectByClientID(Guid ClientGuid)
        {
            DataSet _DataSet = null;
            List<IQAgentSearchRequest> _ListOfSearchRequest = null;
            try
            {
                _DataSet = _ISearchRequestModel.SelectByClientID(ClientGuid);
                _ListOfSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSearchRequest;
        }

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <returns>List of object of IQAgentSearchRequest</returns>
        public List<IQAgentSearchRequest> GetSearchRequestAll()
        {
            try
            {
                List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = null;

                DataSet _DataSet = _ISearchRequestModel.GetSearchRequestAll();

                _ListOfIQAgentSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);

                return _ListOfIQAgentSearchRequest;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public int SearchRawMediaFromPMGSearch(string p_FilePath)
        {
            bool _IsDefaultDate = false;
            bool _IsDefiniteQuery = false;
            int _NoOfInsertedRecords = 0;

            SearchParametersIQAgent _SearchParametersIQAgent = ReadParamsFromFile(p_FilePath, ref _IsDefaultDate, ref _IsDefiniteQuery);

            CommonFunctions.BuildConnectionStringFromUserConfig(_SearchParametersIQAgent.ConnectionString);
            string _DateTimeAppend = DateTime.Now.ToString();
            _DateTimeAppend = _DateTimeAppend.Replace(" ", "_").Replace(":", "_").Replace("/", "_");

            IQAgentLog _IQAgentLog = new IQAgentLog();
            _IQAgentLog.StoredProcedure = new List<string>();
            _IQAgentLog.Tables = new List<string>();
            _IQAgentLog.ProgramStartTime = DateTime.Now.ToString();
            _IQAgentLog.ConfigFileParams = _SearchParametersIQAgent;

            List<Requests> _ListOfRequest = new List<Requests>();

            try
            {

                InsertIQServiceLog("Start Event", p_FilePath);

                List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = null;
                _IQAgentLog.ListOfRequests = _ListOfRequest;

                if (_IsDefiniteQuery == false)
                {
                    _ListOfIQAgentSearchRequest = GetSearchRequestAll();
                    _IQAgentLog.StoredProcedure.Add(CommonConstants.usp_IQAgentSearchRequest_Select);
                    _IQAgentLog.Tables.Add(CommonConstants.Table_IQAgentSearchRequest);
                }
                else
                {
                    _ListOfIQAgentSearchRequest = GetSearchRequestsByClientIDQueryNameVersion(new Guid(_SearchParametersIQAgent.ClientGuid), _SearchParametersIQAgent.QueryName, Convert.ToInt32(_SearchParametersIQAgent.QueryVersion));
                    _IQAgentLog.StoredProcedure.Add(CommonConstants.usp_IQAgentSearchRequest_SelectByClientQueryVersion);
                    _IQAgentLog.Tables.Add(CommonConstants.Table_IQAgentSearchRequest);
                }

                SearchRequest _SearchRequest = new SearchRequest();

                _SearchRequest.StartDate = Convert.ToDateTime(_SearchParametersIQAgent.StartDate);
                _SearchRequest.EndDate = Convert.ToDateTime(_SearchParametersIQAgent.EndDate);


                foreach (IQAgentSearchRequest _IQAgentSearchRequest in _ListOfIQAgentSearchRequest)
                {
                    Requests _Request = new Requests();
                    List<IQ_Agent_FetchLog> ListOfPMGRequests = new List<IQ_Agent_FetchLog>();
                    List<Exception> ListOfExceptions = new List<Exception>();
                    AdvancedSearch _AdvancedSearch = new AdvancedSearch();

                    XDocument _XDocument = XDocument.Parse(_IQAgentSearchRequest.SearchTerm);

                    _Request.ListOfPMGRequests = ListOfPMGRequests;
                    _Request.AdvanceSearchParams = _AdvancedSearch;

                    _ListOfRequest.Add(_Request);
                    
                    var _SearchParams = (from _SearchParam in _XDocument.Elements()
                                        select new
                                        {
                                            _SearchTerm = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_SearchTerm) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_SearchTerm).Value : string.Empty,
                                            _ProgramTitle = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_ProgramTitle) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_ProgramTitle).Value : string.Empty,
                                            _Appearing = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_Appearing) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_Appearing).Value : string.Empty,
                                            //_IQ_Dma_Num = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Dma_Num) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Dma_Num).Value : string.Empty,
                                            _IQ_Dma_Name = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Dma_Name) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Dma_Name).Value : string.Empty,
                                            _IQ_Class_Num = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat_Num) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat_Num).Value : string.Empty,
                                            //_Station_Affil_Num = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_Station_Affil_Num) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_Station_Affil_Num).Value : string.Empty
                                            _Station_Affil = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_Station_Affil) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_Station_Affil).Value : string.Empty
                                            
                                        }).SingleOrDefault();

                    _SearchRequest.Title120 = string.IsNullOrEmpty(_SearchParams._ProgramTitle) ? string.Empty : "\"" + _SearchParams._ProgramTitle + "\"";
                    _SearchRequest.IQDmaName = string.IsNullOrEmpty(_SearchParams._IQ_Dma_Name) || _SearchParams._IQ_Dma_Name == "all" ? new List<string>() : _SearchParams._IQ_Dma_Name.Replace("'", string.Empty).Split(',').ToList();
                    _SearchRequest.IQClassNum = string.IsNullOrEmpty(_SearchParams._IQ_Class_Num) || _SearchParams._IQ_Class_Num == "all" ? new List<string>() : _SearchParams._IQ_Class_Num.Split(',').ToList();
                    //_SearchRequest.Station_Affil_Num = _SearchParams._Station_Affil_Num == "all" ? string.Empty : _SearchParams._Station_Affil_Num;


                    _SearchRequest.StationAffil = string.IsNullOrEmpty(_SearchParams._Station_Affil) || _SearchParams._Station_Affil == "all" ? new List<string>() : _SearchParams._Station_Affil.Replace("'", string.Empty).Split(',').ToList();
                    _SearchRequest.Appearing = _SearchParams._Appearing;
                    _SearchRequest.Terms = _SearchParams._SearchTerm;

                    _AdvancedSearch.Query = _XDocument.ToString();
                    _AdvancedSearch.QueryName = _IQAgentSearchRequest.Query_Name;
                    _AdvancedSearch.QueryVersion = _IQAgentSearchRequest.Query_Version.ToString();
                    _AdvancedSearch.ClientID = _IQAgentSearchRequest.ClientGuid.ToString();
                    SendRequestToPMGNInsertResults(ListOfPMGRequests, _SearchRequest, _SearchParametersIQAgent, _IQAgentSearchRequest, ref _NoOfInsertedRecords, _DateTimeAppend);
                }

                if (_SearchParametersIQAgent.IsPMGSearchLog)
                {
                    _IQAgentLog.StoredProcedure.Add(CommonConstants.usp_PMGSearchLog_Insert);
                    _IQAgentLog.Tables.Add(CommonConstants.Table_PMGSearchLog);
                }
                _IQAgentLog.StoredProcedure.Add(CommonConstants.usp_IQAgentResult_InsertList);
                _IQAgentLog.Tables.Add(CommonConstants.Table_IQAgentResult);

                _IQAgentLog.ProgramEndTime = DateTime.Now.ToString();

                if (CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) != null && CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) == true)
                {
                    GenerateDebugFile(_IQAgentLog, _SearchParametersIQAgent, _DateTimeAppend, null);
                }

                InsertIQServiceLog("Stop Event", p_FilePath);
            }
            catch (Exception _Exception)
            {
                try
                {
                    string _ReturnValue = string.Empty;
                    IIQMediaGroupExceptionsController _IIQMediaGroupExceptionsController = _ControllerFactory.CreateObject<IIQMediaGroupExceptionsController>();
                    _ReturnValue = _IIQMediaGroupExceptionsController.AddIQMediaGroupException(_Exception, "IQAgentFetch_Console");

                    throw _Exception;
                }
                catch (Exception _InnerException)
                {
                    if (CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) != null && CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) == true)
                    {
                        _IQAgentLog.ProgramEndTime = DateTime.Now.ToString();
                        GenerateDebugFile(_IQAgentLog, _SearchParametersIQAgent, _DateTimeAppend, null);
                    }

                    throw new Exception(_Exception.Message + "  " + CommonConstants.Ampersand + "  " + _InnerException.Message);
                }
            }

            return _NoOfInsertedRecords;
        }

        //public int SearchRawMediaFromPMGSearch_old(string p_FilePath)
        //{
        //    bool _IsDefaultDate = false;
        //    bool _IsDefiniteQuery = false;
        //    int _NoOfInsertedRecords = 0;
        //    string _SearchTerm = string.Empty; 

        //    SearchParametersIQAgent _SearchParametersIQAgent = ReadParamsFromFile(p_FilePath, ref _IsDefaultDate, ref _IsDefiniteQuery);

        //    CommonFunctions.BuildConnectionStringFromUserConfig(_SearchParametersIQAgent.ConnectionString);
        //    string _DateTimeAppend = DateTime.Now.ToString();
        //    _DateTimeAppend = _DateTimeAppend.Replace(" ", "_").Replace(":", "_").Replace("/", "_");

        //    IQAgentLog _IQAgentLog = new IQAgentLog();
        //    _IQAgentLog.StoredProcedure = new List<string>();
        //    _IQAgentLog.Tables = new List<string>();
        //    _IQAgentLog.ProgramStartTime = DateTime.Now.ToString();
        //    _IQAgentLog.ConfigFileParams = _SearchParametersIQAgent;

        //    List<Requests> _ListOfRequest = new List<Requests>();

        //    try
        //    {

        //        bool _IsDefaultSettings = false;

        //        InsertIQServiceLog("Start Event", p_FilePath);

        //        List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = null;
        //        _IQAgentLog.ListOfRequests = _ListOfRequest;

        //        if (_IsDefiniteQuery == false)
        //        {
        //            _ListOfIQAgentSearchRequest = GetSearchRequestAll();
        //            _IQAgentLog.StoredProcedure.Add(CommonConstants.usp_IQAgentSearchRequest_Select);
        //            _IQAgentLog.Tables.Add(CommonConstants.Table_IQAgentSearchRequest);
        //        }
        //        else
        //        {
        //            _ListOfIQAgentSearchRequest = GetSearchRequestsByClientIDQueryNameVersion(Convert.ToInt64(_SearchParametersIQAgent.ClientID), _SearchParametersIQAgent.QueryName, Convert.ToInt32(_SearchParametersIQAgent.QueryVersion));
        //            _IQAgentLog.StoredProcedure.Add(CommonConstants.usp_IQAgentSearchRequest_SelectByClientQueryVersion);
        //            _IQAgentLog.Tables.Add(CommonConstants.Table_IQAgentSearchRequest);
        //        }

        //        StatSkedProg _StatSkedProg = new StatSkedProg();

        //        _StatSkedProg.MinDate = Convert.ToDateTime(_SearchParametersIQAgent.StartDate);
        //        _StatSkedProg.MaxDate = Convert.ToDateTime(_SearchParametersIQAgent.EndDate);

        //        string strSTATSKEDSPName = string.Empty;
        //        string strSTATSKEDTableName = string.Empty;

        //        foreach (IQAgentSearchRequest _IQAgentSearchRequest in _ListOfIQAgentSearchRequest)
        //        {
        //            Requests _Request = new Requests();
        //            List<IQ_Agent_FetchLog> ListOfPMGRequests = new List<IQ_Agent_FetchLog>();
        //            List<Exception> ListOfExceptions = new List<Exception>();
        //            AdvancedSearch _AdvancedSearch = new AdvancedSearch();

        //            XDocument _XDocument = XDocument.Parse(_IQAgentSearchRequest.SearchTerm);

        //            var _SearchDefaultSettings = from _SearchParam in _XDocument.Elements()
        //                                         //Descendants("IQAgentRequest")
        //                                         select new
        //                                         {
        //                                             _IsDefaultSettings = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IsDefaultSettings).Value,
        //                                             _SearchTerm = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_SearchTerm).Value

        //                                         };

        //            foreach (var _SearchDefaultSetting in _SearchDefaultSettings)
        //            {
        //                _IsDefaultSettings = Convert.ToBoolean(_SearchDefaultSetting._IsDefaultSettings);
        //                _SearchTerm = _SearchDefaultSetting._SearchTerm;
        //            }


        //            _Request.ListOfPMGRequests = ListOfPMGRequests;
        //            _Request.AdvanceSearchParams = _AdvancedSearch;

        //            _ListOfRequest.Add(_Request);


        //            if (_IsDefaultSettings == false)
        //            {
        //                var _SearchParams = from _SearchParam in _XDocument.Elements()
        //                                    select new
        //                                    {
        //                                        _ProgramTitle = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_ProgramTitle) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_ProgramTitle).Value : string.Empty,
        //                                        //_ProgramDescription = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_ProgramDescription) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_ProgramDescription).Value : string.Empty,
        //                                        _IQ_Dma_Num = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Dma_Num) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Dma_Num).Value : string.Empty,
        //                                        _IQ_Cat = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Cat) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Cat).Value : string.Empty,
        //                                        _IQ_Class = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Sub_Cat).Value : string.Empty,
        //                                        _Station_Affil = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_Station_Affil) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_Station_Affil).Value : string.Empty,
        //                                        _IQ_Dma_Name = _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Dma_Name) != null ? _SearchParam.Element(CommonConstants.IQAgent_XMLTag_IQ_Dma_Name).Value : string.Empty

        //                                    };

        //                foreach (var _SearchParam in _SearchParams)
        //                {
        //                    _StatSkedProg.Title120 = _SearchParam._ProgramTitle;
        //                   // _StatSkedProg.Desc100 = _SearchParam._ProgramDescription;
        //                    _StatSkedProg.IQ_Dma_Num = _SearchParam._IQ_Dma_Num;
        //                    _StatSkedProg.IQ_Dma_Name = _SearchParam._IQ_Dma_Name;
        //                    _StatSkedProg.IQ_Cat = _SearchParam._IQ_Cat;
        //                    _StatSkedProg.IQ_class = _SearchParam._IQ_Class;
        //                    _StatSkedProg.Station_Affil = _SearchParam._Station_Affil;
        //                }

        //                // For Log File
        //                _AdvancedSearch.ProgramTitle = _StatSkedProg.Title120;
        //                _AdvancedSearch.DMARank_Name = _StatSkedProg.IQ_Dma_Name;
        //                //_AdvancedSearch.ProgramCategory = _StatSkedProg.IQ_Cat;
        //                _AdvancedSearch.ProgramCategory = _StatSkedProg.IQ_class;
        //                _AdvancedSearch.AffiliateNetwork = _StatSkedProg.Station_Affil;

        //                /*IRL_GUIDSController _IRL_GUIDSController = _ControllerFactory.CreateObject<IRL_GUIDSController>();

        //                List<RL_GUIDS> _ListOfRL_GUIDs = _IRL_GUIDSController.GetAllRL_GUIDSByStatskedprogParams(_StatSkedProg);*/

        //                IStatSkedProgController _IStatSkedProgController = _ControllerFactory.CreateObject<IStatSkedProgController>();
        //                List<StatSkedProg> _ListOfStatSkedProgKey = _IStatSkedProgController.GetAllIQCCKeyByStatskedprogParams(_StatSkedProg);


        //                int _MaxIQ_CC_KeyPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ListOfStatSkedProgKey.Count) / Convert.ToDouble(_SearchParametersIQAgent.NumIQ_CC_KeyPerSearch))));
        //                StringBuilder _IQ_CC_KeyStringBuilder = new StringBuilder();

        //                strSTATSKEDSPName = CommonConstants.usp_STATSKEDPROG_SelectBySTATSKEDPROGParams;
        //                strSTATSKEDTableName = CommonConstants.Table_STATSKEDPROG;

        //                for (int _Index = 0; _Index < _MaxIQ_CC_KeyPages; _Index++)
        //                {
        //                    _IQ_CC_KeyStringBuilder.Remove(0, _IQ_CC_KeyStringBuilder.Length);
        //                    _IQ_CC_KeyStringBuilder.Append(GenerateIQ_CC_KeysString(_Index, _ListOfStatSkedProgKey, Convert.ToInt32(_SearchParametersIQAgent.NumIQ_CC_KeyPerSearch)));

        //                    SearchRequest _SearchRequest = new SearchRequest();

        //                    _SearchRequest.IQCCKeyList = _IQ_CC_KeyStringBuilder.ToString();
        //                    _SearchRequest.Terms = _SearchTerm;                            

        //                    SendRequestToPMGNInsertResults(ListOfPMGRequests, _SearchRequest, _SearchParametersIQAgent, _IQAgentSearchRequest, ref _NoOfInsertedRecords, _DateTimeAppend);                            
        //                }
        //            }
        //            else
        //            {
        //                SearchRequest _SearchRequest = new SearchRequest();

        //                _SearchRequest.StartDate = Convert.ToDateTime(_SearchParametersIQAgent.StartDate);
        //                _SearchRequest.EndDate = Convert.ToDateTime(_SearchParametersIQAgent.EndDate);
        //                _SearchRequest.Terms = _SearchTerm;                        

        //                _AdvancedSearch.QueryName = _SearchParametersIQAgent.QueryName;
        //                _AdvancedSearch.QueryVersion = _SearchParametersIQAgent.QueryVersion;
        //                _AdvancedSearch.ClientID = _SearchParametersIQAgent.ClientID;

        //                SendRequestToPMGNInsertResults(ListOfPMGRequests, _SearchRequest, _SearchParametersIQAgent, _IQAgentSearchRequest, ref _NoOfInsertedRecords, _DateTimeAppend);
        //            }
        //        }
                
        //        _IQAgentLog.StoredProcedure.Add(CommonConstants.usp_PMGSearchLog_Insert);
        //        _IQAgentLog.Tables.Add(CommonConstants.Table_PMGSearchLog);
        //        _IQAgentLog.StoredProcedure.Add(CommonConstants.usp_IQAgentResult_InsertList);
        //        _IQAgentLog.Tables.Add(CommonConstants.Table_IQAgentResult);               

        //        if (!string.IsNullOrEmpty(strSTATSKEDSPName))
        //        {
        //            _IQAgentLog.StoredProcedure.Add(strSTATSKEDSPName);
        //        }
        //        if (!string.IsNullOrEmpty(strSTATSKEDTableName))
        //        {
        //            _IQAgentLog.Tables.Add(strSTATSKEDTableName);
        //        }

        //        _IQAgentLog.ProgramEndTime = DateTime.Now.ToString();

        //        if (CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) != null && CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) == true)
        //        {
        //            GenerateDebugFile(_IQAgentLog, _SearchParametersIQAgent, _DateTimeAppend, null);
        //        }

        //        InsertIQServiceLog("Stop Event", p_FilePath);
        //    }
        //    catch (Exception _Exception)
        //    {
        //        try
        //        {
        //            string _ReturnValue = string.Empty;
        //            IIQMediaGroupExceptionsController _IIQMediaGroupExceptionsController = _ControllerFactory.CreateObject<IIQMediaGroupExceptionsController>();
        //            _ReturnValue = _IIQMediaGroupExceptionsController.AddIQMediaGroupException(_Exception, "IQAgentFetch_Console");

        //            throw _Exception;
        //        }
        //        catch (Exception _InnerException)
        //        {
        //            if (CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) != null && CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) == true)
        //            {
        //                _IQAgentLog.ProgramEndTime = DateTime.Now.ToString();
        //                GenerateDebugFile(_IQAgentLog, _SearchParametersIQAgent, _DateTimeAppend, null);
        //            }

        //            throw new Exception(_Exception.Message + "  " + CommonConstants.Ampersand + "  " + _InnerException.Message);
        //        }
        //    }

        //    return _NoOfInsertedRecords;
        //}

        private void SendRequestToPMGNInsertResults(List<IQ_Agent_FetchLog> _ListOfIQAgentFetchLog, SearchRequest _SearchRequest, SearchParametersIQAgent p_SearchParametersIQAgent, IQAgentSearchRequest p_IQAgentSearchRequest, ref int p_NoOfInsertedRecords, string _DateTimeAppend)
        {
            try
            {
                ControllerFactory _ControllerFactory = new ControllerFactory();
                IIQAgentResultsController _IIQAgentResultsController = _ControllerFactory.CreateObject<IIQAgentResultsController>();

                SearchEngine _SearchEngine = new SearchEngine(new Uri(p_SearchParametersIQAgent.PMGSearchUrl));

                int _PMGMaxHighlights = 20;

                if (ConfigurationManager.AppSettings[CommonConstants.ConfigPMGMaxHighlights] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings[CommonConstants.ConfigPMGMaxHighlights], out _PMGMaxHighlights);
                }

                Boolean IsPmgLogging = true;

                if (ConfigurationManager.AppSettings["IsPMGLogging"] != null)
                {
                    Boolean.TryParse(ConfigurationManager.AppSettings["IsPMGLogging"], out IsPmgLogging);
                }

                string PmgLogFileLocation = ConfigurationManager.AppSettings["PMGLogFileLocation"];

                int _PageNumber = 0;
                int _MaxPage = 0;

                List<IQAgentResults> _ListOfIQAgentResults = new List<IQAgentResults>();

                SearchResult _SearchResult = null;

                do
                {
                    _ListOfIQAgentResults.Clear();

                    IQ_Agent_FetchLog _IQ_Agent_FetchLog_LogFile = new IQ_Agent_FetchLog();

                    _IQ_Agent_FetchLog_LogFile.StartTime = DateTime.Now.ToString();

                    _SearchRequest.PageNumber = _PageNumber;
                    _SearchRequest.PageSize = Convert.ToInt32(p_SearchParametersIQAgent.NumResultsPerPage);
                    _SearchRequest.MaxHighlights = _PMGMaxHighlights;

                    _SearchResult = _SearchEngine.Search(_SearchRequest);

                    _IQ_Agent_FetchLog_LogFile.EndTime = DateTime.Now.ToString();

                    string _ResponseXml = string.Empty;
                    string _Response = string.Empty;

                    XmlDocument _XmlLogDocument = new XmlDocument();
                    _XmlLogDocument.LoadXml(_SearchResult.ResponseXml);

                    XmlNodeList _XmlNodeList = _XmlLogDocument.GetElementsByTagName("response");

                    if (_XmlNodeList.Count > 0)
                    {
                        XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                        foreach (XmlAttribute item in _XmlAttributeCollection)
                        {
                            if (item.Name == "status")
                            {
                                _Response = _XmlLogDocument.InnerXml;
                                _Response = _Response.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                            }
                        }
                    }
                    else
                    {
                        _Response = null;
                    }


                    if (_SearchResult.ResponseXml != null)
                    {
                        _ResponseXml = _SearchResult.ResponseXml;
                    }

                    _IQ_Agent_FetchLog_LogFile.PMGRequest = SearchLog(_SearchRequest, null);
                    _IQ_Agent_FetchLog_LogFile.PMGResponse = _ResponseXml;

                    if (p_SearchParametersIQAgent.IsPMGSearchLog == true)
                    {
                        SearchLog(_SearchRequest, null, _Response);
                    }
                    foreach (Hit _Hit in _SearchResult.Hits)
                    {
                        IQAgentResults _IQAgentResults = new IQAgentResults();

                        _IQAgentResults.RL_VideoGUID = new Guid(_Hit.Guid);
                        _IQAgentResults.RL_Date = _Hit.Timestamp;
                        _IQAgentResults.RL_Time = _Hit.Hour;
                        _IQAgentResults.Rl_Station = _Hit.StationId;
                        _IQAgentResults.RL_Market = _Hit.Market;
                        _IQAgentResults.Title120 = _Hit.Title120;
                        _IQAgentResults.iq_cc_key = _Hit.Iqcckey;
                        _IQAgentResults.Number_Hits = _Hit.TotalNoOfOccurrence;
                        _IQAgentResults.SearchRequestID = p_IQAgentSearchRequest.ID.Value;

                        
                        _ListOfIQAgentResults.Add(_IQAgentResults);
                    }

                    _MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_SearchResult.TotalHitCount) / Convert.ToDouble(p_SearchParametersIQAgent.NumResultsPerPage))));

                    _PageNumber = _PageNumber + 1;
                    _ListOfIQAgentFetchLog.Add(_IQ_Agent_FetchLog_LogFile);

                    XDocument _XDocumentIQAgentResults = GenerateIQAgentResultsListToXML(_ListOfIQAgentResults);
                    XmlReader _XmlReader = _XDocumentIQAgentResults.CreateReader();

                    try
                    {
                        if (_ListOfIQAgentResults.Count > 0)
                        {
                            string _ReturnValue = _IIQAgentResultsController.InsertIQAgentResultsList(new SqlXml(_XmlReader));
                            p_NoOfInsertedRecords = p_NoOfInsertedRecords + (CommonFunctions.GetIntValue(_ReturnValue) != null && Convert.ToInt32(_ReturnValue) > 0 ? Convert.ToInt32(_ReturnValue) : 0);
                        }
                    }
                    catch (Exception _Exception)
                    {
                        _IQ_Agent_FetchLog_LogFile.Exception = _Exception.StackTrace;
                    }

                } while (_PageNumber < _MaxPage);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        //private string GenerateIQ_CC_KeysString(int p_PageNumber, List<StatSkedProg> p_ListOfStatSkedProg, int p_NoOfGUID)
        //{
        //    try
        //    {
        //        string _IQ_CC_Keys = string.Empty;

        //        if (p_ListOfStatSkedProg != null)
        //        {
        //            int _Condition = p_ListOfStatSkedProg.Count < ((p_PageNumber * p_NoOfGUID) + p_NoOfGUID) ? p_ListOfStatSkedProg.Count : ((p_PageNumber * p_NoOfGUID) + p_NoOfGUID);

        //            for (int _Index = (p_PageNumber * p_NoOfGUID); _Index < _Condition; _Index++)
        //            {
        //                if (string.IsNullOrEmpty(_IQ_CC_Keys))
        //                {
        //                    _IQ_CC_Keys = p_ListOfStatSkedProg[_Index].IQ_CC_Key.ToString();
        //                }
        //                else
        //                {
        //                    _IQ_CC_Keys = _IQ_CC_Keys + CommonConstants.Comma + p_ListOfStatSkedProg[_Index].IQ_CC_Key.ToString();
        //                }
        //            }
        //        }

        //        return _IQ_CC_Keys;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        private void GenerateDebugFile(IQAgentLog _IQAgentLog, SearchParametersIQAgent p_SearchParametersIQAgent, string p_DateTime, Exception p_Exception)
        {
            try
            {
                string _XMLString = CommonFunctions.MakeSerialization(_IQAgentLog);

                XmlDocument _XmlRequestDocument = new XmlDocument();
                _XmlRequestDocument.LoadXml(_XMLString);

                string _FolderPathTime = p_SearchParametersIQAgent.DebugFilePath;

                #region Create Directory to store XML file

                DirectoryInfo _DirectoryInfo;

                if (!Directory.Exists(_FolderPathTime))
                {
                    throw new MyException("Debug File Path does not exist.");
                }
                else
                {
                    _DirectoryInfo = new DirectoryInfo(_FolderPathTime);
                }

                #endregion

                if (!string.IsNullOrEmpty(p_SearchParametersIQAgent.DebugFileFlag) && CommonFunctions.GetBoolValue(p_SearchParametersIQAgent.DebugFileFlag) != null && Convert.ToBoolean(p_SearchParametersIQAgent.DebugFileFlag) == true)
                {

                    _FolderPathTime = _FolderPathTime + CommonConstants.ForwardSlash + _DirectoryInfo.Name;

                    string _FileURL = string.Empty;

                    if (p_Exception == null)
                    {
                        _FileURL = _DirectoryInfo.FullName + CommonConstants.ForwardSlash + "IQ_Agent_FetchLog" + CommonConstants.UnderScore + p_DateTime + CommonConstants.Dot + CommonConstants.XmlText;
                    }
                    else
                    {
                        _FileURL = _DirectoryInfo.FullName + CommonConstants.ForwardSlash + "Exception_IQ_Agent_FetchLog" + CommonConstants.UnderScore + p_DateTime + CommonConstants.Dot + CommonConstants.XmlText;
                    }

                    FileInfo _FileInfo = new FileInfo(_FileURL);
                    string _ExistingInfoTime = string.Empty;

                    _XmlRequestDocument.Save(_FileURL);
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void SearchLog(SearchRequest _SearchRequest, long? _CustomerID, string _Response)
        {
            try
            {

                StringBuilder _FileContent = new StringBuilder();
                _FileContent.AppendFormat("<PMGRequest><Terms>{0}</Terms><PageNumber>{1}</PageNumber><PageSize>{2}</PageSize><MaxHighlights>{3}</MaxHighlights>", _SearchRequest.Terms, _SearchRequest.PageNumber, _SearchRequest.PageSize, _SearchRequest.MaxHighlights);
                if (_SearchRequest.StartDate.HasValue)
                {
                    _FileContent.AppendFormat("<StartDate>{0}</StartDate>", _SearchRequest.StartDate);
                }
                else
                {
                    _FileContent.Append("<StartDate></StartDate>");
                }

                if (!string.IsNullOrEmpty(_SearchRequest.TimeZone))
                {
                    _FileContent.AppendFormat("<IQ_Time_Zone>{0}</IQ_Time_Zone>", _SearchRequest.TimeZone);
                }
                else
                {
                    _FileContent.Append("<IQ_Time_Zone />");
                }

                if (_SearchRequest.IQDmaName.Count() > 0)
                {
                    _FileContent.AppendFormat("<IQ_Dma_Name>{0}</IQ_Dma_Name>", string.Join(",", _SearchRequest.IQDmaName));
                }
                else
                {
                    _FileContent.Append("<IQ_Dma_Num />");
                }

                if (_SearchRequest.IQClassNum.Count() > 0)
                {
                    _FileContent.AppendFormat("<IQ_Class_Num>{0}</IQ_Class_Num>", string.Join(",", _SearchRequest.IQClassNum));
                }
                else
                {
                    _FileContent.Append("<IQ_Class_Num />");
                }

                if (_SearchRequest.StationAffil.Count() > 0)
                {
                    _FileContent.AppendFormat("<Station_Affil>{0}</Station_Affil>", string.Join(",", _SearchRequest.StationAffil));
                }
                else
                {
                    _FileContent.Append("<Station_Affil />");
                }

                //if (!string.IsNullOrEmpty(_SearchRequest.Station_Affil_Num))
                //{
                //    _FileContent += "<Station_Affil_Num>" + _SearchRequest.Station_Affil_Num + "</Station_Affil_Num>";
                //}
                //else
                //{
                //    _FileContent += "<Station_Affil_Num />";
                //}
                if (!string.IsNullOrEmpty(_SearchRequest.Title120))
                {
                    _FileContent.AppendFormat("<Title120>{0}</Title120>", _SearchRequest.Title120);
                }
                else
                {
                    _FileContent.Append("<Title120 />");
                }

                if (!string.IsNullOrEmpty(_SearchRequest.Appearing))
                {
                    _FileContent.AppendFormat("<Appearing>{0}</Appearing>", _SearchRequest.Appearing);
                }
                else
                {
                    _FileContent.Append("<Appearing />");
                }

                if (_SearchRequest.EndDate.HasValue)
                {
                    _FileContent.AppendFormat("<EndDate>{0}</EndDate>", _SearchRequest.EndDate);
                }
                else
                {
                    _FileContent.Append("<EndDate></EndDate>");
                }

                _FileContent.Append("</PMGRequest>");


                _FileContent = _FileContent.Replace("&", "&amp;");

                string _Result = string.Empty;
                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                SearchLog _SearchLog = new SearchLog();
                _SearchLog.CustomerID = Convert.ToInt32(_CustomerID);
                _SearchLog.SearchType = "IQAgent";
                _SearchLog.RequestXML = _FileContent.ToString();
                _SearchLog.ErrorResponseXML = _Response;
                _Result = _ISearchLogController.InsertSearchLog(_SearchLog);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string SearchLog(SearchRequest _SearchRequest, long? _CustomerID)
        {
            try
            {
                StringBuilder _FileContent = new StringBuilder();
                _FileContent.AppendFormat("<PMGRequest><Terms>{0}</Terms><PageNumber>{1}</PageNumber><PageSize>{2}</PageSize><MaxHighlights>{3}</MaxHighlights>", _SearchRequest.Terms,_SearchRequest.PageNumber,_SearchRequest.PageSize,_SearchRequest.MaxHighlights);
                if (_SearchRequest.StartDate.HasValue)
                {
                    _FileContent.AppendFormat("<StartDate>{0}</StartDate>", _SearchRequest.StartDate);
                }
                else
                {
                    _FileContent.Append("<StartDate></StartDate>");
                }

                if (!string.IsNullOrEmpty(_SearchRequest.TimeZone))
                {
                    _FileContent.AppendFormat("<IQ_Time_Zone>{0}</IQ_Time_Zone>", _SearchRequest.TimeZone);
                }
                else
                {
                    _FileContent.Append("<IQ_Time_Zone />");
                }

                if (_SearchRequest.IQDmaName.Count() > 0 )
                {
                    _FileContent.AppendFormat("<IQ_Dma_Name>{0}</IQ_Dma_Name>", string.Join(",", _SearchRequest.IQDmaName));
                }
                else
                {
                    _FileContent.Append("<IQ_Dma_Num />");
                }

                if (_SearchRequest.IQClassNum.Count() > 0)
                {
                    _FileContent.AppendFormat("<IQ_Class_Num>{0}</IQ_Class_Num>", string.Join(",", _SearchRequest.IQClassNum));
                }
                else
                {
                    _FileContent.Append("<IQ_Class_Num />");
                }

                if (_SearchRequest.StationAffil.Count() > 0)
                {
                    _FileContent.AppendFormat("<Station_Affil>{0}</Station_Affil>", string.Join(",", _SearchRequest.StationAffil));
                }
                else
                {
                    _FileContent.Append("<Station_Affil />");
                }

                //if (!string.IsNullOrEmpty(_SearchRequest.Station_Affil_Num))
                //{
                //    _FileContent += "<Station_Affil_Num>" + _SearchRequest.Station_Affil_Num + "</Station_Affil_Num>";
                //}
                //else
                //{
                //    _FileContent += "<Station_Affil_Num />";
                //}
                if (!string.IsNullOrEmpty(_SearchRequest.Title120))
                {
                    _FileContent.AppendFormat("<Title120>{0}</Title120>", _SearchRequest.Title120);
                }
                else
                {
                    _FileContent.Append("<Title120 />");
                }

                if (!string.IsNullOrEmpty(_SearchRequest.Appearing))
                {
                    _FileContent.AppendFormat("<Appearing>{0}</Appearing>", _SearchRequest.Appearing);
                }
                else
                {
                    _FileContent.Append("<Appearing />");
                }

                if (_SearchRequest.EndDate.HasValue)
                {
                    _FileContent.AppendFormat("<EndDate>{0}</EndDate>", _SearchRequest.EndDate);
                }
                else
                {
                    _FileContent.Append("<EndDate></EndDate>");
                }

                _FileContent = _FileContent.Replace("&", "&amp;");
                _FileContent.Append("</PMGRequest>");

                return _FileContent.ToString();

               
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private SearchParametersIQAgent ReadParamsFromFile(string p_FilePath, ref bool p_IsDefaultDate, ref bool p_IsDefiniteQuery)
        {
            try
            {
                SearchParametersIQAgent _SearchParametersIQAgent = new SearchParametersIQAgent();
                DateTime? _Out = null;

                if (!string.IsNullOrEmpty(p_FilePath))
                {
                    string _FolderPath = p_FilePath;
                    FileInfo _FileInfo = new FileInfo(_FolderPath);
                    string _ExistingInfo = string.Empty;

                    if (_FileInfo.Exists)
                    {
                        XmlDocument _XmlDocument = new XmlDocument();

                        _XmlDocument.Load(_FolderPath);
                        _ExistingInfo = _XmlDocument.InnerXml;

                        _SearchParametersIQAgent = (SearchParametersIQAgent)(CommonFunctions.MakeDeserialiazation(_ExistingInfo, _SearchParametersIQAgent));
                    }
                    else
                    {
                        throw new MyException(CommonConstants.MsgFileNotExist);
                    }
                }

                if (string.IsNullOrEmpty(_SearchParametersIQAgent.PMGSearchUrl))
                {
                    throw new MyException(CommonConstants.MsgPmgSearchUrlNotExist);
                }

                if (Convert.ToBoolean(_SearchParametersIQAgent.DebugFileFlag) == true)
                {
                    string _FolderPathTime = _SearchParametersIQAgent.DebugFilePath;

                    DirectoryInfo _DirectoryInfo;
                    if (!Directory.Exists(_FolderPathTime))
                    {
                        throw new MyException("Debug File Path does not exist.");
                    }
                    else
                    {
                        _DirectoryInfo = new DirectoryInfo(_FolderPathTime);
                    }
                }

                p_IsDefiniteQuery = true;

                if (string.IsNullOrEmpty(_SearchParametersIQAgent.ClientGuid))
                {
                    if (string.IsNullOrEmpty(_SearchParametersIQAgent.QueryName))
                    {
                        if (string.IsNullOrEmpty(_SearchParametersIQAgent.QueryVersion))
                        {
                            p_IsDefiniteQuery = false;
                        }
                        else
                        {
                            throw new MyException(CommonConstants.ParamClientID + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                        }
                    }
                    else
                    {
                        throw new MyException(CommonConstants.ParamClientID + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                    }
                }
                else
                {
                    if (new Guid(_SearchParametersIQAgent.ClientGuid) == Guid.Empty)
                    {
                        throw new MyException(CommonConstants.ParamClientID + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                    }
                    else
                    {
                        if (CommonFunctions.GetIntValue(_SearchParametersIQAgent.QueryVersion) == null)
                        {
                            throw new MyException(CommonConstants.ParamQueryVersion + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(_SearchParametersIQAgent.QueryName))
                            {
                                throw new MyException(CommonConstants.ParamClientQuery + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(_SearchParametersIQAgent.ConnectionString))
                {
                    throw new MyException(CommonConstants.ConfigconnectionString + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                }

                //if (string.IsNullOrEmpty(_SearchParametersIQAgent.NumIQ_CC_KeyPerSearch) && CommonFunctions.GetIntValue(_SearchParametersIQAgent.NumIQ_CC_KeyPerSearch) == null)
                //{
                //    throw new MyException(CommonConstants.ParamNumGUIDSPerSearch + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                //}

                if (string.IsNullOrEmpty(_SearchParametersIQAgent.NumResultsPerPage) && CommonFunctions.GetIntValue(_SearchParametersIQAgent.NumResultsPerPage) == null)
                {
                    throw new MyException(CommonConstants.ParamNumResultsPerPage + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                }

                if (string.IsNullOrEmpty(_SearchParametersIQAgent.StartDate))
                {
                    if (string.IsNullOrEmpty(_SearchParametersIQAgent.EndDate))
                    {
                        if (string.IsNullOrEmpty(_SearchParametersIQAgent.DaysDuration))
                        {
                            if(string.IsNullOrEmpty(_SearchParametersIQAgent.HoursDuration))
                            {
                                p_IsDefaultDate = true;

                                _SearchParametersIQAgent.StartDate = DateTime.Now.AddDays(-1).ToShortDateString();
                                _SearchParametersIQAgent.EndDate = DateTime.Now.ToShortDateString();
                            }
                            else
                            {
                                if (CommonFunctions.GetIntValue(_SearchParametersIQAgent.HoursDuration) != null)
                                {
                                    Double hour = Convert.ToDouble(_SearchParametersIQAgent.HoursDuration);
                                    if (hour > 0)
                                    {
                                        _SearchParametersIQAgent.StartDate = DateTime.Now.AddHours(-hour).ToString();
                                        _SearchParametersIQAgent.EndDate = DateTime.Now.ToString();
                                    }
                                    else
                                    {
                                        throw new MyException(CommonConstants.ParamsIncorrectFormat);
                                    }
                                }
                                else
                                {
                                    throw new MyException(CommonConstants.ParamsIncorrectFormat);
                                }
                            }
                        }
                        else
                        {
                            if (CommonFunctions.GetIntValue(_SearchParametersIQAgent.DaysDuration) != null)
                            {
                                _SearchParametersIQAgent.StartDate = DateTime.Now.AddDays((-1) * (Convert.ToInt32(_SearchParametersIQAgent.DaysDuration))).ToShortDateString();
                                _SearchParametersIQAgent.EndDate = DateTime.Now.ToShortDateString();
                            }
                            else
                            {
                                throw new MyException(CommonConstants.ParamDaysDuration + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                            }
                        }
                    }
                    else
                    {
                        throw new MyException(CommonConstants.ParamStartDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                    }
                }
                else
                {
                    _Out = CommonFunctions.GetDateTimeValue(_SearchParametersIQAgent.StartDate);

                    if (_Out == null)
                    {
                        throw new MyException(CommonConstants.ParamStartDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                    }

                    if (string.IsNullOrEmpty(_SearchParametersIQAgent.EndDate))
                    {
                        throw new MyException(CommonConstants.ParamEndDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                    }
                    else
                    {
                        _Out = CommonFunctions.GetDateTimeValue(_SearchParametersIQAgent.EndDate);

                        if (_Out == null)
                        {
                            throw new MyException(CommonConstants.ParamEndDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                        }
                    }
                }

                if (Convert.ToDateTime(_SearchParametersIQAgent.StartDate) > DateTime.Now || Convert.ToDateTime(_SearchParametersIQAgent.EndDate) > DateTime.Now)
                {
                    throw new MyException(CommonConstants.MsgDateGTCurrentDate);
                }

                if (!string.IsNullOrEmpty(_SearchParametersIQAgent.StartDate) && !string.IsNullOrEmpty(_SearchParametersIQAgent.EndDate))
                {
                    DateTime? _StartDate = null;
                    DateTime? _EndDate = null;

                    if (_StartDate > _EndDate)
                    {
                        throw new MyException(CommonConstants.MsgStartEndDate);
                    }
                }

                if (!string.IsNullOrEmpty(_SearchParametersIQAgent.DebugFileFlag) && CommonFunctions.GetBoolValue(_SearchParametersIQAgent.DebugFileFlag) != null)
                {
                    if (Convert.ToBoolean(_SearchParametersIQAgent.DebugFileFlag) == true)
                    {
                        if (string.IsNullOrEmpty(_SearchParametersIQAgent.DebugFilePath))
                        {
                            throw new MyException(CommonConstants.ParamDebugFilePath + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                        }
                        else
                        {
                            if (!Directory.Exists(_SearchParametersIQAgent.DebugFilePath))
                            {
                                throw new MyException("Debug File Path does not exist.");
                            }
                        }
                    }
                }

                return _SearchParametersIQAgent;
            }
            catch (MyException _MyException)
            {
                throw _MyException;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        //private string GenerateDateTime(DateTime p_Date, string p_StationTimeZone, bool p_IsDefaultDate)
        //{
        //    try
        //    {
        //        string _DateTime = string.Empty;

        //        double _CSTValue = -1;
        //        double _MSTValue = -2;
        //        double _PSTValue = -3;


        //        if (p_StationTimeZone.ToLower() == StationTimeZone.CST.ToString().ToLower())
        //        {
        //            _DateTime = p_Date.AddHours(_CSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_CSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
        //            _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;
        //        }
        //        else if (p_StationTimeZone.ToLower() == StationTimeZone.MST.ToString().ToLower())
        //        {
        //            _DateTime = p_Date.AddHours(_MSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_MSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
        //            _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;
        //        }
        //        else if (p_StationTimeZone.ToLower() == StationTimeZone.PST.ToString().ToLower())
        //        {
        //            _DateTime = p_Date.AddHours(_PSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_PSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
        //            _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;
        //        }
        //        else
        //        {
        //            _DateTime = p_Date.ToShortDateString() + CommonConstants.Space + p_Date.Hour.ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
        //            _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;

        //        }

        //        return _DateTime;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        //public string UpdateIsActive(long p_SearchRequestKey, bool p_IsActive)
        //{
        //    try
        //    {
        //        string _Result = string.Empty;
        //        _Result = _ISearchRequestModel.UpdateIsActive(p_SearchRequestKey, p_IsActive);

        //        return _Result;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        ///// <summary>
        ///// Description: This Methods gets Search request Information from DataSet By ClientID.
        ///// </summary>
        ///// <returns>List of Object of Search Request.This will also include InActive records</returns> 
        //public List<IQAgentSearchRequest> SelectAllByClientID(long ClientID)
        //{
        //    DataSet _DataSet = null;
        //    List<IQAgentSearchRequest> _ListOfSearchRequest = null;
        //    try
        //    {
        //        _DataSet = _ISearchRequestModel.SelectAllByClientID(ClientID);
        //        _ListOfSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //    return _ListOfSearchRequest;
        //}


        /// <summary>
        /// Description: This Methods gets Search request Information from DataSet By ClientID & Query Name.
        /// </summary>
        /// <returns>List of Object of Search Request</returns>
        public List<IQAgentSearchRequest> GetSearchRequestsByClientIDQueryName(Guid p_ClientGuid, string p_QueryName)
        {
            DataSet _DataSet = null;
            List<IQAgentSearchRequest> _ListOfSearchRequest = null;
            try
            {
                _DataSet = _ISearchRequestModel.GetSearchRequestsByClientIDQueryName(p_ClientGuid, p_QueryName);
                _ListOfSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSearchRequest;
        }

        public List<IQAgentSearchRequest> GetSearchRequestsByClientIDQueryNameVersion(Guid p_ClientGuid, string p_QueryName, int p_QueryVersion)
        {
            try
            {
                List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = null;

                DataSet _DataSet = _ISearchRequestModel.GetSearchRequestsByClientIDQueryNameVersion(p_ClientGuid, p_QueryName, p_QueryVersion);

                _ListOfIQAgentSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);

                return _ListOfIQAgentSearchRequest;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        ///// <summary>
        ///// This method gets SearchRequest using parameters
        ///// </summary>
        ///// <param name="p_ClientID">ClientID</param>
        ///// <param name="p_QueryName">Query Name</param>
        ///// <param name="p_QueryVersion">Query Version</param>
        ///// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        ///// <returns>List of objects of IQAgentSearchRequest Class</returns>
        //public List<IQAgentSearchRequest> GetSearchRequestsByClientIDQueryNameVersion(long p_ClientID, string p_QueryName, int p_QueryVersion, ConnectionStringKeys p_ConnectionStringKeys)
        //{
        //    try
        //    {
        //        List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = null;

        //        DataSet _DataSet = _ISearchRequestModel.GetSearchRequestsByClientIDQueryNameVersion(p_ClientID, p_QueryName, p_QueryVersion, p_ConnectionStringKeys);

        //        _ListOfIQAgentSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);

        //        return _ListOfIQAgentSearchRequest;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        
        private XDocument GenerateListToXML(List<TermOccurrence> _ListOfTermOccurrence)
        {
            XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                      new XElement("CaptionList",
                       from _TermOccurrence in _ListOfTermOccurrence
                       select new XElement("Caption",
                           string.IsNullOrEmpty(Convert.ToString(_TermOccurrence.SearchTerm)) ? null :
                       new XAttribute("SearchTerm", _TermOccurrence.SearchTerm),
                       string.IsNullOrEmpty(Convert.ToString(_TermOccurrence.SurroundingText)) ? null :
                       new XAttribute("SurroundingText", _TermOccurrence.SurroundingText),
                       string.IsNullOrEmpty(Convert.ToString(_TermOccurrence.TimeOffset)) ? null :
                       new XAttribute("TimeOffset", _TermOccurrence.TimeOffset)
                           )));
            return xmlDocument;
        }

        private XDocument GenerateIQAgentResultsListToXML(List<IQAgentResults> _ListOfIQAgentResults)
        {
            XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                      new XElement("IQAgentResultsList",
                       from _IQAgentResults in _ListOfIQAgentResults
                       select new XElement("IQAgentResult",
                           string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.SearchRequestID)) ? null :
                       new XAttribute("SearchRequestID", _IQAgentResults.SearchRequestID),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.SearchTerm)) ? null :
                       new XAttribute("SearchTerm", _IQAgentResults.SearchTerm),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.RL_VideoGUID)) ? null :
                       new XAttribute("RL_VideoGUID", _IQAgentResults.RL_VideoGUID),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.RL_Date)) ? null :
                       new XAttribute("RL_Date", _IQAgentResults.RL_Date),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.RL_Time)) ? null :
                       new XAttribute("RL_Time", _IQAgentResults.RL_Time),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.Number_Hits)) ? null :
                       new XAttribute("Number_Hits", _IQAgentResults.Number_Hits),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.Rl_Station)) ? null :
                       new XAttribute("Rl_Station", _IQAgentResults.Rl_Station),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.RL_Market)) ? null :
                       new XAttribute("Rl_Market", _IQAgentResults.RL_Market),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.Title120)) ? null :
                       new XAttribute("Title120", _IQAgentResults.Title120),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.iq_cc_key)) ? null :
                       new XAttribute("iq_cc_key", _IQAgentResults.iq_cc_key)

                           )));
            return xmlDocument;
        }

        private void InsertIQServiceLog(string _Event, string _FilePath)
        {
            try
            {
                ControllerFactory _ControllerFactory = new ControllerFactory();
                string _string = string.Empty;

                IIq_Service_logController _IIq_Service_logController = _ControllerFactory.CreateObject<IIq_Service_logController>();
                Iq_Service_log _Iq_Service_log = new Iq_Service_log();
                _Iq_Service_log.ModuleName = "IQ_Agent_fetch";
                _Iq_Service_log.CreatedDatetime = DateTime.Now;
                if (_Event == "Start Event")
                {
                    _Iq_Service_log.ServiceCode = "Start Event";
                }
                else
                {
                    _Iq_Service_log.ServiceCode = "Stop Event";
                }
                _Iq_Service_log.ConfigRequest = InsertIq_Service_log(_FilePath);

                _string = _IIq_Service_logController.AddRL_GUIDS(_Iq_Service_log);

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string InsertIq_Service_log(string _FilePath)
        {
            try
            {                
                string _RequestPath = _FilePath;
                string _FileContent = string.Empty;
                if (!string.IsNullOrEmpty(_RequestPath))
                {                   
                    FileInfo _FileInfo = new FileInfo(_RequestPath);

                    if (_FileInfo.Exists)
                    {
                        XmlDocument _XmlDocument = new XmlDocument();
                        _XmlDocument.Load(_RequestPath);
                        //_ExistingInfo = _XmlDocument.InnerXml;
                        _FileContent = "<ServiceLog>" +
                                        "<filePath>" + _RequestPath + "</filePath>" +
                                        "<configuration>" + _XmlDocument.InnerXml + "</configuration>" +
                                        "</ServiceLog>";
                        //_SearchParameters = (SearchParameters)(CommonFunctions.MakeDeserialiazation(_ExistingInfo, _SearchParameters));

                    }

                }
                return _FileContent;

            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

        //COMMENTED BY MEGHANA ON 15-MARCH-2012 
        ///// <summary>
        ///// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        ///// </summary>
        ///// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        ///// <returns>List of object of IQAgentSearchRequest</returns>
        //public List<IQAgentSearchRequest> GetSearchRequestAll(ConnectionStringKeys p_ConnectionStringKeys)
        //{
        //    try
        //    {
        //        List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = null;

        //        DataSet _DataSet = _ISearchRequestModel.GetSearchRequestAll(p_ConnectionStringKeys);

        //        _ListOfIQAgentSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);

        //        return _ListOfIQAgentSearchRequest;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}
    }

    public class CustomException
    {
        public string Exception;
        public List<IQ_Agent_FetchLog> _ListOfIQAgentFetchLog;
    }

    public class IQAgentLog
    {
        public string ProgramStartTime;
        public string ProgramEndTime;
        public SearchParametersIQAgent ConfigFileParams;
        public List<Requests> ListOfRequests;
        public List<string> StoredProcedure;
        public List<string> Tables;
    }

    public class Requests
    {
        public AdvancedSearch AdvanceSearchParams;
        public List<IQ_Agent_FetchLog> ListOfPMGRequests;
    }
}
