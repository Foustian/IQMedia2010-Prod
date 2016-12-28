using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    internal class IQAgent_SearchRequestController : IIQAgent_SearchRequestController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQAgent_SearchRequestModel _IIQAgent_SearchRequestModel;

        public IQAgent_SearchRequestController()
        {
            _IIQAgent_SearchRequestModel = _ModelFactory.CreateObject<IIQAgent_SearchRequestModel>();
        }
        public void GetAllowedMediaTypesByID(int ID, out bool IsAllowTV, out bool IsAllowNM, out bool IsAllowSM)
        {
            try
            {
                _IIQAgent_SearchRequestModel.GetAllowedMediaTypesByID(ID, out IsAllowTV, out IsAllowNM, out IsAllowSM);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
