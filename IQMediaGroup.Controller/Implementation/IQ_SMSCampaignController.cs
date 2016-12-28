using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Controller.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQ_SMSCampaignController : IIQ_SMSCampaignController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQ_SMSCampaignModel _IIQ_SMSCampaignModel;

        public IQ_SMSCampaignController()
        {
            _IIQ_SMSCampaignModel = _ModelFactory.CreateObject<IIQ_SMSCampaignModel>();
        }
    
        public string  UpdateIIQ_SMSCampaignIsActive(Int64 p_SearchRequestID, Int64 p_HubSpotID, bool p_IsActivated)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IIQ_SMSCampaignModel.UpdateIIQ_SMSCampaignIsActive(p_SearchRequestID,p_HubSpotID,p_IsActivated);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
