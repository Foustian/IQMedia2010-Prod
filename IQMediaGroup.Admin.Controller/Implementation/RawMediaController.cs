using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using IQMediaGroup.Admin.Controller.App_Code;
using IQMediaGroup.Core.Enumeration;
using System.Data;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Model.Factory;
using System.Threading;
using System.Diagnostics;
using System.Web;
//using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Remoting.Contexts;
using System.Web.Hosting;
//using System.Windows.Forms;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IRawMedia
    /// </summary>
    internal class RawMediaController : IRawMediaController
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        private string InsertintoRLStationException(string p_RL_StationID, string p_DateTime, string p_GMT_Adj, string p_DST_Adj, IQ_Process p_IQ_Process, string p_Time_Zone,string p_RQ_Converted_DateTime,bool p_IsMisMatch)
        {
            try
            {
                IRL_Station_exceptionController _IRL_Station_exceptionController = _ControllerFactory.CreateObject<IRL_Station_exceptionController>();
                RL_Station_exception _RL_Station_exception = new RL_Station_exception();
                string _string = string.Empty;

                DateTime? _DateTime = null;
                _DateTime = CommonFunctions.GetDateTimeValue(p_DateTime);

                DateTime? _RQConvertedDate = null;
                _RQConvertedDate = CommonFunctions.GetDateTimeValue(p_RQ_Converted_DateTime);

                if (_DateTime == null)
                {
                    throw new Exception();
                }

                if (p_Time_Zone.ToLower() == StationTimeZone.CST.ToString().ToLower())
                {
                    _RL_Station_exception.Time_zone = StationTimeZone.CST.ToString();
                }
                else if (p_Time_Zone.ToLower() == StationTimeZone.MST.ToString().ToLower())
                {
                    _RL_Station_exception.Time_zone = StationTimeZone.MST.ToString();
                }
                else if (p_Time_Zone.ToLower() == StationTimeZone.PST.ToString().ToLower())
                {
                    _RL_Station_exception.Time_zone = StationTimeZone.PST.ToString();
                }
                else if (p_Time_Zone.ToLower()==StationTimeZone.EST.ToString().ToLower())
                {
                    _RL_Station_exception.Time_zone = StationTimeZone.EST.ToString();
                }

                _RL_Station_exception.GMT_Adj = p_GMT_Adj;
                _RL_Station_exception.DST_Adj = p_DST_Adj;
                _RL_Station_exception.RL_Station_ID = p_RL_StationID;
                _RL_Station_exception.IQ_Process = p_IQ_Process.ToString();
                _RL_Station_exception.RL_Station_Date = Convert.ToDateTime(_DateTime.Value.ToShortDateString());
                _RL_Station_exception.RL_Station_Time = (Convert.ToDateTime(p_DateTime).Hour) * 100;
                _RL_Station_exception.RQ_Converted_Date = _RQConvertedDate;

                if (_RQConvertedDate!=null)
                {
                    _RL_Station_exception.RQ_Converted_Time =  (_RQConvertedDate.Value.Hour * 100);   
                }                

                _string = _IRL_Station_exceptionController.AddRL_Station_exception(_RL_Station_exception,p_IsMisMatch);

                return _string;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private string GenerateDateTime(DateTime p_Date, int p_Time, string p_StationTimeZone, bool p_IsDefaultDate)
        {
            try
            {
                string _DateTime = string.Empty;
                p_Date = new DateTime(p_Date.Year, p_Date.Month, p_Date.Day, Convert.ToInt32(p_Time), 0, 0);

                double _CSTValue = -1;
                double _MSTValue = -2;
                double _PSTValue = -3;               

                if (p_StationTimeZone.ToLower() == StationTimeZone.CST.ToString().ToLower())
                {
                    _DateTime = p_Date.AddHours(_CSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_CSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                    /*_DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;*/
                }
                else if (p_StationTimeZone.ToLower() == StationTimeZone.MST.ToString().ToLower())
                {
                    _DateTime = p_Date.AddHours(_MSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_MSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                    /*_DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;*/
                }
                else if (p_StationTimeZone.ToLower() == StationTimeZone.PST.ToString().ToLower())
                {
                    _DateTime = p_Date.AddHours(_PSTValue).ToShortDateString() + CommonConstants.Space + ((p_Date.AddHours(_PSTValue)).Hour).ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                    /*_DateTime = CommonConstants.DblQuote + _DateTime + CommonConstants.DblQuote;*/
                }
                else
                {
                   _DateTime = p_Date.ToShortDateString() + CommonConstants.Space + p_Time.ToString() + CommonConstants.Colon + CommonConstants.Zero + CommonConstants.Colon + CommonConstants.Zero;
                }

                return _DateTime;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private SearchParameters ReadParamsFromFile(string p_FilePath, ref bool p_IsDefaultDate,ref DateTime p_StartDateTime)
        {
            try
            {
                SearchParameters _SearchParameters = new SearchParameters();
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

                        _SearchParameters = (SearchParameters)(CommonFunctions.MakeDeserialiazation(_ExistingInfo, _SearchParameters));
                        //_SearchParameters=(SearchParameters)(CommonFunctions.DeserializeObject<SearchParameters>(_ExistingInfo));
                    }
                    else
                    {
                        throw new MyException(CommonConstants.MsgFileNotExist);
                    }
                }


                if (_SearchParameters.DebugFileFlag == true)
                {
                    string _FolderPathTime = _SearchParameters.DebugFilePath;

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

                if (string.IsNullOrEmpty(_SearchParameters.DBTable))
                {
                    _SearchParameters.DBTable = CommonConstants.DFDBTable;
                }

                if (string.IsNullOrEmpty(_SearchParameters.LoopCount))
                {
                    _SearchParameters.LoopCount = CommonConstants.DFLoopCount.ToString();
                }

                if (string.IsNullOrEmpty(_SearchParameters.CCSearchResponseTimeOut))
                {
                    _SearchParameters.CCSearchResponseTimeOut = CommonConstants.DFCCSearchResponseTimeOut.ToString();
                }

                if (CommonFunctions.GetIntValue(_SearchParameters.LoopCount) == null || CommonFunctions.GetIntValue(_SearchParameters.LoopCount) <= 0)
                {
                    throw new MyException(CommonConstants.MsgLoopCountInvalid);
                }

                if (CommonFunctions.GetIntValue(_SearchParameters.CCSearchResponseTimeOut) == null || CommonFunctions.GetIntValue(_SearchParameters.CCSearchResponseTimeOut) <= 0)
                {
                    throw new MyException(CommonConstants.MsgCCSearchResponseTimeOutInvalid);
                }

                if (string.IsNullOrEmpty(_SearchParameters.ConnectionString))
                {
                    throw new MyException(CommonConstants.ConfigconnectionString + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                }             

                p_StartDateTime = DateTime.Now;

                if (string.IsNullOrEmpty(_SearchParameters.StartDate))
                {
                    if (string.IsNullOrEmpty(_SearchParameters.EndDate))
                    {
                        if (string.IsNullOrEmpty(_SearchParameters.Time))
                        {
                            p_IsDefaultDate = true;

                            DateTime _Today = DateTime.Now.AddHours(-1);
                            p_StartDateTime = DateTime.Now;
                            _SearchParameters.StartDate = _Today.ToString();
                            _SearchParameters.EndDate = _Today.ToString();
                            _SearchParameters.Time = _Today.Hour.ToString();                           
                        }
                        else
                        {
                            throw new MyException(CommonConstants.ParamStartDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                        }
                    }
                    else
                    {
                        throw new MyException(CommonConstants.ParamStartDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                    }
                }
                else
                {
                    _Out = CommonFunctions.GetDateTimeValue(_SearchParameters.StartDate);

                    if (_Out == null)
                    {
                        throw new MyException(CommonConstants.ParamStartDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                    }

                    if (string.IsNullOrEmpty(_SearchParameters.EndDate))
                    {
                        throw new MyException(CommonConstants.ParamEndDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                    }
                    else
                    {
                        _Out = CommonFunctions.GetDateTimeValue(_SearchParameters.EndDate);

                        if (_Out == null)
                        {
                            throw new MyException(CommonConstants.ParamEndDate + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                        }

                        if (string.IsNullOrEmpty(_SearchParameters.Time))
                        {
                            throw new MyException(CommonConstants.ParamTime + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                        }
                        else
                        {

                        }
                    }
                }

                if (!string.IsNullOrEmpty(_SearchParameters.StartDate) && !string.IsNullOrEmpty(_SearchParameters.EndDate))
                {
                    if (Convert.ToDateTime(_SearchParameters.StartDate) > Convert.ToDateTime(_SearchParameters.EndDate))
                    {
                        throw new MyException(CommonConstants.MsgStartEndDate);
                    }
                }

                int? _TimeChk = null;
                _TimeChk=CommonFunctions.GetIntValue(_SearchParameters.Time);

                if (_TimeChk==null)
                {
                    throw new MyException(CommonConstants.ParamTime + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                }

                if (Convert.ToInt32(_SearchParameters.Time) > 23 || Convert.ToInt32(_SearchParameters.Time) < 0)
                {
                    throw new MyException(CommonConstants.ParamTime + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                }

                if (Convert.ToDateTime(_SearchParameters.StartDate) > DateTime.Now || Convert.ToDateTime(_SearchParameters.EndDate) > DateTime.Now)
                {
                    throw new MyException(CommonConstants.MsgDateGTCurrentDate);
                }

                return _SearchParameters;
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

        private List<RL_GUIDS> GetListOfRMFromWebResponse(string _HttpWebResponse, object p_object,string p_DateTime,string p_ConvertedDateTime)
        {
            try
            {
                string _GMTAdj = string.Empty;
                string _DSTAdj = string.Empty;
                string _RL_StationID=string.Empty;
                string _TimeZone=string.Empty;

                object _RL_STATION = p_object as RL_STATION;

                if (_RL_STATION == null)
                {
                    _RL_STATION = p_object as RL_Station_exception;

                    _GMTAdj = ((RL_Station_exception)_RL_STATION).GMT_Adj;
                    _DSTAdj = ((RL_Station_exception)_RL_STATION).DST_Adj;
                    _RL_StationID=((RL_Station_exception)_RL_STATION).RL_Station_ID;
                    _TimeZone=((RL_Station_exception)_RL_STATION).Time_zone;
                }
                else
                {
                    _GMTAdj = ((RL_STATION)_RL_STATION).gmt_adj;
                    _DSTAdj = ((RL_STATION)_RL_STATION).dst_adj;
                    _RL_StationID=((RL_STATION)_RL_STATION).RL_Station_ID;
                    _TimeZone=((RL_STATION)_RL_STATION).time_zone;
                }

                List<RL_GUIDS> _ListOfRL_GUIDS = new List<RL_GUIDS>();

                string[] _SplitArray = new string[1];
                _SplitArray[0] = CommonConstants.RMSearchGuid;

                string[] _StringArray = _HttpWebResponse.Split(_SplitArray, StringSplitOptions.None);

                for (int _IndexStringArrray = 1; _IndexStringArrray < _StringArray.Length; _IndexStringArrray++)
                {
                    _StringArray[_IndexStringArrray] = CommonConstants.RMSearchGuid + _StringArray[_IndexStringArrray];
                }

                List<KeyValue> _ListOfKeyValue = new List<KeyValue>();

                KeyValue _KeyValueID = new KeyValue(CommonConstants.RMSearchGuid, false);
                KeyValue _KeyValueDate = new KeyValue(CommonConstants.RMSearchHour, false);
                KeyValue _KeyTimeZone = new KeyValue(CommonConstants.RMSearchTimeZone, false);
                KeyValue _KeyStationID = new KeyValue(CommonConstants.RMSearchStationID, false);

                _ListOfKeyValue.Add(_KeyValueDate);
                _ListOfKeyValue.Add(_KeyValueID);
                _ListOfKeyValue.Add(_KeyTimeZone);
                _ListOfKeyValue.Add(_KeyStationID);

                String constructedString = string.Empty;

                foreach (string _String in _StringArray)
                {
                    foreach (KeyValue _KeyValue in _ListOfKeyValue)
                    {
                        CommonFunctions.FindKey(_KeyValue, _String);
                    }

                    bool _Status = false;

                    foreach (KeyValue _FindClipKeyValueStatus in _ListOfKeyValue)
                    {
                        _Status = _FindClipKeyValueStatus._SetKey;

                        if (_Status == false)
                        {
                            break;
                        }
                    }

                    if (_Status == true)
                    {
                        RL_GUIDS _RL_GUIDS = new RL_GUIDS();

                        _RL_GUIDS.RL_GUID = _KeyValueID._KeyValue;
                        _RL_GUIDS.RL_Station_Date = Convert.ToDateTime(_KeyValueDate._KeyValue);
                        _RL_GUIDS.RL_Station_Time = (Convert.ToDateTime(_KeyValueDate._KeyValue).Hour * 100);
                        _RL_GUIDS.RL_Time_zone = _KeyTimeZone._KeyValue;
                        _RL_GUIDS.RL_Station_ID = _KeyStationID._KeyValue;

                        if ((DateTime.UtcNow - DateTime.Now).Hours == Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ConfigUTCDifference].ToString()))
                        {
                            _RL_GUIDS.GMT_Date = _RL_GUIDS.RL_Station_Date.AddHours((-1) * Convert.ToDouble(_GMTAdj));
                            _RL_GUIDS.GMT_Time = (_RL_GUIDS.RL_Station_Date.AddHours((-1) * Convert.ToDouble(_GMTAdj))).Hour * 100;
                        }
                        else
                        {
                            _RL_GUIDS.GMT_Date = _RL_GUIDS.RL_Station_Date.AddHours((-1) * Convert.ToDouble(_GMTAdj) - Convert.ToDouble(_DSTAdj));
                            _RL_GUIDS.GMT_Time = (_RL_GUIDS.RL_Station_Date.AddHours((-1) * Convert.ToDouble(_GMTAdj) - Convert.ToDouble(_DSTAdj))).Hour * 100;
                        }

                        _RL_GUIDS.IQ_CC_Key = _RL_GUIDS.RL_Station_ID + CommonConstants.UnderScore + (_RL_GUIDS.GMT_Date.ToString(CommonConstants.DateFormatYear)) + CommonConstants.UnderScore + _RL_GUIDS.GMT_Time.ToString().PadLeft(4,'0');                        

                        if (Convert.ToDateTime(_KeyValueDate._KeyValue)==Convert.ToDateTime(p_ConvertedDateTime))
                        {
                            _ListOfRL_GUIDS.Add(_RL_GUIDS); 
                        }
                        else
                        {
                            InsertintoRLStationException(_RL_StationID, p_DateTime, _GMTAdj, _DSTAdj, IQ_Process.GUID, _TimeZone, p_ConvertedDateTime,true);
                        }

                        _KeyValueID._KeyValue = string.Empty;
                        _KeyValueID._SetKey = false;

                        _KeyValueDate._KeyValue = string.Empty;
                        _KeyValueDate._SetKey = false;

                        _KeyTimeZone._KeyValue = string.Empty;
                        _KeyTimeZone._SetKey = false;

                        _KeyStationID._KeyValue = string.Empty;
                        _KeyStationID._SetKey = false;
                    }
                }

                return _ListOfRL_GUIDS;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void InsertRLGUIDS(List<RL_GUIDS> _ListOfRL_GUIDS, string p_DateTime, string p_GMT_Adj, string p_DST_Adj, ref int p_NoOfInsertedRecords,DateTime p_RequestDate,int p_RequestTime)
        {
            try
            {
                ControllerFactory _ControllerFactory = new ControllerFactory();
                IRL_GUIDSController _IRL_GUIDSController = _ControllerFactory.CreateObject<IRL_GUIDSController>();

                foreach (RL_GUIDS _RL_GUIDS in _ListOfRL_GUIDS)
                {
                    string _RLGUIDKey = string.Empty;

                    try
                    {
                        _RLGUIDKey = _IRL_GUIDSController.AddRL_GUIDS(_RL_GUIDS,p_RequestDate,p_RequestTime);
                    }
                    catch (Exception _InnerException)
                    {
                        Console.WriteLine(CommonConstants.InsertError + _RL_GUIDS.RL_GUID);
                    }

                    if (!string.IsNullOrEmpty(_RLGUIDKey) && _RLGUIDKey != CommonConstants.Zero)
                    {
                        p_NoOfInsertedRecords = p_NoOfInsertedRecords + 1;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void FilterStringOperation(object p_Object)
        {
            try
            {
                List<object> _ListOfMainObject = (List<object>)p_Object;

                List<string> _ListOfString = (List<string>)_ListOfMainObject[0];
                List<RawMedia> _ListOfRawMedia = (List<RawMedia>)_ListOfMainObject[1];

                StringOperation _StringOperationFirst = new StringOperation();
                StringOperation _StringOperationSecond = new StringOperation();
                StringOperation _StringOperationThird = new StringOperation();
                StringOperation _StringOperationFourth = new StringOperation();
                StringOperation _StringOperationFive = new StringOperation();

                Thread _ThreadStringFirst = new Thread(new ParameterizedThreadStart(_StringOperationFirst.GetRawMediaInfo));
                Thread _ThreadStringSecond = new Thread(new ParameterizedThreadStart(_StringOperationSecond.GetRawMediaInfo));
                Thread _ThreadStringThird = new Thread(new ParameterizedThreadStart(_StringOperationThird.GetRawMediaInfo));
                Thread _ThreadStringFourth = new Thread(new ParameterizedThreadStart(_StringOperationFourth.GetRawMediaInfo));
                Thread _ThreadStringFive = new Thread(new ParameterizedThreadStart(_StringOperationFive.GetRawMediaInfo));

                bool _RunCurrentThread = true;

                int _ListOfStirngCount = _ListOfString.Count;
                int _CurrentIndex = 0;
                int _CopyElements = 200;

                if (_ListOfStirngCount > 200)
                {

                    while (_CurrentIndex < _ListOfStirngCount)
                    {
                        if ((_ListOfStirngCount - _CurrentIndex) > 200)
                        {
                            _CopyElements = 200;
                        }
                        else
                        {
                            _CopyElements = (_ListOfStirngCount - _CurrentIndex);
                        }

                        string[] _StringArray = new string[_CopyElements];
                        _ListOfString.CopyTo(_CurrentIndex, _StringArray, 0, _CopyElements);

                        _CurrentIndex = _CurrentIndex + _CopyElements;

                        List<string> _SubListOfString = new List<string>(_StringArray);

                        while (_RunCurrentThread == true)
                        {

                            if (_ThreadStringFirst.IsAlive == false && _ThreadStringFirst.ThreadState != System.Threading.ThreadState.Running)
                            {
                                List<object> _ListOfObject = new List<object>();

                                _ListOfObject.Add(_SubListOfString);
                                _ListOfObject.Add(_ListOfRawMedia);

                                _RunCurrentThread = false;

                                _ThreadStringFirst = new Thread(new ParameterizedThreadStart(_StringOperationFirst.GetRawMediaInfo));

                                _ThreadStringFirst.Start((object)_ListOfObject);
                            }
                            else if (_ThreadStringSecond.IsAlive == false && _ThreadStringSecond.ThreadState != System.Threading.ThreadState.Running)
                            {
                                List<object> _ListOfObject = new List<object>();

                                _ListOfObject.Add(_SubListOfString);
                                _ListOfObject.Add(_ListOfRawMedia);

                                _RunCurrentThread = false;

                                _ThreadStringSecond = new Thread(new ParameterizedThreadStart(_StringOperationSecond.GetRawMediaInfo));

                                _ThreadStringSecond.Start((object)_ListOfObject);
                            }
                            else if (_ThreadStringThird.IsAlive == false && _ThreadStringThird.ThreadState != System.Threading.ThreadState.Running)
                            {
                                List<object> _ListOfObject = new List<object>();

                                _ListOfObject.Add(_SubListOfString);
                                _ListOfObject.Add(_ListOfRawMedia);

                                _RunCurrentThread = false;

                                _ThreadStringThird = new Thread(new ParameterizedThreadStart(_StringOperationThird.GetRawMediaInfo));

                                _ThreadStringThird.Start((object)_ListOfObject);
                            }
                            else if (_ThreadStringFourth.IsAlive == false && _ThreadStringFourth.ThreadState != System.Threading.ThreadState.Running)
                            {
                                List<object> _ListOfObject = new List<object>();

                                _ListOfObject.Add(_SubListOfString);
                                _ListOfObject.Add(_ListOfRawMedia);

                                _RunCurrentThread = false;

                                _ThreadStringFourth = new Thread(new ParameterizedThreadStart(_StringOperationFourth.GetRawMediaInfo));

                                _ThreadStringFourth.Start((object)_ListOfObject);
                            }
                            else if (_ThreadStringFive.IsAlive == false && _ThreadStringFive.ThreadState != System.Threading.ThreadState.Running)
                            {
                                List<object> _ListOfObject = new List<object>();

                                _ListOfObject.Add(_SubListOfString);
                                _ListOfObject.Add(_ListOfRawMedia);

                                _RunCurrentThread = false;

                                _ThreadStringFive = new Thread(new ParameterizedThreadStart(_StringOperationFive.GetRawMediaInfo));

                                _ThreadStringFive.Start((object)_ListOfObject);
                            }
                        }

                        _RunCurrentThread = true;
                    }
                    List<Thread> _ListOfThread = new List<Thread>();

                    _ListOfThread.Add(_ThreadStringFirst);
                    _ListOfThread.Add(_ThreadStringSecond);
                    _ListOfThread.Add(_ThreadStringThird);
                    _ListOfThread.Add(_ThreadStringFourth);
                    _ListOfThread.Add(_ThreadStringFive);

                    foreach (Thread _Thread in _ListOfThread)
                    {
                        if (_Thread.IsAlive)
                        {
                            _Thread.Join();
                        }
                    }
                }
                else
                {
                    List<object> _ListOfObject = new List<object>();

                    _ListOfObject.Add(_ListOfString);
                    _ListOfObject.Add(_ListOfRawMedia);

                    _StringOperationFirst.GetRawMediaInfo((object)_ListOfObject);
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_DateMin">Min Date</param>
        /// <param name="p_DateMax">Max Date</param>
        /// <param name="p_TimeMin">Min Time</param>
        /// <param name="p_TimeMax">Max Time</param>
        /// <param name="IsDateTime">bool value</param>
        /// <returns>DateTime string</returns>
        public string GenerateTimeFromDate(string p_DateMin, string p_DateMax, int? p_TimeMin, int? p_TimeMax, bool IsDateTime)
        {
            try
            {
                string TimeValue = string.Empty;

                int? _MaxTime = p_TimeMax;
                bool _DayDiff = false;

                for (DateTime _Index = Convert.ToDateTime(Convert.ToDateTime(p_DateMin).ToShortDateString()); _Index <= Convert.ToDateTime(Convert.ToDateTime(p_DateMax).ToShortDateString()); )
                {
                    if (IsDateTime == false)
                    {
                        if (_Index.ToShortDateString() != Convert.ToDateTime(p_DateMax).ToShortDateString())
                        {
                            p_TimeMax = 23;
                            _DayDiff = true;
                        }
                        else
                        {
                            p_TimeMax = _MaxTime;

                            if (_DayDiff == true)
                            {
                                p_TimeMin = 0;
                            }
                        }
                    }

                    for (int? Time = p_TimeMin; Time <= p_TimeMax; Time++)
                    {
                        if (string.IsNullOrEmpty(TimeValue.ToString()))
                        {
                            TimeValue = CommonConstants.DblQuote + TimeValue + _Index.ToShortDateString() + CommonConstants.Space + Time.ToString() + CommonConstants.Colon + CommonConstants.RMZDZT + CommonConstants.DblQuote;
                        }
                        else
                        {
                            TimeValue = TimeValue + CommonConstants.Comma + CommonConstants.DblQuote + _Index.ToShortDateString() + CommonConstants.Space + Time.ToString() + CommonConstants.Colon + CommonConstants.RMZDZT + CommonConstants.DblQuote;
                        }

                    }

                    _Index = _Index.AddDays(1);
                }

                return TimeValue.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        public class StringOperation
        {
            /// <summary>
            /// This method gets RawMedia Information
            /// </summary>
            /// <param name="p_object">ist of object that contains Key value and rawmedia details</param>
            public void GetRawMediaInfo(object p_object)
            {
                try
                {
                    List<object> _ListOfObject = (List<object>)p_object;

                    List<string> _ListOfString = (List<string>)_ListOfObject[0];
                    List<RawMedia> _ListOfRawMedia = (List<RawMedia>)_ListOfObject[1];

                    List<KeyValue> _ListOfKeyValue = new List<KeyValue>();

                    KeyValue _KeyValueID = new KeyValue(CommonConstants.RMSearchGuid, false);
                    KeyValue _KeyValueDate = new KeyValue(CommonConstants.RMSearchHour, false);
                    KeyValue _KeyValueTitle = new KeyValue(CommonConstants.RMSearchStationTitle, false);
                    KeyValue _KeyValueLogo = new KeyValue(CommonConstants.RMSearchStationLogo, false);
                    KeyValue _KeyCacheKey = new KeyValue(CommonConstants.RMSearchCacheKey, false);
                    KeyValue _KeyComplete = new KeyValue(CommonConstants.RMSearchComplete, false);
                    KeyValue _KeyHits = new KeyValue(CommonConstants.RMSearchHits, false);

                    _ListOfKeyValue.Add(_KeyValueTitle);
                    _ListOfKeyValue.Add(_KeyValueDate);
                    _ListOfKeyValue.Add(_KeyValueID);
                    _ListOfKeyValue.Add(_KeyValueLogo);
                    _ListOfKeyValue.Add(_KeyComplete);
                    _ListOfKeyValue.Add(_KeyCacheKey);
                    _ListOfKeyValue.Add(_KeyHits);

                    foreach (string _String in _ListOfString)
                    {
                        foreach (KeyValue _KeyValue in _ListOfKeyValue)
                        {
                            FindKey(_KeyValue, _String);
                        }

                        bool _Status = false;

                        foreach (KeyValue _FindClipKeyValueStatus in _ListOfKeyValue)
                        {
                            _Status = _FindClipKeyValueStatus._SetKey;

                            if (_Status == false)
                            {
                                break;
                            }
                        }

                        if (_Status == true)
                        {
                            RawMedia _RawMedia = new RawMedia();

                            _RawMedia.RawMediaID = new Guid(_KeyValueID._KeyValue);
                            _RawMedia.StationTitle = _KeyValueTitle._KeyValue;
                            _RawMedia.DateTime = Convert.ToDateTime(_KeyValueDate._KeyValue);
                            _RawMedia.StationLogo = _KeyValueLogo._KeyValue;
                            _RawMedia.CacheKey = _KeyCacheKey._KeyValue;
                            _RawMedia.Complete = Convert.ToBoolean(_KeyComplete._KeyValue);

                            if (_RawMedia.Complete == true)
                            {
                                _RawMedia.Hits = Convert.ToInt32(_KeyHits._KeyValue);
                            }

                            _KeyValueID._KeyValue = string.Empty;
                            _KeyValueID._SetKey = false;

                            _KeyValueDate._KeyValue = string.Empty;
                            _KeyValueDate._SetKey = false;

                            _KeyValueTitle._KeyValue = string.Empty;
                            _KeyValueTitle._SetKey = false;

                            _KeyValueLogo._KeyValue = string.Empty;
                            _KeyValueLogo._SetKey = false;

                            _KeyHits._KeyValue = string.Empty;
                            _KeyHits._SetKey = false;

                            _ListOfRawMedia.Add(_RawMedia);
                        }
                    }
                }
                catch (Exception _Exception)
                {
                    throw _Exception;
                }
            }

            /// <summary>
            /// This method is used to find individual records from the web response
            /// Added By:Vishal Parekh
            /// </summary>
            /// <param name="p_FindKeyValue">Key value</param>
            /// <param name="p_String">Response string</param>
            /// <returns>Status of the Key value</returns>
            public bool FindKey(KeyValue p_FindKeyValue, string p_String)
            {
                try
                {
                    if (p_String.Contains(p_FindKeyValue._FindKey))
                    {
                        string _SubString = p_String;
                        _SubString = p_String.Substring(p_String.IndexOf(p_FindKeyValue._FindKey));

                        if (p_FindKeyValue._FindKey == "\"Complete\":")
                        {
                            _SubString = _SubString.Substring(p_FindKeyValue._FindKey.Length, (_SubString.IndexOf(",\"") - p_FindKeyValue._FindKey.Length));
                        }
                        else if (p_FindKeyValue._FindKey == "\"Hits\":")
                        {
                            _SubString = _SubString.Substring(p_FindKeyValue._FindKey.Length, (_SubString.IndexOf("}") - p_FindKeyValue._FindKey.Length));
                        }
                        else
                        {
                            _SubString = _SubString.Substring(p_FindKeyValue._FindKey.Length, (_SubString.IndexOf("\",\"") - p_FindKeyValue._FindKey.Length));
                        }
                        p_FindKeyValue._KeyValue = _SubString;
                        p_FindKeyValue._SetKey = true;

                        return true;
                    }

                    return false;
                }
                catch (Exception _Exception)
                {
                    throw _Exception;
                }
            }
        }

        private void InsertIQServiceLog(string _Event, string _FilePath)
        {
            try
            {
                ControllerFactory _ControllerFactory = new ControllerFactory();
                string _string = string.Empty;

                IIq_Service_logController _IIq_Service_logController = _ControllerFactory.CreateObject<IIq_Service_logController>();
                Iq_Service_log _Iq_Service_log = new Iq_Service_log();
                _Iq_Service_log.ModuleName = "RL_GUID_fetch";
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
    }
}
