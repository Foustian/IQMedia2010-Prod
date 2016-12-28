using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Base;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;


namespace IQMediaGroup.Model.Implementation
{
    internal class InboundReportingModel : IQMediaGroupDataLayer, IInboundReportingModel
    {
        /// <summary>
        /// Description:This Method will Insert InboundReporting.
        /// </summary>
        /// <param name="p_SearchLog">object of InboundReporting</param>
        /// <returns>Primary Key of InboundReporting</returns>
        public string InsertInboundReporting(InboundReporting p_InboundReporting)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RequestCollection", DbType.String, p_InboundReporting.RequestCollection, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@InboundReportingKey", DbType.Int64, p_InboundReporting.InboundReportingKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_InboundReporting_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
