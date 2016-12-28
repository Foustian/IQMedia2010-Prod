using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Model.Interface
{
    public interface IIQ_SMSCampaignModel
    {
        string UpdateIIQ_SMSCampaignIsActive(Int64 p_SearchRequestID,Int64 p_HubSpotID,bool p_IsActivated); 
    }
}
