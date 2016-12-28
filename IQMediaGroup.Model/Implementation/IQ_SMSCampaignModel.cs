using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQ_SMSCampaignModel : IQMediaGroupDataLayer, IIQ_SMSCampaignModel
    {
        public string UpdateIIQ_SMSCampaignIsActive(Int64 p_SearchRequestID,Int64 p_HubSpotID,bool p_IsActivated)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int64, p_SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@HubSpotID", DbType.Int32, p_HubSpotID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActivated", DbType.Boolean, p_IsActivated, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@AffectedResults", DbType.Int32, 0, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_IQ_SMSCampaign_UpdateIsActive", _ListOfDataType);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
