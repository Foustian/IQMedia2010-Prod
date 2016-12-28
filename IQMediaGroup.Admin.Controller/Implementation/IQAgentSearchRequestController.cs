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
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Core.Enumeration;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using System.Threading;
using PMGSearch;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Admin.Controller.Implementation
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

        /// <summary>
        /// Description: This method inserts Search Request Information
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchRequests">Object Of SearchRequests Class</param>
        /// <returns>SearchRequestKey</returns>
        public string InsertSearchRequest(IQAgentSearchRequest p_IQAgentSearchRequest)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ISearchRequestModel.InsertSearchRequest(p_IQAgentSearchRequest);

                return _Result;
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
        private List<IQAgentSearchRequest> FillSearchRequestInformation(DataSet _DataSet)
        {
            List<IQAgentSearchRequest> _ListOfSearchRequest = new List<IQAgentSearchRequest>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQAgentSearchRequest _IQAgentSearchRequest = new IQAgentSearchRequest();
                        _IQAgentSearchRequest.Query_Name = Convert.ToString(_DataRow["Query_Name"]);
                        _IQAgentSearchRequest.SearchRequestKey = Convert.ToInt32(_DataRow["SearchRequestKey"]);
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
        /// Description: This Methods gets Search request Information from DataSet.
        /// Added By:Maulik gandhi
        /// </summary>
        /// <returns>List of Object of Search Request</returns>
        public List<IQAgentSearchRequest> GetSearchRequestByQueryName(IQAgentSearchRequest p_IQAgentSearchRequest)
        {
            DataSet _DataSet = null;
            List<IQAgentSearchRequest> _ListOfSearchRequest = null;
            try
            {
                _DataSet = _ISearchRequestModel.GetSearchRequestByQueryName(p_IQAgentSearchRequest);
                _ListOfSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSearchRequest;
        }

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

                        if (_DataTable.Columns.Contains("SearchRequestKey"))
                        {
                            _IQAgentSearchRequest.SearchRequestKey = CommonFunctions.GetIntValue(_DataRow["SearchRequestKey"].ToString());
                        }

                        if (_DataTable.Columns.Contains("ClientID"))
                        {
                            _IQAgentSearchRequest.ClientID = CommonFunctions.GetIntValue(_DataRow["ClientID"].ToString());
                        }

                        if (_DataTable.Columns.Contains("IQ_Agent_UserID"))
                        {
                            _IQAgentSearchRequest.IQ_Agent_UserID = CommonFunctions.GetInt64Value(_DataRow["IQ_Agent_UserID"].ToString());
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
        public List<IQAgentSearchRequest> SelectByClientID(long ClientID)
        {
            DataSet _DataSet = null;
            List<IQAgentSearchRequest> _ListOfSearchRequest = null;
            try
            {
                _DataSet = _ISearchRequestModel.SelectByClientID(ClientID);
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


        private void SearchLog(string _Terms, int _PageNumber, int _PageSize, int _MaxHighlights, DateTime? _StartDate, DateTime? _EndDate, string _Response, long? _CustomerID, ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                string _FileContent = string.Empty;
                _FileContent = "<PMGRequest>";
                _FileContent += "<Terms>" + _Terms + "</Terms>";
                _FileContent += "<PageNumber>" + _PageNumber + "</PageNumber>";
                _FileContent += "<PageSize>" + _PageSize + "</PageSize>";
                _FileContent += "<MaxHighlights>" + _MaxHighlights + "</MaxHighlights>";
                if (_StartDate.HasValue)
                {
                    _FileContent += "<StartDate>" + _StartDate + "</StartDate>";
                }
                else
                {
                    _FileContent += "<StartDate></StartDate>";
                }
                if (_EndDate.HasValue)
                {
                    _FileContent += "<EndDate>" + _EndDate + "</EndDate>";
                }
                else
                {
                    _FileContent += "<EndDate></EndDate>";
                }
                _FileContent += "</PMGRequest>";

                string _Result = string.Empty;
                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                SearchLog _SearchLog = new SearchLog();
                _SearchLog.CustomerID = Convert.ToInt32(_CustomerID);
                _SearchLog.SearchType = "IQAgent";
                _SearchLog.RequestXML = _FileContent;
                _SearchLog.ErrorResponseXML = _Response;
                _Result = _ISearchLogController.InsertSearchLog(_SearchLog, p_ConnectionStringKeys);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string SearchLog(string _Terms, int _PageNumber, int _PageSize, int _MaxHighlights, DateTime? _StartDate, DateTime? _EndDate, long? _CustomerID)
        {

            string _FileContent = string.Empty;
            _FileContent = "<PMGRequest>";
            _FileContent += "<Terms>" + _Terms + "</Terms>";
            _FileContent += "<PageNumber>" + _PageNumber + "</PageNumber>";
            _FileContent += "<PageSize>" + _PageSize + "</PageSize>";
            _FileContent += "<MaxHighlights>" + _MaxHighlights + "</MaxHighlights>";
            if (_StartDate.HasValue)
            {
                _FileContent += "<StartDate>" + _StartDate + "</StartDate>";
            }
            else
            {
                _FileContent += "<StartDate></StartDate>";
            }
            if (_EndDate.HasValue)
            {
                _FileContent += "<EndDate>" + _EndDate + "</EndDate>";
            }
            else
            {
                _FileContent += "<EndDate></EndDate>";
            }
            _FileContent += "</PMGRequest>";

            return _FileContent;
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
                        //_SearchParameters=(SearchParameters)(CommonFunctions.DeserializeObject<SearchParameters>(_ExistingInfo));
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

                if (string.IsNullOrEmpty(_SearchParametersIQAgent.ClientID))
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
                    if (CommonFunctions.GetInt64Value(_SearchParametersIQAgent.ClientID) == null)
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

                if (string.IsNullOrEmpty(_SearchParametersIQAgent.StartDate))
                {
                    if (string.IsNullOrEmpty(_SearchParametersIQAgent.EndDate))
                    {
                        p_IsDefaultDate = true;

                        _SearchParametersIQAgent.StartDate = DateTime.Now.AddDays(-1).ToString();
                        _SearchParametersIQAgent.EndDate = DateTime.Now.ToString();
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
                                /*_DirectoryInfo = new DirectoryInfo(_FolderPathTime);
                                _DirectoryInfo.Create();*/

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

        private string GenerateDateTime(DateTime p_Date, string p_StationTimeZone, bool p_IsDefaultDate)
        {
            try
            {
                string _DateTime = string.Empty;

                double _CSTValue = -1;
                double _MSTValue = -2;
                double _PSTValue = -3;

                /* if (p_IsDefaultDate == true)
                 {
                     _CSTValue = -3;
                     _MSTValue = -4;
                     _PSTValue = -5;
                 }*/

                if (p_StationTimeZone.ToLower() == StationTimeZone.CST.ToString().ToLower())
                {
                    _DateTime = p_Date.AddHours(_CSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_CSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                    _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;
                }
                else if (p_StationTimeZone.ToLower() == StationTimeZone.MST.ToString().ToLower())
                {
                    _DateTime = p_Date.AddHours(_MSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_MSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                    _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;
                }
                else if (p_StationTimeZone.ToLower() == StationTimeZone.PST.ToString().ToLower())
                {
                    _DateTime = p_Date.AddHours(_PSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_PSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                    _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;
                }
                else
                {
                    /* if (p_IsDefaultDate == true)
                     {
                         _DateTime = p_Date.AddHours(-1).ToShortDateString() + CommonConstants.Space + p_Date.AddHours(-1).Hour.ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                         _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;
                     }
                     else*/
                    //{
                    _DateTime = p_Date.ToShortDateString() + CommonConstants.Space + p_Date.Hour.ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                    _DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;
                    //}
                }

                return _DateTime;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateIsActive(long p_SearchRequestKey, bool p_IsActive,long p_ClientID,string p_Query_Name)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ISearchRequestModel.UpdateIsActive(p_SearchRequestKey, p_IsActive, p_ClientID, p_Query_Name);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods gets Search request Information from DataSet By ClientID.
        /// </summary>
        /// <returns>List of Object of Search Request.This will also include InActive records</returns> 
        public List<IQAgentSearchRequest> SelectAllByClientID(Guid ClientGUID)
        {
            DataSet _DataSet = null;
            List<IQAgentSearchRequest> _ListOfSearchRequest = null;
            try
            {
                _DataSet = _ISearchRequestModel.SelectAllByClientID(ClientGUID);
                _ListOfSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSearchRequest;
        }

        /// <summary>
        /// Description: This Methods gets Search request Information from DataSet By ClientID & Query Name.
        /// </summary>
        /// <returns>List of Object of Search Request</returns>
        public List<IQAgentSearchRequest> GetSearchRequestsByClientIDQueryName(long p_ClientID, string p_QueryName)
        {
            DataSet _DataSet = null;
            List<IQAgentSearchRequest> _ListOfSearchRequest = null;
            try
            {
                _DataSet = _ISearchRequestModel.GetSearchRequestsByClientIDQueryName(p_ClientID, p_QueryName);
                _ListOfSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSearchRequest;
        }     

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
                       //string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.CC_Text)) ? null :
                       //new XAttribute("CC_Text", _IQAgentResults.CC_Text),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.Rl_Station)) ? null :
                       new XAttribute("Rl_Station", _IQAgentResults.Rl_Station),
                       string.IsNullOrEmpty(Convert.ToString(_IQAgentResults.RL_Market)) ? null :
                       new XAttribute("Rl_Market", _IQAgentResults.RL_Market)

                           )));
            return xmlDocument;
        }

        private void InsertIQServiceLog(string _Event, string _FilePath, ConnectionStringKeys p_ConnectionStringKeys)
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

                _string = _IIq_Service_logController.AddRL_GUIDS(_Iq_Service_log, p_ConnectionStringKeys);

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
                //
                string _RequestPath = _FilePath;
                string _FileContent = string.Empty;
                if (!string.IsNullOrEmpty(_RequestPath))
                {
                    //string _FolderPath = _RequestPath;
                    FileInfo _FileInfo = new FileInfo(_RequestPath);

                    if (_FileInfo.Exists)
                    {
                        XmlDocument _XmlDocument = new XmlDocument();
                        _XmlDocument.Load(_RequestPath);
                        //_ExistingInfo = _XmlDocument.InnerXml;
                        _FileContent = "<ServiceLog>";
                        _FileContent += "<filePath>" + _RequestPath + "</filePath>";
                        _FileContent += "<configuration>" + _XmlDocument.InnerXml + "</configuration>";
                        _FileContent += "</ServiceLog>";
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

        /// <summary>
        /// This method gets all SearchTerm and SearchRequestKey with unique QueryName and max Query Version
        /// </summary>
        /// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        /// <returns>List of object of IQAgentSearchRequest</returns>
        public List<IQAgentSearchRequest> GetSearchRequestAll(ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                List<IQAgentSearchRequest> _ListOfIQAgentSearchRequest = null;

                DataSet _DataSet = _ISearchRequestModel.GetSearchRequestAll(p_ConnectionStringKeys);

                _ListOfIQAgentSearchRequest = FillSearchRequestInformationByQueryName(_DataSet);

                return _ListOfIQAgentSearchRequest;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }

    public class CustomException
    {
        public string Exception;
        public List<IQ_Agent_FetchLog> _ListOfIQAgentFetchLog;
    }
}
