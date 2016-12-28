using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Data;
using System.Configuration;


namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of interface ICarsenseExceptionsModel
    /// </summary>
    internal class IQMediaGroupExceptionsModel : IQMediaGroupDataLayer, IIQMediaGroupExceptionsModel
    {
        /// <summary>
        /// This Method adds Exception detail into table.
        /// </summary>
        /// <param name="p_CarSenseExceptions">Exception class of core</param>
        /// <returns>ExceptionKey for added record.</returns>
        public string AddIQMediaGroupException(IQMediaGroupExceptions p_IQMediaGroupExceptions)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ExceptionStackTrace", DbType.String, p_IQMediaGroupExceptions.ExceptionStackTrace, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ExceptionMessage", DbType.String, p_IQMediaGroupExceptions.ExceptionMessage, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedBy", DbType.String, p_IQMediaGroupExceptions.CreatedBy, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedBy", DbType.String, p_IQMediaGroupExceptions.ModifiedBy, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedDate", DbType.DateTime, p_IQMediaGroupExceptions.CreatedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, p_IQMediaGroupExceptions.ModifiedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_IQMediaGroupExceptions.IsActive, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@IQMediaGroupExceptionKey", DbType.Int64, p_IQMediaGroupExceptions.IQMediaGroupExceptionsKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_IQMediaGroupExceptions_Insert", _ListOfDataType);               

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
