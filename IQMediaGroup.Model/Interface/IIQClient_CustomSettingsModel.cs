using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Model.Interface
{
    public interface IIQClient_CustomSettingsModel
    {

        string GetSettingByClientGUID(Guid _clientGUID);

        void GetSentimentSettingsByClientGuid(Guid p_ClientGuid, out float? p_TVLowThreshold, out float? p_TVHighThreshold, out float? p_NMLowThreshold, out float? p_NMHighThreshold, out float? p_SMLowThreshold, out float? p_SMHighThreshold, out float? p_TwitterLowThreshold, out float? p_TwitterHighThreshold);
    }
}
