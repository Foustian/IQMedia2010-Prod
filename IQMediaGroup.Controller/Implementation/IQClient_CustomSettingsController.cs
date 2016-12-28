using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Controller.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQClient_CustomSettingsController : IIQClient_CustomSettingsController
    {

        ModelFactory _ModelFactory = new ModelFactory();
        IIQClient_CustomSettingsModel _IIQClient_CustomSettingsModel = null;

        public IQClient_CustomSettingsController()
        {
            _IIQClient_CustomSettingsModel = _ModelFactory.CreateObject<IIQClient_CustomSettingsModel>();
        }


        public string GetSettingByClientGUID(Guid ClientGUID)
        {
            try
            {
                
                string _Result = _IIQClient_CustomSettingsModel.GetSettingByClientGUID(ClientGUID);
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void GetSentimentSettingsByClientGuid(Guid p_ClientGuid, out float? p_TVLowThreshold, out float? p_TVHighThreshold, out float? p_NMLowThreshold, out float? p_NMHighThreshold, out float? p_SMLowThreshold, out float? p_SMHighThreshold, out float? p_TwitterLowThreshold, out float? p_TwitterHighThreshold)
        {
            try
            {
                _IIQClient_CustomSettingsModel.GetSentimentSettingsByClientGuid(p_ClientGuid, out p_TVLowThreshold, out p_TVHighThreshold, out p_NMLowThreshold, out p_NMHighThreshold, out p_SMLowThreshold, out p_SMHighThreshold, out p_TwitterLowThreshold, out p_TwitterHighThreshold);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


    }
}
