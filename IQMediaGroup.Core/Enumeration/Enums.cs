using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.Enumeration
{
    public enum ConnectionStringKeys
    {
        IQMediaGroupConnectionString = 0,
        IQMediaGroupQAConnection = 1,
        IQMediaGroupProductionConnection = 2,
        IQMediaGroupDeveloperConnection = 3
    }
    
    public enum MicroMyIQSortFeilds
    {
        ClipDate,        
        ClipCreationDate,
        ClipTitle
    }

    public enum RolesName
    {
        AdminTabAccess = 1,
        AdvancedSearchAccess = 2,
        GlobalAdminAccess = 3,
        IQBasic = 4,
        myIQAccess = 5,
        IQAgentWebsiteAccess = 6,
        DownloadClips=7,
        IQCustomAccess=8,
        UGCDownload=9,
        IframeMicrosite=10,
        UGCUploadEdit=11,
        NielsenData=12,
        UGCAutoClip=13,
        iQPremium =14,
        MyIQnew =15,
        iQPremiumSM = 16,
        iQPremiumNM = 17,
        myiQSM = 18,
        myiQNM = 19,
        myiQPM = 20,
        iQAgentReport = 21,
        myiQReport = 22,
        MyIQTwitter = 23,
        iQPremiumTwitter=24,
        iQPremiumAgent= 25,
        iQPremiumRadio = 26,
        iQPremiumSentiment=27,
        CompeteData=28,
        MicrositeDownload=29
    }

    public enum Pages
    {
        IQBasic = 1,
        myIQ = 2,
        IQCustom = 3,
        IQAdvance = 4,
        IQAgent = 5,
        iQPremium = 6,
        MyIQnew = 7
    }


    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

    public enum FullTextLogicalOperator
    {
        [StringValue("AND")]
        AND = 1,
        [StringValue("&")]
        AND1 = 2,
        [StringValue("OR")]
        OR = 3,
        [StringValue("|")]
        OR1 = 4,
        [StringValue("NEAR")]
        NEAR = 5,
        [StringValue("NOT")]
        NOT = 6,
        [StringValue("AND NOT")]
        ANDNOT = 7,
        [StringValue("~")]
        TILT = 8

    }



    public enum RLStationTable
    {
        RL_Station = 1,
        RL_Station_Exception = 2
    }

    //public enum IQ_Process
    //{
    //    GUID = 1,
    //    CCText = 2
    //}

    public enum Script
    {
        RawMedia = 1,
        Clip = 2,
        Login = 3
    }

    public enum NotificationFrequency
    {
        Immediate = 1,
        Hourly = 2,
        OnceDay = 3,
        OnceWeek = 4
    }

    public enum NotificationType
    {
        Email = 1,
        SMS = 2
    }

    public enum SearchType
    {
        IQBasic,
        IQAdvance,
        IQPremium_TV,
        IQPremium_NM,
        IQPremium_SM,
        IQPremium_Twitter
    }

    public enum IQ_MasterReportType
    {
        MyIQ,
        IQAgent
    }

    public enum Report_ReportType
    {
        TV,
        NM,
        SM,
        TW
    }

    public enum Report_Identity
    {
        DailyTVReport,
        DailyNewsReport,
        DailySocialMediaReport,
        DailyMentionReport
    }

    public enum Comete_MediaType
    {
        NM,
        SM
    }

}
