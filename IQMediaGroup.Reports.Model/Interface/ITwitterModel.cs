using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface ITwitterModel
    {
        DataSet GetIQAgent_TwitterResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, out string Query_Name);
        string InsertArchiveTweet(ArchiveTweets _ArchiveTweets);
    }
}
