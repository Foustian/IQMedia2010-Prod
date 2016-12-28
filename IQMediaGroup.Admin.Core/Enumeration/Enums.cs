using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Admin.Core.Enumeration
{
    public enum ConnectionStringKeys
    {
        IQMediaGroupConnectionString = 0,
        IQMediaGroupQAConnection = 1,
        IQMediaGroupProductionConnection = 2,
        IQMediaGroupDeveloperConnection = 3
    }

    public enum RolesName
    {
        AdminTabAccess = 1,
        AdvancedSearchAccess = 2,
        GlobalAdminAccess = 3,
        IQBasic = 4,
        myIQAccess = 5,
        IQAgentWebsiteAccess = 6,
        DownloadClips = 7,
        IQCustomAccess = 8,
        UGCDownload = 9,
        IframeMicrosite = 10,
        UGCUploadEdit = 11,
        NielsenData = 12,
        UGCAutoClip = 13,
        iQPremium =14,
        CompeteData = 15,
        MicrositeDownload = 16
    }

    public enum StationTimeZone
    {
        CST = 1,
        MST = 2,
        PST = 3,
        EST = 4
    }
   
}
