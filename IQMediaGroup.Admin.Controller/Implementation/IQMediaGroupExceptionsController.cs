using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface ICarsenseExceptionsController
    /// </summary>
    internal class IQMediaGroupExceptionsController : IIQMediaGroupExceptionsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQMediaGroupExceptionsModel _IIQMediaGroupExceptionsModel;

        public IQMediaGroupExceptionsController()
        {
            _IIQMediaGroupExceptionsModel = _ModelFactory.CreateObject<IIQMediaGroupExceptionsModel>();
        }

        /// <summary>
        /// This method adds Exception detail
        /// </summary>
        /// <param name="p_CarSenseExceptions">Exception object of core</param>
        /// <returns>ExceptionKey for added Record</returns>
        public string AddIQMediaGroupException(IQMediaGroupExceptions p_IQMediaGroupExceptions)
        {
            string _ReturnValue = string.Empty;
            try
            {
                _ReturnValue = _IIQMediaGroupExceptionsModel.AddIQMediaGroupException(p_IQMediaGroupExceptions);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ReturnValue;
        }
    }
}
