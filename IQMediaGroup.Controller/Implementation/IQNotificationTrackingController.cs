using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Core.Enumeration;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Net.Mail;
using IQMediaGroup.Controller.Factory;
using System.Net;
using System.Text.RegularExpressions;

namespace IQMediaGroup.Controller.Implementation
{
    public class IQNotificationTrackingController : IIQNotificationTrackingController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly IIQNotificationTrackingModel _IIQNotificationTrackingModel;

        public IQNotificationTrackingController()
        {
            _IIQNotificationTrackingModel = _ModelFactory.CreateObject<IIQNotificationTrackingModel>();
        }

        #region IIQNotificationTrackingController Members

        /// <summary>
        /// Description:This method will get IQNotificationTracking
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>List of object of IQNotificationTracking</returns>
        public List<IQNotificationTracking> SelectForNotification()
        {
            DataSet _DataSet = null;
            List<IQNotificationTracking> _ListOfIQNotificationTracking = null;
            try
            {
                _DataSet = _IIQNotificationTrackingModel.SelectForNotification();
                _ListOfIQNotificationTracking = FillNotificationSettings(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfIQNotificationTracking;
        }

        /// <summary>
        /// Description:This method will fill IQNotificationTracking
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dtataset</param>
        /// <returns>List of object of IQNotificationTracking</returns>
        private List<IQNotificationTracking> FillNotificationSettings(DataSet _DataSet)
        {
            List<IQNotificationTracking> _ListOfIQNotificationTracking = new List<IQNotificationTracking>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQNotificationTracking _IQNotificationTracking = new IQNotificationTracking();
                        if (_DataSet.Tables[0].Columns.Contains("IQNotificationTrackingKey") && !_DataRow["IQNotificationTrackingKey"].Equals(DBNull.Value))
                        {
                            _IQNotificationTracking.IQNotificationTrackingKey = Convert.ToInt32(_DataRow["IQNotificationTrackingKey"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("SearchRequestID") && !_DataRow["SearchRequestID"].Equals(DBNull.Value))
                        {
                            _IQNotificationTracking.SearchRequestID = Convert.ToInt32(_DataRow["SearchRequestID"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("Query_Name") && !_DataRow["Query_Name"].Equals(DBNull.Value))
                        {
                            _IQNotificationTracking.Query_Name = Convert.ToString(_DataRow["Query_Name"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("TypeofEntry") && !_DataRow["TypeofEntry"].Equals(DBNull.Value))
                        {
                            _IQNotificationTracking.TypeofEntry = Convert.ToString(_DataRow["TypeofEntry"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("Notification_Address") && !_DataRow["Notification_Address"].Equals(DBNull.Value))
                        {
                            _IQNotificationTracking.Notification_Address = Convert.ToString(_DataRow["Notification_Address"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("Frequency") && !_DataRow["Frequency"].Equals(DBNull.Value))
                        {
                            _IQNotificationTracking.Frequency = Convert.ToString(_DataRow["Frequency"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("Processed_Flag") && !_DataRow["Processed_Flag"].Equals(DBNull.Value))
                        {
                            _IQNotificationTracking.Processed_Flag = Convert.ToInt32(_DataRow["Processed_Flag"]);
                        }
                        if (_DataSet.Tables[0].Columns.Contains("Processed_DateTime") && !_DataRow["Processed_DateTime"].Equals(DBNull.Value))
                        {
                            _IQNotificationTracking.Processed_DateTime = Convert.ToDateTime(_DataRow["Processed_DateTime"]);
                        }
                        _ListOfIQNotificationTracking.Add(_IQNotificationTracking);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfIQNotificationTracking;
        }

        /// <summary>
        /// Description:This method will Update IQNotificationTracking
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationTracking">IQNotificationTrackingKey</param>
        /// <returns>Count</returns>
        public string Update(IQNotificationTracking p_IQNotificationTracking)
        {
            try
            {
                string _Result = _IIQNotificationTrackingModel.Update(p_IQNotificationTracking);
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion IIQNotificationTrackingController Members

        #region Console Application Methods

        public int SendNotifications(string p_ConfigFilePath)
        {

            int Count = 0;
            SearchParameters _SearchParameter = VerifyAndReadParamsFromConfigFile(p_ConfigFilePath);

            CommonFunctions.BuildConnectionStringFromUserConfig(_SearchParameter.ConnectionString);

            try
            {
                // Record into IQService Log as "Start Event".
                InsertIQServiceLog("Start Event", p_ConfigFilePath);

                // Notify user that process is started
                Console.WriteLine("Process Started....");

                IQNotificationLog _IQNotificationLog = new IQNotificationLog();
                _IQNotificationLog.ProcessStart = DateTime.Now.ToString();
                _IQNotificationLog.ConfigFileParams = _SearchParameter;
                _IQNotificationLog.StoredProcedures = new List<string>();
                _IQNotificationLog.Tables = new List<string>();

                List<IQNotificationTracking> _ListOfIQNotificationTracking = SelectForNotification();
                _IQNotificationLog.StoredProcedures.Add(CommonConstants.usp_IQNotificationTracking_SelectByCommunicationFlag);
                _IQNotificationLog.Tables.Add(CommonConstants.Table_IQAgentResult);
                _IQNotificationLog.Tables.Add(CommonConstants.Table_IQNotificationSettings);
                _IQNotificationLog.Tables.Add(CommonConstants.Table_IQNotificationTracking);


                List<IQNotificationTracking_FetchLog> _ListOfIQNotificationTracking_FetchLog = new List<IQNotificationTracking_FetchLog>();
                int UpdatedRecordCount = 0;

                foreach (IQNotificationTracking _IQNotificationTracking in _ListOfIQNotificationTracking)
                {
                    DateTime _StartTimeStamp = DateTime.Now;

                    bool IsEmailSentSuccessfully = false;
                    if (_IQNotificationTracking.Frequency == NotificationFrequency.Immediate.ToString())
                    {
                        IsEmailSentSuccessfully = SendEmail(_IQNotificationTracking, _SearchParameter);
                        if (IsEmailSentSuccessfully)
                        {
                            Count++;
                        }
                    }
                    if (_IQNotificationTracking.Frequency == NotificationFrequency.Hourly.ToString())
                    {
                        int FromMinute = Convert.ToInt32(_SearchParameter.HourlyEmailTimeFromMinute);
                        int ToMinute = Convert.ToInt32(_SearchParameter.HourlyEmailTimeToMinute);

                        if (DateTime.Now.Minute >= FromMinute && DateTime.Now.Minute <= ToMinute)
                        {
                            IsEmailSentSuccessfully = SendEmail(_IQNotificationTracking, _SearchParameter);
                            if (IsEmailSentSuccessfully)
                            {
                                Count++;
                            }
                        }
                    }
                    if (_IQNotificationTracking.Frequency == NotificationFrequency.OnceDay.ToString())
                    {
                        int CurrentHour = Convert.ToInt32(_SearchParameter.DailyEmailTimeHours);
                        if (DateTime.Now.Hour == CurrentHour)
                        {
                            IsEmailSentSuccessfully = SendEmail(_IQNotificationTracking, _SearchParameter);
                            if (IsEmailSentSuccessfully)
                            {
                                Count++;
                            }
                        }
                    }
                    if (_IQNotificationTracking.Frequency == NotificationFrequency.OnceWeek.ToString())
                    {
                        if (GetDayName().ToLower() == _SearchParameter.WeeklyEmailDay.ToLower())
                        {
                            IsEmailSentSuccessfully = SendEmail(_IQNotificationTracking, _SearchParameter);
                            if (IsEmailSentSuccessfully)
                            {
                                Count++;
                            }
                        }
                    }

                    if (IsEmailSentSuccessfully)
                    {
                        // if notifiacation sent successfully update Processed_Flag & Processed_DateTime
                        Update(_IQNotificationTracking);
                        UpdatedRecordCount++;
                    }

                    #region Log File Entry

                    IQNotificationTracking_FetchLog objIQNotificationTracking_FetchLog = new IQNotificationTracking_FetchLog();
                    objIQNotificationTracking_FetchLog.SearchRequestID = _IQNotificationTracking.SearchRequestID.ToString();
                    objIQNotificationTracking_FetchLog.Startdatetimestamp = _StartTimeStamp.ToString();
                    objIQNotificationTracking_FetchLog.Enddatetimestamp = DateTime.Now.ToString();
                    objIQNotificationTracking_FetchLog.Notification_Address = _IQNotificationTracking.Notification_Address;
                    objIQNotificationTracking_FetchLog.Databasereadorwritefailure = string.Empty;
                    objIQNotificationTracking_FetchLog.Frequency = _IQNotificationTracking.Frequency;
                    objIQNotificationTracking_FetchLog.IsEmailSent = IsEmailSentSuccessfully ? "Yes" : "No";
                    _ListOfIQNotificationTracking_FetchLog.Add(objIQNotificationTracking_FetchLog);

                    #endregion Log File Entry

                }

                if (UpdatedRecordCount > 0)
                {
                    _IQNotificationLog.StoredProcedures.Add(CommonConstants.usp_IQNotificationTracking_Update);
                }

                _IQNotificationLog.ListOfIQNotificationLog = _ListOfIQNotificationTracking_FetchLog;
                _IQNotificationLog.ProcessEnd = DateTime.Now.ToString();

                // Record into IQService Log as "End Event".
                WriteDebugFile(_IQNotificationLog, _SearchParameter);

                InsertIQServiceLog("Stop Event", p_ConfigFilePath);
            }
            catch (Exception _Exception)
            {
                try
                {
                    WriteExceptionToDatabase(_Exception);
                    throw _Exception;
                }
                catch (Exception _InnerException)
                {
                    throw _InnerException;
                }
            }
            return Count;
        }

        private string GetDayName()
        {
            string DayName = string.Empty;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    DayName = "Sunday";
                    break;
                case DayOfWeek.Monday:
                    DayName = "Monday";
                    break;
                case DayOfWeek.Tuesday:
                    DayName = "Tuesday";
                    break;
                case DayOfWeek.Wednesday:
                    DayName = "Wednesday";
                    break;
                case DayOfWeek.Thursday:
                    DayName = "Thursday";
                    break;
                case DayOfWeek.Friday:
                    DayName = "Friday";
                    break;
                case DayOfWeek.Saturday:
                    DayName = "Saturday";
                    break;
            }
            return DayName;
        }

        private bool SendEmail(IQNotificationTracking _IQNotificationTracking, SearchParameters _SearchParameter)
        {
            bool retValue = false;
            try
            {
                if (!string.IsNullOrEmpty(_IQNotificationTracking.Notification_Address))
                {
                    string EmailBody = GetEmailBody(_IQNotificationTracking, _SearchParameter);

                    SmtpClient _SmtpClient = new SmtpClient();
                    string MailUserName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServerUser"));
                    string MailPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServerPassword"));

                    NetworkCredential _NetworkCredential = new NetworkCredential(MailUserName, MailPassword);
                    _SmtpClient.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("sSMTPPort"));
                    _SmtpClient.Credentials = _NetworkCredential;
                    _SmtpClient.Host = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServer"));

                    MailMessage _MailMessage = new MailMessage();
                    _MailMessage.From = new MailAddress(_SearchParameter.SMTPFromEMail, "IQMedia Notification");
                    _MailMessage.To.Add(new MailAddress(_IQNotificationTracking.Notification_Address));
                    _MailMessage.Body = EmailBody;


                    _MailMessage.Subject = _SearchParameter.Subject;
                    _MailMessage.IsBodyHtml = true;
                    _SmtpClient.Send(_MailMessage);
                    retValue = true;

                    // seprate try-catch block for MainLog function because exception throw by MainLog function doesn't affect retValue variable.
                    // this variable keeps track whether Email is sent successfully or not.

                    try
                    {
                        MailLog(EmailBody, _IQNotificationTracking, _SearchParameter);
                    }
                    catch (Exception _Exception)
                    {
                        WriteExceptionToDatabase(_Exception);
                    }
                }
            }
            catch (Exception _Exception)
            {
                retValue = false;
                WriteExceptionToDatabase(_Exception);
            }
            return retValue;
        }

        private void MailLog(string _EmailBody, IQNotificationTracking _IQNotificationTracking, SearchParameters _SearchParameter)
        {
            try
            {
                string _FileContent = string.Empty;

                _FileContent = "<EmailLog>";
                _FileContent += "<EmailContent>" + _EmailBody + "</EmailContent>";
                _FileContent += "</EmailLog>";

                string _Result = string.Empty;
                IOutboundReportingController _IOutboundReportingController = _ControllerFactory.CreateObject<IOutboundReportingController>();
                OutboundReporting _OutboundReporting = new OutboundReporting();
                _OutboundReporting.Query_Name = _IQNotificationTracking.Query_Name;
                _OutboundReporting.FromEmailAddress = _SearchParameter.SMTPFromEMail;
                _OutboundReporting.ToEmailAddress = _IQNotificationTracking.Notification_Address;
                _OutboundReporting.MailContent = _FileContent;
                _OutboundReporting.ServiceType = "IQNotification";
                _Result = _IOutboundReportingController.InsertOutboundReportingLog(_OutboundReporting);
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }

        }

        private string GetEmailBody(IQNotificationTracking _IQNotificationTracking, SearchParameters _SearchParameter)
        {
            string EmailBody = string.Empty;
            StreamReader _StreamReader = File.OpenText(_SearchParameter.EmailTemplateFilePath);
            EmailBody = _StreamReader.ReadToEnd();

            EmailBody = EmailBody.Replace("#email#", _IQNotificationTracking.Notification_Address);
            EmailBody = EmailBody.Replace("#query#", _IQNotificationTracking.Query_Name);

            return EmailBody;
        }

        /// <summary>
        /// this method verify config file given by user and fill SearchParameter object from this config file.
        /// </summary>
        /// <param name="p_FilePath"></param>
        /// <returns></returns>
        public static SearchParameters VerifyAndReadParamsFromConfigFile(string p_FilePath)
        {
            try
            {
                SearchParameters _SearchParameters = new SearchParameters();

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
                    }
                    else
                    {
                        throw new MyException(CommonConstants.MsgFileNotExist);
                    }
                }

                if (string.IsNullOrEmpty(_SearchParameters.HourlyEmailTimeFromMinute))
                {
                    throw new MyException(string.Format(CommonConstants.ParamsNotFound, CommonConstants.ParamHourlyEmailTimeFromMinute));
                }

                if (string.IsNullOrEmpty(_SearchParameters.HourlyEmailTimeToMinute))
                {
                    throw new MyException(string.Format(CommonConstants.ParamsNotFound, CommonConstants.ParamHourlyEmailTimeToMinute));
                }

                if (string.IsNullOrEmpty(_SearchParameters.DailyEmailTimeHours))
                {
                    throw new MyException(string.Format(CommonConstants.ParamsNotFound, CommonConstants.ParamDailyEmailTimeHours));
                }

                if (string.IsNullOrEmpty(_SearchParameters.WeeklyEmailDay))
                {
                    throw new MyException(string.Format(CommonConstants.ParamsNotFound, CommonConstants.ParamWeeklyEmailDay));
                }

                if (string.IsNullOrEmpty(_SearchParameters.SMTPFromEMail))
                {
                    throw new MyException(string.Format(CommonConstants.ParamsNotFound, CommonConstants.ParamSMTPFromEMail));
                }

                if (string.IsNullOrEmpty(_SearchParameters.EmailTemplateFilePath))
                {
                    throw new MyException(string.Format(CommonConstants.ParamsNotFound, CommonConstants.ParamEmailTemplateFilePath));
                }

                if (string.IsNullOrEmpty(_SearchParameters.Subject))
                {
                    throw new MyException(string.Format(CommonConstants.ParamsNotFound, CommonConstants.ParamSubject));
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

                if (string.IsNullOrEmpty(_SearchParameters.ConnectionString))
                {
                    throw new MyException(CommonConstants.ConfigconnectionString + CommonConstants.Space + CommonConstants.ParamsIncorrectFormat);
                }

                int HourlyTimeFromMinutes = 0;
                if (!Int32.TryParse(_SearchParameters.HourlyEmailTimeFromMinute, out HourlyTimeFromMinutes))
                {
                    throw new MyException("HourlyEmailTimeFromMinute Parameter is invalid.");
                }

                int HourlyTimeToMinutes = 0;
                if (!Int32.TryParse(_SearchParameters.HourlyEmailTimeToMinute, out HourlyTimeToMinutes))
                {
                    throw new MyException("HourlyEmailTimeToMinute Parameter is invalid.");
                }

                if (HourlyTimeFromMinutes < 0 || HourlyTimeFromMinutes > 59)
                {
                    throw new MyException("HourlyEmailTimeFromMinute Parameter is invalid.");
                }

                if (HourlyTimeToMinutes < 0 || HourlyTimeToMinutes > 59)
                {
                    throw new MyException("HourlyEmailTimeToMinute Parameter is invalid.");
                }


                int Hour = 0;
                if (!Int32.TryParse(_SearchParameters.DailyEmailTimeHours, out Hour))
                {
                    throw new MyException("DailyEmailTimeHours Parameter is invalid.");
                }

                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (!re.IsMatch(_SearchParameters.SMTPFromEMail))
                {
                    throw new MyException("SMTP From Email is invalid.");
                }

                if (!File.Exists(_SearchParameters.EmailTemplateFilePath))
                {
                    throw new MyException("Email template file does not exist.");
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

        /// <summary>
        /// This methods writes debug file if debug flag is true in User Provided Config file.
        /// </summary>
        /// <param name="_SearchParameters"></param>
        /// <param name="_ListOfIQNotificationTracking_FetchLog"></param>
        public void WriteDebugFile(IQNotificationLog _IQNotificationLog, SearchParameters _SearchParameters)
        {
            if (_SearchParameters.DebugFileFlag == true)
            {
                string _DateTimeAppend = DateTime.Now.ToString();
                _DateTimeAppend = _DateTimeAppend.Replace(" ", "_").Replace(":", "_").Replace("/", "_");

                string _XMLString = CommonFunctions.MakeSerialization(_IQNotificationLog);

                XmlDocument _XmlRequestDocument = new XmlDocument();
                _XmlRequestDocument.LoadXml(_XMLString);


                string _FolderPathTime = _SearchParameters.DebugFilePath;

                DirectoryInfo _DirectoryInfo = new DirectoryInfo(_FolderPathTime); ;

                _FolderPathTime = _FolderPathTime + CommonConstants.ForwardSlash + _DirectoryInfo.Name;

                string _FileURL = _DirectoryInfo.FullName + CommonConstants.ForwardSlash + "IQNotificationTracking" + CommonConstants.UnderScore + _DateTimeAppend + CommonConstants.Dot + CommonConstants.XmlText;

                _XmlRequestDocument.Save(_FileURL);
            }
        }

        /// <summary>
        /// this function inserts records into IQ_ServiceLog Table when Process starts as well Process ends.
        /// </summary>
        /// <param name="_ServiceCode">"Start Event" or "Stop Event"</param>
        /// <param name="_FilePath">Config File Path that will be inserted into one of column of IQ_ServiceLog Table</param>
        public void InsertIQServiceLog(string _ServiceCode, string _FilePath)
        {
            ControllerFactory _ControllerFactory = new ControllerFactory();
            string _string = string.Empty;

            IIq_Service_logController _IIq_Service_logController = _ControllerFactory.CreateObject<IIq_Service_logController>();
            Iq_Service_log _Iq_Service_log = new Iq_Service_log();
            _Iq_Service_log.ModuleName = "IQNotificationTracking";
            _Iq_Service_log.CreatedDatetime = DateTime.Now;
            _Iq_Service_log.ServiceCode = _ServiceCode;
            _Iq_Service_log.ConfigRequest = GetConfigFileContentForIQServiceLog(_FilePath);
            _string = _IIq_Service_logController.AddRL_GUIDS(_Iq_Service_log);
        }

        /// <summary>
        /// Read Content of config file from Path provided by user.
        /// </summary>
        /// <param name="_FilePath"></param>
        /// <returns></returns>
        public string GetConfigFileContentForIQServiceLog(string _FilePath)
        {
            try
            {
                string _FileContent = string.Empty;
                if (!string.IsNullOrEmpty(_FilePath))
                {
                    FileInfo _FileInfo = new FileInfo(_FilePath);
                    if (_FileInfo.Exists)
                    {
                        XmlDocument _XmlDocument = new XmlDocument();
                        _XmlDocument.Load(_FilePath);
                        _FileContent = "<ServiceLog>";
                        _FileContent += "<filePath>" + _FilePath + "</filePath>";
                        _FileContent += "<configuration>" + _XmlDocument.InnerXml + "</configuration>";
                        _FileContent += "</ServiceLog>";
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
        /// This methods writes Exception generated to database.
        /// </summary>
        /// <param name="_Exception"></param>
        private void WriteExceptionToDatabase(Exception _Exception)
        {
            IQMediaGroup.Core.HelperClasses.IQMediaGroupExceptions _IQMediaGroupExceptions = new IQMediaGroup.Core.HelperClasses.IQMediaGroupExceptions();
            _IQMediaGroupExceptions.ExceptionStackTrace = _Exception.StackTrace;
            _IQMediaGroupExceptions.ExceptionMessage = _Exception.Message;
            _IQMediaGroupExceptions.CreatedBy = "IQNotificationTracking - Write Exception";
            _IQMediaGroupExceptions.ModifiedBy = "IQNotificationTracking - Write Exception";

            string _ReturnValue = string.Empty;
            IIQMediaGroupExceptionsController _IIQMediaGroupExceptionsController = _ControllerFactory.CreateObject<IIQMediaGroupExceptionsController>();
            _ReturnValue = _IIQMediaGroupExceptionsController.AddIQMediaGroupException(_IQMediaGroupExceptions);
        }

        #endregion Console Application Methods
    }

    public class IQNotificationLog
    {
        public string ProcessStart { get; set; }

        public string ProcessEnd { get; set; }

        public List<IQNotificationTracking_FetchLog> ListOfIQNotificationLog { get; set; }

        public SearchParameters ConfigFileParams { get; set;}

        public List<string> StoredProcedures { get; set; }

        public List<string> Tables { get; set; }
    }
}
