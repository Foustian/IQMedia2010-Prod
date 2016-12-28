using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Implementation
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

        /// <summary>
        /// This method adds Exception detail
        /// </summary>
        /// <param name="p_CarSenseExceptions">Exception object</param>
        /// <param name="p_CreatedBy">Created By</param>
        /// <returns>ExceptionKey for added Record</returns>
        public string AddIQMediaGroupException(Exception p_Exception,string p_CreatedBy)
        {
            string _ReturnValue = string.Empty;

            try
            {
                IQMediaGroupExceptions _IQMediaGroupExceptions = new IQMediaGroupExceptions();

                _IQMediaGroupExceptions.ExceptionStackTrace = p_Exception.StackTrace;
                _IQMediaGroupExceptions.ExceptionMessage = p_Exception.Message;
                _IQMediaGroupExceptions.CreatedBy = p_CreatedBy;
                _IQMediaGroupExceptions.ModifiedBy = p_CreatedBy;

                _ReturnValue = _IIQMediaGroupExceptionsModel.AddIQMediaGroupException(_IQMediaGroupExceptions);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ReturnValue;
        }
    }
}
