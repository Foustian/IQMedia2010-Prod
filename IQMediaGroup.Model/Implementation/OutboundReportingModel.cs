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
    internal class OutboundReportingModel : IQMediaGroupDataLayer, IOutboundReportingModel
    {
        /// <summary>
        /// Description:This Method will Insert ServiceLog.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SearchLog">object of OutboundReporting</param>
        /// <returns>Primary Key of OutboundReporting</returns>
        public string InsertOutboundReportingLog(OutboundReporting p_OutboundReporting)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, p_OutboundReporting.Query_Name, ParameterDirection.Input));
                
                _ListOfDataType.Add(new DataType("@FromEmailAddress", DbType.String, p_OutboundReporting.FromEmailAddress, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ToEmailAddress", DbType.String, p_OutboundReporting.ToEmailAddress, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MailContent", DbType.Xml, p_OutboundReporting.MailContent, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ServiceType", DbType.String, p_OutboundReporting.ServiceType, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@OutboundReportingKey", DbType.Int32, p_OutboundReporting.OutboundReportingKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_OutboundReporting_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }       
    }
}
