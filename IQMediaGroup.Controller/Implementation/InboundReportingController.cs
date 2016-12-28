using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Implementation
{
    public class InboundReportingController : IInboundReportingController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IInboundReportingModel _IInboundReportingModel;

        public InboundReportingController()
        {
            _IInboundReportingModel = _ModelFactory.CreateObject<IInboundReportingModel>();
        }
     
        /// <summary>
        /// Description:This method will insert InboundReporting.
        /// </summary>
        /// <param name="p_SearchLog">Object of InboundReporting</param>
        /// <returns>Primary key of InboundReporting</returns>
        public string InsertInboundReporting(InboundReporting p_InboundReporting)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IInboundReportingModel.InsertInboundReporting(p_InboundReporting);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
