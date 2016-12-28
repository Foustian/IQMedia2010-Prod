using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.Enumeration;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IRL_GUIDS
    /// </summary>
    internal class Iq_Service_logController : IIq_Service_logController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIq_Service_logModel _IIq_Service_logModel;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public Iq_Service_logController()
        {
            _IIq_Service_logModel = _ModelFactory.CreateObject<IIq_Service_logModel>();
        }

        /// <summary>
        /// Description:This Method will insert Iq_Service_log
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Iq_Service_log">object of Iq_Service_log</param>
        /// <returns>Primary key of Iq_Service_log</returns>
        public string AddRL_GUIDS(Iq_Service_log p_Iq_Service_log)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IIq_Service_logModel.AddIq_Service_log(p_Iq_Service_log);
                
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will insert Iq_Service_log
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Iq_Service_log">object of Iq_Service_log</param>
        /// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        /// <returns>Primary key of Iq_Service_log</returns>
        public string AddRL_GUIDS(Iq_Service_log p_Iq_Service_log, ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IIq_Service_logModel.AddIq_Service_log(p_Iq_Service_log, p_ConnectionStringKeys);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}