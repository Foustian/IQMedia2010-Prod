using IQMediaGroup.Core.HelperClasses;
using System;

namespace IQMediaGroup.Controller.Implementation
{
    internal class UGCDownloadTrackingController : IQMediaGroup.Controller.Interface.IUGCDownloadTrackingController
    { 
        private readonly IQMediaGroup.Model.Factory.ModelFactory _ModelFactory = new IQMediaGroup.Model.Factory.ModelFactory();
        private readonly IQMediaGroup.Model.Interface.IUGCDownloadTrackingModel _IUGCDownloadTrackingModel;

        public UGCDownloadTrackingController()
        {
            _IUGCDownloadTrackingModel = _ModelFactory.CreateObject<IQMediaGroup.Model.Interface.IUGCDownloadTrackingModel>();            
        }

        public string Insert(UGCDownloadTracking p_UGCDownloadTracking)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IUGCDownloadTrackingModel.Insert(p_UGCDownloadTracking);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}