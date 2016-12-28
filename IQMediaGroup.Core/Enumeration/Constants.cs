using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.Enumeration
{
    public static class CommonConstants
    {
        #region Static Variable Values

        public static string Space = " ";
        public static string QuestionMark = "?";
        public static string Equal = "=";
        public static string DblQuote = "\"";
        public static string SingleQuote = "'";
        public static string SqBracketOpen = "[";
        public static string SqBracketClose = "]";
        public static string Colon = ":";
        public static string Ampersand = "&";
        public static string Comma = ",";
        public static string BracketOpen = "(";
        public static string BracketClose = ")";
        public static string SemiColon = ";";
        public static string UnderScore = "_";
        public static string Dot = ".";
        public static string XmlText = "xml";
        public static string ForwardSlash = "/";
        public static string Zero = "0";
        public static int Hundred = 100;
        
        #endregion Static Variable Values

        #region Common

        public static string CookieUserName = "userName";
        public static int SMTimeOut = 2147000;
        public static int SessionTimeOutCst = 60;

        public static string MYIQPageTitle = "My iQ";
        public static string IQBasicPageTitle = "iQ Basic";
        public static string IQAdvancedPageTitle = "iQ Advanced";
        public static string IQAgentPageTitle = "iQ Agent";
        public static string IQCustomPageTitle = "iQ Custom";
        public static string HeaderTabPanel = "UCHeaderTabPanelControl";

        public static string CAStarted = "Service Started";
        public static string CACompleted = "Service Completed";
        public static string InsertedRecords = "Record Inserted";

        public static string aMYIQ = "aMYIQ";
        public static string aIQBasic = "aIQBasic";
        public static string aIQAdvanced = "aIQAdvanced";
        public static string aIQAgent = "aIQAgent";
        public static string aIQCustom = "aIQCustom";
        public static string aMYIQnew = "aMYIQnew";
        public static string aIQPremium = "aIQPremium";


        public static string SessionTimeOutMsg = "You have been inactive and have been logged out. Please Login Again.";
        public static string CommonErrorMsg = "You have encountered a System Error. Please Login Again. You may need to clear your system cache.";
        public static string NoResultsFound = "No Results Found";
        public static string CurrentUsers = "CurrentUsers";
        public static string TimeOutException = "The server timed out before receiving a response. Please retry your search.";
        public static string LogOutMsg = "Your account has logged in somewhere else and made your session invalid.";

        public static string CssStaticActiveTab = "active";
        public static string CssStaticInActiveTab = "";

        public static string RMSearchHitList = DblQuote + "HitList" + DblQuote + Colon + SqBracketOpen;

        public static string JSFunctionRMActive = "viewtab(1);";
        public static string JSFunctionClipKey = "Clip";
        public static string JSFunctionClipActive = "viewtab(2);";
        
        public static string RMGridDateTimeLabel = "lblRawMediaDatetime";
        public static string RMGridCacheKeyHidden = "hfCacheKey";
        public static string RMSearchResultLabel = "Raw Media Results";

        public static string RegexExClipID = "^?[{|\\(]?[0-9a-fA-F]{8}[-]?([0-9a-fA-F]{4}[-]?){3}[0-9a-fA-F]{12}[\\)|}]?";
        public static string CaptionMainDivClass = "hit";
        public static string CaptionSeekPointFunction = "setSeekPoint";
        public static string CaptionDateTimeClass = "boldgray";
        public static string CaptionCaptionClass = "caption";
        public static string CaptionActiveHeight = "235px";
        public static string CaptionDeActiveHeight = "50px";

        public static string BeginTxt = "begin";
        public static string EndTxt = "end";
        public static string ClipCaptionDelayTxt = "ClipCaptionDelay";
        public static string ClipCaptionTagName = "p";
        public static string RMZDZT = "0:0";

        public static bool IsLogout;
        public static string CurrentPageNoText = "Page ";
        public static string XMLErrorMessage = "IQM-ClosedCaption is missing";


        #endregion Common

        #region ConfigVariables

        public static string ConfigRawSearch = "RawSearch";
        public static string ConfigNoOfCacheKeyRequest = "NoOfCacheKeyRequest";
        public static string ConfigRawMediaPageSize = "RawMediaPageSize";

        public static string ConfigRawMediaCaptionDelay = "RawMediaCaptionDelay";

        public static string ConfigPMGSearchTotalHitsFromConfig = "PMGSearchTotalHitsFromConfig";
        public static string ConfigPMGMaxListCount = "PMGMaxListCount";
        public static string ConfigPMGMaxHighlights = "PMGMaxHighlights";

        public static string ConfigNoOfResultsFromDBArchiveClip = "NoOfResultsFromDBArchiveClip";

        public static string ConfigIQAgentResultsPageSizeDB = "IQAgentResultsPageSizeDB";
        public static string ConfigIQAgentResultsPageSizeGrid = "IQAgentResultsPageSizeGrid";

        public static string ConfigGetClosedCaptionFromIQ = "GetClosedCaptionFromIQ";

        public static string ConfigMaxNoOfClipsSelected = "MaxNoOfClipsSelected";
        public static string ConfigClip_Download_Location = "Clip_Download_Location";
        public static string ConfigExportClip = "ExportClip";
        public static string ConfigExportClipMsg = "ExportClipMsg";
        public static string ConfigPolicyFileLocation = "PolicyFileLocation";

        public static string ConfigUGCFileUpLoadLocation = "UGCFileUpLoadLocation";
        public static string ConfigUGCFileDownloadLocation = "UGCFileDownloadLocation";
        public static string ConfigUGCFileUploadService = "UGCFileUploadService";

        public static string ConfigIQMediaUserGUID = "RL_User_GUID";

        public static string ConfigSolrQT = "SolrQT";
        public static string ConfigSolrQTIframe = "SolrQTIframe";

        public static string ConfigAndroidDefaultVersion = "AndroidDefaultVersion";
        public static string ConfigAndroidVersionRegex = "AndroidVersionRegex";
        public static string ConfigZoomChartNoOfLables = "ZoomChartNoOfLables";
        public static string ConfigChartRightMargin = "chartRightMargin";
        public static string ConfigChartTitle = "ChartTitle";
        public static string ConfigSearchTipsURL = "SearchTipsURL";

        public static string ConfigSolrNewsUrl = "SolrNewsUrl";
        public static string ConfigSolrSMUrl = "SolrSMUrl";
        public static string ConfigSolrTwitterUrl = "SolrTwitterUrl";

        
        #endregion ConfigVariables

        #region HTML&Other Tag

        public static string HTMLDiv = "div";
        //public static string HTMLWidth = "width";
        public static string HTMLHeight = "height";
        public static string HTMLClass = "class";
        public static string HTMLOnClick = "onclick";
        public static string HTMLBreakLine = "<br />";

        #endregion HTML&Other Tag

        #region Pages

        public static string HomePage = "~/";
        public static string CustomErrorPage = "~/CustomError/";
        public static string CustomErrorPageError = "~/CustomError/Default.aspx?e=to";

        #endregion Pages

        #region Parameters

        public static string ParamsNotFound = "Config Parameter {0} not found in config file.";
        public static string ParamsIncorrectFormat = "Parameter not found or not in Correct Format.";
        public static string ConfigconnectionString = "connectionString";
        public static string ParamStartDate = "Start Date";
        public static string ParamEndDate = "End Date";
        public static string ParamClientID = "ClientID";
        public static string ParamClientQuery = "Client Query";
        public static string ParamQueryVersion = "Query Version";
        public static string ParamDaysDuration = "Days Duration";
        public static string ParamDebugFilePath = "Debug File Path";
        public static string ParamNumResultsPerPage = "NumResultsPerPage";
        
        public static string ParamHourlyEmailTimeFromMinute = "HourlyEmailTimeFromMinute";
        public static string ParamHourlyEmailTimeToMinute = "HourlyEmailTimeToMinute";
        public static string ParamDailyEmailTimeHours = "DailyEmailTimeHours";
        public static string ParamWeeklyEmailDay = "WeeklyEmailDay";
        public static string ParamSMTPFromEMail = "SMTPFromEMail";
        public static string ParamEmailTemplateFilePath = "EmailTemplateFilePath";
        public static string ParamSubject = "Subject";

        #endregion Parameters

        #region Message

        public static string MsgStartEndDate = "End Date should not earlier than Start Date";
        public static string MsgFromToDate = "To Date should not be earlier than From Date";
        public static string MsgDateGTCurrentDate = "Provided Date should not be later than Today's Date";
        public static string MsgFileNotExist = "File doesn't exist.";
        public static string MsgPmgSearchUrlNotExist = "PMGSearchUrl doesn't exist.";
        public static string MsgBasicSearchUA = "Search is currently unavailable";
        public static string MsgSolrSearchUA = "Search is currently unavailable.";
        public static string CategoryAlreadyExists = "Category name already exists";
        public static string CategorySavedSuccessfully = "Category saved successfully";
        public static string CategoryDeletedSuccessfully = "Category deleted successfully";
        public static string CategoryAssociatedWithClip = "Category can not be deleted,There are clips associated with this category";
        public static string DBTimeOutTxt = "timeout expired";
        public static string DBTimeOutMsg = "Search timed out – Please retry";
        public static string CommonErrMsg = "An error occured, please try again!";

        #endregion Message

        #region IQAgent XML Tag Name

        public static string IQAgent_XMLTag_SearchTerm = "SearchTerm";
        public static string IQAgent_XMLTag_ProgramTitle = "ProgramTitle";
        public static string IQAgent_XMLTag_Appearing = "Appearing";
        public static string IQAgent_XMLTag_IQ_Dma_Num = "IQ_Dma_Num";
        public static string IQAgent_XMLTag_IQ_Dma_Name = "IQ_Dma_Name";
        public static string IQAgent_XMLTag_IQ_Sub_Cat_Num = "IQ_Class_Num";
        public static string IQAgent_XMLTag_Station_Affil = "Station_Affil";
        
        #endregion

        #region Database Objects

        public static string usp_IQAgentSearchRequest_Select = "usp_IQAgentSearchRequest_Select";
        public static string usp_IQAgentSearchRequest_SelectByClientQueryVersion = "usp_IQAgentSearchRequest_SelectByClientQueryVersion";
        public static string usp_PMGSearchLog_Insert = "usp_PMGSearchLog_Insert";
        public static string usp_IQAgentResult_InsertList = "usp_IQAgentResult_InsertList";
        public static string usp_IQNotificationTracking_SelectByCommunicationFlag = "usp_IQNotificationTracking_SelectByCommunicationFlag";
        public static string usp_IQNotificationTracking_Update = "usp_IQNotificationTracking_Update";
        public static string Table_IQAgentSearchRequest = "IQAgentSearchRequest";
        public static string Table_PMGSearchLog = "PMGSearchLog";
        public static string Table_IQAgentResult = "IQAgentResults";

        public static string Table_IQNotificationSettings = "IQNotificationSettings";
        public static string Table_IQNotificationTracking = "IQNotificationTracking";


        #endregion

        public static string NavigationPanel = "ucNavigationPanel";

        public static string TopPanel = "TopPanel";

        public static string StaticMasterRightContent = "StaticMasterRightContent";
    }
}
