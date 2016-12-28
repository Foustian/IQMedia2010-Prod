using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Controller.Implementation
{
    internal class OutboundReportingController : IOutboundReportingController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IOutboundReportingModel _IOutboundReportingModel;

        public OutboundReportingController()
        {
            _IOutboundReportingModel = _ModelFactory.CreateObject<IOutboundReportingModel>();
        }

        /// <summary>
        /// Description:This method will insert SearchLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">Object of SearchLog</param>
        /// <returns>Primary key of SearchLog</returns>
        public string InsertOutboundReportingLog(OutboundReporting p_OutboundReporting)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IOutboundReportingModel.InsertOutboundReportingLog(p_OutboundReporting);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }   

    }
}
