using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Admin.Core.Enumeration
{
    public static class CommonConstants
    {
        #region Static Variable Values

        public static string Space = " ";
        public static string DblQuote = "\"";
        public static string Colon = ":";
        public static string Zero = "0";
        
        #endregion Static Variable Values

        #region Common

        public static string IQAgentUserRoleName = "IQAgentUser";
        public static string CommonErrorMsg = "You have encountered a System Error. Please Login Again. You may need to clear your system cache.";
        public static string NoResultsFound = "No Results Found";
        public static string CurrentUsers = "CurrentUsers";
        public static bool IsLogout;

        #endregion Common

        #region ConfigVariables

        public static string ConfigPages = "Pages";
        public static string ConfigIQMediaClientGUID = "IQMediaClientGUID";

        #endregion ConfigVariables

        #region HTML&Other Tag

        public static string HTMLBreakLine = "<br />";

        #endregion HTML&Other Tag

        #region Pages

        public static string CustomErrorPage = "~/CustomError/";

        #endregion Pages

        #region Parameters

        public static string ParamsIncorrectFormat = "Parameter not found or not in Correct Format.";
        public static string ConfigconnectionString = "connectionString";
        public static string ParamStartDate = "Start Date";
        public static string ParamEndDate = "End Date";
        public static string ParamClientID = "ClientID";
        public static string ParamClientQuery = "Client Query";
        public static string ParamQueryVersion = "Query Version";
        public static string ParamDebugFilePath = "Debug File Path";

        #endregion Parameters

        #region Message

        public static string MsgStartEndDate = "End Date should not earlier than Start Date";
        public static string MsgDateGTCurrentDate = "Provided Date should not be later than Today's Date";
        public static string MsgFileNotExist = "File doesn't exist.";
        public static string MsgPmgSearchUrlNotExist = "PMGSearchUrl doesn't exist.";
        public static string NoUserWithIQAgentRole = "No user found with IQ Agent role.";
        public static string ClientNameAlreadyExists = "Client Already Exists.";

        #endregion Message

        #region IQAgent XML Tag Name

        public static string IQAgent_XMLTag_RootTag = "IQAgentRequest";
        public static string IQAgent_XMLTag_SearchTerm = "SearchTerm";
        public static string IQAgent_XMLTag_ProgramTitle = "ProgramTitle";
        public static string IQAgent_XMLTag_Appearing = "Appearing";
        public static string IQAgent_XMLTag_IQ_Dma_Name = "IQ_Dma_Name";
        public static string IQAgent_XMLTag_IQ_Dma_Num = "IQ_Dma_Num";
        public static string IQAgent_XMLTag_IQ_Sub_Cat = "IQ_Class";
        public static string IQAgent_XMLTag_IQ_Sub_Cat_Num = "IQ_Class_Num";
        public static string IQAgent_XMLTag_Station_Affil = "Station_Affil";
        public static string IQAgent_XMLTag_Station_Affil_Num = "Station_Affil_Num";
        public static string IQAgent_XMLTag_IsDefaultSettings = "IsDefaultSettings";
        public static string IQAgent_XMLAttribute_IsManualSelect = "IsManualSelect";

        //public static string IQAgent_XMLTag_ProgramDescription = "ProgramDescription";
        //public static string IQAgent_XMLTag_IQ_Cat = "IQ_Cat";

        #endregion
    }
}
