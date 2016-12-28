using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Services.SMS.Logic
{
    public class ValidationLogic : ILogic
    {
        public bool ValidateSubscriptionInput(Int64? p_HubSpotID, Int64? p_SearchRequestID,string p_Action)
        {
            if (p_HubSpotID==null)
            {
                return false;
            }

            if (p_SearchRequestID==null)
            {
                return false;
            }

            if (!p_Action.Equals("s") && !p_Action.Equals("u"))
            {
                return false;
            }

            return true;
        }
    }
}
